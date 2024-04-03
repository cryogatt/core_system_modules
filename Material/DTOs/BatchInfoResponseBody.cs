using System;
using System.Collections.Generic;

namespace Infrastructure.Material.DTOs
{
    public class BatchInfoResponseBody
    {
        public BatchInfoResponseBody()
        {

        }

        public BatchInfoResponseBody(int uid, string name, string notes, DateTime date, int cryoSeedQty, int testedSeedQty, int sDSeedQty, IEnumerable<MaterialInfoResponseBody> material)
        {
            Uid = uid;
            Name = name;
            Notes = notes;
            Date = date;
            CryoSeedQty = cryoSeedQty;
            TestedSeedQty = testedSeedQty;
            SDSeedQty = sDSeedQty;
            Material = material;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        public int Uid { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///     Free text.
        /// </summary>
        public string Notes { get; set; }
        
        /// <summary>
        ///     Creation date.
        /// </summary>
        public DateTime Date { get; set; }

        public int CryoSeedQty { get; set; }
        public int TestedSeedQty { get; set; }
        public int SDSeedQty { get; set; }

        /// <summary>
        ///     The headers (fields) & values associated to the batch.
        /// </summary>
        public IEnumerable<MaterialInfoResponseBody> Material { get; set; }
    }
}