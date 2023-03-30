using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CIPlatform.Helpers
{
    public class SessionHelper: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            string useremail= context.HttpContext.Session.GetString("useremail");
            if (useremail == null)
            {              
                context.Result = new RedirectToActionResult("Login", "Account",true);
                return;

            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
