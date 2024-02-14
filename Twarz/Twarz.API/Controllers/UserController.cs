using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Twarz.API.Application.Users;

namespace Twarz.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost(Name = "Login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return Unauthorized();
            }
            var token = new JwtSecurityTokenHandler().WriteToken(result);

            return new ObjectResult(token) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
