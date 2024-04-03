using System;
using System.Collections.Generic;

namespace Infrastructure.Material.DTOs
{
    // <summary>
    ///     Provides what a single sample is and its material attributes.
    /// </summary>
    public class PrimaryContainersResponseBody
    {
        public PrimaryContainersResponseBody()
        { }

        public PrimaryContainersResponseBody(string uid, string description, int position, DateTime inceptDate,
            IEnumerable<MaterialInfoResponseBody> material, int tagIdent, int batchId, string batchName)
        {
            Uid = uid;
            Description = description;
            Position = position;
            InceptDate = inceptDate;
            Material = material;
            TagIdent = tagIdent;
            BatchId = batchId;
            BatchName = batchName;
        }
        
        /// <summary>
        ///     Tag uid.
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        ///     Label information.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Position in storage container (box).
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        ///     Date stored.
        /// </summary>
        public DateTime InceptDate { get; set; }

        /// <summary>
        ///     Material headers & fields.
        /// </summary>
        public IEnumerable<MaterialInfoResponseBody> Material { get; set; }

        /// <summary>
        ///     Ident written to tag.
        /// </summary>
        public int TagIdent { get; set; }

        /// <summary>
        ///     Foriegn key to batch.
        /// </summary>
        public int BatchId { get; set; }

        /// <summary>
        ///     Name of associated batch.
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        ///     Validation.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (Uid == null)
                return false;

            if (TagIdent == 0)
                return false;

            if (string.IsNullOrWhiteSpace(Description))
                return false;

            if (BatchId == 0)
                return false;

            return true;
        }
    }
}
