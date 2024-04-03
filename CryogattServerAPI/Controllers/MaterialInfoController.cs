using System;
using System.Web.Http;
using CryogattServerAPI.Trace;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class MaterialInfoController : ApiController
    {
        public MaterialInfoController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        // GET: api/v1/MaterialInfo
        public IHttpActionResult GetMaterials()
        {
            Log.Debug("Invoked");

            try
            {
                return Ok(UnitOfWork.MaterialManager.GetMaterialFields());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}