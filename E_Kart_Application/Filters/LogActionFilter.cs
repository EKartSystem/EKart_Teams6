using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Kart_Application.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private Stopwatch _stopWatch;

        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopWatch = Stopwatch.StartNew();
            var actionName = context.ActionDescriptor.DisplayName;
            var userName = context.HttpContext.User.Identity?.Name ?? "Anonymous";
            _logger.LogInformation($"[START] Executing {actionName} | User: {userName}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopWatch?.Stop();
            var elapsed = _stopWatch?.ElapsedMilliseconds;
            var actionName = context.ActionDescriptor.DisplayName;

            _logger.LogInformation($"[END] Finished {actionName} | Time: {elapsed}ms");
        }
    }
}
