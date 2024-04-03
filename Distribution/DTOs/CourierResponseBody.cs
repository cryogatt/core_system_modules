namespace Distribution.DTOs
{
    public class CourierResponseBody
    {
        public CourierResponseBody()
        {

        }

        public CourierResponseBody(int id, string shipperName, string shipperUid, int shipmentId, string shipmentConsignmentNo)
        {
            Id = id;
            ShipperName = shipperName;
            ShipperUid = shipperUid;
            ShipmentId = shipmentId;
            ShipmentConsignmentNo = shipmentConsignmentNo;
        }

        public int Id { get; set; }

        public string ShipperName { get; set; }

        public string ShipperUid { get; set; }

        public int ShipmentId { get; set; }

        public string ShipmentConsignmentNo { get; set; }
    }
}
