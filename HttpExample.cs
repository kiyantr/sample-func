using System.Net;
using Bogus;
using LocalFunctionProj.Helpers;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace LocalFunctionProj
{
    public class HttpExample
    {
        private readonly ILogger _logger;
        private readonly IAuditService _auditService;

        public HttpExample(ILoggerFactory loggerFactory, IAuditService auditService)
        {
            _logger = loggerFactory.CreateLogger<HttpExample>();
            _auditService = auditService;
        }

        [Function("httpExample")]
        public async Task<Object> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

var id = Guid.NewGuid().ToString();
var claims = new Faker<List<string>>().Generate();
var claims2 = new Faker<List<string>>().Generate();
var owner = new Faker<SimpleUser>();
            var objectA = new Faker<Plan>()
            .RuleFor(x => x.Id, _ => id)
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Claims, f => claims.OrderBy(a => a))
            .RuleFor(x => x.Owner, _ => owner)
            .RuleFor(x => x.UpdatedBy, _ => new Faker<SimpleUser>());

            var objectB = new Faker<Plan>()
            .RuleFor(x => x.Id, _ => id)
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Claims, f => claims.Union(claims2).OrderByDescending(a => a))
            .RuleFor(x => x.Owner, _ => owner)
            .RuleFor(x => x.UpdatedBy, f => new Faker<SimpleUser>());

            var a = new AuditEvent<Plan>()
            {
                OldValues = objectA,
                NewValues = objectB,
            };
            // a.ThresholdReached += c_ThresholdReached;
            // a.Save();

            await _auditService.SaveAuditEvent(a);

            response.WriteString("Welcome to Azure Functions!");

            var result = new
            {
                isError = false,
                data = new
                {
                    item = 1
                }
            };
            return result;
        }
    }

    public interface IAuditService
    {
        Task SaveAuditEvent<T>(AuditEvent<T> auditEvent);

    }
    public class AuditService : IAuditService
    {
        private readonly IMediator _mediator;

        public AuditService(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task SaveAuditEvent<T>(AuditEvent<T> auditEvent)
        {
            // Implement logic to save the audit event (e.g., to database)
            // OnAuditLog?.Invoke(this, auditEvent); // Raise the event with the audit data
            // Create notification instance

            // Publish the event using mediator
            _ = Task.Run(() => _mediator.Publish(auditEvent)) ;
        }
    }
}
