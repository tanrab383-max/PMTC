using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TNK.Services.Authentication;
using TNK.Services.LichSuThaoTac;
using TNK.Model;
using TNK.Services.Users;
using TNK.Core;
using System.Data;
using TNK.Core.Domain;
using TNK.Services.Catalog;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace TNK.Controllers
{
    public class LogController : BasePublicController
    {
        public int total = 0;
        public int pageSize = 30;
        ILichSuThaoTacService _LichSuThaoTacService;
        //HttpContextBase _httpContext;
        IAuthenticationService _authenticationService;
        IUserervice _userService;
        ILoggingService _LoggingService;
        ITuiDinhKhoanService _TuiDinhKhoanService;
        public LogController(IUserervice _userService,
            ILichSuThaoTacService _LichSuThaoTacService,
            ILoggingService _LoggingService,
            HttpContextBase _httpContext,
            ITuiDinhKhoanService _TuiDinhKhoanService,
            IAuthenticationService _authenticationService) : base()
        {
            this._userService = _userService;
            this._LichSuThaoTacService = _LichSuThaoTacService;
            this._LoggingService = _LoggingService;
            this._httpContext = _httpContext;
            this._authenticationService = _authenticationService;
            this._TuiDinhKhoanService = _TuiDinhKhoanService;
        }
        void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string type, string action, int pageSize = 30)
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
                    cookie.Values["action"] = action;
                    _httpContext.Response.Cookies.Set(cookie);
                }
                else
                {
                    from = cookie.Values["from"];
                    to = cookie.Values["to"];
                    query = cookie.Values["query"];
                    action = cookie.Values["action"];
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
                cookie.Values["action"] = action;
                f = Convert.ToDateTime(from + " 00:00:00");
                t = Convert.ToDateTime(to + " 23:59:59");
                HttpContext.Response.Cookies.Set(cookie);
            }
            else if (from == "" && to == "" && query != "")
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
                if (cookie != null)
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
                cookie.Values["action"] = action;
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
        // GET: Log
        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1, string userid = "", string HanhDong = "", int pageSize = 30)
        {
            LichSuThaoTacModel model = new LichSuThaoTacModel();
            Guid UserId = new Guid();
            if (userid == "")
            {
                userid = "00000000-0000-0000-0000-000000000000";
            }
            UserId = Guid.Parse(userid);
            DateTime f = new DateTime();
            DateTime t = new DateTime();
            ViewBag.Title = "Lịch sử tác động";
            ViewBag.From = DateTime.Now.ToString(DateFormat);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            ViewBag.UserId = UserId;
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "LichSuThaoTac", HanhDong, pageSize);
            var list = _LichSuThaoTacService.GetLichSuThaoTac(UserId, f, t, query, ref total, pageSize, p, HanhDong);
            ViewBag.Total = total;
            model.Users = _userService.get();
            ViewBag.Users = model.Users;
            return View(list);
        }

        public ActionResult TuiTienIndex(string from = "", string to = "", string query = "", int p = 1, string MaTui = "")
        {
            LichSuThaoTacModel model = new LichSuThaoTacModel();
            model.TuiDinhKhoan = _TuiDinhKhoanService.ThongKeDinhKhoan();
            ViewBag.MaTuiTien = model.TuiDinhKhoan;
            DateTime f = new DateTime();
            DateTime t = new DateTime();
            ViewBag.Title = "Lịch sử túi tiền";
            ViewBag.From = DateTime.Now.ToString(DateFormat);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "LichSuTuiTien", "");
            var getLSTDTT = _LichSuThaoTacService.getLichSuThayDoiTuiTien(f, t, MaTui);
            var list = _LichSuThaoTacService.LichSuThayDoiTuiTien(f, t, MaTui, p);
            ViewBag.List = list;
            ViewBag.Total = getLSTDTT.Rows.Count;
            ViewBag.getLSTDTT = getLSTDTT;
            return View();
        }


        public ActionResult ChiTiet(string from = "", string to = "", string query = "", int p = 1)
        {
            DateTime f = new DateTime(), t = new DateTime();
            f = Convert.ToDateTime(from);
            if (to == "")
            {
                t = DateTime.Now;
            }
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "ChiTiet");
            var model = _LichSuThaoTacService.getListLichSuTuiDinhKhoan(f, t, query, p);
            ViewBag.Query = query;
            ViewBag.From = from.Substring(0, 10);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            return View(model);
        }
        public ActionResult Lichsu(string from = "", string to = "", string maTui = "", int p = 1)
        {
            DateTime f = new DateTime(), t = new DateTime();
            f = Convert.ToDateTime(from);
            if (to == "")
            {
                t = DateTime.Now;
            }
            SetSearch(ref from, ref to, ref f, ref t, p, ref maTui, "LichSu");
            var model = _LichSuThaoTacService.getLichSuTuiTien(f, t, maTui, p);
            ViewBag.Query = maTui;
            ViewBag.From = from.Substring(0, 10);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="query"></param>
        /// <param name="showSystemLog">none: neu khong hien log xu ly store</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult History(string from = "", string to = "", string query = "", string showSystemLog = "", int page = 1)
        {
            DateTime f = new DateTime(), t = new DateTime();
            f = Convert.ToDateTime(from);
            if (to == "")
            {
                t = DateTime.Now;
            }
            SetSearch(ref from, ref to, ref f, ref t, page, ref query, "History");
            var dataHistory = _LichSuThaoTacService.getHistoryChange(f, t, query, page);
            ViewBag.DataHistory = "";
            ViewBag.SubmitData = "";
            foreach (DataHistory obj in dataHistory)
            {
                ViewBag.DataHistory += Environment.NewLine + Common.DataTableToHtmlHorizontal(Common.LoadDataSetFromXMLString("<table>" + obj.XmlValue + "</table>").Tables[0], obj.TableName);
            }

            var lichSuThaoTac = _LichSuThaoTacService.GetLichSuThaoTac(query);
            if (lichSuThaoTac.Count > 0)
            {
                DataSet ds = Common.LoadDataSetFromXMLString(lichSuThaoTac[0].GhiChu);
                foreach (DataTable tbl in ds.Tables)
                    ViewBag.SubmitData += Environment.NewLine + Common.DataTableToHtmlHorizontal(tbl, tbl.TableName);
                ViewBag.StoreLog = lichSuThaoTac[0].ChiTiet;
            }

            var tblThayDoiTuiTien = _LichSuThaoTacService.GetThayDoiTuiTtien(query);
            ViewBag.TuiTien = Common.DataTableToHtmlVertical(tblThayDoiTuiTien, tblThayDoiTuiTien.TableName);

            ViewBag.Title = "Lịch sử thay đổi";
            ViewBag.Query = query;
            ViewBag.From = from.Substring(0, 10);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            //co hien thong tin xu ly store khong. Chi hien khi xuat trong bao cao lich su het thong
            //khi hien lich su tren phieu thi ko can hien 
            ViewBag.ShowSystemLog = showSystemLog;

            return View("DataHistory");
        }

        //phần logging, phần theo dõi lỗi của TNK
        public ActionResult Logging_Index(string from = "", string query = "", string to = "", int p = 1, string userid = "")
        {
            LoggingModel model = new LoggingModel();
            Guid UserId = new Guid();
            if (userid == "")
            {
                userid = "CAB45478-B777-4F9C-8FAD-19F68F417FAB";
            }
            UserId = Guid.Parse(userid);
            DateTime f = new DateTime();
            DateTime t = new DateTime();
            ViewBag.Title = "Lịch sử lỗi";
            ViewBag.From = DateTime.Now.ToString(DateFormat);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            ViewBag.UserId = UserId;
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "Logging");
            var list = _LoggingService.getLogging(UserId, f, t, query, ref total, pageSize, p);
            ViewBag.Total = total;
            model.Users = _userService.get();
            ViewBag.Users = model.Users;
            ViewBag.List = list;
            return View();
        }
        public ActionResult History_Login(string from = "", string query = "", string to = "", int p = 1, string userid = "", int pageSize = 30)
        {
            LoggingModel model = new LoggingModel();
            Guid UserId = new Guid();
            if (userid == "")
            {
                userid = "00000000-0000-0000-0000-000000000000";
            }
            UserId = Guid.Parse(userid);
            DateTime f = new DateTime();
            DateTime t = new DateTime();
            ViewBag.Title = "Lịch sử đăng nhập";
            ViewBag.From = DateTime.Now.ToString(DateFormat);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            ViewBag.UserId = UserId;
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "Login", pageSize);
            var list = _LoggingService.getLogin(UserId, f, t, query, ref total, pageSize, p);
            ViewBag.Total = total;
            model.Users = _userService.get();
            ViewBag.Users = model.Users;
            return View(list);
        }
        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
        DataTable dtb = new DataTable();
        public ActionResult LichSuDuLieu(string from = "", string query = "", string to = "", int p = 1, string userid = "", int pageSize = 30)
        {
            LoggingModel model = new LoggingModel();
            Guid UserId = new Guid();
            if (userid == "")
            {
                userid = "00000000-0000-0000-0000-000000000000";
            }
            UserId = Guid.Parse(userid);
            DateTime f = new DateTime();
            DateTime t = new DateTime();
            ViewBag.Title = "Lịch sử dữ liệu";
            ViewBag.From = DateTime.Now.ToString(DateFormat);
            ViewBag.To = DateTime.Now.ToString(DateFormat);
            ViewBag.UserId = UserId;
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "DataHistory", pageSize);
            var list = _LoggingService.getDataHistory(UserId, f, t, query, ref total, pageSize, p);
            dtb = list;
            ViewBag.Total = total;
            model.Users = _userService.get();
            ViewBag.Users = model.Users;
            ViewBag.List = list;
            return View();
        }
        //public ActionResult ChiTietLichSuDuLieu(string from = "", string to = "", string query = "", string Id = "", string SessionId = "", string Table = "", int page = 1)
        //{
        //    string xml1 = "";
        //    string xml2 = "";
        //    if (dtb.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dtb.Rows)
        //        {
        //            if (dr["Id"].ToString() == Id)
        //            {
        //                xml1 = dr["XmlValue"].ToString();
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        DataTable dtb_old = _LoggingService.getHistory_New(Id);
        //        xml1 = dtb_old.Rows[0]["XmlValue"].ToString();
        //    }
        //    DataTable dtb_History = _LoggingService.getHistory(SessionId, Table);
        //    xml2 = dtb_History.Rows[0]["XmlValue"].ToString();
        //    //convert xml to datatable
        //    XmlDocument doc1 = new XmlDocument();
        //    xml1 = "<row><Id>AFFEBB57-66DC-4E85-B414-0D77572321BF</Id><MaPhieuThu>TPHKI1808250004</MaPhieuThu><HinhThucThanhToan>DC</HinhThucThanhToan><SoTienThanhToan>0</SoTienThanhToan><MaNganHang>COCBX</MaNganHang><SoThamChieu>TCOBX1808090003</SoThamChieu><TinhTrang>0</TinhTrang><IsActive>1</IsActive><IsDeleted>0</IsDeleted><CreatedBy>016F74F0-F8C7-4C66-9FC1-83D4385F65D2</CreatedBy><CreatedDate>2018-08-27T15:54:33.427</CreatedDate><UpdatedBy>016F74F0-F8C7-4C66-9FC1-83D4385F65D2</UpdatedBy><UpdatedDate>2018-08-27T15:54:33.427</UpdatedDate></row>";
        //    doc1.LoadXml(xml1);
        //    var json1 = JsonConvert.SerializeXmlNode(doc1, Newtonsoft.Json.Formatting.None, true);
        //    if (xml2 != "")
        //    {
        //        XmlDocument doc2 = new XmlDocument();
        //        xml2 = "<row><Id>AFFEBB57-66DC-4E85-B414-0D77572321BF</Id><MaPhieuThu>TPHKI1808250004</MaPhieuThu><HinhThucThanhToan>DC</HinhThucThanhToan><SoTienThanhToan>0</SoTienThanhToan><MaNganHang>COCBX</MaNganHang><SoThamChieu>TCOBX1808090003</SoThamChieu><TinhTrang>0</TinhTrang><IsActive>1</IsActive><IsDeleted>0</IsDeleted><CreatedBy>016F74F0-F8C7-4C66-9FC1-83D4385F65D2</CreatedBy><CreatedDate>2018-08-27T15:54:33.427</CreatedDate><UpdatedBy>016F74F0-F8C7-4C66-9FC1-83D4385F65D2</UpdatedBy><UpdatedDate>2018-08-27T15:54:33.427</UpdatedDate></row>";
        //        doc2.LoadXml(xml2);
        //        var json2 = JsonConvert.SerializeXmlNode(doc2, Newtonsoft.Json.Formatting.None, true);
        //        ViewBag.Json2 = json2;
        //    }
        //    else
        //        ViewBag.Json2 = null;
        //    return View(json1);
        //}

        public ActionResult ChiTietLichSuDuLieu(string from = "", string to = "", string query = "", string Id = "", string SessionId = "", string Table = "", int page = 1)
        {
            string xml1 = "";
            string xml2 = "";
            DataTable dtb_old = new DataTable();
            if (dtb.Rows.Count > 0)
            {
                foreach (DataRow dr in dtb.Rows)
                {
                    if (dr["Id"].ToString() == Id)
                    {
                        xml1 = dr["XmlValue"].ToString();
                        break;
                    }
                }
            }
            else
            {
                dtb_old = _LoggingService.getHistory_New(Id);
                xml1 = dtb_old.Rows[0]["XmlValue"].ToString();
            }
            string MaLoaiPhieu = dtb_old.Rows[0]["MaPhieu"].ToString().Substring(0, 5);
            DataTable dtb_History = _LoggingService.getHistory(SessionId, Table, MaLoaiPhieu);
            xml2 = dtb_History.Rows[0]["XmlValue"].ToString();
            //convert xml to datatable
            xml1 = xml1.Insert(0, "<total>");
            xml1 = xml1.Insert(xml1.Length, "</total>");
            xml2 = xml2.Insert(0, "<total>");
            xml2 = xml2.Insert(xml2.Length, "</total>");
            StringReader theReader = new StringReader(xml1);
            DataSet theDataSet = new DataSet();
            theDataSet.ReadXml(theReader);
            DataTable d1 = theDataSet.Tables[0];
            ViewBag.Colunm1 = theDataSet.Tables[0].Columns.Count;
            ViewBag.D1 = d1;
            string data1 = "";
            for (int i = 0; i < dtb_old.Rows.Count; i++)
            {
                data1 += Environment.NewLine + Common.DataTableToHtmlHorizontal(Common.LoadDataSetFromXMLString("<table>" + dtb_old.Rows[i]["XmlValue"] + "</table>").Tables[0], dtb_old.Rows[i]["TableName"].ToString());
            }
            data1 = data1.ToString().Replace("<table class =''>", "<table class ='table'>");
            ViewBag.Data1 = data1;
            //if (d1.Rows.Count > 0)
            //{
            //    foreach (DataTable tbl in theDataSet.Tables)
            //        ViewBag.D1 += Environment.NewLine + Common.DataTableToHtmlHorizontal(tbl, tbl.TableName);
            //}
            //D1
            List<String> lsColumns1 = new List<string>();

            if (d1.Rows.Count > 0)
            {
                var count = d1.Rows[0].Table.Columns.Count;

                for (int i = 0; i < count; i++)
                {
                    lsColumns1.Add(Convert.ToString(d1.Columns[i].ColumnName.ToString()));
                }
            }
            ViewBag.lsColumns1 = lsColumns1;
            //D2

            StringReader theReader2 = new StringReader(xml2);
            DataSet theDataSet2 = new DataSet();
            theDataSet2.ReadXml(theReader2);
            DataTable d2 = theDataSet2.Tables[0];
            ViewBag.D2 = d2;
            //if (d2.Rows.Count > 0)
            //{
            //    foreach (DataTable tbl in theDataSet2.Tables)
            //        ViewBag.D2 += Environment.NewLine + Common.DataTableToHtmlHorizontal(tbl, tbl.TableName);
            //}
            List<String> lsColumns2 = new List<string>();

            if (d2.Rows.Count > 0)
            {
                var count = d2.Rows[0].Table.Columns.Count;

                for (int i = 0; i < count; i++)
                {
                    lsColumns2.Add(Convert.ToString(d2.Columns[i].ColumnName.ToString()));
                }
            }
            ViewBag.lsColumns2 = lsColumns2;
            ViewBag.Coulunm2 = theDataSet2.Tables[0].Columns.Count;
            string data2 = "";
            for (int i = 0; i < dtb_History.Rows.Count; i++)
            {
                data2 += Environment.NewLine + Common.DataTableToHtmlHorizontal(Common.LoadDataSetFromXMLString("<table>" + dtb_History.Rows[i]["XmlValue"] + "</table>").Tables[0], dtb_History.Rows[i]["TableName"].ToString());
            }
            data2 = data2.ToString().Replace("<table class =''>", "<table class ='table'>");
            ViewBag.Data2 = data2;
            return View();
        }

        public ActionResult ViewData(string type, string maPhieu)
        {
            DataTable tblData = new DataTable("Data");
            string title = "Dữ liệu";
            switch (type)
            {
                case "TDIVU":
                    title = "Dữ liệu thu dịch vụ từ Cyber";
                    tblData = _LoggingService.getDataHistory(maPhieu, "PMTC_CT_01");

                    break;
                default: break;
            }
            ViewBag.Data = Common.DataTableToHtmlVertical(tblData,title);
            return View();
        }
    }
}