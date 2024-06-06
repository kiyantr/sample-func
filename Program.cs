using LocalFunctionProj;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults();

builder.ConfigureServices(services =>
    services
    .AddScoped<IAuditService, AuditService>()
        .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
        .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly))
);
var host = builder
    .Build();

host.Run();
