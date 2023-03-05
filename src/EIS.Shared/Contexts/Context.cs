using System.Diagnostics;

namespace EIS.Shared.Contexts;

public sealed class Context : IContext
{
    public string ActivityId { get; }
    public string TraceId { get; }
    public string? MessageId { get; }
    public string? CausationId { get; }
    public string? UserId { get; }

    public Context()
    {
        ActivityId = Activity.Current?.Id ?? ActivityTraceId.CreateRandom().ToString();
        TraceId = string.Empty;
    }

    public Context(string activityId, string traceId, string? messageId = null, string? causationId = null,
        string? userId = null)
    {
        ActivityId = activityId;
        TraceId = traceId;
        MessageId = messageId;
        CausationId = causationId;
        UserId = userId;
    }
}