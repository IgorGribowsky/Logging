using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;

namespace BrainstormSessions.Filters
{
    public class ErrorHandlingFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                Log.Error(context.Exception.Message + context.Exception.StackTrace);
            }
        }
    }
}
