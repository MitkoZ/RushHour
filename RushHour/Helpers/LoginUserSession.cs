using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RushHour.Helpers
{

    public class LoginUserSession
    {
        #region Properties
        public int UserId { get; private set; }
        public string Name { get; set; }
        public string Email { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public bool IsAdmin { get; private set; }
        #endregion

        #region Constructors
        private LoginUserSession()
        {
            IsAuthenticated = false;
        }
        #endregion

        #region Public properties
        public static LoginUserSession Current
        {
            get
            {
                LoginUserSession loginUserSession = (LoginUserSession)HttpContext.Current.Session["LoginUser"];
                if (loginUserSession == null)
                {
                    loginUserSession = new LoginUserSession();
                    HttpContext.Current.Session["LoginUser"] = loginUserSession;
                }
                return loginUserSession;
            }
        }
        #endregion

        #region public methods
        public void SetCurrentUser(int userId, string name, string email, bool isAdmin)
        {
            this.IsAuthenticated = true;
            this.UserId = userId;
            this.Name = name;
            this.Email = email;
            this.IsAdmin = isAdmin;
        }

        public void Logout()
        {
            this.IsAuthenticated = false;
            this.Name = null;
            this.UserId = 0;
            this.Email = string.Empty;
            this.IsAdmin = false;
        }
        #endregion
    }
}
