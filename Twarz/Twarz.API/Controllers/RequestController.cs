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

namespace Twarz.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RequestController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

                using (var client = new HttpClient())
                {
                    // por el momento harcode aqui
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAAJwZdmw4:APA91bFkHUrFY2dhavJZ7LZ2TeWAkfmrhZbbGnTvGMDvYrcfVGdGX-BR_CoT0tRavUF-MgLRIv30A3bR1sRyFpzAxFk3-g2mS5XxGmjNEWhjfAsjSB2iHyErKTLuBXeY2Evcx9JwGkPi");
                    //var registrationToken = "cU7ccZXeS8eUAHa_oDJxv_:APA91bGuzH7Q9JiSchzbgPODmF7mYYhbwj1Npin5C7IkCu5nhOTK_orZUP9VuvFbr-GXD-6hgq322yzJvx-W3pjFuDsJPqLDhhvnET-NogiCfm3cQhVYhlr5U-RvnegIh65rsAnpBgsQ";

                    
                    var pushNotificationRequest = new PushNotificationRequest
                    {
                        notification = new NotificationMessageBody
                        {
                            title = "New join request",
                            body = "A company to request for join then"
                        },
                        // data = androidNotificationObject,
                        registration_ids = new List<string> { sessionResult.TokenDevice }
                    };

                    string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
                    var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
                    Console.WriteLine(response.Content);
                    
                    ///////////////// Por el momento no hago nada sino puede mandar el mensaje /////////////////
                    //if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    //{
                    //    Console.WriteLine(response.Content);
                    //    //return Ok("Message sent successfully!");
                    //}
                    //else
                    //{
                    //    // There was an error sending the message
                    //    //throw new Exception("Error sending the message.");
                    //}
                }
            }
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
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

    }
}
