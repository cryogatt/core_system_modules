namespace Infrastructure.Material.DTOs
{
    public class BatchResponseBody
    {
        public BatchResponseBody()
        {

        }

        public BatchResponseBody(int uid, string batchType, int sampleQty)
        {
            Uid = uid;
            BatchType = batchType;
            SampleQty = sampleQty;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        ///     Name of Batch type for grouping.
        /// </summary>
        public string BatchType { get; set; }
                
        /// <summary>
        ///     Number of samples in the batch.
        /// </summary>
        public int SampleQty { get; set; }
    }
}
