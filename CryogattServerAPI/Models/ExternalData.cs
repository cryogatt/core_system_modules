using System.Collections.Generic;

namespace CryogattServerAPI.Models
{
    public class ExternalData
    {
        #region Constructors

        public ExternalData() { }

        public ExternalData(Dictionary<string, string> singleRow)
        {
            this.SingleRow = singleRow;
        }
        #endregion Constructors

        #region Public Properties

        public Dictionary<string, string> SingleRow { get; set; }

        #endregion Public Properties
    }
}