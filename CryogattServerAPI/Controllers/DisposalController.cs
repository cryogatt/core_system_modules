using CryogattServerAPI.Trace;
using History.Entities;
using Infrastructure.Container.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class DisposalController : ApiController
    {
        public DisposalController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        public IHttpActionResult GetDisposedItems()
        {
            Log.Debug("Invoked");

            try
            {         
                List<ContainerResponseBody> containers = UnitOfWork.ContainerManager.GetDisposedItems();
                            
                return Ok(new ContainerResponse(containers.Count,
                                                containers));
            }
            catch(Exception)
            {
                return InternalServerError();
            }
        }

        [ResponseType(typeof(ContainerResponseBody))]
        public IHttpActionResult PostDisposedItem(ContainerResponseBody container)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
                return BadRequest();

            if (container == null)
                return BadRequest();

            try
            {
                // Get the current user 
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                var site = user.Sites.FirstOrDefault(); // TODO Fix for multiple sites

                SetStatus(container, user, site);

                return Ok(container);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        private void SetStatus(ContainerResponseBody container, Infrastructure.Users.DTOs.UserResponseBody user, string site)
        {
            UnitOfWork.ContainerManager.AddDisposedItem(container.Uid, user.Id, site);

            var containerStatus = new List<ContainerStatus>
                {
                    new ContainerStatus
                    {
                        ContainerUid = container.Uid,
                        Status = "Disposed"
                    }
                };

            UnitOfWork.HistoryManager.SetContainerStatus(containerStatus, site, user.Id);
        }
    }
}