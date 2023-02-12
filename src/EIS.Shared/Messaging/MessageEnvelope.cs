namespace EIS.Shared.Messaging;

public record MessageEnvelope<T>(T Message, string CorrelationId) where T : IMessage;