using System.Collections.Generic;
using System.Web.Mvc;
using TNK.Core.Domain;
using TNK.Model;
using TNK.Services.Catalog;
using TNK.Services.Configuration;
using TNK.Services.SoThu;
using TNK.Services.Users;
using System.Linq;
using System;
using TNK.Services.SoChi;
using TNK.Services.KhoXe;
using System.Data;
using System.IO;
using OfficeOpenXml;
using TNK.Core.Domain.View;
using TNK.Services.Authentication;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;
using TNK.Helper;
using TNK.Services.Log;
using OfficeOpenXml.Style;
using TNK.Services.Manager;

namespace TNK.Controllers
{
    public class KhoXeController : BasePublicController
    {
        int total = 0;
        int pageSize = 30;
        public string LoaiPhieu = "CTAUN";
        public string ViewIndexName { get { return "../SoChi/" + LoaiPhieu + "Index"; } }
        public string ViewCreateName { get { return "../SoChi/" + LoaiPhieu + "Create"; } }
        public string ViewEditName { get { return "../SoChi/" + LoaiPhieu + "Edit"; } }
        public string ViewDeleteName { get { return "../SoChi/" + LoaiPhieu + "Edit"; } }
        public string UrlIndex { get { return "/" + LoaiPhieu + "/Index"; } }

        IUserervice _userService;
        IConfigService _categoryService;
        ISoChiService _soChiService;
        ISoThuService _soThuService;
        IPhieuThuService _phieuThuService;
        IKhoXeService _khoXeService;
        ILoaiXeService _loaiXeService;
        IAuthenticationService _authenticationService;
        ITuiDinhKhoanService _tuiDinhKhoanService;
        ISoKetChuyenService _soKetChuyenService;
        IConfigService _configService;
        ILogger _log;
        User CurrentUser;
        AF.Library.Logger _logger;
        public string ChonNgay = "NN";
        private HttpContextBase _httpContext;
        public KhoXeController(IUserervice _userService
            , IConfigService _categoryService
            , ISoChiService _soChiService
            , IPhieuThuService _phieuThuService
            , ISoThuService _soThuService
            , IKhoXeService _khoXeService
            , ILoaiXeService _loaiXeService
            , ITuiDinhKhoanService _tuiDinhKhoanService
            , IAuthenticationService _authenticationService
            , IConfigService _configService
            , ILogger _log
            , HttpContextBase _httpContext
            , ISoKetChuyenService _soKetChuyenService
            ) : base()
        {
            this._userService = _userService;
            this._categoryService = _categoryService;
            this._soChiService = _soChiService;
            this._soThuService = _soThuService;
            this._phieuThuService = _phieuThuService;
            this._khoXeService = _khoXeService;
            this._loaiXeService = _loaiXeService;
            this._tuiDinhKhoanService = _tuiDinhKhoanService;
            this._soKetChuyenService = _soKetChuyenService;
            this._authenticationService = _authenticationService;
            this._httpContext = _httpContext;
            this._log = _log;
            this._configService = _configService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }
        private string SetDateDefault()
        {
            return DateTime.Now.ToString(DateFormat);
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
            }
            model.Users = _userService.get();
            model.HTTT = _soChiService.GetHTTT();

            return model;
        }
        #region Nhap xe hang tren duong
        public ActionResult HTDIndex()
        {

            ViewBag.Alert = TempData["Alert"];
            ViewBag.LoaiPhieu = LoaiPhieu;
            var model = _khoXeService.GetDanhSachKhoHangTrenDuong();
            return View("HTDIndex", model);
        }

        public ActionResult HTDCreate()
        {
            ViewBag.Alert = TempData["Alert"];
            KhoXeModel model = new KhoXeModel();
            model.Item = new KhoXe();
            model.TuiHangTrenDuong = _khoXeService.GetTuiHangTrenDuong(Request["p"]);
            model.MaXe = _loaiXeService.GetViewLoaiXeList();
            model.PhieuNhapKho = new PhieuNhapKho();
            model.PhieuNhapKho.MaPhieuChi = model.TuiHangTrenDuong.MaTui;
            ViewBag.Date = SetDateDefault();
            ViewBag.SoTienConLai = _khoXeService.SoTienConLai();
            //model.TuiHangTrenDuong = _khoXeService.GetTuiHangTrenDuong(req);
            return View("HTDCreate", model);
        }
        [HttpPost]
        public ActionResult HTDCreate(KhoXe item,PhieuNhapKho phieuNhapKho)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Alert = TempData["Alert"];
            item.GiaHoaDon = item.GiaVon;
            string msg = _khoXeService.NhapKhoXe(item, phieuNhapKho);
            if (msg == "")
            {
                TempData["Alert"] = "Tạo mới thành công";
                return Redirect("/KhoXe/HTDIndex");
            }
            else
            {               
                TempData["Alert"] = msg;                
                return Redirect("/KhoXe/HTDIndex");
            }
            //model.TuiHangTrenDuong = _khoXeService.GetTuiHangTrenDuong(req);
            //return View();
        }

        #region thống kê kho xe
        public ActionResult ThongKeIndex(string from = "", string to = "")
        {
            DateTime f = new DateTime(), t = new DateTime();
            if (from == "" && to == "")
            {
                f = DateTime.Now;
                t = DateTime.Now;
            }
            else
            {
                if (from == "" || to == "")
                {
                    f = DateTime.Now;
                    t = DateTime.Now;
                }
                else
                {
                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                } 
            }
            var list = _khoXeService.ListDanhSach(f, t);
            string[] DateFrom = f.GetDateTimeFormats();
            string[] DateTo = t.GetDateTimeFormats();
            @ViewBag.From = DateFrom[0];
            @ViewBag.To = DateTo[0];
            return View(list);
        }
        #endregion

        #endregion

        #region Danh sách xe
        public void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string type)
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
        void SetSearch1(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string type, string tinhtrang,string chonngay,int pageSize=30)
        {
            var cookie = Request.Cookies.Get(type);
            if (from == "" && to == "" && query == "" && tinhtrang == "")
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
                    cookie.Values["tinhTrang"] = tinhtrang;
                    cookie.Values["chonngay"] = chonngay;
                    _httpContext.Response.Cookies.Set(cookie);
                }
                else
                {
                    from = cookie.Values["from"];
                    to = cookie.Values["to"];
                    query = cookie.Values["query"];
                    tinhtrang = cookie.Values["tinhtrang"];
                    chonngay = cookie.Values["chonngay"];
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
                cookie.Values["tinhtrang"] = tinhtrang;
                cookie.Values["chonngay"] = chonngay;
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
        //static List<ViewKhoXe> Data = new List<ViewKhoXe>();
        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1,string TinhTrang="", string chonngay = "NN", int tab = 1,int pageSize=30)
        {
            ViewBag.Alert = TempData["Alert"];
            ViewBag.LoaiPhieu = LoaiPhieu;
            DateTime f = new DateTime(), t = new DateTime();
            ViewBag.Date = "01/01/2013";
            SetSearch1(ref from, ref to, ref f, ref t, p, ref query, "KhoXe",TinhTrang,chonngay);
            var model = _khoXeService.GetDanhSachXe(f,t,query,p,ref total,pageSize,TinhTrang,chonngay);
            //var list = _khoXeService.GetDanhSachXeTon(t, query, p, ref total, pageSize);
            ChonNgay = chonngay;
            ViewBag.LoaiXe = _loaiXeService.GetViewLoaiXeList();
            ViewBag.ThueChap = _loaiXeService.ListThueChap();
            ViewBag.Total = total;
            ViewBag.PageSize = pageSize;
            ViewBag.PageIndex = p;
            ViewBag.Tab = tab;   
            return View("KhoXeIndex", model);
        }

        public ActionResult XeTon(string query = "", string to = "", int p = 1)
        {
            ViewBag.Alert = TempData["Alert"];
            string fr = "12/12/2002";
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch(ref fr, ref to, ref f, ref t, p, ref query, "KhoXe");
            var model = _khoXeService.GetDanhSachXeTon(t, query, p, ref total, pageSize);
            ViewBag.LoaiXe = _loaiXeService.GetViewLoaiXeList();
            ViewBag.ThueChap = _loaiXeService.ListThueChap();
            ViewBag.Total = total;
            ViewBag.Tab = 2;
            return View(model);
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
            string message = _soChiService.CreatePhieuChi(item, httt);
            if (message =="")
            {
                TempData["Alert"] = "Tạo mới thành công";
                return Redirect(UrlIndex);
            }
            else
            {
                TempData["Alert"] = "Tạo mới thất bại" + message;
                ViewBag.Alert = "Tạo mới thất bại" + message;
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

        public ActionResult EditKhoXe(KhoXe objKhoXe)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (string.IsNullOrEmpty(objKhoXe.SoKhung))
                return Json(new { IsError = true, Message = "Lỗi: Không có thông tin cập nhật. Vui lòng kiểm tra lại." });

            if (!_khoXeService.CheckExists(objKhoXe.SoKhung))
                return Json(new { IsError = true, Message = "Thông tin không tồn tại. Vui lòng kiểm tra lại." });

            if (_khoXeService.Update(objKhoXe))
                return Json(new { IsError = false, Message = "Cập nhật thành công." });
            else
                return Json(new { IsError = true, Message = "Cập nhật thất bại." });
        }

        [HttpPost]
        public ActionResult Edit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Title = "Cập nhật phiếu";
            string sessionId = "";
            string message = _soChiService.UpdatePhieuChi(item, httt,ref sessionId);
            if (string.IsNullOrEmpty(message))
            {
                ViewBag.Alert = "Cập nhật thành công";
               // return Redirect(UrlIndex);
            }
            else
                ViewBag.Alert = "Cập nhật thất bại:" + message;
            SoChiModel model = GetModel(LoaiPhieu);
            model.LoaiPhieu = LoaiPhieu;
            model.Item = item;
            model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
            if (model.CTPT.Count == 0)
            {
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
            }
            return View(model);
        }
        public ActionResult Delete(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string message = _soChiService.DeletePhieuChi(id);
            if (string.IsNullOrEmpty(message))
                TempData["Alert"] = "Xóa thành công";
            else
            {
                TempData["Alert"] = "Xóa thất bại:" + message;
            }
            return Redirect(UrlIndex);
        }

        [HttpPost]
        public ActionResult DeleteKhoXe(Guid id,string sk)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Content("Bạn không có quyền xóa");
            }
            DateTime NgayHienTai = DateTime.Now;
            int soLuong = _loaiXeService.CheckTheChap(sk,NgayHienTai);
            if (soLuong > 0)
            {
                return Content("Lỗi");
            }
            bool HHHB = _loaiXeService.HHHB(sk, NgayHienTai);//ngày 22.8.2018 không đc xóa xe hoa không khi đã có trả nợ hhhb
            if(HHHB == false)
            {
                return Content("Lỗi: đã trả nợ HHHB");
            }
            string message = _khoXeService.Delete(id);
            if (string.IsNullOrEmpty(message))
                return Content("success");
            else
                return Content(message);
        }

        #endregion

        public ActionResult ImportData(string maTui)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string error = "";
           
            try
            {
                DataTable dtb = new DataTable();
                dtb.TableName = "DataImport";
                dtb.Columns.Add("Grade", typeof(string));
                dtb.Columns.Add("FrameNo", typeof(string));
                dtb.Columns.Add("EngineNo", typeof(string));
                dtb.Columns.Add("Color", typeof(string));
                dtb.Columns.Add("TMSSNo", typeof(string));
                dtb.Columns.Add("VnAmount", typeof(string));

                
                Hashtable hshCol = new Hashtable();
                hshCol.Add("NGAYNHAP", 0);
                hshCol.Add("SOKHUNG", 0);
                hshCol.Add("SOMAY", 0);
                hshCol.Add("GIANHAP(TMV)", 0);
                hshCol.Add("LOAIXE", 0);
                hshCol.Add("MAMAU", 0);
                hshCol.Add("XUATXU", 0);
                hshCol.Add("DOIXE", 0);
                hshCol.Add("HOAHONGHB", 0);
                hshCol.Add("TMSS", 0);
                hshCol.Add("SOHOADON", 0);
                //hshCol.Add("GIANHAP(HĐ)", 0);

                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    List<ViewLoaiXe> lst = _loaiXeService.GetViewLoaiXeList();
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        Stream stream = fileContent.InputStream;
                        using (var package = new ExcelPackage(stream))
                        {
                            var workBook = package.Workbook;
                            if (workBook != null)
                            {
                                if (workBook.Worksheets.Count > 0)
                                {
                                    var ws = workBook.Worksheets[1];
                                    if (ws.Dimension.End.Row < 2)
                                    {
                                        return Json(new { IsError = true, Message = "Không có dữ liệu để import" }, JsonRequestBehavior.AllowGet);
                                    }

                                    #region Kiểm tra file đúng định dạng
                                    bool IsFormat = true;
                                    int beginRowNum = 1;
                                    for(int ii = 1;ii <=20;ii++)
                                    {
                                        if (ws.Cells[beginRowNum, ii].Value != null)
                                        {
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("NGAYNHAP"))
                                                hshCol["NGAYNHAP"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("SOKHUNG"))
                                                hshCol["SOKHUNG"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("SOMAY"))
                                                hshCol["SOMAY"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("GIANHAP(TMV)"))
                                                hshCol["GIANHAP(TMV)"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("LOAIXE"))
                                                hshCol["LOAIXE"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("MAMAU"))
                                                hshCol["MAMAU"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("XUATXU"))
                                                hshCol["XUATXU"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("DOIXE"))
                                                hshCol["DOIXE"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("HOAHONGHB"))
                                                hshCol["HOAHONGHB"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("TMSS"))
                                                hshCol["TMSS"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("SOHOADON"))
                                                hshCol["SOHOADON"] = ii;
                                            //if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("GIANHAP(HĐ)"))
                                            //    hshCol["GIANHAP(HĐ)"] = ii;

                                        }

                                    }
                                    //neu co cot khong ton tai trong danh sach cot can thiet thi canh bao sai dinh dang
                                    foreach (DictionaryEntry o in hshCol)
                                        if (o.Value.ToString() == "0")
                                        {
                                            return Json(new { IsError = true, Message = "File Excel không đúng định dạng" }, JsonRequestBehavior.AllowGet);
                                        }
                                    #endregion

                                    #region Add dữ liệu
                                    int indexNgayNhap = (int)hshCol["NGAYNHAP"];
                                    int indexSoKhung = (int)hshCol["SOKHUNG"];
                                    int indexSoMay = (int)hshCol["SOMAY"];
                                    int indexLoaiXe = (int)hshCol["LOAIXE"];
                                    int indexGiaMua = (int)hshCol["GIANHAP(TMV)"];
                                    int indexMaMau = (int)hshCol["MAMAU"];
                                    int indexXuatXu = (int)hshCol["XUATXU"];
                                    int indexDoiXe = (int)hshCol["DOIXE"];
                                    int indexHoaHongHB = (int)hshCol["HOAHONGHB"];
                                    int indexTMSS = (int)hshCol["TMSS"];
                                    int indexHoaDon = (int)hshCol["SOHOADON"];
                                    //int indexGiaNhapHD = (int)hshCol["GIANHAP(HĐ)"];

                                    //kiem tra rang buoc tien trong hang tren duong con du thi moi import
                                    double tongTien = 0;
                                    for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                    {
                                        try
                                        {
                                            string GiaVon = ws.Cells[rowNum, indexGiaMua].Value == null ? "" : ws.Cells[rowNum, indexGiaMua].Value.ToString();
                                            //string GiaNhapHD = ws.Cells[rowNum, indexGiaNhapHD].Value == null ? "" : ws.Cells[rowNum, indexGiaNhapHD].Value.ToString();
                                            string soKhung = ws.Cells[rowNum, indexSoKhung].Value == null ? "" : ws.Cells[rowNum, indexSoKhung].Value.ToString();
                                            //string HHHB = ws.Cells[rowNum, indexHoaHongHB].Value == null ? "" : ws.Cells[rowNum, indexHoaHongHB].Value.ToString();
                                            
                                            if (string.IsNullOrEmpty(soKhung))
                                            {
                                                error = "Dòng " + rowNum + " không có số khung.";
                                                break;
                                            }
                                            if (!TNK.Core.Common.IsNumeric(GiaVon))
                                            {
                                                error = "Dòng " + rowNum + " giá vốn (" + GiaVon + ") không phải là kiểu số.";
                                                break;
                                            }

                                            
                                            //if (!TNK.Core.Common.IsNumeric(GiaNhapHD))
                                            //{
                                            //    error = "Dòng " + rowNum + " giá nhập hợp đồng (" + GiaNhapHD + ") không phải là kiểu số.";
                                            //    break;
                                            //}
                                            else
                                                tongTien += double.Parse(GiaVon);
                                            
                                        }
                                        catch (Exception e)
                                        {
                                            error += e;
                                            
                                        }
                                    }
                                    if (string.IsNullOrEmpty(error))
                                    {

                                        TuiDinhKhoan tdk = _tuiDinhKhoanService.GetTuiDinhKhoan(maTui);
                                        //cau hinh cho phep nhap tren duong am thi cho nhap không can xe nua
                                        bool choPhepNhapHTDAm = this._configService.GetConfigByCode("CHO_PHEP_NHAP_HTD_AM") == "1";

                                        if (tdk != null && ( tongTien <= tdk.SoTienDangCo || choPhepNhapHTDAm))
                                        {
                                            for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                            {
                                                try
                                                {
                                                    DataRow row = dtb.NewRow();
                                                    DateTime NGAYNHAPKHO = new DateTime();
                                                    string ngayNhap = ws.Cells[rowNum, indexNgayNhap].Value == null ? "" : ws.Cells[rowNum, indexNgayNhap].Value.ToString();
                                                    if (TNK.Core.Common.IsDate(ngayNhap) == false)
                                                    {
                                                        if (TNK.Core.Common.IsNumeric(ngayNhap))
                                                        {
                                                            double DATE = double.Parse(ngayNhap);
                                                            NGAYNHAPKHO = DateTime.FromOADate(DATE);
                                                        }
                                                    }
                                                    else
                                                        NGAYNHAPKHO = Convert.ToDateTime(ngayNhap);
                                                    if (NGAYNHAPKHO.Year > 2000)
                                                    {
                                                        string soKhung = ws.Cells[rowNum, indexSoKhung].Value == null ? "" : ws.Cells[rowNum, indexSoKhung].Value.ToString();
                                                        string soMay = ws.Cells[rowNum, indexSoMay].Value == null ? "" : ws.Cells[rowNum, indexSoMay].Value.ToString();
                                                        string grad = ws.Cells[rowNum, indexLoaiXe].Value == null ? "" : ws.Cells[rowNum, indexLoaiXe].Value.ToString();

                                                        //string soVin = ws.Cells[rowNum, 4].Value == null ? "" : ws.Cells[rowNum, 4].Value.ToString();

                                                        string maMau = ws.Cells[rowNum, indexMaMau].Value == null ? "" : ws.Cells[rowNum, indexMaMau].Value.ToString();
                                                        string xuatXu = ws.Cells[rowNum, indexXuatXu].Value == null ? "" : ws.Cells[rowNum, indexXuatXu].Value.ToString();
                                                        string DoiXe = ws.Cells[rowNum, indexDoiXe].Value == null ? "" : ws.Cells[rowNum, indexDoiXe].Value.ToString();
                                                        string TMSSNo = ws.Cells[rowNum, indexTMSS].Value == null ? "" : ws.Cells[rowNum, indexTMSS].Value.ToString();
                                                        string GiaVon = ws.Cells[rowNum, indexGiaMua].Value == null ? "" : ws.Cells[rowNum, indexGiaMua].Value.ToString();
                                                        string HHHB = ws.Cells[rowNum, indexHoaHongHB].Value == null ? "" : ws.Cells[rowNum, indexHoaHongHB].Value.ToString();
                                                        string hoaDon = ws.Cells[rowNum, indexHoaDon].Value == null ? "" : ws.Cells[rowNum, indexHoaDon].Value.ToString();
                                                        //string GiaNhapHD = ws.Cells[rowNum, indexGiaNhapHD].Value == null ? "" : ws.Cells[rowNum, indexGiaNhapHD].Value.ToString();
                                                        if (string.IsNullOrEmpty(HHHB))
                                                            HHHB = "0";
                                                        grad = grad.Trim();
                                                        maMau = maMau.Trim();
                                                        xuatXu = xuatXu.ToUpper().Trim();
                                                        DoiXe = DoiXe.Trim();
                                                        if (string.IsNullOrEmpty(DoiXe))
                                                        {
                                                            error += Environment.NewLine + "Dòng " + rowNum + " vui lòng nhập năm của đời xe";
                                                            break;
                                                        }
                                                        if (!TNK.Core.Common.IsNumeric(DoiXe))
                                                        {
                                                            error += Environment.NewLine + "Dòng " + rowNum + " đời xe phải là kiểu số";
                                                            break;
                                                        }
                                                        CategoryItem itemNamDoiXe = _loaiXeService.NamDoiXe(DoiXe);
                                                        if(itemNamDoiXe == null)
                                                        {
                                                            error += Environment.NewLine + "Dòng " + rowNum + " đời xe không có trong danh mục đời xe";
                                                            break;
                                                        }
                                                        if (string.IsNullOrEmpty(xuatXu))
                                                        {
                                                            error += Environment.NewLine + "Dòng " + rowNum + " chưa có xuất xứ";
                                                            break;
                                                        }
                                                        if(!string.IsNullOrEmpty(xuatXu) && xuatXu != "CKD" && xuatXu != "CBU")
                                                        {
                                                            error += Environment.NewLine + "Dòng " + rowNum + " xuất xứ phải là CKD hoặc là CBU";
                                                            break;
                                                        }
                                                        if (!string.IsNullOrEmpty(soKhung)
                                                            )
                                                        {
                                                            CategoryItem DoiXeItem = _loaiXeService.NamDoiXe(DoiXe);
                                                            string MaLoaiXe = grad + "-" + maMau + xuatXu + DoiXe;
                                                            ViewLoaiXe itemLoaiXe = lst.Find(x => x.MaLoaiXe == MaLoaiXe &&  x.XuatXu == xuatXu && x.CodeMau == maMau && x.DoiXe == DoiXeItem.Id);
                                                            if (itemLoaiXe == null)
                                                            {
                                                                error += Environment.NewLine + "Dòng " + rowNum + "(" + soKhung + ")Không tìm thấy loại xe (" + MaLoaiXe + ") trong danh mục.";

                                                            }
                                                            else
                                                            {
                                                                KhoXe item = new KhoXe();
                                                                
                                                                //item.
                                                                item.SoKhung = soKhung.Trim();
                                                                item.SoMay = soMay;
                                                                item.SoTMSS = TMSSNo;
                                                                item.NhaCungCap = maTui;
                                                                if (string.IsNullOrEmpty(GiaVon))
                                                                    item.GiaVon = 0;
                                                                else
                                                                    item.GiaVon = Double.Parse(GiaVon);
                                                                item.GiaNiemYet = itemLoaiXe.GiaNiemYet.Value;
                                                                //item.VIN = soVin;
                                                                item.TinhTrang = "N";
                                                                item.MaLoaiXe = itemLoaiXe.MaLoaiXe;
                                                                item.Model = itemLoaiXe.CodeModel;
                                                                item.SoHoaDon = hoaDon;
                                                                if (string.IsNullOrEmpty(GiaVon))
                                                                    item.GiaHoaDon = 0;
                                                                else
                                                                    item.GiaHoaDon = Double.Parse(GiaVon);
                                                                PhieuNhapKho phieuNhap = new PhieuNhapKho();
                                                                phieuNhap.HoaHongHB = Convert.ToDouble(HHHB);
                                                                phieuNhap.NgayNhap = NGAYNHAPKHO;
                                                                phieuNhap.MaPhieuChi = maTui;
                                                                string message = _khoXeService.NhapKhoXe(item, phieuNhap);
                                                                if (message != "")
                                                                    error += Environment.NewLine + "Dòng " + rowNum + "(" + soKhung + "):" + message;

                                                            }
                                                        }
                                                    }
                                                    else
                                                        error += Environment.NewLine + "Dòng " + rowNum + ": Sai định dạng ngày nhập kho. Vui lòng kiểm tra lại.";
                                                    /*  row[0] = ws.Cells[rowNum, 2].Value == null ? "" : ws.Cells[rowNum, 2].Value.ToString();
                                                      row[1] = ws.Cells[rowNum, 3].Value == null ? "" : ws.Cells[rowNum, 3].Value.ToString();
                                                      row[2] = ws.Cells[rowNum, 5].Value == null ? "" : ws.Cells[rowNum, 5].Value.ToString();
                                                      row[3] = ws.Cells[rowNum, 7].Value == null ? "" : ws.Cells[rowNum, 7].Value.ToString();
                                                      row[4] = ws.Cells[rowNum, 8].Value == null ? "" : ws.Cells[rowNum, 8].Value.ToString();
                                                      row[5] = ws.Cells[rowNum, 35].Value == null ? "" : ws.Cells[rowNum, 35].Value.ToString();
                                                      dtb.Rows.Add(row);*/
                                                }
                                                catch (Exception exx)
                                                {
                                                    error += Environment.NewLine + "Dòng " + rowNum + ": " + exx;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            error += "Không đủ số tiền hàng trên đường để nhập kho. Vui lòng tạo phiếu chi hàng trên đường.";
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                   
                }

               
            }
            catch (Exception objEx)
            {
                _log.WriteLog("KhoXeController.ImportData:" + objEx);
                return Json(new { IsError = true, Message = "Lỗi import excel."  });
            }
            if (string.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    IsError = false, Message = ""
                });
            }
            else
            {
                _log.WriteLog("KhoXeController.ImportData:" + error);
                return Json(new { IsError = true, Message = error });
            }
                
        }
        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
        public ActionResult DanhSachPhieuNhap(string from = "", string to = "", string query = "", int p = 1, string TinhTrang = "",int pageSize=30)
        {
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch1(ref from, ref to, ref f, ref t, p, ref query, "DanhSachPhieuNhap", TinhTrang,"", pageSize);
            var model = _khoXeService.GetDanhSachPhieuNhap(f, t, query, p, ref total, pageSize, TinhTrang);
            ViewBag.LoaiPN = TinhTrang;
            ViewBag.Total = total;
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Data = model;
            return View();
        }
        public ActionResult TheChap(string id)
        {
            ViewBag.SoKhung = id;
            ViewBag.NganHang = _loaiXeService.ListThueChap();
            //LichSuTheChapXe XeCuoiCung = _loaiXeService.XeTCCuoiCung(id);
            //if(XeCuoiCung.NgayHetTheChap != null)
            //{
            //    DateTime NHTC = Convert.ToDateTime(XeCuoiCung.NgayHetTheChap);
            //    ViewBag.NHTC = NHTC.ToString("dd/MM/yyyy");
            //}
            //else
            //{
            //    ViewBag.NHTC = null;
            //}
            List<LichSuTheChapXe> model = new List<LichSuTheChapXe>();
            return View(model);
        }
        [HttpPost]
        public ActionResult TheChap(string thechap = "",string id = "")
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string value = HttpContext.Request.RawUrl;
            string[] url = Regex.Split(value, Regex.Escape("/"));
            ViewBag.SoKhung = url[3];
            List<LichSuTheChapXe> model = new List<LichSuTheChapXe>();
            model = _khoXeService.List(url[3]);
            ViewBag.NganHang = _loaiXeService.ListThueChap();
            //LichSuTheChapXe XeCuoiCung = _loaiXeService.XeTCCuoiCung(id);
            //if (XeCuoiCung.NgayHetTheChap != null)
            //{
            //    DateTime NHTC = Convert.ToDateTime(XeCuoiCung.NgayHetTheChap);
            //    ViewBag.NHTC = NHTC.ToString("dd/MM/yyyy");
            //}
            //else
            //{
            //    ViewBag.NHTC = null;
            //}
            //ViewBag.model = model;
            return View(model);
        }
        public ActionResult TheChapXeCreate(string SoKhung,DateTime NgayTheChap,string NganHangTheChap, DateTime? NgayHetTheChap)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            int soLuong = _loaiXeService.CheckLSTC(SoKhung, NgayTheChap, NgayHetTheChap);
            if (soLuong > 0)
            {
                return Json(new { IsError = true, Message = "Xe vẫn còn đang thế chấp" });
            }
            var Data = new LichSuTheChapXe();
            DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
            if (ngayKetChuyenCuoi.Subtract(NgayTheChap).Days >= 0
                || (NgayHetTheChap != null && ngayKetChuyenCuoi.Subtract(NgayHetTheChap.Value).Days >= 0)
                )
            {
                return Json(new { IsError = false, Message = "Không thể thế chấp xe vào khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán." });
            }
            else
            {
                Data.SoKhung = SoKhung;
                Data.NganHangTheChap = NganHangTheChap;
                Data.NgayTheChap = NgayTheChap;
                Data.NgayHetTheChap = NgayHetTheChap;
                string message = _khoXeService.NhapTheChap(Data);
                if (string.IsNullOrEmpty(message))
                    return Json(new { IsError = false, Message = "Thêm thành công." });
                else
                    return Json(new { IsError = true, Message = "Thêm thất bại." });
            }
        }

        public ActionResult EditTheChap(Guid Id, DateTime NgayTheChap, string NganHangTheChap, DateTime? NgayHetTheChap)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
            if (ngayKetChuyenCuoi.Subtract(NgayTheChap).Days >= 0
                || (NgayHetTheChap!= null && ngayKetChuyenCuoi.Subtract(NgayHetTheChap.Value).Days >= 0)
                )
            {
                return Json(new { IsError = false, Message = "Không thể thế chấp xe vào khoảng thời gia đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán." });
            }
            else
            {
                LichSuTheChapXe Data = new LichSuTheChapXe();
                Data.Id = Id;
                Data.NgayHetTheChap = NgayHetTheChap;
                Data.NgayTheChap = NgayTheChap;
                Data.NganHangTheChap = NganHangTheChap;
                string message = _khoXeService.EditTheChap(Data);
                if (string.IsNullOrEmpty(message))
                    return Json(new { IsError = false, Message = "Cập nhật thành công." });
                return Json(new { IsError = true, Message = message });
            }
        }
        public ActionResult DeleteTheChap(Guid Id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            LichSuTheChapXe Data = new LichSuTheChapXe();
            Data.Id = Id;
            string message = _khoXeService.DeleteTheChap(Data);
            if (string.IsNullOrEmpty(message))
                return Json(new { IsError = false, Message = "Xóa thành công." });
            return Json(new { IsError = true, Message = message});
        }
        public FileResult ExportFile()
        {
            var pck = new ExcelPackage();
            string strFileName = "~\\Views\\FileMau\\FileImportKho_Template.xlsx";
            return File(strFileName, "application/vnd.ms-excel", "FileImportKho_Template.xlsx");
        }
        ExcelPackage pck = new ExcelPackage();
        public ActionResult ReportAll(DateTime FromDate, DateTime ToDate, string chonngay,string loai)
        {         
            try {

                pck = new ExcelPackage();
                
                List<string> lstSheetsName = new List<string>();
                if (loai == "DSX")
                {
                    lstSheetsName.AddRange(new string[]
                            {
                            "KhoXe"
                            });
                    foreach (string s in lstSheetsName)
                    {
                        pck.Workbook.Worksheets.Add(s);
                        pck.Workbook.Worksheets[s].Cells.Style.Font.Name = "Arial";
                    }
                    string strFileName = "Danh_Sach_Xe_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".xls";
                    ReportXe(FromDate,ToDate,chonngay);
                    return new ExcelInsuranceDownload(pck, strFileName);
                }
                else
                {

                    lstSheetsName.AddRange(new string[]
                            {
                            "DanhSachXeTon"
                            });
                    foreach (string s in lstSheetsName)
                    {
                        pck.Workbook.Worksheets.Add(s);
                        pck.Workbook.Worksheets[s].Cells.Style.Font.Name = "Arial";
                    }
                    string strFileName = "Danh_Sach_Xe_Ton" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".xls";
                    ReportXeTon(ToDate);
                    return new ExcelInsuranceDownload(pck, strFileName);

                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return View();
        }
        string format = "#,##0";
        private void ReportXe(DateTime FromDate , DateTime ToDate, string chonngay)
        {
           
            
            DataTable Data = _khoXeService.GetListDanhSachXe(FromDate, ToDate, chonngay);
            var sheet = pck.Workbook.Worksheets["KhoXe"];
            sheet.View.FreezePanes(2, 1);
            try
            {
                #region BC KHO XE
                var cellA1N1 = sheet.Cells["A1:M1"];
                cellA1N1.Value = "DANH SÁCH KHO XE";
                cellA1N1.Merge = true;
                cellA1N1.Style.Font.Bold = true;
                cellA1N1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellA1N1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellA1N1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellA1N1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellA1N1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellA1N1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellA2A3 = sheet.Cells["A2"];
                cellA2A3.Value = "STT";
                cellA2A3.Merge = true;
                cellA2A3.Style.Font.Bold = true;
                cellA2A3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellA2A3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellA2A3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellA2A3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellA2A3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellA2A3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellB2B3 = sheet.Cells["B2"];
                cellB2B3.Value = "NGÀY NHẬP";
                cellB2B3.Merge = true;
                cellB2B3.Style.Font.Bold = true;
                cellB2B3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellB2B3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellB2B3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellB2B3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellB2B3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellB2B3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellC2C3 = sheet.Cells["C2"];
                cellC2C3.Value = "NGÀY XUẤT";
                cellC2C3.Merge = true;
                cellC2C3.Style.WrapText = true;
                cellC2C3.Style.Font.Bold = true;
                cellC2C3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellC2C3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellC2C3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellC2C3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellC2C3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellC2C3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellF2F3 = sheet.Cells["D2"];
                cellF2F3.Value = "PHIẾU NHẬP";
                cellF2F3.Merge = true;
                cellF2F3.Style.WrapText = true;
                cellF2F3.Style.Font.Bold = true;
                cellF2F3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellF2F3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellF2F3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellF2F3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellF2F3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellF2F3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellD3 = sheet.Cells["E2"];
                cellD3.Value = "SỐ KHUNG";
                cellD3.Style.Font.Bold = true;
                cellD3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellD3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellD3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellD3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellD3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellD3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellD2E2 = sheet.Cells["F2"];
                cellD2E2.Value = "SỐ MÁY";
                cellD2E2.Merge = true;
                cellD2E2.Style.Font.Bold = true;
                cellD2E2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellD2E2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellD2E2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellD2E2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellD2E2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellD2E2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellE3 = sheet.Cells["G2"];
                cellE3.Value = "SỐ HÓA ĐƠN";
                cellE3.Style.Font.Bold = true;
                cellE3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellE3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellE3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellE3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellE3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellE3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellH3 = sheet.Cells["H2"];
                cellH3.Value = "SỐ TMSS";
                cellH3.Style.Font.Bold = true;
                cellH3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellH3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellH3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellH3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellH3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellH3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellI3 = sheet.Cells["I2"];
                cellI3.Value = "GIÁ MUA";
                cellI3.Style.Font.Bold = true;
                cellI3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellI3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellI3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellI3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellI3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellI3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellJ3 = sheet.Cells["J2"];
                cellJ3.Value = "GIÁ NIÊM YẾT";
                cellJ3.Style.Font.Bold = true;
                cellJ3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellJ3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellJ3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellJ3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellJ3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellJ3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellK3 = sheet.Cells["K2"];
                cellK3.Value = "LOẠI XE";
                cellK3.Style.Font.Bold = true;
                cellK3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellK3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellK3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellK3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellK3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellK3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellL2 = sheet.Cells["L2"];
                cellL2.Value = "NHÀ CUNG CẤP";
                cellL2.Style.Font.Bold = true;
                cellL2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellL2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellL2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellL2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellL2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellL2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellM2 = sheet.Cells["M2"];
                cellM2.Value = "TÌNH TRẠNG";
                cellM2.Style.Font.Bold = true;
                cellM2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellM2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellM2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellM2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellM2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellM2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                int index = 3;
                double TongGiaVon = 0, TongGiaNiemYet = 0;
                string TinhTrang = "";
                
                if(Data.Rows.Count >  0)
                {
                    foreach (DataRow row in Data.Rows)
                    {
                        TongGiaVon += row["GiaVon"].ToString() == "" ? 0 : Convert.ToDouble(row["GiaVon"].ToString());
                        TongGiaNiemYet += row["GiaNiemYet"].ToString() == "" ? 0 : Convert.ToDouble(row["GiaNiemYet"].ToString());
                        TinhTrang = "";
                        switch (row["TinhTrang"].ToString())
                        {
                            case "TC":
                                TinhTrang = "Đang thế chấp";
                                break;
                            case "N":
                                TinhTrang = "Trong kho";
                                break;
                            default:
                                TinhTrang = "Đã xuất";
                                break;
                        }

                        sheet.Cells["A" + index].Value = index - 2;
                        sheet.Cells["A" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["A" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["B" + index].Value = row["NgayNhapKho"].ToString();
                        sheet.Cells["B" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["B" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["C" + index].Value = row["NgayXuatKho"].ToString();
                        //sheet.Cells["C" + index].Style.Numberformat.Format = format;
                        sheet.Cells["C" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["C" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["D" + index].Value = row["MaPhieuNhap"];
                        //sheet.Cells["D" + index].Style.Numberformat.Format = format;
                        sheet.Cells["D" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["D" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["E" + index].Value = row["SoKhung"];
                        //sheet.Cells["E" + index].Style.Numberformat.Format = format;
                        sheet.Cells["E" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["E" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["F" + index].Value = row["SoMay"];
                        //sheet.Cells["F" + index].Style.Numberformat.Format = format;
                        sheet.Cells["F" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["F" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["G" + index].Value = row["SoHoaDon"];
                        sheet.Cells["G" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["G" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["H" + index].Value = row["SoTMSS"].ToString();
                        //sheet.Cells["H" + index].Style.Numberformat.Format = format;
                        sheet.Cells["H" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["H" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["I" + index].Value = row["GiaVon"];
                        sheet.Cells["I" + index].Style.Numberformat.Format = format;
                        sheet.Cells["I" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["I" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["J" + index].Value = row["GiaNiemYet"];
                        sheet.Cells["J" + index].Style.Numberformat.Format = format;
                        sheet.Cells["J" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["J" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["K" + index].Value = row["MaLoaiXe"];
                        //sheet.Cells["K" + index].Style.Numberformat.Format = format;
                        sheet.Cells["K" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["K" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["L" + index].Value = row["NhaCungCap"];
                        //sheet.Cells["L" + index].Style.Numberformat.Format = format;
                        sheet.Cells["L" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["L" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["M" + index].Value = TinhTrang;
                        //sheet.Cells["M" + index].Style.Numberformat.Format = format;
                        sheet.Cells["M" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["M" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        index += 1;
                    }
                }
                
                //index += 1;
                sheet.Cells["A" + index + ":H" + index].Value = "TỔNG";
                sheet.Cells["A" + index + ":H" + index].Merge = true;
                sheet.Cells["A" + index + ":H" + index].Style.Font.Bold = true;
                sheet.Cells["A" + index + ":H" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["A" + index + ":H" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["A" + index + ":H" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A" + index + ":H" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["I" + index].Value = TongGiaVon;
                sheet.Cells["I" + index].Style.Numberformat.Format = format;
                sheet.Cells["I" + index].Style.Font.Bold = true;
                sheet.Cells["I" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["I" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["I" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["I" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["J" + index].Value = TongGiaNiemYet;
                sheet.Cells["J" + index].Style.Numberformat.Format = format;
                sheet.Cells["J" + index].Style.Font.Bold = true;
                sheet.Cells["J" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["J" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["J" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["J" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["K" + index].Value = "";
                //sheet.Cells["K" + index].Style.Numberformat.Format = format;
                sheet.Cells["K" + index].Style.Font.Bold = true;
                sheet.Cells["K" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["K" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["K" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["K" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["L" + index].Value = "";
                //sheet.Cells["L" + index].Style.Numberformat.Format = format;
                sheet.Cells["L" + index].Style.Font.Bold = true;
                sheet.Cells["L" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["L" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["L" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["L" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                sheet.Cells["M" + index].Value = "";
                sheet.Cells["M" + index].Style.Numberformat.Format = format;
                sheet.Cells["M" + index].Style.Font.Bold = true;
                sheet.Cells["M" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["M" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["M" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["M" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                sheet.Cells.AutoFitColumns();
                #endregion
            }
            catch (Exception ex)
            {
                sheet.Cells["A3:A4"].Value = "Lỗi báo cáo" + ex.Message + ". " + ex.InnerException;
                _logger.Error(ex);
            }
        }

        private void ReportXeTon( DateTime ToDate)
        {

            var Data = _khoXeService.GetListDanhSachXeTon(ToDate);
            var sheet = pck.Workbook.Worksheets["DanhSachXeTon"];
            sheet.View.FreezePanes(2, 1);
            try
            {
                #region BC Xe tồn
                var cellA1N1 = sheet.Cells["A1:M1"];
                cellA1N1.Value = "DANH SÁCH KHO XE";
                cellA1N1.Merge = true;
                cellA1N1.Style.Font.Bold = true;
                cellA1N1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellA1N1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellA1N1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellA1N1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellA1N1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellA1N1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellA2A3 = sheet.Cells["A2"];
                cellA2A3.Value = "STT";
                cellA2A3.Merge = true;
                cellA2A3.Style.Font.Bold = true;
                cellA2A3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellA2A3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellA2A3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellA2A3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellA2A3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellA2A3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellB2B3 = sheet.Cells["B2"];
                cellB2B3.Value = "NGÀY NHẬP";
                cellB2B3.Merge = true;
                cellB2B3.Style.Font.Bold = true;
                cellB2B3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellB2B3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellB2B3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellB2B3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellB2B3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellB2B3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellC2C3 = sheet.Cells["C2"];
                cellC2C3.Value = "NGÀY XUẤT";
                cellC2C3.Merge = true;
                cellC2C3.Style.WrapText = true;
                cellC2C3.Style.Font.Bold = true;
                cellC2C3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellC2C3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellC2C3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellC2C3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellC2C3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellC2C3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellF2F3 = sheet.Cells["D2"];
                cellF2F3.Value = "PHIẾU NHẬP";
                cellF2F3.Merge = true;
                cellF2F3.Style.WrapText = true;
                cellF2F3.Style.Font.Bold = true;
                cellF2F3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellF2F3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellF2F3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellF2F3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellF2F3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellF2F3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellD3 = sheet.Cells["E2"];
                cellD3.Value = "SỐ KHUNG";
                cellD3.Style.Font.Bold = true;
                cellD3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellD3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellD3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellD3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellD3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellD3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellD2E2 = sheet.Cells["F2"];
                cellD2E2.Value = "SỐ MÁY";
                cellD2E2.Merge = true;
                cellD2E2.Style.Font.Bold = true;
                cellD2E2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellD2E2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellD2E2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellD2E2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellD2E2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellD2E2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellE3 = sheet.Cells["G2"];
                cellE3.Value = "SỐ HÓA ĐƠN";
                cellE3.Style.Font.Bold = true;
                cellE3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellE3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellE3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellE3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellE3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellE3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellH3 = sheet.Cells["H2"];
                cellH3.Value = "SỐ TMSS";
                cellH3.Style.Font.Bold = true;
                cellH3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellH3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellH3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellH3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellH3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellH3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellI3 = sheet.Cells["I2"];
                cellI3.Value = "GIÁ MUA";
                cellI3.Style.Font.Bold = true;
                cellI3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellI3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellI3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellI3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellI3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellI3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellJ3 = sheet.Cells["J2"];
                cellJ3.Value = "GIÁ NIÊM YẾT";
                cellJ3.Style.Font.Bold = true;
                cellJ3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellJ3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellJ3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellJ3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellJ3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellJ3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellK3 = sheet.Cells["K2"];
                cellK3.Value = "LOẠI XE";
                cellK3.Style.Font.Bold = true;
                cellK3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellK3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellK3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellK3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellK3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellK3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellL2 = sheet.Cells["L2"];
                cellL2.Value = "NHÀ CUNG CẤP";
                cellL2.Style.Font.Bold = true;
                cellL2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellL2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellL2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellL2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellL2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellL2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellM2 = sheet.Cells["M2"];
                cellM2.Value = "TÌNH TRẠNG";
                cellM2.Style.Font.Bold = true;
                cellM2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellM2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellM2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellM2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellM2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellM2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                int index = 3;
                double TongGiaVon = 0, TongGiaNiemYet = 0;
                string TinhTrang = "";

                if (Data.Rows.Count > 0)
                {
                    foreach (DataRow row in Data.Rows)
                    {
                        TongGiaVon += row["GiaVon"].ToString() == "" ? 0 : Convert.ToDouble(row["GiaVon"].ToString());
                        TongGiaNiemYet += row["GiaNiemYet"].ToString() == "" ? 0 : Convert.ToDouble(row["GiaNiemYet"].ToString());
                        TinhTrang = "";
                        switch (row["TinhTrang"].ToString())
                        {
                            case "TC":
                                TinhTrang = "Đang thế chấp";
                                break;
                            case "N":
                                TinhTrang = "Trong kho";
                                break;
                            default:
                                TinhTrang = "Đã xuất";
                                break;
                        }

                        sheet.Cells["A" + index].Value = index - 2;
                        sheet.Cells["A" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["A" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["B" + index].Value = row["NgayNhapKho"].ToString();
                        sheet.Cells["B" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["B" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["C" + index].Value = row["NgayXuatKho"].ToString();
                        //sheet.Cells["C" + index].Style.Numberformat.Format = format;
                        sheet.Cells["C" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["C" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["D" + index].Value = row["MaPhieuNhap"];
                        //sheet.Cells["D" + index].Style.Numberformat.Format = format;
                        sheet.Cells["D" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["D" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["E" + index].Value = row["SoKhung"];
                        //sheet.Cells["E" + index].Style.Numberformat.Format = format;
                        sheet.Cells["E" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["E" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["F" + index].Value = row["SoMay"];
                        //sheet.Cells["F" + index].Style.Numberformat.Format = format;
                        sheet.Cells["F" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["F" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["G" + index].Value = row["SoHoaDon"];
                        sheet.Cells["G" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["G" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["H" + index].Value = row["SoTMSS"].ToString();
                        //sheet.Cells["H" + index].Style.Numberformat.Format = format;
                        sheet.Cells["H" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["H" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["I" + index].Value = row["GiaVon"];
                        sheet.Cells["I" + index].Style.Numberformat.Format = format;
                        sheet.Cells["I" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["I" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["J" + index].Value = row["GiaNiemYet"];
                        sheet.Cells["J" + index].Style.Numberformat.Format = format;
                        sheet.Cells["J" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["J" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["K" + index].Value = row["MaLoaiXe"];
                        //sheet.Cells["K" + index].Style.Numberformat.Format = format;
                        sheet.Cells["K" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["K" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["L" + index].Value = row["NhaCungCap"];
                        //sheet.Cells["L" + index].Style.Numberformat.Format = format;
                        sheet.Cells["L" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["L" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["M" + index].Value = TinhTrang;
                        //sheet.Cells["M" + index].Style.Numberformat.Format = format;
                        sheet.Cells["M" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["M" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        index += 1;
                    }
                }

                //index += 1;
                sheet.Cells["A" + index + ":H" + index].Value = "TỔNG";
                sheet.Cells["A" + index + ":H" + index].Merge = true;
                sheet.Cells["A" + index + ":H" + index].Style.Font.Bold = true;
                sheet.Cells["A" + index + ":H" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["A" + index + ":H" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["A" + index + ":H" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A" + index + ":H" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["I" + index].Value = TongGiaVon;
                sheet.Cells["I" + index].Style.Numberformat.Format = format;
                sheet.Cells["I" + index].Style.Font.Bold = true;
                sheet.Cells["I" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["I" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["I" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["I" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["J" + index].Value = TongGiaNiemYet;
                sheet.Cells["J" + index].Style.Numberformat.Format = format;
                sheet.Cells["J" + index].Style.Font.Bold = true;
                sheet.Cells["J" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["J" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["J" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["J" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["K" + index].Value = "";
                //sheet.Cells["K" + index].Style.Numberformat.Format = format;
                sheet.Cells["K" + index].Style.Font.Bold = true;
                sheet.Cells["K" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["K" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["K" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["K" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["L" + index].Value = "";
                //sheet.Cells["L" + index].Style.Numberformat.Format = format;
                sheet.Cells["L" + index].Style.Font.Bold = true;
                sheet.Cells["L" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["L" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["L" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["L" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                sheet.Cells["M" + index].Value = "";
                sheet.Cells["M" + index].Style.Numberformat.Format = format;
                sheet.Cells["M" + index].Style.Font.Bold = true;
                sheet.Cells["M" + index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["M" + index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Coral);
                sheet.Cells["M" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["M" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                sheet.Cells.AutoFitColumns();
                #endregion
            }
            catch (Exception ex)
            {
                sheet.Cells["A3:A4"].Value = "Lỗi báo cáo" + ex.Message + ". " + ex.InnerException;
                _logger.Error(ex);
            }
        }
    }
}