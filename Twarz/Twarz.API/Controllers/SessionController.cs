using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Threading.Tasks;
using Twarz.API.Application.Sessions.Commands;
using Twarz.API.Application.Sessions.Queries;
using Twarz.API.Commands;

namespace Twarz.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SessionController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpPost(Name = "SaveSession")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<string>> SaveSession([FromBody] CreateSessionCommand command)
        {
            var result = await _mediator.Send(command);
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpGet("{documentNumber}", Name = "GetSession")]
        [ProducesResponseType(typeof(IEnumerable<SessionMv>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<SessionMv>>> GetSessionsByDocumentNumber(string documentNumber)
        {
            var query = new GetListSessionQuery(documentNumber);
            var sessions = await _mediator.Send(query);
            return Ok(sessions);
        }
    }
}
