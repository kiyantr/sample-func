using MediatR;

namespace LocalFunctionProj.MediatRHelpers.BaseClasses;

public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected abstract Task<TResponse> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken);

    async Task<TResponse> IRequestHandler<TRequest, TResponse>.Handle(TRequest request, CancellationToken cancellationToken)
    {
        return await ProcessRequestAsync(request, cancellationToken);
    }
}