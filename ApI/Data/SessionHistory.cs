namespace ApI.Data
{
    public class SessionHistory
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string LicancePlate { get; set; }
        public Guid UserId { get; set; }
        public Guid GarageId { get; set; }
    }
}
