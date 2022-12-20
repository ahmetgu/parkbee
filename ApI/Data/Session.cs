namespace ApI.Data
{
    public class Session
    {
        public Guid Id { get; set; }

        public DateTime StartTime { get; set; }  

        public Guid UserId  { get; set; }

        public Guid GarageId { get; set; }

        public string LicencePlate { get; set; }
    }
}
