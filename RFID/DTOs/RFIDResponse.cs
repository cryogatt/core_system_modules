using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RFID.DTOs
{
    public class RFIDResponse
    {
        public RFIDResponse(int totalRecords, List<RFIDResponseBody> tags)
        {
            TotalRecords = totalRecords;
            Tags = tags;
        }

        public int TotalRecords { get; set; }

        public List<RFIDResponseBody> Tags { get; set; }
    }
}
