using System;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Web.Http.Description;
using Infrastructure.Container.DTOs;
using CryogattServerAPI.Trace;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class ContainersController : ApiController
    {
        public ContainersController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        // GET: api/v1/Container/uid
        [ResponseType(typeof(ContainerResponseBody))]
        public IHttpActionResult GetContainer(string uid)
        {
            Log.Debug("Invoked");

            try
            {
                // Query the database for specified container
                ContainerResponseBody container = UnitOfWork.ContainerManager.GetContainerResponseBody(uid);

                if (container == null)
                {
                    return BadRequest("Container not in database!");
                }
                // Return response
                return Ok(container);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // GET: api/v1/Containers?UIDS={UID}
        [ResponseType(typeof(ContainerResponseBody))]
        public IHttpActionResult GetContainerContents()
        {
            Log.Debug("Invoked");

            string uid = null;

            foreach (var param in Request.GetQueryNameValuePairs())
            {
                if (param.Key.ToUpper().Trim() == "UIDS")
                {
                    uid = param.Value;
                    break;
                }
            }
            try
            {
                // Return root level containers by default
                if (uid == null)
                    return Ok(UnitOfWork.ContainerManager.GetUsersSiteContents(RequestContext.Principal.Identity.Name));
                else
                    return Ok(UnitOfWork.ContainerManager.GetContainerContentsResponse(uid));
  
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        /// Add new Container record
        /// </summary>
        /// <param name="uid"></param> Used as ethier as the uid of the parent for storage and set to the uid of the newContainer when not stored
        /// <param name="newContainer"></param>
        /// <returns></returns>
        [ResponseType(typeof(ContainerResponseBody))]
        public IHttpActionResult PostNewContainer(string uid, ContainerResponseBody newContainer)
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
                // if uid already in the container database
                if (UnitOfWork.ContainerManager.GetContainerResponseBody(newContainer.Uid) != null)
                {
                    Log.Error("Container already in database!");
                    return BadRequest("Container already in database!");
                }

                // Get current user
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                // If uid is being used to set the parent other set to location of users site
                var id = (newContainer.Uid != uid) 
                    ? UnitOfWork.ContainerManager.AddContainers(newContainer, user.Id, uid) 
                    : UnitOfWork.ContainerManager.AddContainers(newContainer, user.Id, null, UnitOfWork.RuleContainerLevelCalculator);

                if (id == 0)
                    throw new Exception("Container ws not created");

                // TODO Add audit

                //return newly created entity
                return Ok(UnitOfWork.ContainerManager.GetContainerResponseBody(newContainer.Uid));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        /// PUT: api/Containers/uid (edited edittedContainer) - where uid is the Container.Uid field
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="edittedContainer"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContainer(string uid, ContainerResponseBody edittedContainer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (uid != edittedContainer.Uid)
                return BadRequest();

            try
            {
                // Get current user
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                // Update the container
                UnitOfWork.ContainerManager.UpdateContainer(edittedContainer, user.Id);

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}

