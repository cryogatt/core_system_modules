
using System.Collections.Generic;

namespace Infrastructure.Container.DTOs
{
    public class ContainerResponse
    {
        public ContainerResponse(int totalRecords, List<ContainerResponseBody> containers)
        {
            TotalRecords = totalRecords;
            Containers = containers;
        }

        public int TotalRecords { get; set; }

        public List<ContainerResponseBody> Containers { get; set; }
    }
}
