using System;
using System.Web.Http;
using CryogattServerAPI.Trace;

namespace CryogattServerAPI.Controllers
{
    public class VersionController : ApiController
    {
        // GET: api/v1/Materials
        public IHttpActionResult GetVersion()
        {
            try
            {
                Log.Debug("Invoked");

                return Ok(new Models.Version());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}