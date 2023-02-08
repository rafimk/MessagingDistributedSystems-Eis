namespace EIS.Outbox;

public class EisOutbox
{
    [Key]
    public string MessageId { get; set; }
    public string TopicOrQueName { get; set; }
    public string MessageType { get; set; }
    public string Payload { get; set; }
    public DateTime RecivedAt { get; set;}
    public DateTime? PublishedAt { get; set; }
}