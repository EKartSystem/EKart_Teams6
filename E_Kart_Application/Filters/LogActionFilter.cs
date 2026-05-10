using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Kart_Application.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            _logger.LogInformation($"[END] Finished {actionName}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        { 
            var actionName = context.ActionDescriptor.DisplayName;
            var userName = context.HttpContext.User.Identity?.Name ?? "Anonymous";
            _logger.LogInformation($"[START] Executing {actionName} | User: {userName}");
        }
    }
}
