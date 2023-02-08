namespace Eis.Shared.Outbox;

public class OutboxOptions
{
    public bool Enabled { get; set; }
    public string Interval { get; set; }
    public int MessageEvictionWindowInDays { get; set; }
}