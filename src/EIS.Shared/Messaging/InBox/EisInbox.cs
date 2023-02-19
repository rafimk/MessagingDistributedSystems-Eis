using System.ComponentModel.DataAnnotations;

namespace EIS.Shared.Messaging.InBox;

public class EisInbox
{
    [Key]
    public string MessageId { get; set; }
    public DateTime ProcessedAt { get; set; }
}