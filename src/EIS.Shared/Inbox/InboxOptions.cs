namespace Eis.Shared.Inbox;

public class InboxOptions
{
    public bool Enabled { get; set; }
    public string Interval { get; set; }
    public int MessageEvictionWindowInDays { get; set; }
}