using MediatR;

namespace LocalFunctionProj.MediatRHelpers.BaseClasses;

public abstract record ApplicationRequest<TResponse> : IRequest<TResponse>;

public abstract record ApplicationNotification : INotification;
