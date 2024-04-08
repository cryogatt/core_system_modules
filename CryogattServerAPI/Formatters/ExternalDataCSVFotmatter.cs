using CryogattServerAPI.Models;
using CryogattServerAPI.Trace;
using Infrastructure.Material.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace CryogattServerAPI.Formatters
{
    // ReSharper disable once InconsistentNaming
    public class ExternalDataCSVFormatter : BufferedMediaTypeFormatter
    {
        private static readonly char[] SpecialChars = new char[] { ',', '\n', '\r', '"' };

        private static readonly string Header =
            "Label," +
            "Stored in," +
            "Position," +
            "Stack," +
            "Dewar," +
            "Batch Name," +
            "Crop," +
            "Accession No," +
            "Line No," +
            "Introduction Date," +
            "Health Status," +
            "Cryo Date," +
            "Qtty Cryo," +
            "Viability Tested Qty," +
            "Cryobank Qty," +
            "Safety Duplication Qty," +
            "Viabilty Culture Date," +
            "Regrowth Date," +
            "Regrowth Rate," +
            "SN," 
            ;

        public ExternalDataCSVFormatter()
        {
            // Add the supported media type.
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
        }

        public override bool CanWriteType(Type type)
        {
            // TODO - Implement this..
            return true;
        }

        public override bool CanReadType(Type type)
        {
            // TODO - Implement this..
            return true;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            try
            {
                using (var writer = new StreamWriter(writeStream))
                {
                    writer.WriteLine(Header);
                    var data = value as List<AliquotResponseBody>;
                    foreach (var datum in data)
                    {
                        WriteItem(datum, writer);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            List<ExternalData> result;

            try
            {
                // Convert input stream to a streamReader
                StreamReader reader = new StreamReader(readStream);

                // Read
                string csv = reader.ReadToEnd();

                // Split each line
                List<string> lines = csv.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

                // Instantiate list
                result = new List<ExternalData>();

                // Parse for each header
                var headers = ParseLine(lines.First());

                // Remove headers
                lines.RemoveAt(0);

                int rowNo = 0;
                foreach (string line in lines)
                {
                    if (line.Count() != 0)
                    {
                        // New dictionary for headers and values of each row
                        Dictionary<string, string> singleRow = new Dictionary<string, string>();

                        // Parse the line and add to result
                        List<string> column = ParseLine(line);

                        int columnNo = 0;
                        foreach (string value in column)
                        {
                            // Add hearders & values to dictionary
                            singleRow.Add(headers[columnNo], value);
                            columnNo++;
                        }
                        // Convert to type
                        ExternalData newRow = new ExternalData(singleRow);
                        // Add to response
                        result.Add(newRow);
                        // Move to next Row                            
                        rowNo++;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            return result;
        }

        #region Private Methods

        /// <summary>
        /// Write a single ExternalData record to the streamwriter
        /// </summary>
        /// <param name="datum">The data record</param>
        /// <param name="writer">The stream writer</param>
        private void WriteItem(AliquotResponseBody datum, StreamWriter writer)
        {
            // Set location properties 
            writer.Write(Escape(datum.PrimaryDescription) + ",");
            writer.Write(Escape(datum.ParentDescription) + ",");
            writer.Write(Escape(datum.Position) + ",");
            writer.Write(Escape(datum.GrandParentDescription) + ",");
            writer.Write(Escape(datum.GreatGrandParentDescription) + ",");
            writer.Write(Escape(datum.BatchName) + ",");

            // Set Material properties
            foreach (MaterialInfoResponseBody d in datum.Material)
            {
                writer.Write(Escape(d.AttributeValueName) + ",");
            }

            // Add new line
            writer.WriteLine("");
        }

        /// <summary>
        /// Escape special character in an object
        /// </summary>
        /// <param name="o">The object</param>
        /// <returns>The escaped string</returns>
        private string Escape(object o)
        {
            if (o == null)
            {
                return "";
            }
            string field = o.ToString();
            if (field.IndexOfAny(SpecialChars) != -1)
            {
                // Delimit the entire field with quotes and replace embedded quotes with "".
                return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
            }
            else return field;
        }

        /// <summary>
        /// Parse a line from the csv and return a populated ExternalData data structure
        /// </summary>
        /// <param name="line">The line from the CSV file</param>
        /// <returns>The populated ExternalData structure</returns>
        private List<string> ParseLine(string line)
        {
            List<string> result = new List<string>();

            // Split the line into fields
            List<string> fields = line.Split(new[] { "," }, StringSplitOptions.None).ToList();


            // Trap empty lines
            if (!IsEmptyLine(fields))
            {
                // Create data structure to store the data and populate
                result = new List<string>(fields);
            }

            return result;
        }

        private bool IsEmptyLine(List<string> fields)
        {
            foreach (string field in fields)
            {
                if ((field != null) && (!field.Trim().Equals(string.Empty)))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Private Methods
    }
}