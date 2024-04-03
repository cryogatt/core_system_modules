using System;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.WebPages;
using CryogattServerAPI.Models;
using Infrastructure.Material.Entities;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class ImportExportController : ApiController
    {
        public ImportExportController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private IUnitOfWork UnitOfWork;

        /// <summary>
        ///     GET: api/v1/ExportRecordsCropFilter
        /// </summary>
        /// <returns></returns>
        //public IHttpActionResult GetExportRecordsCropFilter()
        //{
        //    Log.Debug("Invoked");

        //    // Initialise start and end dates to filter records
        //    DateTime startDate = new DateTime(1970, 1, 1);
        //    DateTime endDate = new DateTime();

        //    // Parse parameters and set start/end dates
        //    var queryParams = Request.GetQueryNameValuePairs();
        //    foreach (var p in queryParams)
        //    {
        //        if (p.Key.ToUpper().Trim() == "STARTDATE")
        //        {
        //            startDate = DateTime.ParseExact(p.Value, "d", CultureInfo.GetCultureInfo("en-gb"));
        //        }
        //        if (p.Key.ToUpper().Trim() == "ENDDATE")
        //        {
        //            endDate = DateTime.ParseExact(p.Value, "d", CultureInfo.GetCultureInfo("en-gb"));
        //            // Make end of day
        //            endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
        //        }
        //    }
        //    try
        //    {
        //        // Get data from db
        //        var resp = UnitOfWork.MaterialManager.GetAliquotsByDate(startDate, endDate);

        //        if (resp == null)
        //            return BadRequest("There are no records to export!");

        //        // Return response
        //        return Ok(resp.Aliquots);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.ToString());
        //        return InternalServerError();
        //    }
        //}

        /// <summary>
        ///     GET: api/v1/ExportRecords
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetExportRecords()
        {
            Log.Debug("Invoked");

            // Initialise start and end dates to filter records
            DateTime startDate = new DateTime(1970, 1, 1);
            DateTime endDate = new DateTime();
            string crop = string.Empty;
            bool hasCrop = false;

            // Parse parameters and set start/end dates
            var queryParams = Request.GetQueryNameValuePairs();
            foreach (var p in queryParams)
            {
                if (p.Key.ToUpper().Trim() == "STARTDATE")
                {
                    startDate = DateTime.ParseExact(p.Value, "d", CultureInfo.GetCultureInfo("en-gb"));
                }
                if (p.Key.ToUpper().Trim() == "ENDDATE")
                {
                    endDate = DateTime.ParseExact(p.Value, "d", CultureInfo.GetCultureInfo("en-gb"));
                    // Make end of day
                    endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                }
            }
            try
            {
                // Get data from db
                var resp =  UnitOfWork.MaterialManager.GetAliquotsByDate(startDate, endDate);

                if (resp == null)
                    return BadRequest("There are no records to export!");
                
                // Return response
                return Ok(resp.Aliquots);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST: api/v1/ImportRecords (import records)
        [ResponseType(typeof(void))]
        public IHttpActionResult PostImportRecords(List<ExternalData> records)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }
            try
            {
                // Get the user
                var user = UnitOfWork.UserManager.GetUser(RequestContext.Principal.Identity.Name);

                StringBuilder outputMessage = new StringBuilder();

                if (records.Count == 0)
                    return BadRequest("File empty!");

                records = records.Where(r => r.SingleRow.Count() != 0).ToList();
                // For every row
                foreach (ExternalData row in records)
                {
                    var existingBatch = UnitOfWork.MaterialManager.GetBatchInfo("Batch " + row.SingleRow.Values.ElementAt(0));

                    // If in  already in db
                    if (existingBatch != null)
                    {
                        Log.Error(existingBatch.Name + " " + "already exists in database!");
                        outputMessage.AppendLine(existingBatch.Name + " " + "already exists in database!");
                        continue;
                    }

                    // Create new record
                    var batch = new BatchInfo(
                        0,
                        "Batch " + row.SingleRow.Values.ElementAt(0),
                        "Batch imported via .CSV file",
                        user.Groups.ElementAt(0).Id,
                        int.Parse(row.SingleRow.Values.ElementAt(7)),
                        int.Parse(row.SingleRow.Values.ElementAt(6)),
                        int.Parse(row.SingleRow.Values.ElementAt(8)),
                        DateTime.Now);

                    // Commit to database
                    int batchId = UnitOfWork.MaterialManager.AddBatch(batch);

                    if (batchId == 0)
                    {
                        Log.Error(batch.Name + " " + "could not be added to the database!");
                        outputMessage.AppendLine(batch.Name + " " + "could not be added to the database!");
                        break;
                    }

                    int columnNo = 0;
                    // For every column
                    foreach (var field in row.SingleRow.Keys)
                    {
                        columnNo++;

                        // Skip first column
                        if (columnNo == 1)
                            continue;

                        var header = UnitOfWork.MaterialManager.GetAttributeField(field);

                        // Field exists in db
                        if (header == null)
                        {
                            outputMessage.AppendLine("Header ignored as not in database: " + field);
                            continue;
                        }

                        if (!row.SingleRow[field].IsEmpty())
                        {
                            // Create new value record
                            var value = new Infrastructure.Material.Entities.AttributeValue(
                                0,
                                row.SingleRow[field],
                                header.Id,
                                batch.Id);

                            // Add to db
                            int valueId = UnitOfWork.MaterialManager.AddAttributeValue(value);
                            if (valueId == 0)
                            {
                                Log.Error("Unable to add value for field");
                                return BadRequest("Error entering values into database!");
                            }
                        }
                    }
                }
                return Ok(outputMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}