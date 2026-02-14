using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TNK.Core.Domain;
using TNK.Model;
using TNK.Services.Authentication;
using TNK.Services.NhanVien;

namespace TNK.Controllers
{
    public class NhanVienController : BasePublicController
    {
        int total = 0;
        int pageSize = 30;
        private  INhanVienService _nhanVienService;
        private  HttpContextBase _httpContext;
        private  IAuthenticationService _authenticationService;
        public NhanVienController(INhanVienService _nhanVienService, 
            HttpContextBase _httpContext, 
            IAuthenticationService _authenticationService) : base()
        {
            this._nhanVienService = _nhanVienService;
            this._httpContext = _httpContext;
            this._authenticationService = _authenticationService;
        }

        void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string type,int pageSize=30)
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
        // GET: NhanVien
        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1,int pageSize=30)
        {
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Title = "Nhân Viên";
            ViewBag.LinkCreate = "NhanVien/NhanVienCreate";
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "NhanVien", pageSize);
            var list = _nhanVienService.GetNhanVien(query, p, ref total, pageSize);
            ViewBag.total = total;
            return View("NhanVienIndex", list);
        }
        public ActionResult NhanVienCreate()
        {
            NhanVienModel model = new NhanVienModel();
            model.Item = new NhanVien();
            ViewBag.TieuDe = "Thêm mới nhân viên";
            return View("NhanVienCreate",model);
        }

        [HttpPost]
        public ActionResult NhanVienCreate(NhanVien item)
        {
            ViewBag.Title = "Thêm Mới Nhân Viên";
            bool result = _nhanVienService.CreateNhanVien(item);
            if(result == true)
            {
                TempData["Alert"] = "Tạo mới nhân viên thành công";
                return Redirect("Index");
            }
            else
            {
                ViewBag.Alert = "Thất bại:" + ViewBag.Title;
            }
            return null;
        }

        public ActionResult NhanVienUpdate(Guid Id)
        {
            ViewBag.Alert = TempData["Alert"];
            NhanVien model = _nhanVienService.GetNhanVien(Id);
            if(model == null)
                ViewBag.Alert = "Mã nhân viên không đúng";
            return View("NhanVienUpdate", model);
        }
        [HttpPost]
        public ActionResult NhanVienUpdate(NhanVien model)
        {
            ViewBag.TieuDe = "Cập nhật nhân viên";
            if (_nhanVienService.UpdateNhanVien(model))
            {
                ViewBag.Alert = "Cập nhật thành công";
                TempData["Alert"] = "Cập nhật thành công";
                return Redirect("~/NhanVien/Index");
            }
            else
            {
                ViewBag.Alert = "Cập nhật thất bại";
            }
            return null;
        }
        [HttpPost]
        public bool DeleteNhanVien(Guid Id)
        {
            if (_nhanVienService.DeleteNhanVien(Id))
            {
                TempData["Alert"] = "Xóa thành công";
                Redirect("/NhanVien/Index");
                return true;
            }
            else
            {
                TempData["Alert"] = "Xóa thất bại";
                return false;
            }
            
        }
    }
}