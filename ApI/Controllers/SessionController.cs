using ApI.Data;
using ApI.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SessionController : ControllerBase
    {
        private readonly ISessionManager _sessionManager;

        public SessionController(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        [HttpPost()]
        public async Task<ActionResult<Guid>> StartSession(SessionInput sessionInfo, CancellationToken cancellationToken)
        {
            var result = await _sessionManager.StartSession(sessionInfo, cancellationToken);

            return result.Status switch
            {
                StatusType.Conflict => new ConflictObjectResult(result.Message),
                StatusType.NotFound => new NotFoundObjectResult(result.Message),
                StatusType.DependencyFail => new StatusCodeResult((int)HttpStatusCode.FailedDependency),
                StatusType.Success => new OkObjectResult(result.Value),
                _ => new StatusCodeResult((int)HttpStatusCode.InternalServerError),
            };
        }

        [HttpDelete("{sessionId}")]
        public async Task<ActionResult> StopSession(Guid sessionId, CancellationToken cancellationToken)
        {
            var result = await _sessionManager.StopSession(sessionId, cancellationToken);

            return result.Status switch
            {
                StatusType.Conflict => new ConflictObjectResult(result.Message),
                StatusType.NotFound => new NotFoundObjectResult(result.Message),
                StatusType.DependencyFail => new StatusCodeResult((int)HttpStatusCode.FailedDependency),
                StatusType.Success => new OkResult(),
                _ => new StatusCodeResult((int)HttpStatusCode.InternalServerError),
            };
        }
    }
}
