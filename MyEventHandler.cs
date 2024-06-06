
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using LocalFunctionProj.Helpers;
using LocalFunctionProj.MediatRHelpers.BaseClasses;

namespace LocalFunctionProj
{
    public class MyEventHandler : NotificationHandler<AuditEvent<Plan>>
    {
        private readonly IMapper _mapper;

        public MyEventHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected override async Task ProcessNotificationAsync(AuditEvent<Plan> request, CancellationToken cancellationToken)
        {
            var diff = PropertyDifferenceExtensions.CompareObjects(request.OldValues, request.NewValues);
            // var aaa = _mapper.Map<AuditEventCls>(request);
            var diffString = JsonSerializer.Serialize(diff);
            System.Console.WriteLine(diffString);
            throw new Exception("sdfkljsdfjdslkfdslfjdsl");
        }
    }
}
