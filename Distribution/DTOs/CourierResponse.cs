using System.Collections.Generic;

namespace Distribution.DTOs
{
    public class CourierResponse
    {
        public CourierResponse()
        {

        }

        public CourierResponse(int totalRecords, List<CourierResponseBody> couriers)
        {
            TotalRecords = totalRecords;
            Couriers = couriers;
        }

        public int TotalRecords { get; set; }

        public List<CourierResponseBody> Couriers { get; set; }
    }
}
