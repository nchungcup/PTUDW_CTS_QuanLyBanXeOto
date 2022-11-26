using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System.Linq;

namespace PTUDW_CTS_QuanLyBanXeOto.Controllers
{
    public class ClientBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var adminses = HttpContext.Session.GetString("username");
            var clientses = HttpContext.Session.GetString("client");
            if (adminses == null && clientses == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Signin", controller = "SigninSignup", area = "default" }));
            }
            else if (adminses != null && clientses == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home", area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
