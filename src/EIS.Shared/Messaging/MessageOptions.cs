namespace EIS.Shared.Messaging;

public class MessageOptions
{
    public bool Enabled { get; set; }
    public string Interval { get; set; }
    public int MessageEvictionWindowInDays { get; set; }
}