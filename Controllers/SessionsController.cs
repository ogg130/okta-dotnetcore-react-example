using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using okta_dotnetcore_react_example.Data;
using okta_dotnetcore_react_example.Models;

namespace okta_dotnetcore_react_example.Controllers
{
    [Authorize]
    [Route("/api/[controller]")]
    public class SessionsController : Controller
    {
        private readonly ApiContext _context;
        public SessionsController(ApiContext context)
        {
           _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSessions()
        {
            var userId = User.Claims.SingleOrDefault(u => u.Type == "uid")?.Value;
            var sessions = await _context.Sessions.AsAsyncEnumerable().Where(x => x.UserId == userId).ToListAsync();
            return Ok(sessions);
        }

        [HttpGet("/api/sessions/{sessionId}")]
        public IActionResult GetSessionById(int sessionId)
        {
            var session = _context.Sessions.SingleOrDefault(x => x.SessionId == sessionId);
            if (session == null)
            {
                return NotFound();
            }
            return Ok(session);
        }

        [HttpPost]
        public IActionResult AddSession([FromBody] Session session)
        {
            session.UserId = User.Claims.SingleOrDefault(u => u.Type == "uid")?.Value;
            _context.Add<Session>(session);
            _context.SaveChanges();
            return Created($"api/sessions/{session.SessionId}", session);
        }

        [HttpPost("/api/sessions/{sessionId}")]
        public IActionResult UpdateSession([FromBody] Session session)
        {
            var savedSession = _context.Sessions.SingleOrDefault(x => x.SessionId == session.SessionId);
            if (savedSession == null)
            {
                return NotFound();
            }
            if (savedSession.UserId != User.Claims.SingleOrDefault(u => u.Type == "uid")?.Value)
            {
                return Unauthorized();
            }
            savedSession.Title = session.Title;
            savedSession.Abstract = session.Abstract;
            _context.SaveChanges();
            return Ok(savedSession);
        }

        [HttpDelete("/api/sessions/{sessionId}")]
        public IActionResult Delete(int sessionId)
        {
            var session = _context.Sessions.SingleOrDefault(sess => sess.SessionId == sessionId);
            if (session == null)
            {
                return NotFound();
            }
            if (session.UserId != User.Claims.SingleOrDefault(u => u.Type == "uid")?.Value)
            {
                return Unauthorized();
            }
            _context.Remove(session);
            _context.SaveChanges();
            return Ok();
        }
    }
}
