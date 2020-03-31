using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace VG_AspNetCore_Web.ActionFilters
{
    public class LogActionAttribute : TypeFilterAttribute
    {
        public LogActionAttribute(bool on = false) : base(typeof(LogAction))
        {
            Arguments = new object[] { on };
        }

        private class LogAction : Attribute, IAsyncActionFilter
        {
            private readonly ILogger<LogAction> _logger;
            private readonly bool _on;
            public LogAction(ILogger<LogAction> logger, bool on)
            {
                _logger = logger;
                _on = on;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (_on)
                {
                    var controllerName = (string)context.RouteData.Values["Controller"];
                    var actionName = (string)context.RouteData.Values["Action"];
                    _logger.LogInformation($"Controller: {controllerName}, Action: {actionName}, OnActionExecuting - {DateTime.Now}");
                }
                var resultContext = await next();
                if (_on)
                {
                    var controllerName = (string)resultContext.RouteData.Values["Controller"];
                    var actionName = (string)resultContext.RouteData.Values["Action"];
                    _logger.LogInformation($"Controller: {controllerName}, Action: {actionName}, OnActionExecuted - {DateTime.Now}");
                }
            }
        }
    }
}