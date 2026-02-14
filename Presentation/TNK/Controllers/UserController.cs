using TNK.Core;
using TNK.Core.Domain;
using TNK.Model.Authentication;
using TNK.Services.Authentication;
using TNK.Services.Roles;
using TNK.Services.Users;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using TNK.Core.Caching;
using TNK.Data;
using System.Web.Security;

namespace TNK.Controllers
{
    [Authorize]
    public class UserController : BasePublicController
    {
        IUserRegistrationService _userRegistrationService;
        IUserervice _userService;
        IAuthenticationService _authenticationService;
        IRoleService _roleService;
        AuthenticationModel _auModel = new AuthenticationModel();
        HttpContextBase _httpContext;
        ICacheManager _cacheManager;
        public UserController(
            IUserRegistrationService _userRegistrationService
            , IUserervice _userService
            , IAuthenticationService _authenticationService
            , IRoleService _roleService
            , ICacheManager _cacheManager
            , HttpContextBase _httpContext) : base()
        {
            this._userRegistrationService = _userRegistrationService;
            this._userService = _userService;
            this._authenticationService = _authenticationService;
            this._roleService = _roleService;
            this._httpContext = _httpContext;
            this._cacheManager = _cacheManager;

            _auModel.Area = Areas.Web;
            _auModel.Controller = ListController.User;
        }
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            if (_authenticationService.GetAuthenticatedUser() == null)
            {
                ViewBag.ReturnUrl = ReturnUrl;
                string hostName = Request.Url.Host;
                hostName = hostName.Replace("wwww","");
                hostName = hostName.Replace("https://", "");
                hostName = hostName.Replace("http://", "");
                log.Param("hostname",hostName);
                Dictionary<string,string> lstStore =  StoreService.GetDealerByDomain(hostName);                
                ViewBag.ListStore = lstStore;
                ViewBag.StoreByDomain = StoreService.GetDealerIdByDomain(hostName);
                return View();
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }

            var user = _authenticationService.GetAuthenticatedUser();
            _cacheManager.Remove(CacheKey.keyRoleByUser + user.UserId);
            _cacheManager.Remove(CacheKey.keyMenuByRole + user.UserId);
            _cacheManager.Remove(CacheKey.keyRoleInMenuAction + user.UserId);

            _userRegistrationService.Insert_Logout();
            _authenticationService.SignOut();
            return Redirect("~/User/Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string userName, string passWord, string ReturnUrl,string selectedStoreId)
        {
            try
            {
                //clear session cho nay de bo menu khi bi dinh cache
                Session.Clear();
                string key = string.Format("{0}_{1}", userName, selectedStoreId);
                //var user = _cacheManager.Get(CacheKey.keyUserName + key,
                _cacheManager.Remove(CacheKey.keyUserName + key);
                if (_userRegistrationService.ValidateUser(userName, passWord, selectedStoreId))
                {
                    //Lưu tên showrom
                    var storeIdCookie = new HttpCookie("StoreId", selectedStoreId)
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddYears(1)
                    };
                    Response.Cookies.Add(storeIdCookie);
                    //var user = _userRegistrationService.GetUser(userName, selectedStoreId);
                    //if (user != null)
                    //{
                    //    var uid = user.UserId;
                    //    _cacheManager.Remove(CacheKey.keyRoleByUser + uid);
                    //    _cacheManager.Remove(CacheKey.keyMenuByRole + uid);
                    //    _cacheManager.Remove(CacheKey.keyRoleInMenuAction + uid);
                       
                    //}
                    HttpContext.Session["StoreId"] = selectedStoreId;
                    if (string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect("/");
                    }
                    return Redirect(ReturnUrl);
                }
                else
                {
                    ViewBag.Title = "Đăng nhập thất bại";
                    ViewBag.ErrorMessage = "Đăng nhập thất bại. Vui lòng kiểm tra lại tên đăng nhập hoặc mật khẩu.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                ViewBag.Title = "Đăng nhập thất bại";
                return View();
            }
            
        }

        public ActionResult Create()
        {
            _auModel.Action = "Create";
            if (_userRegistrationService.CheckRole(_auModel))
            {
                User model = new User();
                return View(model);
            }
            return Redirect("~/Home/NoAccess");
        }

        [HttpPost]
        public ActionResult Create(User model)
        {
            _auModel.Action = "Create";
            if (_userRegistrationService.CheckRole(_auModel))
            {
                var obj = model;
                _userRegistrationService.CreateUser(obj);
                return Redirect("~/User/Index");
            }
            return Redirect("~/Home/NoAccess");
        }

        public ActionResult Index()
        {
            _auModel.Action = "Index";
            
            var user = _userRegistrationService.getCurrentUser();
            if (_userRegistrationService.CheckRole(_auModel) )//&& (user.UserName == "admin" ) )
            {
                _auModel.Action = Actions.Edit;
                ViewBag.Edit = _userRegistrationService.CheckRole(_auModel);
                _auModel.Action = Actions.Details;
                ViewBag.Details = _userRegistrationService.CheckRole(_auModel);
                _auModel.Action = Actions.Delete;
                ViewBag.Delete = _userRegistrationService.CheckRole(_auModel);
                _auModel.Action = Actions.Create;
                ViewBag.Create = _userRegistrationService.CheckRole(_auModel);

                List<User> model = _userService.get();
                return View(model);
            }
            else
                return Redirect("~/Home/NoAccess");
        }


        public ActionResult Edit(int id = 0)
        {
            _auModel.Action = Actions.Edit;
            if (_userRegistrationService.CheckRole(_auModel))
            {
                User model = new User();
                if (id != 0)
                    model = _userService.getByID(id);
                return View(model);
            }
            else
                return Redirect("~/Home/NoAccess");
        }

        [HttpPost]
        public ActionResult Edit(User model)
        {
            if (_userRegistrationService.CheckRole(_auModel))
            {
                if (_userService.Update(model, (Guid)_userRegistrationService.getCurrentUser().UserId))
                {
                    ViewBag.Alert = "Cập nhật thành công";
                }
                else
                {
                    ViewBag.Alert = "Cập nhật thất bại.";
                }

                return View(model);
            }
            else
                return Redirect("~/Home/NoAccess");
        }

        public ActionResult Details(int id)
        {
            _auModel.Action = Actions.Details;
            if (_userRegistrationService.CheckRole(_auModel))
            {
                User model = new User();
                if (id != 0)
                    model = _userService.getByID(id);
                return View(model);
            }
            else
                return Redirect("~/Home/NoAccess");
        }

        public ActionResult Delete(int id)
        {
            _auModel.Action = Actions.Delete;
            if (_userRegistrationService.CheckRole(_auModel))
            {
                _userService.Delete(id, (Guid)_userRegistrationService.getCurrentUser().UserId);
                return Redirect("~/User/Index");
            }
            else
                return Redirect("~/Home/NoAccess");
        }
        public ActionResult Reset(int id)
        {
            _auModel.Action = Actions.Edit;
            try
            {
                if (_userRegistrationService.CheckRole(_auModel))
                {
                    string message = "Đã cập nhật lại mật khẩu thành " + _userRegistrationService.ResetPassWord(id);
                    return Json(new
                    {
                        IsError = false,
                        Message = message
                    });
                }
                else
                {
                    return Json(new
                    {
                        IsError = true,
                        Message = "Bạn không có quyền thực hiện chức năng này"
                    });
                }
                
            } catch (Exception ex)
            {
                return Json(new
                {
                    IsError = true,
                    Message = ex.ToString()
                });
            }
        }
        public ActionResult Role(Guid id)
        {
            var model = _roleService.getRoleUser(id);
            ViewBag.UserId = id;
            return View(model);
        }
        public ActionResult UpdateRoleUser(bool check, Guid id, Guid roleid)
        {
            if (_roleService.UpdateRoleUser(check, id, roleid, _userRegistrationService.getCurrentUser().UserId))
            {
                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "error" });
            }
        }
        public ActionResult ChangePassWord()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassWord(string PassWordOld, string PassWordNew)
        {
            if (_userRegistrationService.ChangePassWord(User.Identity.Name, PassWordOld, PassWordNew))
                ViewBag.Alert = "Cập nhật thành công";
            else
                ViewBag.Alert = "Cập nhật thất bại";
            return View();
        }
        
        
        //phan get ip va hostname
        protected override void InvokeAction()
        {
            this._userRegistrationService.SetIPClient(GetIPClient());
            this._userRegistrationService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
    }
}
