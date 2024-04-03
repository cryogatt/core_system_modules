using System;
using System.Web.Http;
using System.Collections.Generic;
using CryogattServerAPI.Trace;
using Infrastructure.Users.Entites;
using System.Linq;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class SitesController : ApiController
    {
        public SitesController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        ///     GET: api/v1/Sites
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetAllSites()
        {
            try
            {
                Log.Debug("Invoked");

                List<Site> sites = UnitOfWork.UserManager.GetAllSites();

                if (sites == null)
                    throw new Exception("No Sites available!");

                var resp = sites.Select(s => s.Name);

                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}