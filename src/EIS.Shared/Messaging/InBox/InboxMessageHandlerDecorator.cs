using EIS.Shared.Accessors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EIS.Shared.Messaging.InBox;

public class InboxMessageHandlerDecorator<TMessage> : INotificationHandler<TMessage> where TMessage : class, INotification
{
    private readonly INotificationHandler<TMessage> _handler;
    private readonly IMessageIdAccessor _messageIdAccessor;
    private readonly DbContext _dbContext;

    public InboxMessageHandlerDecorator(INotificationHandler<TMessage> handler, Func<DbContext> getContext, 
        IMessageIdAccessor messageIdAccessor)
    {
        _handler = handler;
        _messageIdAccessor = messageIdAccessor;
        _dbContext = getContext();
    }

    public async void Handle(TMessage message)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var messageId = _messageIdAccessor.GetMessageId();
            
            if (await _dbContext.Set<EisInbox>().AnyAsync(x => x.MessageId == messageId))
            {
                return;
            }

            _handler.Handle(message);

            var deduplicationModel = new EisInbox {MessageId = messageId, ProcessedAt = DateTime.UtcNow};
            await _dbContext.Set<EisInbox>().AddAsync(deduplicationModel);

            await transaction.CommitAsync();
            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}