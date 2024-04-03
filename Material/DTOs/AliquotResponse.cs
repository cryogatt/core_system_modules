using System.Collections.Generic;

namespace Infrastructure.Material.DTOs
{
    public class AliquotResponse
    {
        public AliquotResponse()
        {

        }

        public AliquotResponse(int totalRecords, List<AliquotResponseBody> aliquots)
        {
            TotalRecords = totalRecords;
            Aliquots = aliquots;
        }

        public int TotalRecords { get; set; }

        public List<AliquotResponseBody> Aliquots { get; set; }
    }
}
