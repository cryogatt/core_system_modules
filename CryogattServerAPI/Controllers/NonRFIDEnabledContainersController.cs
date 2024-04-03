using System;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using Infrastructure.Container.DTOs;

namespace CryogattServerAPI.Controllers
{

    // ReSharper disable once InconsistentNaming
    public class NonRFIDEnabledContainersController : ApiController
    {
        public NonRFIDEnabledContainersController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        /// POST: /api/NonRFIDEnabledContainers
        /// TODO - Extend for addition of ContainsQtty, etc.
        [ResponseType(typeof(void))]
        public IHttpActionResult PostContainer(ContainerResponseBody newContainer)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValidField("Uid") &&
                !ModelState.IsValidField("Description") &&
                !ModelState.IsValidField("Ident"))
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            if (newContainer == null)
            {
                Log.Error("No container passed");
                return BadRequest("No container passed");
            }

            //  Validate input 
            if (newContainer.ContainsQtty > 100)
                return BadRequest("Cannot create more than 100 sub-containers!");

            try
            {
                // Get current user
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                var id = UnitOfWork.ContainerManager.AddContainers(newContainer, user.Id, null, UnitOfWork.RuleContainerLevelCalculator);

                if (id == 0)
                    throw new Exception("Container ws not created");

                //return newly created entity
                return Ok(UnitOfWork.ContainerManager.GetContainerResponseBody(newContainer.Uid));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError(ex);
            }
        }
    }
}