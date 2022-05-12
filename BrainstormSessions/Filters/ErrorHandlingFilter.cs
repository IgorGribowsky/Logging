using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;

namespace BrainstormSessions.Filters
{
    public class ErrorHandlingFilter : Attribute, IActionFilter
    {
        private readonly ILogger _logger;

        public ErrorHandlingFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                _logger.Error(context.Exception.Message + context.Exception.StackTrace);
            }
        }
    }
}
