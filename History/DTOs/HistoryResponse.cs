using System.Collections.Generic;

namespace Infrastructure.History.DTOs
{
    public class HistoryResponse
    {
        public HistoryResponse()
        {

        }

        public HistoryResponse(int totalRecords, List<HistoryResponseBody> history)
        {
            TotalRecords = totalRecords;
            History = history;
        }

        public int TotalRecords { get; set; }

        public List<HistoryResponseBody> History { get; set; }
    }
}
