using System;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using System.Collections.Generic;
using ContainerTypes;
using System.Net.Http;
using Infrastructure.Container.DTOs;
using ContainerLevels;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class ContainerIdentsController : ApiController
    {
        public ContainerIdentsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        ///  GET: api/v1/ContainerIdents
        /// </summary>
        /// <returns>All General types that are none RFID enabled for a given level (Defaults to Root Level)</returns>
        [ResponseType(typeof(GeneralTypeResponse))]
        public IHttpActionResult GetGeneralNonRFIDEnabledContainers()
        {
            Log.Debug("Invoked");

            ContainerLevelTypes level = ContainerLevelTypes.ROOT_LEVEL_CONTAINER;
            foreach (var param in Request.GetQueryNameValuePairs())
            {
                if (param.Key.ToUpper().Trim() == "LEVEL")
                {
                    level = (ContainerLevelTypes)Enum.Parse(typeof(ContainerLevelTypes), param.Value);
                }
            }

            try
            {
                // Retreive all types that are non RFID enabled
                List<ContainerMake> types = ContainerIdentTypes.GetGeneralNonRFIDEnabledContainers();

                types.RemoveAll(t => UnitOfWork.RuleContainerLevelCalculator.GetLevel(t.Ident) != level);

                // Convert to Response
                GeneralTypeResponse resp = new GeneralTypeResponse
                {
                    Types = new List<GeneralTypeResponseBody>()
                };

                types.ForEach(t => resp.Types.Add(new GeneralTypeResponseBody(t)));
                resp.TotalRecords = resp.Types.Count;

                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// GET: api/v1/ContainerIdents/uid - where uid is general type ident.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>All Subtypes that are none RFID enabled belonging to the given general type</returns>
        [ResponseType(typeof(SubtypeResponse))]
        public IHttpActionResult Getsubtypes(UInt16 uid)
        {
            Log.Debug("Invoked");

            try
            {
                // Retreive all subtypes that are not RFID enabled
                List<ContainerModel> subtypes = ContainerIdentTypes.GetSubtypeNonRFIDEnabledContainers(uid);

                // Convert to response
                SubtypeResponse resp = new SubtypeResponse()
                {
                    Subtypes = new List<SubtypeResponseBody>()
                };
                subtypes.ForEach(s => resp.Subtypes.Add(new SubtypeResponseBody(s)));
                resp.TotalRecords = resp.Subtypes.Count;

                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError(ex);
            }
        }
    }
}
