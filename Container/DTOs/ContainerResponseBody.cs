using Newtonsoft.Json;
using System;

namespace Infrastructure.Container.DTOs
{
    public class ContainerResponseBody
    {
        public ContainerResponseBody()
        { }

        public ContainerResponseBody(string uid, string description, string ident, 
            int tagIdent, DateTime? inceptDate, int containsQtty, string containsIdent)
        {
            Uid = uid;
            Description = description;
            Ident = ident;
            TagIdent = tagIdent;
            InceptDate = inceptDate;
            ContainsQtty = containsQtty;
            ContainsIdent = containsIdent;
        }

        /// <summary>
        ///     Tag Uid
        /// </summary>
        [JsonProperty]
        public string Uid { get; set; }

        /// <summary>
        ///     Label info.
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>
        ///     Name of Type.
        /// </summary>
        [JsonProperty]
        public string Ident { get; set; }

        /// <summary>
        ///     The code written to the tag.
        /// </summary>
        [JsonProperty]
        public int TagIdent { get; set; }

        /// <summary>
        ///     Date stored.
        /// </summary>
        [JsonProperty]
        public DateTime? InceptDate { get; set; }

        /// <summary>
        ///     Number of contents.
        /// </summary>
        [JsonProperty]
        public int ContainsQtty { get; set; }

        /// <summary>
        ///     Type of contents.
        /// </summary>
        [JsonProperty]
        public string ContainsIdent { get; set; }
    }
}
