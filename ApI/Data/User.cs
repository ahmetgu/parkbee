namespace ApI.Data;

public class User
{
    public Guid Id { get; set; }

    public string PartnerId { get; set; }

    public Session ActiveSession { get; set; }

    public bool HasActiveSession => ActiveSession != null;
}