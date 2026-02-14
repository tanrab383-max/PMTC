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
using System.Web;
using System.Web.Security;
using TNK.Services.KhoXe;
using TNK.Core.Domain.View;
using TNK.Services.Authentication;

namespace TNK.Controllers
{
    public class CMXTTController : BasePublicController
    {
        public string LoaiPhieu = "CMXTT";
        public string ViewIndexName { get { return "../SoChi/" + LoaiPhieu + "Index"; } }
        public string ViewCreateName { get { return "../SoChi/" + LoaiPhieu + "Create"; } }
        public string ViewEditName { get { return "../SoChi/" + LoaiPhieu + "Edit"; } }
        public string ViewDeleteName { get { return "../SoChi/" + LoaiPhieu + "Edit"; } }
        public string UrlIndex { get { return "/" + LoaiPhieu + "/Index"; } }
        HttpContextBase _httpContext;
        IUserervice _userService;
        ICategoryService _categoryService;
        ISoChiService _soChiService;
        ISoThuService _soThuService;
        ILoaiXeService _loaiXeService;
        IKhoXeService _khoXeService;
        IAuthenticationService _authenticationService;
        User CurrentUser;
        int total = 0;
        int pageSize = 30;
        public CMXTTController(IUserervice _userService
            , ICategoryService _categoryService
            , ISoChiService _soChiService
            , ISoThuService _soThuService
            , ILoaiXeService _loaiXeService
            , IKhoXeService _khoXeService
            , HttpContextBase _httpContext
            , IAuthenticationService _authenticationService
            ) : base()
        {
            this._userService = _userService;
            this._categoryService = _categoryService;
            this._soChiService = _soChiService;
            this._soThuService = _soThuService;
            this._loaiXeService = _loaiXeService;
            this._khoXeService = _khoXeService;
            this._httpContext = _httpContext;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }

        void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string type)
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
                _httpContext.Response.Cookies.Set(cookie);
            }
            ViewBag.Total = total;
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
           // ViewBag.Alert = TempData["Alert"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&p=";
        }

        public SoChiModel GetModel(string code)
        {
            SoChiModel model = new SoChiModel();
            model.LoaiPhieu = LoaiPhieu;
            switch (code)
            {
                case "CMHTD":
                    model.DoiTac = _soChiService.GetDoiTac("NCC");
                    break;
                case "CMXTT":
                    model.DoiTac = _soChiService.GetDoiTac("NCC");
                    model.MaXe = _loaiXeService.GetViewLoaiXeList();
                    break;
            }
            model.Users = _userService.get();
            model.HTTT = _soChiService.GetHTTT();

            return model;
        }

        #region 
        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1)
        {
            //ViewBag.Alert = TempData["Alert"];
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, LoaiPhieu);           
            var model = _soChiService.GetPhieuChiList(LoaiPhieu,f, t, query, p, ref total, pageSize);
            return View(ViewIndexName, model);
        }
        public ActionResult Create()
        {
            ViewBag.Title = "Tạo phiếu";
            SoChiModel model = GetModel(LoaiPhieu);
            model.CTPT = new List<ChiTietPhieuChi>();
            model.CTPT.Add(new ChiTietPhieuChi());
            return View(ViewCreateName, model);
        }
        [HttpPost]
        public ActionResult Create(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Title = "Tạo phiếu ";
            item.MaLoaiPhieu = LoaiPhieu;
            List<ViewKhoXe> lstKhoXe = _khoXeService.GetDanhSachXe();

            if (lstKhoXe.Any(x => x.SoKhung == item.SoKhung))
                ViewBag.Alert = "Xe đã có trong kho";
            string message = _soChiService.CreatePhieuChi(item, httt);
            if (message =="")
            {
                TempData["Info"] = "Tạo mới thành công";
                return Redirect(UrlIndex);
            }
            else
            {
                TempData["Info"] = "Tạo mới thất bại:" + message;
                ViewBag.Error = "Tạo mới thất bại:" + message;
            }
                
            SoChiModel model = GetModel(LoaiPhieu);
            model.Item = item;
            model.CTPT = httt;
            return View(ViewCreateName, model);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật phiếu mua ";
            SoChiModel model = GetModel(LoaiPhieu);
            model.Item = _soChiService.GetPhieuChi(id);
            model.LoaiPhieu = LoaiPhieu;
            if (model.Item == null || model.Item.MaLoaiPhieu != LoaiPhieu)
            {
                return Redirect(UrlIndex);
            }
            model.CTPT = _soChiService.GetCTPC(id);
            if (model.CTPT.Count == 0)
            {
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
            }
            return View(ViewEditName, model);
        }
        [HttpPost]
        public ActionResult Edit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            //ViewBag.Title = "Cập nhật phiếu";
            //if (_soChiService.UpdatePhieuChi(item, httt))
            //{
            //    ViewBag.Alert = "Cập nhật thành công";
            //   // return Redirect(UrlIndex);
            //}
            //else
            //    ViewBag.Alert = "Cập nhật thất bại";
            SoChiModel model = GetModel(LoaiPhieu);
            model.LoaiPhieu = LoaiPhieu;
            model.Item = item;
            model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
            if (model.CTPT.Count == 0)
            {
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
            }
            return Redirect(UrlIndex);
            return View(ViewEditName, model);
        }
        public ActionResult Delete(string id)
        {
            //if (_soChiService.DeletePhieuChi(id))
            //    TempData["Alert"] = "Xóa thành công";
            //else
            //    TempData["Alert"] = "Xóa thất bại";
            return Redirect(UrlIndex);
        }

        #endregion
        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
    }
}