using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using CryogattServerAPI.Trace;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class HistoryController : ApiController
    {
        public HistoryController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        // GET: api/v1/History/{UID}
        public IHttpActionResult GetContainerHistory(string uid)
        {
            Log.Debug("Invoked");

            try
            {
                // Is in database
                if (UnitOfWork.ContainerManager.GetContainerResponseBody(uid) == null)
                {
                    Log.Error("Container not in database");
                    return BadRequest("Container not in database");
                }

                return Ok(UnitOfWork.HistoryManager.GetContainerHistory(uid));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        ///     POST: api/v1/History/{UID}
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IHttpActionResult PostAuditCheckPoint(string uid)
        {
            Log.Debug("Invoked");

            try
            {
                // Get the container id from the parent uid
                var container = UnitOfWork.ContainerManager.GetContainer(uid);

                if (container == null)
                {
                    Log.Error("Container not found in database!");
                    return BadRequest("Container not found in database!");
                }

                // Get the current user 
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                // Get the site the user belongs to
                var site = UnitOfWork.ContainerManager.GetUsersSiteContainer(user.Username);

                // Update the history
                UnitOfWork.HistoryManager.AddCheckpoint(container, "Audited", site.Description, user.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        /// Enter checkpoints for a list of items that belong to the same parent
        /// </summary>
        /// <param name="uid">The 'reason' for checkpoint</param>
        /// <param name="uids">The uids of the tags</param>
        /// <returns></returns>
        public IHttpActionResult PostAuditCheckPoints(List<string> uids)
        {
            Log.Debug("Invoked");

            try
            {
                // Get containers from given uids
                var containers = uids
                    .Select(uid => UnitOfWork.ContainerManager.GetContainer(uid))
                    .ToList();

                if (containers.Any(c => c == null))
                {
                    Log.Error("Container not in database!");
                    return BadRequest("Container not in database!");
                }

                // Get the current user 
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                // Get the site the user belongs to
                var site = UnitOfWork.ContainerManager.GetUsersSiteContainer(user.Username);

                // Audit
                UnitOfWork.ContainerManager.AuditContainers(containers, user.Id, site.Description);               
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }

            return Ok();
        }
    }
}
