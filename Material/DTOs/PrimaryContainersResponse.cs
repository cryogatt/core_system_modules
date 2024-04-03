using System.Collections.Generic;

namespace Infrastructure.Material.DTOs
{
    public class PrimaryContainersResponse
    {
        public PrimaryContainersResponse()
        { }

        public PrimaryContainersResponse(int totalRecords, List<PrimaryContainersResponseBody> primaryContainers)
        {
            TotalRecords = totalRecords;
            PrimaryContainers = primaryContainers;
        }

        public int TotalRecords { get; set; }

        public List<PrimaryContainersResponseBody> PrimaryContainers { get; set; }
    }
}
