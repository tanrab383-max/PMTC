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
using TNK.Core.Domain.View;
using TNK.Services.KhoXe;
using TNK.Services.No;
using TNK.Services.NhanVien;
using TNK.Services.Log;
using TNK.Core;
using OfficeOpenXml;
using TNK.Helper;
using System.Data;
using OfficeOpenXml.Style;
using TNK.Core.Data;
using System.Collections;
using System.IO;
using TNK.Services.Authentication;
using System.Globalization;
using System.Configuration;
using TNK.Data;
using log4net;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using System.Web.Management;

namespace TNK.Controllers
{
    public class SoChiController : BasePublicController
    {
        IUserervice _userService;
        ICategoryService _categoryService;
        ISoChiService _soChiService;
        ISoThuService _soThuService;
        IDoiTacService _doiTacService;
        IRepository<PhieuChi> _PhieuChiRepository;
        IRepository<DoiTac> _DoiTacRepository;
        //INhanVienService _NhanVienService;
        IPhieuThuService _phieuThuService;
        IChiTietNhapKhoPhuTungService _chiTietNhapKhoPhuTungService;
        ITuiDinhKhoanService _tuiDinhKhoanService;
        IKhoTaiSanService _khoTaiSanService;
        ILoaiXeService _loaiXeService;
        INoPhaiTraService _noPhaiTraService;
        //ILogger _logger;
        IRepository<KhoPhuTung> _KhoPhuTungRepository;
        IKhoPhuTungService _khoPhuTungService;
        IAuthenticationService _authenticationService;
        IUserRegistrationService _userRegistrationService;
        User CurrentUser;
        string ConnectionString = "";
        void getConnectionString()
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            string connectString = TNKObjectContext.GetConnectionString();
            if (settings != null && connectString == null)
            {
                int count = settings.Count;
                ConnectionString = settings[count - 1].ConnectionString;
            }
            else
                ConnectionString = connectString;
        }
        public SoChiController(IUserervice _userService
            , ICategoryService _categoryService
            , ISoChiService _soChiService
            , IPhieuThuService _phieuThuService
            , ILoaiXeService _loaiXeService
            , IDoiTacService _doiTacService
            , IChiTietNhapKhoPhuTungService _chiTietNhapKhoPhuTungService
            , ITuiDinhKhoanService _tuiDinhKhoanService
            , IKhoTaiSanService _khoTaiSanService
            , HttpContextBase _httpContext
            , ISoThuService _soThuService
            , INoPhaiTraService _noPhaiTraService
            , ILogger _logger
            , IRepository<PhieuChi> _PhieuChiRepository
            , IRepository<DoiTac> _DoiTacRepository
            , IRepository<KhoPhuTung> _KhoPhuTungRepository
            , IKhoPhuTungService _khoPhuTungServices
            , IAuthenticationService _authenticationService
            , IUserRegistrationService _userRegistrationService
            ) : base(_logger)//base(_userService,_userRegistrationService,_logger )
        {
            this._userService = _userService;
            this._categoryService = _categoryService;
            this._loaiXeService = _loaiXeService;
            this._chiTietNhapKhoPhuTungService = _chiTietNhapKhoPhuTungService;
            this._httpContext = _httpContext;
            this._soChiService = _soChiService;
            this._tuiDinhKhoanService = _tuiDinhKhoanService;
            this._phieuThuService = _phieuThuService;
            this._soThuService = _soThuService;
            this._khoTaiSanService = _khoTaiSanService;
            this._noPhaiTraService = _noPhaiTraService;
            this._logger = _logger;
            this._PhieuChiRepository = _PhieuChiRepository;
            this._DoiTacRepository = _DoiTacRepository;
            this._doiTacService = _doiTacService;
            this._KhoPhuTungRepository = _KhoPhuTungRepository;
            this._khoPhuTungService = _khoPhuTungService;
            this._userRegistrationService = _userRegistrationService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
            getConnectionString();
        }


        #region Phan loai no
        /* === tuy theo loai no ma hien thi tieu de Danh sach no, Khach hang no tuong ung*/
        string GetTenLoaiNo(string maLoaiNo)
        {
            switch (maLoaiNo)
            {
                case "NGCNG": return "gia công ngoài";
                case "HHTXE": return "hoa hồng tài xế";
                case "CHHBX":
                    return "hoa hồng bán xe";
                case "CTHBH": return "thu hộ bảo hiểm";
                case "NTHBH": return "thu hộ bảo hiểm";
                case "NTHPK": return "thu hộ phụ kiện";
                case "NMUTS": return "mua tài sản";
                case "NVAMU": return "vay mượn";
                case "NPTPK": return "phụ tùng phụ kiện";
                case "NMPTU": return "mua phụ tùng - phụ kiện";

                case "NTASA": return "tài sản";
                case "NTIVA": return "tiền vay";
                case "NMXTT": return "mua xe";
                case "NBHBX": return "bảo hiểm bán xe";
                case "NTKHA": return "khác";
                case "NBHXH": return "BHXH";
                case "N2GTX": return "thu hộ giấy tờ xe";
                case "N2THK": return "thu hộ khác";
                case "NKHAC": return "khác";
                case "K1GHB": return "khuyến mãi gia hạn bảo hành";
                case "N1GHB": return "gia hạn bảo hành";
                case "NBDTK": return "bảo dưỡng tiết kiệm";
                case "NKMPK": return "Khuyến mãi phụ kiện - phụ tùng";
                case "NTOHO": return "tổng hợp";
            }
            return "";
        }
        string GetKhachHangNoTitle(string maLoaiNo)
        {
            switch (maLoaiNo)
            {
                case "NGCNG": return "Khách hàng";
                case "NMXTT": return "Đối tác";
                case "HHTXE":
                case "CHHBX": return "Khách hàng";
                case "CTHBH":
                case "NTHBH":
                case "NBHBX": return "Công ty bảo hiểm";
                case "NTHPK":
                case "NPTPK": return "Nhà cung cấp";
                case "NTASA": return "Đơn vị";
                case "NTIVA": return "Ngân hàng";
                case "NKHAC": return "Khách hàng";
                case "N2GTX": return "Đối tác";
                default:
                    return "Khách hàng";

            }
        }

        string HienThiNguoiBaoLanhNo(string maLoaiNo)
        {
            switch (maLoaiNo)
            {
                case "NNHBX":
                case "NTAUN":
                case "NBHBX":
                case "NBHDV": return "none";
                case "NBAXE":
                case "NDVPT":
                case "NPTPK": return "display";
            }
            return "";
        }

        void CreateLink(string maLoaiPhieu, string action)
        {
            ViewBag.LinkCreate = "/SoChi/" + maLoaiPhieu + "Create";
            ViewBag.LinkDelete = "/SoChi/" + maLoaiPhieu + "Delete";
            ViewBag.LinkEdit = "/SoChi/" + maLoaiPhieu + "Edit";
            if (maLoaiPhieu == "CCOMX")
            {
                ViewBag.LinkIndex = "/SoChi/CocMuaXe";
            }
            else if (maLoaiPhieu == "CCOPT")
            {
                ViewBag.LinkIndex = "/SoChi/CocMuaPhuTung";
            }
            else if (maLoaiPhieu == "CCOGC")
            {
                ViewBag.LinkIndex = "/SoChi/CocGCN";
            }
            else if (maLoaiPhieu == "CCOTH")
            {
                ViewBag.LinkIndex = "/SoChi/CocTH";
            }
            else if (maLoaiPhieu == "CCOTS")
            {
                ViewBag.LinkIndex = "/SoChi/CocMuaTaiSan";
            }
            else
            {
                ViewBag.LinkIndex = "/SoChi/" + maLoaiPhieu + "Index";
            }
            switch (maLoaiPhieu)
            {
                case "CHCMX":
                    ViewBag.Title = " hoàn cọc mua xe";
                    break;
                case "CHCDV":
                    ViewBag.Title = " hoàn cọc dịch vụ";
                    break;
                case "CTAUN":
                    ViewBag.Title = " tạm ứng";
                    break;
                case "CMXTT":
                    ViewBag.Title = " mua xe thực tế";
                    break;
                case "CMHTD":
                    ViewBag.Title = " mua hàng trên đường";
                    break;
                case "CMPTU":
                    ViewBag.Title = " mua phụ tùng";
                    break;
                case "CVAMU":
                    ViewBag.Title = " cho vay mượn";
                    break;
                case "CMUTS":
                    ViewBag.Title = " mua tài sản";
                    break;
                case "CTTVA":
                    ViewBag.Title = " trả tiền vay";
                    break;
                case "CDATU":
                    ViewBag.Title = " đầu tư";
                    break;
                case "CTOHO":
                    ViewBag.Title = " tổng hợp";
                    break;
                case "CDACO":
                    ViewBag.Title = " cọc mua tài sản";
                    break;
                case "CKHAC":
                    ViewBag.Title = " khác";
                    break;
                case "CHIHO":
                    ViewBag.Title = " hộ";
                    break;
            }
            switch (action.ToLower())
            {
                case "index":
                    ViewBag.Title = "Danh sách chi" + ViewBag.Title;
                    break;
                case "create":
                    ViewBag.Title = "Tạo mới phiếu chi" + ViewBag.Title;
                    break;
                case "edit":
                    ViewBag.Title = "Cập nhật phiếu chi" + ViewBag.Title;
                    break;
            }


        }
        #endregion Phan loai no   
        [HttpPost]
        public ActionResult DeletePhieuChi(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Content("Bạn không có quyền xóa");
            }
            string message = _soChiService.DeletePhieuChi(id);
            if (string.IsNullOrEmpty(message))
                return Content("success");
            else
                return Content(message);

        }

        public ActionResult Delete(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Content("Bạn không có quyền xóa");
            }
            //tìm mã phiếu nợ từ id, sau đó từ mã phiếu nợ tìm ra các phiếu nợ ở phía sau:
            //ViewNoDaTra NoPhaiTra = _soChiService.getNoDaTraById(id);
            //string result = _soChiService.getPhieuTraNo(NoPhaiTra);
            //if (result != "")
            //    return Content(result);
            string message = _soChiService.DeletePhieuChi(id);
            if (string.IsNullOrEmpty(message))
                return Content("success");
            else
                return Content(message);

        }
        //xoa record hang tren duong
        [HttpPost]
        public string DeleteHTD(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return "Bạn không có quyền thao tác!";
            }
            string message = _soChiService.DeletePhieuChi(id);
            if (string.IsNullOrEmpty(message))
            {
                // TempData["Alert"] = "Xóa thành công";
                return "";
            }
            else
            {
                // TempData["Alert"] = "Xóa thất bại";
                return message;
            }
            //if (!string.IsNullOrEmpty(_soChiService.DeletePhieuChi(id,true)))
            //{
            //    TempData["Alert"] = "Xóa thành công";
            //    return true;
            //}
            //else
            //{
            //    TempData["Alert"] = "Xóa thất bại";
            //    return false;
            //}
        }

        void SetSearch(int p, string from, string to, string query, string id, int pageSize = 30)
        {
            ViewBag.Total = total;
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            // ViewBag.Alert = TempData["Alert"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Type = id;
            ViewBag.Url = Request.Url.AbsolutePath + "?id=" + id + "&query=" + query + "&from=" + from + "&to=" + to + "&p=";
        }

        void SetSearch1(int p, string from, string to, string query, string id)
        {
            ViewBag.Total = total;
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            // ViewBag.Alert = TempData["Alert"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Type = id;
            ViewBag.Url = Request.Url.AbsolutePath + "?id=" + id + "&query_complete=" + query + "&from=" + from + "&to=" + to + "&p=";
        }

        void SetDate(ref string from, ref string to, ref DateTime f, ref DateTime t, ref string query_Complete, string type, int p = 1)
        {

            var cookie = Request.Cookies.Get(type);
            try
            {
                if (from == "" && to == "" && query_Complete == "")
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
                        cookie.Values["query"] = query_Complete;
                        _httpContext.Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        from = cookie.Values["from"];
                        to = cookie.Values["to"];
                        query_Complete = cookie.Values["query"];
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
                    cookie.Values["query"] = query_Complete;

                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                    _httpContext.Response.Cookies.Set(cookie);
                    Request.Cookies.Remove(type); //remove cookie sau khi su dung
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Alert = TempData["Alert"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query_Complete;
            ViewBag.Url = Request.Url.AbsolutePath + "?id=" + type + "&query_complete=" + query_Complete + "&from=" + from + "&to=" + to + "&p=";
        }
        //phan truc them 
        public void SetSearch1(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string type, ref string tinhtrang, int pageSize = 30)
        {

            var cookie = Request.Cookies.Get(type);
            try
            {
                if (from == "" && to == "" && query == "" && tinhtrang == "")
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
                        cookie.Values["tinhTrang"] = query;
                        _httpContext.Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        from = cookie.Values["from"];
                        to = cookie.Values["to"];
                        query = cookie.Values["query"];
                        tinhtrang = cookie.Values["tinhtrang"];
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
                    cookie.Values["tinhtrang"] = tinhtrang;


                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                    _httpContext.Response.Cookies.Set(cookie);
                    Request.Cookies.Remove(type); //remove cookie sau khi su dung
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
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&tinhtrang=" + tinhtrang + "&p=";
        }
        public ActionResult NoItem()
        {
            return View();
        }
        public SoChiModel GetModel(string code, bool isForInsert = true)
        {
            SoChiModel model = new SoChiModel();
            try
            {

                switch (code)
                {
                    case "CMHTD":
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        model.DoiTac = model.DoiTac.Where(x => x.MaDoiTac == "HVN" || x.MaDoiTac == "TOYOTA" || x.MaDoiTac == "MMV").ToList();
                        break;
                    case "CMXTT":
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        model.MaXe = _loaiXeService.GetViewLoaiXeList();
                        break;
                    case "CMPTU":
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        model.KhoPhuTung = _tuiDinhKhoanService.GetKhoNhapPhuTung(isForInsert);
                        model.ListCTNKPT = new List<ChiTietNhapKhoPhuTung>();
                        model.ListCTNKPT.Add(new ChiTietNhapKhoPhuTung());
                        break;
                    case "CDATU":
                        model.DoiTac = _soChiService.GetDoiTac("NDT");
                        model.DuAnDauTu = _soChiService.GetDuAnDauTu("DUAN");
                        break;
                    case "CTOHO":
                        model.LyDoChi = _soChiService.GetLyDoChi("CKA");
                        model.PhongBanChi = _categoryService.GetCategoryItem("PBAN");
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        break;
                    case "CMUTS":
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        model.TenTaiSan = _soChiService.GetTenTaiSan("TAISA");
                        break;
                    case "CDACO":
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        model.TenTaiSan = _soChiService.GetTenTaiSan("TAISA");
                        break;
                    case "CCOMX":
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        break;
                    case "CKHAC":
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        //model.LyDoChi = _categoryService.GetCategoryItem("LOAICHIKHAC");
                        model.LyDoChi = _soChiService.GetLyDoChi("CPKHAC");
                        break;
                    case "CHIHO":
                        model.DoiTac = _soChiService.GetDoiTac("NCC");
                        //model.LyDoChi = _categoryService.GetCategoryItem("LOAICHIKHAC");
                        model.LyDoChi = _soChiService.GetLyDoChi("CHIHO");
                        break;

                }
                model.Users = _userService.get();
                model.HTTT = _soChiService.GetHTTT();
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.GetModel(" + code + ")", ex);
            }
            return model;
        }
        #region Danh sach phieu chi
        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1, string maLoaiPhieu = "", string chonngay = "NC")
        {
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearchPhieuThu_Chi(ref from, ref to, ref f, ref t, p, ref query, maLoaiPhieu, chonngay);
                var model = _soChiService.GetListPhieuChi(f, t, query, p, ref total, pageSize, maLoaiPhieu, chonngay);
                ViewBag.Total = total;
                List<DanhMucDienGiai> LoaiPhieuChi = _soChiService.LoaiPhieuChi();
                ViewBag.LoaiPhieuChi = LoaiPhieuChi;
                ViewBag.maLoaiPhieu = maLoaiPhieu;
                return View("ListPhieuChi", model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.Index", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        #endregion
        #region Chi mua phụ tùng
        public ActionResult CMPTUIndex(string from = "", string to = "", string query = "", int p = 1, int pageSize = 30)
        {
            // ViewBag.Alert = TempData["Alert"];
            /*ViewBag.LinkCreate = "/SoChi/CMPTUCreate";
            ViewBag.LinkDelete = "/SoChi/CMPTUDelete";
            ViewBag.LinkEdit = "/SoChi/CMPTUEdit";
            ViewBag.LinkIndex = "/SoChi/CMPTUIndex";
            */
            try
            {
                CreateLink("CMPTU", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CMPTU");
                // var model = _soChiService.GetPhieuChiList("CMPTU", f, t, query, p, ref total, pageSize);
                List<ViewDanhSachPhieuNhapKhoPhuTung> model = _soChiService.GetListChiMuaPhuTung(f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                ViewBag.PageSize = pageSize;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMPTUIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CMPTUTemp(string from = "", string to = "", string query = "", int p = 1, int pageSize = 30)
        {
            try
            {
                CreateLink("CMPTU", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CMPTU");
                var model = _soChiService.GetListCMPTUTemp( f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                ViewBag.PageSize = pageSize;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMPTUIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]        
        public ActionResult CMPTU_DongBoTuCyber(string maHS, string soChungTu)
        {
            try
            {
                string[] lst = soChungTu.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
               
                int totalSuccess = 0;
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string error = "";
                try
                {
                    //lay danh sach phieu chi mua phu tung tu Cyber
                    DataTableCollection ds = _soChiService.GetListCMPTUChoDongBoCyber(ConnectionString,  soChungTu);
                    DataTable dsPhieuNhap = ds[0];
                    //lay danh sach chi tiet phieu chi mua phu tung tu Cyber
                    DataTable dsChiTiet = ds[1];
                    //import dữ lieu vào database


                    List<PhieuChi> lstPhieuChi = new List<Core.Domain.PhieuChi>();
                    List<List<ChiTietPhieuChi>> lstChiTietPhieuChi = new List<List<ChiTietPhieuChi>>();
                    List<List<ChiTietNhapKhoPhuTung>> ListCTNKPT = new List<List<ChiTietNhapKhoPhuTung>>();
                    List<ChiTietPhieuChi> Coc = new List<ChiTietPhieuChi>();
                    int beginRowNum = 1;
                    string SoChungTu = "";
                    double SoDongCuaFileExcel = 0;
                    int dem = 0;
                    foreach (DataRow r in dsPhieuNhap.Rows)
                    {
                        PhieuChi item = new PhieuChi();
                        List<ChiTietPhieuChi> CTPC = new List<ChiTietPhieuChi>();
                        List<ChiTietNhapKhoPhuTung> CTNKPT = new List<ChiTietNhapKhoPhuTung>();
                        item.MaLoaiPhieu = "CMPTU";
                        item.SoChungTu = r["so_ct"].ToString();//do so chung tu co the trung nen phai them ma_hs
                        item.NgayChi = Convert.ToDateTime(r["ngay_ct"].ToString());
                        item.NgayHachToan = Convert.ToDateTime(r["ngay_ct"].ToString());
                        item.NguoiThuTien = CurrentUser.UserId;
                        item.GhiChu = r["Dien_giai"].ToString();
                        item.NgayNhapKho = Convert.ToDateTime(r["ngay_ct"].ToString());                       
                        item.NoiDung = r["Dien_giai"].ToString();
                        item.TongCong = Convert.ToDouble(r["ThanhTien"].ToString());
                        item.DoiTac = r["MaDoiTac"].ToString();
                        item.NhaCungCap = r["MaDoiTac"].ToString();
                        item.CyberId = r["CyberId"].ToString();
                       
                        //phan phieu nhap kho
                        CTNKPT.Add(new ChiTietNhapKhoPhuTung());
                        CTNKPT[0].MaKho = r["MaKho"].ToString().ToUpper().Trim();
                        CTNKPT[0].NgayNhapKho = Convert.ToDateTime(r["ngay_ct"].ToString());
                        CTNKPT[0].SoTien = Convert.ToDouble(r["ThanhTien"].ToString());
                        CTNKPT[0].SoLuong = Convert.ToDouble(r["SoLuong"].ToString());
                        lstPhieuChi.Add(item);
                        lstChiTietPhieuChi.Add(CTPC);
                        ListCTNKPT.Add(CTNKPT);
                        dem++;

                    }

                    for (int i = 0; i < lstPhieuChi.Count; i++)
                    {
                        string xml = "<formdata><Action>PhieuThu</Action>";
                        xml += Environment.NewLine + "<submit>Đồng bộ từ Cyber</submit>";
                        if (lstPhieuChi[i] != null)
                            xml += TNK.Core.Common.ConvertObjectToXMLString(lstPhieuChi[i]);
                        if (lstChiTietPhieuChi[i] != null)
                        {
                            xml += Environment.NewLine + "<ThanhToan>";
                            foreach (ChiTietPhieuChi ct in lstChiTietPhieuChi[i])
                                xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                            xml += Environment.NewLine + "</ThanhToan>";
                        }
                        if (ListCTNKPT != null)
                        {
                            xml += Environment.NewLine + "<ChiTietNhapKhoPhuTung>";
                            foreach (ChiTietNhapKhoPhuTung ctknpt in ListCTNKPT[i])
                                xml += TNK.Core.Common.ConvertObjectToXMLString(ctknpt);
                            xml += Environment.NewLine + "</ChiTietNhapKho>";
                        }
                        xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
                        xml += "</formdata>";
                        //_phieuThuService.CreatePhieuThu(item, httt, false, xml);
                        //_soChiService.CreatePhieuChi(lstPhieuChi[i], lstChiTietPhieuChi[i], ListCTNKPT[i], CTPC, true, xml)
                        string messagePC = _soChiService.CreatePhieuChi_ImportCMPT(lstPhieuChi[i], lstChiTietPhieuChi[i], ListCTNKPT[i], Coc, true, xml);
                        if (!string.IsNullOrEmpty(messagePC))
                            error += Environment.NewLine + "Dòng " + (i + 1) + "(" + SoChungTu + "):" + messagePC;
                    }

                    
                }
                catch (Exception objEx)
                {
                    _logger.WriteLog("SoThuController.CMPTU_DongBoTuCyber:" + objEx);
                    return Json(new { IsError = true, Message = "Lỗi đồng bộ dữ liệu." });
                }
                if (string.IsNullOrEmpty(error))
                {
                    return Json(new
                    {
                        IsError = false,
                        Message = error
                    });
                }
                else
                {
                    _logger.WriteLog("SoThuController.CMPTU_DongBoTuCyber:" + error);
                    return Json(new { IsError = true, Message = error });
                }
                return Json(new { IsError = true, Message = "Đã tạo mới " + totalSuccess + " phiếu nhập kho. " });
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMPTU_DongBoTuCyber", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
                return Json(new { IsError = true, Message = "Lỗi khi đồng bộ phụ tùng từ Cyber qua PMTC." + ex.Message });
                //return "Lỗi khi đồng bộ phụ tùng từ Cyber qua PMTC." + ex.Message;
            }
           // return "";
        }
        #region Chi khác
        public ActionResult CKHACIndex(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                CreateLink("CKHAC", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CKHAC");
                var model = _soChiService.GetPhieuChiList("CKHAC", f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CKHACIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CKHACCreate()
        {
            try
            {
                CreateLink("CKHAC", "create");
                SoChiModel model = GetModel("CKHAC", true);
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CKHACCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CKHACCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                string xml = "<Action>CKHACCreate</Action>";
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

                ViewBag.Title = "Tạo phiếu chi khác";
                item.MaLoaiPhieu = "CKHAC";
                //item.NgayChi = DateTime.Now;
                //item.NgayHachToan = DateTime.Now;
                CreateLink("CKHAC", "create");
                if (_soChiService.CreatePhieuChi(item, httt, true, xml) == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    ViewBag.Error = "Tạo mới thất bại";
                    TempData["Error"] = "Tạo mới thất bại";
                }
                SoChiModel model = GetModel("CKHAC");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CKHACCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CKHACEdit(string id, string action = "", string a = "")
        {
            try
            {
                ViewBag.Title = "Cập nhật phiếu chi khác";
                CreateLink("CKHAC", "edit");
                SoChiModel model = GetModel("CKHAC", false);
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null || model.Item.MaLoaiPhieu != "CKHAC")
                {
                    TempData["Error"] = "Không tìm thấy mã phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                //return Redirect(ViewBag.LinkIndex);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CKHACEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CKHACEdit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                string xml = "<Action>CKHACEdit</Action>";
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

                ViewBag.Title = "Cập nhật phiếu chi khác";
                CreateLink("CKHAC", "edit");
                string sessionId = "";
                string message = _soChiService.UpdatePhieuChi(item, httt, ref sessionId, true, xml);
                if (string.IsNullOrEmpty(message))
                {

                    TempData["Info"] = "Cập nhật thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Cập nhật thất bại:" + message;
                }
                SoChiModel model = GetModel("CKHAC");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CKHACEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }


        #endregion

        #region CHI HO - THU HO
        public ActionResult CHIHOIndex(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                CreateLink("CHIHO", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CHIHO");
                var model = _soChiService.GetPhieuChiList("CHIHO", f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CHIHOIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CHIHOCreate()
        {
            try
            {
                CreateLink("CHIHO", "create");
                SoChiModel model = GetModel("CHIHO", true);
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CHIHOCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CHIHOCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                string xml = "<Action>CHIHOCreate</Action>";
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

                ViewBag.Title = "Tạo phiếu chi khác";
                item.MaLoaiPhieu = "CHIHO";
                //item.NgayChi = DateTime.Now;
                //item.NgayHachToan = DateTime.Now;
                CreateLink("CHIHO", "create");
                if (_soChiService.CreatePhieuChi(item, httt, true, xml) == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    ViewBag.Error = "Tạo mới thất bại";
                    TempData["Error"] = "Tạo mới thất bại";
                }
                SoChiModel model = GetModel("CHIHO");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CHIHOCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CHIHOEdit(string id, string action = "", string a = "")
        {
            try
            {
                ViewBag.Title = "Cập nhật phiếu chi khác";
                CreateLink("CHIHO", "edit");
                SoChiModel model = GetModel("CHIHO", false);
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null || model.Item.MaLoaiPhieu != "CHIHO")
                {
                    TempData["Error"] = "Không tìm thấy mã phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                //return Redirect(ViewBag.LinkIndex);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CHIHOEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CHIHOEdit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                string xml = "<Action>CHIHOEdit</Action>";
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

                ViewBag.Title = "Cập nhật phiếu chi khác";
                CreateLink("CHIHO", "edit");
                string sessionId = "";
                string message = _soChiService.UpdatePhieuChi(item, httt, ref sessionId, true, xml);
                if (string.IsNullOrEmpty(message))
                {

                    TempData["Info"] = "Cập nhật thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Cập nhật thất bại:" + message;
                }
                SoChiModel model = GetModel("CHIHO");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CHIHOEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CHIHODelete(string id)
        {
            try
            {
                CreateLink("CHIHO", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    TempData["Info"] = "Xóa thành công";
                else
                    TempData["Error"] = "Xóa thất bại:" + message;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CHIHODelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        #endregion
        public ActionResult CMPTUCreate(string id)
        {
            try
            {
                CreateLink("CMPTU", "create");
                SoChiModel model = GetModel("CMPTU", true);
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.Item = new PhieuChi();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    string code = Common.DecodeHtmlUrl(id);
                    DataTableCollection dataTableCollection = _soChiService.GetListCMPTUChiTietTemp(ConnectionString, code);
                   var header= dataTableCollection[0].AsEnumerable().Select(row => new {
                       CyberId = row.Field<string>("CyberId"),
                       So_ct = row.Field<string>("So_ct"),
                       Ngay_ct = row.Field<DateTime>("Ngay_ct"),
                       ThanhTien = row.Field<decimal>("ThanhTien"),
                       Dien_giai = row.Field<string>("Dien_giai"),
                       DoiTac = row.Field<string>("MaDoiTac"),
                   }).FirstOrDefault();

                    var details = dataTableCollection[1].AsEnumerable().Select(row => new ChiTietNhapKhoPhuTung
                    {
                        MaKho = row.Field<string>("MaKhoPMTC"),
                        SoLuong = (double)row.Field<decimal>("SoLuong"),
                        SoTien = (double)row.Field<decimal>("GiaNhap"),
                    }).ToList();
                    model.Item = new PhieuChi
                    {
                        CyberId = header.CyberId,
                        SoChungTu = header.So_ct,
                        NgayChi = header.Ngay_ct,
                        NgayHachToan = header.Ngay_ct,
                        NoiDung = header.Dien_giai,
                        TongCong = (double)header.ThanhTien,
                        SoTienChi = (double)header.ThanhTien,
                        DoiTac = header.DoiTac,
                    };
                    model.ListCTNKPT = details;
                }
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMPTUCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CMPTUCreate(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietNhapKhoPhuTung> ListCTNKPT, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CMPTUCreate</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);
                if (httt != null)
                {
                    xml += Environment.NewLine + "<ThanhToan>";
                    foreach (ChiTietPhieuChi ct in httt)
                        xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                    xml += Environment.NewLine + "</ThanhToan>";
                }
                if (ListCTNKPT != null)
                {
                    xml += Environment.NewLine + "<ChiTietNhapKho>";
                    foreach (ChiTietNhapKhoPhuTung ct in ListCTNKPT)
                        xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                    xml += Environment.NewLine + "</ChiTietNhapKho>";
                }
                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";

                ViewBag.Title = "Tạo phiếu mua phụ tùng";
                item.MaLoaiPhieu = "CMPTU";
                //item.NgayChi = DateTime.Now;
                //item.NgayHachToan = DateTime.Now;
                CreateLink("CMPTU", "create");
                string message = _soChiService.CreatePhieuChi(item, httt, ListCTNKPT, CTPC, true, xml);
                if (string.IsNullOrEmpty(message))
                {

                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    ViewBag.Error = "Tạo mới thất bại:" + message;
                    TempData["Error"] = "Tạo mới thất bại:" + message;
                }
                SoChiModel model = GetModel("CMPTU");
                model.Item = item;
                model.CTPT = httt;
                model.ListCTNKPT = ListCTNKPT;
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMPTUCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CMPTUEdit(string id, string action = "", string a = "")
        {
            try
            {
                ViewBag.Title = "Cập nhật phiếu mua phụ tùng";
                CreateLink("CMPTU", "edit");
                SoChiModel model = GetModel("CMPTU", false);
                model.Item = _soChiService.GetPhieuChi(id);
                model.Item.TongCong = Math.Round(model.Item.TongCong);
                model.ListCTNKPT = _chiTietNhapKhoPhuTungService.GetCTNKPT(id);
                foreach (var item in model.ListCTNKPT)
                {
                    item.SoTien = Math.Round(item.SoTien);
                }
                if (model.ListCTNKPT.Count == 0)
                    model.ListCTNKPT.Add(new ChiTietNhapKhoPhuTung());

                if (model.Item == null || model.Item.MaLoaiPhieu != "CMPTU")
                {
                    TempData["Error"] = "Không tìm thấy mã phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                //return Redirect(ViewBag.LinkIndex);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMPTUEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CMPTUEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietNhapKhoPhuTung> ListCTNKPT, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CMPTUEdit</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);
                if (httt != null)
                {
                    xml += Environment.NewLine + "<ThanhToan>";
                    foreach (ChiTietPhieuChi ct in httt)
                        xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                    xml += Environment.NewLine + "</ThanhToan>";
                }
                if (ListCTNKPT != null)
                {
                    xml += Environment.NewLine + "<ChiTietNhapKho>";
                    foreach (ChiTietNhapKhoPhuTung ct in ListCTNKPT)
                        xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                    xml += Environment.NewLine + "</ChiTietNhapKho>";
                }
                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";

                ViewBag.Title = "Cập nhật phiếu mua phụ tùng";
                CreateLink("CMPTU", "edit");
                string sessionId = "";
                string message = _soChiService.UpdatePhieuChi(item, httt, ListCTNKPT, CTPC, ref sessionId, true, xml);
                if (string.IsNullOrEmpty(message))
                {

                    TempData["Info"] = "Cập nhật thành công";
                    //ViewBag.Info = "Cập nhật thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Cập nhật thất bại:" + message;
                    //ViewBag.Alert = "Cập nhật thất bại:" + message;
                }
                SoChiModel model = GetModel("CMPTU");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMPTUEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CMPTUDelete(string id)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Content("Bạn không có quyền được xóa");
                }
                CreateLink("CMPTU", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    return Content("success");
                else
                    return Content("Không được phép xóa phiếu vì:" + message);
                //return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMPTUDelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
                return Content(ex.ToString());
            }
            //return View();
        }
        public ActionResult CKHACDelete(string id)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                CreateLink("CKHAC", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    TempData["Info"] = "Xóa thành công";
                else
                    TempData["Error"] = "Xóa thất bại:" + message;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CKHACDelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        #endregion

        #region Chi cho vay mượn
        public ActionResult CVAMUIndex(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                CreateLink("CVAMU", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CVAMU", tinhtrang);
                var model = _soChiService.GetPhieuChiList("CVAMU", f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CVAMUIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CVAMUCreate()
        {
            try
            {
                ViewBag.Title = "Tạo phiếu mua hàng trên đường";
                CreateLink("CVAMU", "create");
                SoChiModel model = GetModel("CVAMU");
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.DoiTac = _soChiService.GetDoiTac_VM("NCC");
                CategoryItem item = new CategoryItem();
                item.Code = "NH";
                item.Name = "Ngắn hạn";
                CategoryItem item2 = new CategoryItem();
                item2.Code = "DH";
                item2.Name = "Dài hạn";
                List<CategoryItem> lst = new List<CategoryItem>();
                lst.Add(item);
                lst.Add(item2);
                model.PhanLoai = lst;

                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CVAMUCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CVAMUCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CVAMUCreate</Action>";
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

                item.MaLoaiPhieu = "CVAMU";
                CreateLink("CVAMU", "create");
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }

                SoChiModel model = GetModel("CVAMU");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CVAMUCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CVAMUEdit(string id, string type = "", string a = "")
        {
            try
            {
                CreateLink("CVAMU", "edit");
                SoChiModel model = GetModel("CVAMU");
                model.Item = _soChiService.GetPhieuChi(id);
                model.DoiTac = _soChiService.GetDoiTac_VM("NCC");
                CategoryItem item = new CategoryItem();
                item.Code = "NH";
                item.Name = "Ngắn hạn";
                CategoryItem item2 = new CategoryItem();
                item2.Code = "DH";
                item2.Name = "Dài hạn";
                List<CategoryItem> lst = new List<CategoryItem>();
                lst.Add(item);
                lst.Add(item2);
                model.PhanLoai = lst;
                if (model.Item == null || model.Item.MaLoaiPhieu != "CVAMU")
                {
                    TempData["Error"] = "Không tìm thấy mã phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CVAMUEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CVAMUEdit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CVAMUEdit</Action>";
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

                CreateLink("CVAMU", "edit");
                ActionResult rs = Update(item, httt, xml);
                if (rs != null)
                    return rs;
                //if (_soChiService.UpdatePhieuChi(item, httt))
                //{
                //    ViewBag.Alert = "Cập nhật thành công";
                //    TempData["Alert"] = "Tạo mới thành công";
                //    return Redirect(ViewBag.LinkIndex);
                //}
                //else
                //    ViewBag.Alert = "Cập nhật thất bại";
                SoChiModel model = GetModel("CVAMU");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CVAMUEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CVAMUDelete(string id)
        {
            try
            {
                CreateLink("CVAMU", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    TempData["Info"] = "Xóa thành công";
                else
                    TempData["Error"] = "Xóa thất bại:" + message;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CVAMUDelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        #endregion

        #region Chi tạm ứng CTAUN
        public ActionResult CTAUNIndex(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                CreateLink("CTAUN", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CTAUN", tinhtrang);
                var model = _soChiService.GetCTAUNList(f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                return View(model);
            }

            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTAUNIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        //phan truc them autocomplete
        public JsonResult AutoComplete()
        {
            List<NhanVien> items = _soChiService.GetNhanVien();
            var list = items.Where(x => x.IsActive == true && x.IsDeleted == false).Select(x => x.HoTen).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CTAUNCreate()
        {
            try
            {
                ViewBag.Title = "Tạo phiếu mua hàng trên đường";
                CreateLink("CTAUN", "create");
                SoChiModel model = GetModel("CTAUN");
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.GetNhanVien = new List<NhanVien>();
                model.GetNhanVien = _soChiService.GetNhanVien();
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTAUNCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CTAUNCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CTAUNCreate</Action>";
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

                item.MaLoaiPhieu = "CTAUN";
                CreateLink("CTAUN", "create");
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Tạo mới thất bại";
                }

                SoChiModel model = GetModel("CTAUN");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTAUNCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CTAUNEdit(string id, string action = "", string a = "")
        {
            try
            {
                CreateLink("CTAUN", "edit");
                SoChiModel model = GetModel("CTAUN");
                model.Item = _soChiService.GetPhieuChi(id);
                model.GetNhanVien = _soChiService.GetNhanVien();
                if (model.Item == null || model.Item.MaLoaiPhieu != "CTAUN")
                {
                    TempData["Error"] = "Không tìm thấy mã phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTAUNEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CTAUNEdit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CTAUNEdit</Action>";
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

                CreateLink("CTAUN", "edit");
                //item.ConLai = item.TongCong;
                ActionResult rs = Update(item, httt, xml);
                if (rs != null)
                    return rs;
                SoChiModel model = GetModel("CTAUN");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTAUNEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CTAUNDelete(string id)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                CreateLink("CTAUN", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    return Content("success");
                else
                    return Content(message);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTAUNDelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
                return Content(ex.ToString());
            }
            return View();
        }


        #endregion

        #region Chi mua xe thực tế CMXTT
        public ActionResult CMXTTIndex(string from = "", string to = "", string query = "", int p = 1, int pageSize = 30)
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                CreateLink("CMXTT", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CMXTT", pageSize);
                var model = _soChiService.GetPhieuChiList("CMXTT", f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMXTTIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult

            CMXTTCreate()
        {
            try
            {
                ViewBag.Title = "Tạo phiếu mua hàng trên đường";
                CreateLink("CMXTT", "create");
                SoChiModel model = GetModel("CMXTT");
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                //model.ChiCoc = _soChiService.GetChiCoc();
                ViewBag.Date = SetDateDefault();
                ViewBag.Type = "CMXTT";
                //ViewBag.Date = DateTime.Now;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMXTTCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CMXTTCreate(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                ViewBag.Date = SetDateDefault1();
                string xml = "<Action>CMXTTCreate</Action>";
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

                item.MaLoaiPhieu = "CMXTT";
                CreateLink("CMXTT", "create");
                item.TongCong = item.GiaVon;
                /* item.NgayChi = DateTime.Now;
                 item.NgayHachToan = DateTime.Now;*/
                string message = _soChiService.CreatePhieuChi(item, httt, CTPC, true, xml);
                if (message == "")
                {
                    //insert vao kho xe duoc xu ly duoi store sp_XuLySauKhiInsertPhieuChi
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }

                else
                {
                    TempData["Error"] = "Lỗi tạo phiếu:" + message;
                    _logger.WriteLog("SoChiControler.CMXTTEdit", message);
                }
                SoChiModel model = GetModel("CMXTT");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMXTTCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CMXTTEdit(string id, string action = "", string a = "")
        {
            try
            {
                CreateLink("CMXTT", "edit");
                SoChiModel model = GetModel("CMXTT");
                model.Item = _soChiService.GetPhieuChi(id);
                List<NoPhaiTra> lstNPTr = _soChiService.GetNoPhaiTra(id);
                if (lstNPTr.Count > 0)
                    model.ItemNoPhaiTra = lstNPTr[0];
                else
                    model.ItemNoPhaiTra = new NoPhaiTra();
                if (model.Item == null || model.Item.MaLoaiPhieu != "CMXTT")
                {
                    TempData["Error"] = "Không tìm thấy mã phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                model.CTPC = _soChiService.getListCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMXTTEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CMXTTEdit(PhieuChi item, List<ChiTietPhieuChi> httt, NoPhaiTra noPhaiTra, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CMXTTEdit</Action>";
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

                CreateLink("CMXTT", "edit");
                noPhaiTra.LoaiPhieuNo = "NMXTT";
                noPhaiTra.ThongTinDoiTac = item.DoiTac;
                item.TongCong = item.GiaVon;
                string sessionId = "";
                string message = _soChiService.UpdatePhieuChi(item, httt, noPhaiTra, CTPC, ref sessionId, true, xml);
                if (string.IsNullOrEmpty(message))
                {
                    //ViewBag.Alert = "Cập nhật thành công";
                    TempData["Info"] = "Cập nhật thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Cập nhật thất bại:" + message;
                    ViewBag.Error = "Cập nhật thất bại:" + message;
                    _logger.WriteLog(TempData["Error"].ToString());
                }
                //SoChiModel model = GetModel("CMXTT");
                //model.Item = item;
                //model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                //if (model.CTPT.Count == 0)
                //{
                //    model.CTPT = new List<ChiTietPhieuChi>();
                //    model.CTPT.Add(new ChiTietPhieuChi());
                //}

            }
            catch (Exception ex)
            {

                _logger.WriteLog("SoChiController.CMXTTEdit:", ex.InnerException);

            }
            return Redirect("~/SoChi/CMXTTIndex");
            //return View();
        }

        public ActionResult CMXTTDelete(string id)
        {
            try
            {
                CreateLink("CMXTT", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    TempData["Info"] = "Xóa thành công";
                else
                    TempData["Error"] = "Xóa thất bại:" + message;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMXTTDelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        #endregion

        #region Chi đầu tư CDATU

        public ActionResult CDATUIndex(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                CreateLink("CDATU", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CDATU");
                var model = _soChiService.GetPhieuChiList("CDATU", f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDATUIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CDATUCreate()
        {
            try
            {
                CreateLink("CDATU", "create");
                SoChiModel model = GetModel("CDATU");
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDATUCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CDATUCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CDATUCreate</Action>";
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

                item.MaLoaiPhieu = "CDATU";
                CreateLink("CDATU", "create");
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                SoChiModel model = GetModel("CDATU");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDATUCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CDATUEdit(string id, string action = "", string a = "")
        {
            try
            {
                CreateLink("CDATU", "edit");
                SoChiModel model = GetModel("CDATU");
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null || model.Item.MaLoaiPhieu != "CDATU")
                {
                    TempData["Error"] = "Không tìm thấy mã phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDATUEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CDATUEdit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CDATUEdit</Action>";
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

                CreateLink("CDATU", "edit");
                ActionResult rs = Update(item, httt, xml);
                if (rs != null)
                    return rs;
                SoChiModel model = GetModel("CDATU");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDATUEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CDATUDelete(string id)
        {
            try
            {
                CreateLink("CDATU", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    TempData["Info"] = "Xóa thành công";
                else
                    TempData["Error"] = "Xóa thất bại:" + message;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDATUDelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        #endregion

        #region Chi tổng hợp- chi khác (CTOHO)

        public ActionResult CTOHOIndex(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                CreateLink("CTOHO", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CTOHO");
                var model = _soChiService.GetPhieuChiList("CTOHO", f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTOHOIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CTOHOCreate()
        {
            try
            {
                CreateLink("CTOHO", "create");
                SoChiModel model = GetModel("CTOHO");
                model.Item = new PhieuChi();
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTOHOIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CTOHOCreate(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CTOHOCreate</Action>";
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

                item.MaLoaiPhieu = "CTOHO";
                CreateLink("CTOHO", "create");
                string message = _soChiService.CreatePhieuChi(item, httt, CTPC, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                SoChiModel model = GetModel("CTOHO");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);
                //return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTOHOCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }

        public ActionResult CTOHOEdit(string id, string action = "", string a = "")
        {
            try
            {
                double STTT = 0;
                CreateLink("CTOHO", "edit");
                SoChiModel model = GetModel("CTOHO");
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null || model.Item.MaLoaiPhieu != "CTOHO")
                {
                    TempData["Error"] = "Không tìm thấy mã phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id).Where(s => s.SoTienThanhToan > 0).ToList();
                if (model.CTPT.Count > 0)
                {
                    foreach (var item in model.CTPT)
                    {
                        STTT += item.SoTienThanhToan;
                    }
                }
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                ViewBag.SoTienThanhToan = STTT;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTOHOEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CTOHOEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CTOHOEdit</Action>";
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
                CreateLink("CTOHO", "edit");
                ActionResult rs = Update(item, httt, CTPC, xml);
                if (rs != null)
                    return rs;
                SoChiModel model = GetModel("CTOHO");
                model.Item = item;
                //truc them 23.11.2017
                //item.NgayChi.ToString("dd/MM/yyyy");
                //item.NgayHachToan.Value.ToString("dd/MM/yyyy");
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);
                //return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTOHOEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CTOHODelete(string id)
        {
            try
            {
                CreateLink("CTOHO", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    TempData["Info"] = "Xóa thành công";
                else
                    TempData["Error"] = "Xóa thất bại:" + message;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTOHODelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        #endregion


        #region Chi mua tài sản (CMUTS)

        public ActionResult CMUTSIndex(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                CreateLink("CMUTS", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CMUTS", tinhtrang);
                var model = _soChiService.GetPhieuChiList("CMUTS", f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMUTSIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CMUTSCreate()
        {
            try
            {
                CreateLink("CMUTS", "create");
                SoChiModel model = GetModel("CMUTS");
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMUTSCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CMUTSCreate(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CMUTSCreate</Action>";
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
                item.DoiTac = item.NhaCungCap;
                item.MaLoaiPhieu = "CMUTS";
                //item.NgayChi = DateTime.Now;
                //item.NgayHachToan = DateTime.Now;
                CreateLink("CMUTS", "create");
                string message = _soChiService.CreatePhieuChi(item, httt, CTPC, true, xml);
                if (message == "")
                {
                    //insert tai san vao kho duoc xu ly duoi store sp_XuLySauKhiInsertPhieuChi

                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                    TempData["Error"] = "Lỗi tạo phiếu:" + message;
                }
                SoChiModel model = GetModel("CMUTS");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMUTSCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
            //return View(model);
        }

        public ActionResult CMUTSEdit(string id, string action = "", string a = "")
        {
            try
            {
                CreateLink("CMUTS", "edit");
                SoChiModel model = GetModel("CMUTS");
                model.Item = _soChiService.GetPhieuChi(id);
                model.CTPC = _soChiService.getListCTPC(id);
                if (model.Item == null || model.Item.MaLoaiPhieu != "CMUTS")
                {
                    TempData["Error"] = "Lỗi sửa phiếu: Không tìm thấy phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMUTSEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CMUTSEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CMUTSEdit</Action>";
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

                CreateLink("CMUTS", "edit");
                ActionResult rs = Update(item, httt, CTPC, xml);
                if (rs != null)
                    return rs;
                SoChiModel model = GetModel("CMUTS");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);
                //return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMUTSEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }
        public ActionResult CMUTSDelete(string id)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                CreateLink("CMUTS", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    TempData["Info"] = "Xóa thành công";
                else
                    TempData["Error"] = "Xóa thất bại:" + message;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMUTSDelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }

        #endregion

        #region Chi đặt cọc (CDACO)

        public ActionResult CDACOIndex(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                CreateLink("CDACO", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CDACO");
                var model = _soChiService.GetPhieuChiList("CDACO", f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDACOIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CDACOCreate()
        {
            try
            {
                CreateLink("CDACO", "create");
                SoChiModel model = GetModel("CDACO");
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDACOCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CDACOCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CDACOCreate</Action>";
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

                item.MaLoaiPhieu = "CDACO";
                CreateLink("CDACO", "create");
                //item.ConLai = item.TongCong;
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                return Redirect(ViewBag.LinkIndex);
                SoChiModel model = GetModel("CDACO");
                model.Item = item;
                model.CTPT = httt;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDACOCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CDACOEdit(string id, string action = "", string a = "")
        {
            try
            {
                CreateLink("CDACO", "edit");
                SoChiModel model = GetModel("CDACO");
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null || model.Item.MaLoaiPhieu != "CDACO")
                {
                    TempData["Error"] = "Lỗi sửa phiếu: không tìm thấy phiếu " + id + " hoặc phiếu đã bị xóa";
                    return Redirect(ViewBag.LinkIndex);
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDACOEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult Update(PhieuChi item, List<ChiTietPhieuChi> httt, string giaTriTrenGiaoDien)
        {
            try
            {
                string sessionId = "";
                string message = _soChiService.UpdatePhieuChi(item, httt, ref sessionId, true, giaTriTrenGiaoDien);
                if (string.IsNullOrEmpty(message))
                {
                    TempData["Info"] = "Cập nhật thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Cập nhật thất bại:" + message;
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.Update", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }
        public ActionResult Update(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC, string giaTriTrenGiaoDien)
        {
            try
            {
                string sessionId = "";
                string message = _soChiService.UpdatePhieuChi(item, httt, CTPC, ref sessionId, true, giaTriTrenGiaoDien);
                if (string.IsNullOrEmpty(message))
                {
                    // ViewBag.Alert = "Cập nhật thành công";
                    TempData["Info"] = "Cập nhật thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    //ViewBag.Alert = "Cập nhật thất bại:" + message;
                    TempData["Error"] = "Cập nhật thất bại:" + message;
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.Update", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CDACOEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CDACOEdit</Action>";
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
                item.MaLoaiPhieu = "CDACO";
                CreateLink("CDACO", "edit");
                ActionResult rs = Update(item, httt, CTPC, xml);
                if (rs != null)
                    return rs;

                SoChiModel model = GetModel("CDACO");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);
                //return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDACOEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }
        public ActionResult CDACODelete(string id)
        {
            try
            {
                CreateLink("CDACO", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                    TempData["Info"] = "Xóa thành công";
                else
                    TempData["Error"] = "Xóa thất bại:" + message;
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CDACODelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }

        #endregion

        #region Chi mua hàng trên đường
        public ActionResult CMHTDIndex(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                CreateLink("CMHTD", "index");
                ViewBag.Alert = TempData["Alert"];
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CMHTD");
                var model = _soChiService.GetPhieuChiList("CMHTD", f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMHTDIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }
        public ActionResult CMHTDCreate()
        {
            try
            {
                CreateLink("CMHTD", "create");
                SoChiModel model = GetModel("CMHTD");
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                ViewBag.Date = SetDateDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMHTDCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CMHTDCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CMHTDCreate</Action>";
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

                CreateLink("CMHTD", "create");
                item.MaLoaiPhieu = "CMHTD";
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect("CMHTDIndex");
                }
                else
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                SoChiModel model = GetModel("CMHTD");
                model.Item = item;
                model.CTPT = httt;
                return Redirect(ViewBag.LinkIndex);
                //return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMHTDCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult CMHTDEdit(string id)
        {
            try
            {
                CreateLink("CMHTD", "edit");
                SoChiModel model = GetModel("CMHTD");
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null || model.Item.MaLoaiPhieu != "CMHTD")
                {
                    return Redirect("/SoChi/CMHTDIndex");
                }
                model.CTPT = _soChiService.GetCTPC(id);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMHTDEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CMHTDEdit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CMHTDEdit</Action>";
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

                CreateLink("CMHTD", "edit");
                //khong cho edit nữa
                //ActionResult rs = Update(item, httt,xml);
                //if (rs != null)
                //    return rs;

                //SoChiModel model = GetModel("CMHTD");
                //model.Item = item;
                //model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                //if (model.CTPT.Count == 0)
                //{
                //    model.CTPT = new List<ChiTietPhieuChi>();
                //    model.CTPT.Add(new ChiTietPhieuChi());
                //}
                return Redirect(ViewBag.LinkIndex);
                //return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMHTDEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CMHTDDelete(string id)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                CreateLink("CMHTD", "edit");
                string message = _soChiService.DeletePhieuChi(id);
                if (string.IsNullOrEmpty(message))
                {
                    TempData["Info"] = "Xóa thành công";
                }
                else
                {
                    TempData["Error"] = "Xóa thất bại:" + message;
                    _logger.WriteLog("CMHTDDelete(" + id + "):" + message);
                }
                return Redirect("/SoChi/CMHTDIndex");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CMHTDDelete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }

        #endregion

        #region Chi thu hộ

        public ActionResult PhieuChiHo(string id = "", string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "", int pageSize = 30)
        {
            try
            {
                ViewBag.Title = "Danh sách nợ " + GetTenLoaiNo(id);
                ViewBag.KhachHangNoTitle = "Công ty";
                DateTime f = new DateTime(), t = new DateTime();
                List<ViewNoPhaiTra> model = new List<ViewNoPhaiTra>();
                SetSearch1(ref from, ref to, ref f, ref t, p, ref query, id, ref tinhtrang);
                model = _soChiService.GetNPTs1(id, f, t, query, p, ref total, pageSize, tinhtrang);
                SetSearch(p, from, to, query, id);
                ViewBag.Total = total;
                ViewBag.Type = id;
                ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&tinhtrang=" + tinhtrang + "&p=";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChi", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }

        public ActionResult PhieuChiHoCreate(string id, string type)
        {
            try
            {
                var cookie = Request.Cookies.Get("link_PC");
                if (cookie != null)
                {
                    id = cookie.Value;
                    id = id.Replace(",", ";");
                }
                Request.Cookies.Remove("link_PC");

                ViewBag.Title = "Tạo mới phiếu chi nợ";
                ViewBag.KhachHangNoTitle = "Công ty";
                var model = GetModelCreate();
                ViewBag.LinkIndex = "/SoChi/PhieuChiHo/" + type;
                ViewBag.Type = type;
                ViewBag.ChuoiUpdatedDate = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(id);
                model.VNPT = new ViewNoPhaiTra();
                model.VNPT.HoaHong = 0;

                ViewBag.Id = id;
                var result = id.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in result)
                {
                    var obj = _soChiService.GetNPT(item);
                    model.VNPT.MaPhieuNo += obj.MaPhieuNo + ";";
                    model.VNPT.SoTienNo += obj.SoTienNo;
                    model.VNPT.HoaHong += obj.HoaHong;
                    model.VNPT.KhachHang += obj.Hoten + ".\n";
                    model.VNPT.ThongTinDonViNo = obj.DonViNo;
                    model.VNPT.DonViNo = obj.DonViNo;
                }
                model.VNPT.SoTienConLai = model.VNPT.SoTienNo;
                model.VNPT.LoaiPhieuNo = type;
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                if (model.VNPT.MaPhieuNo != "")
                    model.VNPT.MaPhieuNo = model.VNPT.MaPhieuNo.Substring(0, model.VNPT.MaPhieuNo.Length - 1);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiHoCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        [HttpPost]
        public ActionResult PhieuChiHoCreate(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC, string submit, string ids, string ChuoiUpdatedDate)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>PhieuChiHoCreate</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
                if (ids == null)
                    xml += Environment.NewLine + "<ids>" + ids + "</ids>";
                if (ChuoiUpdatedDate == null)
                    xml += Environment.NewLine + "<ChuoiUpdatedDate>" + ChuoiUpdatedDate + "</ChuoiUpdatedDate>";
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

                ViewBag.Title = "Thu nợ";
                //lay thong tin doi tac tra no la thong tin phieu no dau tien trong danh sach

                string chuoiUpdatedDateDatabase = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(!string.IsNullOrEmpty(ids) ? ids : item.MaPhieuLienQuan);
                if (ChuoiUpdatedDate != chuoiUpdatedDateDatabase)
                {
                    TempData["Warning"] = "Không thể tạo phiếu chi nợ vì phiếu nợ gốc đã có người cập nhật trước đó.";
                    _logger.WriteLog("PhieuChiCreate:" + TempData["Warning"].ToString() + ".ChuoiUpdatedDate:" + ChuoiUpdatedDate + ".ChuoiUpdatedDate tren database:" + chuoiUpdatedDateDatabase);
                    return Redirect("/SoChi/" + item.MaLoaiPhieu + "Index");
                }
                else
                {
                    string message = _soChiService.CreatePhieuChi(item, httt, false, xml);
                    if (message == "")
                    {
                        TempData["Info"] = "Tạo mới thành công";
                        if (!string.IsNullOrEmpty(ids))
                        {
                            var result = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var obj in result)
                            {
                                _noPhaiTraService.UpdateMaPhieuChi(obj, item.MaPhieuChi);
                            }
                        }
                        _soChiService.XuLySauKhiInsertPhieuChi(item.MaPhieuChi, xml);
                        return Redirect("/SoChi/" + item.MaLoaiPhieu + "Index");
                    }
                    else
                    {
                        TempData["Info"] = "Lỗi tạo phiếu:" + message;
                        ViewBag.Error = "Lỗi tạo phiếu:" + message;
                         return Redirect("/SoChi/" + item.MaLoaiPhieu + "Index");
                    }
                    
                    SoChiModel model = GetModelCreate();
                    model.Item = item;
                    model.CTPT = httt;
                    if (model.CTPT.Count == 0)
                    {
                        model.CTPT = new List<ChiTietPhieuChi>();
                        model.CTPT.Add(new ChiTietPhieuChi());
                    }
                    return Redirect(ViewBag.LinkIndex);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiHoCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult PhieuChiHoComplete(string id = "", string from = "", string to = "", string query_complete = "", int p = 1, int pageSize = 30)
        {
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetDate(ref from, ref to, ref f, ref t, ref query_complete, id.Substring(0, 5), p);
                ViewBag.Title = "Danh sách nợ ";
                ViewBag.KhachHangNoTitle = "Công ty";
                List<ViewNoDaTra> model = new List<ViewNoDaTra>();
                int total = 1000;
                model = _soChiService.GetPhieuChiComplete_NTHBH(id, f, t, query_complete, p, ref total, pageSize);
                SetSearch1(p, from, to, query_complete, id);
                total = model.Count;
                ViewBag.Type = id;
                ViewBag.Total = total;
                ViewBag.Query_Complete = query_complete;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiHoComplete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }

        public ActionResult PhieuChiHoEdit(string id, string type)
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                ViewBag.Title = "Cập nhật phiếu chi nợ";
                ViewBag.KhachHangNoTitle = "Khách hàng";
                ViewBag.LinkIndex = "/SoChi/PhieuChiHo/" + type;
                ViewBag.Type = type;
                var model = GetModelCreate();
                model.VNDT = _soChiService.GetPhieuChiCompleteById(id);
                model.VNDT.KhachHang = model.VNDT.Hoten + "\n" + model.VNDT.BienSo.Replace("|", "\n");
                model.CTPT = _soChiService.GetCTPC(id);
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiHoEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult PhieuChiHoEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC, string submit)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>PhieuChiHoEdit</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
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

                ViewBag.Alert = TempData["Alert"];
                ViewBag.Title = "Thu nợ";

                ActionResult rs;
                rs = Update(item, httt, xml);
                if (rs != null)
                    return rs;
                SoChiModel model = GetModelCreate();
                model.CTPT = httt;
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }




        #endregion


        #region Chi nợ
        public ActionResult NKMPKIndex(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                ViewBag.Title = "Danh sách nợ khuyến mãi phụ kiện - phụ tùng";
                ViewBag.KhachHangNoTitle = "Khách hàng";
                DateTime f = new DateTime(), t = new DateTime();
                List<ViewNoPhaiTra> model = new List<ViewNoPhaiTra>();
                SetSearch1(ref from, ref to, ref f, ref t, p, ref query, "NKMPK", ref tinhtrang);
                model = _soChiService.GetNPTs1("NKMPK", f, t, query, p, ref total, pageSize, tinhtrang);
                SetSearch(p, from, to, query, "NKMPK");
                ViewBag.Total = total;
                ViewBag.Type = "NKMPK";
                ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&tinhtrang=" + tinhtrang + "&p=";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChi", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult NKMPKComplete(string from = "", string to = "", string query_complete = "", int p = 1)
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                ViewBag.Title = " Thu trả nợ khuyến mãi phụ tùng - phụ kiện";
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query_complete, "NKMPK");
                var model = _phieuThuService.GetTNKMPK(f, t, query_complete, p, ref total, pageSize);
                ViewBag.Total = total;
                ViewBag.Tab = 2;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.NKMPKComplete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult PhieuChiCreateNKMPK(string id)
        {
            try
            {

                ViewBag.Type = "NKMPK";
                ViewBag.Alert = TempData["Alert"];
                ViewBag.Title = "Chi nợ khuyến mãi phụ kiện";
                ViewBag.CurrentUser = CurrentUser.UserName;
                SoChiModel model;
                model = GetModelCreate();
                model.Item = new PhieuChi();
                DataTable dt = new DataTable();
                model.MaXe = _loaiXeService.GetViewLoaiXeList();
                var PhieuNo = _noPhaiTraService.getNoPhaiTra(id);
                ViewBag.NoKhuyenMai = PhieuNo.SoTienConLai;
                model.Item.MaPhieuLienQuan = PhieuNo.MaPhieuNo;
                model.Item.MaLoaiPhieu = "CKMPK";
                ViewBag.ListPhuTung = _phieuThuService.GetListCTBPKKM("");
                double TongCong = PhieuNo.SoTienConLai;
                model.Item.TongCong = PhieuNo.SoTienConLai;
                int i = model == null ? 0 : model.CTPT.Count;
                if (model != null)
                {
                    foreach (var temp in model.CTPT)
                    {
                        if (temp.MaNganHang == "COCDV")
                            i -= 1;
                    }
                    ViewBag.CountCTPT = i;
                }
                List<ViewHinhThucThanhToan> list = _phieuThuService.ThuHoaHongNgay();
                ViewBag.ThuHoaHong = list;
                ViewBag.TongCong = TongCong;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoThuController:PhieuChiCreateNKMPK" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }

        [HttpPost]
        public ActionResult PhieuChiCreateNKMPK(PhieuChi item, List<ChiTietBanPhuKien> LPT, double nokhuyenmai, string submit)
        {
            try
            {
                double TongKMTra = 0;
                foreach (ChiTietBanPhuKien phukien in LPT)
                {
                    TongKMTra += phukien.GiaVon + phukien.TienCong;
                }
                item.SoTienChi = TongKMTra;
                item.ConLai = nokhuyenmai - TongKMTra;
                string message = _soChiService.CreatePhieuChi(item);
                if (message != "")
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                message = _phieuThuService.CreateCTBanPhuKien(item.MaPhieuChi, LPT, "NKMPK");
                if (message != "")
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                TempData["Info"] = "Tạo mới thành công";
                return Redirect("/SoChi/PhieuChi/NKMPK");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoThuController:PhieuChiCreateNKMPK" + ex.ToString());
                TempData["Error"] = ex;
            }
            return Redirect("/SoChi/PhieuChi/NKMPK");
        }

        public ActionResult PhieuChi(string id = "", string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "", int pageSize = 30)
        {
            try
            {
                ViewBag.Title = "Danh sách nợ " + GetTenLoaiNo(id);
                ViewBag.KhachHangNoTitle = GetKhachHangNoTitle(id);
                DateTime f = new DateTime(), t = new DateTime();
                List<ViewNoPhaiTra> model = new List<ViewNoPhaiTra>();
                SetSearch1(ref from, ref to, ref f, ref t, p, ref query, id, ref tinhtrang, pageSize);
                if (id == "NTHBH")
                    model = _soChiService.GetNPTs1(id, f, t, query, p, ref total, pageSize, tinhtrang);
                else if (id == "NBHBX" || id == "N2THK" || id == "N2GTX")
                    model = _soChiService.GetNPTs1(id, f, t, query, p, ref total, pageSize, tinhtrang);
                else
                    model = _soChiService.GetNPTs1(id, f, t, query, p, ref total, pageSize, tinhtrang);
                SetSearch(p, from, to, query, id, pageSize);
                ViewBag.Total = total;
                ViewBag.Type = id;
                ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&tinhtrang=" + tinhtrang + "&p=";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChi", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }

        public ActionResult PhieuChiCreate(string id, string type)
        {
            try
            {
                string[] listType = { "NTHPK", "CTHBH", "NBHBX", "NPTPK", "NTHBH", "N2GTX", "NKHAC", "N2THK", "NGCNG", "HHTXE", "NMPTU" };
                bool flag = listType.Contains((string)type);
                if (flag || ViewBag.Type == "NMPTU" || ViewBag.Type == "N2THK" || ViewBag.Type == "NGCNG" || ViewBag.Type == "N2GTX" || ViewBag.Type == "HHTXE" || ViewBag.Type == "NKHAC")
                {
                    var cookie = Request.Cookies.Get("link_PC");
                    if (cookie != null)
                    {
                        id = cookie.Value;
                        id = id.Replace(",", ";");
                    }
                    Request.Cookies.Remove("link_PC"); //remove cookie sau khi su dung
                }
                //cookie.Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies.Add(cookie);
                //HttpContext.Current.Request.Cookies.Clear();
                ViewBag.Title = "Tạo mới phiếu chi nợ " + GetTenLoaiNo(type);
                ViewBag.KhachHangNoTitle = GetKhachHangNoTitle(type);
                // ViewBag.Alert = TempData["Alert"];
                string[] list = { "NTHPK", "CTHBH", "NBHBX", "NPTPK", "NTHBH", "NBHXH", "NMPTU", "N2THK", "NGCNG", "N2GTX", "HHTXE", "NGCNG", "NKHAC" };//
                var model = GetModelCreate();
                ViewBag.LinkIndex = "/SoChi/PhieuChi/" + type;
                ViewBag.Type = type;
                ViewBag.ChuoiUpdatedDate = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(id);
                if (list.Contains(type))
                {
                    model.VNPT = new ViewNoPhaiTra();
                    if (type == "NBHBX" || type == "NTHBH")
                        model.VNPT.HoaHong = 0;
                    ViewBag.Id = id;
                    var result = id.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    int demN2THK = 0;
                    foreach (var item in result)
                    {
                        var obj = _soChiService.GetNPT(item);
                        model.VNPT.MaPhieuNo += obj.MaPhieuNo + ";";
                        model.VNPT.SoTienNo += obj.SoTienNo;
                        //model.VNPT.SoTienConLai += obj.SoTienConLai;
                        model.VNPT.HoaHong += obj.HoaHong;
                        if (type == "NTHBH" || type == "NBHBX" || type == "NMPTU" || type == "N2THK" || type == "NGCNG" || type == "NKHAC")
                        {
                            //model.VNPT.KhachHang += obj.Hoten + ",\n";
                            if (type == "NMPTU" || type == "N2THK" || type == "N2GTX")
                            {
                                if (model.VNPT.KhachHang == null)
                                {
                                    model.VNPT.KhachHang += obj.KhachHang;
                                }
                            }
                            else if (type == "N2THK")
                            {
                                if (demN2THK == 0)
                                    model.VNPT.KhachHang = obj.DonViNo;
                                demN2THK++;
                            }
                            else if (type == "NKHAC")
                            {
                                model.VNPT.KhachHang += obj.KhachHang + ",\n";
                            }
                            else
                            {
                                if (type == "HHTXE")
                                    model.VNPT.KhachHang += obj.Hoten + ",";
                                else
                                    model.VNPT.KhachHang += obj.Hoten + ",\n";
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(model.VNPT.KhachHang))
                            {
                                model.VNPT.KhachHang = obj.KhachHang + "\n";
                            }
                            model.VNPT.KhachHang += obj.MaPhieuNo + "\n";
                            model.VNPT.KhachHang += obj.Hoten;
                        }
                        model.VNPT.ThongTinDonViNo = obj.DonViNo;
                        model.VNPT.DonViNo = obj.DonViNo;
                    }
                    if (type != "N2GTX" || type != "NKHAC")
                    {
                        model.VNPT.SoTienConLai = model.VNPT.SoTienNo;
                    }
                    model.VNPT.LoaiPhieuNo = type;
                    model.DoiTac = _soChiService.GetDoiTac("NCC");
                    if (model.VNPT.MaPhieuNo != "")
                        model.VNPT.MaPhieuNo = model.VNPT.MaPhieuNo.Substring(0, model.VNPT.MaPhieuNo.Length - 1);
                    string MPLQ = model.VNPT.MaPhieuNo;
                    Array lstString = new Array[100];
                    lstString = MPLQ.Split(';');
                    if ((type == "N2THK" || type == "NMPTU" || type == "NGCNG" || type == "N2GTX" || type == "HHTXE" || type == "NKHAC") && lstString.Length > 0)
                    {
                        ViewBag.SoTienChi = 0;
                        foreach (var arr in lstString)
                        {
                            List<PhieuChi> pt = _PhieuChiRepository.Table.Where(x => x.MaPhieuLienQuan == arr.ToString() && x.IsDeleted == false).ToList();
                            foreach (var item in pt)
                            {
                                ViewBag.SoTienChi += item.SoTienChi;
                            }
                        }

                    }
                    ViewBag.MPLQ = MPLQ;
                    return View(model);
                }
                else
                {
                    model.VNPT = _soChiService.GetNPT(id);
                    model.DoiTac = _soChiService.GetDoiTac("NCC");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public ActionResult PhieuChiComplete(string id = "", string from = "", string to = "", string query_complete = "", int p = 1, int pageSize = 30)
        {
            try
            {
                string[] list = { "NTHPK", "CTHBH", "NPTPK", "NTHBH", "NBHBX", "NMPTU", "N2THK", "NGCNG", "N2GTX", "NKHAC" };
                DateTime f = new DateTime(), t = new DateTime();
                SetDate(ref from, ref to, ref f, ref t, ref query_complete, id.Substring(0, 5), p);
                ViewBag.Title = "Danh sách nợ " + GetTenLoaiNo(id);
                ViewBag.KhachHangNoTitle = GetKhachHangNoTitle(id);
                List<ViewNoDaTra> model = new List<ViewNoDaTra>();
                if (list.Contains(id))
                {
                    if (id == "NTHBH" || id == "NKHAC")
                    {
                        int size = 1000;
                        model = _soChiService.GetPhieuChiComplete_NTHBH(id, f, t, query_complete, p, ref size, pageSize);
                        total = model.Count;
                    }
                    else
                    {
                        model = _soChiService.GetPhieuChiComplete(id, f, t, query_complete, p, ref total, pageSize);
                    }
                }
                else if (id.Contains("N1") || id.Contains("K1") || id.Contains("N2GTX"))
                {
                    int size = 1000;
                    model = _soChiService.GetPhieuChiComplete_NTHBH(id, f, t, query_complete, p, ref size, pageSize);
                    total = model.Count;
                }
                else
                {
                    model = _soChiService.GetNoDaTra(id, f, t, query_complete, p, ref total, pageSize);
                }
                SetSearch1(p, from, to, query_complete, id);
                ViewBag.Type = id;
                ViewBag.Query_Complete = query_complete;
                ViewBag.PageSize = pageSize;
                //ViewBag.Url = Request.Url.AbsolutePath + "?id="+id+"&query_complete=" + query_complelte + "&from=" + from + "&to=" + to +"&p=" + p;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiComplete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();

        }

        public ActionResult PhieuChiEdit(string id, string type)
        {
            try
            {
                string[] list = { "NTHPK", "CTHBH", "NBHBX", "NPTPK", "NTHBH", "NMPTU", "N2THK", "NKHAC" };//
                ViewBag.Alert = TempData["Alert"];
                ViewBag.Title = "Cập nhật phiếu chi nợ " + GetTenLoaiNo(type);
                ViewBag.KhachHangNoTitle = GetKhachHangNoTitle(type);
                ViewBag.LinkIndex = "/SoChi/PhieuChi/" + type;
                ViewBag.Type = type;
                var model = GetModelCreate();
                if (!list.Contains(type))
                {
                    model.VNDT = _soChiService.GetNoDaTra(id);
                    if (/*type == "N2THK" || */type == "N2GTX")
                    {
                        model.VNDT.KhachHang = model.VNDT.DoiTac + "\n" + model.VNDT.BienSo.Replace("|", "\n");
                    }
                    else
                    {
                        model.VNDT.KhachHang = model.VNDT.Hoten + "\n" + model.VNDT.BienSo.Replace("|", "\n");
                    }
                }
                else
                {
                    model.VNDT = _soChiService.GetPhieuChiCompleteById(id);
                    model.VNDT.KhachHang = model.VNDT.Hoten + "\n" + model.VNDT.BienSo.Replace("|", "\n");
                }
                model.CTPT = _soChiService.GetCTPC(id);
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                if (type == "NGCNG")
                {
                    DoiTac DoiTac = _DoiTacRepository.Table.Where(x => x.MaDoiTac == model.VNDT.DoiTac).FirstOrDefault();
                    if (DoiTac != null)
                    {
                        ViewBag.DoiTac = DoiTac.TenDoiTac;
                        ViewBag.MaDoiTac = DoiTac.MaDoiTac;
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult PhieuChiEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC, string submit)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>PhieuChiEdit</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
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

                ViewBag.Alert = TempData["Alert"];
                ViewBag.Title = "Thu nợ";

                ActionResult rs;
                if (item.MaLoaiPhieu == "NGCNG")
                    rs = Update(item, httt, CTPC, xml);
                else
                    rs = Update(item, httt, xml);
                if (rs != null)
                    return rs;
                SoChiModel model = GetModelCreate();
                model.CTPT = httt;
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect(ViewBag.LinkIndex);
                //return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiEdit", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        [HttpPost]
        public ActionResult PhieuChiCreate(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC, string submit, string ids, string ChuoiUpdatedDate)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>PhieuChiCreate</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
                if (ids == null)
                    xml += Environment.NewLine + "<ids>" + ids + "</ids>";
                if (ChuoiUpdatedDate == null)
                    xml += Environment.NewLine + "<ChuoiUpdatedDate>" + ChuoiUpdatedDate + "</ChuoiUpdatedDate>";
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

                ViewBag.Title = "Thu nợ";
                //lay thong tin doi tac tra no la thong tin phieu no dau tien trong danh sach

                string chuoiUpdatedDateDatabase = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(!string.IsNullOrEmpty(ids) ? ids : item.MaPhieuLienQuan);
                if (CTPC != null && item.MaLoaiPhieu == "NGCNG")
                {
                    string maPhieu = "";
                    foreach (var temp in CTPC)
                    {
                        maPhieu = temp.SoThamChieu;
                        break;
                    }
                    PhieuChi phieuChi = _PhieuChiRepository.Table.Where(x => x.MaPhieuChi == maPhieu).FirstOrDefault();
                    item.DoiTac = phieuChi.DoiTac;
                }
                if (ChuoiUpdatedDate != chuoiUpdatedDateDatabase)
                {
                    TempData["Warning"] = "Không thể tạo phiếu chi nợ vì phiếu nợ gốc đã có người cập nhật trước đó.";
                    _logger.WriteLog("PhieuChiCreate:" + TempData["Warning"].ToString() + ".ChuoiUpdatedDate:" + ChuoiUpdatedDate + ".ChuoiUpdatedDate tren database:" + chuoiUpdatedDateDatabase);
                    return Redirect("/SoChi/PhieuChi/" + item.MaLoaiPhieu);
                }
                else
                {
                    string message = "";
                    if (item.MaLoaiPhieu == "NGCNG")
                    {
                        if (item.DoiTac == null)
                            item.DoiTac = item.NhaCungCap;
                        message = _soChiService.CreatePhieuChi(item, httt, CTPC, true, xml);
                    }

                    else
                        message = _soChiService.CreatePhieuChi(item, httt, false, xml);
                    if (message == "")
                    {
                        TempData["Info"] = "Tạo mới thành công";
                        if (!string.IsNullOrEmpty(ids))
                        {
                            var result = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var obj in result)
                            {
                                _noPhaiTraService.UpdateMaPhieuChi(obj, item.MaPhieuChi);
                            }
                        }
                        _soChiService.XuLySauKhiInsertPhieuChi(item.MaPhieuChi, xml);
                        return Redirect("/SoChi/PhieuChi/" + item.MaLoaiPhieu);
                    }
                    else
                    {
                        TempData["Info"] = "Lỗi tạo phiếu:" + message;
                        ViewBag.Error = "Lỗi tạo phiếu:" + message;
                        return Redirect("/SoChi/PhieuChi/" + item.MaLoaiPhieu);
                    }

                    SoChiModel model = GetModelCreate();
                    model.Item = item;
                    model.CTPT = httt;
                    if (model.CTPT.Count == 0)
                    {
                        model.CTPT = new List<ChiTietPhieuChi>();
                        model.CTPT.Add(new ChiTietPhieuChi());
                    }
                    return Redirect(ViewBag.LinkIndex);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChiCreate", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }

        public SoChiModel GetModelCreate()
        {
            SoChiModel model = new SoChiModel();
            model.Users = _userService.get();
            model.CTPT = new List<ChiTietPhieuChi>();
            model.CTPT.Add(new ChiTietPhieuChi());
            model.Users = _userService.get();
            model.HTTT = _soChiService.GetHTTT();
            return model;
        }

        private string SetDateDefault()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
        private string SetDateDefault1()
        {
            return DateTime.Now.ToString("MM/dd/yyyy");
        }
        /*
        public ActionResult TCODVEdit(string id)
        {
            ViewBag.Title = "Thu cọc PT - DV";
            SoChiModel model = GetModel("TCODV");
            model.Item = _soChiService.GetPhieuThu(id);
            if (model.Item == null || model.Item.MaLoaiPhieu != "TCODV")
            {
                return Redirect("/SoThu/NoItem");
            }
            model.CTPT = _soChiService.GetCTPT(id);
            if (model.CTPT.Count == 0)
            {
                model.CTPT = new List<ChiTietPhieuThu>();
                model.CTPT.Add(new ChiTietPhieuThu());
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult TCODVEdit(PhieuThu item, List<ChiTietPhieuThu> httt)
        {
            ViewBag.Title = "Thu cọc PT - DV";
            if (_soChiService.UpdatePhieuThu(item, httt))
            {
                ViewBag.Alert = "Cập nhật thành công";
            }
            else
                ViewBag.Alert = "Cập nhật thất bại";
            SoChiModel model = GetModel("TCODV");
            model.Item = item;
            model.CTPT = _soChiService.GetCTPT(item.MaPhieuThu);
            if (model.CTPT.Count == 0)
            {
                model.CTPT = new List<ChiTietPhieuThu>();
                model.CTPT.Add(new ChiTietPhieuThu());
            }
            return View(model);
        }
        public ActionResult TCODVCreate()
        {
            ViewBag.Title = "Thu cọc PT - DV";
            SoChiModel model = GetModel("TCODV");
            model.CTPT = new List<ChiTietPhieuThu>();
            model.CTPT.Add(new ChiTietPhieuThu());
            return View(model);
        }
        [HttpPost]
        public ActionResult TCODVCreate(PhieuThu item, List<ChiTietPhieuThu> httt)
        {
            ViewBag.Title = "Thu cọc PT - DV";
            item.MaLoaiPhieu = "TCODV";
            if (_soChiService.CreatePhieuThu(item, httt))
            {
                TempData["Alert"] = "Tạo mới thành công";
                return Redirect("TCODVIndex");
            }
            else
                ViewBag.Alert = "Tạo mới thất bại";
            SoChiModel model = GetModel("TCODV");
            model.Item = item;
            model.CTPT = httt;
            return View(model);
        }
        public ActionResult TCODVDelete(string id)
        {
            if (_soChiService.DeletePhieuThu(id))
                TempData["Alert"] = "Xóa thành công";
            else
                TempData["Alert"] = "Xóa thất bại";
            return Redirect("/SoThu/TCODVIndex");
        }
        */
        #endregion

        #region Hoàn cọc phụ tùng
        public ActionResult CHCPTComplete(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CHCPT");
                var model = _soChiService.GetCHCPT(f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex;
                _logger.WriteLog("SoChiController:CHCPTComplete" + ex.ToString());
            }
            return View();
        }
        public ActionResult CHCPTIndex(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CHCPT", tinhtrang);
                var model = _phieuThuService.GetTCODV(f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                ViewBag.Title = "Hoàn cọc phụ tùng";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCPTIndex" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        public ActionResult CHCPTCreate(string id)
        {
            try
            {
                ViewBag.Title = "Hoàn cọc phụ tùng";
                SoThuModel model = new SoThuModel();

                model.Item = _phieuThuService.GetPhieuThu(id);
                if (model.Item == null)
                    return null;
                ViewBag.ChuoiUpdatedDate = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(id);
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.CTPT = new List<ChiTietPhieuThu>();
                model.CTPT.Add(new ChiTietPhieuThu());
                model.MX = _categoryService.GetCategoryItemByCode(model.Item.MauXe);
                model.MAX = _categoryService.GetCategoryItemByCode(model.Item.MaXe);
                model.Item.KeToanTruong = Guid.Empty;
                model.Item.NguoiLapPhieu = Guid.Empty;
                model.Item.NguoiNopTien = Guid.Empty;
                model.Item.NguoiThuTien = Guid.Empty;
                model.Item.GhiChu = "";
                model.Item.SoChungTu = "";
                model.Item.MaPhieuLienQuan = id;
                model.Item.MaLoaiPhieu = "CHCPT";
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuThu>();
                    model.CTPT.Add(new ChiTietPhieuThu());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCPTCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CHCPTCreate(PhieuChi item, List<ChiTietPhieuChi> httt, string submit, string ChuoiUpdatedDate)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CHCPTCreate</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
                if (ChuoiUpdatedDate == null)
                    xml += Environment.NewLine + "<ChuoiUpdatedDate>" + ChuoiUpdatedDate + "</ChuoiUpdatedDate>";
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

                ViewBag.Title = "Hoàn cọc phụ tùng";
                string chuoiUpdatedDateDatabase = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(item.MaPhieuLienQuan);
                if (ChuoiUpdatedDate != chuoiUpdatedDateDatabase)
                {
                    TempData["Warning"] = "Không thể tạo phiếu hoàn cọc vì phiếu thu cọc đã có người cập nhật trước đó.";
                    _logger.WriteLog("Warning:" + item.MaPhieuLienQuan + "-" + TempData["Warning"]);

                    return Redirect("/SoChi/CHCPTIndex?tab=2");
                }
                else
                {
                    string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                    if (message == "")
                    {
                        TempData["Info"] = "Tạo mới thành công";
                        if (submit.Equals("Lưu"))
                            return Redirect("/SoChi/CHCPTIndex?tab=2");
                    }
                    else
                    {
                        TempData["Info"] = "Lỗi tạo phiếu:" + message;
                        TempData["Error"] = "Lỗi tạo phiếu:" + message;
                        return Redirect("/SoChi/CHCPTIndex?tab=2");
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCPTCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        public ActionResult CHCPTEdit(string id)
        {
            try
            {
                ViewBag.Title = "Hoàn cọc phụ tùng";
                SoChiModel model = new SoChiModel();
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null)
                    return null;
                model.CTPT = _soChiService.GetCTPC(id);
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.TCODV = _phieuThuService.GetPhieuThu(model.Item.MaPhieuLienQuan);
                model.MX = _categoryService.GetCategoryItemByCode(model.TCODV.MauXe);
                model.MAX = _categoryService.GetCategoryItemByCode(model.TCODV.MaXe);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCPTEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CHCPTEdit(PhieuChi item, List<ChiTietPhieuChi> httt, string submit)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CHCPTEdit</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
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

                ActionResult rs = Update(item, httt, xml);
                if (rs != null)
                    return rs;
                SoChiModel model = new SoChiModel();
                model.Item = item;
                model.CTPT = httt;
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.TCOBX = _phieuThuService.GetPhieuThu(model.Item.MaPhieuLienQuan);
                model.MX = _categoryService.GetCategoryItemByCode(model.TCOBX.MauXe);
                model.MAX = _categoryService.GetCategoryItemByCode(model.TCOBX.MaXe);
                return Redirect("/SoChi/CHCPTIndex?tab=2");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCPTEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        #endregion 

        #region Hoàn cọc bán xe
        public ActionResult CHCBXIndex(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "", int tab = 1)
        {
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CHCBX", tinhtrang);
                var model = _phieuThuService.GetTCOBX(f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Title = "Hoàn cọc bán xe";
                ViewBag.Total = total;
                ViewBag.Tab = tab;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCBXIndex" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }

        public ActionResult CHCBXEdit(string id)
        {
            try
            {
                ViewBag.Title = "Hoàn cọc bán xe";
                SoChiModel model = new SoChiModel();
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null)
                    return null;
                model.CTPT = _soChiService.GetCTPC(id);
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.TCOBX = _phieuThuService.GetPhieuThu(model.Item.MaPhieuLienQuan);
                model.MX = _categoryService.GetCategoryItemByCode(model.TCOBX.MauXe);
                model.MAX = _categoryService.GetCategoryItemByCode(model.TCOBX.MaXe);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCBXEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CHCBXEdit(PhieuChi item, List<ChiTietPhieuChi> httt, string submit)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CHCBXEdit</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
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
                CreateLink("CHCBX", "create");
                ActionResult rs = Update(item, httt, xml);
                if (rs != null)
                {
                    return Redirect("/SoChi/CHCBXIndex?tab=2");

                }
                SoChiModel model = new SoChiModel();
                model.Item = item;
                model.CTPT = httt;
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.TCOBX = _phieuThuService.GetPhieuThu(model.Item.MaPhieuLienQuan);
                model.MX = _categoryService.GetCategoryItemByCode(model.TCOBX.MauXe);
                model.MAX = _categoryService.GetCategoryItemByCode(model.TCOBX.MaXe);
                //return View(model);
                return Redirect("/SoChi/CHCBXIndex?tab=2");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCBXEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        public ActionResult CHCBXCreate(string id)
        {
            try
            {
                ViewBag.Title = "Hoàn cọc bán xe";
                SoThuModel model = new SoThuModel();
                model.Item = _phieuThuService.GetPhieuThu(id);
                if (model.Item == null)
                    return null;
                ViewBag.ChuoiUpdatedDate = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(id);
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.CTPT = new List<ChiTietPhieuThu>();
                model.CTPT.Add(new ChiTietPhieuThu());
                model.MX = _categoryService.GetCategoryItemByCode(model.Item.MauXe);
                model.MAX = _categoryService.GetCategoryItemByCode(model.Item.MaXe);
                model.Item.KeToanTruong = Guid.Empty;
                model.Item.NguoiLapPhieu = Guid.Empty;
                model.Item.NguoiNopTien = Guid.Empty;
                model.Item.NguoiThuTien = Guid.Empty;
                model.Item.GhiChu = "";
                model.Item.SoChungTu = "";
                model.Item.MaPhieuLienQuan = id;
                model.Item.MaLoaiPhieu = "CHCBX";
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuThu>();
                    model.CTPT.Add(new ChiTietPhieuThu());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCBXCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CHCBXCreate(PhieuChi item, List<ChiTietPhieuChi> httt, string submit, string ChuoiUpdatedDate)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CHCBXCreate</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
                if (ChuoiUpdatedDate == null)
                    xml += Environment.NewLine + "<ChuoiUpdatedDate>" + ChuoiUpdatedDate + "</ChuoiUpdatedDate>";
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
                CreateLink("CHCBX", "create");
                ViewBag.Title = "Hoàn cọc bán xe";
                string chuoiUpdatedDateDatabase = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(item.MaPhieuLienQuan);
                if (ChuoiUpdatedDate != chuoiUpdatedDateDatabase)
                {
                    TempData["Warning"] = "Không thể tạo phiếu hoàn cọc vì phiếu thu cọc đã có người cập nhật trước đó.";
                    _logger.WriteLog("Warning:" + item.MaPhieuLienQuan + "-" + TempData["Warning"]);
                    return Redirect("/SoChi/CHCBXIndex?tab=2");
                }
                else
                {
                    string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                    if (message == "")
                    {
                        TempData["Info"] = "Tạo mới thành công";
                        if (submit.Equals("Lưu"))
                            return Redirect("/SoChi/CHCBXIndex?tab=2");
                    }
                    else
                    {
                        TempData["Error"] = "Lỗi tạo phiếu:" + message;
                        return Redirect("/SoChi/CHCBXIndex?tab=2");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCBXCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }

        public ActionResult CHCBXComplete(string from = "", string to = "", string query = "", int p = 1, int tab = 2, int pageSize = 30)
        {
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CHCBX", pageSize);
                var model = _soChiService.GetCOCDT(f, t, query, p, ref total, pageSize);
                ViewBag.Tab = tab;
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCBXComplete" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        #endregion

        protected override void InvokeAction()
        {
            this._soChiService.SetIPClient(GetIPClient());
            this._soChiService.SetHostNameClient(GetHostNameClient());
            if (Request != null)
                this._soChiService.SetClientBrowser(Request.Browser.Type + "-" + Request.Browser.Type + "-" + Request.Browser.Version + "-" + Request.Url.ToString());

            base.InvokeAction();

        }
        #region phần hoàn đặt cọc
        public ActionResult THDCIndex(string from = "", string to = "", string query = "", int p = 1, string maloaiphieu = "")
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "");
                var model = _soChiService.ListThuHoanCoc("", f, t, query, p, ref total, pageSize, maloaiphieu);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.THDCIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult THDCComplete(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "");
                var model = _soChiService.ListThuHoanCocComplete("", f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.THDCComplete", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult THDCCreate(string id)
        {
            try
            {
                SoChiModel model = new SoChiModel();
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null)
                    return null;
                ViewBag.ChuoiUpdatedDate = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(id);
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.Item.KeToanTruong = Guid.Empty;
                model.Item.NguoiLapPhieu = Guid.Empty;
                model.Item.NguoiNopTien = Guid.Empty;
                model.Item.NguoiThuTien = Guid.Empty;
                model.Item.GhiChu = "";
                model.Item.SoChungTu = "";
                model.Item.MaPhieuLienQuan = id;
                //model.Item.MaLoaiPhieu = "CHCBX";
                //string[] list = { "CCOMX", "CDACO", "CCOPT", "CCOGC", "CCOTH" };
                string TieuDe = "";
                switch (model.Item.MaLoaiPhieu)
                {
                    case "CCOMX":
                        TieuDe = "mua xe";
                        break;
                    case "CDACO":
                        TieuDe = "mua tài sản";
                        break;
                    case "CCOPT":
                        TieuDe = "phụ tùng";
                        break;
                    case "CCOGC":
                        TieuDe = "gia công";
                        break;
                    case "CCOTH":
                        TieuDe = "tổng hợp";
                        break;

                }
                ViewBag.Title = "Thu hoàn đặt cọc " + TieuDe;
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                //tinh tong so tien da thu cua phieu chi nay
                ViewBag.TongSoTienDaThu = _soChiService.TongSoTienChi(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:THDCCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult THDCCreate(PhieuThu item, List<ChiTietPhieuThu> httt, string submit, string ChuoiUpdatedDate)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>THDCCreate</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
                if (ChuoiUpdatedDate == null)
                    xml += Environment.NewLine + "<ChuoiUpdatedDate>" + ChuoiUpdatedDate + "</ChuoiUpdatedDate>";
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
                string chuoiUpdatedDateDatabase = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(item.MaPhieuLienQuan);
                if (ChuoiUpdatedDate != chuoiUpdatedDateDatabase)
                {
                    TempData["Warning"] = "Không thể tạo phiếu hoàn cọc vì phiếu thu cọc đã có người cập nhật trước đó.";
                    _logger.WriteLog("Warning:" + item.MaPhieuLienQuan + "-" + TempData["Warning"]);
                    return Redirect("/SoChi/THDCIndex");
                }
                else
                {
                    if (item.MaLoaiPhieu == "CCOPT")
                    {
                        item.MaLoaiPhieu = "HCOPT";
                    }
                    else if (item.MaLoaiPhieu == "CCOGC")
                    {
                        item.MaLoaiPhieu = "HCOGC";
                    }
                    else if (item.MaLoaiPhieu == "CCOMX")
                    {
                        item.MaLoaiPhieu = "HCOMX";
                    }
                    else if (item.MaLoaiPhieu == "CDACO")
                    {
                        item.MaLoaiPhieu = "HDACO";
                    }
                    else if (item.MaLoaiPhieu == "CCOTH")
                    {
                        item.MaLoaiPhieu = "HCOTH";
                    }
                    string message = _phieuThuService.CreatePhieuThu(item, httt, true, xml);
                    if (message == "")
                    {
                        TempData["Info"] = "Tạo mới thành công";
                        if (submit.Equals("Lưu"))
                            return Redirect("/SoChi/THDCIndex");
                    }
                    else
                    {
                        TempData["Error"] = "Lỗi tạo phiếu:" + message;
                        return Redirect("/SoChi/THDCIndex");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:THDCCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }

        public ActionResult THDCEdit(string id)
        {
            try

            {
                ViewBag.Title = "Chi tiết thu hoàn đặt cọc";
                SoThuModel model = new SoThuModel();
                model.Item = _phieuThuService.GetPhieuThu(id);
                if (model.Item == null)
                    return null;
                model.CTPT = _phieuThuService.GetCTPT(id);
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:THDCEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult DeletePhieuThu(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Content("Bạn không có quyền xóa");
            }
            string message = _soChiService.DeletePhieuThu(id);
            if (string.IsNullOrEmpty(message))
                return Content("success");
            else
                return Content(message);

        }
        #endregion

        #region phần về cọc
        //phan coc chi mua tai san
        public ActionResult CocMuaTaiSan(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                DateTime f = new DateTime();
                DateTime t = new DateTime();
                ViewBag.Title = "Chi cọc mua tai san";
                var model = new object();
                SetSearch1(ref from, ref to, ref f, ref t, p, ref query, "CCOTS", ref tinhtrang);
                model = _soChiService.GetPhieuChiList("CCOTS", f, t, query, p, ref total, pageSize, tinhtrang);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCBXComplete" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }

        public ActionResult CocMTSCreate()
        {
            try
            {
                ViewBag.Date = SetDateDefault();
                SoChiModel model = new SoChiModel();
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                model.Users = _userService.get();
                model.HTTT = _soChiService.GetHTTT();
                //model.ChiCoc = _soChiService.GetChiCoc();
                ViewBag.Title = "Thông tin phiếu";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMTSCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CocMTSCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocMuaTaiSanCreate</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);
                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
                item.MaLoaiPhieu = "CCOTS";
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect("/SoChi/CocMuaTaiSan");
                }
                else
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                //SoChiModel model = GetModel("CMHTD");
                //model.Item = item;
                return Redirect("/SoChi/CocMuaTaiSan");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMTSCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        //phan coc mua xe thực tế
        public ActionResult CocMuaXe(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                DateTime f = new DateTime();
                DateTime t = new DateTime();
                ViewBag.Title = "Chi cọc mua xe";
                var model = new object();
                SetSearch1(ref from, ref to, ref f, ref t, p, ref query, "CCOMX", ref tinhtrang);
                model = _soChiService.GetPhieuChiList("CCOMX", f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaXe" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        public ActionResult CocMuaXeCreate()
        {
            try
            {
                ViewBag.Date = SetDateDefault();
                SoChiModel model = new SoChiModel();
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                model.Users = _userService.get();
                model.HTTT = _soChiService.GetHTTT();
                //model.ChiCoc = _soChiService.GetChiCoc();
                ViewBag.Title = "Thông tin phiếu";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaXeCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CocMuaXeCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocMuaXeCreate</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);
                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
                item.MaLoaiPhieu = "CCOMX";
                //item.ConLai = item.TongCong;
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect("/SoChi/CocMuaXe");
                }
                else
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                SoChiModel model = GetModel("CMHTD");
                model.Item = item;
                return Redirect("/SoChi/CocMuaXe");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaXeCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        //phan cọc mua phụ tùng
        public ActionResult CocMuaPhuTung(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                DateTime f = new DateTime();
                DateTime t = new DateTime();
                ViewBag.Title = "Chi cọc phụ tùng";
                var model = new object();
                SetSearch1(ref from, ref to, ref f, ref t, p, ref query, "CCOPT", ref tinhtrang);
                model = _soChiService.GetPhieuChiList("CCOPT", f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaXeCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        public ActionResult CocMuaPhuTungCreate()
        {
            try
            {

                ViewBag.Date = SetDateDefault();
                SoChiModel model = new SoChiModel();
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.DoiTac = _soChiService.GetDoiTac("NCC");

                model.Users = _userService.get();
                model.HTTT = _soChiService.GetHTTT();
                ViewBag.Title = "Thông tin phiếu";
                List<KhoPhuTung> list = _KhoPhuTungRepository.Table.Where(x => x.IsActive == true).ToList();
                ViewBag.ListKho = list;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaPhuTungCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CocMuaPhuTungCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocMuaPhuTungCreate</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);
                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
                item.MaLoaiPhieu = "CCOPT";
                //item.ConLai = item.TongCong;

                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect("/SoChi/CocMuaPhuTung");
                }
                else
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }

                SoChiModel model = GetModel("CMHTD");
                model.Item = item;
                return Redirect("/SoChi/CocMuaPhuTung");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaPhuTungCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        //phan coc gai cong ngoai
        public ActionResult CocGCN(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                DateTime f = new DateTime();
                DateTime t = new DateTime();
                ViewBag.Title = "Chi cọc gia công ngoài";
                var model = new object();
                SetSearch1(ref from, ref to, ref f, ref t, p, ref query, "CCOGC", ref tinhtrang);
                model = _soChiService.GetPhieuChiList("CCOGC", f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocGCN" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        public ActionResult CocGCNCreate()
        {
            try
            {
                ViewBag.Date = SetDateDefault();
                SoChiModel model = new SoChiModel();
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                model.Users = _userService.get();
                model.HTTT = _soChiService.GetHTTT();
                ViewBag.Title = "Thông tin phiếu";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocGCNCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CocGCNCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocGCNCreate</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);
                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
                item.MaLoaiPhieu = "CCOGC";
                //item.ConLai = item.TongCong;
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect("/SoChi/CocGCN");
                }
                else
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                SoChiModel model = GetModel("CMHTD");
                model.Item = item;
                return Redirect("/SoChi/CocGCN");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocGCNCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        // phan coc tong hop
        public ActionResult CocTH(string from = "", string to = "", string query = "", int p = 1, string tinhtrang = "")
        {
            try
            {
                DateTime f = new DateTime();
                DateTime t = new DateTime();
                ViewBag.Title = "Chi cọc tổng hợp";
                var model = new object();
                SetSearch1(ref from, ref to, ref f, ref t, p, ref query, "CCOTH", ref tinhtrang);
                model = _soChiService.GetPhieuChiList("CCOTH", f, t, query, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocTH" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        public ActionResult CocTHCreate()
        {
            try
            {
                ViewBag.Date = SetDateDefault();
                SoChiModel model = new SoChiModel();
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                model.Users = _userService.get();
                model.HTTT = _soChiService.GetHTTT();
                model.LyDoChi = _soChiService.GetLyDoChi("CKA");
                ViewBag.Title = "Thông tin phiếu";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocTH" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CocTHCreate(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocGCNCreate</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);
                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
                item.MaLoaiPhieu = "CCOTH";
                //item.ConLai = item.TongCong;
                string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                if (message == "")
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect("/SoChi/CocTH");
                }
                else
                {
                    TempData["Info"] = "Lỗi tạo phiếu:" + message;
                    ViewBag.Error = "Lỗi tạo phiếu:" + message;
                }
                SoChiModel model = GetModel("CMHTD");
                model.Item = item;
                return Redirect("/SoChi/CocTH");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocTHCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }

        // phan ve ncc va loai coc
        //mua xe thuc te
        [HttpPost]
        public JsonResult DanhSachCocMXTT(string maPhieuChi, string Id)
        {
            var list = new List<PhieuChi>();
            if (Id != "")
            {
                List<PhieuChi> items = _soChiService.GetChiCocMXTT(maPhieuChi, Id);
                //list = items.Where(x => x.MaPhieuChi != null && x.IsDeleted == false).Select(x => x).ToList();
                if (maPhieuChi == "")
                    list = items.Where(x => x.MaPhieuChi != null && x.TongCong > 0).ToList();
                else
                    list = items.Where(x => x.MaPhieuChi != null && (x.SoTienChi > 0 || x.TongCong > 0)).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //mua phu tung
        [HttpPost]
        public JsonResult DanhSachCocMUPT(string maPhieuChi, string Id)
        {
            var list = new List<PhieuChi>();
            if (Id != "")
            {
                List<PhieuChi> items = _soChiService.GetChiCocMPTU(maPhieuChi, Id);
                if (maPhieuChi == "")
                    list = items.Where(x => x.MaPhieuChi != null && x.TongCong > 0).ToList();
                else
                    list = items.Where(x => x.MaPhieuChi != null && (x.SoTienChi > 0 || x.TongCong > 0)).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //mua tai san
        [HttpPost]
        public JsonResult DanhSachCocMUTS(string maPhieuChi, string Id)
        {
            var list = new List<PhieuChi>();
            if (Id != "")
            {
                List<PhieuChi> items = _soChiService.GetChiCocMUTS(maPhieuChi, Id);
                //list = items.Where(x => x.MaPhieuChi != null).Select(x => x).ToList();
                if (maPhieuChi == "")
                    list = items.Where(x => x.MaPhieuChi != null && x.TongCong > 0).ToList();
                else
                    list = items.Where(x => x.MaPhieuChi != null && (x.SoTienChi > 0 || x.TongCong > 0)).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //phan coc tong hop
        [HttpPost]
        public JsonResult DanhSachCocTH(string maPhieuChi, string Id)
        {
            var list = new List<PhieuChi>();
            if (Id != "")
            {
                List<PhieuChi> items = _soChiService.GetChiCocTH(maPhieuChi, Id);
                //list = items.Where(x => x.MaPhieuChi != null).Select(x => x).ToList();
                if (maPhieuChi == "")
                    list = items.Where(x => (x.MaPhieuChi != null) && x.TongCong > 0).ToList();
                else
                    list = items.Where(x => x.MaPhieuChi != null && (x.SoTienChi > 0 || x.TongCong > 0)).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //phan coc gia công ngoài
        [HttpPost]
        public JsonResult DanhSachCocGCN(string maPhieuChi, string Id)
        {
            var list = new List<PhieuChi>();
            if (Id != "")
            {
                List<PhieuChi> items = _soChiService.GetChiCocGCN(maPhieuChi, Id);
                //list = items.Where(x => x.MaPhieuChi != null).Select(x => x).ToList();
                if (maPhieuChi == "")
                    list = items.Where(x => x.MaPhieuChi != null && x.TongCong > 0).ToList();
                else
                    list = items.Where(x => x.MaPhieuChi != null && (x.SoTienChi > 0 || x.TongCong > 0)).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //phan ve coc mua xe
        [HttpPost]
        public string DeleteCOC(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return "Bạn không có quyền xóa";
            }
            string message = _soChiService.DeletePhieuChi(id);
            if (string.IsNullOrEmpty(message))
            {
                return "success";
            }
            else
            {
                return string.Format("Xóa không thành công : {0}", message);
            }
        }
        public ActionResult CocMuaXeEdit(string ID, string action = "", string a = "")
        {
            try
            {
                SoChiModel model = GetModel("CCOMX");
                model.Item = _soChiService.GetPhieuChi(ID);
                if (model.Item == null)
                {
                    return Redirect("/SoChi/Index");
                }
                model.CTPT = _soChiService.GetCTPC(ID);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaXeEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CocMuaXeEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocMuaXeEdit</Action>";
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
                CreateLink("CCOMX", "edit");
                //item.ConLai = item.TongCong;
                ActionResult rs = Update(item, httt, CTPC, xml);
                if (rs != null)
                    return rs;

                SoChiModel model = GetModel("CCOMX");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect("/SoChi/CocMuaXe");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaXeEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }

        public ActionResult CocMuaPTEdit(string ID, string action = "CocMuaPTEdit")
        {
            try
            {
                List<KhoPhuTung> list = _KhoPhuTungRepository.Table.Where(x => x.IsActive == true).ToList();
                ViewBag.ListKho = list;
                SoChiModel model = GetModel("CCOPT");
                model.Item = _soChiService.GetPhieuChi(ID);

                if (model.Item == null)
                {
                    return Redirect("/SoChi/Index");
                }
                model.CTPT = _soChiService.GetCTPC(ID);
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.action = action;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaPTEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }

        [HttpPost]
        public ActionResult CocMuaPTEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocMuaPTEdit</Action>";
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
                CreateLink("CCOPT", "edit");
                //item.ConLai = item.TongCong;
                ActionResult rs = Update(item, httt, CTPC, xml);
                if (rs != null)
                    return rs;

                SoChiModel model = GetModel("CCOPT");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect("/SoChi/CocMuaPhuTung");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaXeEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }


        public ActionResult CocGCNEdit(string ID, string action = "", string a = "")
        {
            try
            {
                SoChiModel model = GetModel("CCOGC");
                model.Item = _soChiService.GetPhieuChi(ID);

                if (model.Item == null)
                {
                    return Redirect("/SoChi/Index");
                }
                model.CTPT = _soChiService.GetCTPC(ID);
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocGCNEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }

        [HttpPost]
        public ActionResult CocGCNEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocGCNEdit</Action>";
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
                CreateLink("CCOGC", "edit");
                //item.ConLai = item.TongCong;
                ActionResult rs = Update(item, httt, CTPC, xml);
                if (rs != null)
                    return rs;

                SoChiModel model = GetModel("CCOGC");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect("/SoChi/CocGCN");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocGCNEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        public ActionResult CocTHEdit(string ID, string action = "", string a = "")
        {
            try
            {
                SoChiModel model = GetModel("CCOTH");
                model.Item = _soChiService.GetPhieuChi(ID);

                if (model.Item == null)
                {
                    return Redirect("/SoChi/Index");
                }
                model.CTPT = _soChiService.GetCTPC(ID);
                model.LyDoChi = _soChiService.GetLyDoChi("CKA");
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                ViewBag.a = a;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocTHEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CocTHEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocTHEdit</Action>";
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
                CreateLink("CCOTH", "edit");
                //item.ConLai = item.TongCong;
                ActionResult rs = Update(item, httt, CTPC, xml);
                if (rs != null)
                    return rs;

                SoChiModel model = GetModel("CCOTH");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect("/SoChi/CocTH");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocTHEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        public ActionResult CocMuaTaiSanEdit(string ID)
        {
            try
            {
                SoChiModel model = GetModel("CCOTS");
                model.Item = _soChiService.GetPhieuChi(ID);

                if (model.Item == null)
                {
                    return Redirect("/SoChi/Index");
                }
                model.CTPT = _soChiService.GetCTPC(ID);
                model.DoiTac = _soChiService.GetDoiTac("NCC");
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocTHEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CocMuaTaiSanEdit(PhieuChi item, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CocMuaTaiSanEdit</Action>";
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
                CreateLink("CCOTS", "edit");
                //item.ConLai = item.TongCong;
                ActionResult rs = Update(item, httt, CTPC, xml);
                if (rs != null)
                    return rs;

                SoChiModel model = GetModel("CCOTS");
                model.Item = item;
                model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuChi>();
                    model.CTPT.Add(new ChiTietPhieuChi());
                }
                return Redirect("/SoChi/CocMuaTaiSan");
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CocMuaTaiSanEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        #endregion
        #region phần cập nhật hóa đơn chi
        public ActionResult Update_HDChi(string from = "", string to = "", string query = "", int p = 1, string maLoaiPhieu = "")
        {
            try
            {
                string id_LoaiPhieu = "PhieuChi";
                ViewBag.ListLoai = _phieuThuService.ListLoaiPhieu(id_LoaiPhieu);
                ViewBag.Title = "Cập nhật hóa đơn chi";
                DateTime f = new DateTime();
                DateTime t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "Update_HDChi");
                var model = _soChiService.GetListPhieuChi(f, t, p, ref total, pageSize, query, maLoaiPhieu);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:Update_HDChi" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();
        }
        public ActionResult Update_SoHoaDonChi(string maPhieuChi, string soHoaDon)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (string.IsNullOrEmpty(maPhieuChi))
                return Json(new { IsError = true, Message = "Lỗi: Không có thông tin cập nhật. Vui lòng kiểm tra lại." });
            if (_soChiService.Update_SoHoaDon(maPhieuChi, soHoaDon))
                return Json(new { IsError = false, Message = "Cập nhật thành công." });
            else
                return Json(new { IsError = true, Message = "Cập nhật thất bại." });
        }
        #endregion

        #region phan export excel
        public void XuatExcel(DateTime FromDate, DateTime ToDate, string MaLoaiPhieu, ref ExcelPackage pck)
        {
            string format = "#,##0";
            DataTable data = new DataTable();
            data = _soChiService.XuatExcel(FromDate, ToDate, MaLoaiPhieu);
            List<string> Mang = new List<string>() { "Tổng cộng", "Số tiền cọc" , "Số tiền chi", "Số tiền",
                "Thanh toán", "Còn lại", "Tổng cộng tiền", "Đã trả", "Còn nợ","Số tiền nợ" ,"Số tiền thanh toán",
            "Số tiền thanh toán"};

            string TuNgay = FromDate.Day + "/" + FromDate.Month + "/" + FromDate.Year;
            string DenNgay = ToDate.Day + "/" + ToDate.Month + "/" + ToDate.Year;
            var sheetName = "Báo_Cáo_Phiếu_Chi";
            var sheet = pck.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == sheetName);

            if (sheet == null) // Nếu chưa tồn tại thì add mới
            {
                sheet = pck.Workbook.Worksheets.Add(sheetName);
            }
            else
            {
                Console.WriteLine($"Sheet {sheetName} đã tồn tại!");
            }
            #region báo cáo phiếu thu

            int soCot = data.Columns.Count;
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
            List<string> lstTenCot = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            //lstTenCot.AddRange()


            var cellA1P1 = sheet.Cells["A1:" + lstTenCot[soCot] + "1"];
            cellA1P1.Value = "DANH SÁCH PHIẾU CHI";
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
                cell.Style.Font.Name = "Arial";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            }
            //double TongThanhToan = 0, TongTienMat = 0, TongConLai = 0;
            //int dem = 0;
            //xuat du lieu
            for (int r = 0; r < data.Rows.Count; r++)
            {
                //dem++;
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
            //dem = dem + 1;
            //var cellACuoiPCuoi = sheet.Cells["A" + dem + ":"+ lstTenCot[soCot - 4] + dem];
            //cellACuoiPCuoi.Value = "TỔNG";
            //cellACuoiPCuoi.Merge = true;
            //cellACuoiPCuoi.Style.Font.Bold = true;
            //cellACuoiPCuoi.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //cellACuoiPCuoi.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
            //cellACuoiPCuoi.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //cellACuoiPCuoi.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //cellACuoiPCuoi.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //cellACuoiPCuoi.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            //var cellAThanhToan = sheet.Cells["A" + dem + ":" + lstTenCot[soCot - 3] + dem];
            //cellAThanhToan.Value = TongThanhToan;
            //cellAThanhToan.Merge = true;
            //cellAThanhToan.Style.Font.Bold = true;
            //cellAThanhToan.Style.Numberformat.Format = format;
            //cellAThanhToan.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //cellAThanhToan.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
            //cellAThanhToan.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //cellAThanhToan.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //cellAThanhToan.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //cellAThanhToan.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            //var cellTienMat = sheet.Cells["A" + dem + ":" + lstTenCot[soCot - 2] + dem];
            //cellTienMat.Value = TongThanhToan;
            //cellTienMat.Merge = true;
            //cellTienMat.Style.Font.Bold = true;
            //cellTienMat.Style.Numberformat.Format = format;
            //cellTienMat.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //cellTienMat.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
            //cellTienMat.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //cellTienMat.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //cellTienMat.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //cellTienMat.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            //var cellConLai = sheet.Cells["A" + dem + ":" + lstTenCot[soCot - 1] + dem];
            //cellConLai.Value = TongThanhToan;
            //cellConLai.Merge = true;
            //cellConLai.Style.Font.Bold = true;
            //cellConLai.Style.Numberformat.Format = format;
            //cellConLai.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //cellConLai.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
            //cellConLai.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //cellConLai.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //cellConLai.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //cellConLai.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            sheet.Cells.AutoFitColumns();
            #endregion
        }
        public ActionResult Export_Excel(DateTime FromDate, DateTime ToDate, string MaLoaiPhieu)
        {
            var excel = new ExcelPackage();
            XuatExcel(FromDate, ToDate, MaLoaiPhieu, ref excel);
            string strFileName = "Danh_Sach_Phieu_Chi_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".xls";
            return new ExcelInsuranceDownload(excel, strFileName);
        }
        #endregion

        #region phần import excel
        //phần file mẫu chi tổng hợp
        //ExportFile_ChiTongHop
        public FileResult ExportFile_ChiTongHop()
        {
            var pck = new ExcelPackage();
            string strFileName = "~\\Views\\FileMau\\Import_ChiTongHop.xlsx";
            return File(strFileName, "application/vnd.ms-excel", "Import_ChiTongHop.xlsx");
        }
        public ActionResult ImportData_ChiTongHop()
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string error = "";
            try
            {
                Hashtable hshCol = new Hashtable();
                hshCol.Add("STT", 1);
                hshCol.Add("Số chứng từ", 0);
                hshCol.Add("Ngày chi", 0);
                hshCol.Add("Ngày hoạch toán", 0);
                hshCol.Add("Họ tên", 0);
                hshCol.Add("Địa chỉ", 0);
                hshCol.Add("Điện thoại", 0);

                hshCol.Add("Lý do chi", 0);
                hshCol.Add("Nội dung", 0);
                hshCol.Add("Ngân hàng", 0);
                hshCol.Add("HTTT", 0);
                hshCol.Add("Số tiền", 0);
                //hshCol.Add("Treo tiền", 0);
                hshCol.Add("Số tham chiếu", 0);
                hshCol.Add("Ghi chú", 0);
                hshCol.Add("Người duyệt", 0);
                hshCol.Add("Người thu tiền", 0);
                List<PhieuChi> lstPhieuChi = new List<PhieuChi>();
                List<List<ChiTietPhieuChi>> lstChiTietPhieuChi = new List<List<ChiTietPhieuChi>>();
                int beginRowNum = 1;
                string SoChungTu = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
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

                                    for (int ii = 1; ii <= 20; ii++)
                                    {
                                        if (ws.Cells[beginRowNum, ii].Value != null)
                                        {
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("STT"))
                                                hshCol["STT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số chứng từ"))
                                                hshCol["Số chứng từ"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngày chi"))
                                                hshCol["Ngày chi"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngày hoạch toán"))
                                                hshCol["Ngày hoạch toán"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Họ tên"))
                                                hshCol["Họ tên"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Địa chỉ"))
                                                hshCol["Địa chỉ"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Điện thoại"))
                                                hshCol["Điện thoại"] = ii;

                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Lý do chi"))
                                                hshCol["Lý do chi"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Nội dung"))
                                                hshCol["Nội dung"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngân hàng"))
                                                hshCol["Ngân hàng"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("HTTT"))
                                                hshCol["HTTT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tiền"))
                                                hshCol["Số tiền"] = ii;
                                            //if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Treo tiền"))
                                            //    hshCol["Treo tiền"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tham chiếu"))
                                                hshCol["Số tham chiếu"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ghi chú"))
                                                hshCol["Ghi chú"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Người duyệt"))
                                                hshCol["Người duyệt"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Người thu tiền"))
                                                hshCol["Người thu tiền"] = ii;
                                        }

                                    }
                                    //neu co cot khong ton tai trong danh sach cot can thiet thi canh bao sai dinh dang
                                    foreach (DictionaryEntry o in hshCol)
                                        if (o.Value.ToString() == "0")
                                        {
                                            return Json(new { IsError = true, Message = "File excel thiếu cột " + o.Key.ToString() + ". Vui lòng kiểm tra lại." }, JsonRequestBehavior.AllowGet);
                                        }
                                    #endregion

                                    #region Add dữ liệu
                                    int indexSoChungTu = (int)hshCol["Số chứng từ"];
                                    int indexNgayChi = (int)hshCol["Ngày chi"];
                                    int indexNgayHoachToan = (int)hshCol["Ngày hoạch toán"];
                                    int indexHoTen = (int)hshCol["Họ tên"];
                                    int indexDiaChi = (int)hshCol["Địa chỉ"];
                                    int indexDienThoai = (int)hshCol["Điện thoại"];

                                    int indexLyDoChi = (int)hshCol["Lý do chi"];
                                    int indexNoiDung = (int)hshCol["Nội dung"];

                                    int indexNganHang = (int)hshCol["Ngân hàng"];
                                    int indexHTTT = (int)hshCol["HTTT"];
                                    int indexSoTien = (int)hshCol["Số tiền"];
                                    /*nt indexTreoTien = (int)hshCol["Treo tiền"];*/
                                    int indexSoThamChieu = (int)hshCol["Số tham chiếu"];
                                    int indexGhiChu = (int)hshCol["Ghi chú"];
                                    int indexNguoiDuyet = (int)hshCol["Người duyệt"];
                                    int indexNguoiThuTien = (int)hshCol["Người thu tiền"];
                                    //kiem tra rang buoc tien trong hang tren duong con du thi moi import
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                        {
                                            try
                                            {

                                                //DataRow row = dtb.NewRow();
                                                DateTime NGAYCHI = new DateTime();
                                                DateTime NGAYHOACHTOAN = new DateTime();
                                                string ngaychi = ws.Cells[rowNum, indexNgayChi].Value == null ? "" : ws.Cells[rowNum, indexNgayChi].Value.ToString();
                                                SoChungTu = ws.Cells[rowNum, indexSoChungTu].Value == null ? "" : ws.Cells[rowNum, indexSoChungTu].Value.ToString();
                                                string NgayHoachToan = ws.Cells[rowNum, indexNgayHoachToan].Value == null ? "" : ws.Cells[rowNum, indexNgayHoachToan].Value.ToString();
                                                string DienThoai = ws.Cells[rowNum, indexDienThoai].Value == null ? "" : ws.Cells[rowNum, indexDienThoai].Value.ToString();
                                                string DiaChi = ws.Cells[rowNum, indexDiaChi].Value == null ? "" : ws.Cells[rowNum, indexDiaChi].Value.ToString();
                                                string HoTen = ws.Cells[rowNum, indexHoTen].Value == null ? "" : ws.Cells[rowNum, indexHoTen].Value.ToString();
                                                string NoiDung = ws.Cells[rowNum, indexNoiDung].Value == null ? "" : ws.Cells[rowNum, indexNoiDung].Value.ToString();

                                                string LyDoChi = ws.Cells[rowNum, indexLyDoChi].Value == null ? "" : ws.Cells[rowNum, indexLyDoChi].Value.ToString();
                                                string NguoiDuyet = ws.Cells[rowNum, indexNguoiDuyet].Value == null ? "" : ws.Cells[rowNum, indexNguoiDuyet].Value.ToString();

                                                string NganHang = ws.Cells[rowNum, indexNganHang].Value == null ? "" : ws.Cells[rowNum, indexNganHang].Value.ToString();
                                                string HTTT = ws.Cells[rowNum, indexHTTT].Value == null ? "" : ws.Cells[rowNum, indexHTTT].Value.ToString();
                                                string SoTien = ws.Cells[rowNum, indexSoTien].Value == null ? "" : ws.Cells[rowNum, indexSoTien].Value.ToString();
                                                string SoThamChieu = ws.Cells[rowNum, indexSoThamChieu].Value == null ? "" : ws.Cells[rowNum, indexSoThamChieu].Value.ToString();
                                                string GhiChu = ws.Cells[rowNum, indexGhiChu].Value == null ? "" : ws.Cells[rowNum, indexGhiChu].Value.ToString();
                                                //string TreoTien = ws.Cells[rowNum, indexTreoTien].Value == null ? "" : ws.Cells[rowNum, indexTreoTien].Value.ToString();
                                                string NguoiThuTien = ws.Cells[rowNum, indexNguoiThuTien].Value == null ? "" : ws.Cells[rowNum, indexNguoiThuTien].Value.ToString();
                                                HTTT = HTTT.ToUpper();
                                                if (string.IsNullOrEmpty(SoTien))
                                                    SoTien = "0";
                                                if (!TNK.Core.Common.IsNumeric(SoTien) && !string.IsNullOrEmpty(SoTien))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền (" + SoTien + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && Convert.ToDouble(SoTien) <= 0 && !string.IsNullOrEmpty(HTTT))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền không được nhỏ hơn bằng 0.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && !TNK.Core.Common.IsNumeric(SoTien))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền (" + SoTien + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(HTTT) && HTTT != "CK" && HTTT != "CT" && HTTT != "TM")
                                                {
                                                    error = "Dòng " + rowNum + " sai hình thức thanh toán.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && !_doiTacService.getDoiTacById(NganHang) && NganHang != "QuyChinh" && NganHang != "QuyPhu")
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy ngân hàng " + NganHang;
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(SoChungTu))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Vui lòng nhập số chứng từ.";
                                                    break;
                                                }
                                                User us = _userService.getByUserName(NguoiThuTien);
                                                if (us == null)
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy người thu tiền " + NguoiThuTien;
                                                    break;
                                                }
                                                if (HTTT == "TM" && NganHang != "QuyChinh" && NganHang != "QuyPhu")
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " sai hình thức thanh toán";
                                                    break;
                                                }
                                                if (HTTT != "TM" && (NganHang == "QuyChinh" || NganHang == "QuyPhu"))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " sai hình thức thanh toán";
                                                    break;
                                                }
                                                LyDoChi = LyDoChi.ToString().ToUpper();
                                                if (string.IsNullOrEmpty(LyDoChi))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập lý do chi";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(LyDoChi))
                                                {
                                                    List<CategoryItem> list = _soChiService.GetLyDoChi("CKA");
                                                    bool flag = false;
                                                    for (int i = 0; i < list.Count; i++)
                                                    {
                                                        if (list[i].Code == LyDoChi)
                                                            flag = true;
                                                    }
                                                    if (flag == false)
                                                    {
                                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng kiểm tra lại lý do chi";
                                                        break;
                                                    }
                                                }
                                                if (string.IsNullOrEmpty(NguoiDuyet))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập người duyệt";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NguoiDuyet))
                                                {

                                                }
                                                if ((Convert.ToDouble(SoTien) > 0 && (string.IsNullOrEmpty(NganHang) || string.IsNullOrEmpty(HTTT)))
                                                    || (!string.IsNullOrEmpty(NganHang) && (string.IsNullOrEmpty(SoTien) || string.IsNullOrEmpty(HTTT)))
                                                    || (!string.IsNullOrEmpty(HTTT) && (string.IsNullOrEmpty(NganHang) || string.IsNullOrEmpty(SoTien)))
                                                    )
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập đủ thông tin thanh toán(ngân hàng, HTTT,số tiền).";
                                                    break;
                                                }
                                                if (TNK.Core.Common.IsDate(ngaychi) == false)
                                                {
                                                    if (TNK.Core.Common.IsNumeric(ngaychi))
                                                    {
                                                        double DATE = double.Parse(ngaychi);
                                                        NGAYCHI = DateTime.FromOADate(DATE);
                                                    }
                                                }
                                                else
                                                    NGAYCHI = Convert.ToDateTime(ngaychi);
                                                if (TNK.Core.Common.IsDate(NgayHoachToan) == false)
                                                {
                                                    if (TNK.Core.Common.IsNumeric(NgayHoachToan))
                                                    {
                                                        double DATE = double.Parse(NgayHoachToan);
                                                        NGAYHOACHTOAN = DateTime.FromOADate(DATE);
                                                    }
                                                }
                                                else
                                                    NGAYHOACHTOAN = Convert.ToDateTime(NgayHoachToan);
                                                if (NGAYCHI.Year > 2000 && NGAYHOACHTOAN.Year > 2000)
                                                {
                                                    PhieuChi item = new PhieuChi();
                                                    List<ChiTietPhieuChi> CTPT = new List<ChiTietPhieuChi>();
                                                    item.MaLoaiPhieu = "CTOHO";
                                                    item.SoChungTu = SoChungTu;
                                                    item.NgayChi = Convert.ToDateTime(NGAYCHI);
                                                    item.NgayHachToan = Convert.ToDateTime(NGAYHOACHTOAN);
                                                    item.NguoiThuTien = us.UserId;
                                                    item.GhiChu = GhiChu;
                                                    item.ConLai = item.TongCong;
                                                    item.HoTen = HoTen;
                                                    item.DiaChi = DiaChi;
                                                    item.DienThoai = DienThoai;
                                                    item.NoiDung = NoiDung;
                                                    item.SoTienChi = Convert.ToDouble(SoTien);
                                                    item.GhiChu = GhiChu;
                                                    item.TongCong = item.SoTienChi;
                                                    item.LyDoChi = LyDoChi;
                                                    item.NguoiDuyet = NguoiDuyet;
                                                    //phan httt
                                                    HTTT = HTTT.ToUpper();
                                                    CTPT.Add(new ChiTietPhieuChi());
                                                    CTPT[0].HinhThucThanhToan = HTTT;
                                                    NganHang = NganHang.ToUpper();
                                                    CTPT[0].MaNganHang = NganHang;
                                                    CTPT[0].SoTienThanhToan = Convert.ToDouble(SoTien);
                                                    CTPT[0].Temp = NganHang + "-" + HTTT;
                                                    CTPT[0].SoThamChieu = SoThamChieu;
                                                    //if (TreoTien == "1")
                                                    //    CTPT[0].TinhTrang = true;
                                                    //else
                                                    //    CTPT[0].TinhTrang = false;

                                                    double TongThanhToan = 0;
                                                    foreach (var thanhtoan in CTPT)
                                                    {
                                                        TongThanhToan += thanhtoan.SoTienThanhToan;
                                                    }
                                                    item.SoTienChi = TongThanhToan;
                                                    if (item.SoTienChi > item.TongCong)
                                                    {
                                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " số tiền chi phải nhỏ hơn số tiền cần thanh toán";
                                                        break;
                                                    }
                                                    lstPhieuChi.Add(item);
                                                    lstChiTietPhieuChi.Add(CTPT);
                                                    #endregion
                                                }
                                                else
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Sai định dạng ngày thu. Vui lòng kiểm tra lại.";

                                            }
                                            catch (Exception exx)
                                            {
                                                error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": " + exx;
                                            }
                                        }
                                    }
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        for (int i = 0; i < lstPhieuChi.Count; i++)
                                        {

                                            string xml = "<formdata><Action>PhieuThu</Action>";
                                            xml += Environment.NewLine + "<submit>Import từ excel</submit>";
                                            if (lstPhieuChi[i] != null)
                                                xml += TNK.Core.Common.ConvertObjectToXMLString(lstPhieuChi[i]);
                                            if (lstChiTietPhieuChi[i] != null)
                                            {
                                                xml += Environment.NewLine + "<ThanhToan>";
                                                foreach (ChiTietPhieuChi ct in lstChiTietPhieuChi[i])
                                                    xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                                                xml += Environment.NewLine + "</ThanhToan>";
                                            }
                                            xml += "</formdata>";

                                            string message = _soChiService.CreatePhieuChi(lstPhieuChi[i], lstChiTietPhieuChi[i], true, xml);
                                            if (message != "")
                                            {
                                                error += Environment.NewLine + "Dòng " + (i + 1) + "(" + SoChungTu + "):" + message;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                _logger.WriteLog("SoThuController.ImportData_TongHop:" + objEx);
                return Json(new { IsError = true, Message = "Lỗi import excel." });
            }
            if (string.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    IsError = false,
                    Message = ""
                });
            }
            else
            {
                _logger.WriteLog("SoThuController.ImportData_TCBX:" + error);
                return Json(new { IsError = true, Message = error });
            }
        }
        // phan import chi tam ung
        // Lay file mau
        public FileResult ExportFile_ChiTamUng()
        {
            var pck = new ExcelPackage();
            string strFileName = "~\\Views\\FileMau\\Import_ChiTamUng.xlsx";
            return File(strFileName, "application/vnd.ms-excel", "Import_ChiTamUng.xlsx");
        }
        public ActionResult ImportData_ChiTamUng()
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string error = "";
            try
            {
                Hashtable hshCol = new Hashtable();
                hshCol.Add("STT", 1);
                hshCol.Add("Số chứng từ", 0);
                hshCol.Add("Ngày chi", 0);
                hshCol.Add("Ngày hoạch toán", 0);
                hshCol.Add("Họ tên", 0);
                hshCol.Add("Địa chỉ", 0);
                hshCol.Add("Điện thoại", 0);
                hshCol.Add("Lý do chi", 0);
                hshCol.Add("Nội dung", 0);
                hshCol.Add("Ngân hàng", 0);
                hshCol.Add("HTTT", 0);
                hshCol.Add("Số tiền", 0);
                //hshCol.Add("Treo tiền", 0);
                hshCol.Add("Số tham chiếu", 0);
                hshCol.Add("Ghi chú", 0);
                hshCol.Add("Người duyệt", 0);
                hshCol.Add("Người thu tiền", 0);
                List<PhieuChi> lstPhieuChi = new List<PhieuChi>();
                List<List<ChiTietPhieuChi>> lstChiTietPhieuChi = new List<List<ChiTietPhieuChi>>();
                int beginRowNum = 1;
                string SoChungTu = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
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

                                    for (int ii = 1; ii <= 20; ii++)
                                    {
                                        if (ws.Cells[beginRowNum, ii].Value != null)
                                        {
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("STT"))
                                                hshCol["STT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số chứng từ"))
                                                hshCol["Số chứng từ"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngày chi"))
                                                hshCol["Ngày chi"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngày hoạch toán"))
                                                hshCol["Ngày hoạch toán"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Họ tên"))
                                                hshCol["Họ tên"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Địa chỉ"))
                                                hshCol["Địa chỉ"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Điện thoại"))
                                                hshCol["Điện thoại"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Lý do chi"))
                                                hshCol["Lý do chi"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Nội dung"))
                                                hshCol["Nội dung"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngân hàng"))
                                                hshCol["Ngân hàng"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("HTTT"))
                                                hshCol["HTTT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tiền"))
                                                hshCol["Số tiền"] = ii;
                                            //if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Treo tiền"))
                                            //    hshCol["Treo tiền"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tham chiếu"))
                                                hshCol["Số tham chiếu"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ghi chú"))
                                                hshCol["Ghi chú"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Người duyệt"))
                                                hshCol["Người duyệt"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Người thu tiền"))
                                                hshCol["Người thu tiền"] = ii;
                                        }

                                    }
                                    //neu co cot khong ton tai trong danh sach cot can thiet thi canh bao sai dinh dang
                                    foreach (DictionaryEntry o in hshCol)
                                        if (o.Value.ToString() == "0")
                                        {
                                            return Json(new { IsError = true, Message = "File excel thiếu cột " + o.Key.ToString() + ". Vui lòng kiểm tra lại." }, JsonRequestBehavior.AllowGet);
                                        }
                                    #endregion

                                    #region Add dữ liệu
                                    int indexSoChungTu = (int)hshCol["Số chứng từ"];
                                    int indexNgayChi = (int)hshCol["Ngày chi"];
                                    int indexNgayHoachToan = (int)hshCol["Ngày hoạch toán"];
                                    int indexHoTen = (int)hshCol["Họ tên"];
                                    int indexDiaChi = (int)hshCol["Địa chỉ"];
                                    int indexDienThoai = (int)hshCol["Điện thoại"];
                                    int indexLyDoChi = (int)hshCol["Lý do chi"];
                                    int indexNoiDung = (int)hshCol["Nội dung"];

                                    int indexNganHang = (int)hshCol["Ngân hàng"];
                                    int indexHTTT = (int)hshCol["HTTT"];
                                    int indexSoTien = (int)hshCol["Số tiền"];
                                    //int indexTreoTien = (int)hshCol["Treo tiền"];
                                    int indexSoThamChieu = (int)hshCol["Số tham chiếu"];
                                    int indexGhiChu = (int)hshCol["Ghi chú"];
                                    int indexNguoiDuyet = (int)hshCol["Người duyệt"];
                                    int indexNguoiThuTien = (int)hshCol["Người thu tiền"];
                                    //kiem tra rang buoc tien trong hang tren duong con du thi moi import
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                        {
                                            try
                                            {

                                                //DataRow row = dtb.NewRow();
                                                DateTime NGAYCHI = new DateTime();
                                                DateTime NGAYHOACHTOAN = new DateTime();
                                                string ngaychi = ws.Cells[rowNum, indexNgayChi].Value == null ? "" : ws.Cells[rowNum, indexNgayChi].Value.ToString();
                                                SoChungTu = ws.Cells[rowNum, indexSoChungTu].Value == null ? "" : ws.Cells[rowNum, indexSoChungTu].Value.ToString();
                                                string NgayHoachToan = ws.Cells[rowNum, indexNgayHoachToan].Value == null ? "" : ws.Cells[rowNum, indexNgayHoachToan].Value.ToString();
                                                string DienThoai = ws.Cells[rowNum, indexDienThoai].Value == null ? "" : ws.Cells[rowNum, indexDienThoai].Value.ToString();
                                                string LyDoChi = ws.Cells[rowNum, indexLyDoChi].Value == null ? "" : ws.Cells[rowNum, indexLyDoChi].Value.ToString();
                                                string DiaChi = ws.Cells[rowNum, indexDiaChi].Value == null ? "" : ws.Cells[rowNum, indexDiaChi].Value.ToString();
                                                string HoTen = ws.Cells[rowNum, indexHoTen].Value == null ? "" : ws.Cells[rowNum, indexHoTen].Value.ToString();
                                                string NoiDung = ws.Cells[rowNum, indexNoiDung].Value == null ? "" : ws.Cells[rowNum, indexNoiDung].Value.ToString();


                                                string NguoiDuyet = ws.Cells[rowNum, indexNguoiDuyet].Value == null ? "" : ws.Cells[rowNum, indexNguoiDuyet].Value.ToString();

                                                string NganHang = ws.Cells[rowNum, indexNganHang].Value == null ? "" : ws.Cells[rowNum, indexNganHang].Value.ToString();
                                                string HTTT = ws.Cells[rowNum, indexHTTT].Value == null ? "" : ws.Cells[rowNum, indexHTTT].Value.ToString();
                                                string SoTien = ws.Cells[rowNum, indexSoTien].Value == null ? "" : ws.Cells[rowNum, indexSoTien].Value.ToString();
                                                string SoThamChieu = ws.Cells[rowNum, indexSoThamChieu].Value == null ? "" : ws.Cells[rowNum, indexSoThamChieu].Value.ToString();
                                                string GhiChu = ws.Cells[rowNum, indexGhiChu].Value == null ? "" : ws.Cells[rowNum, indexGhiChu].Value.ToString();
                                                //string TreoTien = ws.Cells[rowNum, indexTreoTien].Value == null ? "" : ws.Cells[rowNum, indexTreoTien].Value.ToString();
                                                string NguoiThuTien = ws.Cells[rowNum, indexNguoiThuTien].Value == null ? "" : ws.Cells[rowNum, indexNguoiThuTien].Value.ToString();
                                                HTTT = HTTT.ToUpper();
                                                LyDoChi = LyDoChi.ToUpper();
                                                if (string.IsNullOrEmpty(SoTien))
                                                    SoTien = "0";
                                                if (!TNK.Core.Common.IsNumeric(SoTien) && !string.IsNullOrEmpty(SoTien))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền (" + SoTien + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && !TNK.Core.Common.IsNumeric(SoTien))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền (" + SoTien + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(HTTT) && HTTT != "CK" && HTTT != "CT" && HTTT != "TM")
                                                {
                                                    error = "Dòng " + rowNum + " sai hình thức thanh toán.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && !_doiTacService.getDoiTacById(NganHang) && NganHang != "QuyChinh" && NganHang != "QuyPhu")
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy ngân hàng " + NganHang;
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && Convert.ToDouble(SoTien) <= 0 && !string.IsNullOrEmpty(HTTT))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền không được nhỏ hơn bằng 0.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(SoChungTu))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Vui lòng nhập số chứng từ.";
                                                    break;
                                                }
                                                User us = _userService.getByUserName(NguoiThuTien);
                                                if (us == null)
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy người thu tiền " + NguoiThuTien;
                                                    break;
                                                }
                                                if (HTTT == "TM" && NganHang != "QuyChinh" && NganHang != "QuyPhu")
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " sai hình thức thanh toán";
                                                    break;
                                                }
                                                if (HTTT != "TM" && (NganHang == "QuyChinh" || NganHang == "QuyPhu"))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " sai hình thức thanh toán";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(NguoiDuyet))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập người duyệt";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(LyDoChi))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập lý do chi";
                                                    break;
                                                }
                                                if (LyDoChi.Trim() != "CP" && LyDoChi.Trim() != "GCN" && LyDoChi.Trim() != "VTX" && LyDoChi.Trim() != "KHAC" && LyDoChi.Trim() != "GTX")
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng kiểm tra lại lý do chi";
                                                    break;
                                                }
                                                //if (!string.IsNullOrEmpty(NguoiDuyet))
                                                //{

                                                //}
                                                if ((Convert.ToDouble(SoTien) > 0 && (string.IsNullOrEmpty(NganHang) || string.IsNullOrEmpty(HTTT)))
                                                    || (!string.IsNullOrEmpty(NganHang) && (string.IsNullOrEmpty(SoTien) || string.IsNullOrEmpty(HTTT)))
                                                    || (!string.IsNullOrEmpty(HTTT) && (string.IsNullOrEmpty(NganHang) || string.IsNullOrEmpty(SoTien)))
                                                    )
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập đủ thông tin thanh toán(ngân hàng, HTTT,số tiền).";
                                                    break;
                                                }
                                                if (TNK.Core.Common.IsDate(ngaychi) == false)
                                                {
                                                    if (TNK.Core.Common.IsNumeric(ngaychi))
                                                    {
                                                        double DATE = double.Parse(ngaychi);
                                                        NGAYCHI = DateTime.FromOADate(DATE);
                                                    }
                                                }
                                                else
                                                    NGAYCHI = Convert.ToDateTime(ngaychi);
                                                if (TNK.Core.Common.IsDate(NgayHoachToan) == false)
                                                {
                                                    if (TNK.Core.Common.IsNumeric(NgayHoachToan))
                                                    {
                                                        double DATE = double.Parse(NgayHoachToan);
                                                        NGAYHOACHTOAN = DateTime.FromOADate(DATE);
                                                    }
                                                }
                                                else
                                                    NGAYHOACHTOAN = Convert.ToDateTime(NgayHoachToan);
                                                if (NGAYCHI.Year > 2000 && NGAYHOACHTOAN.Year > 2000)
                                                {
                                                    PhieuChi item = new PhieuChi();
                                                    List<ChiTietPhieuChi> CTPT = new List<ChiTietPhieuChi>();
                                                    item.MaLoaiPhieu = "CTAUN";
                                                    item.SoChungTu = SoChungTu;
                                                    item.NgayChi = Convert.ToDateTime(NGAYCHI);
                                                    item.NgayHachToan = Convert.ToDateTime(NGAYHOACHTOAN);
                                                    item.NguoiThuTien = us.UserId;
                                                    item.GhiChu = GhiChu;
                                                    item.ConLai = item.TongCong;
                                                    item.HoTen = HoTen;
                                                    item.DiaChi = DiaChi;
                                                    item.DienThoai = DienThoai;
                                                    item.NoiDung = NoiDung;
                                                    item.SoTienChi = Convert.ToDouble(SoTien);
                                                    item.GhiChu = GhiChu;
                                                    item.TongCong = item.SoTienChi;
                                                    item.NguoiDuyet = NguoiDuyet;
                                                    item.LyDoChi = LyDoChi;
                                                    //phan httt
                                                    HTTT = HTTT.ToUpper();
                                                    CTPT.Add(new ChiTietPhieuChi());
                                                    CTPT[0].HinhThucThanhToan = HTTT;
                                                    NganHang = NganHang.ToUpper();
                                                    CTPT[0].MaNganHang = NganHang;
                                                    CTPT[0].SoTienThanhToan = Convert.ToDouble(SoTien);
                                                    CTPT[0].Temp = NganHang + "-" + HTTT;
                                                    CTPT[0].SoThamChieu = SoThamChieu;
                                                    //if (TreoTien == "1")
                                                    //    CTPT[0].TinhTrang = true;
                                                    //else
                                                    //    CTPT[0].TinhTrang = false;

                                                    double TongThanhToan = 0;
                                                    foreach (var thanhtoan in CTPT)
                                                    {
                                                        TongThanhToan += thanhtoan.SoTienThanhToan;
                                                    }
                                                    item.SoTienChi = TongThanhToan;
                                                    if (item.SoTienChi > item.TongCong)
                                                    {
                                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " số tiền chi phải nhỏ hơn số tiền cần thanh toán";
                                                        break;
                                                    }
                                                    lstPhieuChi.Add(item);
                                                    lstChiTietPhieuChi.Add(CTPT);
                                                    #endregion
                                                }
                                                else
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Sai định dạng ngày thu. Vui lòng kiểm tra lại.";

                                            }
                                            catch (Exception exx)
                                            {
                                                error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": " + exx;
                                            }
                                        }
                                    }
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        for (int i = 0; i < lstPhieuChi.Count; i++)
                                        {

                                            string xml = "<formdata><Action>PhieuThu</Action>";
                                            xml += Environment.NewLine + "<submit>Import từ excel</submit>";
                                            if (lstPhieuChi[i] != null)
                                                xml += TNK.Core.Common.ConvertObjectToXMLString(lstPhieuChi[i]);
                                            if (lstChiTietPhieuChi[i] != null)
                                            {
                                                xml += Environment.NewLine + "<ThanhToan>";
                                                foreach (ChiTietPhieuChi ct in lstChiTietPhieuChi[i])
                                                    xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                                                xml += Environment.NewLine + "</ThanhToan>";
                                            }
                                            xml += "</formdata>";

                                            string message = _soChiService.CreatePhieuChi(lstPhieuChi[i], lstChiTietPhieuChi[i], true, xml);
                                            if (message != "")
                                            {
                                                error += Environment.NewLine + "Dòng " + (i + 1) + "(" + SoChungTu + "):" + message;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                _logger.WriteLog("SoThuController.ImportData_TCBX:" + objEx);
                return Json(new { IsError = true, Message = "Lỗi import excel." });
            }
            if (string.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    IsError = false,
                    Message = ""
                });
            }
            else
            {
                _logger.WriteLog("SoThuController.ImportData_TCBX:" + error);
                return Json(new { IsError = true, Message = error });
            }
        }

        public FileResult ExportFile_NoGiaCongNgoai()
        {
            var pck = new ExcelPackage();
            string strFileName = "~\\Views\\FileMau\\FileImport_NoGCN.xlsx";
            return File(strFileName, "application/vnd.ms-excel", "FileImport_NoGCN.xlsx");
        }
        public ActionResult ImportData_NoGiaCongNgoai()
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string error = "";
            try
            {
                Hashtable hshCol = new Hashtable();
                hshCol.Add("STT", 1);
                hshCol.Add("Số Quyết Toán", 0);
                hshCol.Add("Tên Công Việc", 0);
                hshCol.Add("Số tiền nợ", 0);

                List<PhieuChi> lstPhieuChi = new List<PhieuChi>();
                List<List<ChiTietPhieuChi>> lstChiTietPhieuChi = new List<List<ChiTietPhieuChi>>();
                int beginRowNum = 1;
                string SoChungTu = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
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

                                    for (int ii = 1; ii <= 20; ii++)
                                    {
                                        if (ws.Cells[beginRowNum, ii].Value != null)
                                        {
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("STT"))
                                                hshCol["STT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số Quyết Toán"))
                                                hshCol["Số Quyết Toán"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Tên Công Việc"))
                                                hshCol["Tên Công Việc"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tiền nợ"))
                                                hshCol["Số tiền nợ"] = ii;
                                        }

                                    }
                                    //neu co cot khong ton tai trong danh sach cot can thiet thi canh bao sai dinh dang
                                    foreach (DictionaryEntry o in hshCol)
                                        if (o.Value.ToString() == "0")
                                        {
                                            return Json(new { IsError = true, Message = "File excel thiếu cột " + o.Key.ToString() + ". Vui lòng kiểm tra lại." }, JsonRequestBehavior.AllowGet);
                                        }
                                    #endregion

                                    #region Add dữ liệu
                                    int indexSoQuyetToan = (int)hshCol["Số Quyết Toán"];
                                    int indexTenCongViec = (int)hshCol["Tên Công Việc"];
                                    int indexSoTienNo = (int)hshCol["Số tiền nợ"];
                                    DataTable tblImport = new DataTable("Import");
                                    tblImport.Columns.Add("STT");
                                    tblImport.Columns.Add("SoQuyetToan");
                                    tblImport.Columns.Add("TenCongViec");
                                    tblImport.Columns.Add("SoTienNo");
                                    int dong = 1;
                                    //kiem tra rang buoc tien trong hang tren duong con du thi moi import
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                        {
                                            try
                                            {

                                                //DataRow row = dtb.NewRow();

                                                string soQuyetToan = ws.Cells[rowNum, indexSoQuyetToan].Value == null ? "" : ws.Cells[rowNum, indexSoQuyetToan].Value.ToString();
                                                string tenCongViec = ws.Cells[rowNum, indexTenCongViec].Value == null ? "" : ws.Cells[rowNum, indexTenCongViec].Value.ToString();
                                                string soTienNo = ws.Cells[rowNum, indexSoTienNo].Value == null ? "0" : ws.Cells[rowNum, indexSoTienNo].Value.ToString();

                                                if (string.IsNullOrEmpty(soTienNo))
                                                    soTienNo = "0";
                                                if (!TNK.Core.Common.IsNumeric(soTienNo) && !string.IsNullOrEmpty(soTienNo))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền (" + soTienNo + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(soQuyetToan))
                                                {
                                                    error = "Dòng " + rowNum + " không có số quyết toán.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(tenCongViec))
                                                {
                                                    error = "Dòng " + rowNum + " không có tên công việc.";
                                                    break;
                                                }
                                                //tim dong chi tiet dich vu tuong ung voi RO và ten cong việc
                                                DataRow r = tblImport.NewRow();
                                                r["STT"] = dong++;
                                                r["SoQuyetToan"] = soQuyetToan;
                                                r["TenCongViec"] = tenCongViec;
                                                r["SoTienNo"] = soTienNo;
                                                tblImport.Rows.Add(r);

                                            }
                                            catch (Exception exx)
                                            {
                                                error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": " + exx;
                                            }
                                        }
                                    }
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        for (int i = 0; i < lstPhieuChi.Count; i++)
                                        {

                                            string xml = "<formdata><Action>PhieuThu</Action>";
                                            xml += Environment.NewLine + "<submit>Import từ excel</submit>";
                                            if (lstPhieuChi[i] != null)
                                                xml += TNK.Core.Common.ConvertObjectToXMLString(lstPhieuChi[i]);
                                            if (lstChiTietPhieuChi[i] != null)
                                            {
                                                xml += Environment.NewLine + "<ThanhToan>";
                                                foreach (ChiTietPhieuChi ct in lstChiTietPhieuChi[i])
                                                    xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                                                xml += Environment.NewLine + "</ThanhToan>";
                                            }
                                            xml += "</formdata>";

                                            string message = _soChiService.CreatePhieuChi(lstPhieuChi[i], lstChiTietPhieuChi[i], true, xml);
                                            if (message != "")
                                            {
                                                error += Environment.NewLine + "Dòng " + (i + 1) + "(" + SoChungTu + "):" + message;
                                            }
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
                _logger.WriteLog("SoThuController.ImportData_TCBX:" + objEx);
                return Json(new { IsError = true, Message = "Lỗi import excel." });
            }
            if (string.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    IsError = false,
                    Message = ""
                });
            }
            else
            {
                _logger.WriteLog("SoThuController.ImportData_TCBX:" + error);
                return Json(new { IsError = true, Message = error });
            }
        }
        #endregion

        //Phan import chi mua phu tùng
        #region CMPT
        public FileResult ExportFile_CMPT()
        {
            var pck = new ExcelPackage();
            string strFileName = "~\\Views\\FileMau\\Import_MuaPhuTung.xlsx";
            return File(strFileName, "application/vnd.ms-excel", "Import_MuaPhuTung.xlsx");
        }

        ///phần import CMPT
        #region Import Chi mua phụ tùng
        public ActionResult ImportData_CMPT()
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string error = "";
            try
            {
                Hashtable hshCol = new Hashtable();
                hshCol.Add("STT", 1);
                hshCol.Add("Số chứng từ", 0);
                hshCol.Add("Ngày chi", 0);
                hshCol.Add("Ngày hoạch toán", 0);
                hshCol.Add("Nhà cung cấp", 0);
                hshCol.Add("Tổng tiền", 0);
                hshCol.Add("Số lượng", 0);

                hshCol.Add("Ngày nhập kho", 0);
                hshCol.Add("Kho nhập", 0);
                //hshCol.Add("Số tiền", 0);
                hshCol.Add("Nội dung", 0);
                hshCol.Add("Ngân hàng", 0);
                hshCol.Add("HTTT", 0);
                hshCol.Add("Số tiền thanh toán", 0);

                hshCol.Add("Số tham chiếu", 0);
                hshCol.Add("Ghi chú", 0);
                hshCol.Add("Người duyệt", 0);
                hshCol.Add("Người thu tiền", 0);



                List<PhieuChi> lstPhieuChi = new List<Core.Domain.PhieuChi>();
                List<List<ChiTietPhieuChi>> lstChiTietPhieuChi = new List<List<ChiTietPhieuChi>>();
                List<List<ChiTietNhapKhoPhuTung>> ListCTNKPT = new List<List<ChiTietNhapKhoPhuTung>>();
                List<ChiTietPhieuChi> Coc = new List<ChiTietPhieuChi>();
                int beginRowNum = 1;
                string SoChungTu = "";
                double SoDongCuaFileExcel = 0;
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        Stream stream = fileContent.InputStream;
                        using (var package = new ExcelPackage(stream))
                        {
                            var workBook = package.Workbook;
                            if (workBook != null)
                            {
                                //SoDongCuaFileExcel = workBook.Worksheets.Count - 1;
                                if (workBook.Worksheets.Count > 0)
                                {
                                    var ws = workBook.Worksheets[1];
                                    SoDongCuaFileExcel = ws.Dimension.End.Row;
                                    if (ws.Dimension.End.Row < 2)
                                    {
                                        return Json(new { IsError = true, Message = "Không có dữ liệu để import" }, JsonRequestBehavior.AllowGet);
                                    }

                                    #region Kiểm tra file đúng định dạng

                                    for (int ii = 1; ii <= 20; ii++)
                                    {
                                        if (ws.Cells[beginRowNum, ii].Value != null)
                                        {
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("STT"))
                                                hshCol["STT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số chứng từ"))
                                                hshCol["Số chứng từ"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngày chi"))
                                                hshCol["Ngày chi"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngày hoạch toán"))
                                                hshCol["Ngày hoạch toán"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Nhà cung cấp"))
                                                hshCol["Nhà cung cấp"] = ii;

                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Tổng tiền"))
                                                hshCol["Tổng tiền"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số lượng"))
                                                hshCol["Số lượng"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngày nhập kho"))
                                                hshCol["Ngày nhập kho"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Kho nhập"))
                                                hshCol["Kho nhập"] = ii;
                                            //if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tiền"))
                                            //    hshCol["Số tiền"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Nội dung"))
                                                hshCol["Nội dung"] = ii;

                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ngân hàng"))
                                                hshCol["Ngân hàng"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("HTTT"))
                                                hshCol["HTTT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tiền thanh toán"))
                                                hshCol["Số tiền thanh toán"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tham chiếu"))
                                                hshCol["Số tham chiếu"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Ghi chú"))
                                                hshCol["Ghi chú"] = ii;

                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Người duyệt"))
                                                hshCol["Người duyệt"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Người thu tiền"))
                                                hshCol["Người thu tiền"] = ii;
                                        }

                                    }
                                    //neu co cot khong ton tai trong danh sach cot can thiet thi canh bao sai dinh dang
                                    foreach (DictionaryEntry o in hshCol)
                                        if (o.Value.ToString() == "0")
                                        {
                                            return Json(new { IsError = true, Message = "File excel thiếu cột " + o.Key.ToString() + ". Vui lòng kiểm tra lại." }, JsonRequestBehavior.AllowGet);
                                        }
                                    #endregion

                                    #region Add dữ liệu
                                    int indexSoChungTu = (int)hshCol["Số chứng từ"];
                                    int indexNgayChi = (int)hshCol["Ngày chi"];
                                    int indexNgayHoachToan = (int)hshCol["Ngày hoạch toán"];
                                    int indexNhaCungCap = (int)hshCol["Nhà cung cấp"];
                                    int indexTongTien = (int)hshCol["Tổng tiền"];
                                    int indexSoLuong = (int)hshCol["Số lượng"];

                                    int indexNgayNhapKho = (int)hshCol["Ngày nhập kho"];
                                    int indexKhoNhap = (int)hshCol["Kho nhập"];
                                    //int indexSoTien = (int)hshCol["Số tiền"];
                                    int indexNoiDung = (int)hshCol["Nội dung"];
                                    int indexNganHang = (int)hshCol["Ngân hàng"];

                                    int indexHTTT = (int)hshCol["HTTT"];
                                    int indexSoTienThanhToan = (int)hshCol["Số tiền thanh toán"];
                                    int indexSoThamChieu = (int)hshCol["Số tham chiếu"];
                                    int indexGhiChu = (int)hshCol["Ghi chú"];
                                    int indexNguoiDuyet = (int)hshCol["Người duyệt"];


                                    int indexNguoiThuTien = (int)hshCol["Người thu tiền"];



                                    //kiem tra rang buoc tien trong hang tren duong con du thi moi import
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        double dem = 0;
                                        for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                        {
                                            try
                                            {
                                                if (dem == SoDongCuaFileExcel)
                                                    break;
                                                //DataRow row = dtb.NewRow();
                                                DateTime NGAYCHI = new DateTime();
                                                DateTime NGAYHOACHTOAN = new DateTime();
                                                DateTime NGAYNHAPKHO = new DateTime();
                                                string ngayChi = ws.Cells[rowNum, indexNgayChi].Value == null ? "" : ws.Cells[rowNum, indexNgayChi].Value.ToString();
                                                SoChungTu = ws.Cells[rowNum, indexSoChungTu].Value == null ? "" : ws.Cells[rowNum, indexSoChungTu].Value.ToString();
                                                string NgayHoachToan = ws.Cells[rowNum, indexNgayHoachToan].Value == null ? "" : ws.Cells[rowNum, indexNgayHoachToan].Value.ToString();
                                                string NhaCungCap = ws.Cells[rowNum, indexNhaCungCap].Value == null ? "" : ws.Cells[rowNum, indexNhaCungCap].Value.ToString();
                                                string TongTien = ws.Cells[rowNum, indexTongTien].Value == null ? "" : ws.Cells[rowNum, indexTongTien].Value.ToString();
                                                string SoLuong = ws.Cells[rowNum, indexSoLuong].Value == null ? "" : ws.Cells[rowNum, indexSoLuong].Value.ToString();
                                                string NgayNhapKho = ws.Cells[rowNum, indexNgayNhapKho].Value == null ? "" : ws.Cells[rowNum, indexNgayNhapKho].Value.ToString();

                                                string KhoNhap = ws.Cells[rowNum, indexKhoNhap].Value == null ? "" : ws.Cells[rowNum, indexKhoNhap].Value.ToString();
                                                string NoiDung = ws.Cells[rowNum, indexNoiDung].Value == null ? "" : ws.Cells[rowNum, indexNoiDung].Value.ToString();
                                                string NganHang = ws.Cells[rowNum, indexNganHang].Value == null ? "" : ws.Cells[rowNum, indexNganHang].Value.ToString();
                                                string HTTT = ws.Cells[rowNum, indexHTTT].Value == null ? "" : ws.Cells[rowNum, indexHTTT].Value.ToString();
                                                string SoTienThanhToan = ws.Cells[rowNum, indexSoTienThanhToan].Value == null ? "" : ws.Cells[rowNum, indexSoTienThanhToan].Value.ToString();

                                                string SoThamChieu = ws.Cells[rowNum, indexSoThamChieu].Value == null ? "" : ws.Cells[rowNum, indexSoThamChieu].Value.ToString();
                                                string GhiChu = ws.Cells[rowNum, indexGhiChu].Value == null ? "" : ws.Cells[rowNum, indexGhiChu].Value.ToString();
                                                string NguoiDuyet = ws.Cells[rowNum, indexNguoiDuyet].Value == null ? "" : ws.Cells[rowNum, indexNguoiDuyet].Value.ToString();
                                                string NguoiThuTien = ws.Cells[rowNum, indexNguoiThuTien].Value == null ? "" : ws.Cells[rowNum, indexNguoiThuTien].Value.ToString();
                                                HTTT = HTTT.ToUpper();
                                                //NganHang = NganHang.ToUpper();
                                                if (string.IsNullOrEmpty(TongTien))
                                                {
                                                    error = "Dòng " + rowNum + " tổng tiền không được trống.";
                                                    break;
                                                }
                                                if (!TNK.Core.Common.IsNumeric(TongTien) && !string.IsNullOrEmpty(TongTien))
                                                {
                                                    error = "Dòng " + rowNum + " giá bán (" + TongTien + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(SoLuong))
                                                {
                                                    error = "Dòng " + rowNum + " số lượng không được trống.";
                                                    break;
                                                }
                                                if (TNK.Core.Common.IsNumeric(SoLuong) && decimal.Parse(SoLuong)==0)
                                                {
                                                    error = "Dòng " + rowNum + " số lượng phải > 0";
                                                    break;
                                                }
                                                if (!TNK.Core.Common.IsNumeric(SoLuong) && !string.IsNullOrEmpty(SoLuong))
                                                {
                                                    error = "Dòng " + rowNum + " số lượng (" + SoLuong + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(SoTienThanhToan))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền thanh toán không được trống.";
                                                    break;
                                                }

                                                if (!TNK.Core.Common.IsNumeric(SoTienThanhToan))
                                                {
                                                    error = "Dòng " + rowNum + " hoa hồng tài xế (" + SoTienThanhToan + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && !TNK.Core.Common.IsNumeric(SoTienThanhToan))
                                                {
                                                    error = "Dòng " + rowNum + " số tiền (" + SoTienThanhToan + ") không phải là kiểu số.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(HTTT) && HTTT != "CK" && HTTT != "CT" && HTTT != "TM")
                                                {
                                                    error = "Dòng " + rowNum + " sai hình thức thanh toán.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && !_doiTacService.getDoiTacById(NganHang) && NganHang != "QuyChinh" && NganHang != "QuyPhu")
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy ngân hàng " + NganHang;
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(SoChungTu))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Vui lòng nhập số chứng từ.";
                                                    break;
                                                }
                                                //if (string.IsNullOrEmpty(SoChungTu))
                                                //{
                                                //    error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Vui lòng nhập số chứng từ.";
                                                //    break;
                                                //}
                                                if (string.IsNullOrEmpty(NgayHoachToan))
                                                {
                                                    error = "Dòng " + rowNum + " ngày hoạch toán không được trống.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(ngayChi))
                                                {
                                                    error = "Dòng " + rowNum + " ngày chi không được trống.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(NhaCungCap))
                                                {
                                                    error = "Dòng " + rowNum + " nhà cung cấp không được trống.";
                                                    break;
                                                }
                                                string NCC = _soChiService.CheckNCC_CMPT_Import(NhaCungCap);

                                                if (NCC == "")
                                                {
                                                    error = "Dòng " + rowNum + " không tìm thấy nhà cung cấp.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(KhoNhap))
                                                {
                                                    error = "Dòng " + rowNum + " kho nhập không được trống.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(NoiDung))
                                                {
                                                    error = "Dòng " + rowNum + " nội dung không được trống.";
                                                    break;
                                                }
                                                //phần check kho nhập dưới database
                                                bool CheckMaKhoNhap = _soChiService.CheckKhoNhap(KhoNhap);
                                                if (CheckMaKhoNhap == false)
                                                {
                                                    error = "Dòng " + rowNum + " không tìm thấy kho nhập.";
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(NguoiThuTien))
                                                {
                                                    error = "Dòng " + rowNum + " người thu tiền không được rỗng.";
                                                    break;
                                                }
                                                User us = _userService.getByUserName(NguoiThuTien);
                                                if (us == null)
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy người thu tiền " + NguoiThuTien;
                                                    break;
                                                }
                                                //phần về ngân hàng và httt.
                                                if (HTTT == "TM" && NganHang != "QuyChinh" && NganHang != "QuyPhu")
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " sai hình thức thanh toán";
                                                    break;
                                                }
                                                if (HTTT != "TM" && (NganHang == "QuyChinh" || NganHang == "QuyPhu"))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " sai hình thức thanh toán";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && string.IsNullOrEmpty(HTTT))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " hình thức thanh toán không được trống.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && string.IsNullOrEmpty(SoTienThanhToan))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " số tiền thanh toán không được trống.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && Convert.ToDouble(SoTienThanhToan) == 0)
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " số tiền thanh toán không được bằng 0.";
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(NganHang) && string.IsNullOrEmpty(HTTT))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " hình thức thanh toán không được trống.";
                                                    break;
                                                }
                                                //phần số tiền:
                                                if (Convert.ToDouble(SoTienThanhToan) > Convert.ToDouble(TongTien))
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " số tiền thanh toán không lớn hơn tổng tiền.";
                                                    break;
                                                }
                                                //if (!_phieuThuService.KiemTraSoQuyetToan(SoChungTu))
                                                //{
                                                //    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " đã bị trùng số chứng từ";
                                                //    break;
                                                //}
                                                if ((Convert.ToDouble(SoTienThanhToan) > 0 && (string.IsNullOrEmpty(NganHang) || string.IsNullOrEmpty(HTTT)))
                                                    || (!string.IsNullOrEmpty(NganHang) && (string.IsNullOrEmpty(SoTienThanhToan) || string.IsNullOrEmpty(HTTT)))
                                                    || (!string.IsNullOrEmpty(HTTT) && (string.IsNullOrEmpty(NganHang) || string.IsNullOrEmpty(SoTienThanhToan)))
                                                    )
                                                {
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập đủ thông tin thanh toán(ngân hàng, HTTT,số tiền).";
                                                    break;
                                                }
                                                //Ngày chi
                                                if (TNK.Core.Common.IsDate(ngayChi) == false)
                                                {
                                                    if (TNK.Core.Common.IsNumeric(ngayChi))
                                                    {
                                                        double DATE = double.Parse(ngayChi);
                                                        NGAYCHI = DateTime.FromOADate(DATE);
                                                    }
                                                }
                                                else
                                                    NGAYCHI = Convert.ToDateTime(ngayChi);
                                                //Ngày nhập kho
                                                if (!string.IsNullOrEmpty(NgayNhapKho))
                                                {
                                                    if (TNK.Core.Common.IsDate(NgayNhapKho) == false)
                                                    {
                                                        if (TNK.Core.Common.IsNumeric(NgayNhapKho))
                                                        {
                                                            double DATE = double.Parse(NgayNhapKho);
                                                            NGAYNHAPKHO = DateTime.FromOADate(DATE);
                                                        }
                                                    }
                                                    else
                                                        NGAYNHAPKHO = Convert.ToDateTime(NgayNhapKho);
                                                }
                                                else
                                                    NGAYNHAPKHO = DateTime.Now;
                                                //Ngày hoạch toán
                                                if (TNK.Core.Common.IsDate(NgayHoachToan) == false)
                                                {
                                                    if (TNK.Core.Common.IsNumeric(NgayHoachToan))
                                                    {
                                                        double DATE = double.Parse(NgayHoachToan);
                                                        NGAYHOACHTOAN = DateTime.FromOADate(DATE);
                                                    }
                                                }
                                                else
                                                    NGAYHOACHTOAN = Convert.ToDateTime(NgayHoachToan);

                                                if (NGAYCHI.Year > 2000)
                                                {
                                                    PhieuChi item = new PhieuChi();
                                                    List<ChiTietPhieuChi> CTPC = new List<ChiTietPhieuChi>();
                                                    List<ChiTietNhapKhoPhuTung> CTNKPT = new List<ChiTietNhapKhoPhuTung>();
                                                    item.MaLoaiPhieu = "CMPTU";
                                                    item.SoChungTu = SoChungTu;
                                                    item.NgayChi = Convert.ToDateTime(NGAYCHI);
                                                    item.NgayHachToan = Convert.ToDateTime(NGAYHOACHTOAN);
                                                    item.NguoiThuTien = us.UserId;
                                                    item.GhiChu = GhiChu;
                                                    item.NgayNhapKho = NGAYNHAPKHO;
                                                    item.SoChungTu = SoChungTu;
                                                    item.NoiDung = NoiDung;
                                                    item.TongCong = Convert.ToDouble(TongTien);
                                                    item.DoiTac = NCC;
                                                    item.NhaCungCap = NCC;
                                                    //item.SoTienChi = Convert.ToDouble(SoTienThanhToan);
                                                    //item.ConLai = item.TongCong - item.SoTienChi;
                                                    // nhớ tạo phiếu nhâp
                                                    //phan httt
                                                    //HTTT = HTTT.ToUpper();
                                                    CTPC.Add(new ChiTietPhieuChi());
                                                    CTPC[0].HinhThucThanhToan = HTTT;
                                                    //NganHang = NganHang.ToUpper();
                                                    CTPC[0].MaNganHang = NganHang;
                                                    CTPC[0].SoTienThanhToan = Convert.ToDouble(SoTienThanhToan);
                                                    CTPC[0].Temp = NganHang + "-" + HTTT;
                                                    CTPC[0].SoThamChieu = SoThamChieu;
                                                    //double TongThanhToan = 0;
                                                    //foreach (var thanhtoan in CTPC)
                                                    //{
                                                    //    TongThanhToan += thanhtoan.SoTienThanhToan;
                                                    //}

                                                    //if (item.SoTienThu > item.TongCong)
                                                    //{
                                                    //    error += Environment.NewLine + "Dòng " + (rowNum - 1) + " số tiền thu phải nhỏ hơn số tiền cần thanh toán";
                                                    //    break;
                                                    //}
                                                    //phan phieu nhap kho
                                                    CTNKPT.Add(new ChiTietNhapKhoPhuTung());
                                                    CTNKPT[0].MaKho = KhoNhap.ToUpper().Trim();
                                                    CTNKPT[0].NgayNhapKho = Convert.ToDateTime(NGAYNHAPKHO);
                                                    CTNKPT[0].SoTien = Convert.ToDouble(TongTien);
                                                    //CTNKPT[0].SoLuong = Convert.ToDouble(SoLuong);
                                                    lstPhieuChi.Add(item);
                                                    lstChiTietPhieuChi.Add(CTPC);
                                                    ListCTNKPT.Add(CTNKPT);
                                                    dem++;
                                                    #endregion
                                                }
                                                else
                                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Sai định dạng ngày thu. Vui lòng kiểm tra lại.";

                                            }
                                            catch (Exception exx)
                                            {
                                                error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": " + exx;
                                            }
                                        }
                                    }
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        for (int i = 0; i < lstPhieuChi.Count; i++)
                                        {

                                            string xml = "<formdata><Action>PhieuThu</Action>";
                                            xml += Environment.NewLine + "<submit>Import từ excel</submit>";
                                            if (lstPhieuChi[i] != null)
                                                xml += TNK.Core.Common.ConvertObjectToXMLString(lstPhieuChi[i]);
                                            if (lstChiTietPhieuChi[i] != null)
                                            {
                                                xml += Environment.NewLine + "<ThanhToan>";
                                                foreach (ChiTietPhieuChi ct in lstChiTietPhieuChi[i])
                                                    xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                                                xml += Environment.NewLine + "</ThanhToan>";
                                            }
                                            if (ListCTNKPT != null)
                                            {
                                                xml += Environment.NewLine + "<ChiTietNhapKhoPhuTung>";
                                                foreach (ChiTietNhapKhoPhuTung ctknpt in ListCTNKPT[i])
                                                    xml += TNK.Core.Common.ConvertObjectToXMLString(ctknpt);
                                                xml += Environment.NewLine + "</ChiTietNhapKho>";
                                            }
                                            xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";
                                            xml += "</formdata>";
                                            //_phieuThuService.CreatePhieuThu(item, httt, false, xml);
                                            //_soChiService.CreatePhieuChi(lstPhieuChi[i], lstChiTietPhieuChi[i], ListCTNKPT[i], CTPC, true, xml)
                                            string messagePC = _soChiService.CreatePhieuChi_ImportCMPT(lstPhieuChi[i], lstChiTietPhieuChi[i], ListCTNKPT[i], Coc, true, xml);
                                            if (!string.IsNullOrEmpty(messagePC))
                                                error += Environment.NewLine + "Dòng " + (i + 1) + "(" + SoChungTu + "):" + messagePC;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                _logger.WriteLog("SoThuController.ImportData_CMPT:" + objEx);
                return Json(new { IsError = true, Message = "Lỗi import excel." });
            }
            if (string.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    IsError = false,
                    Message = ""
                });
            }
            else
            {
                _logger.WriteLog("SoThuController.ImportData_CMPT:" + error);
                return Json(new { IsError = true, Message = error });
            }
        }
        #endregion
        #endregion

        public JsonResult GetTongTienKhauHao(string MaDuAn)
        {
            double TongCong = _soChiService.GetTongTienKhauHao("create", MaDuAn);
            return Json(TongCong);
        }
        public JsonResult GetKhoTaiSan(string NhomTaiSan)
        {
            try
            {
                List<ViewKhoTaiSan> ListKhoTS = _khoTaiSanService.GetTaiSan().Where(x => x.MaLoaiTS == NhomTaiSan && x.GiaTriConLai > 0).ToList();
                return Json(new { ListKhoTS = ListKhoTS, IsError = false });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true });
            }
        }
        public JsonResult GetTaiSan(string MaId)
        {
            try
            {
                ViewKhoTaiSan TaiSan = _khoTaiSanService.GetTaiSan().SingleOrDefault(x => x.KId == Guid.Parse(MaId));
                return Json(new { TaiSan });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true });
            }
        }

        public JsonResult GetChiDauTuDuAn(string DuAn)
        {
            try
            {
                List<PhieuChi> ListCDATU = _PhieuChiRepository.Table.Where(x => x.MaLoaiPhieu == "CDATU" && x.DuAn == DuAn && x.ConLai != 0 && x.IsDeleted == false).ToList();
                return Json(new { ListCDATU = ListCDATU, IsError = false });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true });
            }
        }
        public JsonResult GetPhieuChiDuAn(string MaId)
        {
            try
            {
                PhieuChi PhieuChi = _PhieuChiRepository.Table.SingleOrDefault(x => x.MaPhieuChi == MaId);
                return Json(new { PhieuChi });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true });
            }
        }
        [HttpPost]
        public ActionResult CKHDTCreate(PhieuChi item, List<CHI_TIET_KHAU_HAO> CTKH)
        {
            DateTime start = DateTime.Now;
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Content("Bạn không có quyền tạo mới");
                }
                string xml = "<Action>CKHACCreate</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);

                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";

                ViewBag.Title = "Tạo phiếu chi khác";
                item.MaLoaiPhieu = "CKHDT";
                //item.NgayChi = DateTime.Now;
                //item.NgayHachToan = DateTime.Now;
                CreateLink("CKHDT", "create");
                if (_soChiService.CreatePhieuChiKHDT(item, CTKH, CurrentUser.UserId))
                {
                    TempData["Info"] = "Tạo mới thành công";
                    return Redirect(ViewBag.LinkIndex);
                }
                else
                {
                    TempData["Error"] = "Lỗi tạo phiếu";
                    ViewBag.Error = "Tạo mới thất bại";
                }

                DateTime end = DateTime.Now;
                this.AddActionLog(CurrentUser.UserName, "SoChi", "CKHDTCreate", start, end, "");
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CKHDTCreate", ex);
                DateTime end = DateTime.Now;
                this.AddActionLog(CurrentUser.UserName, "SoChi", "CKHDTCreate", start, end, ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CKHDTDelete(string maphieuchi)
        {
            try
            {
                CreateLink("CKHDT", "edit");
                string message = _soChiService.DeletePhieuChi(maphieuchi);
                if (string.IsNullOrEmpty(message))
                {
                    return Content("success");
                }
                else
                    return Content("Xóa thất bại:" + message);
            }
            catch (Exception ex)
            {
                return Content(Constants.ERR_NORMAL);
            }

        }
        public ActionResult CKHDTEdit(string id)
        {
            try
            {
                ViewBag.Title = "Chi khấu hao đầu tư";
                SoChiModel model = GetModelCreate();
                model.Item = _soChiService.GetPhieuChi(id);
                model.DuAnDauTu = _soChiService.GetDuAnDauTuKhauHau("DUAN");
                ViewBag.CTKH = _soChiService.GetViewCTKH(id, "CDATU");
                ViewBag.SoTienKhauHao = _soChiService.GetTongTienKhauHao("update", model.Item.DuAn);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChi", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CKHTSIndex(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                ViewBag.Alert = TempData["Alert"];
                CreateLink("CKHTS", "index");
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CTAUN");
                List<ViewCKHHA> model = new List<ViewCKHHA>();
                model = _soChiService.GetCKHTS(f, t, p, ref total, pageSize, query);
                ViewBag.Total = total;
                return View(model);
            }

            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CTAUNIndex", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CKHTSEdit(string id)
        {
            try
            {
                ViewBag.Title = "Chi khấu hao tài sản";
                SoChiModel model = GetModelCreate();
                model.Item = _soChiService.GetPhieuChi(id);
                model.KhoTaiSan = _soChiService.GetListCTKH(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChi", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        public ActionResult CKHTSCreate()
        {
            try
            {
                SoChiModel model = GetModelCreate();
                model.Item = new PhieuChi();
                model.TenTaiSan = _soChiService.GetTenTaiSan("TS");
                model.KhoTaiSan = _khoTaiSanService.GetTaiSan();
                ViewBag.Title = "Chi khấu hao tài sản";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.PhieuChi", ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CKHTSCreate(PhieuChi item, List<ViewKhoTaiSan> TS)
        {
            DateTime start = DateTime.Now;
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Content("Bạn không có quyền tạo mới");
                }
                string xml = "<Action>CKHACCreate</Action>";
                if (item != null)
                    xml += TNK.Core.Common.ConvertObjectToXMLString(item);

                xml = "<formdata>" + Environment.NewLine + xml + Environment.NewLine + "</formdata>";

                ViewBag.Title = "Tạo phiếu chi khấu hao tài sản";
                item.MaLoaiPhieu = "CKHTS";
                //item.NgayChi = DateTime.Now;
                //item.NgayHachToan = DateTime.Now;
                CreateLink("CKHTS", "create");
                if (!_soChiService.CreatePhieuChiKHTS(item, TS, CurrentUser.UserId))
                {
                    ViewBag.Error = "Tạo mới thất bại";
                    TempData["Error"] = "Tạo mới thất bại";
                }
                TempData["Info"] = "Tạo mới thành công";
                DateTime end = DateTime.Now;
                this.AddActionLog(CurrentUser.UserName, "SoChi", "CKHDTCreate", start, end, "");
                return Redirect(ViewBag.LinkIndex);

            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiControler.CKHDTCreate", ex);
                DateTime end = DateTime.Now;
                this.AddActionLog(CurrentUser.UserName, "SoChi", "CKHDTCreate", start, end, ex);
                TempData["Error"] = Constants.ERR_NORMAL;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CKHTSDelete(string maphieuchi)
        {
            try
            {
                CreateLink("CKHTS", "edit");
                string message = _soChiService.DeletePhieuChi(maphieuchi);

                if (!string.IsNullOrEmpty(message))
                {
                    return Content("Xóa thất bại:" + message);
                }
                if (!_soChiService.DeletedCTKH(maphieuchi))
                {
                    return Content("Xóa thất bại:" + message);
                }
                return Content("success");
            }
            catch (Exception ex)
            {
                return Content(Constants.ERR_NORMAL);
            }

        }
        public ActionResult ChiTietChiNo(string MaPhieuNo)
        {
            var model = _soChiService.GetListLichSuChiNo(MaPhieuNo);
            ViewBag.Title = "LỊCH SỬ CHI NỢ";
            return View(model);
        }
        [HttpPost]
        public ActionResult ExportFileType(string type)
        {
            string strFileName = "";
            string filename = "";
            switch (type)
            {
                case "NGCNG":
                    strFileName = "~\\Views\\FileMau\\FileImport_NoGCN.xlsx";
                    filename = "FileImport_NoGCNG.xlsx";
                    break;
                case "HHTXE":
                    strFileName = "~\\Views\\FileMau\\FileImport_HHTXE.xlsx";
                    filename = "FileImport_HHTXE.xlsx";
                    break;
            }

            //var path = System.AppDomain.CurrentDomain.BaseDirectory;
            //var httt = _soThuService.GetHTTT().Select(x => x.MaNganHang).Distinct().ToList();
            //string str = Server.MapPath("~\\Views\\FileMau\\" + filename);
            string str = Server.MapPath(strFileName);
            FileInfo template = new FileInfo(str);
            ExcelPackage pck = new ExcelPackage(new FileInfo(str));
            return new ExcelInsuranceDownload(pck, filename);
        }
        [HttpPost]
        public ActionResult ImportData_NGCNG()
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string error = "";

            try
            {
                Hashtable hshCol = new Hashtable();
                hshCol.Add("STT", 1);
                hshCol.Add("Số Quyết Toán", 0);
                hshCol.Add("Tên Công Việc", 0);
                hshCol.Add("Số tiền nợ", 0);

                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
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
                                    for (int ii = 1; ii <= 20; ii++)
                                    {
                                        if (ws.Cells[beginRowNum, ii].Value != null)
                                        {
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("STT"))
                                                hshCol["STT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số Quyết Toán"))
                                                hshCol["Số Quyết Toán"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Tên Công Việc"))
                                                hshCol["Tên Công Việc"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tiền nợ"))
                                                hshCol["Số tiền nợ"] = ii;
                                        }

                                    }
                                    //neu co cot khong ton tai trong danh sach cot can thiet thi canh bao sai dinh dang
                                    foreach (DictionaryEntry o in hshCol)
                                        if (o.Value.ToString() == "0")
                                        {
                                            return Json(new { IsError = true, Message = "File excel thiếu cột " + o.Key.ToString() + ". Vui lòng kiểm tra lại." }, JsonRequestBehavior.AllowGet);
                                        }
                                    #endregion

                                    #region Add dữ liệu
                                    int indexSTT = (int)hshCol["STT"];
                                    int indexMaPhieu = (int)hshCol["Số Quyết Toán"];
                                    int indexTenCongViec = (int)hshCol["Tên Công Việc"];
                                    int indexSoTienNo = (int)hshCol["Số tiền nợ"];


                                    //kiem tra rang buoc tien trong hang tren duong con du thi moi import
                                    double tongTien = 0;
                                    for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                    {
                                        try
                                        {
                                            string STT = ws.Cells[rowNum, indexSTT].Value == null ? "" : ws.Cells[rowNum, indexSTT].Value.ToString();

                                            if (!string.IsNullOrEmpty(STT) && STT != "")
                                            {
                                                string soQuyetToan = ws.Cells[rowNum, indexMaPhieu].Value.ToString();
                                                if (string.IsNullOrEmpty(soQuyetToan))
                                                {
                                                    error = "Dòng " + (rowNum - 1) + " không có số quyết toán.";
                                                    break;
                                                }

                                                string tenCongViec = ws.Cells[rowNum, indexTenCongViec].Value.ToString();
                                                if (string.IsNullOrEmpty(tenCongViec))
                                                {
                                                    error = "Dòng " + (rowNum - 1) + " không có tên công việc.";
                                                    break;
                                                }
                                                string SOTIENNO = ws.Cells[rowNum, indexSoTienNo].Value == null ? "" : ws.Cells[rowNum, indexSoTienNo].Value.ToString();

                                                if (!TNK.Core.Common.IsNumeric(SOTIENNO))
                                                {
                                                    error = "Dòng " + (rowNum - 1) + " số tiền nợ (" + SOTIENNO + ") không phải là kiểu số.";
                                                    break;
                                                }

                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            error += e;

                                        }
                                    }
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        //TuiDinhKhoan tdk = new TuiDinhKhoan(); /*_tuiDinhKhoanService.GetTuiDinhKhoan(maTui);*/
                                        for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                        {
                                            try
                                            {
                                                string STT = ws.Cells[rowNum, indexSTT].Value == null ? "" : ws.Cells[rowNum, indexSTT].Value.ToString();
                                                if (!string.IsNullOrEmpty(STT) && STT != "")
                                                {
                                                    //DataRow row = dtb.NewRow();
                                                    double soTienNo = Convert.ToDouble(ws.Cells[rowNum, indexSoTienNo].Value.ToString());
                                                    string soQuyetToan = ws.Cells[rowNum, indexMaPhieu].Value.ToString();
                                                    string tenCongViec = ws.Cells[rowNum, indexTenCongViec].Value.ToString();
                                                    string Message = "";// _phieuThuService.CheckThuNoHHGP(MaPhieuNo, NGAYNO);
                                                    if (!string.IsNullOrEmpty(Message))
                                                        error += Message;
                                                    if (Message == "")
                                                    {
                                                        Message = _soChiService.ImportNoGCN(soQuyetToan, tenCongViec, soTienNo);
                                                        if (!string.IsNullOrEmpty(Message))
                                                            error += "</br> - " + Message;
                                                    }

                                                }

                                            }
                                            catch (Exception exx)
                                            {
                                                error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": " + exx;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                }
                if (!string.IsNullOrEmpty(error))
                    _logger.WriteLog(error);
                else
                    _logger.WriteLog("Đã import thành công nợ gia công ngoài.");
            }
            catch (Exception objEx)
            {
                _logger.WriteLog("SoThuController.ImportData:" + objEx);
                return Json(new { IsError = true, Message = "Lỗi import excel." });
            }
            if (string.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    IsError = false,
                    Message = ""
                });
            }
            else
            {
                _logger.WriteLog("SoThuController.ImportData:" + error);
                return Json(new { IsError = true, Message = error });
            }

        }

        [HttpPost]
        public ActionResult ImportData_HHTXE()
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string error = "";

            try
            {
                Hashtable hshCol = new Hashtable();
                hshCol.Add("STT", 1);
                hshCol.Add("Số Quyết Toán", 0);
                hshCol.Add("Số tiền nợ", 0);
                hshCol.Add("Người duyệt", 0);
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
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
                                    for (int ii = 1; ii <= 20; ii++)
                                    {
                                        if (ws.Cells[beginRowNum, ii].Value != null)
                                        {
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().ToUpper().Equals("STT"))
                                                hshCol["STT"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số Quyết Toán"))
                                                hshCol["Số Quyết Toán"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Số tiền nợ"))
                                                hshCol["Số tiền nợ"] = ii;
                                            if (ws.Cells[beginRowNum, ii].Value.ToString().Trim().Equals("Người duyệt"))
                                                hshCol["Người duyệt"] = ii;
                                        }

                                    }
                                    //neu co cot khong ton tai trong danh sach cot can thiet thi canh bao sai dinh dang
                                    foreach (DictionaryEntry o in hshCol)
                                        if (o.Value.ToString() == "0")
                                        {
                                            return Json(new { IsError = true, Message = "File excel thiếu cột " + o.Key.ToString() + ". Vui lòng kiểm tra lại." }, JsonRequestBehavior.AllowGet);
                                        }
                                    #endregion

                                    #region Add dữ liệu
                                    int indexSTT = (int)hshCol["STT"];
                                    int indexMaPhieu = (int)hshCol["Số Quyết Toán"];
                                    int indexSoTienNo = (int)hshCol["Số tiền nợ"];
                                    int indexNguoiDuyet = (int)hshCol["Người duyệt"];


                                    //kiem tra rang buoc tien trong hang tren duong con du thi moi import
                                    double tongTien = 0;
                                    for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                    {
                                        try
                                        {
                                            string STT = ws.Cells[rowNum, indexSTT].Value == null ? "" : ws.Cells[rowNum, indexSTT].Value.ToString();

                                            if (!string.IsNullOrEmpty(STT) && STT != "")
                                            {
                                                string soQuyetToan = ws.Cells[rowNum, indexMaPhieu].Value.ToString();
                                                if (string.IsNullOrEmpty(soQuyetToan))
                                                {
                                                    error = "Dòng " + (rowNum - 1) + " không có số quyết toán.";
                                                    break;
                                                }

                                                string SOTIENNO = ws.Cells[rowNum, indexSoTienNo].Value == null ? "" : ws.Cells[rowNum, indexSoTienNo].Value.ToString();

                                                if (!TNK.Core.Common.IsNumeric(SOTIENNO))
                                                {
                                                    error = "Dòng " + (rowNum - 1) + " số tiền nợ (" + SOTIENNO + ") không phải là kiểu số.";
                                                    break;
                                                }

                                                string nguoiDuyet = ws.Cells[rowNum, indexNguoiDuyet].Value == null ? "" : ws.Cells[rowNum, indexNguoiDuyet].Value.ToString();

                                                if (string.IsNullOrEmpty(nguoiDuyet))
                                                {
                                                    error = "Dòng " + (rowNum - 1) + " không có người duyệt.";
                                                    break;
                                                }

                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            error += e;

                                        }
                                    }
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        //TuiDinhKhoan tdk = new TuiDinhKhoan(); /*_tuiDinhKhoanService.GetTuiDinhKhoan(maTui);*/
                                        for (var rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                                        {
                                            try
                                            {
                                                string STT = ws.Cells[rowNum, indexSTT].Value == null ? "" : ws.Cells[rowNum, indexSTT].Value.ToString();
                                                if (!string.IsNullOrEmpty(STT) && STT != "")
                                                {
                                                    //DataRow row = dtb.NewRow();
                                                    double soTienNo = Convert.ToDouble(ws.Cells[rowNum, indexSoTienNo].Value.ToString());
                                                    string soQuyetToan = ws.Cells[rowNum, indexMaPhieu].Value.ToString();
                                                    string nguoiDuyet = ws.Cells[rowNum, indexNguoiDuyet].Value.ToString();
                                                    string Message = "";// _phieuThuService.CheckThuNoHHGP(MaPhieuNo, NGAYNO);
                                                    if (!string.IsNullOrEmpty(Message))
                                                        error += Message;
                                                    if (Message == "")
                                                    {
                                                        Message = _soChiService.ImportNoHHTXE(soQuyetToan, soTienNo, nguoiDuyet);
                                                        if (!string.IsNullOrEmpty(Message))
                                                            error += "</br> - " + Message;
                                                    }

                                                }

                                            }
                                            catch (Exception exx)
                                            {
                                                error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": " + exx;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                }
                if (!string.IsNullOrEmpty(error))
                    _logger.WriteLog(error);
                else
                    _logger.WriteLog("Đã import thành công nợ hoa hồng tài xế.");
            }
            catch (Exception objEx)
            {
                _logger.WriteLog("SoThuController.ImportData:" + objEx);
                return Json(new { IsError = true, Message = "Lỗi import excel." });
            }
            if (string.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    IsError = false,
                    Message = ""
                });
            }
            else
            {
                _logger.WriteLog("SoThuController.ImportData:" + error);
                return Json(new { IsError = true, Message = error });
            }

        }
        public ActionResult CHCNOIndex(string from = "", string to = "", string query = "", int p = 1, int pageSize = 30, string tinhtrang = "")
        {
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CHCNO", tinhtrang);
                var model = _phieuThuService.ListThuCongNo(f, t, p, ref total, pageSize, tinhtrang);
                ViewBag.Total = total;
                ViewBag.Title = "Hoàn công nợ bảo hiểm dịch vụ";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCNOIndex" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        public ActionResult CHCNOCreate(string id)
        {
            try
            {
                ViewBag.Title = "Hoàn công nợ bảo hiểm dịch vụ";
                SoThuModel model = new SoThuModel();

                model.Item = _phieuThuService.GetPhieuThu(id);
                if (model.Item == null)
                    return null;
                ViewBag.ChuoiUpdatedDate = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(id);
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.CTPT = new List<ChiTietPhieuThu>();
                model.CTPT.Add(new ChiTietPhieuThu());
                model.DoiTac = _soThuService.GetDoiTac("BH");
                model.Item.KeToanTruong = Guid.Empty;
                model.Item.NguoiLapPhieu = Guid.Empty;
                model.Item.NguoiNopTien = Guid.Empty;
                model.Item.NguoiThuTien = Guid.Empty;
                model.Item.GhiChu = "";
                model.Item.SoChungTu = "";
                model.Item.MaPhieuLienQuan = id;
                model.Item.MaLoaiPhieu = "CHCNO";
                if (model.CTPT.Count == 0)
                {
                    model.CTPT = new List<ChiTietPhieuThu>();
                    model.CTPT.Add(new ChiTietPhieuThu());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCNOCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult CHCNOCreate(PhieuChi item, List<ChiTietPhieuChi> httt, string submit, string ChuoiUpdatedDate)
        {
            try
            {
                if (CurrentUser.ReadOnly == true)
                {
                    return Redirect("/Home/NoAccess");
                }
                string xml = "<Action>CHCNOCreate</Action>";
                if (submit == null)
                    xml += Environment.NewLine + "<submit>" + submit + "</submit>";
                if (ChuoiUpdatedDate == null)
                    xml += Environment.NewLine + "<ChuoiUpdatedDate>" + ChuoiUpdatedDate + "</ChuoiUpdatedDate>";
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

                ViewBag.Title = "Hoàn công nợ bảo hiểm dịch vụ";
                string chuoiUpdatedDateDatabase = _phieuThuService.LayNgayUpdateCuaCacPhieuNo(item.MaPhieuLienQuan);
                if (ChuoiUpdatedDate != chuoiUpdatedDateDatabase)
                {
                    TempData["Warning"] = "Không thể tạo phiếu hoàn vì phiếu thu công nợ đã có người cập nhật trước đó.";
                    _logger.WriteLog("Warning:" + item.MaPhieuLienQuan + "-" + TempData["Warning"]);

                    return Redirect("/SoChi/CHCPTIndex?tab=2");
                }
                else
                {
                    string message = _soChiService.CreatePhieuChi(item, httt, true, xml);
                    if (message == "")
                    {
                        TempData["Info"] = "Tạo mới thành công";
                        if (submit.Equals("Lưu"))
                            return Redirect("/SoChi/CHCNOIndex?tab=2");
                    }
                    else
                    {
                        TempData["Info"] = "Lỗi tạo phiếu:" + message;
                        TempData["Error"] = "Lỗi tạo phiếu:" + message;
                        return Redirect("/SoChi/CHCNOIndex?tab=2");
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCNOCreate" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        public ActionResult CHCNOComplete(string from = "", string to = "", string query = "", int p = 1)
        {
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "CHCNO");
                var model = _soChiService.GetCHCNO(f, t, query, p, ref total, pageSize);
                ViewBag.Total = total;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex;
                _logger.WriteLog("SoChiController:CHCNOComplete" + ex.ToString());
            }
            return View();
        }
        public ActionResult CHCNOEdit(string id)
        {
            try
            {
                ViewBag.Title = "Hoàn công nợ bảo hiểm dịch vụ";
                SoChiModel model = new SoChiModel();
                model.Item = _soChiService.GetPhieuChi(id);
                if (model.Item == null)
                    return null;
                model.CTPT = _soChiService.GetCTPC(id);
                model.Users = _userService.get();
                model.HTTT = _soThuService.GetHTTT();
                model.DoiTac = _soThuService.GetDoiTac("BH");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoChiController:CHCPTEdit" + ex.ToString());
                TempData["Error"] = ex;
            }
            return View();

        }
        [HttpPost]
        public ActionResult ImportData_CHCNO()
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }

            string error = "";
            string SoChungTu = "";
            try
            {
                Hashtable hshCol = new Hashtable
                {
                    { "STT", 0},
                    { "Mã phiếu thu", 0 },
                    { "Số chứng từ chi", 0 },
                    { "Ngày chi", 0 },
                    { "Ngày hoàn tiền", 0 },
                    { "Ngân hàng", 0 },
                    { "HTTT", 0 },
                    { "Số tiền", 0 },
                    { "Ghi chú", 0 },
                    { "Người duyệt", 0 },
                    { "Người chi tiền", 0 }
                };

           
                List<PhieuChi> lstPhieuChi = new List<PhieuChi>();
                List<List<ChiTietPhieuChi>> lstChiTietPhieuChi = new List<List<ChiTietPhieuChi>>();
                foreach (string file in Request.Files)
                {

                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        Stream stream = fileContent.InputStream;
                        using (var package = new ExcelPackage(stream))
                        {
                            var workBook = package.Workbook;
                            if (workBook == null || workBook.Worksheets.Count == 0)
                                continue;

                            var ws = workBook.Worksheets.First();
                            if (ws.Dimension.End.Row < 2)
                                return Json(new { IsError = true, Message = "Không có dữ liệu để import" }, JsonRequestBehavior.AllowGet);

                            // Kiểm tra định dạng cột
                            //bool isFormatValid = true;
                            int beginRowNum = 1;

                            for (int i = 1; i <= 20; i++)
                            {
                                var cellValue = ws.Cells[beginRowNum, i].Value?.ToString().Trim();
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    foreach (string key in hshCol.Keys)
                                    {
                                        if (cellValue.Equals(key, StringComparison.OrdinalIgnoreCase))
                                        {
                                            hshCol[key] = i;
                                            break;
                                        }
                                    }
                                }
                            }

                            foreach (DictionaryEntry col in hshCol)
                            {
                                if ((int)col.Value == 0)
                                {
                                    return Json(new { IsError = true, Message = "File excel thiếu cột: " + col.Key }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            int indexSoChungTu = (int)hshCol["Số chứng từ chi"];
                            int indexMaPhieuThu = (int)hshCol["Mã phiếu thu"];
                            int indexNgayChi = (int)hshCol["Ngày chi"];
                            int indexNgayHoachToan = (int)hshCol["Ngày hoàn tiền"];
                            int indexNganHang = (int)hshCol["Ngân hàng"];
                            int indexHTTT = (int)hshCol["HTTT"];
                            int indexSoTien = (int)hshCol["Số tiền"];
                            int indexGhiChu = (int)hshCol["Ghi chú"];
                            int indexNguoiDuyet = (int)hshCol["Người duyệt"];
                            int indexNguoiChiTien = (int)hshCol["Người chi tiền"];
                            // Bắt đầu đọc dữ liệu
                            for (int rowNum = beginRowNum + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                            {
                                try
                                {

                                    //DataRow row = dtb.NewRow();
                                    DateTime NGAYCHI = new DateTime();
                                    DateTime NGAYHOACHTOAN = new DateTime();
                                    string ngaychi = ws.Cells[rowNum, indexNgayChi].Value == null ? "" : ws.Cells[rowNum, indexNgayChi].Value.ToString();
                                    string MaPhieuThu = ws.Cells[rowNum, indexMaPhieuThu].Value == null ? "" : ws.Cells[rowNum, indexMaPhieuThu].Value.ToString();
                                    SoChungTu = ws.Cells[rowNum, indexSoChungTu].Value == null ? "" : ws.Cells[rowNum, indexSoChungTu].Value.ToString();
                                    string NgayHoachToan = ws.Cells[rowNum, indexNgayHoachToan].Value == null ? "" : ws.Cells[rowNum, indexNgayHoachToan].Value.ToString();
                                 
                                    string NguoiDuyet = ws.Cells[rowNum, indexNguoiDuyet].Value == null ? "" : ws.Cells[rowNum, indexNguoiDuyet].Value.ToString();

                                    string NganHang = ws.Cells[rowNum, indexNganHang].Value == null ? "" : ws.Cells[rowNum, indexNganHang].Value.ToString();
                                    string HTTT = ws.Cells[rowNum, indexHTTT].Value == null ? "" : ws.Cells[rowNum, indexHTTT].Value.ToString();
                                    string SoTien = ws.Cells[rowNum, indexSoTien].Value == null ? "" : ws.Cells[rowNum, indexSoTien].Value.ToString();
                                    string GhiChu = ws.Cells[rowNum, indexGhiChu].Value == null ? "" : ws.Cells[rowNum, indexGhiChu].Value.ToString();
                                    //string TreoTien = ws.Cells[rowNum, indexTreoTien].Value == null ? "" : ws.Cells[rowNum, indexTreoTien].Value.ToString();
                                    string NguoiChiTien = ws.Cells[rowNum, indexNguoiChiTien].Value == null ? "" : ws.Cells[rowNum, indexNguoiChiTien].Value.ToString();
                                    HTTT = HTTT.ToUpper();
                                    if (string.IsNullOrEmpty(SoTien))
                                        SoTien = "0";
                                    if (!TNK.Core.Common.IsNumeric(SoTien) && !string.IsNullOrEmpty(SoTien))
                                    {
                                        error = "Dòng " + rowNum + " số tiền (" + SoTien + ") không phải là kiểu số.";
                                        break;
                                    }
                                    if (!string.IsNullOrEmpty(NganHang) && !TNK.Core.Common.IsNumeric(SoTien))
                                    {
                                        error = "Dòng " + rowNum + " số tiền (" + SoTien + ") không phải là kiểu số.";
                                        break;
                                    }
                                    if (!string.IsNullOrEmpty(HTTT) && HTTT != "CK" && HTTT != "CT" && HTTT != "TM")
                                    {
                                        error = "Dòng " + rowNum + " sai hình thức thanh toán.";
                                        break;
                                    }
                                    if (!string.IsNullOrEmpty(NganHang) && !_doiTacService.getDoiTacById(NganHang) && NganHang != "QuyChinh" && NganHang != "QuyPhu")
                                    {
                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy ngân hàng " + NganHang;
                                        break;
                                    }
                                    if (!string.IsNullOrEmpty(NganHang) && Convert.ToDouble(SoTien) <= 0 && !string.IsNullOrEmpty(HTTT))
                                    {
                                        error = "Dòng " + rowNum + " số tiền không được nhỏ hơn bằng 0.";
                                        break;
                                    }
                                    if (string.IsNullOrEmpty(SoChungTu))
                                    {
                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Vui lòng nhập số chứng từ.";
                                        break;
                                    }
                                    User us = _userService.getByUserName(NguoiChiTien);
                                    if (us == null)
                                    {
                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy người chi tiền " + NguoiChiTien;
                                        break;
                                    }
                                    if (HTTT == "TM" && NganHang != "QuyChinh" && NganHang != "QuyPhu")
                                    {
                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " sai hình thức thanh toán";
                                        break;
                                    }
                                    if (HTTT != "TM" && (NganHang == "QuyChinh" || NganHang == "QuyPhu"))
                                    {
                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " sai hình thức thanh toán";
                                        break;
                                    }
                                    if (string.IsNullOrEmpty(NguoiDuyet))
                                    {
                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập người duyệt";
                                        break;
                                    }
                                    if ((Convert.ToDouble(SoTien) > 0 && (string.IsNullOrEmpty(NganHang) || string.IsNullOrEmpty(HTTT)))
                                        || (!string.IsNullOrEmpty(NganHang) && (string.IsNullOrEmpty(SoTien) || string.IsNullOrEmpty(HTTT)))
                                        || (!string.IsNullOrEmpty(HTTT) && (string.IsNullOrEmpty(NganHang) || string.IsNullOrEmpty(SoTien)))
                                        )
                                    {
                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + " vui lòng nhập đủ thông tin thanh toán(ngân hàng, HTTT,số tiền).";
                                        break;
                                    }
                                    if (TNK.Core.Common.IsDate(ngaychi) == false)
                                    {
                                        if (TNK.Core.Common.IsNumeric(ngaychi))
                                        {
                                            double DATE = double.Parse(ngaychi);
                                            NGAYCHI = DateTime.FromOADate(DATE);
                                        }
                                    }
                                    else
                                        NGAYCHI = Convert.ToDateTime(ngaychi);
                                    if (TNK.Core.Common.IsDate(NgayHoachToan) == false)
                                    {
                                        if (TNK.Core.Common.IsNumeric(NgayHoachToan))
                                        {
                                            double DATE = double.Parse(NgayHoachToan);
                                            NGAYHOACHTOAN = DateTime.FromOADate(DATE);
                                        }
                                    }
                                    else
                                        NGAYHOACHTOAN = Convert.ToDateTime(NgayHoachToan);
                                    if (NGAYCHI.Year > 2000 && NGAYHOACHTOAN.Year > 2000)
                                    {
                                        PhieuChi item = new PhieuChi();
                                        List<ChiTietPhieuChi> CTPT = new List<ChiTietPhieuChi>();
                                        item.MaLoaiPhieu = "CHCNO";
                                        item.SoChungTu = SoChungTu;
                                        item.NgayChi = Convert.ToDateTime(NGAYCHI);
                                        item.NgayHachToan = Convert.ToDateTime(NGAYHOACHTOAN);
                                        item.NguoiThuTien = us.UserId;
                                        item.GhiChu = GhiChu;
                                        item.ConLai = item.TongCong;
                                        //item.NoiDung = NoiDung;
                                        item.SoTienChi = Convert.ToDouble(SoTien);
                                        item.GhiChu = GhiChu;
                                        item.TongCong = item.SoTienChi;
                                        item.NguoiDuyet = NguoiDuyet;
                                        item.MaPhieuLienQuan = MaPhieuThu;
                                        //phan httt
                                        HTTT = HTTT.ToUpper();
                                        CTPT.Add(new ChiTietPhieuChi());
                                        CTPT[0].HinhThucThanhToan = HTTT;
                                        NganHang = NganHang.ToUpper();
                                        CTPT[0].MaNganHang = NganHang;
                                        CTPT[0].SoTienThanhToan = Convert.ToDouble(SoTien);
                                        CTPT[0].Temp = NganHang + "-" + HTTT;
                                        
                                        double TongThanhToan = 0;
                                        foreach (var thanhtoan in CTPT)
                                        {
                                            TongThanhToan += thanhtoan.SoTienThanhToan;
                                        }
                                        item.SoTienChi = TongThanhToan;
                                        if (item.SoTienChi > item.TongCong)
                                        {
                                            error += Environment.NewLine + "Dòng " + (rowNum - 1) + " số tiền chi phải nhỏ hơn số tiền cần thanh toán";
                                            break;
                                        }
                                        var phieuThu = _phieuThuService.GetPhieuThu(MaPhieuThu);
                                        if (phieuThu == null)
                                        {
                                            error += Environment.NewLine + "Dòng " + (rowNum - 1) + " không tìm thấy phiếu thu";
                                            break;
                                        }
                                        else if (phieuThu.ConLai < Convert.ToDouble(SoTien))
                                        {
                                            error += Environment.NewLine + "Dòng " + (rowNum - 1) + " số tiền thanh toán lớn hơn số tiền còn lại";
                                            break;
                                        }
                                        item.DoiTac = phieuThu.DoiTac;
                                        lstPhieuChi.Add(item);
                                        lstChiTietPhieuChi.Add(CTPT);
                                    }
                                    else
                                        error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": Sai định dạng ngày thu. Vui lòng kiểm tra lại.";

                                }
                                catch (Exception exx)
                                {
                                    error += Environment.NewLine + "Dòng " + (rowNum - 1) + ": " + exx;
                                }
                            }
                        }
                    }
                }
                
                if (!string.IsNullOrEmpty(error))
                {
                    _logger.WriteLog(error);
                    return Json(new { IsError = true, Message = error });
                }
                else
                {
                    for (int i = 0; i < lstPhieuChi.Count; i++)
                    {

                        string xml = "<formdata><Action>PhieuThu</Action>";
                        xml += Environment.NewLine + "<submit>Import từ excel</submit>";
                        if (lstPhieuChi[i] != null)
                            xml += TNK.Core.Common.ConvertObjectToXMLString(lstPhieuChi[i]);
                        if (lstChiTietPhieuChi[i] != null)
                        {
                            xml += Environment.NewLine + "<ThanhToan>";
                            foreach (ChiTietPhieuChi ct in lstChiTietPhieuChi[i])
                                xml += TNK.Core.Common.ConvertObjectToXMLString(ct);
                            xml += Environment.NewLine + "</ThanhToan>";
                        }
                        xml += "</formdata>";

                        string message = _soChiService.CreatePhieuChi(lstPhieuChi[i], lstChiTietPhieuChi[i], true, xml);
                        if (message != "")
                        {
                            error += Environment.NewLine + "Dòng " + (i + 1) + "(" + SoChungTu + "):" + message;
                        }
                    }
                    _logger.WriteLog("Đã import thành công phiếu chi.");
                    return Json(new { IsError = false, Message = "Import thành công." });
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SoThuController.ImportData_CHCNO: " + ex);
                return Json(new { IsError = true, Message = "Lỗi import dữ liệu." });
            }
        }
        //[HttpPost]
        public ActionResult ExportFile_CHCNO()
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            var httt = _soChiService.GetHTTT().Select(x => x.MaNganHang).Distinct().ToList();
            string str = Server.MapPath("~\\Views\\FileMau\\" + "FileMauImportCHCN.xlsx");
            FileInfo template = new FileInfo(str);
            ExcelPackage pck = new ExcelPackage(new FileInfo(str));

            var sheet = pck.Workbook.Worksheets["DanhMuc_NganHang"];
            sheet.Cells["A2"].LoadFromCollection(httt, false);

            return new ExcelInsuranceDownload(pck, "FileMauImportCHCN.xlsx");
        }
    }
   
}

