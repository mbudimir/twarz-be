using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Twarz.API.Application.Company.Commands;
using Twarz.API.Application.Requests.Queries;

namespace Twarz.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpPost(Name = "SaveCompany")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<string>> SaveCompany([FromBody] CreateCompanyCommand command)
        {
            var result = await _mediator.Send(command);
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        [Authorize]
        [HttpGet("", Name = "GetRequestByCompany")]
        [ProducesResponseType(typeof(IEnumerable<RequestCompanyMv>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<RequestCompanyMv>>> GetRequestByCompany()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //identity.FindFirst("ClaimName").Value;
            var companyId = identity.Claims.FirstOrDefault(x => x.Type == "CompanyId");
            if (companyId == null)
            {
                return NotFound("El usuario loqueado no tiene compañia asociada.");
            }

            var query = new GetListRequestByCompanyIdQuery(Convert.ToInt32(companyId.Value));
            var requests = await _mediator.Send(query);
            return Ok(requests);
        }

    }
}
