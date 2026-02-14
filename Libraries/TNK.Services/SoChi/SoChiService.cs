using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Data;
using TNK.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Services.Authentication;
using TNK.Core.Caching;
using TNK.Services.Log;
using TNK.Services.No;
using System.Data;
using TNK.Services.LichSuThaoTac;
using TNK.Services.Catalog;
using TNK.Services.Users;
using CRM.Web.Mail;

namespace TNK.Services.SoChi
{
    public class SoChiService : ISoChiService
    {
        IRepository<ViewHinhThucThanhToan> _htttRepository;
        IRepository<ChiTietPhieuChi> _ctptRepository;
        IRepository<ChiTietPhieuThu> _ChiTietPhieuThuRepository;
        IRepository<CHI_TIET_PHIEU_CHI> _CHI_TIET_PHIEU_CHIRepository;
        IRepository<ChiTietNhapKhoPhuTung> _ctnkptRepository;
        IRepository<PhieuChi> _PhieuChiRepository;
        IRepository<DoiTac> _doiTacRepository;
        IRepository<ViewPhieuChi> _ViewPhieuChiRepository;
        IRepository<ViewListPhieuChi> _ViewListPhieuChiRepository;
        IRepository<NoPhaiThu> _noPhaiThuRepository;
        IRepository<TNK.Core.Domain.NhanVien> _NhanVienRepository;
        IRepository<NoPhaiTra> _noPhaiTraRepository;
        IRepository<ViewNoPhaiTra> _VNPTRepository;
        IRepository<ViewNoDaTra> _VNDTRepository;
        IRepository<ViewNoPhaiTraTuPhieuThu> _viewNoPhaiTraTuPhieuThuRepository;
        IAuthenticationService _authenticationService;
        IRepository<ViewCOCDT> _viewCOCDTRepository;
        IRepository<ViewCHCPT> _viewCHCPTRepository;
        IRepository<ViewCHCNO> _viewCHCNORepository;
        IRepository<ViewCTAUN> _viewCTAUNRepository;
        IRepository<ViewChiCoc> _viewChiCocRepository;
        IRepository<DanhMucDienGiai> _DanhMucDienGiaiRepository;
        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;
        ILichSuThaoTacService _lichSuThaoTacService;
        IChiTietNhapKhoPhuTungService _ctnkptService;
        INoPhaiTraService _noPhaiTraService;
        IRepository<TNK.Core.Domain.KhoXe> _khoXeRepository;
        IRepository<PhieuThu> _PhieuThuRepository;
        ITuiDinhKhoanService _tuiDinhKhoanService;
        IRepository<KhoPhuTung> _KhoPhuTungRepository;
        IUserervice _userService;

        IRepository<ViewChiTietKhauHao> _viewCTKHRepository;
        IRepository<CHI_TIET_KHAU_HAO> _ctkhRepository;
        IRepository<ViewCKHHA> _viewCKHHARepository;
        IRepository<ViewDanhSachPhieuNhapKhoPhuTung> _viewDanhSachPhieuNhapKhoPhuTungRepository;

        string _ipClient = "";
        string _hostNameClient = "";
        string _clientBrowser = "";
        public SoChiService(IRepository<ViewHinhThucThanhToan> _htttRepository
            , IDbContext _dbContext
            , IRepository<ChiTietPhieuChi> _ctptRepository
            , IRepository<CHI_TIET_PHIEU_CHI> _CHI_TIET_PHIEU_CHIRepository
            , IRepository<ChiTietNhapKhoPhuTung> _ctnkptRepository
            , IRepository<PhieuChi> _PhieuChiRepository
            , IRepository<ViewPhieuChi> _ViewPhieuChiRepository
             , IAuthenticationService _authenticationService
            , IRepository<DoiTac> _doiTacRepository
            , IRepository<NoPhaiThu> _noPhaiThuRepository
            , IRepository<NoPhaiTra> _noPhaiTraRepository
            , IRepository<ViewNoPhaiTraTuPhieuThu> _viewNoPhaiTraTuPhieuThuRepository
            , IRepository<ViewNoPhaiTra> _VNPTRepository
            , IRepository<ViewNoDaTra> _VNDTRepository
            , IRepository<ViewCOCDT> _viewCOCDTRepository
            , IRepository<ViewCHCPT> _viewCHCPTRepository
            , IRepository<ViewChiCoc> _viewChiCocRepository
            , IRepository<DanhMucDienGiai> _DanhMucDienGiaiRepository
            , ICacheManager _cacheManager
            , ILogger _log
            , ILichSuThaoTacService _lichSuThaoTacService
            , IChiTietNhapKhoPhuTungService _ctnkptService
            , INoPhaiTraService _noPhaiTraService
            , IRepository<ViewCTAUN> _viewCTAUNRepository
            , IRepository<TNK.Core.Domain.NhanVien> _NhanVienRepository
             , IRepository<TNK.Core.Domain.KhoXe> _khoXeRepository
            , IRepository<ChiTietPhieuThu> _ChiTietPhieuThuRepository
            , IRepository<PhieuThu> _PhieuThuRepository
            , IRepository<ViewListPhieuChi> _ViewListPhieuChiRepository
            , ITuiDinhKhoanService _tuiDinhKhoanService
            , IRepository<KhoPhuTung> _KhoPhuTungRepository
            , IUserervice _userService

             , IRepository<ViewChiTietKhauHao> _viewCTKHRepository
            , IRepository<CHI_TIET_KHAU_HAO> _ctkhRepository
            , IRepository<ViewCKHHA> _viewCKHHARepository
            , IRepository<ViewCHCNO> _viewCHCNORepository
              , IRepository<ViewDanhSachPhieuNhapKhoPhuTung> _viewDanhSachPhieuNhapKhoPhuTungRepository

            )
        {
            this._htttRepository = _htttRepository;
            this._dbContext = _dbContext;
            this._PhieuChiRepository = _PhieuChiRepository;
            this._authenticationService = _authenticationService;
            this._doiTacRepository = _doiTacRepository;
            this._ctptRepository = _ctptRepository;
            this._CHI_TIET_PHIEU_CHIRepository = _CHI_TIET_PHIEU_CHIRepository;
            this._ctnkptRepository = _ctnkptRepository;
            this._noPhaiThuRepository = _noPhaiThuRepository;
            this._noPhaiTraRepository = _noPhaiTraRepository;
            this._VNDTRepository = _VNDTRepository;
            this._viewNoPhaiTraTuPhieuThuRepository = _viewNoPhaiTraTuPhieuThuRepository;
            this._VNPTRepository = _VNPTRepository;
            this._cacheManager = _cacheManager;
            this._log = _log;
            this._lichSuThaoTacService = _lichSuThaoTacService;
            this._ViewPhieuChiRepository = _ViewPhieuChiRepository;
            this._viewCOCDTRepository = _viewCOCDTRepository;
            this._viewChiCocRepository = _viewChiCocRepository;
            this._ctnkptService = _ctnkptService;
            this._viewCHCPTRepository = _viewCHCPTRepository;
            this._noPhaiTraService = _noPhaiTraService;
            this._viewCTAUNRepository = _viewCTAUNRepository;
            this._NhanVienRepository = _NhanVienRepository;
            this._khoXeRepository = _khoXeRepository;
            this._DanhMucDienGiaiRepository = _DanhMucDienGiaiRepository;
            this._ChiTietPhieuThuRepository = _ChiTietPhieuThuRepository;
            this._PhieuThuRepository = _PhieuThuRepository;
            this._ViewListPhieuChiRepository = _ViewListPhieuChiRepository;
            this._tuiDinhKhoanService = _tuiDinhKhoanService;
            this._KhoPhuTungRepository = _KhoPhuTungRepository;
            this._userService = _userService;

            this._viewCTKHRepository = _viewCTKHRepository;
            this._ctkhRepository = _ctkhRepository;
            this._viewCKHHARepository = _viewCKHHARepository;
            this._viewCHCNORepository = _viewCHCNORepository;
            this._viewDanhSachPhieuNhapKhoPhuTungRepository = _viewDanhSachPhieuNhapKhoPhuTungRepository;
        }
        static int gioGuiEmailLast = Convert.ToInt32(DateTime.Now.Hour.ToString());
        static bool Send = false;
        public string ContentMail(string maPhieuChi)
        {
            string Content = "";
            PhieuChi PC = _PhieuChiRepository.Table.Where(x => x.MaPhieuChi == maPhieuChi && x.IsDeleted == false).FirstOrDefault();
            Guid nguoiTao = PC.CreatedBy.Value;
            Guid nguoiSua = PC.UpdatedBy.Value;
            User nguoitacDong = new User();
            if (nguoiTao != nguoiSua)
            {
                nguoitacDong = _userService.getByUserId(nguoiTao);
            }
            else
            {
                nguoitacDong = _userService.getByUserId(nguoiSua);
            }
            Content = "Mã phiếu chi " + maPhieuChi + " do " + nguoitacDong.UserName + " đã thao tác lúc " + (PC.UpdatedDate == null ? PC.CreatedDate : PC.UpdatedDate) + " đã làm mất cân!";
            return Content;
        }
        public void SetClientBrowser(string browser)
        {
            this._clientBrowser = browser;
        }
        public void SetIPClient(string ip)
        {
            this._ipClient = ip;
        }
        public void SetHostNameClient(string host)
        {
            this._hostNameClient = host;
        }
        public List<ViewNoDaTra> GetPhieuChiComplete(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _dbContext.ExecuteStoredProcedureList<ViewNoDaTra>("getPhieuChiComplete");
                q = q.Where(x => x.NgayChi >= from && x.NgayChi <= to && x.LoaiPhieuNo == id).ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    var loweredQuery = query.ToLower();
                    q = q.Where(x =>
                        (!string.IsNullOrEmpty(x.MaPhieuNo) && x.MaPhieuNo.ToLower().Contains(loweredQuery)) ||
                        (!string.IsNullOrEmpty(x.ChungTuThu) && x.ChungTuThu.ToLower().Contains(loweredQuery)) ||
                        (!string.IsNullOrEmpty(x.KhachHang) && x.KhachHang.ToLower().Contains(loweredQuery)) ||
                        (!string.IsNullOrEmpty(x.Hoten) && x.Hoten.ToLower().Contains(loweredQuery)) ||
                        (!string.IsNullOrEmpty(x.DoiTac) && x.DoiTac.ToLower().Contains(loweredQuery)) ||
                        (!string.IsNullOrEmpty(x.MaPhieuLienQuan) && x.MaPhieuLienQuan.ToLower().Contains(loweredQuery)) ||
                        (!string.IsNullOrEmpty(x.MaPhieuChi) && x.MaPhieuChi.ToLower().Contains(loweredQuery)) ||
                        (!string.IsNullOrEmpty(x.MaPhieuChi) && loweredQuery.Contains(x.MaPhieuChi.ToLower())) ||
                        (!string.IsNullOrEmpty(x.ChungTuThu) && loweredQuery.Contains(x.ChungTuThu.ToLower()))
                    ).ToList();
                }
                var result = q.OrderByDescending(x => x.NgayNo).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //{
                //    _log.WriteLog("SoChiService.GetPhieuChiComplete" + e.InnerException.ToString());
                //}
                //else
                //{
                //    _log.WriteLog("SoChiService.GetPhieuChiComplete" + e.Message.ToString());
                //}
                _log.WriteLog("SoChiService.GetPhieuChiComplete", e);
                return null;
            }
        }
        public List<ViewNoDaTra> GetPhieuChiComplete_NTHBH(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _dbContext.ExecuteStoredProcedureList<ViewNoDaTra>("getPhieuChiComplete");
                q = q.Where(x => x.NgayChi >= from && x.NgayChi <= to && x.LoaiPhieuNo == id).ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuNo.ToLower().Contains(query)
                    || x.ChungTuThu.ToLower().Contains(query)
                    ||(x.KhachHang?.ToLower().Contains(query) ?? false)
                    || x.Hoten.Contains(query)
                    || x.MaPhieuLienQuan.Contains(query)

                    || query.Contains(x.MaPhieuChi.ToLower())
                    || query.Contains(x.ChungTuThu.ToLower())
                    ).ToList();
                }
                var result = q.OrderByDescending(x => x.NgayNo).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception e)
            {
                _log.WriteLog("SoChiService.GetPhieuChiComplete", e);
                return null;
            }
        }
        public ViewNoDaTra GetPhieuChiCompleteById(string id)
        {
            try
            {
                SqlParameter param = new SqlParameter("id", id);
                var data = _dbContext.ExecuteStoredProcedureList<ViewNoDaTra>("getPhieuChiCompleteById", param);
                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //{
                //    _log.WriteLog("SoChiService.GetPhieuChiCompleteById(" + id + "):" + ex.InnerException.ToString());
                //}
                //else
                //{
                //    _log.WriteLog("SoChiService.GetPhieuChiCompleteById(" + id + "):" + ex.Message.ToString());
                //}
                _log.WriteLog("SoChiService.GetPhieuChiCompleteById(" + id + "):", ex);
                return null;
            }
        }
        public List<ViewNoDaTra> GetNoDaTra(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                if (query == "undefined")
                    query = "";
                var q = _VNDTRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to && x.LoaiPhieuNo == id).Distinct();
                if (!string.IsNullOrEmpty(query))
                {
                    if (id == "N2THK")
                    {
                        q = q.Where(x => x.MaPhieuNo.Contains(query)
                        || x.KhachHang.Contains(query)
                        || x.MaPhieuNo.Contains(query.ToLower())
                        || x.ChungTuThu.Contains(query.ToLower())
                        || x.MaPhieuChi.Contains(query.ToLower())
                        || x.NoiDung.Contains(query.ToLower()));
                    }
                    else
                    {
                        q = q.Where(x => x.MaPhieuNo.Contains(query)
                        || x.KhachHang.Contains(query)
                        || x.MaPhieuNo.Contains(query.ToLower())
                        || x.ChungTuThu.Contains(query.ToLower())
                        || x.MaPhieuChi.Contains(query.ToLower())
                        || x.MaPhieuChi.Contains(query.ToLower())
                        || x.SoPhieuQuyetToan.Contains(query.ToLower()));
                    }

                }
                var result = q.OrderByDescending(x => x.NgayChi).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //{
                //    if(ex.InnerException != null)
                //    {
                //        _log.WriteLog("SoChiService.GetNoDaTra(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.InnerException.ToString());
                //    }
                //    else
                //    {
                //        _log.WriteLog("SoChiService.GetNoDaTra(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.Message.ToString());
                //    }
                _log.WriteLog("SoChiService.GetNoDaTra(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):", ex);
                return null;
            }
        }
        public ViewNoDaTra GetNoDaTra(string id)
        {
            try
            {
                return _VNDTRepository.Table.FirstOrDefault(x => x.SoPhieuQuyetToan == id);
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNoDaTra(" + id + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNoDaTra(" + id + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNoDaTra(" + id + "):", ex);
                return null;
            }
        }
        public ViewNoDaTra GetNoDaTra_ThuHo(string id)
        {
            try
            {
                return _VNDTRepository.Table.FirstOrDefault(x => x.MaPhieuNo == id);
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNoDaTra(" + id + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNoDaTra(" + id + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNoDaTra(" + id + "):", ex);
                return null;
            }
        }

        public List<ViewNoPhaiTra> GetNPTs(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _VNPTRepository.Table.Where(x => x.NgayNo >= from && x.NgayNo <= to && x.LoaiPhieuNo == id && x.SoTienNo > 0);
                if (!string.IsNullOrEmpty(query))
                {
                    if (id == "NMUTS")
                    {
                        q = q.Where(x => x.ChungTuThu.ToLower().Contains(query)
                        || x.KhachHang.ToLower().Contains(query)
                         || query.Contains(x.ChungTuThu.ToLower())
                        );
                    }
                    else
                    {
                        q = q.Where(x => x.MaPhieuNo.Contains(query)
                        || x.KhachHang.Contains(query)
                        || x.BienSo.Contains(query)
                        || x.SoPhieuQuyetToan.Contains(query)

                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.SoPhieuQuyetToan.ToLower())
                        );
                    }
                }
                var result = q.OrderByDescending(x => x.SoHopDong).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNPTs(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNPTs(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNPTs(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):", ex);
                return null;
            }
        }
        //phan truc them
        public List<ViewNoPhaiTra> GetNPTs1(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang)
        {
            try
            {
                var q = _VNPTRepository.Table.Where(x => x.NgayNo >= from && x.NgayNo <= to && x.LoaiPhieuNo == id && x.SoTienNo > 0);
                if (!string.IsNullOrEmpty(query))
                {
                    if (id == "NGCNG")
                    {
                        q = q.Where(x => x.ChungTuThu.ToLower().Contains(query.ToLower())
                        || x.SoPhieuQuyetToan.ToLower().Contains(query.ToLower())
                        || x.SoHopDong.ToLower().Contains(query.ToLower())
                        || x.KhachHang.ToLower().Contains(query)
                        || x.BienSo.ToLower().Contains(query)
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.SoPhieuQuyetToan.ToLower())
                        || query.Contains(x.KhachHang.ToLower())
                         || query.Contains(x.SoHopDong.ToLower())
                        );
                    }
                    else if (id == "NMPTU")
                    {
                        q = q.Where(x => x.KhachHang.ToLower().Contains(query)
                        || x.MaPhieuNo.ToLower().Contains(query)
                        || x.GhiChu.ToLower().Contains(query)
                        || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        );
                    }
                    else if (id == "NMUTS")
                    {
                        q = q.Where(x => x.KhachHang.ToLower().Contains(query)
                        || x.MaPhieuNo.ToLower().Contains(query)
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.KhachHang.ToLower())
                        );
                    }
                    else if (id == "NBHBX" || id == "N2GTX" || id == "N2THK")
                    {
                        q = q.Where(x => x.KhachHang.ToLower().Contains(query)
                        || x.MaPhieuNo.ToLower().Contains(query)
                        || x.Hoten.ToLower().Contains(query)
                        || x.ThongTinKhac.ToLower().Contains(query.ToLower())
                        || x.SoHopDong.ToLower().Contains(query)
                        || x.TenDoiTac.ToLower().Contains(query)
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.ThongTinKhac.ToLower())
                        );
                    }
                    else if (id == "HHTXE")
                    {
                        q = q.Where(x => x.BienSo.ToLower().Contains(query)
                        || x.MaPhieuNo.ToLower().Contains(query)
                        || x.KhachHang.ToLower().Contains(query)
                        || x.SoPhieuQuyetToan.ToLower().Contains(query)

                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.SoPhieuQuyetToan.ToLower())
                        || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        );
                    }
                    else if (id == "CHHBX" || id == "NTHPK" || id == "NTHBH")
                    {
                        q = q.Where(x => x.MaPhieuNo.ToLower().Contains(query)
                        || x.KhachHang.ToLower().Contains(query)
                        || x.Hoten.ToLower().Contains(query)
                        || x.SoHopDong.ToLower().Contains(query)
                        || x.BienSo.ToLower().Contains(query)
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.KhachHang.ToLower())

                        );
                    }
                    else if (id == "NTKHA")
                    {
                        q = q.Where(x => x.SoHopDong.ToLower().Contains(query)
                        || x.Hoten.ToLower().Contains(query)
                        || x.KhachHang.ToLower().Contains(query)
                        || x.SoKhung.ToLower().Contains(query)
                        || x.SoMay.ToLower().Contains(query)

                        || query.Contains(x.SoHopDong.ToLower())
                        || query.Contains(x.Hoten.ToLower())
                        || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.SoKhung.ToLower())
                        || query.Contains(x.SoMay.ToLower())
                        
                        );
                    }
                    else
                    {
                        q = q.Where(x => x.MaPhieuNo.Contains(query)
                        || x.KhachHang.ToLower().Contains(query.ToLower())
                        || x.BienSo.ToLower().Contains(query.ToLower())
                        || x.SoPhieuQuyetToan.ToLower().Contains(query.ToLower())
                        || x.GhiChu.ToLower().Contains(query.ToLower())

                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.BienSo.ToLower())

                        || query.Contains(x.SoKhung.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        || query.Contains(x.GhiChu.ToLower())
                        );
                    }
                }
                if (tinhtrang == "C")
                {
                    q = q.Where(x => x.SoTienConLai > 1);
                }
                else if (tinhtrang == "D")
                {
                    q = q.Where(x => x.SoTienConLai <= 1);
                }
                else if (tinhtrang == "H")
                {
                    q = q.Where(x => x.HSD == "HH");
                }

                var result = q.OrderByDescending(x => x.NgayNo).ToList();
                total = result.Count;
                
                foreach (var item in result)
                {
                    var str = item.GhiChu == null ? "" : item.GhiChu;
                    item.GhiChu = "";
                    foreach (var strg in str.Split(';'))
                    {
                        item.GhiChu += strg + ",\r\n";
                    }
                }
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNPTs(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNPTs(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNPTs(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):", ex);
                return null;
            }
        }
        public ViewNoPhaiTra GetNPT(string id)
        {
            try
            {
                return _VNPTRepository.Table.FirstOrDefault(x => x.MaPhieuNo == id);
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNPT(" + id + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNPT(" + id + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNPT(" + id + "):", ex);
                return null;
            }
        }

        public List<ViewCHCPT> GetCHCPT(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _viewCHCPTRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuChi.Contains(query.ToLower())
                    || x.HoTen.ToLower().Contains(query.ToLower())
                    || x.SoChungTu.ToLower().Contains(query.ToLower())
                    || x.BienSo.Contains(query.ToLower())

                      || query.Contains(x.MaPhieuChi.ToLower())
                      || query.Contains(x.BienSo.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuChi).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetCHCPT(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetCHCPT(" + from + ","+to+ "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetCHCPT(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):", ex);
                return null;
            }
        }

        public List<ViewCOCDT> GetCOCDT(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _viewCOCDTRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuChi.Contains(query)
                    || x.SoHopDong.Contains(query)

                      || query.Contains(x.MaPhieuChi.ToLower())
                      || query.Contains(x.SoHopDong.ToLower())
                      || query.Contains(x.HoTen.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuChi).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetCOCDT(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetCOCDT(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetCOCDT(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):", ex);
                return null;
            }
        }
        public ViewNoPhaiTraTuPhieuThu GetNoPhaiTraTuPhieuThu(string maPhieuNo)
        {
            try
            {
                List<ViewNoPhaiTraTuPhieuThu> ls = _viewNoPhaiTraTuPhieuThuRepository.Table.Where(x => x.MaPhieuNo == maPhieuNo && x.IsDeleted == false).ToList();
                if (ls.Count > 0)
                    return ls[0];
                return null;
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNoPhaiTraTuPhieuThu(" + maPhieuNo + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNoPhaiTraTuPhieuThu(" + maPhieuNo + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNoPhaiTraTuPhieuThu(" + maPhieuNo + "):", ex);
                return null;
            }
        }
        public List<ViewNoPhaiTraTuPhieuThu> GetNoPhaiTraTuPhieuThuList(string loaiPhieuNo, DateTime from, DateTime to)
        {
            try
            {
                List<ViewNoPhaiTraTuPhieuThu> ls = _viewNoPhaiTraTuPhieuThuRepository.Table.Where(x => x.LoaiPhieuNo == loaiPhieuNo && x.IsDeleted == false && x.SoTienConLai > 0).ToList();
                return ls;
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNoPhaiTraTuPhieuThuList(" + loaiPhieuNo + "," + from + "," + to + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNoPhaiTraTuPhieuThuList(" + loaiPhieuNo + ","+ from +","+ to +"):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNoPhaiTraTuPhieuThuList(" + loaiPhieuNo + "," + from + "," + to + "):", ex);
                return null;
            }
        }
        public List<PhieuChi> GetPhieuChiList(string loaiPhieu, DateTime from, DateTime to)
        {
            try
            {
                List<PhieuChi> ls = _PhieuChiRepository.Table.Where(x => x.MaLoaiPhieu == loaiPhieu && x.IsDeleted == false).OrderByDescending(x => x.NgayChi).ToList();
                return ls;
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetPhieuChiList(" + loaiPhieu + "," + from + "," + to + "):" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetPhieuChiList(" + loaiPhieu + "," + from + "," + to + "):" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetPhieuChiList(" + loaiPhieu + "," + from + "," + to + "):", ex);
                return null;
            }
        }
        public List<ViewPhieuChi> GetCMHTD()
        {
            try
            {
                List<ViewPhieuChi> ls = _ViewPhieuChiRepository.Table.Where(x => x.MaLoaiPhieu == "CMHTD" && x.IsDeleted == false).ToList();
                return ls;
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetCMHTD:" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetCMHTD:" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetCMHTD:", ex);
                return null;
            }

        }
        public List<ViewCTAUN> GetCTAUNList(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang)
        {
            try
            {
                var q = _viewCTAUNRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to);
                int num = q.Count();
                if (!string.IsNullOrEmpty(query))
                    q = q.Where(x => (x.SoChungTu.Contains(query)
                    || x.HoTen.Contains(query)
                    || x.DienThoai.Contains(query)
                    || x.TinhTrang.Contains(query))
                    || x.MaPhieuChi.Contains(query)

                      || query.Contains(x.MaPhieuChi.ToLower())
                      || query.Contains(x.SoChungTu.ToLower())
                       || query.Contains(x.DienThoai.ToLower())
                    //|| query.Contains(x.LyDoChi.ToLower())

                    );
                if (tinhtrang == "D")
                {
                    q = q.Where(x => x.TinhTrang == "Đã trả");
                }
                else if (tinhtrang == "C")
                {
                    q = q.Where(x => x.TinhTrang == "Còn nợ");
                }
                var result = q.OrderByDescending(x => x.NgayChi).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetCTAUNList:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ")" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetCTAUNList:("+ from +","+ to + "," + query + "," + p + "," + total + "," + pageSize + ")" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetCTAUNList:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ")", ex);
                return null;
            }

        }
        public List<ViewHinhThucThanhToan> GetHTTT()
        {
            try
            {
                return _htttRepository.Table.OrderBy(x => x.HienThi).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetHTTT" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetHTTT" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetHTTT", ex);
                return null;
            }
        }
        /// <summary>
        /// Lấy danh sách đối tác theo loai
        /// </summary>
        /// <param name="id">
        /// NDT: nhà đầu tư
        /// NCC: nhà cung cấp
        /// PK: phụ kiện
        /// </param>
        /// <returns></returns>
        public List<DoiTac> GetDoiTac(string id)
        {
            try
            {
                SqlParameter loai = new SqlParameter("loai", id);
                SqlParameter isForInsert = new SqlParameter("isForInsert", true);
                return _dbContext.ExecuteStoredProcedureList<DoiTac>("sp_SelectDoiTac", loai, isForInsert).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetDoiTac" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetDoiTac" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetDoiTac", ex);
                return null;
            }
        }
        public List<DoiTac> GetDoiTac_VM(string id)
        {
            try
            {
                List<DoiTac> list = _doiTacRepository.Table.Where(x => x.LoaiDoiTac == id && x.IsDeleted == false).ToList();
                return list;
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.GetDoiTac_VM", ex);
                return null;
            }
        }
        public string CheckNCC_CMPT_Import(string TenNCC)
        {
            try
            {
                List<DoiTac> list = GetDoiTac("NCC");
                string MaDoitac = "";
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item.TenDoiTac.ToLower().Trim() == TenNCC.ToLower().Trim())
                        {
                            MaDoitac = item.MaDoiTac.Trim();
                            break;
                        }
                    }
                }
                return MaDoitac;
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.CheckNCC_CMPT_Import", ex);
                return null;
            }
        }
        public bool CheckKhoNhap(string KhoNhap)
        {
            try
            {
                KhoNhap = KhoNhap.Trim().ToUpper();
                List<ViewNguonKhuyenMai> list = _tuiDinhKhoanService.GetKhoNhapPhuTung(true);
                ViewNguonKhuyenMai item = list.Where(x => x.NguonKhuyenMai == KhoNhap.Trim()).FirstOrDefault();
                if (item != null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.CheckKhoNhap", ex);
                return false;
            }
        }
        /// <summary>
        /// Lay du an đầu tư từ danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CategoryItem> GetDuAnDauTu(string id)
        {
            try
            {
                SqlParameter loai = new SqlParameter("loai", id);
                SqlParameter isForInsert = new SqlParameter("isForInsert", true);
                return _dbContext.ExecuteStoredProcedureList<CategoryItem>("sp_SelectDuAnDauTu", loai, isForInsert).ToList();
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetDuAnDauTu : ("+ id +")" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetDuAnDauTu : (" + id + ")" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetDuAnDauTu : (" + id + ")", ex);
                return null;
            }

        }

        /// <summary>
        /// Lay lý do chi từ danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CategoryItem> GetLyDoChi(string id)
        {
            try
            {
                SqlParameter loai = new SqlParameter("loai", id);
                SqlParameter isForInsert = new SqlParameter("isForInsert", true);
                return _dbContext.ExecuteStoredProcedureList<CategoryItem>("sp_SelectLyDoChi", loai, isForInsert).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetLyDoChi : (" + id + ")" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetLyDoChi : (" + id + ")" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetLyDoChi : (" + id + ")", ex.Message.ToString());
                return null;
            }

        }

        /// <summary>
        /// Lay tên tài sản  từ danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CategoryItem> GetTenTaiSan(string id)
        {
            try
            {
                SqlParameter loai = new SqlParameter("loai", id);
                SqlParameter isForInsert = new SqlParameter("isForInsert", true);
                return _dbContext.ExecuteStoredProcedureList<CategoryItem>("sp_SelectTenTaiSan", loai, isForInsert).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetTenTaiSan : (" + id + ")" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetTenTaiSan : (" + id + ")" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetTenTaiSan : (" + id + ")", ex);
                return null;
            }
        }

        public List<ChiTietPhieuChi> GetCTPC(string id)
        {
            try
            {
                return _ctptRepository.Table.Where(x => x.MaPhieuChi == id && x.IsDeleted == false && x.IsActive == true && x.HinhThucThanhToan != "DC").ToList().Select(x => new ChiTietPhieuChi
                {
                    Id = x.Id,
                    TinhTrang = x.TinhTrang,
                    Temp = x.MaNganHang + "-" + x.HinhThucThanhToan,
                    HinhThucThanhToan = x.HinhThucThanhToan,
                    MaPhieuChi = x.MaPhieuChi,
                    MaNganHang = x.MaNganHang,
                    SoThamChieu = x.SoThamChieu,
                    SoTienThanhToan = x.SoTienThanhToan
                }).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetCTPC : (" + id + ")" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetCTPC : (" + id + ")" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetCTPC : (" + id + ")", ex);
                return null;
            }

        }
        // get list CHI_TIET_PHIEU_CHI
        public List<ChiTietPhieuChi> getListCTPC(string id)
        {
            try
            {
                return _ctptRepository.Table.Where(x => x.MaPhieuChi == id && x.IsDeleted == false && x.IsActive == true).ToList().Select(x => new ChiTietPhieuChi
                {
                    Id = x.Id,
                    TinhTrang = x.TinhTrang,
                    HinhThucThanhToan = x.HinhThucThanhToan,
                    MaPhieuChi = x.MaPhieuChi,
                    MaNganHang = x.MaNganHang,
                    SoThamChieu = x.SoThamChieu,
                    SoTienThanhToan = x.SoTienThanhToan
                }).ToList();
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.getListCTPC : (" + id + ")" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.getListCTPC : (" + id + ")" + ex.Message.ToString());
                _log.WriteLog("SoChiService.getListCTPC : (" + id + ")", ex);
                return null;
            }
        }
        public string CreateId(string mlp)
        {
            try
            {
                SqlParameter MaLoaiPhieu = new SqlParameter("MaLoaiPhieu", mlp);
                SqlParameter MaPhieuChi = new SqlParameter();
                MaPhieuChi.Direction = System.Data.ParameterDirection.Output;
                MaPhieuChi.DbType = System.Data.DbType.String;
                MaPhieuChi.Size = 100;
                MaPhieuChi.ParameterName = "MaPhieuChi";
                _dbContext.ExecuteStoredProcedure("sp_GetLastId",
                    MaLoaiPhieu,
                    MaPhieuChi
                    );
                return MaPhieuChi.Value.ToString();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    _log.WriteLog("SoChiService.CreateId : (" + mlp + ")" + ex.InnerException.ToString());
                else
                    _log.WriteLog("SoChiService.CreateId : (" + mlp + ")" + ex.Message.ToString());
                return null;
            }

        }
        public bool CreateChiTietPhieuChi(string maPhieuChi, List<ChiTietPhieuChi> httt)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                foreach (var item in httt)
                {
                    if (!string.IsNullOrEmpty(item.Temp))
                    {
                        var a = item.Temp.Split('-');
                        if (a.Length == 2)
                        {
                            item.Id = Guid.NewGuid();
                            item.MaPhieuChi = maPhieuChi;
                            item.HinhThucThanhToan = a[1];
                            item.MaNganHang = a[0];
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = uid;
                            item.CreatedDate = d;
                            item.UpdatedDate = d;
                            item.UpdatedBy = uid;
                            _ctptRepository.Insert(item);
                            _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, item.MaPhieuChi, item.MaNganHang, "CREATE", "CreateChiTietPhieuChi", "Mã phiếu chi :" + item.MaPhieuChi + " mã ngân hàng :" + item.MaNganHang + " Tổng cộng :" + item.SoTienThanhToan, _authenticationService.GetAuthenticatedUser().UserId);
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.CreateChiTietPhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.CreateChiTietPhieuChi: " + e.Message.ToString());
                _log.WriteLog("SoChiService.CreateChiTietPhieuChi: " + e);
                return false;
            }
        }
        public string CreatePhieuChi(PhieuChi model)
        {
            try
            {
                SqlParameter MPC = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuChi", MPC, Ngay, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();

                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuChi = CreateId(model.MaLoaiPhieu);
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                if (model.TongCong == 0)
                    model.TongCong = model.SoTienChi;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        model.ConLai = model.TongCong;
                        break;
                }
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                _PhieuChiRepository.Insert(model);
                XuLySauKhiInsertPhieuChi(model.MaPhieuChi, "");
                return "";
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.CreatePhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.CreatePhieuChi: " + e.Message.ToString());
                _log.WriteLog("SoChiService.CreatePhieuChi: ", e);
                return e.Message;
            }
        }
        public string CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, bool isGoiXuLySauKhiInsertPhieuChi = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                //Xu ly truoc khi insert phieu chi
                if (model.MaLoaiPhieu == "CMXTT")
                {
                    if (_khoXeRepository.Table.Where(x => x.IsDeleted == false && x.SoKhung == model.SoKhung).ToList().Count > 0)
                        return "Xe với số khung " + model.SoKhung + " đã có trong kho. Vui lòng kiểm tra lại.";
                }
                SqlParameter MPC = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuChi", MPC, Ngay, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuChi = CreateId(model.MaLoaiPhieu);
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                model.SoTienChi = 0;
                //model.NgayHachToan = model.NgayChi;           
                foreach (var item in httt)
                {
                    if (item.IsDeleted == false)
                    {
                        if (!string.IsNullOrEmpty(item.Temp))
                        {
                            var a = item.Temp.Split('-');
                            if (a.Length == 2)
                            {
                                item.Id = Guid.NewGuid();
                                item.MaPhieuChi = model.MaPhieuChi;
                                item.HinhThucThanhToan = a[1];
                                item.MaNganHang = a[0];
                                item.IsActive = true;
                                item.IsDeleted = false;
                                item.CreatedBy = uid;
                                item.CreatedDate = d;
                                item.UpdatedDate = d;
                                item.UpdatedBy = uid;
                                _ctptRepository.Insert(item);
                                model.SoTienChi += item.SoTienThanhToan;
                            }
                        }
                    }
                }
                if (model.TongCong == 0)
                    model.TongCong = model.SoTienChi;
                if (model.TongCong > model.SoTienChi)
                    model.ConLai = model.TongCong - model.SoTienChi;
                if (model.MaLoaiPhieu == "CMHTD")
                    model.NhaCungCap = model.DoiTac;
                if (model.MaLoaiPhieu == "CDACO" || model.MaLoaiPhieu == "CCOMX" || model.MaLoaiPhieu == "CCOPT" || model.MaLoaiPhieu == "CCOGC" || model.MaLoaiPhieu == "CCOTH" || model.MaLoaiPhieu == "CKHAC")
                    model.ConLai = model.TongCong;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        model.ConLai = model.TongCong;
                        break;
                    case "CTAUN":
                        model.ConLai = model.TongCong;
                        break;

                }
                if (model.MaLoaiPhieu == "CRUVO")
                {
                    model.TongCong = model.SoTienChi;
                }
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                _PhieuChiRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuChi, model.MaLoaiPhieu, "CREATE", "CreatePhieuChi", "Mã phiếu chi :" + model.MaPhieuChi + " mã loại phiếu :" + model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuChi)
                    XuLySauKhiInsertPhieuChi(model.MaPhieuChi, giaTriTrenGiaoDien);
                return "";
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): " + e.Message.ToString());
                _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): ", e);
                return "Lỗi khi tạo mới phiếu chi.";
            }
        }
        //phan luu chi_tiet_phieu_chi
        public string CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> CTPC, bool isGoiXuLySauKhiInsertPhieuChi = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                if (model.MaLoaiPhieu == "CMXTT" && !model.MaXe.Contains("XC-"))
                {
                    if (_khoXeRepository.Table.Any(x => x.IsDeleted == false && x.SoKhung.Trim() == model.SoKhung.Trim()))
                        return "Xe với số khung " + model.SoKhung.Trim() + " đã có trong kho. Vui lòng kiểm tra lại.";
                }

                SqlParameter MPC = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuChi", MPC, Ngay, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
               
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuChi = CreateId(model.MaLoaiPhieu);
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                model.SoTienChi = 0;
                foreach (var item in httt)
                {
                    if (!string.IsNullOrEmpty(item.Temp) && item.IsDeleted == false)
                    {
                        var a = item.Temp.Split('-');
                        if (a.Length == 2)
                        {
                            item.Id = Guid.NewGuid();
                            item.MaPhieuChi = model.MaPhieuChi;
                            item.HinhThucThanhToan = a[1];
                            item.MaNganHang = a[0];
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = uid;
                            item.CreatedDate = d;
                            item.UpdatedDate = d;
                            item.UpdatedBy = uid;
                            _ctptRepository.Insert(item);
                            model.SoTienChi += item.SoTienThanhToan;
                        }
                    }
                }
                if (CTPC != null)
                {
                    foreach (var ctpc in CTPC)
                    {
                        ctpc.Id = Guid.NewGuid();
                        ctpc.IsActive = true;
                        ctpc.IsDeleted = false;
                        ctpc.MaPhieuChi = model.MaPhieuChi;
                        ctpc.CreatedBy = uid;
                        ctpc.CreatedDate = d;
                        ctpc.UpdatedDate = d;
                        ctpc.UpdatedBy = uid;
                        ctpc.TinhTrang = false;
                        ctpc.HinhThucThanhToan = "DC";
                        ctpc.GhiChu = "";
                        ctpc.MaNganHang = "";
                        _ctptRepository.Insert(ctpc);
                        model.SoTienChi += ctpc.SoTienThanhToan;
                    }
                }
                if (CTPC != null)
                {
                    foreach (var item_PC in CTPC)
                    {
                        var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_PC.SoThamChieu);
                        data_PC.SoTienChi = item_PC.SoTienThanhToan;
                        //data_PC.ConLai = data_PC.TongCong - data_PC.SoTienChi;
                        data_PC.UpdatedDate = d;
                        data_PC.UpdatedBy = uid;
                        _PhieuChiRepository.Update(data_PC);
                    }

                }
                if (model.TongCong == 0)
                    model.TongCong = model.SoTienChi;
                if (model.TongCong > model.SoTienChi)
                    model.ConLai = model.TongCong - model.SoTienChi;
                if (model.MaLoaiPhieu == "CMHTD")
                    model.NhaCungCap = model.DoiTac;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        model.ConLai = model.TongCong;
                        break;
                }
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                if (!string.IsNullOrWhiteSpace(model.SoKhung))
                    model.SoKhung = model.SoKhung.Trim();
                _PhieuChiRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuChi, model.MaLoaiPhieu, "CREATE", "CreatePhieuChi", "Mã phiếu chi :" + model.MaPhieuChi + " mã loại phiếu :" + model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuChi)
                    XuLySauKhiInsertPhieuChi(model.MaPhieuChi, giaTriTrenGiaoDien);
                return "";
            }
            catch (Exception e)
            {
                //if (e.InnerException != null)
                //    _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): " + e.Message.ToString());
                _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): ", e);
                return "Lỗi khi tạo mới phiếu chi.";
            }
        }
        public string CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, List<ChiTietNhapKhoPhuTung> ListCTNKPT, List<ChiTietPhieuChi> CTPC, bool isGoiXuLySauKhiInsertPhieuChi = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                SqlParameter MPC = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuChi", MPC, Ngay, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuChi = CreateId(model.MaLoaiPhieu);
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                foreach (var item in httt)
                {
                    if (!string.IsNullOrEmpty(item.Temp) && item.IsDeleted == false)
                    {
                        var a = item.Temp.Split('-');
                        if (a.Length == 2)
                        {
                            item.Id = Guid.NewGuid();
                            item.MaPhieuChi = model.MaPhieuChi;
                            item.HinhThucThanhToan = a[1];
                            item.MaNganHang = a[0];
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = uid;
                            item.CreatedDate = d;
                            item.UpdatedDate = d;
                            item.UpdatedBy = uid;
                            _ctptRepository.Insert(item);
                            model.SoTienChi += item.SoTienThanhToan;
                        }
                    }
                }
                foreach (var itemCTNKPT in ListCTNKPT)
                {
                    if (!itemCTNKPT.IsDeleted)
                    {
                        itemCTNKPT.MaPhieuChi = model.MaPhieuChi;
                        itemCTNKPT.NgayNhapKho = model.NgayNhapKho;
                        _ctnkptService.InsertCTNKPT(itemCTNKPT);
                    }
                }
                if (CTPC != null)
                {
                    foreach (var ctpc in CTPC)
                    {
                        if (ctpc != null)
                        {
                            ctpc.Id = Guid.NewGuid();
                            ctpc.IsActive = true;
                            ctpc.IsDeleted = false;
                            ctpc.MaPhieuChi = model.MaPhieuChi;
                            ctpc.CreatedBy = uid;
                            ctpc.CreatedDate = d;
                            ctpc.UpdatedDate = d;
                            ctpc.UpdatedBy = uid;
                            ctpc.TinhTrang = false;
                            ctpc.HinhThucThanhToan = "DC";
                            ctpc.GhiChu = "";
                            ctpc.MaNganHang = "";
                            _ctptRepository.Insert(ctpc);
                        }

                        model.SoTienChi += ctpc.SoTienThanhToan;
                    }
                }
                if (CTPC != null)
                {
                    foreach (var item_PC in CTPC)
                    {
                        if (item_PC != null)
                        {
                            var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_PC.SoThamChieu);
                            data_PC.SoTienChi = item_PC.SoTienThanhToan;
                            data_PC.UpdatedDate = d;
                            data_PC.UpdatedBy = uid;
                            //data_PC.ConLai = data_PC.TongCong - data_PC.SoTienChi;
                            _PhieuChiRepository.Update(data_PC);
                        }

                    }

                }
                if (model.TongCong == 0)
                    model.TongCong = model.SoTienChi;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        model.ConLai = model.TongCong;
                        break;
                }
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                _PhieuChiRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuChi, model.MaLoaiPhieu, "CREATE", "CreatePhieuChi", "Mã phiếu chi :" + model.MaPhieuChi + " mã loại phiếu :" + model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuChi)
                    XuLySauKhiInsertPhieuChi(model.MaPhieuChi, giaTriTrenGiaoDien);
                return "";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): " + e.InnerException.ToString());
                else
                    _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): " + e.Message.ToString());
                return e.Message;
            }
        }
        public string CreatePhieuChi_ImportCMPT(PhieuChi model, List<ChiTietPhieuChi> httt, List<ChiTietNhapKhoPhuTung> ListCTNKPT, List<ChiTietPhieuChi> CTPC, bool isGoiXuLySauKhiInsertPhieuChi = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                SqlParameter MPC = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuChi", MPC, Ngay, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuChi = CreateId(model.MaLoaiPhieu);
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                if (httt != null)
                {
                    foreach (var item in httt)
                    {
                        if (!string.IsNullOrEmpty(item.Temp) && item.IsDeleted == false)
                        {
                            var a = item.Temp.Split('-');
                            if (a.Length == 2)
                            {
                                item.Id = Guid.NewGuid();
                                item.MaPhieuChi = model.MaPhieuChi;
                                item.HinhThucThanhToan = a[1];
                                item.MaNganHang = a[0];
                                item.IsActive = true;
                                item.IsDeleted = false;
                                item.CreatedBy = uid;
                                item.CreatedDate = d;
                                item.UpdatedDate = d;
                                item.UpdatedBy = uid;
                                _ctptRepository.Insert(item);
                                model.SoTienChi += item.SoTienThanhToan;
                            }
                        }
                    }
                }
                if (ListCTNKPT != null)
                {
                    foreach (var itemCTNKPT in ListCTNKPT)
                    {
                        if (!itemCTNKPT.IsDeleted)
                        {
                            itemCTNKPT.MaPhieuChi = model.MaPhieuChi;
                            //itemCTNKPT.NgayNhapKho = model.NgayNhapKho; trúc bỏ vì phiếu import sẽ có cột ngày nhập kho rồi
                            _ctnkptService.InsertCTNKPT(itemCTNKPT);
                        }
                    }
                }
                if (CTPC != null)
                {
                    foreach (var ctpc in CTPC)
                    {
                        if (ctpc != null)
                        {
                            ctpc.Id = Guid.NewGuid();
                            ctpc.IsActive = true;
                            ctpc.IsDeleted = false;
                            ctpc.MaPhieuChi = model.MaPhieuChi;
                            ctpc.CreatedBy = uid;
                            ctpc.CreatedDate = d;
                            ctpc.UpdatedDate = d;
                            ctpc.UpdatedBy = uid;
                            ctpc.TinhTrang = false;
                            ctpc.HinhThucThanhToan = "DC";
                            ctpc.GhiChu = "";
                            ctpc.MaNganHang = "";
                            _ctptRepository.Insert(ctpc);
                        }

                        model.SoTienChi += ctpc.SoTienThanhToan;
                    }
                }
                if (CTPC != null)
                {
                    foreach (var item_PC in CTPC)
                    {
                        if (item_PC != null)
                        {
                            var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_PC.SoThamChieu);
                            data_PC.SoTienChi = item_PC.SoTienThanhToan;
                            data_PC.UpdatedDate = d;
                            data_PC.UpdatedBy = uid;
                            //data_PC.ConLai = data_PC.TongCong - data_PC.SoTienChi;
                            _PhieuChiRepository.Update(data_PC);
                        }

                    }

                }
                if (model.TongCong == 0)
                    model.TongCong = model.SoTienChi;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        model.ConLai = model.TongCong;
                        break;
                }
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                _PhieuChiRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuChi, model.MaLoaiPhieu, "CREATE", "CreatePhieuChi", "Mã phiếu chi :" + model.MaPhieuChi + " mã loại phiếu :" + model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuChi)
                    XuLySauKhiInsertPhieuChi(model.MaPhieuChi, giaTriTrenGiaoDien);
                return "";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): " + e.InnerException.ToString());
                else
                    _log.WriteLog("SoChiService.CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt): " + e.Message.ToString());
                return e.Message;
            }
        }
        public string XuLySauKhiInsertPhieuChi(string maPhieuChi, string sessionId, string giaTriTrenGiaoDien = "")
        {
            try
            {
                giaTriTrenGiaoDien = "<Browser>" + _clientBrowser + "</Browser>" + giaTriTrenGiaoDien;
                SqlParameter MPT = new SqlParameter("MaPhieuChi", maPhieuChi);
                SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);
                SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                Error.Direction = System.Data.ParameterDirection.Output;
                Error.Size = 4000;
                _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiInsertPhieuChi", MPT, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Error);
                if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                {
                    int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                    if (gioGuiEmailLast != GioGuiEmail)
                    {
                        MailHelper Mail = new MailHelper();
                        string Content = ContentMail(maPhieuChi);
                        Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                        //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                        Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                        gioGuiEmailLast = GioGuiEmail;
                    }
                    else
                    {
                        if (Send == false)
                        {
                            MailHelper Mail = new MailHelper();
                            string Content = ContentMail(maPhieuChi);
                            Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            gioGuiEmailLast = GioGuiEmail;
                            Send = true;
                        }
                    }
                }
                return Error.Value.ToString();
            }
            catch (Exception e)
            {
                _log.WriteLog("SoChiService.XuLySauKhiInsertPhieuChi: ", e);
                return null;
            }
        }
        public string UpdatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, ref string sessionId, bool isGoiXuLySauKhiUpdatePhieuChi = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                giaTriTrenGiaoDien = giaTriTrenGiaoDien.Replace("<formdata>", "<formdata>" + "<Browser>" + _clientBrowser + "</Browser>");
                var obj = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == model.MaPhieuChi && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                    return "Phiếu không tồn tại hoặc đã bị xóa";
                SqlParameter MPT1 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuChi", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                sessionId = sessionid.Value.ToString();
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                // truc them
                var NPT = _noPhaiThuRepository.Table.FirstOrDefault(x => x.MaPhieuPhatSinh == model.MaPhieuChi);

                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.SoTienChi = 0;
                obj.TongCong = model.TongCong;
                obj.GiaVon = model.GiaVon;
                obj.GiaBan = model.GiaBan;
                obj.GhiChu = model.GhiChu;
                obj.NguoiDuyet = model.NguoiDuyet;
                SqlParameter pSessionId = new SqlParameter("SessionId", sessionid.Value);
                SqlParameter pMaPhieuChi = new SqlParameter("maPhieuChi", model.MaPhieuChi);
                //neu la loại chi tổng hợp mà bị thay đổi lý do chi thì phải xóa các thanh toán cọc đã sử dụng của phiếu này
                if (model.MaLoaiPhieu == "CTOHO")
                {
                    if (obj.LyDoChi != model.LyDoChi)
                    {
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuChiSuDungCoc", pSessionId, pMaPhieuChi);
                    }
                }
                //neu la loại chi tổng hợp mà bị thay đổi lý do chi thì phải xóa các thanh toán cọc đã sử dụng của phiếu này
                if (model.MaLoaiPhieu == "CMPTU"
                    || model.MaLoaiPhieu == "CMUTS"
                    || model.MaLoaiPhieu == "CMXTT"
                    || model.MaLoaiPhieu == "NGCNG"
                    )
                {
                    if (obj.DoiTac != model.DoiTac)
                    {
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuChiSuDungCoc", pSessionId, pMaPhieuChi);
                    }
                }
                SetValue(model, ref obj,uid);
                //var lst = httt.Select(x => x.Id);
                //var notexist = _ctptRepository.Table.Where(x => lst.Contains(x.Id)).ToList();
                foreach (var item in httt)
                {
                    var data = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id);
                    if (!item.IsDeleted && !string.IsNullOrEmpty(item.Temp))
                    {
                        bool flag = true;
                        if (data == null)
                        {
                            data = new ChiTietPhieuChi();
                            data.Id = Guid.NewGuid();
                            data.IsDeleted = false;
                            data.IsActive = true;
                            data.CreatedBy = uid;
                            data.CreatedDate = d;
                            flag = false;
                        }
                        var a = item.Temp.Split('-');
                        data.MaPhieuChi = obj.MaPhieuChi;
                        data.HinhThucThanhToan = a[1];
                        data.MaNganHang = a[0];
                        data.SoThamChieu = item.SoThamChieu;
                        data.TinhTrang = item.TinhTrang;
                        data.SoTienThanhToan = item.SoTienThanhToan;
                        data.UpdatedBy = uid;
                        data.UpdatedDate = d;
                        if (!flag)
                        {
                            _ctptRepository.Insert(data);
                        }
                        else
                        {
                            _ctptRepository.Update(data);
                        }
                        obj.SoTienChi += data.SoTienThanhToan;
                        ////tfs
                        ////phan truc sua
                        //if (model.MaLoaiPhieu == "CMUTS")
                        //{
                        //    obj.TongCong = model.TongCong;
                        //    obj.ConLai = model.TongCong - data.SoTienThanhToan;
                        //    //npt.SoTienConLai = model.TongCong - data.SoTienThanhToan;
                        //}
                        //else
                        //{
                        //    obj.TongCong += data.SoTienThanhToan;
                        //}

                    }
                    else
                    {
                        if (data != null)
                        {
                            data.IsDeleted = true;
                            data.UpdatedDate = d;
                            data.UpdatedBy = uid;
                            _ctptRepository.Update(data);
                        }
                    }
                }
                //foreach (var item in notexist)
                //{
                //    var data = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id);

                //}
                if (model.TongCong == 0)
                {
                    obj.TongCong = obj.SoTienChi;
                    if (obj.MaLoaiPhieu == "CDACO" || obj.MaLoaiPhieu == "CCOMX" || obj.MaLoaiPhieu == "CCOPT" || obj.MaLoaiPhieu == "CCOGC" || obj.MaLoaiPhieu == "CCOTH")
                        obj.ConLai = model.TongCong;
                    //truc them cap nhat so tien con lai cho ds chi tam ung
                    if (model.MaLoaiPhieu == "CTAUN")
                    {
                        NPT.SoTienConLai = obj.TongCong - NPT.SoTienDaTra;
                    }
                }
                else
                {
                    //truc them
                    if (model.MaLoaiPhieu == "CTAUN")
                    {
                        NPT.SoTienConLai = model.TongCong - NPT.SoTienDaTra;
                    }
                }

                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        obj.ConLai = obj.TongCong;
                        break;
                    case "CTAUN":
                        obj.ConLai = obj.TongCong;
                        break;
                }
                if (obj.MaLoaiPhieu != "CTAUN")
                {
                    obj.ConLai = obj.TongCong - obj.SoTienChi;
                }
                obj.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                _PhieuChiRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuChi, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuChi", "Mã phiếu chi :" + obj.MaPhieuChi + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);

                //truc them
                if (NPT != null)
                {
                    _noPhaiThuRepository.Update(NPT);
                }
                if (isGoiXuLySauKhiUpdatePhieuChi)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý sau khi update
                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);

                    SqlParameter Message = new SqlParameter("message", message.Value != null ? message.Value.ToString() : "");//xử lý sau khi update
                    Message.Direction = System.Data.ParameterDirection.InputOutput;
                    SqlParameter Action = new SqlParameter("Action", "UPDATE");//xử lý sau khi update
                    Message.Size = 4000;
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    Error.Size = 4000;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuChi", MPT2, SessionId2, Action, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Message, Error);
                    string val = Message.Value != null ? Message.Value.ToString() : "";
                    if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                    {
                        int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                        if (gioGuiEmailLast != GioGuiEmail)
                        {
                            MailHelper Mail = new MailHelper();
                            string Content = ContentMail(model.MaPhieuChi);
                            Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            gioGuiEmailLast = GioGuiEmail;
                        }
                        else
                        {
                            if (Send == false)
                            {
                                MailHelper Mail = new MailHelper();
                                string Content = ContentMail(model.MaPhieuChi);
                                Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                                // Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                                Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                                gioGuiEmailLast = GioGuiEmail;
                                Send = true;
                            }
                        }
                    }
                    return Error.Value.ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.UpdatePhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.UpdatePhieuChi: " + e.Message.ToString());
                _log.WriteLog("SoChiService.UpdatePhieuChi: ", e);
                return "ERROR:" + e.ToString();
            }
        }

        public string UpdatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, List<ChiTietPhieuChi> ChiTietCoc, ref string sessionId, bool isGoiXuLySauKhiUpdatePhieuChi = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                giaTriTrenGiaoDien = giaTriTrenGiaoDien.Replace("<formdata>", "<formdata>" + "<Browser>" + _clientBrowser + "</Browser>");
                var obj = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == model.MaPhieuChi && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                    return "Phiếu không tồn tại hoặc đã bị xóa";
                SqlParameter MPT1 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuChi", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                sessionId = sessionid.Value.ToString();
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;


                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.SoTienChi = 0;
                obj.TongCong = model.TongCong;
                obj.GiaVon = model.GiaVon;
                obj.GiaBan = model.GiaBan;
                obj.GhiChu = model.GhiChu;
                obj.NguoiDuyet = model.NguoiDuyet;
                SqlParameter pSessionId = new SqlParameter("SessionId", sessionid.Value);
                SqlParameter pMaPhieuChi = new SqlParameter("maPhieuChi", model.MaPhieuChi);
                //neu la loại chi tổng hợp mà bị thay đổi lý do chi thì phải xóa các thanh toán cọc đã sử dụng của phiếu này
                if (model.MaLoaiPhieu == "CTOHO")
                {
                    if (obj.LyDoChi != model.LyDoChi)
                    {
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuChiSuDungCoc", pSessionId, pMaPhieuChi);
                        //obj.LyDoChi = model.LyDoChi;
                    }
                }
                //neu la loại chi tổng hợp mà bị thay đổi lý do chi thì phải xóa các thanh toán cọc đã sử dụng của phiếu này
                if (model.MaLoaiPhieu == "CMPTU"
                    || model.MaLoaiPhieu == "CMUTS"
                    || model.MaLoaiPhieu == "CMXTT"
                    || model.MaLoaiPhieu == "NGCNG"
                    )
                {
                    if (obj.DoiTac != model.DoiTac)
                    {
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuChiSuDungCoc", pSessionId, pMaPhieuChi);
                        //obj = model.LyDoChi;
                    }
                }

                //var lst = httt.Select(x => x.Id);
                //var notexist = _ctptRepository.Table.Where(x => lst.Contains(x.Id)).ToList();
                foreach (var item in httt)
                {
                    var data = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id && x.IsDeleted == false);
                    if (!item.IsDeleted && !string.IsNullOrEmpty(item.Temp))
                    {
                        bool flag = true;
                        if (data == null)
                        {
                            data = new ChiTietPhieuChi();
                            data.Id = Guid.NewGuid();
                            data.IsDeleted = false;
                            data.IsActive = true;
                            data.CreatedBy = uid;
                            data.CreatedDate = d;
                            flag = false;

                        }
                        var a = item.Temp.Split('-');
                        data.MaPhieuChi = obj.MaPhieuChi;
                        data.HinhThucThanhToan = a[1];
                        data.MaNganHang = a[0];
                        data.SoThamChieu = item.SoThamChieu;
                        data.TinhTrang = item.TinhTrang;
                        data.SoTienThanhToan = item.SoTienThanhToan;
                        data.UpdatedBy = uid;
                        data.UpdatedDate = d;
                        if (!flag)
                        {
                            _ctptRepository.Insert(data);
                        }
                        else
                        {
                            _ctptRepository.Update(data);
                        }
                        obj.SoTienChi += data.SoTienThanhToan;
                    }
                    else
                    {
                        if (data != null)
                        {
                            data.IsDeleted = true;
                            data.UpdatedDate = d;
                            data.UpdatedBy = uid;
                            _ctptRepository.Update(data);
                        }
                    }
                }
                //phan update CTPC
                if (ChiTietCoc != null)
                {
                    if (obj.LyDoChi != model.LyDoChi || obj.DoiTac != model.DoiTac)
                    {
                        foreach (var item_ctpc in ChiTietCoc)
                        {
                            item_ctpc.UpdatedBy = uid;
                            item_ctpc.UpdatedDate = d;
                            item_ctpc.IsActive = true;
                            item_ctpc.IsDeleted = false;
                            item_ctpc.CreatedBy = uid;
                            item_ctpc.CreatedDate = d;
                            item_ctpc.Id = Guid.NewGuid();
                            item_ctpc.MaPhieuChi = model.MaPhieuChi;
                            item_ctpc.TinhTrang = false;
                            item_ctpc.HinhThucThanhToan = "DC";
                            _ctptRepository.Insert(item_ctpc);
                            obj.SoTienChi += item_ctpc.SoTienThanhToan;
                        }
                    }
                    else
                    {
                        foreach (var item_ctpc in ChiTietCoc)
                        {
                            PhieuChi maPhieuChiCoc = _PhieuChiRepository.Table.Where(x => x.MaPhieuChi == item_ctpc.SoThamChieu && x.IsDeleted == false).FirstOrDefault();
                            var data_CTPC = _ctptRepository.Table.FirstOrDefault(x => x.MaPhieuChi == model.MaPhieuChi && x.IsDeleted == false && x.SoThamChieu == maPhieuChiCoc.MaPhieuChi);
                            if (data_CTPC != null)
                            {
                                data_CTPC.UpdatedBy = uid;
                                data_CTPC.UpdatedDate = d;
                                data_CTPC.SoTienThanhToan = item_ctpc.SoTienThanhToan;
                                _ctptRepository.Update(data_CTPC);
                                var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_ctpc.SoThamChieu);
                                if (data_PC != null)
                                {
                                    data_PC.SoTienChi = item_ctpc.SoTienThanhToan;
                                    data_PC.UpdatedBy = uid;
                                    data_PC.UpdatedDate = d;
                                    _PhieuChiRepository.Update(data_PC);
                                }
                                obj.SoTienChi += data_CTPC.SoTienThanhToan;
                            }
                            else
                            {
                                data_CTPC = new ChiTietPhieuChi();
                                data_CTPC.CreatedBy = uid;
                                data_CTPC.CreatedDate = d;
                                data_CTPC.SoTienThanhToan = item_ctpc.SoTienThanhToan;
                                data_CTPC.Id = Guid.NewGuid();
                                data_CTPC.HinhThucThanhToan = "DC";
                                data_CTPC.UpdatedBy = uid;
                                data_CTPC.UpdatedDate = d;
                                data_CTPC.MaPhieuChi = model.MaPhieuChi;
                                data_CTPC.SoThamChieu = maPhieuChiCoc.MaPhieuChi;
                                _ctptRepository.Insert(data_CTPC);
                                var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_ctpc.SoThamChieu);
                                if (data_PC != null)
                                {
                                    data_PC.SoTienChi = item_ctpc.SoTienThanhToan;
                                    data_PC.UpdatedBy = uid;
                                    data_PC.UpdatedDate = d;
                                    _PhieuChiRepository.Update(data_PC);
                                }
                                obj.SoTienChi += data_CTPC.SoTienThanhToan;
                            }
                        }
                    }
                }
                SetValue(model, ref obj,uid);
                //obj.DoiTac = model.DoiTac;
                if (model.TongCong == 0)
                    obj.TongCong = obj.SoTienChi;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        obj.ConLai = model.TongCong;
                        break;
                }
                if (obj.MaLoaiPhieu == "CDACO" || obj.MaLoaiPhieu == "CCOMX"
                    || obj.MaLoaiPhieu == "CCOPT" || obj.MaLoaiPhieu == "CCOGC"
                    || obj.MaLoaiPhieu == "CCOTH")
                {
                    obj.TongCong = obj.SoTienChi;
                    obj.ConLai = obj.TongCong;
                }

                else
                    obj.ConLai = obj.TongCong - obj.SoTienChi;

                //if (obj.LyDoChi != model.LyDoChi)
                _PhieuChiRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuChi, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuChi", "Mã phiếu chi :" + obj.MaPhieuChi + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiUpdatePhieuChi)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý sau khi update
                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);

                    SqlParameter Message = new SqlParameter("message", message.Value != null ? message.Value.ToString() : "");//xử lý sau khi update
                    Message.Direction = System.Data.ParameterDirection.InputOutput;
                    SqlParameter Action = new SqlParameter("Action", "UPDATE");//xử lý sau khi update
                    Message.Size = 4000;
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    Error.Size = 4000;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuChi", MPT2, SessionId2, Action, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Message, Error);
                    string val = Message.Value != null ? Message.Value.ToString() : "";
                    return Error.Value.ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                //if (e.InnerException != null)
                //    _log.WriteLog("SoChiService.UpdatePhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.UpdatePhieuChi: " + e.Message.ToString());
                _log.WriteLog("SoChiService.UpdatePhieuChi: ", e);
                return "ERROR:" + e.ToString();
            }
        }


        public string UpdatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, NoPhaiTra noPhaiTra, List<ChiTietPhieuChi> ChiTietCoc, ref string sessionId, bool isGoiXuLySauKhiUpdatePhieuChi = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                giaTriTrenGiaoDien = giaTriTrenGiaoDien.Replace("<formdata>", "<formdata>" + "<Browser>" + _clientBrowser + "</Browser>");
                var obj = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == model.MaPhieuChi && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                    return "Phiếu không tồn tại hoặc đã bị xóa";
                SqlParameter MPT1 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update

                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuChi", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();

                sessionId = sessionid.Value.ToString();
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;

                if (model.MaLoaiPhieu == "CMXTT")
                {
                    obj.NgayVeDuKien = model.NgayVeDuKien;
                    obj.NgayVeThucTe = model.NgayVeThucTe;
                    obj.NguoiDuyet = model.NguoiDuyet;
                }
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.SoTienChi = 0;
                obj.TongCong = model.TongCong;
                obj.GiaVon = model.GiaVon;
                obj.GiaBan = model.GiaBan;
                obj.GhiChu = model.GhiChu;
                obj.NguoiDuyet = model.NguoiDuyet;
                SqlParameter pSessionId = new SqlParameter("SessionId", sessionid.Value);
                SqlParameter pMaPhieuChi = new SqlParameter("maPhieuChi", model.MaPhieuChi);
                //neu la loại chi tổng hợp mà bị thay đổi lý do chi thì phải xóa các thanh toán cọc đã sử dụng của phiếu này
                if (model.MaLoaiPhieu == "CTOHO")
                {
                    if (obj.LyDoChi != model.LyDoChi)
                    {
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuChiSuDungCoc", pSessionId, pMaPhieuChi);
                    }
                }
                //neu la loại chi tổng hợp mà bị thay đổi lý do chi thì phải xóa các thanh toán cọc đã sử dụng của phiếu này
                if (model.MaLoaiPhieu == "CMPTU"
                    || model.MaLoaiPhieu == "CMUTS"
                    || model.MaLoaiPhieu == "CMXTT"
                    || model.MaLoaiPhieu == "NGCNG"
                    )
                {
                    if (obj.DoiTac != model.DoiTac)
                    {
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuChiSuDungCoc", pSessionId, pMaPhieuChi);
                    }
                }
                //SetValue(model, ref obj);
                //var lst = httt.Select(x => x.Id);
                //var notexist = _ctptRepository.Table.Where(x => lst.Contains(x.Id)).ToList();
                foreach (var item in httt)
                {
                    var data = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id);
                    if (!item.IsDeleted && !string.IsNullOrEmpty(item.Temp))
                    {
                        bool flag = true;
                        if (data == null)
                        {
                            data = new ChiTietPhieuChi();
                            data.Id = Guid.NewGuid();
                            data.IsDeleted = false;
                            data.IsActive = true;
                            data.CreatedBy = uid;
                            data.CreatedDate = d;
                            flag = false;
                        }
                        var a = item.Temp.Split('-');
                        data.MaPhieuChi = obj.MaPhieuChi;
                        data.HinhThucThanhToan = a[1];
                        data.MaNganHang = a[0];
                        data.SoThamChieu = item.SoThamChieu;
                        data.TinhTrang = item.TinhTrang;
                        data.SoTienThanhToan = item.SoTienThanhToan;
                        data.UpdatedBy = uid;
                        data.UpdatedDate = d;
                        if (!flag)
                        {
                            _ctptRepository.Insert(data);
                        }
                        else
                        {
                            _ctptRepository.Update(data);
                        }

                        obj.SoTienChi += data.SoTienThanhToan;
                    }
                    else
                    {
                        if (data != null)
                        {
                            data.IsDeleted = true;
                            data.UpdatedDate = d;
                            data.UpdatedBy = uid;
                            _ctptRepository.Update(data);
                        }
                    }
                }
                //phan update CTPC
                if (ChiTietCoc != null)
                {
                    if (obj.LyDoChi != model.LyDoChi || obj.DoiTac != model.DoiTac)
                    {
                        foreach (var item_ctpc in ChiTietCoc)
                        {
                            item_ctpc.UpdatedBy = uid;
                            item_ctpc.UpdatedDate = d;
                            item_ctpc.IsActive = true;
                            item_ctpc.IsDeleted = false;
                            item_ctpc.CreatedBy = uid;
                            item_ctpc.CreatedDate = d;
                            item_ctpc.Id = Guid.NewGuid();
                            item_ctpc.MaPhieuChi = model.MaPhieuChi;
                            item_ctpc.TinhTrang = false;
                            item_ctpc.HinhThucThanhToan = "DC";
                            _ctptRepository.Insert(item_ctpc);
                            obj.SoTienChi += item_ctpc.SoTienThanhToan;
                        }
                    }
                    else
                    {
                        foreach (var item_ctpc in ChiTietCoc)
                        {
                            PhieuChi maPhieuChiCoc = _PhieuChiRepository.Table.Where(x => x.MaPhieuChi == item_ctpc.SoThamChieu && x.IsDeleted == false).FirstOrDefault();
                            var data_CTPC = _ctptRepository.Table.FirstOrDefault(x => x.MaPhieuChi == model.MaPhieuChi && x.IsDeleted == false && x.SoThamChieu == maPhieuChiCoc.MaPhieuChi);
                            if (data_CTPC != null)
                            {
                                data_CTPC.UpdatedBy = uid;
                                data_CTPC.UpdatedDate = d;
                                data_CTPC.SoTienThanhToan = item_ctpc.SoTienThanhToan;
                                _ctptRepository.Update(data_CTPC);
                                var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_ctpc.SoThamChieu);
                                if (data_PC != null)
                                {
                                    data_PC.SoTienChi = item_ctpc.SoTienThanhToan;
                                    //data_PC.ConLai = data_PC.TongCong - data_PC.SoTienChi;
                                    data_PC.UpdatedBy = uid;
                                    data_PC.UpdatedDate = d;
                                    _PhieuChiRepository.Update(data_PC);
                                }
                                obj.SoTienChi += data_CTPC.SoTienThanhToan;
                            }
                            else
                            {
                                data_CTPC = new ChiTietPhieuChi();
                                data_CTPC.CreatedBy = uid;
                                data_CTPC.CreatedDate = d;
                                data_CTPC.SoTienThanhToan = item_ctpc.SoTienThanhToan;
                                data_CTPC.Id = Guid.NewGuid();
                                data_CTPC.HinhThucThanhToan = "DC";
                                data_CTPC.UpdatedBy = uid;
                                data_CTPC.UpdatedDate = d;
                                data_CTPC.MaPhieuChi = model.MaPhieuChi;
                                data_CTPC.SoThamChieu = maPhieuChiCoc.MaPhieuChi;
                                _ctptRepository.Insert(data_CTPC);
                                var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_ctpc.SoThamChieu);
                                if (data_PC != null)
                                {
                                    data_PC.SoTienChi = item_ctpc.SoTienThanhToan;
                                    //data_PC.ConLai = data_PC.TongCong - data_PC.SoTienChi;
                                    data_PC.UpdatedBy = uid;
                                    data_PC.UpdatedDate = d;
                                    _PhieuChiRepository.Update(data_PC);
                                }
                                obj.SoTienChi += data_CTPC.SoTienThanhToan;
                            }
                        }
                    }
                }
                SetValue(model, ref obj,uid);

                if (model.TongCong == 0)
                    obj.TongCong = obj.SoTienChi;
                obj.ConLai = obj.TongCong - obj.SoTienChi;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        obj.ConLai = model.TongCong;
                        break;
                }
                _PhieuChiRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuChi, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuChi", "Mã phiếu chi :" + obj.MaPhieuChi + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                //cap nhat nophaitra: neu chua co phieu no thi insert, da co thi update
                noPhaiTra.SoTienNo = obj.TongCong - obj.SoTienChi;
                noPhaiTra.SoTienConLai = noPhaiTra.SoTienNo - noPhaiTra.SoTienDaTra;
                noPhaiTra.DonViNo = model.DoiTac;
                try
                {
                    if (string.IsNullOrEmpty(noPhaiTra.MaPhieuNo) && noPhaiTra.SoTienNo > 0)
                        _noPhaiTraService.InsertNoPhaiTra(noPhaiTra, obj.MaPhieuChi);
                    else
                    {
                        if (noPhaiTra.SoTienNo > 0)
                            _noPhaiTraService.UpdateNoPhaiTra(noPhaiTra);
                        else
                            _noPhaiTraService.DeleteNoPhaiTra(noPhaiTra, "Delete do số tiền nợ được update = 0 khi edit phiếu chi");
                    }
                }
                catch (Exception ex)
                {
                    _log.WriteLog(ex.ToString());
                }
                if (isGoiXuLySauKhiUpdatePhieuChi)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý sau khi update

                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                    SqlParameter Message = new SqlParameter("message", message.Value != null ? message.Value.ToString() : "");//xử lý sau khi update
                    Message.Direction = System.Data.ParameterDirection.InputOutput;
                    SqlParameter Action = new SqlParameter("Action", "UPDATE");//xử lý sau khi update
                    Message.Size = 4000;
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    Error.Size = 4000;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuChi", MPT2, SessionId2, Action, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Message, Error);

                    string val = Message.Value != null ? Message.Value.ToString() : "";
                    return Error.Value.ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.UpdatePhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.UpdatePhieuChi: " + e.Message.ToString());
                _log.WriteLog("SoChiService.UpdatePhieuChi: ", e);
                return "ERROR:" + e.ToString();
            }
        }

        public void SetValue(PhieuChi model, ref PhieuChi obj,Guid uid)
        {
            obj.BienSo = model.BienSo;
            obj.ConLai = model.ConLai;

            obj.CTBH = model.CTBH;
            obj.DiaChi = model.DiaChi;
            obj.DienThoai = model.DienThoai;
            obj.DoiTac = model.DoiTac;

            obj.DVCV = model.DVCV;
            obj.GhiChu = model.GhiChu;
            obj.GiaBan = model.GiaBan;
            obj.GiaVon = model.GiaVon;
            obj.HoTen = model.HoTen;
            obj.KeToanTruong = model.KeToanTruong;
            obj.LoaiXe = model.LoaiXe;
            obj.MauXe = model.MauXe;
            obj.MaXe = model.MaXe;

            obj.NguoiLapPhieu = model.NguoiLapPhieu;
            obj.NguoiNopTien = model.NguoiNopTien;
            obj.NgayHachToan = model.NgayHachToan.Value;
            obj.NgayChi = model.NgayChi;
            obj.SoHoaDon = model.SoHoaDon;
            obj.NguoiThuTien = model.NguoiThuTien==null? uid: model.NguoiThuTien;
            obj.NguoiUngTien = model.NguoiUngTien;
            obj.NhaCungCap = model.NhaCungCap;
            obj.NoiDung = model.NoiDung;
            obj.SoChungTu = model.SoChungTu;
            obj.SoHopDong = model.SoHopDong;
            obj.SoTienUng = model.SoTienUng;
            obj.TinhTrangPhieu = model.TinhTrangPhieu;
            obj.TongCong = model.TongCong;

            obj.DuAn = model.DuAn;
            obj.LyDoChi = model.LyDoChi;
            obj.TaiSan = model.TaiSan;

            obj.SoKhung = model.SoKhung;
            obj.SoMay = model.SoMay;

            obj.GhiChu = model.GhiChu;
            obj.NguoiDuyet = model.NguoiDuyet;
        }
        public string UpdatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, List<ChiTietNhapKhoPhuTung> ListCTNKPT, List<ChiTietPhieuChi> ChiTietCoc, ref string sessionId, bool isGoiXuLySauKhiUpdatePhieuChi = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                giaTriTrenGiaoDien = giaTriTrenGiaoDien.Replace("<formdata>", "<formdata>" + "<Browser>" + _clientBrowser + "</Browser>");
                var obj = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == model.MaPhieuChi && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                    return "";
                SqlParameter MPT1 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update

                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayChi);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuChi", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();

                sessionId = sessionid.Value.ToString();
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;


                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.SoTienChi = 0;
                obj.TongCong = model.TongCong;
                obj.GiaVon = model.GiaVon;
                obj.GiaBan = model.GiaBan;
                obj.NgayChi = model.NgayChi;
                obj.NgayNhapKho = model.NgayNhapKho;
                obj.NgayHachToan = model.NgayHachToan;
                obj.NguoiDuyet = model.NguoiDuyet;
                obj.GhiChu = model.GhiChu;
                SqlParameter pSessionId = new SqlParameter("SessionId", sessionid.Value);
                SqlParameter pMaPhieuChi = new SqlParameter("maPhieuChi", model.MaPhieuChi);
                //neu la loại chi tổng hợp mà bị thay đổi lý do chi thì phải xóa các thanh toán cọc đã sử dụng của phiếu này
                if (model.MaLoaiPhieu == "CTOHO")
                {
                    if (obj.LyDoChi != model.LyDoChi)
                    {
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuChiSuDungCoc", pSessionId, pMaPhieuChi);
                    }
                }
                //neu la loại chi tổng hợp mà bị thay đổi lý do chi thì phải xóa các thanh toán cọc đã sử dụng của phiếu này
                if (model.MaLoaiPhieu == "CMPTU"
                    || model.MaLoaiPhieu == "CMUTS"
                    || model.MaLoaiPhieu == "CMXTT"
                    || model.MaLoaiPhieu == "NGCNG"
                    )
                {
                    if (obj.DoiTac != model.DoiTac)
                    {
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuChiSuDungCoc", pSessionId, pMaPhieuChi);
                    }
                }

                //var lst = httt.Select(x => x.Id);
                //var notexist = _ctptRepository.Table.Where(x => lst.Contains(x.Id)).ToList();
                foreach (var item in httt)
                {
                    var data = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id && x.IsDeleted == false);
                    if (!item.IsDeleted && !string.IsNullOrEmpty(item.Temp))
                    {
                        bool flag = true;
                        if (data == null)
                        {
                            data = new ChiTietPhieuChi();
                            data.Id = Guid.NewGuid();
                            data.IsDeleted = false;
                            data.IsActive = true;
                            data.CreatedBy = uid;
                            data.CreatedDate = d;
                            flag = false;
                        }
                        var a = item.Temp.Split('-');
                        data.MaPhieuChi = obj.MaPhieuChi;
                        data.HinhThucThanhToan = a[1];
                        data.MaNganHang = a[0];
                        data.SoThamChieu = item.SoThamChieu;
                        data.TinhTrang = item.TinhTrang;
                        data.SoTienThanhToan = item.SoTienThanhToan;
                        data.UpdatedBy = uid;
                        data.UpdatedDate = d;
                        if (!flag)
                        {
                            _ctptRepository.Insert(data);
                        }
                        else
                        {
                            _ctptRepository.Update(data);
                        }

                        obj.SoTienChi += data.SoTienThanhToan;
                    }
                    else
                    {
                        if (data != null)
                        {
                            data.IsDeleted = true;
                            data.UpdatedDate = d;
                            data.UpdatedBy = uid;
                            _ctptRepository.Update(data);
                        }
                    }
                }
                foreach (var itemCTNKT in ListCTNKPT)
                {
                    if (itemCTNKT.Id != Guid.Empty)
                    {
                        itemCTNKT.NgayNhapKho = model.NgayNhapKho;
                        _ctnkptService.UpdateCTNKPT(itemCTNKT);
                    }
                    else
                    {
                        itemCTNKT.NgayNhapKho = model.NgayNhapKho;
                        itemCTNKT.MaPhieuChi = model.MaPhieuChi;
                        _ctnkptService.InsertCTNKPT(itemCTNKT);
                    }
                }
                //phan update CTPC
                if (ChiTietCoc != null)
                {
                    if (obj.LyDoChi != model.LyDoChi || obj.DoiTac != model.DoiTac)
                    {
                        foreach (var item_ctpc in ChiTietCoc)
                        {
                            item_ctpc.UpdatedBy = uid;
                            item_ctpc.UpdatedDate = d;
                            item_ctpc.IsActive = true;
                            item_ctpc.IsDeleted = false;
                            item_ctpc.CreatedBy = uid;
                            item_ctpc.CreatedDate = d;
                            item_ctpc.Id = Guid.NewGuid();
                            item_ctpc.MaPhieuChi = model.MaPhieuChi;
                            item_ctpc.TinhTrang = false;
                            item_ctpc.HinhThucThanhToan = "DC";
                            _ctptRepository.Insert(item_ctpc);
                            obj.SoTienChi += item_ctpc.SoTienThanhToan;
                        }
                    }
                    else
                    {
                        foreach (var item_ctpc in ChiTietCoc)
                        {
                            PhieuChi maPhieuChiCoc = _PhieuChiRepository.Table.Where(x => x.MaPhieuChi == item_ctpc.SoThamChieu && x.IsDeleted == false).FirstOrDefault();
                            var data_CTPC = _ctptRepository.Table.FirstOrDefault(x => x.MaPhieuChi == model.MaPhieuChi && x.IsDeleted == false && x.SoThamChieu == maPhieuChiCoc.MaPhieuChi);
                            if (data_CTPC != null)
                            {
                                data_CTPC.UpdatedBy = uid;
                                data_CTPC.UpdatedDate = d;
                                data_CTPC.SoTienThanhToan = item_ctpc.SoTienThanhToan;
                                _ctptRepository.Update(data_CTPC);
                                var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_ctpc.SoThamChieu);
                                if (data_PC != null)
                                {
                                    data_PC.SoTienChi = item_ctpc.SoTienThanhToan;
                                    data_PC.UpdatedBy = uid;
                                    data_PC.UpdatedDate = d;
                                    _PhieuChiRepository.Update(data_PC);
                                }
                                obj.SoTienChi += data_CTPC.SoTienThanhToan;
                            }
                            else
                            {
                                data_CTPC = new ChiTietPhieuChi();
                                data_CTPC.CreatedBy = uid;
                                data_CTPC.CreatedDate = d;
                                data_CTPC.SoTienThanhToan = item_ctpc.SoTienThanhToan;
                                data_CTPC.Id = Guid.NewGuid();
                                data_CTPC.HinhThucThanhToan = "DC";
                                data_CTPC.UpdatedBy = uid;
                                data_CTPC.UpdatedDate = d;
                                data_CTPC.MaPhieuChi = model.MaPhieuChi;
                                data_CTPC.SoThamChieu = maPhieuChiCoc.MaPhieuChi;
                                _ctptRepository.Insert(data_CTPC);
                                var data_PC = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == item_ctpc.SoThamChieu);
                                if (data_PC != null)
                                {
                                    data_PC.SoTienChi = item_ctpc.SoTienThanhToan;
                                    data_PC.UpdatedBy = uid;
                                    data_PC.UpdatedDate = d;
                                    _PhieuChiRepository.Update(data_PC);
                                }
                                obj.SoTienChi += data_CTPC.SoTienThanhToan;
                            }
                        }
                    }
                }
                SetValue(model, ref obj,uid);
                if (model.TongCong == 0)
                    obj.TongCong = obj.SoTienChi;
                obj.ConLai = obj.TongCong - obj.SoTienChi;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        obj.ConLai = model.TongCong;
                        break;
                }
                _PhieuChiRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuChi, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuChi", "Mã phiếu chi :" + obj.MaPhieuChi + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiUpdatePhieuChi)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý sau khi update
                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);

                    SqlParameter Message = new SqlParameter("message", message.Value != null ? message.Value.ToString() : "");//xử lý sau khi update
                    Message.Direction = System.Data.ParameterDirection.InputOutput;
                    SqlParameter Action = new SqlParameter("Action", "UPDATE");//xử lý sau khi update
                    Message.Size = 4000;
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    Error.Size = 4000;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuChi", MPT2, SessionId2, Action, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Message, Error);
                    string val = Message.Value != null ? Message.Value.ToString() : "";
                    return Error.Value.ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                _log.WriteLog("SoChiService.UpdatePhieuChi: ", e);
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.UpdatePhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.UpdatePhieuChi: " + e.Message.ToString());
                return "ERROR:" + e.ToString();
            }
        }

        public string DeletePhieuChi(string id, bool isGoiXuLySauKhiUpdatePhieuChi = true)
        {
            try
            {
                var obj = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == id && x.IsDeleted == false);
                if (obj == null)
                    return "Phiếu không tồn tại hoặc đã bị xóa";
                SqlParameter MPT1 = new SqlParameter("MaPhieuChi", obj.MaPhieuChi);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update

                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiDeletePhieuChi", MPT1, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                string MaLoaiPhieu = id.Substring(0, 5);
                if (MaLoaiPhieu == "NMPTU" || MaLoaiPhieu == "N2THK")
                {
                    string MPLQ = "";
                    PhieuChi PC1 = _PhieuChiRepository.Table.Where(x => x.MaPhieuChi == id).FirstOrDefault();
                    DateTime CreatePC1 = PC1.CreatedDate.Value;
                    List<PhieuChi> listPC = _PhieuChiRepository.Table.Where(x => x.MaPhieuLienQuan.Contains(PC1.MaPhieuLienQuan) == true && x.CreatedDate > PC1.CreatedDate && x.IsDeleted == false).ToList();
                    foreach (var item in listPC)
                    {
                        MPLQ += item.MaPhieuLienQuan;
                    }
                    if (listPC.Count > 0)
                        return "Đã có phiếu chi nợ khác tạo sau phiếu chi nợ này. Các phiếu liên quan: " + MPLQ;
                }
                obj.IsDeleted = true;
                obj.IsActive = false;
                obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedDate = DateTime.Now;
                _PhieuChiRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuChi, obj.MaLoaiPhieu, "DELETE", "DeletePhieuChi", "Mã phiếu chi :" + obj.MaPhieuChi + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiUpdatePhieuChi)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieu", obj.MaPhieuChi);//xử lý sau khi update
                                                                                    //  _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiDeletePhieuChi", MPT2);

                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", "");
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                    SqlParameter Message = new SqlParameter("message", message.Value != null ? message.Value.ToString() : "");//xử lý sau khi update
                    Message.Direction = System.Data.ParameterDirection.InputOutput;
                    SqlParameter Action = new SqlParameter("Action", "DELETE");//xử lý sau khi update
                    Message.Size = 4000;
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    Error.Size = 4000;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuChi", MPT2, SessionId2, Action, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Message, Error);
                    string val = Message.Value != null ? Message.Value.ToString() : "";
                    return Error.Value.ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                //if(e.InnerException!=null)
                //    _log.WriteLog("SoChiService.DeletePhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.DeletePhieuChi: " + e.Message.ToString());
                _log.WriteLog("SoChiService.DeletePhieuChi: ", e);
                return "ERROR:" + e.ToString();
            }
        }
        public string DeletePhieuThu(string id, bool isGoiXuLySauKhiUdatePhieuThu = true)
        {
            try
            {
                var obj = _PhieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == id && x.IsDeleted == false);
                if (obj == null)
                    return "Phiếu không tồn tại hoặc đã bị xóa";
                SqlParameter MPT1 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update

                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiDeletePhieuThu", MPT1, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                obj.IsDeleted = true;
                obj.IsActive = false;
                obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedDate = DateTime.Now;
                _PhieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "DELETE", "DeletePhieuChi", "Mã phiếu chi :" + obj.MaPhieuThu + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiUdatePhieuThu)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieu", obj.MaPhieuThu);//xử lý sau khi update
                                                                                    //  _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiDeletePhieuChi", MPT2);

                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", "");
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                    SqlParameter Message = new SqlParameter("message", message.Value != null ? message.Value.ToString() : "");//xử lý sau khi update
                    Message.Direction = System.Data.ParameterDirection.InputOutput;
                    SqlParameter Action = new SqlParameter("Action", "DELETE");//xử lý sau khi update
                    Message.Size = 4000;
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    Error.Size = 4000;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2, SessionId2, Action, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Error);
                    string val = Message.Value != null ? Message.Value.ToString() : "";
                    return Error.Value.ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                //if (e.InnerException != null)
                //    _log.WriteLog("SoChiService.DeletePhieuThu: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.DeletePhieuThu: " + e.Message.ToString());
                _log.WriteLog("SoChiService.DeletePhieuThu: ", e);
                return "ERROR:" + e.ToString();
            }
        }
        public PhieuChi GetPhieuChi(string id)
        {
            try
            {
                var list = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == id && x.IsDeleted == false);
                return list;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    _log.WriteLog("SoChiService.GetPhieuChi:(" + id + ") " + ex.InnerException.ToString());
                else
                    _log.WriteLog("SoChiService.GetPhieuChi:(" + id + ") " + ex.Message.ToString());
                return null;
            }

        }
        public List<ChiTietPhieuChi> GetCTPTByBienSo(string bienso)
        {
            try
            {
                SqlParameter bs = new SqlParameter("bienso", bienso);
                return _dbContext.ExecuteStoredProcedureList<ChiTietPhieuChi>("GetCTPTByBienSo", bs).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException!=null)
                //    _log.WriteLog("SoChiService.GetCTPTByBienSo:(" + bienso + ") " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetCTPTByBienSo:(" + bienso + ") " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetCTPTByBienSo:(" + bienso + ") ", ex);
                return null;
            }

        }
        public List<ChiTietPhieuChi> GetCTPT(string id)
        {
            try
            {
                return _ctptRepository.Table.Where(x => x.MaPhieuChi == id && x.IsDeleted == false && x.IsActive == true).ToList().Select(x => new ChiTietPhieuChi
                {
                    Id = x.Id,
                    TinhTrang = x.TinhTrang,
                    Temp = x.MaNganHang + "-" + x.HinhThucThanhToan,
                    HinhThucThanhToan = x.HinhThucThanhToan,
                    MaPhieuChi = x.MaPhieuChi,
                    MaNganHang = x.MaNganHang,
                    SoThamChieu = x.SoThamChieu,
                    SoTienThanhToan = x.SoTienThanhToan
                }).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetCTPT:(" + id + ") " + ex.Message.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetCTPT:(" + id + ") " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetCTPT:(" + id + ") ", ex);
                return null;
            }

        }
        public bool UpdateCTPT(ChiTietPhieuChi model)
        {
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var obj = _ctptRepository.Table.FirstOrDefault(x => x.Id == model.Id);
                obj.MaNganHang = model.MaNganHang;
                obj.SoTienThanhToan = model.SoTienThanhToan;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                _ctptRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuChi, obj.MaNganHang, "UPDATE", "UpdateCTPT", "Mã phiếu chi :" + obj.MaPhieuChi + " mã loại phiếu :" + obj.MaNganHang + " Tổng cộng :" + obj.SoTienThanhToan, _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("UpdateCTPT: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("UpdateCTPT: " + e.Message.ToString());
                _log.WriteLog("UpdateCTPT: ", e);
                return false;
            }
        }

        public bool UpdateCTPT(List<ChiTietPhieuChi> lst)
        {
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                foreach (var item in lst)
                {
                    var obj = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id);
                    if (obj != null)
                    {
                        obj.SoTienThanhToan = item.SoTienThanhToan;
                        obj.UpdatedBy = uid;
                        obj.UpdatedDate = d;
                        _ctptRepository.Update(obj);
                        _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuChi, obj.MaNganHang, "UPDATE", "UpdateCTPT", "Mã phiếu chi :" + obj.MaPhieuChi + " mã loại phiếu :" + obj.MaNganHang + " Tổng cộng :" + obj.SoTienThanhToan, _authenticationService.GetAuthenticatedUser().UserId);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("UpdateCTPT: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("UpdateCTPT: " + e.Message.ToString());
                _log.WriteLog("UpdateCTPT: ", e);
                return false;
            }
        }
        public string ImportNoGCN(string sSoQuyetToan, string sTenCongViec, double fSoTienNo)
        {
            try
            {
                SqlParameter soQuyetToan = new SqlParameter("SoQuyetToan",sSoQuyetToan);
                SqlParameter tenCongViec = new SqlParameter("TenCongViec", sTenCongViec);
                SqlParameter soTienNo = new SqlParameter("SoTienNo", fSoTienNo);
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 1000;
                message.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_ImportNoGCN",new SqlParameter[] { soQuyetToan,tenCongViec,soTienNo,message});
                return message.Value.ToString();
            }
            catch (Exception e)
            {
                _log.WriteLog("ImportNoGCN: ", e);
                return e.Message;
            }
        }

        public string ImportNoHHTXE(string sSoQuyetToan, double fSoTienNo,string sNguoiDuyet)
        {
            try
            {
                SqlParameter soQuyetToan = new SqlParameter("SoQuyetToan", sSoQuyetToan);
                SqlParameter soTienNo = new SqlParameter("SoTienNo", fSoTienNo);
                SqlParameter nguoiDuyet = new SqlParameter("NguoiDuyet", sNguoiDuyet);
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 1000;
                message.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_ImportNoHHTXE", new SqlParameter[] { soQuyetToan, soTienNo, nguoiDuyet, message });
                return message.Value.ToString();
            }
            catch (Exception e)
            {
                _log.WriteLog("ImportNoHHTXE: ", e);
                return e.Message;
            }
        }
        //public bool DeleteHTD(string MaPhieuChi)
        //{
        //    try
        //    {
        //        var obj = _PhieuChiRepository.Table.FirstOrDefault(x=>x.MaPhieuChi == MaPhieuChi && x.IsDeleted == false);
        //        if (obj == null)
        //            return false;
        //        var uid = _authenticationService.GetAuthenticatedUser().UserId;
        //        obj.IsDeleted = true;
        //        obj.UpdatedBy = uid;
        //        obj.UpdatedDate = DateTime.Now;
        //        _PhieuChiRepository.Update(obj);
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.WriteLog("SoChiService.DeleteHTD:("+ MaPhieuChi +") " + ex.Message.ToString());
        //        return false;
        //    }
        //}
        public bool UpdateNoPhaiTra(NoPhaiTra info)
        {

            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var obj = _noPhaiTraRepository.Table.FirstOrDefault(x => x.MaPhieuNo == info.MaPhieuNo);
                if (obj != null)
                {
                    obj.SoTienNo = info.SoTienNo;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    _noPhaiTraRepository.Update(obj);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuChi, obj.MaPhieuNo, "UPDATE", "UpdateNoPhaiTra", "Mã phiếu chi :" + obj.MaPhieuChi + " mã phiếu nợ :" + obj.MaPhieuNo + " Tổng cộng :" + obj.SoTienConLai, _authenticationService.GetAuthenticatedUser().UserId);
                }
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.UpdateNoPhaiTra: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.UpdateNoPhaiTra: " + e.Message.ToString());
                _log.WriteLog("SoChiService.UpdateNoPhaiTra: ", e);
                return false;
            }
        }

        public bool InsertNoPhaiTra(NoPhaiTra info, string id)
        {
            try
            {
                SqlParameter MPN = new SqlParameter("MaPhieuNo", System.Data.SqlDbType.NVarChar);
                MPN.Size = 15;
                MPN.Direction = System.Data.ParameterDirection.Output;
                SqlParameter LPN = new SqlParameter("LoaiPhieuNo", "HHTXE");
                SqlParameter MPPS = new SqlParameter("MaPhieuPhatSinh", id);
                SqlParameter TTDT = new SqlParameter("ThongTinDoiTac", "");
                SqlParameter STN = new SqlParameter("SoTienNo", info.SoTienNo);
                _dbContext.ExecuteStoredProcedure("sp_InsertNoPhaiTra", MPN, LPN, MPPS, TTDT, STN);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, info.MaPhieuChi, info.MaPhieuNo, "INSERT", "InsertNoPhaiTra", "Mã phiếu chi :" + info.MaPhieuChi + " mã phiếu nợ :" + info.MaPhieuNo + " Tổng cộng :" + info.SoTienConLai, _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.InsertNoPhaiTra: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.InsertNoPhaiTra: " + e.Message.ToString());
                _log.WriteLog("SoChiService.InsertNoPhaiTra: ", e);
                return false;
            }
        }


        public bool UpdateNoPhaiThu(NoPhaiThu model)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                var obj = _noPhaiThuRepository.Table.FirstOrDefault(x => x.MaPhieuNo == model.MaPhieuNo);
                if (obj != null)
                {
                    obj.SoTienNo = model.SoTienNo;
                    obj.NguoiBaoLanh = model.NguoiBaoLanh;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    _noPhaiThuRepository.Update(obj);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaPhieuNo, "UPDATE", "UpdateNoPhaiThu", "Mã phiếu chi :" + obj.MaPhieuThu + " mã phiếu nợ :" + obj.MaPhieuNo + " Tổng cộng :" + obj.SoTienConLai, _authenticationService.GetAuthenticatedUser().UserId);
                }
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.UpdateNoPhaiThus: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.UpdateNoPhaiThus: " + e.Message.ToString());
                _log.WriteLog("SoChiService.UpdateNoPhaiThus: ", e);
                return false;
            }
        }
        public bool InsertNoPhaiThu(string loaiphieu, string maPhieuChi, string nguoibaolanh, double sotienno)
        {
            try
            {
                SqlParameter MPN = new SqlParameter("MaPhieuNo", System.Data.SqlDbType.NVarChar);
                MPN.Size = 15;
                MPN.Direction = System.Data.ParameterDirection.Output;
                SqlParameter LPN = new SqlParameter("LoaiPhieuNo", loaiphieu);
                SqlParameter MPPS = new SqlParameter("MaPhieuPhatSinh", maPhieuChi);
                SqlParameter TTDT = new SqlParameter("NguoiBaoLanh", nguoibaolanh);
                SqlParameter STN = new SqlParameter("SoTienNo", sotienno);
                _dbContext.ExecuteStoredProcedure("sp_InsertNoPhaiThu", MPN, LPN, MPPS, TTDT, STN);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, maPhieuChi, loaiphieu, "INSERT", "InsertNoPhaiThu", "Mã phiếu chi :" + maPhieuChi + " mã phiếu nợ :" + loaiphieu + " người bảo lãnh :" + nguoibaolanh, _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.InsertNoPhaiThu: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.InsertNoPhaiThu: " + e.Message.ToString());
                _log.WriteLog("SoChiService.InsertNoPhaiThu: ", e);
                return false;
            }
        }

        public List<NoPhaiThu> GetNoPhaiThu(string id)
        {
            try
            {
                return _noPhaiThuRepository.Table.Where(x => x.MaPhieuPhatSinh == id && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNoPhaiThu: " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNoPhaiThu: " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNoPhaiThu: ", ex);
                return null;
            }

        }

        public List<NoPhaiTra> GetNoPhaiTra(string id)
        {
            try
            {
                return _noPhaiTraRepository.Table.Where(x => x.MaPhieuPhatSinh == id && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNoPhaiTra:(" + id + ") " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNoPhaiTra:("+ id +") " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNoPhaiTra:(" + id + ") ", ex);
                return null;
            }
        }
        public List<ViewPhieuChi> ListThuHoanCoc(string loaiPhieu, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string maloaiphieu)
        {
            try
            {
                //string[] list = { "CCOMX", "CDACO", "CCOPT", "CCOGC", "CCOTH" };
                var q = _ViewPhieuChiRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to && x.IsDeleted == false);
                if (maloaiphieu == "")
                    q = q.Where(x => x.MaLoaiPhieu == "CCOMX" || x.MaLoaiPhieu == "CDACO" || x.MaLoaiPhieu == "CCOPT" || x.MaLoaiPhieu == "CCOGC" || x.MaLoaiPhieu == "CCOTH");
                else
                    q = q.Where(x => x.MaLoaiPhieu == maloaiphieu);
                if (!string.IsNullOrEmpty(loaiPhieu))
                    q = q.Where(x => x.MaLoaiPhieu == loaiPhieu);
                if (!string.IsNullOrEmpty(query))
                    q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                    || x.MaPhieuChi.ToLower().Contains(query)
                    || x.TenNhaCungCap.ToLower().Contains(query)
                    || query.Contains(x.SoChungTu.ToLower())
                    || query.Contains(x.TenNhaCungCap.ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                     || query.Contains(x.MaPhieuChi.ToLower())
                     ));
                var result = q.OrderByDescending(x => x.CreatedDate).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.ListThuHoanCoc:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.ListThuHoanCoc:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                _log.WriteLog("SoChiService.ListThuHoanCoc:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") ", ex);
                return null;
            }
        }
        public List<PhieuThu> ListThuHoanCocComplete(string loaiPhieu, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                string[] list = { "CCOMX", "CDACO", "CCOPT", "CCOGC", "CCOTH" };
                var q = _PhieuThuRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.IsDeleted == false);
                q = q.Where(x => x.MaLoaiPhieu == "HCOMX" || x.MaLoaiPhieu == "HDACO" || x.MaLoaiPhieu == "HCOPT" || x.MaLoaiPhieu == "HCOGC" || x.MaLoaiPhieu == "HCOTH");
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)

                    || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception e)
            {
                //if (e.InnerException != null)
                //{
                //    _log.WriteLog("SoChiService.ListThuHoanCocComplete" + e.InnerException.ToString());
                //}
                //else
                //{
                //    _log.WriteLog("SoChiService.ListThuHoanCocComplete" + e.Message.ToString());
                //}
                _log.WriteLog("SoChiService.ListThuHoanCocComplete", e);
                return null;
            }
        }
        public List<ViewPhieuChi> GetPhieuChiList(string loaiPhieu, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _ViewPhieuChiRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to && x.IsDeleted == false);
                if (!string.IsNullOrEmpty(loaiPhieu))
                    q = q.Where(x => x.MaLoaiPhieu == loaiPhieu);
                if (loaiPhieu == "CMXTT")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                        || x.DoiTac.ToLower().Contains(query)
                        || x.SoKhung.ToLower().Contains(query)
                        || x.SoMay.ToLower().Contains(query)
                        || x.LoaiXe.ToLower().Contains(query))


                        || query.Contains(x.SoChungTu.ToLower())
                        || query.Contains(x.DoiTac.ToLower())
                        || query.Contains(x.SoKhung.ToLower())
                        || query.Contains(x.SoMay.ToLower())
                        || query.Contains(x.LoaiXe.ToLower())
                         || query.Contains(x.MaPhieuChi.ToLower())

                        );
                }
                else if (loaiPhieu == "CTOHO")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.Contains(query)
                        || x.HoTen.Contains(query)
                        || x.LyDoChi.Contains(query)
                        || x.NoiDung.Contains(query)

                        || query.Contains(x.SoChungTu.ToLower())
                        || query.Contains(x.LyDoChi.ToLower())
                        || query.Contains(x.NoiDung.ToLower())

                        ));
                }
                else if (loaiPhieu == "CVAMU")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.Contains(query)
                        || x.HoTen.Contains(query)
                        || x.DienThoai.Contains(query)
                        || x.TinhTrangPhieu.Contains(query))
                        || query.Contains(x.SoChungTu.ToLower())

                        );
                }
                else if (loaiPhieu == "CMUTS")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                        || x.TenTaiSan.ToLower().Contains(query)
                        || x.TenNhaCungCap.ToLower().Contains(query))
                        || query.Contains(x.SoChungTu.ToLower())

                        );
                }
                else if (loaiPhieu == "CDATU")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                        || x.DuAn.ToLower().Contains(query)
                        || x.TenDoiTac.ToLower().Contains(query))

                        || query.Contains(x.SoChungTu.ToLower())
                        );
                }

                else
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.MaPhieuChi.Contains(query)
                        || x.HoTen.Contains(query)
                        || x.SoHopDong.Contains(query)
                        || x.NhaCungCap.ToLower().Contains(query)
                        || x.TenDoiTac.ToLower().Contains(query)
                        || x.TenNhaCungCap.ToLower().Contains(query)
                        || x.SoChungTu.Contains(query)
                        || x.SoKhung.Contains(query))

                        || query.Contains(x.SoChungTu.ToLower())
                        || query.Contains(x.MaPhieuChi.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        );
                }


                var result = q.OrderByDescending(x => x.CreatedDate).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetPhieuChiList:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetPhieuChiList:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetPhieuChiList:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") ", ex);
                return null;
            }
        }
        public List<ViewPhieuChi> GetPhieuChiList(string loaiPhieu, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang)
        {
            try
            {
                var q = _ViewPhieuChiRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to && x.IsDeleted == false);
                if (!string.IsNullOrEmpty(loaiPhieu))
                    q = q.Where(x => x.MaLoaiPhieu == loaiPhieu);
                if (loaiPhieu == "CMXTT")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                        || x.DoiTac.ToLower().Contains(query)
                        || x.SoKhung.ToLower().Contains(query)
                        || x.SoMay.ToLower().Contains(query)
                        || x.LoaiXe.ToLower().Contains(query))

                         || query.Contains(x.SoChungTu.ToLower())
                        || query.Contains(x.MaPhieuChi.ToLower())
                        || query.Contains(x.DoiTac.ToLower())
                        || query.Contains(x.SoKhung.ToLower())
                        || query.Contains(x.SoMay.ToLower())
                        || query.Contains(x.LoaiXe.ToLower())

                        );
                }
                else if (loaiPhieu == "CTOHO")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.Contains(query)
                        || x.HoTen.Contains(query)
                        || x.LyDoChi.Contains(query))

                        || query.Contains(x.SoChungTu.ToLower())
                        || query.Contains(x.MaPhieuChi.ToLower())
                        || query.Contains(x.LyDoChi.ToLower())

                        );
                }
                else if (loaiPhieu == "CVAMU")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.Contains(query)
                        || x.HoTen.Contains(query)
                        || x.DienThoai.Contains(query)
                        || x.TinhTrangPhieu.Contains(query))

                        || query.Contains(x.SoChungTu.ToLower())

                        );
                }
                else if (loaiPhieu == "CMUTS")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                        || x.TenTaiSan.ToLower().Contains(query)
                        || x.TenNhaCungCap.ToLower().Contains(query))

                        || query.Contains(x.SoChungTu.ToLower())
                        );
                }
                else if (loaiPhieu == "CDATU")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                        || x.DuAn.ToLower().Contains(query)
                        || x.TenDoiTac.ToLower().Contains(query))

                        || query.Contains(x.SoChungTu.ToLower())
                        || query.Contains(x.MaPhieuChi.ToLower())

                        );
                }
                else if (loaiPhieu == "CCOMX" || loaiPhieu == "CCOPT")
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                        || x.TenNhaCungCap.ToLower().Contains(query))

                        || query.Contains(x.SoChungTu.ToLower())

                        );
                }
                else
                {
                    if (!string.IsNullOrEmpty(query))
                        q = q.Where(x => (x.MaPhieuChi.Contains(query)
                        || x.HoTen.Contains(query)
                        || x.SoHopDong.Contains(query)
                        || x.SoChungTu.Contains(query))

                        || query.Contains(x.SoChungTu.ToLower())
                        || query.Contains(x.MaPhieuChi.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        );
                }
                if (tinhtrang == "H")
                {
                    q = q.Where(x => x.ConLaiPhaiTra <= 0);
                }
                else if (tinhtrang == "C")
                {
                    q = q.Where(x => x.ConLaiPhaiTra > 0);
                }
                var result = q.OrderByDescending(x => x.NgayChi).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetPhieuChiList:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetPhieuChiList:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetPhieuChiList:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") ", ex);
                return null;
            }
        }

        public List<ViewListPhieuChi> GetListPhieuChi(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string maLoaiPhieu, string chonngay)
        {
            try
            {
                query = query.ToLower();
                if (chonngay == "NC")
                {
                    var list = _ViewListPhieuChiRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to);
                    if (maLoaiPhieu != "")
                        list = list.Where(x => x.MaLoaiPhieu == maLoaiPhieu);
                    if (!string.IsNullOrEmpty(query))
                    {
                        list = list.Where(x => x.Hoten.ToLower().Contains(query.ToLower())
                        || x.MaPhieuChi.ToLower().Contains(query.ToLower())
                        || x.SoHopDong.ToLower().Contains(query.ToLower())
                        || x.MaLoaiPhieu.ToLower().Contains(query.ToLower())
                        || x.Hoten.ToLower().Contains(query.ToLower())
                        || x.HoTenKH.ToLower().Contains(query.ToLower())
                        || x.DoiTac.ToLower().Contains(query.ToLower())
                        || x.MaLoaiPhieu.ToLower().Contains(query.ToLower())

                                || query.Contains(x.SoChungTu.ToLower())
                                || query.Contains(x.MaPhieuChi.ToLower())
                                || query.Contains(x.SoHopDong.ToLower())
                        );
                    }

                    var result = list.OrderBy(x => x.MaLoaiPhieu).ToList();
                    total = result.Count;
                    return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    var list = _ViewListPhieuChiRepository.Table.Where(x => x.NgayHachToan >= from && x.NgayHachToan <= to);
                    if (maLoaiPhieu != "")
                        list = list.Where(x => x.MaLoaiPhieu == maLoaiPhieu);
                    if (!string.IsNullOrEmpty(query))
                    {
                        list = list.Where(x => x.Hoten.ToLower().Contains(query.ToLower())
                        || x.MaPhieuChi.ToLower().Contains(query.ToLower())
                        || x.SoHopDong.ToLower().Contains(query.ToLower())
                        || x.MaLoaiPhieu.ToLower().Contains(query.ToLower())

                                || query.Contains(x.SoChungTu.ToLower())
                                || query.Contains(x.MaPhieuChi.ToLower())
                                || query.Contains(x.SoHopDong.ToLower())
                        );
                    }

                    var result = list.OrderBy(x => x.MaLoaiPhieu).ToList();
                    total = result.Count;
                    return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetListPhieuChi:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetListPhieuChi:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetListPhieuChi:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") ", ex);
                return null;
            }

        }

        #region Chi mua phu tung (CMPTU)

        public List<ChiTietNhapKhoPhuTung> GetCTNKPT(string id)
        {
            try
            {
                return _ctnkptRepository.Table.Where(x => x.MaPhieuChi == id && x.IsDeleted == false && x.IsActive == true).ToList().Select(x => new ChiTietNhapKhoPhuTung
                {
                    Id = x.Id,
                    MaKho = x.MaKho,
                    MaPhieuChi = x.MaPhieuChi,
                    NgayNhapKho = x.NgayNhapKho,
                    SoTien = x.SoTien,
                    GhiChu = x.GhiChu
                }).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetCTNKPT:(" + id + ")" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetCTNKPT:(" + id + ")" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetCTNKPT:(" + id + ")", ex);
                return null;
            }

        }
        public bool UpdateCTNKPT(ChiTietNhapKhoPhuTung model)
        {
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var obj = _ctnkptRepository.Table.FirstOrDefault(x => x.Id == model.Id);
                obj.MaKho = model.MaKho;
                obj.SoTien = model.SoTien;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                _ctnkptRepository.Update(obj);
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("UpdateCTPT: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("UpdateCTPT: " + e.Message.ToString());
                _log.WriteLog("UpdateCTPT: ", e);
                return false;
            }
        }

        public bool UpdateCTNKPT(List<ChiTietNhapKhoPhuTung> lst)
        {
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                foreach (var item in lst)
                {
                    var obj = _ctnkptRepository.Table.FirstOrDefault(x => x.Id == item.Id);
                    if (obj != null)
                    {
                        obj.SoTien = item.SoTien;
                        obj.UpdatedBy = uid;
                        obj.UpdatedDate = d;
                        _ctnkptRepository.Update(obj);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("UpdateCTPT: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("UpdateCTPT: " + e.Message.ToString());
                _log.WriteLog("UpdateCTPT: ", e);
                return false;
            }
        }
        public bool InsertCTNKPT(ChiTietNhapKhoPhuTung info)
        {
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                info.Id = Guid.NewGuid();
                info.UpdatedBy = uid;
                info.UpdatedDate = d;
                info.CreatedBy = uid;
                info.CreatedDate = d;
                info.IsDeleted = false;
                info.IsActive = true;
                _ctnkptRepository.Insert(info);
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("InsertCTNKPT: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("InsertCTNKPT: " + e.Message.ToString());
                _log.WriteLog("InsertCTNKPT: ", e);
                return false;
            }
        }
        #endregion Chi mua phu tung

        public List<ViewChiCoc> GetChiCoc()
        {
            try
            {
                var q = _viewChiCocRepository.Table.Where(x => x.MaPhieuChi != "");
                List<ViewChiCoc> list = q.OrderByDescending(x => x.NgayChi).ToList();
                return list;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetChiCoc:" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetChiCoc:" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetChiCoc:", ex);
                return null;
            }
        }
        //mua xe thucte
        public List<PhieuChi> GetChiCocMXTT(string maPhieuChi, string NCC)
        {
            try
            {
                var q = _PhieuChiRepository.Table.Where(x => x.DoiTac == NCC && x.MaLoaiPhieu == "CCOMX" && x.IsDeleted == false);
                List<PhieuChi> list = q.OrderByDescending(x => x.NgayChi).ToList();
                foreach (var temp in list)
                {
                    List<ChiTietPhieuChi> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuChi == maPhieuChi
                    && x.HinhThucThanhToan == "DC"
                    && x.SoThamChieu == temp.MaPhieuChi
                    && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai.Value + TongSoTien;
                    temp.SoTienChi = TongSoTien;
                }
                return list;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetChiCocMXTT:" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetChiCocMXTT:" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetChiCocMXTT:", ex);
                return null;
            }
        }
        //mua phu tung
        public List<PhieuChi> GetChiCocMPTU(string maPhieuChi, string NCC)
        {
            try
            {
                var q = _PhieuChiRepository.Table.Where(x => x.DoiTac == NCC && x.MaLoaiPhieu == "CCOPT" && x.IsDeleted == false);
                List<PhieuChi> list = q.OrderByDescending(x => x.NgayChi).ToList();
                foreach (var temp in list)
                {
                    List<ChiTietPhieuChi> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuChi == maPhieuChi
                    && x.HinhThucThanhToan == "DC"
                    && x.SoThamChieu == temp.MaPhieuChi
                    && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai.Value + TongSoTien;
                    temp.SoTienChi = TongSoTien;
                }
                return list;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetChiCocMPTU:" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetChiCocMPTU:" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetChiCocMPTU:", ex);
                return null;
            }
        }
        // mua tai san
        public List<PhieuChi> GetChiCocMUTS(string maPhieuChi, string NCC)
        {
            try
            {
                var q = _PhieuChiRepository.Table.Where(x => x.DoiTac == NCC && x.MaLoaiPhieu == "CDACO" && x.IsDeleted == false);
                List<PhieuChi> list = q.OrderByDescending(x => x.NgayChi).ToList();
                foreach (var temp in list)
                {
                    List<ChiTietPhieuChi> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuChi == maPhieuChi
                    && x.HinhThucThanhToan == "DC"
                    && x.SoThamChieu == temp.MaPhieuChi
                    && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai.Value + TongSoTien;
                    temp.SoTienChi = TongSoTien;
                }
                return list;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetChiCocMUTS:" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetChiCocMUTS:" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetChiCocMUTS:", ex);
                return null;
            }
        }
        //
        public List<PhieuChi> GetChiCocGCN(string maPhieuChi, string NCC)
        {
            try
            {
                var q = _PhieuChiRepository.Table.Where(x => x.DoiTac == NCC && x.MaLoaiPhieu == "CCOGC" && x.IsDeleted == false);
                List<PhieuChi> list = q.OrderByDescending(x => x.NgayChi).ToList();
                foreach (var temp in list)
                {
                    List<ChiTietPhieuChi> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuChi == maPhieuChi
                    && x.HinhThucThanhToan == "DC"
                    && x.SoThamChieu == temp.MaPhieuChi
                    && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai.Value + TongSoTien;
                    temp.SoTienChi = TongSoTien;
                }
                return list;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetChiCocGCN:" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetChiCocGCN:" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetChiCocGCN:", ex);
                return null;
            }
        }
        //coc tong hop
        public List<PhieuChi> GetChiCocTH(string maPhieuChi, string LDC)
        {
            try
            {
                var q = _PhieuChiRepository.Table.Where(x => x.LyDoChi == LDC && x.MaLoaiPhieu == "CCOTH" && x.IsDeleted == false);
                List<PhieuChi> list = q.OrderByDescending(x => x.NgayChi).ToList();
                foreach (var temp in list)
                {
                    List<ChiTietPhieuChi> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuChi == maPhieuChi
                    && x.HinhThucThanhToan == "DC"
                    && x.SoThamChieu == temp.MaPhieuChi
                    && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai.Value + TongSoTien;
                    temp.SoTienChi = TongSoTien;

                }
                return list;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetChiCocTH:" + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetChiCocTH:" + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetChiCocTH:", ex);
                return null;
            }
        }
        #region phần trúc thêm
        public List<TNK.Core.Domain.NhanVien> GetNhanVien()
        {
            try
            {
                return _NhanVienRepository.Table.Where(x => x.HoTen != null && x.IsActive == true && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetNhanVien: " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetNhanVien: " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetNhanVien: ", ex);
                return null;
            }

        }
        #endregion


        public List<PhieuChi> GetListPhieuChi(DateTime from, DateTime to, int p, ref int total, int pageSize, string query, string maLoaiPhieu)
        {
            query = query.ToLower();
            var list = new List<PhieuChi>();
            if (maLoaiPhieu == "")
                list = _PhieuChiRepository.Table.Where(x => x.SoHopDong != null && x.IsDeleted == false && x.NgayChi >= from && x.NgayChi <= to).ToList();
            else if (maLoaiPhieu != "")
            {
                list = _PhieuChiRepository.Table.Where(x => x.SoHopDong != null && x.IsDeleted == false && x.NgayChi >= from && x.NgayChi <= to && x.MaLoaiPhieu == maLoaiPhieu).ToList();
            }
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(x => x.SoHopDong.ToString().Contains(query)
                || x.MaLoaiPhieu.ToLower().Contains(query)
                || x.HoTen.ToLower().Contains(query)

                || query.Contains(x.SoChungTu.ToLower())
                || query.Contains(x.MaPhieuChi.ToLower())
                || query.Contains(x.SoHopDong.ToLower())
                ).ToList();
            }
            var result = list.OrderBy(x => x.MaLoaiPhieu).ToList();
            total = result.Count;
            return result.ToList();
        }
        //cap nhat hoa don chi
        public bool Update_SoHoaDon(string maPhieuChi, string soHoaDon)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var data = _PhieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == maPhieuChi && x.IsDeleted == false);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    data.SoHoaDon = soHoaDon;
                    data.UpdatedBy = uid;
                    data.UpdatedDate = DateTime.Now;
                    _PhieuChiRepository.Update(data);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, maPhieuChi, soHoaDon, "UPDATE", "Update_SoHoaDon", "Mã phiếu chi :" + maPhieuChi + " mã phiếu nợ :" + maPhieuChi + " số hóa đơn :" + soHoaDon, _authenticationService.GetAuthenticatedUser().UserId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.Update:", ex);
                return false;
            }
        }
        public DataTable XuatExcel(DateTime FromDate, DateTime ToDate, string MaLoaiPhieu)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter maloaiphieu = new SqlParameter("maloaiphieu", MaLoaiPhieu);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_PhieuChi", fromdate, todate, maloaiphieu);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PSoChiService.XuatExcel(" + FromDate + "," + ToDate + ",," + MaLoaiPhieu + "):", ex);
                return null;
            }
        }
        public List<DanhMucDienGiai> LoaiPhieuChi()
        {
            try
            {
                return _DanhMucDienGiaiRepository.Table.Where(x => x.TableSuDung == "PhieuChi" || x.TableSuDung == "NoPhaiTra" && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Ten).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.LoaiPhieuChi", ex);
                return null;
            }
        }
        public double TongSoTienChi(string MaPhieuChi)
        {
            List<ChiTietPhieuThu> list = _ChiTietPhieuThuRepository.Table.Where(x => x.SoThamChieu == MaPhieuChi && x.IsDeleted == false).ToList();
            double Tong = 0;
            foreach (var item in list)
            {
                Tong += item.SoTienThanhToan;
            }
            return Tong;
        }
        public ViewNoDaTra getNoDaTraById(string id)
        {
            return _VNDTRepository.Table.Where(x => x.SoPhieuQuyetToan == id).FirstOrDefault();
        }
        public string getPhieuTraNo(ViewNoDaTra VNDT)
        {
            List<ViewNoDaTra> list = _VNDTRepository.Table.Where(x => x.MaPhieuNo == VNDT.MaPhieuNo
            && x.NgayHachToan >= VNDT.NgayHachToan && x.SoTienConLai > VNDT.SoTienConLai).ToList();
            if (list.Count == 0)
                return "";
            else
                return "Tồn tại phiếu trả nợ sau phiếu này. Cần xóa phiếu đó trước";
        }
        public void CreateRoleInmenu(Guid id)
        {
            SqlParameter pid = new SqlParameter("RoleId", id);
            _dbContext.ExecuteStoredProcedure("sp_CreateRoleInMenu", pid);
        }

        public List<CategoryItem> GetDuAnDauTuKhauHau(string id)
        {
            try
            {
                return _dbContext.ExecuteStoredProcedureList<CategoryItem>("sp_SelectDuAnDauTuKhauHao").ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.GetDuAnDauTu : (" + id + ")", ex);
                return null;
            }

        }

        public bool CreatePhieuChiKHDT(PhieuChi model, List<CHI_TIET_KHAU_HAO> lstCTKH, Guid UserId)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuChi = CreateId(model.MaLoaiPhieu);
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                if (model.TongCong == 0)
                    model.TongCong = model.SoTienChi;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        model.ConLai = model.TongCong;
                        break;
                }
                _PhieuChiRepository.Insert(model);

                foreach (CHI_TIET_KHAU_HAO item in lstCTKH)
                {
                    if (item.SoTienKhauHao > 0)
                    {
                        item.MaId = Guid.NewGuid();
                        item.MaPhieuChiKhauHao = model.MaPhieuChi;
                        item.ConLaiSauKhauHao = item.SoTienConLai - item.SoTienKhauHao;
                        item.CreatedBy = UserId;
                        item.CreatedDate = DateTime.Now;
                        _ctkhRepository.Insert(item);
                    }
                }

                XuLySauKhiInsertPhieuChi(model.MaPhieuChi, "");
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.CreatePhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.CreatePhieuChi: " + e.Message.ToString());
                _log.WriteLog("SoChiService.CreatePhieuChi: ", e);
                return false;
            }
        }
        public bool CreatePhieuChiKHTS(PhieuChi model, List<ViewKhoTaiSan> TS, Guid UserID)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuChi = CreateId(model.MaLoaiPhieu);
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                if (model.TongCong == 0)
                    model.TongCong = model.SoTienChi;
                switch (model.MaLoaiPhieu)
                {
                    case "CVAMU":
                        model.ConLai = model.TongCong;
                        break;
                }
                _PhieuChiRepository.Insert(model);

                foreach (ViewKhoTaiSan item in TS)
                {
                    if (item.TienKhauHao > 0)
                    {
                        CHI_TIET_KHAU_HAO obj = new CHI_TIET_KHAU_HAO();
                        obj.MaId = Guid.NewGuid();
                        obj.MaPhieuChiKhauHao = model.MaPhieuChi;
                        obj.MaPhieuChiGoc = item.MaPhieuNhap;
                        obj.SoTienConLai = item.GiaTriConLai;
                        obj.SoTienKhauHao = item.TienKhauHao;
                        obj.ConLaiSauKhauHao = item.GiaTriConLai - item.TienKhauHao;
                        obj.CreatedDate = DateTime.Now;
                        obj.CreatedBy = UserID;
                        _ctkhRepository.Insert(obj);
                    }
                }

                XuLySauKhiInsertPhieuChi(model.MaPhieuChi, "");
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException != null)
                //    _log.WriteLog("SoChiService.CreatePhieuChi: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.CreatePhieuChi: " + e.Message.ToString());
                _log.WriteLog("SoChiService.CreatePhieuChi: ", e);
                return false;
            }
        }


        public string GetPhieuChiNo(string MaPhieuNo)
        {
            try
            {
                SqlParameter PrMaPhieuNo = new SqlParameter("maphieuno", MaPhieuNo);
                SqlParameter PrResult = new SqlParameter("result", "");
                PrResult.Direction = ParameterDirection.Output;
                PrResult.Size = 400;
                _dbContext.ExecuteStoredProcedure("sp_Get_PhieuChiNo", PrMaPhieuNo, PrResult);
                string result = PrResult.Value == null ? "" : PrResult.Value.ToString();
                return result;
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.GetPhieuChiNo", ex);
                return null;
            }
        }

        public List<ViewCKHHA> GetCKHDT(DateTime from, DateTime to, int p, ref int total, int pageSize, string query)
        {
            try
            {
                var list = _viewCKHHARepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to && x.MaLoaiPhieu == "CKHDT");
                var s = list.ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.MaPhieuChi.ToLower().Contains(query.ToLower())
                            || x.MaLoaiPhieu.ToLower().Contains(query.ToLower())
                            || query.Contains(x.SoChungTu.ToLower())
                            || query.Contains(x.MaPhieuChi.ToLower())
                    );
                }

                var result = list.OrderBy(x => x.MaLoaiPhieu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public double GetTongTienKhauHao(string type, string MaDuAn)
        {
            try
            {
                double TongCong = 0;
                if (type == "create")
                {
                    TongCong = _PhieuChiRepository.Table.Where(x => x.MaLoaiPhieu == "CDATU" && x.DuAn == MaDuAn && x.IsDeleted == false).Sum(x => (x.ConLai.Value));
                }
                else
                {
                    TongCong = _PhieuChiRepository.Table.Where(x => x.MaLoaiPhieu == "CDATU" && x.DuAn == MaDuAn && x.IsDeleted == false).Sum(x => (x.SoTienChi));
                }
                return TongCong;
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                return 0;
            }
        }
        public List<ViewCKHHA> GetCKHTS(DateTime from, DateTime to, int p, ref int total, int pageSize, string query)
        {
            try
            {
                var list = _viewCKHHARepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to && x.MaLoaiPhieu == "CKHTS");
                var s = list.ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.MaPhieuChi.ToLower().Contains(query.ToLower())
                            || x.MaLoaiPhieu.ToLower().Contains(query.ToLower())
                            || query.Contains(x.SoChungTu.ToLower())
                            || query.Contains(x.MaPhieuChi.ToLower())
                    );
                }

                var result = list.OrderBy(x => x.MaLoaiPhieu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                return null;
            }
        }
        public List<ViewKhoTaiSan> GetListCTKH(string maphieuchi)
        {
            try
            {
                SqlParameter PrMaPhieuChi = new SqlParameter("maphieuchi", maphieuchi);
                return _dbContext.ExecuteStoredProcedureList<ViewKhoTaiSan>("GetViewKhoTaiSanKhauHao", PrMaPhieuChi).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                return null;
            }
        }
        public List<ViewChiTietKhauHao> GetViewCTKH(string maphieuchi, string maloaiphieu)
        {
            try
            {
                return _viewCTKHRepository.Table.Where(x => x.MaPhieuChiKhauHao == maphieuchi && x.MaLoaiPhieu == maloaiphieu).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                return null;
            }
        }
        public bool DeletedCTKH(string maphieuchi)
        {
            try
            {
                List<CHI_TIET_KHAU_HAO> list = _ctkhRepository.Table.Where(x => x.MaPhieuChiKhauHao == maphieuchi && x.IsDeleted == false).ToList();
                foreach (CHI_TIET_KHAU_HAO item in list)
                {
                    item.IsDeleted = true;
                    item.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                    item.UpdatedDate = DateTime.Now;
                    _ctkhRepository.Update(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                return false;
            }
        }
        public List<LichSuChiNo> GetListLichSuChiNo(string MaPhieuNo)
        {
            try
            {
                SqlParameter PrMaPhieuNo = new SqlParameter("MaPhieuNo", MaPhieuNo);
                var lst = _dbContext.ExecuteStoredProcedureList<LichSuChiNo>("sp_LichSuChiNo", PrMaPhieuNo).ToList();
                return lst;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<ViewCHCNO> GetCHCNO(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _viewCHCNORepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuChi.Contains(query.ToLower())
                    || x.TenDoiTac.ToLower().Contains(query.ToLower())
                    || x.SoChungTu.ToLower().Contains(query.ToLower())
                      || query.Contains(x.MaPhieuChi.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuChi).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.GetCHCPT(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):", ex);
                return null;
            }
        }
        public List<ViewChiMuaPTTemp> GetListCMPTUTemp(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                SqlParameter Prfrom = new SqlParameter("from", from);
                SqlParameter Prto = new SqlParameter("to", to);
                SqlParameter Pquery = new SqlParameter("query", query);
                var lst = _dbContext.ExecuteStoredProcedureList<ViewChiMuaPTTemp>("sp_Cyber_DS_LayPhieuNhapKho", Prfrom, Prto, Pquery).ToList();
                total = lst.Count;
                return lst.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.GetListCMPTUTemp",ex);
                return null;
            }
        }
        public DataTableCollection GetListCMPTUChiTietTemp(string ConnectionString,string So_ct)
        {
            try
            {
                SqlParameter So_ct_ = new SqlParameter("So_ct", So_ct);
                var lst = _dbContext.ExecuteStoredProcedureDataTableMulti(ConnectionString, "sp_Cyber_LayChiTietPhieuNhapKho", So_ct_);
                return lst;
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.GetListCMPTUChiTietTemp", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="soChungTu">SoChungTu = ma_hs + "-" + so_ct</param>
        /// <returns></returns>
        public DataTableCollection GetListCMPTUChoDongBoCyber(string ConnectionString,  string soChungTu)
        {
            try
            {
                SqlParameter So_ct_ = new SqlParameter("@lstSoChungTu", soChungTu);
                var lst = _dbContext.ExecuteStoredProcedureDataTableMulti(ConnectionString, "sp_Cyber_DS_LayPhieuNhapKho_DeDongBoQuaPMTC", So_ct_);
                return lst;
            }
            catch (Exception ex)
            {
                _log.WriteLog("SoChiService.GetListCMPTUChoDongBoCyber", ex);
                return null;
            }
        }

        public List<ViewDanhSachPhieuNhapKhoPhuTung> GetListChiMuaPhuTung(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _viewDanhSachPhieuNhapKhoPhuTungRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to);

                if (!string.IsNullOrEmpty(query))
                   q = q.Where(x => (x.SoChungTu.ToLower().Contains(query)
                                        || x.NoiDung.ToLower().Contains(query)
                                        || x.TenDoiTac.ToLower().Contains(query)
                                        || x.SoChungTu.ToLower().Contains(query)
                                        || query.Contains(x.SoChungTu.ToLower())
                                        || query.Contains(x.NoiDung.ToLower())
                                        || query.Contains(x.TenDoiTac.ToLower())
                                    )
                        );

                var result = q.OrderByDescending(x => x.CreatedDate).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                //if(ex.InnerException != null)
                //    _log.WriteLog("SoChiService.GetPhieuChiList:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoChiService.GetPhieuChiList:(" + loaiPhieu + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                _log.WriteLog("SoChiService.GetListChiMuaPhuTung:(" + query + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") ", ex);
                return null;
            }
        }
    }
}
