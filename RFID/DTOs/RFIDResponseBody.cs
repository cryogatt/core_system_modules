using System.Collections.Generic;

namespace Infrastructure.RFID.DTOs
{
    /// <summary>
    /// Provides the tag details of a container and the information regarding what type of material
    /// is in the container and its location in storage - ToDo Consider removing Material as its batchName could suffice
    /// </summary>
    public class RFIDResponseBody
    {
        public string Uid { get; set; }
        public int Position { get; set; }
        public string Description { get; set; }
        public string ContainerType { get; set; }
        public string BatchName { get; set; }
        public Dictionary<string, string> ParentUidDescription { get; set; }
    }
}
