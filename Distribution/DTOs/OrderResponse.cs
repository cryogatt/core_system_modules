using System.Collections.Generic;

namespace Distribution.DTOs
{
    public class OrdersResponse
    {
        public OrdersResponse()
        {

        }

        public OrdersResponse(int totalRecords, List<OrdersResponseBody> orders)
        {
            TotalRecords = totalRecords;
            Orders = orders;
        }

        public int TotalRecords { get; set; }

        public List<OrdersResponseBody> Orders { get; set; }
    }
}
