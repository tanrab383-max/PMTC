using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNK.Core.Domain;
using TNK.Helper;
using TNK.Model;
using TNK.Services.Authentication;
using TNK.Services.ChuyenTien;
using TNK.Services.SoThu;
using TNK.Services.Users;

namespace TNK.Controllers
{
    public class ChuyenTienController : BasePublicController
    {
        IChuyenTienService _chuyenTienService;
        IUserervice _userService;
        ISoThuService _soThuService;
        IAuthenticationService _authenticationService;
        User CurrentUser;
        public ChuyenTienController(IChuyenTienService _chuyenTienService, HttpContextBase _httpContext
            , IUserervice _userService, ISoThuService _soThuService
            , IAuthenticationService _authenticationService)
            : base()
        {
            this._chuyenTienService = _chuyenTienService;
            this._httpContext = _httpContext;
            this._userService = _userService;
            this._soThuService = _soThuService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }


        // GET: ChuyenTien
        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1, string export = "",int pageSize=30)
        {
            ViewBag.Title = "Quản lý chuyển tiền nội bộ";
            ViewBag.Alert = TempData["Alert"];
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CTNB");
            if (export == "Export")
            {
                //ExportExcel("TCODV", f, t, query);
            }
            var model = _chuyenTienService.GetVCTPCTNB(f, t, query, p, ref total, pageSize);
            ViewBag.Total = total;
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Tạo mới chuyển tiền nội bộ";
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Action = "Create";
            var model = new PhieuChuyenTienModel();
            GetModel(model);
            return View(model);
        }
        public void GetModel(PhieuChuyenTienModel model)
        {
            model.Users = _userService.get();
            model.HTTT = _soThuService.GetHTTT();
            if (string.IsNullOrEmpty(model.Item.MaPhieu))
            {
                model.Item.NgayChuyen = DateTime.Now;
                model.Item.MaLoaiPhieu = "CTIEN";
            }
            else
            {
                model.CTPT = _chuyenTienService.GetChiTietPhieuChuyenTienNoiBo(model.Item.MaPhieu);
            }
            if (model.CTPT.Count == 0)
            {
                model.CTPT.Add(new Core.Domain.ChiTietPhieuChuyenTienNoiBo());
            }
            model.VTKTNH = _chuyenTienService.GetVTKTNH();
        }
        [HttpPost]
        public ActionResult Create(PhieuChuyenTienNoiBo item, List<ChiTietPhieuChuyenTienNoiBo> httt, string submit)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Title = "Tạo mới chuyển tiền nội bộ";
            string xml = "<Action>ChuyenTienCreate</Action>";
            if (item != null)
                xml += TNK.Core.Common.ConvertObjectToXMLString(item);
            if (httt != null)
            {
                xml += Environment.NewLine + "<ChiTietPhieuChuyenTienNoiBo>";
                foreach (ChiTietPhieuChuyenTienNoiBo ct in httt)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                xml += Environment.NewLine + "</ChiTietPhieuChuyenTienNoiBo>";
            }           
            xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
            string message = _chuyenTienService.CreatePhieu(item, httt, xml);
            if (string.IsNullOrEmpty(message))
            {
                ViewBag.Alert = "Tạo mới thành công";
            }
            else
            {
                ViewBag.Alert = "Tạo mới thất bại:" + message;
                TempData["Error"] = message;
            }
            if (submit.Equals("Lưu"))
                return Redirect("/ChuyenTien/Index");
            else
                return Redirect("/ChuyenTien/Create");
        }

        public ActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật chuyển tiền nội bộ";
            ViewBag.Action = "View";
            var model = new PhieuChuyenTienModel();
            model.Item = _chuyenTienService.GetPhieuChuyenTienNoiBo(id);
            GetModel(model);
            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Edit(PhieuChuyenTienNoiBo item, List<ChiTietPhieuChuyenTienNoiBo> httt, string submit)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Title = "Cập nhật chuyển tiền nội bộ";
            string message = _chuyenTienService.UpdatePhieu(item, httt);
            if (string.IsNullOrEmpty(message))
            {
                TempData["Alert"] = "Cập nhật thành công";
                return Redirect(Request.RawUrl);
            }
            else
            {
                ViewBag.Alert = "Cập nhật thất bại:" + message;
                TempData["Alert"] = "Cập nhật thất bại:" + message;
            }
            return View();
        }

        public ActionResult Delete(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string message = _chuyenTienService.DeletePhieu(id);
            if (string.IsNullOrEmpty(message))
            {
                TempData["Alert"] = "Xóa thành công";
            }
            else
            {
                ViewBag.Alert = "Xóa thất bại:" + message;
                TempData["Error"] = message;
            }
                return Redirect("/ChuyenTien/Index");
        }

        protected override void InvokeAction()
        {
            this._chuyenTienService.SetIPClient(GetIPClient());
            this._chuyenTienService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
        public ActionResult Export_Excel(DateTime FromDate, DateTime ToDate, string Query)
        {
            var excel = new ExcelPackage();
            XuatExcel(FromDate, ToDate, Query, ref excel);
            string strFileName = "Danh_Sach_Phieu_Thu_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".xls";
            return new ExcelInsuranceDownload(excel, strFileName);
        }

        public void XuatExcel(DateTime FromDate, DateTime ToDate, string Query, ref ExcelPackage pck)
        {
            string format = "#,##0";
            DataTable data = new DataTable();
            List<string> Mang = new List<string>() {"Số Tiền", "Số tiền", "Số tiền đã sử dụng" , "Trả cọc", "Còn lại",
                "Tổng cộng", "Sử dụng", "Đã trả", "Còn nợ","Đã thanh toán" ,"Giá bán",
            "Giảm giá","Thực thu","Tổng nợ","Thanh toán","Số tiền thu","Tổng phải thu","Thanh toán ngân hàng","Thanh toán tiền mặt"};
            string tenFile = FromDate.Day + "/" + FromDate.Month + "/" + FromDate.Year + "_" + ToDate.Day + "/" + ToDate.Month + "/" + ToDate.Year;
            data = _chuyenTienService.XuatExcel(FromDate, ToDate, Query);
            string TuNgay = FromDate.Day + "/" + FromDate.Month + "/" + FromDate.Year;
            string DenNgay = ToDate.Day + "/" + ToDate.Month + "/" + ToDate.Year;
            var sheet = pck.Workbook.Worksheets.Add("Báo_Cáo_Phiếu_Thu_Chuyển_Tiền_Nội_Bộ_");
            #region báo cáo phiếu thu
            int cotSoTien = 0;
            DataRow rTongCong = data.NewRow();
            for (int i = 0; i < data.Columns.Count; i++)
            {

                if (Mang.Contains(data.Columns[i].ColumnName))
                {
                    if (cotSoTien == 0) cotSoTien = i;
                    rTongCong[i] = 0;
                }
            }

            int soCot = data.Columns.Count;
            List<string> lstTenCot = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            //lstTenCot.AddRange()

            var cellA1P1 = sheet.Cells["A1:" + lstTenCot[soCot] + "1"];
            cellA1P1.Value = "DANH SÁCH PHIẾU THU";
            cellA1P1.Merge = true;
            cellA1P1.Style.Font.Bold = true;
            cellA1P1.Style.Font.Name = "Arial";
            cellA1P1.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellA1P1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
            cellA1P1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cellA1P1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cellA1P1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cellA1P1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            var cellA2P2 = sheet.Cells["A2:" + lstTenCot[soCot] + "2"];
            cellA2P2.Value = "Từ ngày " + TuNgay + " Đến ngày " + DenNgay;
            cellA2P2.Merge = true;
            cellA2P2.Style.Font.Bold = true;
            cellA2P2.Style.Font.Name = "Arial";
            cellA2P2.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellA2P2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
            cellA2P2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cellA2P2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cellA2P2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cellA2P2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            var cellA2 = sheet.Cells["A3"];
            cellA2.Value = "STT";
            cellA2.Style.Font.Bold = true;
            cellA2.Style.Font.Name = "Arial";
            cellA2.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellA2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
            cellA2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cellA2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cellA2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cellA2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            //tao header
            for (int i = 0; i < data.Columns.Count; i++)
            {
                var cell = sheet.Cells[lstTenCot[i + 1] + "3"];
                cell.Value = data.Columns[i].ColumnName;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            }
            //xuat du lieu
            for (int r = 0; r < data.Rows.Count; r++)
            {
                //dua du lieu vao cot so thu tu
                var cellIndex = sheet.Cells["A" + (r + 4)];
                cellIndex.Value = r + 1;
                cellIndex.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellIndex.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cellIndex.AutoFitColumns();
                //dua du lieu vao tung cell
                for (int c = 0; c < data.Columns.Count; c++)
                {
                    var cellData = sheet.Cells[lstTenCot[c + 1] + (r + 4)];
                    cellData.Value = data.Rows[r][c];
                    if (Mang.Contains(data.Columns[c].ColumnName))
                    {
                        rTongCong[c] = double.Parse(rTongCong[c].ToString()) + float.Parse(data.Rows[r][c].ToString());
                    }

                    if (data.Columns[c].DataType == typeof(String))
                    {
                        cellData.Style.WrapText = true;
                        cellData.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cellData.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }
                    if (
                        data.Columns[c].DataType == typeof(float)
                        || data.Columns[c].DataType == typeof(double)
                        || data.Columns[c].DataType == typeof(decimal)
                        )
                    {
                        cellData.Style.Numberformat.Format = format;
                        cellData.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cellData.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    cellData.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    cellData.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    cellData.Style.Font.Name = "Arial";
                    cellData.AutoFitColumns();
                }
            }
            for (int c = 0; c < data.Columns.Count; c++)
            {
                var cellData = sheet.Cells[lstTenCot[c + 1] + (data.Rows.Count + 4)];
                cellData.Value = rTongCong[c];

                if (data.Columns[c].DataType == typeof(String))
                {
                    cellData.Style.WrapText = true;
                    cellData.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cellData.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                }
                if (
                    data.Columns[c].DataType == typeof(float)
                    || data.Columns[c].DataType == typeof(double)
                    || data.Columns[c].DataType == typeof(decimal)
                    )
                {
                    cellData.Style.Numberformat.Format = format;
                    cellData.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cellData.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                cellData.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellData.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cellData.Style.Font.Bold = true;
                cellData.Style.Font.Name = "Arial";
                cellData.AutoFitColumns();
            }
            var sheetTong = sheet.Cells["A" + (data.Rows.Count + 4) + ":" + lstTenCot[cotSoTien] + (data.Rows.Count + 4)];
            sheetTong.Value = "Tổng cộng";
            sheetTong.Merge = true;
            sheetTong.Style.Font.Bold = true;
            sheetTong.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells.AutoFitColumns();
            #endregion
        }
    }
}