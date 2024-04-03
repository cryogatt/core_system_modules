using System.Collections.Generic;

namespace Infrastructure.Material.DTOs
{
    public class BatchInfoResponse
    {
        public BatchInfoResponse()
        { }

        public BatchInfoResponse(int totalRecords, List<BatchInfoResponseBody> batches)
        {
            TotalRecords = totalRecords;
            Batches = batches;
        }

        public int TotalRecords { get; set; }

        public List<BatchInfoResponseBody> Batches { get; set; }
    }
}
