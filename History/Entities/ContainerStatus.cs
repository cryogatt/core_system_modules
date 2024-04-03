namespace History.Entities
{
    public class ContainerStatus
    {
        public ContainerStatus()
        {

        }

        public ContainerStatus(int id, string containerUid, string status)
        {
            Id = id;
            ContainerUid = containerUid;
            Status = status;
        }

        public int Id { get; set; }

        public string ContainerUid { get; set; }

        public string Status { get; set; }
    }
}
