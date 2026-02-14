using System.Collections.Generic;
using System.Web.Mvc;
using TNK.Core.Domain;
using TNK.Model;
using TNK.Services.Catalog;
using TNK.Services.SoThu;
using TNK.Services.Users;
using System.Linq;
using System;
using TNK.Services.SoChi;
using TNK.Services.KhoXe;
using System.Web.Security;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using TNK.Services.Authentication;

namespace TNK.Controllers
{
    public class KhoPhuTungController : BasePublicController
    {
       
        int total = 0;
        int pageSize = 30;

        IUserervice _userService;
        ICategoryService _categoryService;
        IKhoPhuTungService _khoService;
        HttpContextBase _httpContext;
        
        ISoChiService _soChiService;
        IAuthenticationService _authenticationService;
        User CurrentUser;

        public KhoPhuTungController(IUserervice _userService
            , ICategoryService _categoryService
            , IKhoPhuTungService _khoService          
            , ISoChiService _soChiService
            , IAuthenticationService _authenticationService
               , HttpContextBase _httpContext
            ) : base()
        {
            this._userService = _userService;
            this._categoryService = _categoryService;
            this._khoService = _khoService;
          
            this._soChiService = _soChiService;
            this._httpContext = _httpContext;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }

        void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query,string type)
        {
           
            var cookie = Request.Cookies.Get(type);
            if (from == "" && to == "" && query == "")
            {
                if (cookie == null)
                {
                    from = DateTime.Now.ToString(DateFormat);
                    to = DateTime.Now.ToString(DateFormat);
                    cookie = new HttpCookie(type);
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.Domain = FormsAuthentication.CookieDomain;
                    cookie.Secure = FormsAuthentication.RequireSSL;
                    cookie.Path = FormsAuthentication.FormsCookiePath;
                    cookie.Values["from"] = from;
                    cookie.Values["to"] = to;
                    cookie.Values["query"] = query;
                    _httpContext.Response.Cookies.Set(cookie);
                }
                else
                {
                    from = cookie.Values["from"];
                    to = cookie.Values["to"];
                    query = cookie.Values["query"];
                }
                f = Convert.ToDateTime(from + " 00:00:00");
                t = Convert.ToDateTime(to + " 23:59:59");
            }
            else if (from != "" && to != "")
            {
                if (cookie == null)
                {
                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                    cookie = new HttpCookie(type);
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.Domain = FormsAuthentication.CookieDomain;
                    cookie.Secure = FormsAuthentication.RequireSSL;
                    cookie.Path = FormsAuthentication.FormsCookiePath;
                }
                cookie.Values["from"] = from;
                cookie.Values["to"] = to;
                cookie.Values["query"] = query;
                f = Convert.ToDateTime(from + " 00:00:00");
                t = Convert.ToDateTime(to + " 23:59:59");
                HttpContext.Response.Cookies.Set(cookie);
            }
            ViewBag.Total = total;
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Alert = TempData["Alert"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&p=";
        }

        void SetSearch(int p, string from, string to, string query)
        {
            ViewBag.Total = total;
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Alert = TempData["Alert"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Url = Request.Url.AbsolutePath + "?from=" + from + "&to=" + to + "&p=";
        }

        void SetDate(ref string from, ref string to, ref DateTime f, ref DateTime t)
        {
            if (from == "")
            {
                from = DateTime.Now.ToString(DateFormat);
            }
            f = Convert.ToDateTime(from + " 00:00:00");
            if (to == "")
            {
                to = DateTime.Now.ToString(DateFormat);
            }
            t = Convert.ToDateTime(to + " 23:59:59");
        }

        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1)
        {
            //ViewBag.Alert = TempData["Alert"];
            ViewBag.Title = "Kho phụ tùng";
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "KhoPhuTung");         
            var model = _khoService.GetKhoPhuTung( query, p, ref total, pageSize);
            return View("KhoPhuTungIndex",model);
        }

        public ActionResult Edit(string id)
        {
            //ViewBag.Alert = TempData["Alert"];
            ViewBag.Title = "Điều chỉnh kho phụ tùng";

            Guid rs = Guid.Empty;

            KhoPhuTung model = _khoService.GetKhoPhuTung(id);
            model.TongTien = Math.Round(model.TongTien.Value);
            if (model == null)
                ViewBag.Alert = "Mã kho không đúng";

            return View("KhoPhuTungEdit", model);
        }
        [HttpPost]
        public ActionResult Edit(KhoPhuTung item)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_khoService.UpdateKhoPhuTung(item) )
            {
                ViewBag.Info = "Cập nhật thành công";
                TempData["Info"] = "Cập nhật thành công";
                return Redirect("/KhoPhuTung/Index");
            }
            else
            {
                ViewBag.Error = "Cập nhật thất bại";
            }
           
            return View(item);
        }

        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
        [HttpPost]
        public ActionResult EditKPT(string MaKho, string NgayDC,double SoTien)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Json(new {IsError = true,Message = "Bạn không có quyền trên chức năng này" });
            }
            DieuChinhKhoPhuTung obj = new DieuChinhKhoPhuTung();
            obj.MaKho = MaKho;
            obj.NgayDieuChinh = Convert.ToDateTime(NgayDC);
            obj.SoTienDieuChinh = SoTien;
            string message = _khoService.EditKhoPhuTung(obj);
            if (string.IsNullOrEmpty(message))
            {
                TempData["Info"] = "Điều chỉnh kho thành công";
                return Json(new{IsError = false,Message ="" });
            }
            else
            {
                TempData["Error"] = "Lỗi điều chỉnh kho :" + message;
                return Json(new { IsError = true, Message = message });
            }
        }
        [HttpPost]
        public string DanhSachKPT()
        {
            var item = _khoService.GetList();
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(item);
            return JSONString;
        }
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Json(new { IsError = false, Meeage = "Bạn không có quyền xóa" });
            }
            string message = _khoService.DeleteDieuChinhKho(Id);
            if (string.IsNullOrEmpty(message))
                return Json(new { IsError = false, Message = "" });
            else
                return Json(new { IsError = true, Message = message });
        }
    }
}