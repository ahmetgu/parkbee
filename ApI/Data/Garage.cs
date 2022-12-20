namespace ApI.Data;

public class Garage
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Capacity { get; set; }

    public List<Door> Doors { get; set; }

    public List<Session> ActiveSessions { get; set; }

    public int AvailableSpots => Capacity - (ActiveSessions != null ? ActiveSessions.Count : 0);
            
}