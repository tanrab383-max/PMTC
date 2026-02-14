using TNK.Core.Caching;
using TNK.Core.Domain;
using TNK.Core.Fakes;
using TNK.Data;
using TNK.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;

namespace TNK.Services.Authentication
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly TimeSpan _expirationTimeSpan;
        private readonly IUserervice _Userervice;
        private readonly ICacheManager _cacheManager;
        IDbContext _dbContext;

        #endregion
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        public FormsAuthenticationService(HttpContextBase httpContext, IUserervice _Userervice, ICacheManager _cacheManager)
        {
            this._httpContext = httpContext;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
            this._Userervice = _Userervice;
            this._cacheManager = _cacheManager;
        }


        #endregion

        #region Utilities

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <param name="ticket">Ticket</param>
        protected virtual TNK.Core.Domain.User GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
            //Thangtx thêm để bỏ user admin
            if (ticket.Name == "admin")
                return null;
            
            var username = ticket.UserData.Split('|')[0];
            var store = ticket.UserData.Split('|')[1];

            if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(store))
                return null;

            string key = string.Format("{0}_{1}", username, store);
            var user = _cacheManager.Get(CacheKey.keyUserName + key, () => _Userervice.get(username, key));
            return user;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="createPersistentCookie">A value indicating whether to create a persistent cookie</param>
        public virtual void SignIn(Core.Domain.User user, bool createPersistentCookie, string storeId)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            var userData = $"{user.UserName}|{storeId}";
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.UserName,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Sign out
        /// </summary>
        public virtual void SignOut()
        {
            var ticket = ((FormsIdentity)_httpContext.User.Identity).Ticket;
            var username = ticket?.UserData.Split('|')[0];
            var store = ticket?.UserData.Split('|')[1];
            string key = string.Format("{0}_{1}", username, store);
            //var user = _cacheManager.Get(CacheKey.keyUserName + key,
            _cacheManager.Remove(CacheKey.keyUser + key);
            _cacheManager.Remove(CacheKey.keyUserName + key);
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Get authenticated user
        /// </summary>
        public virtual TNK.Core.Domain.User GetAuthenticatedUser()
        {

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            //_cacheManager.Clear();
            var user = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
            return user;
        }

        #endregion

    }
}
