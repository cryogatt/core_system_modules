using System.Collections.Generic;

namespace Infrastructure.Material.DTOs
{
    /// <summary>
    ///     Combines the material information with its field names and values.
    /// </summary>
    public class MaterialInfoResponse
    {
        public MaterialInfoResponse(int totalRecords, IEnumerable<MaterialInfoResponseBody> materialInfo)
        {
            TotalRecords = totalRecords;
            MaterialInfo = materialInfo;
        }

        public int TotalRecords { get; set; }

        public IEnumerable<MaterialInfoResponseBody> MaterialInfo { get; set; }
    }
}
