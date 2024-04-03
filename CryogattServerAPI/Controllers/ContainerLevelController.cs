using System;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using System.Net.Http;
using ContainerLevels;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class ContainerLevelController : ApiController
    {
        public ContainerLevelController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        /// GET: api/v1/ContainerLevel/uid (Get the types of contents for a given container using its uid)
        /// i.e Does this container have samples inside 
        /// Assumes all contents are of the same level.
        [ResponseType(typeof(ContainerLevelTypes))]
        public IHttpActionResult GetContainerContentsLevel(string uid)
        {
            Log.Debug("Invoked");

            if (uid == null)
            {
                Log.Error("No uid passed");
                return BadRequest("No uid passed!");
            }

            try
            {
                // Exists
                if (UnitOfWork.ContainerManager.GetContainerResponseBody(uid) == null)
                {
                    Log.Error("UID not in database: " + uid);
                    return BadRequest("UID not in database: " + uid);
                }

                var level = UnitOfWork.ContainerManager.GetContainerLevel(uid, UnitOfWork.RuleContainerLevelCalculator);
                
                if (level == 0)
                {
                    Log.Error("Container has no contents!");
                    return BadRequest("Container has no contents!");
                }

                return Ok(level);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError(ex);
            }
        }

        /// GET: api/v1/ContainerLevel?TAGIDENT={ident}
        /// Get the level for a given Ident
        [ResponseType(typeof(ContainerLevelTypes))]
        public IHttpActionResult GetContainerLevel()
        {
            Log.Debug("Invoked");

            int ident = 0;

            foreach (var param in Request.GetQueryNameValuePairs())
            {
                if (param.Key.ToUpper().Trim() == "TAGIDENT")
                {
                    ident = int.Parse(param.Value);
                }
            }

            // Exists
            if (ident == 0)
            {
                Log.Error("No given tag ident!");
                return BadRequest("No given tag ident!!");
            }

            try
            {
                var level = UnitOfWork.ContainerManager.GetContainerLevel(ident, UnitOfWork.RuleContainerLevelCalculator);

                if (level == 0)
                {
                    Log.Error("Tag Identidy not recognised!");
                    return BadRequest("Tag Identidy not recognised!");
                }

                return Ok(level);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError(ex);
            }
        }
    }
}
