using System.Xml.Linq;
using LocalFunctionProj.MediatRHelpers.BaseClasses;
using MediatR;

namespace LocalFunctionProj;

public record AuditEvent<T> : ApplicationNotification
{
    public int Id { get; set; } // Auto-incrementing ID (optional)
    public DateTime DateTime { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; } // Optional, for storing user's full name
    public string EntityType { get; set; }
    public int EntityId { get; set; }
    public string Action { get; set; }
    public T OldValues { get; set; } // Optional, for storing a JSON representation of old values before change
    public T NewValues { get; set; } // Optional, for storing a JSON representation of new values after change
    public string IpAddress { get; set; } // Optional, for storing the user's IP address

    public AuditEvent()
    {
        DateTime = DateTime.UtcNow;

    }
}

public class AuditEventCls
{
    public int Id { get; set; } // Auto-incrementing ID (optional)
    public DateTime DateTime { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; } // Optional, for storing user's full name
    public string EntityType { get; set; }
    public int EntityId { get; set; }
    public string Action { get; set; }
    public string OldValues { get; set; } // Optional, for storing a JSON representation of old values before change
    public string NewValues { get; set; } // Optional, for storing a JSON representation of new values after change
    public string IpAddress { get; set; } // Optional, for storing the user's IP address

    public AuditEventCls()
    {
        DateTime = DateTime.UtcNow;

    }
}

public class Plan
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Group { get; set; }
    public IEnumerable<string> Claims { get; set; } = Enumerable.Empty<string>();
    public Dictionary<string, object> Features { get; set; } = new Dictionary<string, object>();
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public SimpleUser Owner { get; set; }
    public SimpleUser UpdatedBy { get; set; }
    public int Order { get; set; }
}

public class SimpleUser
{
    public string Id { get; set; }

    public string XCid { get; set; }

    public string Fullname { get; set; }

    public string ImageURL { get; set; }

    public int BackgroundColor { get; set; }

    public string CompanyId { get; set; }

    public string CompanyDomain { get; set; }

    public string Email { get; set; }
}