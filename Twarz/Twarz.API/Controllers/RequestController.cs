using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Twarz.API.Application.Company.Commands;
using Twarz.API.Application.Requests.Commands;
using Twarz.API.Application.Sessions.Queries;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Twarz.API.Application.Requests.Queries;
using Twarz.API.Domains.Enums;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Twarz.API.Hubs;
using static Twarz.API.Models.PushTemplates;
using System;

namespace Twarz.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<NotificationHub, INotificationHub> _notification;

        public RequestController(IMediator mediator, IHubContext<NotificationHub, INotificationHub> notification)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _notification = notification ?? throw new ArgumentNullException();
        }


        [HttpPost(Name = "SaveRequest")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<string>> SaveRequest([FromBody] CreateRequestCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (result > 0)
            {
                string url = "https://fcm.googleapis.com/fcm/send";
                GetSessionbyIdQuery sessionCommand = new GetSessionbyIdQuery(command.SessionId);
                var sessionResult = await _mediator.Send(sessionCommand);

                await SendNotification(sessionResult.TokenDevice, "Ha recibido una solicitud de verificación");
            }
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }


        [HttpGet("{id}", Name = "GetRequestById")]
        [ProducesResponseType(typeof(RequestMv), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<RequestMv>> GetRequestById(int id)
        {

            GetByIdRequestQuery query = new GetByIdRequestQuery(id);
            var requests = await _mediator.Send(query);
            return Ok(requests);
        }


        [HttpPatch("{id}/{newStatus}", Name = "UpdateStatusRequest/{id:int}/{newStatus:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> UpdateStatusRequest(int id, int newStatus)
        {
            ChangeStatusRequestCommand command = new ChangeStatusRequestCommand
            {
                Id = id,
                newState = (RequestStatusEnum)newStatus
            };
            var result = await _mediator.Send(command);
            if (result == 0)
            {
                return new NotFoundResult();
            }
            if (result == -1)
            {
                return new BadRequestResult();
            }

            await _notification.Clients.Group(id.ToString()).SendMessage(new Notification
            {
                RequestId = id.ToString(),
                SessionName = "Un cliente",
                Status = ((RequestStatusEnum)newStatus).ToString(),
            });


            GetByIdRequestQuery query = new GetByIdRequestQuery(id);

            if (((RequestStatusEnum)newStatus) == RequestStatusEnum.TimeOut)
            {
                var sessionResult = await _mediator.Send(query);
                await SendNotification(sessionResult.Session.TokenDevice, "La solicitud ha caducado por timeout");
            }

            return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
        }

        [Authorize]
        [HttpGet("", Name = "GetRequest")]
        [ProducesResponseType(typeof(IEnumerable<RequestMv>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<RequestMv>>> GetRequest()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //identity.FindFirst("ClaimName").Value;
            var claimDocuement = identity.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claimDocuement == null)
            {
                return NotFound("El usuario loqueado no tiene documento asociado.");
            }

            var documentNumber = claimDocuement.Value;

            var query = new GetListRequestQuery(documentNumber);
            var requests = await _mediator.Send(query);
            return Ok(requests);
        }

        private async Task SendNotification(string tokenDevice, string message)
        {
            string url = "https://fcm.googleapis.com/fcm/send";
            using (var client = new HttpClient())
            {
                // por el momento harcode aqui
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAAJwZdmw4:APA91bFkHUrFY2dhavJZ7LZ2TeWAkfmrhZbbGnTvGMDvYrcfVGdGX-BR_CoT0tRavUF-MgLRIv30A3bR1sRyFpzAxFk3-g2mS5XxGmjNEWhjfAsjSB2iHyErKTLuBXeY2Evcx9JwGkPi");

                var pushNotificationRequest = new PushNotificationRequest
                {
                    notification = new NotificationMessageBody
                    {
                        title = "Twarz",
                        body = message
                    },
                    // data = androidNotificationObject,
                    registration_ids = new List<string> { tokenDevice }
                };

                string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
                var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
                Console.WriteLine(response.Content);


            }
        }

    }
}
