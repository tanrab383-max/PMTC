using TNK.Services.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Model;
using TNK.Services.Authentication;
using TNK.Services.Catalog;
using TNK.Services.GopVon;
using TNK.Services.SoThu;
using TNK.Services.Users;
using TNK.Services.SoChi;

namespace TNK.Controllers
{
    public class GopVonController : BasePublicController
    {
        IUserervice _userService;
        IPhieuThuService _phieuThuService;
        ISoThuService _soThuService;
        IDoiTacService _doiTacService;
        IGopVonService _gopVonService;
        IAuthenticationService _authenticationService;
        ISoChiService _soChiService;
        ILogger _log;
        User CurrentUser;
        public GopVonController(IUserervice _userService,
            ISoThuService _soThuService,
            IPhieuThuService _phieuThuService,
            IDoiTacService _doiTacService,
            IGopVonService _gopVonService,
            ISoChiService _soChiService,
        HttpContextBase _httpContext,
        ILogger _log,
            IAuthenticationService _authenticationService) : base()
        {
            this._userService = _userService;
            this._soThuService = _soThuService;
            this._phieuThuService = _phieuThuService;
            this._doiTacService = _doiTacService;
            this._gopVonService = _gopVonService;
            this._httpContext = _httpContext;
            this._soChiService = _soChiService;
            this._log = _log;
            this._authenticationService = _authenticationService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }
        // GET: GopVon
        //public ActionResult Index()
        //{
        //    return View();
        //}
        private string SetDateDefault()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
        public int total = 0;
        public int pageSize = 20;
        public SoThuModel GetModelCreate()
        {
            SoThuModel model = new SoThuModel();
            model.Item = new PhieuThu();
            model.Users = _userService.get();
            model.HTTT = _soThuService.GetHTTT();
            model.CTPT = new List<ChiTietPhieuThu>();
            model.CTPT.Add(new ChiTietPhieuThu());
            return model;
        }

        public SoChiModel GetModelCreateRut()
        {
            SoChiModel model = new SoChiModel();
            model.Item = new PhieuChi();
            model.Users = _userService.get();
            model.HTTT = _soThuService.GetHTTT();
            model.CTPT = new List<ChiTietPhieuChi>();
            model.CTPT.Add(new ChiTietPhieuChi());
            return model;
        }
        public ChiTietGopVonModel GetCTGVon()
        {
            ChiTietGopVonModel model = new ChiTietGopVonModel();
            model.DT = _doiTacService.GetNDT();
            model.DGV = new DotGopVon();
            model.CTGV = new List<ChiTietGopVon>();
            model.CTGV.Add(new ChiTietGopVon());
            return model;
        }
        //
        public void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query,ref string dvgv, string type)
        {

            var cookie = Request.Cookies.Get(type);
            try
            {
                if (from == "" && to == "" && query == "" && dvgv == "")
                {
                    if (cookie == null)
                    {
                        from = DateTime.Now.ToString(DateFormat);
                        to = DateTime.Now.ToString(DateFormat);
                        cookie = new HttpCookie(type);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                        cookie.Values["from"] = from;
                        cookie.Values["to"] = to;
                        cookie.Values["query"] = query;
                        cookie.Values["dvgv"] = dvgv;
                        _httpContext.Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        from = cookie.Values["from"];
                        to = cookie.Values["to"];
                        query = cookie.Values["query"];
                        dvgv = cookie.Values["dvgv"];
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
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                    }
                    cookie.Values["from"] = from;
                    cookie.Values["to"] = to;
                    cookie.Values["query"] = query;
                    cookie.Values["dvgv"] = dvgv;

                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                    _httpContext.Response.Cookies.Set(cookie);
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Warning = TempData["Warning"];
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Info = TempData["Info"];
            ViewBag.Error = TempData["Error"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&p=";
        }
        //phan phieu thu gop von
        public ActionResult PhieuThuGopVonIndex(string from = "", string to = "", string query = "",string dvgv ="", int p = 1)
        {
            DateTime f = new DateTime(), t = new DateTime();
            ViewBag.query = query;
            ViewBag.From = DateTime.Now.ToString(DateFormat);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            ViewBag.DoiTac = _doiTacService.GetNDT();
            SetSearch(ref from, ref to, ref f, ref t, p, ref query,ref dvgv, "PhieuThuGopVon");
            var list = _gopVonService.ListPTGopVon(f,t,query,dvgv,p, ref total, pageSize);
            ViewBag.Title = "Danh sách phiếu thu góp vốn";
            ViewBag.List = list;
            return View();
        }
        // phan danh sach dot gop von
        public ActionResult DSDotGopVon(string from = "", string to = "",string query="", int p = 1)
        {
            DateTime f = new DateTime(), t = new DateTime();
            ViewBag.From = DateTime.Now.ToString(DateFormat);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            SetSearch(ref from, ref to, ref f, ref t, p, ref query,"DSDotGopVon");
            var list = _gopVonService.ListDotGopVon(f, t, query,p, ref total, pageSize);
            ViewBag.Title = "Danh sách đợt góp vốn";
            ViewBag.List = list;
            return View();
        }//phan xem chi tiet gop von
        public ActionResult XemCTGV(string id)
        {
            ChiTietGopVonModel model = new ChiTietGopVonModel();
            model.CTGV = _gopVonService.XemCTGV(id);
            model.DGV = _gopVonService.XemDotGopVon(id);
            model.DT = _doiTacService.GetNDT();
            ViewBag.Date = model.DGV.NgayGopVon.ToString("dd/MM/yyyy");
            return View(model);
        }
        //xoa dot gop von
        [HttpPost]
        public ActionResult DeleteGopVon(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Json(new { IsError = true, Message = "Bạn không có quyền thao tác" });
            }
            string message = _gopVonService.DeleteDotGopVon(id, _authenticationService.GetAuthenticatedUser().UserId);
            if(string.IsNullOrEmpty(message))
            {
                TempData["Alert"] = "Xóa thành công";
                return Json(new { IsError = false, Message = "Đã xóa đợt góp vốn" });
                
            }
            else
            {
                TempData["Alert"] = "Xóa thất bại:" + message;
                return Json(new { IsError = true, Message = message });
            }
        }

        
        public ActionResult PhieuThuGopVonCreate(string id)
        {
            ViewBag.Title = "Phiếu thu góp vốn";
            SoThuModel model = GetModelCreate();
            model.CTBTS = new List<ChiTietBanTaiSan>();
            model.CTBTS.Add(new ChiTietBanTaiSan());
            
            //List<ViewDoiTac> doiTac = _doiTacService.GetNDT();
            List<DotGopVon> dotGopVon = _gopVonService.ListDotGopVon();
            List<ChiTietGopVon> CTGV = _gopVonService.ListCTGV();
            model.Item.MaLoaiPhieu = "TGOVO";
            //phan gan
            DoiTac doiTac = _doiTacService.GetDoiTac(id);
            ViewBag.doiTac = doiTac;
            ViewBag.DotGopVon = dotGopVon;
            ViewBag.Date = SetDateDefault();
            double soTienCanGop = _gopVonService.TongTienGop(id);
            ViewBag.ID = id;
            ViewBag.soTienCanGop = soTienCanGop;
            return View(model);
        }
        [HttpPost]
        public ActionResult PhieuThuGopVonCreate(PhieuThu item, List<ChiTietPhieuThu> httt)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            item.MaLoaiPhieu = "TGOVO";
            string xml = "<Action>DotGopVon</Action>";
            if (item != null)
                xml += TNK.Core.Common.ConvertObjectToXMLString(item);
            if (httt != null)
            {
                xml += Environment.NewLine + "<ThanhToan>";
                foreach (ChiTietPhieuThu ct in httt)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                xml += Environment.NewLine + "</ThanhToan>";
            }
            xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
            TempData["Info"] = "Tạo mới thành công";
            string result = _phieuThuService.CreatePhieuThu(item, httt);
            if (string.IsNullOrEmpty(result))
            {
                _phieuThuService.XuLySauKhiInsertPhieuThu(item.MaPhieuThu, xml);
                TempData["Info"] = "Tạo mới thành công";
                return Redirect("/GopVon/GopVon_Total");
            }
            else
            {
                ViewBag.Error = "Tạo mới thất bại";
                TempData["Error"] = result;
                return Redirect("/GopVon/GopVon_Total");
            }
        }

        public ActionResult PhieuChiRutVonCreate(string id)
        {
            ViewBag.Title = "Phiếu chi rút vốn";
            SoChiModel model = GetModelCreateRut();

            //List<ViewDoiTac> doiTac = _doiTacService.GetNDT();
            List<DotGopVon> dotGopVon = _gopVonService.ListDotRutVon();
            List<ChiTietGopVon> CTGV = _gopVonService.ListCTRV();
            model.Item.MaLoaiPhieu = "CRUVO";
            //phan gan
            DoiTac doiTac = _doiTacService.GetDoiTac(id);
            ViewBag.doiTac = doiTac;
            ViewBag.DotGopVon = dotGopVon;
            ViewBag.Date = SetDateDefault();
            double soTienCanGop = _gopVonService.TongTienRut(id);
            ViewBag.ID = id;
            ViewBag.soTienCanGop = soTienCanGop;
            //phần tính tiền thực góp
            double TongTienThucGop = _phieuThuService.TienThucGop();
            ViewBag.TongTienThucGop = TongTienThucGop;
            return View(model);
        }
        [HttpPost]
        public ActionResult PhieuChiRutVonCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            item.MaLoaiPhieu = "CRUVO";
            string xml = "<Action>DotGopVon</Action>";
            if (item != null)
                xml += TNK.Core.Common.ConvertObjectToXMLString(item);
            if (httt != null)
            {
                xml += Environment.NewLine + "<ThanhToan>";
                foreach (ChiTietPhieuChi ct in httt)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                xml += Environment.NewLine + "</ThanhToan>";
            }
            xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
            TempData["Info"] = "Tạo mới thành công";
            string result = _soChiService.CreatePhieuChi(item, httt);
            if (string.IsNullOrEmpty(result))
            {
                _soChiService.XuLySauKhiInsertPhieuChi(item.MaPhieuChi, xml);
                return Redirect("/GopVon/GopVon_Total");
            }
            else
            {
                ViewBag.Error = result;
                TempData["Error"] = ViewBag.Error;
                return Redirect("/GopVon/GopVon_Total");
            }
        }

        //phan gop von chi tiet

        public ActionResult KhoiTaoGopVon()
        {
            ViewBag.Title = "Khởi tạo góp vốn";
            ChiTietGopVonModel model = GetCTGVon();
            ViewBag.DT = model.DT;
            ViewBag.CTGV = model.CTGV;
            ViewBag.Date = SetDateDefault();
            return View(model);
        }
        public ActionResult TaoGopVon()
        {
            try
            {
                ViewBag.Title = "Khởi tạo góp vốn";
                ChiTietGopVonModel model = GetCTGVon();
                ViewBag.DT = _doiTacService.GetNDT();
                int dem = 0;
                //string id = _gopVonService.DotGVCuoi();
                model.CTGV = _gopVonService.ListCTGV();
                model.LstDGV = _gopVonService.ListDGV();
                if (model.CTGV == null)
                {
                    dem = model.DT.Count;
                    model.CTGV = new List<ChiTietGopVon>();
                }
                else
                {
                    dem = model.DT.Count - model.CTGV.Count;
                }
                if (dem > 0)
                {
                    for (int i = 0; i < dem; i++)
                    {
                        model.CTGV.Add(new ChiTietGopVon());
                    }
                }
                ViewBag.CTGV = model.CTGV;
                ViewBag.Date = SetDateDefault();
                //phan tinh toan tien gop von
                DataTable ListTongHop = new DataTable();
                ListTongHop = _gopVonService.ListTongHop();
                ViewBag.ListTongHop = ListTongHop;
                return View(model);
            }
            catch(Exception ex)
            {
                _log.WriteLog("GopVonController.TaoGopVon:" + ex.Message.ToString());
                return null;
            }
        }
        [HttpPost]
        public ActionResult TaoGopVon(DotGopVon DGV1,List<ChiTietGopVon> CTGV1)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            double total = 0;
            foreach(var item in CTGV1)
            {
                total += item.SoTien;
            }
            if(total == 0)
            {
                return Redirect("/GopVon/GopVon_Total");
            }
            DGV1.LoaiGopVon = "";
            string message = _gopVonService.DGVon_Insert(DGV1, CTGV1);
            if (string.IsNullOrEmpty(message) )
            {
                TempData["Alert"] = "Tạo mới đợt góp vốn thành công";
                return Redirect("/GopVon/GopVon_Total");
            }
            else
            {
                TempData["Alert"] = " Tạo mới đợt góp vốn thất bại:" + message;
            }
            return Redirect("/GopVon/GopVon_Total");
        }

        public ActionResult TaoRutVon()
        {
            try
            {
                ViewBag.Title = "Khởi tạo rút vốn";
                ChiTietGopVonModel model = GetCTGVon();
                ViewBag.DT = _doiTacService.GetNDT();
                int dem = 0;
                //string id = _gopVonService.DotGVCuoi();
                model.CTGV = _gopVonService.ListCTGV();
                model.LstDGV = _gopVonService.ListDGV();
                if (model.CTGV == null)
                {
                    dem = model.DT.Count;
                    model.CTGV = new List<ChiTietGopVon>();
                }
                else
                {
                    dem = model.DT.Count - model.CTGV.Count;
                }
                if (dem > 0)
                {
                    for (int i = 0; i < dem; i++)
                    {
                        model.CTGV.Add(new ChiTietGopVon());
                    }
                }
                ViewBag.CTGV = model.CTGV;
                ViewBag.Date = SetDateDefault();
                //phan tinh toan tien gop von
                DataTable ListTongHop = new DataTable();
                ListTongHop = _gopVonService.ListTongHop();
                ViewBag.ListTongHop = ListTongHop;
                return View(model);
            }
            catch (Exception ex)
            {
                _log.WriteLog("GopVonController.TaoRutVon:" + ex.Message.ToString());
                return null;
            }
        }
        [HttpPost]
        public ActionResult TaoRutVon(DotGopVon DGV1, List<ChiTietGopVon> CTGV1)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            double total = 0;
            foreach (var item in CTGV1)
            {
                total += item.SoTien;
            }
            if (total == 0)
            {
                return Redirect("/GopVon/GopVon_Total");
            }
            DGV1.LoaiGopVon = "R";
            foreach(var item in CTGV1)
            {
                item.LoaiPhieu = "R";
            }
            string message = _gopVonService.DGVon_Insert(DGV1, CTGV1);
            if (string.IsNullOrEmpty(message))
            {
                TempData["Alert"] = "Tạo mới đợt góp vốn thành công";
                return Redirect("/GopVon/GopVon_Total");
            }
            else
            {
                TempData["Alert"] = " Tạo mới đợt góp vốn thất bại:" + message;
            }
            return Redirect("/GopVon/GopVon_Total");
        }

        [HttpPost]
        public bool DeletePTGopVon(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return false;
            }

            string message = _gopVonService.DeletePTGopVon(id);
            if (string.IsNullOrEmpty(message))
            {
                TempData["Alert"] = "Xóa thành công";
                Redirect("/GopVon/PhieuThuGopVonIndex");
                return true;
            }
            else
            {
                TempData["Alert"] = "Xóa thất bại:" + message;
                return false;
            }
        }
        [HttpPost]
        public bool DeletePCRutVon(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return false;
            }
            string message = _gopVonService.DeletePCRutVon(id);
            if (string.IsNullOrEmpty(message))
            {
                TempData["Alert"] = "Xóa thành công";
                Redirect("/GopVon/PhieuThuGopVonIndex");
                return true;
            }
            else
            {
                TempData["Alert"] = "Xóa thất bại:" + message;
                return false;
            }
        }

        //public JsonResult AutoComplete()
        //{
        //    List<ViewDoiTac> doiTac = _doiTacService.GetNDT();
        //    var list = doiTac.Select(x => x.TenDoiTac).ToList();
        //    return Json(list,JsonRequestBehavior.AllowGet);
        //}

        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
        #region phần về quản lý góp vốn
        public ActionResult GopVon_Total()
        {
            ViewBag.DoiTac = _doiTacService.GetNDT();
            ChiTietGopVonModel model = new ChiTietGopVonModel();
            model.DT = _doiTacService.GetNDT();
            if(model.DT.Count <= 0)
            {
                ViewBag.Alert = "Vui lòng nhập nhà đầu tư";
                return View();
            }
            model.CTGV = new List<ChiTietGopVon>();
            if(model.CTGV.Count == 0 && model.CTGV.Count < model.DT.Count)
            {
                for(int i = 0; i < model.DT.Count; i++)
                {
                    model.CTGV.Add(new ChiTietGopVon());
                }
            }
            
            for(int i = 0; i < model.CTGV.Count; i++)
            {
                model.CTGV[i].MaDoiTac = model.DT[i].madoitac;
            }
            if (model.DGV == null)
            {
                model.DGV = new DotGopVon();
                model.DGV.NgayGopVon = DateTime.Now;
            }
            DataTable ListTongHop = new DataTable();
            ListTongHop = _gopVonService.ListTongHop();
            ViewBag.ListTongHop = ListTongHop;
            ViewBag.count = ListTongHop.Rows.Count;
            if (TempData["Error"] != null )
            {
                ViewBag.Error = TempData["Error"];
                TempData["Error"] = null;
            }
            return View(model);
        }
        [HttpPost]
        public string HistoryGV(string from = "", string to = "",int tab = 1)
        {
            DateTime From = new DateTime();
            DateTime To = new DateTime();
            if (from == "" && to == "")
            {
                From = DateTime.Now;
                To = DateTime.Now;
            }
            else
            {
                From = Convert.ToDateTime(from);
                To = Convert.ToDateTime(to);
            }
            /*if (From.ToShortDateString() == DateTime.Now.ToShortDateString())
                From = Convert.ToDateTime("01/01/2014");
            */

            var list = _gopVonService.ListDotGopVon(From,To,"");
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(list);
            return JSONString;
        }

        [HttpPost]
        public string HistoryRutVon(string from = "", string to = "", int tab = 1)
        {
            DateTime From = new DateTime();
            DateTime To = new DateTime();
            if (from == "" && to == "")
            {
                From = DateTime.Now;
                To = DateTime.Now;
            }
            else
            {
                From = Convert.ToDateTime(from);
                To = Convert.ToDateTime(to);
            }
            if (From.ToShortDateString() == DateTime.Now.ToShortDateString())
                From = Convert.ToDateTime("01/01/2014");
            var list = _gopVonService.ListDotGopVon(From, To,"R");
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(list);
            return JSONString;
        }
        [HttpPost]
        public string DSThuGV(string from, string to,string doitac,string query)
        {
            DateTime From = Convert.ToDateTime(from);
            DateTime To = Convert.ToDateTime(to);
            var DT = _gopVonService.ListPTGopVon(From, To,doitac, query,"G");
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(DT);
            return JSONString;
        }

        [HttpPost]
        public string DSRutVon(string from, string to, string doitac, string query)
        {
            DateTime From = Convert.ToDateTime(from);
            DateTime To = Convert.ToDateTime(to);
            var DT = _gopVonService.ListPTGopVon(From, To, doitac, query,"R");
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(DT);
            return JSONString;
        }
        #endregion
    }
}