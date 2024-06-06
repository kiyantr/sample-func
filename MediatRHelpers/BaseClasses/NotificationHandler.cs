using MediatR;

namespace LocalFunctionProj.MediatRHelpers.BaseClasses;

public abstract class NotificationHandler<TRequest> : INotificationHandler<TRequest> where TRequest : INotification
{
    public async Task Handle(TRequest notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => ProcessNotificationAsync(notification, cancellationToken));
    }

    protected abstract Task ProcessNotificationAsync(TRequest request, CancellationToken cancellationToken);
}