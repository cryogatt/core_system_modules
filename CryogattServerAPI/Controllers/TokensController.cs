using System.Web.Http;
using CryogattServerAPI.Trace;

namespace CryogattServerAPI.Controllers
{
    public class TokensController : ApiController
    {
        // POST: api/v1/Tokens (login) - dummy function to allow pipeline login requests
        public IHttpActionResult PostTokens()
        {
            Log.Debug("Invoked");

            return NotFound();
        }
    }
}
