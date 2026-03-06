using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exam_Mgmt.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.Request.Cookies["username"];

            if (string.IsNullOrEmpty(username))
            {
                context.Result = new UnauthorizedObjectResult("Please login first");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}