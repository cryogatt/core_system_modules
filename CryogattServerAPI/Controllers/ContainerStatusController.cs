using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CryogattServerAPI.Trace;
using History.Entities;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class ContainerStatusController : ApiController
    {
        public ContainerStatusController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;
        
        public IHttpActionResult PostContainerStatus(IEnumerable<ContainerStatus> containerStatus)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                // Get the current user 
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                var site = user.Sites.FirstOrDefault(); // TODO Fix for multiple sites

                UnitOfWork.HistoryManager.SetContainerStatus(containerStatus, site, user.Id);
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.ToString());

                return BadRequest(ex.Message);
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
