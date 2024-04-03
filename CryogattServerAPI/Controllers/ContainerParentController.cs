using System;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using Infrastructure.Container.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class ContainerParentController : ApiController
    {
        public ContainerParentController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        // GET: api/v1/ContainerParent/{parentUID}
        [ResponseType(typeof(ContainerResponse))]
        public IHttpActionResult GetContainerParents(string uid)
        {
            Log.Debug("Invoked");

            if (uid == null)
            {
                Log.Error("UID not recognised!");
                return BadRequest("UID not recognised!");
            }

            try
            {
                var result = UnitOfWork.ContainerManager.GetContainersParents(uid);

                if (result == null)
                    return BadRequest("uid not in database: " + uid);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}
