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
using OfficeOpenXml;
using TNK.Helper;
using OfficeOpenXml.Style;
using TNK.Services.Authentication;

namespace TNK.Controllers
{
    public class KhoTaiSanController : BasePublicController
    {
       
        int total = 0;
        int pageSize = 30;

        IUserervice _userService;
        ICategoryService _categoryService;
        IKhoTaiSanService _khoService;
        HttpContextBase _httpContext;
        IAuthenticationService _authenticationService;
        User CurrentUser;
        ISoChiService _soChiService;
        AF.Library.Logger _logger;
        public KhoTaiSanController(IUserervice _userService
            , ICategoryService _categoryService
            , IKhoTaiSanService _khoService          
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

        void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query,string type,int pageSize=30)
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

        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1,int pageSize=30)
        {
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Title = "Kho tài sản";
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "KhoTaiSan", pageSize);         
            var model = _khoService.GetTaiSan(f, t, query, p, ref total, pageSize);
            ViewBag.Total = total;
            return View("KhoTaiSanIndex",model);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Title = "Điều chỉnh tài sản";           
            KhoTaiSanModel model = new KhoTaiSanModel();            
            Guid rs = Guid.Empty;
            if (Guid.TryParse(id, out rs))
            {
                model.Item = _khoService.GetTaiSan(new Guid(id));
                model.ListTenTaiSan = _soChiService.GetTenTaiSan("TAISA");
                model.ListNhaCungCap = _soChiService.GetDoiTac("NCC");
                if (model.Item.GiaTriConLai == null)
                    model.Item.GiaTriConLai = 0;
            }
            else
                ViewBag.Alert = "Mã tài sản không đúng";
           
            return View("KhoTaiSanEdit", model);
        }
        [HttpPost]
        public ActionResult Edit(KhoTaiSan item)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_khoService.UpdateTaiSan(item))
            {
                ViewBag.Alert = "Cập nhật thành công";
                TempData["Alert"] = "Cập nhật thành công";
                return Redirect("/KhoTaiSan/Index");
            }
            else
                ViewBag.Alert = "Cập nhật thất bại";
           
            return View(item);
        }

        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
        ExcelPackage pck = new ExcelPackage();

        public ActionResult ReportAll(DateTime FromDate, DateTime ToDate,string Query)
        {
            try
            {

                pck = new ExcelPackage();

                List<string> lstSheetsName = new List<string>();
                lstSheetsName.AddRange(new string[]
                            {
                            "KhoTaiSan"
                            });
                foreach (string s in lstSheetsName)
                {
                    pck.Workbook.Worksheets.Add(s);
                    pck.Workbook.Worksheets[s].Cells.Style.Font.Name = "Arial";
                }
                string strFileName = "Danh_Tài_Sản" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".xlsx";
                ReportTaiSan(FromDate, ToDate, Query);
                return new ExcelInsuranceDownload(pck, strFileName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return View();
        }
        string format = "#,##0";
        private void ReportTaiSan(DateTime FromDate, DateTime ToDate,string Query)
        {
            DateTime f = new DateTime(), t = new DateTime();
            int p = 1;
            string query = "", from = "", to = "";
            SetSearch(ref from, ref to, ref f, ref t, p, ref Query, "KhoTaiSan");

            var Data = _khoService.GetListAll(FromDate, ToDate, Query);
            var sheet = pck.Workbook.Worksheets["KhoTaiSan"];
            sheet.View.FreezePanes(2, 1);
            try
            {
                #region BC KHO TÀI SẢN
                var cellA1N1 = sheet.Cells["A1:L1"];
                cellA1N1.Value = "DANH SÁCH KHO TÀI SẢN";
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
                cellC2C3.Value = "PHIẾU NHẬP";
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
                cellF2F3.Value = "TÊN LOẠI TÀI SẢN";
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
                cellD3.Value = "TÊN TÀI SẢN";
                cellD3.Style.Font.Bold = true;
                cellD3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellD3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellD3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellD3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellD3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellD3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellD2E2 = sheet.Cells["F2"];
                cellD2E2.Value = "GIÁ MUA";
                cellD2E2.Merge = true;
                cellD2E2.Style.Font.Bold = true;
                cellD2E2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellD2E2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellD2E2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellD2E2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellD2E2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellD2E2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellE3 = sheet.Cells["G2"];
                cellE3.Value = "THỜI GIAN KHẤU HAO";
                cellE3.Style.Font.Bold = true;
                cellE3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellE3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellE3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellE3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellE3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellE3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellH3 = sheet.Cells["H2"];
                cellH3.Value = "HÌNH THỨC KHẤU HAO";
                cellH3.Style.Font.Bold = true;
                cellH3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellH3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellH3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellH3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellH3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellH3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellI3 = sheet.Cells["I2"];
                cellI3.Value = "GIÁ TRỊ CÒN LẠI";
                cellI3.Style.Font.Bold = true;
                cellI3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellI3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellI3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellI3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellI3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellI3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellJ3 = sheet.Cells["J2"];
                cellJ3.Value = "NHÀ CUNG CẤP";
                cellJ3.Style.Font.Bold = true;
                cellJ3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellJ3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellJ3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellJ3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellJ3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellJ3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellK3 = sheet.Cells["K2"];
                cellK3.Value = "GIÁ BÁN TS";
                cellK3.Style.Font.Bold = true;
                cellK3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellK3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellK3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellK3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellK3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellK3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var cellL3 = sheet.Cells["L2"];
                cellL3.Value = "NGÀY BÁN TS";
                cellL3.Style.Font.Bold = true;
                cellL3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cellL3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cellL3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cellL3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellL3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellL3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                int index = 3;

                if (Data.Count > 0)
                {
                    foreach (var item in Data)
                    {

                        sheet.Cells["A" + index].Value = index - 2;
                        sheet.Cells["A" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["A" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["B" + index].Value = @item.NgayNhapKho.ToString(DateFormat);
                        sheet.Cells["B" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["B" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["C" + index].Value = item.MaPhieuNhap;
                        //sheet.Cells["C" + index].Style.Numberformat.Format = format;
                        sheet.Cells["C" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["C" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["D" + index].Value = item.TenLoaiTS;
                        //sheet.Cells["D" + index].Style.Numberformat.Format = format;
                        sheet.Cells["D" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["D" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["E" + index].Value = item.TenTaiSan;
                        //sheet.Cells["E" + index].Style.Numberformat.Format = format;
                        sheet.Cells["E" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["E" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["F" + index].Value = item.GiaMua;
                        sheet.Cells["F" + index].Style.Numberformat.Format = format;
                        sheet.Cells["F" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["F" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["G" + index].Value = item.ThoiGianKhauHao;
                        sheet.Cells["G" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["G" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["H" + index].Value = item.HinhThucKhauHao;
                        //sheet.Cells["H" + index].Style.Numberformat.Format = format;
                        sheet.Cells["H" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["H" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["I" + index].Value = item.GiaTriConLai;
                        sheet.Cells["I" + index].Style.Numberformat.Format = format;
                        sheet.Cells["I" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["I" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["J" + index].Value = item.NhaCungCap;
                        //sheet.Cells["J" + index].Style.Numberformat.Format = format;
                        sheet.Cells["J" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["J" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["K" + index].Value = item.GiaBan;
                        sheet.Cells["K" + index].Style.Numberformat.Format = format;
                        sheet.Cells["K" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["K" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        sheet.Cells["L" + index].Value = item.NgayThu.ToString(DateFormat) == "01/01/2018" ? "" : item.NgayThu.ToString(DateFormat);
                        //sheet.Cells["J" + index].Style.Numberformat.Format = format;
                        sheet.Cells["L" + index].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells["L" + index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        index += 1;
                    }
                }
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