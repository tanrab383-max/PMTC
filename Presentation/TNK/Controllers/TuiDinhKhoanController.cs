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

namespace TNK.Controllers
{
    public class TuiDinhKhoanController : BasePublicController
    {
        IUserervice _userService;
        ITuiDinhKhoanService _TuiDinhKhoanService;
        IGiaoDichTuiTienService _GiaoDichTuiTienService;
        private ICategoryService _categoryService;
        int total = 0;
        int pageSize = 30;
        HttpContextBase _httpContext;
        public TuiDinhKhoanController(IUserervice _userService
            , ITuiDinhKhoanService _TuiDinhKhoanService
            , ICategoryService _categoryService
            , ISoChiService _soChiService
            , IGiaoDichTuiTienService _GiaoDichTuiTienService
            , HttpContextBase _httpContext
            ) : base()
        {
            this._userService = _userService;
            this._TuiDinhKhoanService = _TuiDinhKhoanService;
            this._categoryService = _categoryService;
            this._GiaoDichTuiTienService = _GiaoDichTuiTienService;
            this._httpContext = _httpContext;
        }

        void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, ref string type)
        {
            var cookie = Request.Cookies.Get(type);
            if (from == "" && to == "")
            {
                from = DateTime.Now.AddDays(-30).ToString(DateFormat);
                to = DateTime.Now.ToString(DateFormat);

                f = Convert.ToDateTime(from + " 00:00:00");
                t = Convert.ToDateTime(to + " 23:59:59");
            }
            else if (from != "" && to != "")
            {
                f = Convert.ToDateTime(from + " 00:00:00");
                t = Convert.ToDateTime(to + " 23:59:59");
                if (cookie == null)
                {
                    cookie = new HttpCookie(type);
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.Domain = FormsAuthentication.CookieDomain;
                    cookie.Secure = FormsAuthentication.RequireSSL;
                    cookie.Path = FormsAuthentication.FormsCookiePath;
                }
                cookie.Values["from"] = from;
                cookie.Values["to"] = to;
                //  cookie.Values["query"] = query;
                //  cookie.Values["type"] = type;
                f = Convert.ToDateTime(from + " 00:00:00");
                t = Convert.ToDateTime(to + " 23:59:59");
                _httpContext.Response.Cookies.Set(cookie);
            }
            if (cookie == null)
            {

                cookie = new HttpCookie(type);
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.Domain = FormsAuthentication.CookieDomain;
                cookie.Secure = FormsAuthentication.RequireSSL;
                cookie.Path = FormsAuthentication.FormsCookiePath;
                cookie.Values["from"] = from;
                cookie.Values["to"] = to;
                //    cookie.Values["query"] = query;
                //    cookie.Values["type"] = type;
                _httpContext.Response.Cookies.Set(cookie);
            }
            else
            {
                from = cookie.Values["from"];
                to = cookie.Values["to"];
                //    query= cookie.Values["query"];
                //    type = cookie.Values["type"];
            }
            ViewBag.Total = total;
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Alert = TempData["Alert"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.MaTui = query;
            ViewBag.LoaiGiaoDich = type;
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&type=" + type + "&from=" + from + "&to=" + to + "&p=";
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
                from = DateTime.Parse("2000-01-01").ToString(DateFormat);
            }
            f = Convert.ToDateTime(from + " 00:00:00");
            if (to == "")
            {
                to = DateTime.Now.ToString(DateFormat);
            }
            t = Convert.ToDateTime(to + " 23:59:59");
        }

        public ActionResult Index()
        {
            ViewBag.Alert = TempData["Alert"];
            var model = _TuiDinhKhoanService.GetTuiDinhKhoanList("");
            var lastSessionId = model.OrderByDescending(x => x.LastSessionId).ToList()[0].LastSessionId;
            ViewBag.LastSessionid = lastSessionId;
            return View("../Category/TuiDinhKhoanIndex", model);
        }
        public object GetModel(string from = "", string to = "", string query = "", string type = "", int p = 1, string maTui = "")
        {
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, ref type);
            switch (type)
            {
                case "TM":
                case "NH":
                    return _GiaoDichTuiTienService.GetDanhSachGiaoDich(query, type, f, t, p, ref total, pageSize);

                case "NOTRA":
                    return _TuiDinhKhoanService.GetListNoPhaiTra(query, f, t, p, ref total, pageSize);

                case "NOTHU":
                    return _TuiDinhKhoanService.GetListNoPhaiThu(query, f, t, p, ref total, pageSize);

                case "KM":
                case "KHO":
                case "KHOPT":
                    return _TuiDinhKhoanService.GetListChiTietXuatNhapKhoPhuTung(query, maTui, f, t, p, ref total, pageSize);

                case "HTD":
                    return _TuiDinhKhoanService.GetListHangTrenDuong(query, f, t, p, ref total, pageSize);

                case "DT":
                case "CO":
                    return _TuiDinhKhoanService.GetListPhieuThu(query, maTui, f, t, p, ref total, pageSize);

            }
            return null;

        }
        public ActionResult ChiTiet(string from = "", string to = "", string query = "", string type = "", int p = 1, string maTui = "")
        {
            DateTime f = new DateTime(), t = new DateTime();
            if (query == "NO_PHTRA"
                || query == "NO_PHTHU"
                || query.ToUpper().Contains("KHO")
                || query == "DTBX"
                 || query == "DTDV"
                 || query == "COCBX"
                 || query == "COCDV"
                 || query == "DTKHAC"
                )
                query = "";
            var model = GetModel(from, to, query, type, p, maTui);
            ViewBag.Type = type;
            ViewBag.MaTui = maTui;
            switch (type)
            {
                case "TM":

                case "NH":
                    return View("../Category/TuiDinhKhoanChiTiet", model);

                case "NOTRA":

                    return View("../Category/ChiTietNoPhaiTra", model);

                case "NOTHU":
                    return View("../Category/ChiTietNoPhaiThu", model);
                case "KM":
                case "KHO":
                case "KHOPT":
                    return View("../Category/ChiTietNhapXuatKhoPT", model);
                case "HTD":
                    return View("../Category/ChiTietPhieuHTD", model);
                case "DT":
                    return View("../Category/ChiTietPhieuDoanhThu", model);
                case "CO":
                    return View("../Category/ChiTietPhieuCoc", model);

            }
            return View();
        }


        // phần trúc làm : thống kê định khoản
        public ActionResult ThongKeDinhKhoan()
        {
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Title = "Thống kê định khoản";
            var model = _TuiDinhKhoanService.ThongKeDinhKhoan();
            double tien = 0, taisan = 0, vay = 0, nv = 0;
            for (int i = 0; i < model.Count; i++)
            {
                if (model[i].MaTui == "TIEN")
                {
                    tien += Convert.ToDouble(model[i].SoTienDangCo);
                }
                else if (model[i].MaTui == "VAY")
                {
                    vay += Convert.ToDouble(model[i].SoTienDangCo);
                }
                else if (model[i].MaTui == "TAISAN")
                {
                    taisan += Convert.ToDouble(model[i].SoTienDangCo);
                }
                else if (model[i].MaTui == "NGUONVON")
                {
                    nv += Convert.ToDouble(model[i].SoTienDangCo);
                }
            }

            //title
            TuiDinhKhoan TIEN = _TuiDinhKhoanService.get_tien();
            TuiDinhKhoan VAY = _TuiDinhKhoanService.get_vay();
            TuiDinhKhoan TAISAN = _TuiDinhKhoanService.get_taisan();
            TuiDinhKhoan NGUONVON = _TuiDinhKhoanService.get_nguonvon();
            ViewBag.TitleTien = TIEN.TenTui;
            ViewBag.TitleVAY = VAY.TenTui;
            ViewBag.TitleTAISAN = TAISAN.TenTui;
            ViewBag.TitleNV = NGUONVON.TenTui;
            ViewBag.Total1 = tien + taisan;
            Int64 Total1 = (Int64)ViewBag.Total1;
            ViewBag.Total1 = Total1;
            ViewBag.Total2 = vay + nv;
            Int64 Total2 = (Int64)ViewBag.Total2;
            ViewBag.Total2 = Total2;
            //tong tien
            ViewBag.Tien = tien;
            ViewBag.Vay = vay;
            ViewBag.Taisan = taisan;
            ViewBag.Nguonvon = nv;
            return View(model);
        }

        public ActionResult GetCategory_tien(string parent = "")
        {
            List<TreeItem> model = new List<TreeItem>();
            TuiDinhKhoan tien = _TuiDinhKhoanService.get_tien();
            if (parent == "#")
            {
                TreeItem item = new TreeItem();
                item.id = "node_0";
                item.text = tien.TenTui;
                item.children = true;
                item.type = "root";
                item.children = true;
                item.icon = "fa fa-folder";
                model.Add(item);
            }
            else if (parent == "node_0")
            {
                parent = "TIEN";
                model = _TuiDinhKhoanService.getByParent_tien(parent).Select(x => new TreeItem
                {
                    id = "node_" + x.MaTui,
                    text = x.TenTui + " ( " + (TNK.Core.Common.ToCurrencyString(x.SoTienDangCo.Value)) + ")",
                    children = _TuiDinhKhoanService.getByParent_tien(x.MaTui).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
            }
            else
            {
                int dem = parent.Length;
                string tukhoa = parent.Substring(5, dem - 5);
                parent = tukhoa;
                model = _TuiDinhKhoanService.getByParent_tien(parent).Select(x => new TreeItem
                {
                    id = "node_" + x.MaTui,
                    text = x.TenTui + " ( " + (TNK.Core.Common.ToCurrencyString(x.SoTienDangCo.Value)) + ")",
                    children = _TuiDinhKhoanService.getByParent_tien(x.MaTui).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategory_vay(string parent = "")
        {
            List<TreeItem> model = new List<TreeItem>();
            TuiDinhKhoan vay = _TuiDinhKhoanService.get_vay();
            if (parent == "#")
            {
                TreeItem item = new TreeItem();
                item.id = "node_1";
                item.text = vay.TenTui;
                item.children = true;
                item.type = "root";
                item.children = true;
                item.icon = "fa fa-folder";
                model.Add(item);
            }
            else if (parent == "node_1")
            {
                parent = "VAY";
                model = _TuiDinhKhoanService.getByParent_vay(parent).Select(x => new TreeItem
                {
                    id = "node_" + x.MaTui,
                    text = x.TenTui + " ( " + (TNK.Core.Common.ToCurrencyString(x.SoTienDangCo.Value)) + ")",
                    children = _TuiDinhKhoanService.getByParent_vay(x.MaTui).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
            }
            else
            {
                int dem = parent.Length;
                string tukhoa = parent.Substring(5, dem - 5);
                parent = tukhoa;
                model = _TuiDinhKhoanService.getByParent_vay(parent).Select(x => new TreeItem
                {
                    id = "node_" + x.MaTui,
                    text = x.TenTui + " ( " + (TNK.Core.Common.ToCurrencyString(x.SoTienDangCo.Value)) + ")",
                    children = _TuiDinhKhoanService.getByParent_vay(x.MaTui).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategory_taisan(string parent = "")
        {
            List<TreeItem> model = new List<TreeItem>();
            TuiDinhKhoan taisan = _TuiDinhKhoanService.get_taisan();
            if (parent == "#")
            {
                TreeItem item = new TreeItem();
                item.id = "node_2";
                item.text = taisan.TenTui;
                item.children = true;
                item.type = "root";
                item.children = true;
                item.icon = "fa fa-folder";
                model.Add(item);
            }
            else if (parent == "node_2")
            {
                parent = "TAISAN";
                model = _TuiDinhKhoanService.getByParent_taisan(parent).Select(x => new TreeItem
                {
                    id = "node_" + x.MaTui,
                    text = x.TenTui + " ( " + (TNK.Core.Common.ToCurrencyString(x.SoTienDangCo.Value)) + ")",
                    children = _TuiDinhKhoanService.getByParent_taisan(x.MaTui).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
            }
            else
            {
                int dem = parent.Length;
                string tukhoa = parent.Substring(5, dem - 5);
                parent = tukhoa;
                model = _TuiDinhKhoanService.getByParent_taisan(parent).Select(x => new TreeItem
                {
                    id = "node_" + x.MaTui,
                    text = x.TenTui + " ( " + (TNK.Core.Common.ToCurrencyString(x.SoTienDangCo.Value)) + ")",
                    children = _TuiDinhKhoanService.getByParent_taisan(x.MaTui).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategory_nguonvon(string parent = "")
        {
            List<TreeItem> model = new List<TreeItem>();
            TuiDinhKhoan nv = _TuiDinhKhoanService.get_nguonvon();
            if (parent == "#")
            {
                TreeItem item = new TreeItem();
                item.id = "node_3";
                item.text = nv.TenTui;
                item.children = true;
                item.type = "root";
                item.children = true;
                item.icon = "fa fa-folder";
                model.Add(item);
            }
            else if (parent == "node_3")
            {
                parent = "NGUONVON";
                model = _TuiDinhKhoanService.getByParent_nguonvon(parent).Select(x => new TreeItem
                {
                    id = "node_" + x.MaTui,
                    text = x.TenTui + " ( " + (TNK.Core.Common.ToCurrencyString(x.SoTienDangCo.Value)) + ")",
                    children = _TuiDinhKhoanService.getByParent_nguonvon(x.MaTui).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
            }
            else
            {
                int dem = parent.Length;
                string tukhoa = parent.Substring(5, dem - 5);
                parent = tukhoa;
                model = _TuiDinhKhoanService.getByParent_nguonvon(parent).Select(x => new TreeItem
                {
                    id = "node_" + x.MaTui,
                    text = x.TenTui + " ( " + (TNK.Core.Common.ToCurrencyString(x.SoTienDangCo.Value)) + ")",
                    children = _TuiDinhKhoanService.getByParent_nguonvon(x.MaTui).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoryItem(string id)
        {
            var model = _categoryService.GetCategoryItem(id);
            return View(model);
        }
        public ActionResult GetCategoryItem_vay(string id)
        {
            var model = _categoryService.GetCategoryItem(id);
            return View(model);
        }
        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
    }
}