using System;

namespace Infrastructure.History.DTOs
{
    public class HistoryResponseBody
    {
        public HistoryResponseBody()
        {

        }

        public HistoryResponseBody(string username, string reason, string location, DateTime date)
        {
            Username = username;
            Reason = reason;
            Location = location;
            Date = date;
        }

        /// <summary>
        ///     Who.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     What/Why.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        ///     Where.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        ///     When.
        /// </summary>
        public DateTime Date { get; set; }
    }
}