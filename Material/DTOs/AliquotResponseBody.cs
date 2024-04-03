using Newtonsoft.Json;
using System.Collections.Generic;

namespace Infrastructure.Material.DTOs
{
    /// <summary>
    ///     Provides what a single sample is and its current location in storage.
    /// </summary>
    public class AliquotResponseBody
    {
        public AliquotResponseBody(string uid, string batchName, List<MaterialInfoResponseBody> material, int position, 
            string primaryDescription, string parentDescription, string grandParentDescription, string greatGrandParentDescription, string site, string status)
        {
            Uid = uid;
            BatchName = batchName;
            Material = material;
            Position = position;
            PrimaryDescription = primaryDescription;
            ParentDescription = parentDescription;
            GrandParentDescription = grandParentDescription;
            GreatGrandParentDescription = greatGrandParentDescription;
            Site = site;
            Status = status;
        }

        public AliquotResponseBody()
        {

        }

        /// <summary>
        ///     Tag Uid.
        /// </summary>
        [JsonProperty]
        public string Uid { get; set; }

        /// <summary>
        ///     Name of the batch the sample belongs to.
        /// </summary>
        [JsonProperty]
        public string BatchName { get; set; }

        /// <summary>
        ///     Headers and fields.
        /// </summary>
        [JsonProperty]
        public List<MaterialInfoResponseBody> Material { get; set; }

        /// <summary>
        ///     Position (only applies to vials in boxes).
        /// </summary>
        [JsonProperty]
        public int Position { get; set; }

        /// <summary>
        ///     Sample label infomation.
        /// </summary>
        [JsonProperty]
        public string PrimaryDescription { get; set; }

        /// <summary>
        ///     The descripion of the item the samples is stored in.
        /// </summary>
        [JsonProperty]
        public string ParentDescription { get; set; }//box

        /// <summary>
        ///     The descripion of the item the samples parent is stored in.
        /// </summary>
        [JsonProperty]
        public string GrandParentDescription { get; set; }//rack

        /// <summary>
        ///     The descripion of the item the samples parents parent is stored in.
        /// </summary>
        [JsonProperty]
        public string GreatGrandParentDescription { get; set; }//dewar

        /// <summary>
        ///     The site the samples is stored in.
        /// </summary>
        [JsonProperty]
        public string Site { get; set; }

        /// <summary>
        ///     Container status.
        /// </summary>
        [JsonProperty]
        public string Status { get; set; }
    }
}
