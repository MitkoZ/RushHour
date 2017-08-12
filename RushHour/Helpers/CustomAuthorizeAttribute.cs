using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RushHour.Helpers
{
    public class CustomAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public string AccessRightsInput { get; set; }
        private List<AllowAccessTo> AccessRights { get; set; }
        private enum AllowAccessTo
        {
            AnonymousUser = 1,
            NormalLoggedUser = 2,
            Admin = 3
        }
        public bool Redirect { get; set; } //if this property is set to true then the user won't be showed the AccessDenied page, but he will be redirected to a chosen action
        public CustomAuthorizeAttribute()
        {
            this.AccessRights = new List<AllowAccessTo>();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            {
                return;
            }

            if (AccessRightsInput == null) //Default behavior: Don't allow access to anyone
            {
                filterContext.Result = new ViewResult { ViewName = "AccessDenied" };
            }
            else if (AccessRightsInput != null)
            {
                #region Setting Rights
                //AnonymousUser, NormalLoggedUser, Admin
                if (AccessRightsInput[0].ToString() == "1")
                {
                    AccessRights.Add(AllowAccessTo.AnonymousUser);
                }
                if (AccessRightsInput[1].ToString() == "1")
                {
                    AccessRights.Add(AllowAccessTo.NormalLoggedUser);
                }
                if (AccessRightsInput[2].ToString() == "1")
                {
                    AccessRights.Add(AllowAccessTo.Admin);
                }
                #endregion
                #region Access Granted
                if (AccessRights.Contains(AllowAccessTo.AnonymousUser) && !LoginUserSession.Current.IsAuthenticated && !LoginUserSession.Current.IsAdmin) //acess granted only for an anonymous user
                {
                    return;
                }
                else if (AccessRights.Contains(AllowAccessTo.NormalLoggedUser) && LoginUserSession.Current.IsAuthenticated && !LoginUserSession.Current.IsAdmin) //acess granted only for an normal logged user
                {
                    return;
                }
                else if (AccessRights.Contains(AllowAccessTo.Admin) && LoginUserSession.Current.IsAuthenticated && LoginUserSession.Current.IsAdmin) //acess granted only for an admin
                {
                    return;
                }
                #endregion
                #region Access Denied
                //if this is reached, then the user doesn't have the rights to enter the action and will be either redirected to an other action or AccessDenied page
                if (Redirect && !LoginUserSession.Current.IsAuthenticated && !LoginUserSession.Current.IsAdmin)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));  //redirect to anonymous user main page
                }
                else if (Redirect && LoginUserSession.Current.IsAuthenticated && !LoginUserSession.Current.IsAdmin)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));  //redirect to normal logged user main page
                }
                else if (Redirect && LoginUserSession.Current.IsAuthenticated && LoginUserSession.Current.IsAdmin)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" })); //redirect to admin user main page
                }
                else
                {
                    filterContext.Result = new ViewResult { ViewName = "AccessDenied" }; //if this is reached, then the user doesn't have the rights to enter the action neither does he have to be redirected
                }
                #endregion
            }
        }
    }
}