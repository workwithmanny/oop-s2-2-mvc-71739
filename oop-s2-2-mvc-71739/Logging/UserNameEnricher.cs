using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace oop_s2_2_mvc_71739.Logging;

public sealed class UserNameEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var userName = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (!string.IsNullOrWhiteSpace(userName))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserName", userName));
        }
    }
}
