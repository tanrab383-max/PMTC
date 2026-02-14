using CRM.Web.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Data;
using TNK.Model;
using TNK.Services.Authentication;
using TNK.Services.Common;
using TNK.Services.LichSuThaoTac;
using TNK.Services.Log;
using TNK.Services.Users;

namespace TNK.Services.SoThu
{
    public class PhieuThuService : IPhieuThuService
    {
        IRepository<ViewTCOC> _VTCOCRepository;
        IRepository<ViewTCODV> _VTCODVRepository;
        IRepository<ViewTCOBX> _VTCOBXRepository;
        IRepository<ViewTBAHI> _VTBAHIRepository;
        IRepository<ViewTHHHB> _VTHHHBRepository;
        IRepository<ViewTVAMU> _VTVAMURepository;
        IRepository<ViewTLADT> _VTLADTRepository;
        IRepository<ViewTDIVU> _VTDIVURepository;
        IRepository<ViewTBAXE> _VTBAXERepository;
        IRepository<ViewTKHAC> _VTKHACRepository;
        IRepository<ViewTPHKI> _VTPHKIRepository;
        IRepository<ViewNKMPK> _VNKMPKRepository;
        IRepository<ViewTBDTK> _VTBDTKRepository;
        IRepository<ViewTBATS> _VTBATSRepository;
        IRepository<ViewCongNo> _VCNRepository;
        IRepository<ViewKhoXe> _VKXRepository;
        IRepository<ChiTietBanTaiSan> _CTBTSRepository;
        IRepository<DanhMucDienGiai> _DMDGRepository;
        IRepository<ViewNoPhaiThu> _VNPTRepository;
        IRepository<ViewNoPhaiThuBHDV> _VNPTBHDVRepository;
        IRepository<ViewNoDaThu> _VNDTRepository;
        IRepository<ViewCCOMX> _ViewCCOMXRepository;
        IRepository<ViewTCODV> _ViewTCODVRepository;
        IRepository<Config> _configRepository;
        IRepository<PhieuThu> _phieuThuRepository;
        IRepository<ViewPhieuThu> _ViewphieuThuRepository;
        IRepository<ViewPhieuThuCNO> _ViewphieuThuCNORepository;
        IRepository<ViewTBHXH> _ViewTBHXHRepository;
        IRepository<ChiTietGopVon> _CTGVRepository;
        IRepository<ChiTietBanPhuKien> _CTBPKRepository;
        IRepository<ChiTietBaoDuongTietKiem> _CTBDTKRespository;
        IRepository<KhoPhuTung> _KPTRepository;
        IRepository<ViewHinhThucThanhToan> _ViewHinhThucThanhToan;
        IRepository<DanhMucDienGiai> _DanhMucDienGiaiRepository;
        IRepository<ViewGiaoDichCoc> _ViewGiaoDichCocRepository;
        IAuthenticationService _authenticationService;
        ILogger _log;
        ILichSuThaoTacService _lichSuThaoTacService;
        ICommonService _commonService;
        ISoThuService _sothuService;
        IRepository<ChiTietPhieuThu> _ctptRepository;
        IRepository<REPAIR_ORDER_PYS> _PDVTRepository;
        IRepository<ViewPhieuDichVuTam> _VPDVTRepository;
        IDbContext _dbContext;
        IRepository<NoPhaiTra> _NPTRepository;
        IRepository<NoPhaiThu> _NPThuRepository;
        IRepository<ChiTietPhieuChuyenTienNoiBo> _ctPCTNBRepositority;
        IRepository<NoPhaiThu> _NoPhaiThuRepositority;
        IRepository<ViewNoPhaiTra> _VNPTraRepository;
        IRepository<PhieuQuyetToan> _phieuQTRepository;
        IUserervice _userService;
        User CurrentUser;
        string _ipClient = "";
        string _hostNameClient = "";
        string _clientBrowser = "";
        AF.Library.Logger logger = new AF.Library.Logger("PhieuThuService");
        IRepository<ViewPhieuBDTKTamCybers> _ViewPhieuBDTKTamCybersRepository;
        public PhieuThuService(
              IRepository<ViewTCODV> _VTCODVRepository
            , IRepository<ViewTCOC> _VTCOCRepository
            , IRepository<ViewTCOBX> _VTCOBXRepository
            , IRepository<ViewTBAHI> _VTBAHIRepository
            , IRepository<ViewTHHHB> _VTHHHBRepository
            , IRepository<ViewTVAMU> _VTVAMURepository
            , IRepository<ViewTLADT> _VTLADTRepository
            , IRepository<ViewTDIVU> _VTDIVURepository
            , IRepository<ViewTBAXE> _VTBAXERepository
            , IRepository<ViewTKHAC> _VTKHACRepository
            , IRepository<ViewTPHKI> _VTPHKIRepository
            , IRepository<ViewNKMPK> _VNKMPKRepository
            , IRepository<ViewTBDTK> _VTBDTKRepository
            , IRepository<ViewTBATS> _VTBATSRepository
            , IRepository<ViewCongNo> _VCNRepository
            , IRepository<ViewNoPhaiThu> _VNPTRepository
            , IRepository<ViewNoPhaiThuBHDV> _VNPTBHDVRepository
            , IRepository<ViewNoDaThu> _VNDTRepository
            , IRepository<PhieuThu> _phieuThuRepository
            , IRepository<ChiTietGopVon> _CTGVRepository
            , IRepository<ChiTietBanPhuKien> _CTBPKRepository
            , IRepository<ChiTietBaoDuongTietKiem> CTBDTKRespository
            , IRepository<KhoPhuTung> _KPTRepository
            , IRepository<REPAIR_ORDER_PYS> _PDVTRepository
            , IRepository<ViewPhieuDichVuTam> _VPDVTRepository
            , IRepository<ViewCCOMX> _ViewCCOMXRepository
            , IRepository<ViewTCODV> _ViewTCODVRepository
            , IRepository<NoPhaiTra> _NPTRepository
            , IRepository<ViewKhoXe> _VKXRepository
            , IRepository<DanhMucDienGiai> _DanhMucDienGiaiRepository
            , ISoThuService _sothuService
            ,IRepository<ViewHinhThucThanhToan> _ViewHinhThucThanhToan
            , IRepository<ChiTietPhieuThu> _ctptRepository
             , IRepository<Config> _configRepository
            , IRepository<ChiTietBanTaiSan> _CTBTSRepository
            ,IRepository<DanhMucDienGiai> _DMDGRepository
            , IAuthenticationService _authenticationService
            , ICommonService _commonService
            , ILogger _log
            , ILichSuThaoTacService _lichSuThaoTacService
            , IDbContext _dbContext
            ,IRepository<ChiTietPhieuChuyenTienNoiBo> _ctPCTNBRepositority
            ,IRepository<NoPhaiThu> _NPThuRepository
            , IRepository<ViewPhieuThu> _ViewphieuThuRepository
            , IRepository<ViewGiaoDichCoc> _ViewGiaoDichCocRepository
            , IRepository<ViewTBHXH> _ViewTBHXHRepository
            , IRepository<NoPhaiThu> _noPhaiThuRepositority
            , IRepository<ViewNoPhaiTra> _VNPTraRepository
            , IUserervice _userService
            , IRepository<ViewPhieuBDTKTamCybers> _ViewPhieuBDTKTamCybersRepository
            , IRepository<ViewPhieuThuCNO> _ViewphieuThuCNORepository
            , IRepository<PhieuQuyetToan> _phieuQTRepository
            )
        {
            this._VTCOCRepository = _VTCOCRepository;
            this._VTCODVRepository = _VTCODVRepository;
            this._VTCOBXRepository = _VTCOBXRepository;
            this._VTBAHIRepository = _VTBAHIRepository;
            this._VTHHHBRepository = _VTHHHBRepository;
            this._VTVAMURepository = _VTVAMURepository;
            this._VTLADTRepository = _VTLADTRepository;
            this._VTDIVURepository = _VTDIVURepository;
            this._VTBAXERepository = _VTBAXERepository;
            this._VTKHACRepository = _VTKHACRepository;
            this._VTPHKIRepository = _VTPHKIRepository;
            this._VNKMPKRepository = _VNKMPKRepository;
            this._VTBDTKRepository = _VTBDTKRepository;
            this._VTBATSRepository = _VTBATSRepository;
            this._VCNRepository = _VCNRepository;
            this._VNDTRepository = _VNDTRepository;
            this._VNPTRepository = _VNPTRepository;
            this._VNPTBHDVRepository = _VNPTBHDVRepository;
            this._PDVTRepository = _PDVTRepository;
            this._VPDVTRepository = _VPDVTRepository;
            this._NPTRepository = _NPTRepository;
            this._ViewHinhThucThanhToan = _ViewHinhThucThanhToan;
            this._ctptRepository = _ctptRepository;
            this._ViewCCOMXRepository = _ViewCCOMXRepository;
            this._ViewTCODVRepository = _ViewTCODVRepository;
            this._phieuThuRepository = _phieuThuRepository;
            this._authenticationService = _authenticationService;
            this._commonService = _commonService;
            this._configRepository = _configRepository;
            this._DanhMucDienGiaiRepository = _DanhMucDienGiaiRepository;
            this._log = _log;
            this._lichSuThaoTacService = _lichSuThaoTacService;
            this._dbContext = _dbContext;
            this._sothuService = _sothuService;
            this._CTBTSRepository = _CTBTSRepository;
            this._DMDGRepository = _DMDGRepository;
            this._CTGVRepository = _CTGVRepository;
            this._CTBPKRepository = _CTBPKRepository;
            this._CTBDTKRespository = CTBDTKRespository;
            this._KPTRepository = _KPTRepository;
            this._ctPCTNBRepositority = _ctPCTNBRepositority;
            this._NPThuRepository = _NPThuRepository;
            this._ViewphieuThuRepository = _ViewphieuThuRepository;
            this._VKXRepository = _VKXRepository;
            this._ViewGiaoDichCocRepository = _ViewGiaoDichCocRepository;
            this._ViewTBHXHRepository = _ViewTBHXHRepository;
            this._NoPhaiThuRepositority = _noPhaiThuRepositority;
            this._VNPTraRepository = _VNPTraRepository;
            this._userService = _userService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
            this._ViewPhieuBDTKTamCybersRepository = _ViewPhieuBDTKTamCybersRepository;
            this._ViewphieuThuCNORepository = _ViewphieuThuCNORepository;
            this._phieuQTRepository = _phieuQTRepository;
        }
        static int gioGuiEmailLast = Convert.ToInt32(DateTime.Now.Hour.ToString());
        static bool Send = false;
        public string ContentMail(string maPhieuThu)
        {
            string Content = "";
            PhieuThu PT = _phieuThuRepository.Table.Where(x => x.MaPhieuThu == maPhieuThu && x.IsDeleted == false).FirstOrDefault();
            if(PT!= null)
            {
                Guid nguoiTao = PT.CreatedBy.Value;
                Guid nguoiSua = PT.UpdatedBy.Value;
                User nguoitacDong = new User();
                if (nguoiTao != nguoiSua)
                {
                    nguoitacDong = _userService.getByUserId(nguoiTao);
                }
                else
                {
                    nguoitacDong = _userService.getByUserId(nguoiSua);
                }
                Content = "Mã phiếu thu " + maPhieuThu + " do " + nguoitacDong.UserName + " đã thao tác lúc " + (PT.UpdatedDate == null ? PT.CreatedDate : PT.UpdatedDate) + " đã làm mất cân!";
            }
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
        public List<ViewTPHKI> GetTPHKI(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTPHKIRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && !(x.TongCong == 0 && !string.IsNullOrEmpty(x.SoHopDong)));
                var aa = _VTPHKIRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)                   
                    || x.SoChungTu.ToLower().Contains(query)
                    //|| x.LoaiXe.ToLower().Contains(query) 
                    //|| x.NhaCungCap.ToLower().Contains(query)
                    || x.DienThoai.ToLower().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                   // || query.Contains(x.DienThoai.ToLower())
                   // || query.Contains(x.HoTen.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
          catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTPHKI:("+ from +","+ to + "," + query + "," + p + "," + total + "," + pageSize + ") " , ex);
                return null;
            }
        }

        public List<ViewTPHKI> GetTPHKITab(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTPHKIRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.TongCong == 0 && !string.IsNullOrEmpty(x.SoHopDong));
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.DienThoai.ToLower().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                   // || query.Contains(x.DienThoai.ToLower())
                   // || query.Contains(x.HoTen.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTPHKITab:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") " ,ex);
                return null;
            }
        }

        public List<ViewTPHKITemp> GetTPHKITabTemp(DateTime from, DateTime to, string query, string typeTicketTemp, int p, ref int total, int pageSize, ref List<ViewTPHKITemp> listViewFull, string loai)
        {
            try
            {
                SqlParameter pfrom = new SqlParameter("from", from);
                SqlParameter pto = new SqlParameter("to", to);
                SqlParameter pquery = new SqlParameter("keyWord", query);
                SqlParameter ptypeTicketTemp = new SqlParameter("typeTicketTemp", typeTicketTemp);
                List<ViewTPHKITemp> list = new List<ViewTPHKITemp>();
                if(!string.IsNullOrWhiteSpace(loai))
                {
                    ptypeTicketTemp = new SqlParameter("typeTicketTemp", string.Empty);
                    list = _dbContext.ExecuteStoredProcedureList<ViewTPHKITemp>("sp_DanhSach_PhuKien_HuaTang", pfrom, pto, pquery, ptypeTicketTemp).ToList();
                }
                else
                {
                    list = _dbContext.ExecuteStoredProcedureList<ViewTPHKITemp>("sp_DanhSach_PhuKien", pfrom, pto, pquery, ptypeTicketTemp).ToList();
                }
                
                //if (query != "")
                //{
                //    list = list.Where(x =>x.TEN_KHACH_HANG.ToString().ToLower().Contains(query.ToLower())).ToList();
                //}
                listViewFull = list;
                total = list.Count;
                if (query != "")
                {
                    return list.Take(pageSize).ToList();
                }
                return list.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ViewNKMPK> GetTNKMPK(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VNKMPKRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                var s = q.ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.DienThoai.ToLower().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                     || query.Contains(x.DienThoai.ToLower())
                     || query.Contains(x.HoTen.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTPHKITab:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") ", ex);
                return null;
            }
        }
        public List<ViewPhieuThu> GetBDTK(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,  string type)
        {
            try
            {
                query = query.ToLower();
                if (type == "TBL")
                {
                    //co số tiền thu thì cho vào thu bán lẻ
                    var q = _ViewphieuThuRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.MaLoaiPhieu == "TBDTK"
                            && !( x.SoTienThu == 0 && !string.IsNullOrEmpty(x.SoHopDong))); // (x.SoHopDong == null || x.SoHopDong == ""));
                    if (!string.IsNullOrEmpty(query))
                    {
                        q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                        || x.SoChungTu.ToLower().Contains(query)
                        || x.DienThoai.ToLower().Contains(query)
                        || x.Hoten.ToLower().Contains(query)
                        || query.Contains(x.MaPhieuThu.ToLower())
                        || query.Contains(x.SoChungTu.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                        || query.Contains(x.SoKhung.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        );
                    }
                    var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                    total = result.Count;
                    return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    //so tien = 0 và có số hợp đồng thi cho vao phieu KM (thang.tran - 20220622)
                    var q = _ViewphieuThuRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.MaLoaiPhieu == "TBDTK" && x.SoTienThu == 0 && x.SoHopDong != null && x.SoHopDong != "");
                        if (!string.IsNullOrEmpty(query))
                        {
                            q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                            || x.SoChungTu.ToLower().Contains(query)
                            || x.DienThoai.ToLower().Contains(query)
                            || x.Hoten.ToLower().Contains(query)
                            || query.Contains(x.MaPhieuThu.ToLower())
                            || query.Contains(x.SoChungTu.ToLower())
                            || query.Contains(x.BienSo.ToLower())
                            || query.Contains(x.SoKhung.ToLower())
                            || query.Contains(x.SoHopDong.ToLower())
                            );
                        }
                        var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                        total = result.Count;
                        return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }    

            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTPHKITab:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") ", ex);
                return null;
            }
        }
        public PhieuThu GetPhieuThuBySHD(string shd)
        {
            try
            {
                return _phieuThuRepository.Table.FirstOrDefault(x => x.SoHopDong == shd && x.LoaiXe != "");
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetPhieuThuBySHD:(" + shd + ") " ,ex);
                return null;
            }
            
        }
        public PhieuThu GetPhieuThuTBAXEBySHD(string shd)
        {
            try
            {
                return _phieuThuRepository.Table.FirstOrDefault(x =>( x.SoHopDong == shd || x.SoKhung.ToUpper() == shd.ToUpper()) && x.MaLoaiPhieu == "TBAXE" && x.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetPhieuThuTBAXEBySHD:(" + shd + ") " ,ex);
                return null;
            }
        }

        public PhieuThu GetPhieuThuTDIVUBySoDichVu(string soDichVu)
        {
            try
            {
                return _phieuThuRepository.Table.FirstOrDefault(x => x.SoHopDong == soDichVu && x.MaLoaiPhieu == "TDIVU" && x.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetPhieuThuTDIVUBySoDichVu:(" + soDichVu + ") " ,ex);
                return null;
            }
        }

        public List<ViewNoDaThu> GetNBHDV(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)//truc cua ngay 10/4/2018
        {
            try
            {
                var list = _dbContext.ExecuteStoredProcedureList<ViewNoDaThu>("getNoBHDV").Where(x => x.LoaiPhieuNo == id && x.NgayThu >= from && x.NgayThu <= to 
                && (x.MaPhieuNo.Contains(query) || x.SoPhieuQuyetToan.Contains(query) 
                || x.KhachHang.Contains(query) || (x.BienSo != null && x.BienSo.Contains(query))
                || (x.ChungTuThu != null && x.ChungTuThu.Contains(query)))).ToList();
                total = list.Count();
                return list.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNBHDV:" , ex);
                return null;
            }
            
        }

        public ViewNoDaThu GetNBHDVById(string id)
        {
            try
            {
                SqlParameter pid = new SqlParameter("id", id);
                return _dbContext.ExecuteStoredProcedureList<ViewNoDaThu>("getNoBHDVById", pid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNBHDVById:",ex);
                return null;
            }
            

        }

        public List<ViewTKHAC> GetTKHAC(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTKHACRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.ThongTinKhac.ToLower().Contains(query)
                     || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTKHAC:" ,ex);
                return null;
            }
            
        }

        public List<PhieuThu> GetKMTMV(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _phieuThuRepository.Table.Where(x => x.NgayThu >= from 
                            && x.NgayThu <= to 
                            && x.MaLoaiPhieu == "KMTMV"
                            && x.IsDeleted == false);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.NoiDung.ToLower().Contains(query)
                     || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetKMTMV:" ,ex);
                return null;
            }

        }

        public ViewNoPhaiThu GetNoPhaiThu(string id)
        {
            try
            {
                return _VNPTRepository.Table.FirstOrDefault(x => x.MaPhieuNo == id);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoPhaiThu:" , ex);
                return null;
            }
            
        }

        public ViewNoDaThu GetNoDaThu(string id)
        {
            try
            {
                return _VNDTRepository.Table.FirstOrDefault(x => x.SoPhieuQuyetToan == id);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThu:" , ex);
                return null;
            }
            
        }
        public List<ViewNoDaThu> GetNoDaThus(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            query = query.ToLower();
            try
            {
                var q = _VNDTRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.LoaiPhieuNo == id);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.KhachHang.ToLower().Contains(query)
                    || x.ChungTuThu.ToLower().Contains(query)
                    || x.TinhTrang.ToLower().Contains(query)
                    || x.MaPhieuNo.ToLower().Contains(query)
                    || x.SoPhieuQuyetToan.ToLower().Contains(query)
                    //|| query.Contains(x.KhachHang.ToLower())
                    || query.Contains(x.ChungTuThu.ToLower())
                    || query.Contains(x.MaPhieuNo.ToLower())
                    || query.Contains(x.BienSo.ToLower())
                    || query.Contains(x.SoPhieuQuyetToan.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThus:", ex);
                return null;
            }
        }

        public List<ViewNoPhaiThuBHDV> GetNoPhaiThuBHDVs(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {

                if (id.ToUpper() == "NOTMV")
                {
                    var q = _VNPTBHDVRepository.Table.Where(x => x.LoaiPhieuNo == id);
                    var result = q.OrderByDescending(x => x.SoHopDong).ToList();
                    total = result.Count;
                    return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    var q = _VNPTBHDVRepository.Table.Where(x => x.NgayNo >= from && x.NgayNo <= to && x.LoaiPhieuNo == id);
                    if (!string.IsNullOrEmpty(query))
                    {
                        query = query.ToLower();
                        if (id == "NHHBH")
                        {
                            q = q.Where(x => (x.KhachHang.ToLower().Contains(query)
                           || x.ChungTuThu.ToLower().Contains(query)
                           || x.TinhTrang.ToLower().Contains(query)
                           || x.MaPhieuNo.ToLower().Contains(query)
                           || x.Hoten.ToLower().Contains(query.ToLower())
                           || x.MaPhieuPhatSinh.ToLower().Contains(query)
                           || x.GhiChu.ToLower().Contains(query) //sau nay bo phan nay de tranh bi cham, chi de thoi gian ban dau luc khoi tao
                           || x.SoHopDong.ToLower().Contains(query)
                           || x.NoiDung.ToLower().Contains(query)

                        || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.MaPhieuPhatSinh.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        //  || query.Contains(x.GhiChu.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                           //   || query.Contains(x.NoiDung.ToLower())
                           )
                           && x.SoTienNo > 0
                           );
                        }
                        else
                        {
                            q = q.Where(x => (x.KhachHang.ToLower().Contains(query)
                           || x.ChungTuThu.ToLower().Contains(query)
                           || x.TinhTrang.ToLower().Contains(query)
                           || x.MaPhieuNo.ToLower().Contains(query)
                           || x.Hoten.ToLower().Contains(query.ToLower())
                           || x.MaPhieuPhatSinh.ToLower().Contains(query)
                           || x.GhiChu.ToLower().Contains(query) //sau nay bo phan nay de tranh bi cham, chi de thoi gian ban dau luc khoi tao
                           || x.SoHopDong.ToLower().Contains(query)
                           || x.NoiDung.ToLower().Contains(query)
                           || x.BienSo.ToLower().Contains(query)
                           || x.KhachHang.ToLower().Contains(query)
                           || x.DienThoai.ToLower().Contains(query)
                        //   || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.MaPhieuPhatSinh.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        //  || query.Contains(x.GhiChu.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                           //   || query.Contains(x.NoiDung.ToLower())
                           )
                           && x.SoTienNo > 0
                           );
                        }
                    }
                    else
                        q = q.Where(x => x.SoTienNo > 0);

                    var result = q.OrderByDescending(x => x.SoHopDong).ToList();
                    total = result.Count;
                    return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThus:", ex);
                return null;
            }
        }
       
        //search theo tinh trang giai trinh
        public List<ViewNoPhaiThuBHDV> GetNoPhaiThuBHDVs_GiaiTrinh(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang, string dagiaitrinh)
        {
            try
            {
                query = query.ToLower();
                SqlParameter pFrom = new SqlParameter("from", from);//xử lý trước khi update
                SqlParameter pTo = new SqlParameter("to", to);
               
                SqlParameter pQuery = new SqlParameter("query", query);//xử lý trước khi update
                SqlParameter pTinhTrang = new SqlParameter("tinhTrang", tinhtrang);
                SqlParameter pDaGiaiTrinh = new SqlParameter("dagiaitrinh", dagiaitrinh);
                var result = _dbContext.ExecuteStoredProcedureList<ViewNoPhaiThuBHDV>("sp_SelectNoPhaiThuNBHDV_GiaiTrinh", pTinhTrang, pFrom, pTo, pQuery,pDaGiaiTrinh ).ToList();
                               
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThus:", ex);

                return null;
            }
        }

        //search theo tinh trang
        public List<ViewNoPhaiThuBHDV> GetNoPhaiThuBHDVs(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang, string loaiBH)
        {
            try
            {
                query = query.ToLower();
                var q = _VNPTBHDVRepository.Table.Where(x => x.NgayNo >= from && x.NgayNo <= to && x.LoaiPhieuNo == id);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => (x.KhachHang.ToLower().Contains(query)
                       || x.ChungTuThu.ToLower().Contains(query)
                       || x.TinhTrang.ToLower().Contains(query)
                       || x.MaPhieuNo.ToLower().Contains(query)
                       || x.Hoten.ToLower().Contains(query.ToLower())
                       || x.MaPhieuPhatSinh.ToLower().Contains(query)
                       || x.GhiChu.ToLower().Contains(query.ToLower()) //sau nay bo phan nay de tranh bi cham, chi de thoi gian ban dau luc khoi tao
                       || x.SoPhieuQuyetToan.ToLower().Contains(query.ToLower())
                       || x.SoHopDong.ToLower().Contains(query)
                       || x.TenDoiTac.ToLower().Contains(query)
                       || x.ThongTinKhac.ToLower().Contains(query.ToLower())
                       || x.NoiDung.ToLower().Contains(query.ToLower())
                    ////|| query.Contains(x.KhachHang.ToLower())
                    || query.Contains(x.ChungTuThu.ToLower())
                    //|| query.Contains(x.MaPhieuNo.ToLower())
                       || query.Contains(x.MaPhieuPhatSinh.ToLower())
                       || query.Contains(x.MaPhieuNo.ToLower())
                       //|| query.Contains(x.GhiChu.ToLower())
                       || query.Contains(x.SoHopDong.ToLower())
                       || query.Contains(x.BienSo.ToLower())
                       || query.Contains(x.TenDoiTac.ToLower())

                       //   //   || query.Contains(x.NoiDung.ToLower())
                       )
                       && x.SoTienNo >= 1
                       );

                }
                else
                {
                    q = q.Where(x => x.SoTienNo >= 1);
                }
                if (tinhtrang == "H")
                {
                    q = q.Where(x => x.TinhTrang == "Hoàn tất");
                }
                else if (tinhtrang == "C")
                {
                    q = q.Where(x => x.TinhTrang == "Chưa hoàn tất");
                }
                if (!string.IsNullOrEmpty(loaiBH))
                {
                    q = q.Where(x => x.LoaiNo == loaiBH);
                }
                var result = new List<ViewNoPhaiThuBHDV>();
                if (id == "NBHDV")
                    result = q.ToList().OrderByDescending(x => x.SoPhieuQuyetToan).ToList();
                else if (id == "NNHBX")
                    result = q.OrderByDescending(x => x.SoHopDong).ToList();
                else
                    result = q.AsParallel().OrderByDescending(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThus:", ex);

                return null;
            }
        }


        public List<ViewNoPhaiThu> GetNoPhaiThus(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {

                if (id.ToUpper() == "NOTMV")
                {
                    var q = _VNPTRepository.Table.Where(x => x.LoaiPhieuNo == id);
                    var result = q.OrderByDescending(x => x.SoHopDong).ToList();
                    total = result.Count;
                    return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    var q = _VNPTRepository.Table.Where(x => x.NgayNo >= from && x.NgayNo <= to && x.LoaiPhieuNo == id);
                    if (!string.IsNullOrEmpty(query))
                    {
                        query = query.ToLower();
                        if(id== "NHHBH")
                        {
                            q = q.Where(x => (x.KhachHang.ToLower().Contains(query)
                           || x.ChungTuThu.ToLower().Contains(query)
                           || x.TinhTrang.ToLower().Contains(query)
                           || x.MaPhieuNo.ToLower().Contains(query)
                           || x.Hoten.ToLower().Contains(query.ToLower())
                           || x.MaPhieuPhatSinh.ToLower().Contains(query)
                           || x.GhiChu.ToLower().Contains(query) //sau nay bo phan nay de tranh bi cham, chi de thoi gian ban dau luc khoi tao
                           || x.SoHopDong.ToLower().Contains(query)
                           || x.NoiDung.ToLower().Contains(query)

                        || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.MaPhieuPhatSinh.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        //  || query.Contains(x.GhiChu.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                           //   || query.Contains(x.NoiDung.ToLower())
                           )
                           && x.SoTienNo > 0
                           );
                        }
                        else
                        {
                            q = q.Where(x => (x.KhachHang.ToLower().Contains(query)
                           || x.ChungTuThu.ToLower().Contains(query)
                           || x.TinhTrang.ToLower().Contains(query)
                           || x.MaPhieuNo.ToLower().Contains(query)
                           || x.Hoten.ToLower().Contains(query.ToLower())
                           || x.MaPhieuPhatSinh.ToLower().Contains(query)
                           || x.GhiChu.ToLower().Contains(query) //sau nay bo phan nay de tranh bi cham, chi de thoi gian ban dau luc khoi tao
                           || x.SoHopDong.ToLower().Contains(query)
                           || x.NoiDung.ToLower().Contains(query)
                           || x.BienSo.ToLower().Contains(query)
                           || x.KhachHang.ToLower().Contains(query)
                           || x.DienThoai.ToLower().Contains(query)
                        //   || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.MaPhieuPhatSinh.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        //  || query.Contains(x.GhiChu.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                           //   || query.Contains(x.NoiDung.ToLower())
                           )
                           && x.SoTienNo > 0
                           );
                        }
                    }
                    else 
                        q = q.Where(x => x.SoTienNo > 0);

                    var result = q.OrderByDescending(x => x.SoHopDong).ToList();
                    total = result.Count;
                    return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThus:" ,ex);
                return null;
            }
        }

        //search theo tinh trang
        public List<ViewNoPhaiThu> GetNoPhaiThus(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang)
        {
            try
            {
                query = query.ToLower();
                var q = _VNPTRepository.Table.Where(x => x.NgayNo >= from && x.NgayNo <= to && x.LoaiPhieuNo == id);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => (x.KhachHang.ToLower().Contains(query.ToLower())
                       || x.ChungTuThu.ToLower().Contains(query.ToLower())
                       || x.TinhTrang.ToLower().Contains(query.ToLower())
                       || x.MaPhieuNo.ToLower().Contains(query.ToLower())
                       || x.Hoten.ToLower().Contains(query.ToLower())
                       || x.MaPhieuPhatSinh.ToLower().Contains(query.ToLower())
                       || x.GhiChu.ToLower().Contains(query.ToLower()) //sau nay bo phan nay de tranh bi cham, chi de thoi gian ban dau luc khoi tao
                       || x.SoPhieuQuyetToan.ToLower().Contains(query.ToLower())
                       || x.SoHopDong.ToLower().Contains(query.ToLower())
                       || x.TenDoiTac.ToLower().Contains(query.ToLower())
                       || x.SoHoaDon.ToLower().Contains(query.ToLower())

                    //|| query.Contains(x.KhachHang.ToLower())
                    || query.Contains(x.ChungTuThu.ToLower())
                    || query.Contains(x.MaPhieuNo.ToLower())
                    || query.Contains(x.MaPhieuPhatSinh.ToLower())
                    || query.Contains(x.MaPhieuNo.ToLower())
                    || query.Contains(x.GhiChu.ToLower())
                    || query.Contains(x.SoHopDong.ToLower())
                    || query.Contains(x.BienSo.ToLower())
                    || query.Contains(x.TenDoiTac.ToLower())
                    || query.Contains(x.SoHoaDon.ToLower())
                       //   || query.Contains(x.NoiDung.ToLower())
                       )
                       && x.SoTienNo > 0
                       );
                    
                }
                else
                {
                    q = q.Where(x => x.SoTienNo > 1);
                }
                if(tinhtrang == "H")
                {
                    q = q.Where(x=>x.TinhTrang == "Hoàn tất");
                }
                else if(tinhtrang == "C")
                {
                    q = q.Where(x => x.TinhTrang == "Chưa hoàn tất");
                }
                var result = new List<ViewNoPhaiThu>();
                if (id == "NBHDV")
                    result = q.OrderByDescending(x => x.SoPhieuQuyetToan).ToList();
                else if (id == "NNHBX")
                    result = q.OrderByDescending(x => x.SoHopDong).ToList();
                else
                    result = q.OrderByDescending(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThus:" ,ex);
                
                return null;
            }
        }

        public List<ViewNoPhaiThu> GetNoPhaiThu()
        {
            try
            {
                return _VNPTRepository.Table.ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoPhaiThu:" ,ex);
                return null;
            }
            
        }

        public ViewTBAXE GetTBAXE(string soKhung)
        {
            try
            {
                return _VTBAXERepository.Table.FirstOrDefault(x => x.SoKhung == soKhung);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTBAXE:("+ soKhung + ")" ,ex);
                if(ex.InnerException != null)
                    _log.WriteLog("PhieuThuService.GetTBAXE:(" + soKhung + ")" + ex.InnerException.ToString());
                return null;
            }
        }

        public ViewTBAXE GetCocXe(string soHopDong)
        {
            try
            {
                return _VTBAXERepository.Table.FirstOrDefault(x => x.SoHopDong == soHopDong);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTBAXE:(" + soHopDong + ")", ex);
                if (ex.InnerException != null)
                    _log.WriteLog("PhieuThuService.GetTBAXE:(" + soHopDong + ")" + ex.InnerException.ToString());
                return null;
            }
        }

        public List<ViewTBAXE> GetTBAXEBySoHopDong(string soHopDong)
        {
            try
            {
                return _VTBAXERepository.Table.Where(x => x.SoHopDong.ToString() == soHopDong).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTBAXEBySoHopDong:(" + soHopDong + ")" ,ex);
              
                return null;
            }
        }

        public List<ViewTBAXE> GetTBAXE(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang)
        {
            try
            {
                /*
                query = query.ToLower();
                //var q = _VTBAXERepository.Table.Where(x => x.NgayHopDong >= from && x.NgayHopDong <= to);
                var q = _VTBAXERepository.Table.Where(x => x.NgayHopDong >= from);
                var r = q.ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.SoHopDong.ToString().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || x.SoKhung.ToLower().Contains(query)
                     || x.DienThoai.ToLower().Contains(query)

                      || query.Contains(x.SoHopDong.ToString().ToLower())
                 //   || query.Contains(x.HoTen.ToLower())
                    || query.Contains(x.SoKhung.ToLower())
                    
                    );
                }
                if(tinhtrang == "TK")
                {
                    q = q.Where(x => x.TinhTrang == tinhtrang);
                }
                else if(tinhtrang == "CNK")
                {
                    q = q.Where(x => x.TinhTrang == tinhtrang);
                }
                var result = q.OrderByDescending(x => x.SoHopDong).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                */
                SqlParameter pFrom = new SqlParameter("from", from);//xử lý trước khi update
                SqlParameter pTo = new SqlParameter("to", to);

                SqlParameter pQuery = new SqlParameter("query", query);//xử lý trước khi update
                SqlParameter pTinhTrang = new SqlParameter("tinhTrang", tinhtrang);

                pQuery.Size = 4000;
                pTinhTrang.Size = 100;
                //DataTable dt = _dbContext.ExecuteStoredProcedureDataTable("sp_GetTBAXE", pFrom, pTo, pQuery, pTinhTrang);
                var list = _dbContext.ExecuteStoredProcedureList<ViewTBAXE>("sp_GetTBAXE",pFrom,pTo,pQuery,pTinhTrang).ToList();
                //List<ViewTBAXE> list = new List<ViewTBAXE>();
                //for(int i =0;  i < dt.Rows.Count; i++)
                //{
                //    ViewTBAXE v = new ViewTBAXE();
                //    v.SoHopDong = Convert.ToInt32(dt.Rows[i]["SoHopDong"]);
                //    v.NgayHopDong = Convert.ToDateTime(dt.Rows[i]["NgayHopDong"]);
                //    v.HoTen = dt.Rows[i]["HoTen"].ToString();
                //    v.DiaChi = dt.Rows[i]["DiaChi"].ToString();
                //    v.DienThoai = dt.Rows[i]["DienThoai"].ToString();
                //    v.SoKhung = dt.Rows[i]["SoKhung"].ToString();
                //    v.SoMay = dt.Rows[i]["SoMay"].ToString();
                //    v.LoaiXe = dt.Rows[i]["LoaiXe"].ToString();
                //    v.GiaNiemYet = Convert.ToInt32(dt.Rows[i]["GiaNiemYet"]);
                //    v.GiamGia = Convert.ToInt32(dt.Rows[i]["GiamGia"]);
                //    v.GiaBan = Convert.ToInt32(dt.Rows[i]["GiaBan"]);
                //    v.MauXe = dt.Rows[i]["MauXe"].ToString();
                //    v.TinhTrang = dt.Rows[i]["TinhTrang"].ToString();
                //    list.Add(v);
                //}
                total = list.Count;
                return list.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTBAXE:" ,ex);
                return null;
            }
           
        }
        public List<ViewTDIVU> GetTDIVU(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTDIVURepository.Table.Where(x => x.MaLoaiPhieu == id && x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.SoHopDong.ToString().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || x.BienSo.ToLower().Contains(query)
                    || x.DienThoai.ToLower().Contains(query)
                    || x.SoKhung.ToLower().Contains(query)
                    || x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoThamChieu.ToLower().Contains(query)

                    || query.Contains(x.SoHopDong.ToString().ToLower())
                   // || query.Contains(x.HoTen.ToLower())
                   // || query.Contains(x.SoKhung.ToLower())

                    || query.Contains(x.BienSo.ToString().ToLower())
                   // || query.Contains(x.DienThoai.ToLower())
                    || query.Contains(x.MaPhieuThu.ToLower())
                    );
                }
                var result = q.ToList().OrderByDescending(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTDIVU:" ,ex);
                return null;
            }
            
        }
        public List<ViewTLADT> GetTLADT(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTLADTRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.DTTL.ToLower().Contains(query)

                    || query.Contains(x.MaPhieuThu.ToString().ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                    || query.Contains(x.DTTL.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTLADT:" ,ex);
                return null;
            }
            
        }
        public List<ViewTHHHB> GetTHHHB(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTHHHBRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.DoiTac.ToLower().Contains(query)

                    || query.Contains(x.MaPhieuThu.ToString().ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                    || query.Contains(x.DoiTac.ToLower())

                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTHHHB:" ,ex);
                return null;
            }
            
        }
        public List<ViewTVAMU> GetTVAMU(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTVAMURepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.DVCV.ToLower().Contains(query)
                    || x.DienThoai.ToLower().Contains(query)

                    || query.Contains(x.MaPhieuThu.ToString().ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                    || query.Contains(x.DVCV.ToLower())
                //    || query.Contains(x.DienThoai.ToString().ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTVAMU:" ,ex);
                return null;
            }
            
        }
        /// <summary>
        /// Chi lay nhung phieu chua bo coc nếu là phiếu tạo mới, nếu phiếu edit thì lấy luôn phiếu bỏ cọc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public List<ViewTCOC> GetTCOC(string maSo, bool isForInsert)
        //{
        //    try
        //    {
        //        if (isForInsert)
        //            return _VTCOCRepository.Table.Where(x => maSo != "" && maSo!=null && (x.BienSo == maSo || x.SoHopDong == maSo || x.HoTen.ToLower().Contains(maSo.ToLower())) && x.TTPCode != "B").ToList();
        //        else
        //            return _VTCOCRepository.Table.Where(x => maSo != "" && maSo != null && (x.BienSo == maSo || x.SoHopDong == maSo || x.HoTen.ToLower().Contains(maSo.ToLower()))).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.WriteLog("PhieuThuService.GetTCOC:" ,ex);
        //        return null;
        //    }
        //}
        //phan load coc co ho ten
        public List<ViewTCOC> GetTCOC(string maPhieuThu,string maSo, string HoTen, bool isForInsert)
        {
            try
            {
                if (maSo == "")
                    maSo = HoTen;
                List<ViewTCOC> list = new List<ViewTCOC>();
                if (isForInsert)
                    list =  _VTCOCRepository.Table.Where(x => maSo != "" && maSo != null && (x.BienSo == maSo || x.SoHopDong == maSo || x.HoTen.ToLower().Contains(HoTen.ToLower())) && x.TTPCode != "B").ToList();
                else
                    list = _VTCOCRepository.Table.Where(x => maSo != "" && maSo != null && (x.BienSo == maSo || x.SoHopDong == maSo || x.HoTen.ToLower().Contains(HoTen.ToLower()))).ToList();
                
                foreach(var temp in list)
                {
                    List<ChiTietPhieuThu> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuThu == maPhieuThu
                        && x.HinhThucThanhToan == "DC"
                        && x.SoThamChieu == temp.MaPhieuThu
                        && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach(var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai + TongSoTien;
                    temp.SoTienDaSuDung = TongSoTien;
                }
                // Lấy cọc còn lại != 0 Lộc 200828
                list = list.Where(x => x.TongCong != 0).ToList();
                return list;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCOC:" ,ex);
                return null;
            }
        }

        /// <summary>
        /// Lay coc theo dieu kien Bien so xe + So hop dong
        /// </summary>
        /// <param name="maPhieuThu"></param>
        /// <param name="maSo"></param>
        /// <param name="HoTen"></param>
        /// <param name="isForInsert"></param>
        /// <returns></returns>
        /// thang.tran them ngay 23/5/2022 xu ly viec coc dich vu + coc ban xe duoc su dung chung o cac phieu co dung coc
        public List<ViewTCOC> GetTCOC_V2(string maPhieuThu, string bienSo, string soHopDong, string hoTen, bool isForInsert)
        {
            try
            {

                List<ViewTCOC> list = new List<ViewTCOC>();
                //if (isForInsert)
                //    list = _VTCOCRepository.Table.Where(x => maSo != "" && maSo != null && (x.BienSo == maSo || x.SoHopDong == maSo || x.HoTen.ToLower().Contains(HoTen.ToLower())) && x.TTPCode != "B").ToList();
                //else
                //    list = _VTCOCRepository.Table.Where(x => maSo != "" && maSo != null && (x.BienSo == maSo || x.SoHopDong == maSo || x.HoTen.ToLower().Contains(HoTen.ToLower()))).ToList();

                SqlParameter BienSo = new SqlParameter("BienSo", bienSo == null ? "" : bienSo);
                SqlParameter SoHopDong = new SqlParameter("SoHopDong", soHopDong == null ? "" : soHopDong);
                SqlParameter HoTen = new SqlParameter("HoTen", hoTen == null ? "" : hoTen);
                SqlParameter IsForInsert = new SqlParameter("isForInsert", isForInsert);
                SqlParameter MaPhieuThu = new SqlParameter("MaPhieuThu", maPhieuThu == null ? "" : maPhieuThu);

                list = _dbContext.ExecuteStoredProcedureList<ViewTCOC>("sp_LoadDanhSachCocLenPhieuThu", BienSo, SoHopDong, HoTen, IsForInsert, MaPhieuThu).ToList();

                foreach (var temp in list)
                {
                    List<ChiTietPhieuThu> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuThu == maPhieuThu
                        && x.HinhThucThanhToan == "DC"
                        && x.SoThamChieu == temp.MaPhieuThu
                        && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai + TongSoTien;
                    temp.SoTienDaSuDung = TongSoTien;
                }
                // Lấy cọc còn lại != 0 Lộc 200828
                // thang comment tai day 26/5/2022
                //list = list.Where(x => !(x.ConLai == 0 && x.).ToList();
                return list;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCOC:", ex);
                return null;
            }
        }

        public List<PhieuThu> ListCongNo(string maPhieuThu, string donViNo, bool isForInsert)
        {
            try
            {
                List<PhieuThu> list = new List<PhieuThu>();
                if (isForInsert)
                    list = _phieuThuRepository.Table.Where(x =>x.DoiTac == donViNo && x.MaLoaiPhieu == "TCONO" && x.IsDeleted == false).ToList();
                else
                    list = _phieuThuRepository.Table.Where(x => x.DoiTac == donViNo && x.MaLoaiPhieu == "TCONO" && x.IsDeleted == false).ToList();

                foreach (var temp in list)
                {
                    List<ChiTietPhieuThu> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuThu == maPhieuThu
                        && x.SoThamChieu == temp.MaPhieuThu
                        && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai.Value + TongSoTien;
                    temp.SoTienDaSuDung = TongSoTien;
                }
                return list;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCOC:" ,ex);
                return null;
            }
        }

        /// <summary>
        /// Chi lay nhung phieu chua bo coc nếu là phiếu tạo mới, nếu phiếu edit thì lấy luôn phiếu bỏ cọc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ViewTCODV> GetTCODVByBienSo(string bienSoXe, bool isForInsert,string maPhieuThu ="")
        {
            try
            {
                if (string.IsNullOrEmpty(bienSoXe))
                    bienSoXe = "";
                bienSoXe = bienSoXe.Trim().Replace(".", "");
                if (isForInsert)
                    return _VTCODVRepository.Table.Where(x => x.BienSo == bienSoXe && x.TTPCode != "B" & !string.IsNullOrEmpty( x.BienSo)).ToList();
                else
                {
                    SqlParameter MaPhieu = new SqlParameter("MaPhieuDichVu", maPhieuThu);//xử lý trước khi update
                    SqlParameter BienSoXe = new SqlParameter("BSX", bienSoXe);
                    return _dbContext.ExecuteStoredProcedureList<ViewTCODV>("sp_SelectCocDichVu", BienSoXe, MaPhieu).ToList();
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCODVByBienSo:" ,ex);
             
                return null;
            }
            
        }
        /// <summary>
        /// Lấy các phiếu bảo dưỡng tiết kiệm theo biển số xe + các phiếu BDTK theo số khung
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public List<ChiTietBaoDuongTietKiem> GetNBDTK(string bienSoXe, string soKhung, bool isForInsert, string maPhieuThu = "")
        public List<ViewChiTietBaoDuongTietKiem> GetNBDTK(string bienSoXe, string soKhung, bool isForInsert, string maPhieuThu = "")
        {
            try
            {
                if (string.IsNullOrEmpty(bienSoXe))
                    bienSoXe = "";
                bienSoXe = bienSoXe.Trim().Replace(".", "");
                //if (isForInsert)
                //    return _VNPTraRepository.Table.Where(x => x.BienSo == bienSoXe & !string.IsNullOrEmpty(x.BienSo)).ToList();
                //else
                //{
                SqlParameter MaPhieuDichVu = new SqlParameter("MaPhieuDichVu", string.IsNullOrEmpty(maPhieuThu) ? "" : maPhieuThu);//xử lý trước khi update
                SqlParameter BSX = new SqlParameter("BSX", string.IsNullOrEmpty(bienSoXe) ? "" : bienSoXe);
                SqlParameter SoKhung = new SqlParameter("SoKhung", string.IsNullOrEmpty(soKhung) ? "" : soKhung);
                return _dbContext.ExecuteStoredProcedureList<ViewChiTietBaoDuongTietKiem>("sp_SelectNBDTK", BSX, SoKhung, MaPhieuDichVu).ToList();
                //}
            }   
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNDHTK:", ex);
                return null;
            }

        }
        public List<ViewTBAHI> GetTBAHI(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTBAHIRepository.Table.Where(x => x.MaPhieuThu.Contains("TBAHI") && x.NgayThu >= from && x.NgayThu <= to && !(x.TongCong == 0 && x.SoHopDong != null && x.SoHopDong != ""));
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x =>                        
                        (
                            (x.MaPhieuThu.ToLower().Contains(query)
                            || x.SoChungTu.ToLower().Contains(query)
                            || x.HoTen.ToLower().Contains(query.ToLower())
                            || x.CTBH.ToLower().Contains(query)

                            || query.Contains(x.MaPhieuThu.ToString().ToLower())
                            || query.Contains(x.SoChungTu.ToLower())
                           // || query.Contains(x.HoTen.ToLower())
                            || query.Contains(x.CTBH.ToString().ToLower())
                            )

                        )
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Where(s=>s.MaPhieuThu.Contains("TBAHI")).Count();
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTBAHI:" ,ex);
               
                return null;
            }
            
        }

        public List<ViewTBAHI> GetTBAHITab(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tab)
        {
            try
            {
                query = query.ToLower();
                var q = _VTBAHIRepository.Table.Where(x => x.MaPhieuThu.Contains(tab) && x.NgayThu >= from && x.NgayThu <= to && x.TongCong == 0 && x.SoHopDong != null && x.SoHopDong != "" && x.MaPhieuThu.Contains(tab));
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x =>                                           
                        (x.MaPhieuThu.ToLower().Contains(query)
                        || x.SoChungTu.ToLower().Contains(query)
                        || x.HoTen.ToLower().Contains(query.ToLower())
                        || x.CTBH.ToLower().Contains(query)

                        || query.Contains(x.MaPhieuThu.ToString().ToLower())
                        || query.Contains(x.SoChungTu.ToLower())
                       // || query.Contains(x.HoTen.ToLower())
                        || query.Contains(x.CTBH.ToString().ToLower())
                        )
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTBAHITab:" ,ex);
                if (ex.InnerException != null)
                    _log.WriteLog("PhieuThuService.GetTBAHITab:" + ex.InnerException.ToString());
                return null;
            }
            
        }

        public List<ChiTietBanTaiSan> GetCTBTS(string id)
        {
            try
            {
                return _CTBTSRepository.Table.Where(x => x.MaPhieuThu == id && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetCTBTS:" ,ex);
                if (ex.InnerException != null)
                    _log.WriteLog("PhieuThuService.GetCTBTS:" + ex.InnerException.ToString());
                return null;
            }
        }

        public ChiTietBanTaiSan GetCTBTS(Guid id)
        {
            try
            {
                return _CTBTSRepository.Table.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetCTBTS:" ,ex);
             
                return null;
            }
            
        }

        public bool InsertCTBTS(ChiTietBanTaiSan ctbts)
        {
            try
            {
                _CTBTSRepository.Insert(ctbts);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, ctbts.MaPhieuThu, Convert.ToString(ctbts.MaTaiSan), "INSERT", "Insert Chi tiet ban tai san", "Insert Phieu chi tiet ban tai san", _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.InsertCTBTS:" ,ex);
              
                return false;
            }
        }

        public bool UpdateCTBTS(ChiTietBanTaiSan ctbts)
        {
            try
            {
                _CTBTSRepository.Update(ctbts);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, ctbts.MaPhieuThu, Convert.ToString(ctbts.MaTaiSan), "UPDATE", "Update Chi tiet ban tai san", "Update Phieu chi tiet ban tai san", _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.UpdateCTBTS:" ,ex);
            
                return false;
            }
        }
        public List<ViewTBATS> GetTBATS(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTBATSRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || x.DienThoai.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.TenTS.ToLower().Contains(query)
                    || x.MaPhieuThu.ToLower().Contains(query)
                    || query.Contains(x.MaPhieuThu.ToString().ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                  //  || query.Contains(x.HoTen.ToLower())
                  //  || query.Contains(x.DienThoai.ToString().ToLower())
                    || query.Contains(x.TenTS.ToString().ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTBATS:" ,ex);
          
                return null;
            }
            
        }

        public List<ViewTCOBX> GetTCOBX(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTCOBXRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || x.SoHopDong.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.DienThoai.ToLower().Contains(query)
                    || x.TinhTrangPhieu.ToLower().Contains(query)

                    || query.Contains(x.MaPhieuThu.ToString().ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                   // || query.Contains(x.HoTen.ToLower())
                  //  || query.Contains(x.DienThoai.ToString().ToLower())
                    || query.Contains(x.SoHopDong.ToString().ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCOBX:" ,ex);
               
                return null;
            }
            
        }
        public List<ViewTCOBX> GetTCOBX(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang)
        {
            try
            {
                query = query.ToLower();
                var q = _VTCOBXRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || x.SoHopDong.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.DienThoai.ToLower().Contains(query)

                    || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                    //|| query.Contains(x.HoTen.ToLower())
                   // || query.Contains(x.DienThoai.ToLower())
                    || query.Contains(x.SoHopDong.ToString().ToLower())
                    );
                }
                if (tinhtrang == "H")
                {
                    q = q.Where(x => x.TinhTrangPhieu == tinhtrang);
                }
                else if (tinhtrang == "D")
                {
                    q = q.Where(x => x.TinhTrangPhieu == tinhtrang);
                }
                else if (tinhtrang == "C")
                {
                    q = q.Where(x => x.TinhTrangPhieu == tinhtrang);
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCOBX:" ,ex);
             
                return null;
            }
        }

        public List<ViewTCODV> GetTCODV(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var q = _VTCODVRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                        || x.HoTen.ToLower().Contains(query)
                        || x.BienSo.ToLower().Contains(query)
                        || x.SoChungTu.ToLower().Contains(query)
                         || x.DienThoai.ToLower().Contains(query)
                        || x.TinhTrangPhieu.ToLower().Contains(query.ToLower())

                        || query.Contains(x.MaPhieuThu.ToLower())
                        || query.Contains(x.SoChungTu.ToLower())
                       // || query.Contains(x.HoTen.ToLower())
                       // || query.Contains(x.DienThoai.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                        );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCODV:" ,ex);
              
                return null;
            }
            
        }
        public List<ViewTCODV> GetTCODV(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang)
        {
            try
            {
                query = query.ToLower();
                var q = _VTCODVRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                        || x.HoTen.ToLower().Contains(query)
                        || x.BienSo.ToLower().Contains(query)
                        || x.SoChungTu.ToLower().Contains(query)
                         || x.DienThoai.ToLower().Contains(query)
                        || x.TinhTrangPhieu.ToLower().Contains(query.ToLower())

                        || query.Contains(x.MaPhieuThu.ToLower())
                        || query.Contains(x.SoChungTu.ToLower())
                       // || query.Contains(x.HoTen.ToLower())
                       // || query.Contains(x.DienThoai.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                        );
                }
                if (tinhtrang == "H")
                {
                    q = q.Where(x => x.TinhTrangPhieu == tinhtrang);
                }
                else if (tinhtrang == "D")
                {
                    q = q.Where(x => x.TinhTrangPhieu == tinhtrang);
                }
                else if (tinhtrang == "C")
                {
                    q = q.Where(x => x.TinhTrangPhieu == tinhtrang);
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCODV:" ,ex);
              
                return null;
            }
        }

        public List<ViewTCOBX> GetTCOBXBySHD(string shd, bool isForInsert)
        {
            try
            {
                shd = shd.Trim().Replace(".", "");
                if (isForInsert)
                    return _VTCOBXRepository.Table.Where(x => x.SoHopDong == shd && x.TTPCode != "B").ToList();
                else
                    return _VTCOBXRepository.Table.Where(x => x.SoHopDong == shd).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCOBXBySHD:" ,ex);
             
                return null;
            }
            
        }

        public DataTable GetListCTPDVTCybers(DateTime fromDate, DateTime toDate, string query)
        {
            try
            {
                SqlParameter strQuery = new SqlParameter("Query", query);
                SqlParameter fromdate = new SqlParameter("Fromdate", fromDate);
                SqlParameter todate = new SqlParameter("Todate", toDate);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_getListDetailPhieuTamDVCyberSoft", fromdate, todate, strQuery);
                
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetListCTPDVTCybers:", ex);

                return null;
            }
        }
        public List<CTPDVTamCyber> GetCTPDVTCybers(string id)
        {
            try
            {
                if (id == null)
                    id = "";
                SqlParameter strQuery = new SqlParameter("id", id);
                var result = _dbContext.ExecuteStoredProcedureObjs<CTPDVTamCyber>("sp_getDetailsPhieuTamDVCyberSoft", strQuery).ToList();
                return result;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetPDVT:", ex);

                return null;
            }
        }
        public PhieuDVTamCyber GetPDVTCybers(string id)
        {
            try
            {
                SqlParameter strQuery = new SqlParameter("Query", id);
                SqlParameter fromdate = new SqlParameter("Fromdate", DBNull.Value);
                SqlParameter todate = new SqlParameter("Todate", DBNull.Value);
                var q = _dbContext.ExecuteStoredProcedureObjs<PhieuDVTamCyber>("sp_getListPhieuTamDVCyberSoft", fromdate, todate, strQuery).ToList().FirstOrDefault();
                return q;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetPDVT:", ex);

                return null;
            }
        }
        public List<PhieuDVTamCyber> GetPDVT(DateTime from, DateTime to, int p, ref int total, int pageSize, string query)
        {
            try
            {
                query = query.ToLower();
                //var q = _VPDVTRepository.Table.Where(x => x.RO_DATE >= from && x.RO_DATE <= to);
                SqlParameter fromdate = new SqlParameter("Fromdate", from);
                SqlParameter todate = new SqlParameter("Todate", to);
                SqlParameter strQuery = new SqlParameter("Query", query);
                var q = _dbContext.ExecuteStoredProcedureObjs<PhieuDVTamCyber>("sp_getListPhieuTamDVCyberSoft", fromdate, todate, strQuery).ToList();
                var result = q.OrderByDescending(x => x.RO_DATE).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetPDVT:", ex);

                return null;
            }
        }
        public DataTable GetPDVT_Datable(DateTime from, DateTime to,string query)
        {
            try
            {
                query = query.ToLower();
                //var q = _VPDVTRepository.Table.Where(x => x.RO_DATE >= from && x.RO_DATE <= to);
                SqlParameter fromdate = new SqlParameter("Fromdate", from);
                SqlParameter todate = new SqlParameter("Todate", to);
                SqlParameter strQuery = new SqlParameter("Query", query);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_getListPhieuTamDVCyberSoft", fromdate, todate, strQuery);             
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetPDVT:", ex);

                return null;
            }
        }
        public List<REPAIR_ORDER_PYS> GetPDVT(string id)
        {            
            try
            {
                if (id == null)
                    id = "";
                SqlParameter ID = new SqlParameter("RepairOrderNo", id);
                ID.Size = 100;
                return _dbContext.ExecuteStoredProcedureList<REPAIR_ORDER_PYS>("sp_SelectREPAIR_ORDER_PYS", ID).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetPDVT:" ,ex);
              
                return null;
            }
        }
        #region chi tiết phiếu Thu
        public bool CreateChiTietPhieuThu(string maPhieuThu, List<ChiTietPhieuThu> httt)
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
                            item.MaPhieuThu = maPhieuThu;
                            item.HinhThucThanhToan = a[1];
                            item.MaNganHang = a[0];
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = uid;
                            item.CreatedDate = d;
                            item.UpdatedDate = d;
                            item.UpdatedBy = uid;
                          
                            _ctptRepository.Insert(item);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.CreateChiTietPhieuThu: " ,ex);
            
                return false;
            }
        }
        public List<ChiTietPhieuThu> GetCTPTByBienSo(string bienso)
        {
            try
            {
                SqlParameter bs = new SqlParameter("bienso", bienso);
                return _dbContext.ExecuteStoredProcedureList<ChiTietPhieuThu>("GetCTPTByBienSo", bs).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetCTPTByBienSo: " ,ex);
              
                return null;
            }
            
        }
        public List<ChiTietPhieuThu> GetCTPT(string id)
        {
            try
            {
                return _ctptRepository.Table.Where(x => x.MaPhieuThu == id && x.IsDeleted == false && x.IsActive == true).OrderBy(x => x.CreatedDate).ToList().Select(x => new ChiTietPhieuThu
                {
                    Id = x.Id,
                    TinhTrang = x.TinhTrang,
                    //Temp = x.MaNganHang.ToUpper() + "-" + x.HinhThucThanhToan!=null? x.HinhThucThanhToan:"",
                    Temp = x.MaNganHang.ToUpper() + "-" + x.HinhThucThanhToan,
                    HinhThucThanhToan = x.HinhThucThanhToan != null ? x.HinhThucThanhToan : "",
                    MaPhieuThu = x.MaPhieuThu,
                    MaNganHang = x.MaNganHang != null ? x.MaNganHang : "",
                    SoThamChieu = x.SoThamChieu != null ? x.SoThamChieu : "",
                    SoTienThanhToan = x.SoTienThanhToan,
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetCTPT: " ,ex);
                
                return null;
            }
            
        }
        public List<ChiTietPhieuThu> GetCTPT_BHDV(string id)
        {
            try
            {
                return _ctptRepository.Table.Where(x => x.MaPhieuThu == id && x.IsDeleted == false && x.IsActive == true && x.HinhThucThanhToan != "DC").OrderBy(x => x.CreatedDate).ToList().Select(x => new ChiTietPhieuThu
                {
                    Id = x.Id,
                    TinhTrang = x.TinhTrang,
                    Temp = x.MaNganHang + "-" + x.HinhThucThanhToan,
                    HinhThucThanhToan = x.HinhThucThanhToan,
                    MaPhieuThu = x.MaPhieuThu,
                    MaNganHang = x.MaNganHang,
                    SoThamChieu = x.SoThamChieu,
                    SoTienThanhToan = x.SoTienThanhToan
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetCTPT: " ,ex);
              
                return null;
            }
        }
        public List<ChiTietPhieuThu> GetCTPT_CongNo(string id)
        {
            try
            {
                return _ctptRepository.Table.Where(x => x.MaPhieuThu == id && x.IsDeleted == false && x.IsActive == true && x.HinhThucThanhToan == "DC").OrderBy(x => x.CreatedDate).ToList().Select(x => new ChiTietPhieuThu
                {
                    Id = x.Id,
                    TinhTrang = x.TinhTrang,
                    Temp = x.MaNganHang + "-" + x.HinhThucThanhToan,
                    HinhThucThanhToan = x.HinhThucThanhToan,
                    MaPhieuThu = x.MaPhieuThu,
                    MaNganHang = x.MaNganHang,
                    SoThamChieu = x.SoThamChieu,
                    SoTienThanhToan = x.SoTienThanhToan
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetCTPT: " ,ex);
              
                return null;
            }
        }
        public bool ChangeTreoTien(Guid id, bool check)
        {
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var obj = _ctptRepository.Table.FirstOrDefault(x => x.Id == id);
                obj.TinhTrang = check;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                _ctptRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, Convert.ToString(id), Convert.ToString(id), "ChangeTreoTien", "ChangeTreoTien", "ChangeTreoTien", _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.ChangeTreoTien: " ,ex);
               
                return false;
            }
        }
        public bool SaveTienTra(Guid id, string ngaytientra, string maPhieuThu)
        {
            try
            {
                if (maPhieuThu.Substring(0, 5).ToUpper() != "CTIEN") //neu khong phai chuyen tien noi bo thi update ChiTietPhieuThu
                {
                    DateTime d = DateTime.Now;
                    var uid = _authenticationService.GetAuthenticatedUser().UserId;
                    var obj = _ctptRepository.Table.FirstOrDefault(x => x.Id == id);
                    if (obj == null)
                        return false;
                    obj.TinhTrang = string.IsNullOrEmpty(ngaytientra) ? true : false;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    if (!string.IsNullOrEmpty(ngaytientra))
                        obj.NgayTienVao = DateTime.Parse(ngaytientra);
                    else
                        obj.NgayTienVao = null;
                    _ctptRepository.Update(obj);

                }
                else //neu la phieu chuyen tien noi bo thi update NgayTienVao tren ChitietChuyentienNoiBo
                {
                    DateTime d = DateTime.Now;
                    var uid = _authenticationService.GetAuthenticatedUser().UserId;
                    var obj = _ctPCTNBRepositority.Table.FirstOrDefault(x => x.Id == id);
                    if (obj == null)
                        return false;
                    obj.TinhTrang = string.IsNullOrEmpty(ngaytientra) ? true : false;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    if (!string.IsNullOrEmpty(ngaytientra))
                        obj.NgayTienVao = DateTime.Parse(ngaytientra);
                    else
                        obj.NgayTienVao = null;
                    _ctPCTNBRepositority.Update(obj);
                }
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, Convert.ToString(id), Convert.ToString(id), "SaveTienTra", "SaveTienTra", "SaveTienTra", _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.SaveTienTra: " ,ex);
             
                return false;
            }
        }
        public bool UpdateCTPT(ChiTietPhieuThu model,PhieuThu data)
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
                obj.NgayTreoTien = data.NgayThu;
                _ctptRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuThu, model.MaNganHang, "Update", "UpdateCTPT", "UpdateCTPT", _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.UpdateCTPT: " ,ex);
               
                return false;
            }
        }
        public bool UpdateCTPT(List<ChiTietPhieuThu> lst,PhieuThu data)
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
                        obj.NgayTreoTien = data.NgayThu;
                        _ctptRepository.Update(obj);
                        _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, item.MaPhieuThu, item.MaNganHang, "Update", "Update list CTPT", "Update list CTPT", _authenticationService.GetAuthenticatedUser().UserId);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.UpdateCTPT: " ,ex);
               
                return false;
            }
        }
        public bool InsertCTPT(ChiTietPhieuThu info)
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
                
                _ctptRepository.Insert(info);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, info.MaPhieuThu, info.MaNganHang, "Insert", "Insert CTPT", "Insert CTPT", _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.InsertCTPT: " ,ex);
           
                return false;
            }
        }
        #endregion
        #region Phiếu Thu
        public PhieuThu GetPhieuThu(string id)
        {
            try
            {
                
                return _phieuThuRepository.Table.SingleOrDefault(x => x.MaPhieuThu == id && x.IsDeleted == false);//&& x.IsActive == true);
            }
            catch (Exception ex)
            {
              
                    _log.WriteLog("PhieuThuService.GetPhieuThu: " ,ex);
                return null;
            }
            
        }
        public ViewPhieuThu GetViewPhieuThu(string id, string type)
        {
            try
            {
                return _ViewphieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == id  && x.IsDeleted == false );

            }catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetViewPhieuThu: ", ex);
                return null;
            }
        }
        public bool CreatePhieuThu(PhieuThu model)
        {
            try
            {
                SqlParameter MPT1 = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi insert
                SqlParameter message = new SqlParameter("Message", "");
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                SqlParameter SoHopDong = new SqlParameter("SoHopDong", model.SoHopDong);
                SqlParameter BienSo = new SqlParameter("BienSo", model.BienSo);
                SqlParameter TongCong = new SqlParameter("TongCong", model.TongCong);
                SqlParameter SoTienThu = new SqlParameter("SoTienThu", model.SoTienThu);
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay gia trị trả về trước khi insert
              
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuThu", MPT1, Ngay, message,SoHopDong,BienSo,TongCong,SoTienThu);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return false;

                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuThu = _commonService.CreateId(model.MaLoaiPhieu);
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                model.CreatedDate = d;
                if(model.MaLoaiPhieu != "NHHHB")
                {
                    model.BienSo = model.BienSo.Replace(" ", "").Replace(".", "");
                    model.SoHopDong = model.SoHopDong.Replace(" ", "").Replace(".", "");
                }
                model.MaPhieuThu = _commonService.CreateId(model.MaLoaiPhieu);
                //  if (model.NgayHachToan == null)
                model.NgayHachToan = model.NgayThu;

                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                _phieuThuRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuThu, model.MaLoaiPhieu, "Create", "CreatePhieuThu", "CreatePhieuThu", _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch (Exception ex)
            {
               
                    _log.WriteLog("PhieuThuService.PhieuThuService.CreatePhieuThu(PhieuThu model): " ,ex);
                
                return false;
            }
        }

        /// <summary>
        /// Dung cho dong bo Cyber
        /// </summary>
        /// <param name="model"></param>
        /// <param name="httt"></param>
        /// <param name="isGoiXuLySauKhiInsertPhieuThu"></param>
        /// <param name="giaTriTrenGiaoDien"></param>
        /// <param name="isXuLyTinhToanTuiTien"></param>
        /// <returns></returns>
        public string CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt
            , bool isGoiXuLySauKhiInsertPhieuThu = true
            , string giaTriTrenGiaoDien = ""
            , bool isXuLyTinhToanTuiTien = true)
        {
            //if(giaTriTrenGiaoDien != "Import")
            //    _dbContext.BeginTransaction();
            try
            {
                //kiem tra ngay ThuCoc co NgayThuPhieu dich vu/ban xe... khong
                string kiemTraDanhSachCoc = KiemTraDanhSachCocTruocKhiInsert(model.MaLoaiPhieu, model, model.NgayThu, httt);
                if (!string.IsNullOrEmpty(kiemTraDanhSachCoc))
                    return kiemTraDanhSachCoc;

                if (model.MaLoaiPhieu == "TBAHITemp")
                    model.MaLoaiPhieu = "TBAHI";
                //kiểm tra trước khi insert Phiếu Thu
                SqlParameter MPT1 = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi insert 
                SqlParameter message = new SqlParameter("Message", "");
                SqlParameter Ngay = new SqlParameter("NgayThu", model.NgayThu);
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay gia trị trả về trước khi insert
                if (model.MaLoaiPhieu == "TDIVU")
                {
                    PhieuThu PT = _phieuThuRepository.Table.Where(x => x.SoHopDong == model.SoHopDong && x.IsDeleted == false && x.MaLoaiPhieu == "TDIVU").FirstOrDefault();
                    if (PT != null)
                    {
                        message.Value = "Phiếu này đã tồn tại.";
                        return message.Value.ToString();
                    }
                }
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuThu", MPT1, message, Ngay);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuThu = _commonService.CreateId(model.MaLoaiPhieu);
                //if (model.NgayHachToan == null)
                model.NgayHachToan = model.NgayThu;
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                if (model.BienSo != null)
                {
                    model.BienSo = model.BienSo.Replace(" ", "").Replace(".", "");
                }
                if (model.SoHopDong != null)
                {
                    model.SoHopDong = model.SoHopDong.Replace(" ", "");//.Replace(".", "");
                }
                //xu ly tong so tien thu duoi database
                //model.SoTienThu = 0;
                double soTien = 0;
                foreach (var item in httt)
                {
                    if (!string.IsNullOrEmpty(item.Temp) && item.IsDeleted == false)
                    {
                        var a = item.Temp.Split('-');
                        if (a.Length == 2)
                        {
                            item.Id = Guid.NewGuid();
                            item.MaPhieuThu = model.MaPhieuThu;
                            item.HinhThucThanhToan = a[1];
                            item.MaNganHang = a[0];
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = uid;
                            item.CreatedDate = DateTime.Now;
                            item.UpdatedDate = d;
                            item.UpdatedBy = uid;
                            if (item.TinhTrang) //neu phieu bi treo thi ghi nhan ngay treo
                                item.NgayTreoTien = model.NgayThu;
                            //else if (item.NgayTienVao == null) //neu phieu nay truoc do chua bi treo tien hoặc chưa bị chuyển tình trạng hết treo   thi cap nhat ngay vao tien la ngay thu
                            //    item.NgayTienVao = model.NgayThu;
                            //if (giaTriTrenGiaoDien == "Import")
                            //    _dbContext.BeginTransaction();
                            _ctptRepository.Insert(item);
                            //_dbContext.CommitTransaction();

                            //       model.SoTienThu += item.SoTienThanhToan;
                            soTien += item.SoTienThanhToan;
                        }
                    }
                }

                if (model.TongCong == 0)
                    model.TongCong = soTien;
                model.SoTienThu = soTien;
                if (model.MaLoaiPhieu == "TCOBX"
                   || model.MaLoaiPhieu == "TCODV"
                   || model.MaLoaiPhieu == "TCONO")
                    model.ConLai = model.TongCong;
                if (model.MaLoaiPhieu == "TGOVO")
                {
                    model.TongCong = model.SoTienThu;
                }
                _phieuThuRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuThu, model.MaLoaiPhieu, "Create", "CreatePhieuThu", "Mã phiếu thu :" + model.MaPhieuThu + " mã loại phiếu :" + model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuThu)
                    _sothuService.XuLySauKhiInsertPhieuThu(model.MaPhieuThu, giaTriTrenGiaoDien, "", "");
                //_dbContext.CommitTransaction();
                //_loggingService.RemoveCache(model.MaLoaiPhieu,"");
                return "";
            }
            catch (Exception ex)
            {

                //_dbContext.RollbackTransaction();
                _log.WriteLog("PhieuThuService.PhieuThuService.CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt): ", ex);

                return "Lỗi tạo mới phiếu thu";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chuoiPhieuNo">Theo dinh dang gom cac chuoi: MaPhieuNo_UpdatedDate(yyyyMMddHHmmss). 
        /// Ví dụ: NBAXE1710010001_20171009081000;NBAXE1710010002_20171009081005;NBAXE171001000120171009081009
        /// Khi lay du lieu len thi lay thong tin ma phieu no va UpdatedDate dua vao 1 chuoi, khi luu xuong thi truyen chuoi nay xuong xem co bi thay doi ko
        /// </param>
        /// <returns></returns>
        public string LayNgayUpdateCuaCacPhieuNo(string lstMaPhieuNo)
        {
            SqlParameter paramLstMaPhieuNo = new SqlParameter("lstMaPhieuNo", lstMaPhieuNo);//xử lý trước khi update
            SqlParameter paramKetQua = new SqlParameter("ketQua", "");
            paramKetQua.Direction = System.Data.ParameterDirection.Output;
            paramKetQua.Size = 40000;

            _dbContext.ExecuteStoredProcedure("sp_LayChuoiUpdatedDatePhieuNo", paramLstMaPhieuNo, paramKetQua);
            return paramKetQua.Value.ToString();
        }
        public string CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt
            , bool isGoiXuLySauKhiInsertPhieuThu = true
            ,string giaTriTrenGiaoDien = "" 
            )
        {
            try
            {
                //kiem tra ngay ThuCoc co NgayThuPhieu dich vu/ban xe... khong
                string kiemTraDanhSachCoc = KiemTraDanhSachCocTruocKhiInsert(model.MaLoaiPhieu, model, model.NgayThu, httt);
                if (!string.IsNullOrEmpty(kiemTraDanhSachCoc))
                    return kiemTraDanhSachCoc;

                if (model.MaLoaiPhieu == "TBAHITemp")
                    model.MaLoaiPhieu = "TBAHI";
                //kiểm tra trước khi insert Phiếu Thu
                SqlParameter MPT1 = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi insert
                SqlParameter message = new SqlParameter("Message", "");
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                SqlParameter SoHopDong = new SqlParameter("SoHopDong", string.IsNullOrEmpty(model.SoHopDong)?"":model.SoHopDong);
                SqlParameter BienSo = new SqlParameter("BienSo", string.IsNullOrEmpty(model.BienSo)?"":model.BienSo);
                SqlParameter TongCong = new SqlParameter("TongCong", model.TongCong);
                SqlParameter SoTienThu = new SqlParameter("SoTienThu", model.SoTienThu);
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay gia trị trả về trước khi insert
                if (model.MaLoaiPhieu == "TDIVU" && model.SoHopDong != null)
                {
                    PhieuThu PT = _phieuThuRepository.Table.Where(x => x.SoHopDong == model.SoHopDong && x.IsDeleted == false && x.MaLoaiPhieu == "TDIVU").FirstOrDefault();
                    if (PT != null)
                    {
                        message.Value = "Mã phiếu này đã tồn tại.";
                        return message.Value.ToString();
                    }
                }
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuThu", MPT1, Ngay, message, SoHopDong, BienSo, TongCong, SoTienThu);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaLoaiPhieu = model.MaLoaiPhieu == "TBDTKKM" ? "TBDTK" : model.MaLoaiPhieu;
                model.MaPhieuThu = _commonService.CreateId(model.MaLoaiPhieu);
                //if (model.NgayHachToan == null)
                model.NgayHachToan = model.NgayThu;
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                if(model.BienSo != null)
                {
                    model.BienSo = model.BienSo.Replace(" ", "").Replace(".", "");
                }
                if(model.SoHopDong != null)
                {
                    model.SoHopDong = model.SoHopDong.Replace(" ", "").Replace(".", "");
                }
                //xu ly tong so tien thu duoi database
                //model.SoTienThu = 0;
                double soTien = 0;
                foreach (var item in httt)
                {
                    if (!string.IsNullOrEmpty(item.Temp) && item.IsDeleted == false)
                    {
                        var a = item.Temp.Split('-');
                        if (a.Length == 2)
                        {
                            item.Id = Guid.NewGuid();
                            item.MaPhieuThu = model.MaPhieuThu;
                            item.HinhThucThanhToan = a[1];
                            item.MaNganHang = a[0];
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = uid;
                            item.CreatedDate = DateTime.Now;
                            item.UpdatedDate = d;
                            item.UpdatedBy = uid;
                            if (item.TinhTrang) //neu phieu bi treo thi ghi nhan ngay treo
                                item.NgayTreoTien = model.NgayThu;
                            //else if (item.NgayTienVao == null) //neu phieu nay truoc do chua bi treo tien hoặc chưa bị chuyển tình trạng hết treo   thi cap nhat ngay vao tien la ngay thu
                            //    item.NgayTienVao = model.NgayThu;
                            _ctptRepository.Insert(item);
                            //       model.SoTienThu += item.SoTienThanhToan;
                            soTien += item.SoTienThanhToan;
                        }
                    }
                }

                if (model.TongCong == 0)
                    model.TongCong = soTien;
                model.SoTienThu = soTien;
                if (model.MaLoaiPhieu == "TCOBX"
                   || model.MaLoaiPhieu == "TCODV"
                   || model.MaLoaiPhieu == "TCONO")
                    model.ConLai = model.TongCong;
                if (model.MaLoaiPhieu == "TGOVO")
                {
                    model.TongCong = model.SoTienThu;
                }
                if (model.MaLoaiPhieu == "TGOVO")
                {
                    model.TongCong = model.SoTienThu;
                }
                //if (model.MaLoaiPhieu == "TBDTK")
                //{
                //    ViewGoiBDTK BDTK = GetListGBDTK().SingleOrDefault(x => x.Ma == model.LoaiBaoHiem);
                //    model.GiaBan = BDTK == null ? model.GiaBan : BDTK.GiaBan;
                //}
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                //model.GhiChu = model.GhiChu + "(import)";
                if (!string.IsNullOrWhiteSpace(model.SoKhung))
                    model.SoKhung = model.SoKhung.Trim();
                _phieuThuRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuThu, model.MaLoaiPhieu, "Create", "CreatePhieuThu", "Mã phiếu thu :" + model.MaPhieuThu + " mã loại phiếu :" +model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuThu)
                {
                    _sothuService.XuLySauKhiInsertPhieuThu(model.MaPhieuThu, giaTriTrenGiaoDien);
                }
                else if(model.MaLoaiPhieu == "TDIVU")//neu day la import phieu dich vu (khong goi xu ly sau update) thi phai xu ly phieu dich vu
                {
                    SqlParameter pIsDeleted = new SqlParameter("isDeleted", SqlDbType.Bit);
                    pIsDeleted.SqlValue = 0;
                    SqlParameter pMaPhieuThu = new SqlParameter("MaPhieuThu", model.MaPhieuThu);
                    SqlParameter pMessage = new SqlParameter("Message", "");
                    pMessage.Direction = ParameterDirection.Output;
                    SqlParameter pGoiXuLyTinhToanTuiTien = new SqlParameter("goiXuLyTinhToanTuiTien", SqlDbType.Bit);
                    pGoiXuLyTinhToanTuiTien.SqlValue = 0;
                    SqlParameter pSessionId = new SqlParameter("SessionId", "");
                    _dbContext.ExecuteStoredProcedure("sp_XuLyPhieuThuDichVu", pMaPhieuThu, pIsDeleted,pMessage,pSessionId,pGoiXuLyTinhToanTuiTien);
                }
                return "";
            }
            catch (Exception ex)
            {
              
                _log.WriteLog("PhieuThuService.PhieuThuService.CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt): " ,ex);
                return "Lỗi tạo mới phiếu thu";
            }
        }
     
        public string CreatePhieuThu_ThuNo(PhieuThu model, List<ChiTietPhieuThu> httt, List<ChiTietPhieuThu> Coc
            , bool isGoiXuLySauKhiInsertPhieuThu = true
            , string giaTriTrenGiaoDien = ""
            )
        {
            try
            {
                //kiem tra ngay ThuCoc co NgayThuPhieu dich vu/ban xe... khong
                string kiemTraDanhSachCoc = KiemTraDanhSachCocTruocKhiInsert(model.MaLoaiPhieu, model, model.NgayThu, httt);
                if (!string.IsNullOrEmpty(kiemTraDanhSachCoc))
                    return kiemTraDanhSachCoc;

                //kiểm tra trước khi insert Phiếu Thu
                SqlParameter MPT1 = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi insert
                SqlParameter message = new SqlParameter("Message", "");
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                SqlParameter SoHopDong = new SqlParameter("SoHopDong", string.IsNullOrEmpty(model.SoHopDong) ? "" : model.SoHopDong);
                SqlParameter BienSo = new SqlParameter("BienSo", string.IsNullOrEmpty(model.BienSo) ? "" : model.BienSo);
                SqlParameter TongCong = new SqlParameter("TongCong", model.TongCong);
                SqlParameter SoTienThu = new SqlParameter("SoTienThu", model.SoTienThu);
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay gia trị trả về trước khi insert
               
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuThu", MPT1, Ngay, message, SoHopDong, BienSo, TongCong, SoTienThu);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuThu = _commonService.CreateId(model.MaLoaiPhieu);
                //if (model.NgayHachToan == null)
                model.NgayHachToan = model.NgayThu;
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                if (model.BienSo != null)
                {
                    model.BienSo = model.BienSo.Replace(" ", "").Replace(".", "");
                }
                if (model.SoHopDong != null)
                {
                    model.SoHopDong = model.SoHopDong.Replace(" ", "").Replace(".", "");
                }
                //xu ly tong so tien thu duoi database
                //model.SoTienThu = 0;
                double soTien = 0;
                foreach (var item in httt)
                {
                    if (!string.IsNullOrEmpty(item.Temp) && item.IsDeleted == false)
                    {
                        var a = item.Temp.Split('-');
                        if (a.Length == 2)
                        {
                            item.Id = Guid.NewGuid();
                            item.MaPhieuThu = model.MaPhieuThu;
                            item.HinhThucThanhToan = a[1];
                            item.MaNganHang = a[0];
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = uid;
                            item.CreatedDate = DateTime.Now;
                            item.UpdatedDate = d;
                            item.UpdatedBy = uid;
                            if (item.TinhTrang) //neu phieu bi treo thi ghi nhan ngay treo
                                item.NgayTreoTien = model.NgayThu;
                            //else if (item.NgayTienVao == null) //neu phieu nay truoc do chua bi treo tien hoặc chưa bị chuyển tình trạng hết treo   thi cap nhat ngay vao tien la ngay thu
                            //    item.NgayTienVao = model.NgayThu;
                            _ctptRepository.Insert(item);
                            //       model.SoTienThu += item.SoTienThanhToan;
                            soTien += item.SoTienThanhToan;
                        }
                    }
                }
                if(Coc != null)
                {
                    foreach(var c in Coc)
                    {
                        c.Id = Guid.NewGuid();
                        c.MaPhieuThu = model.MaPhieuThu;
                        c.HinhThucThanhToan = "DC";
                        c.MaNganHang = "NBHDV";
                        c.GhiChu = "Công nợ BHDV";
                        c.TinhTrang = false;
                        c.IsActive = true;
                        c.IsDeleted = false;
                        c.CreatedBy = uid;
                        c.UpdatedBy = uid;
                        c.CreatedDate = d;
                        c.UpdatedDate = d;
                        _ctptRepository.Insert(c);
                        soTien += c.SoTienThanhToan;
                    }
                }
                if (model.TongCong == 0)
                    model.TongCong = soTien;
                model.SoTienThu = soTien;
                if (model.MaLoaiPhieu == "TCOBX"
                   || model.MaLoaiPhieu == "TCODV"
                   || model.MaLoaiPhieu == "TCONO")
                    model.ConLai = model.TongCong;

                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                _phieuThuRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuThu, model.MaLoaiPhieu, "Create", "CreatePhieuThu", "Mã phiếu thu :" + model.MaPhieuThu + " mã loại phiếu :" + model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuThu)
                    _sothuService.XuLySauKhiInsertPhieuThu(model.MaPhieuThu, giaTriTrenGiaoDien);
                return "";
            }
            catch (Exception ex)
            {
               
                    _log.WriteLog("PhieuThuService.PhieuThuService.CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt): " ,ex);

                return "Lỗi tạo mới phiếu thu";
            }
        }
        public string KiemTraCocTruocKhiUpdatePhieuThu(List<ChiTietPhieuThu> lstCoc, string maPhieuThu)
        {
            if (lstCoc != null)
            {
                foreach (ChiTietPhieuThu obj in lstCoc)
                {
                    SqlParameter MPT1 = new SqlParameter("MaPhieuThuBanXe_DichVu", maPhieuThu);//xử lý trước khi update
                    SqlParameter Message = new SqlParameter("Message", "");
                    Message.Size = 4000;
                    Message.Direction = System.Data.ParameterDirection.Output;
                    SqlParameter MaPhieuCoc = new SqlParameter("maPhieuCoc", obj.SoThamChieu);//xử lý trước khi update
                    MaPhieuCoc.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter SoTienSuDungCoc = new SqlParameter("soTienSuDungCoc", obj.SoTienThanhToan);
                    SoTienSuDungCoc.Direction = System.Data.ParameterDirection.Input;
                    _dbContext.ExecuteStoredProcedure("sp_KiemTraCocTruocKhiUpdatePhieuThu", MPT1, Message, MaPhieuCoc, SoTienSuDungCoc);
                    if (Message.Value != null && !string.IsNullOrEmpty(Message.Value.ToString()))
                        return Message.Value.ToString();
                }
            }
            return "";
        }
        public string KiemTraRangBuocTruocKhiUpdatePhieuThuBanXe(PhieuThu phieuThu,NoPhaiThu noBanXe, NoPhaiThu noNganHang, List<ChiTietPhieuThu> lstCoc, List<ChiTietKhuyenMai> lstCTKM)
        {
            try
            {
                if (noBanXe.SoTienDaTra == 0)
                    if (_VNPTRepository.Table.Where(x => x.MaPhieuNo == noBanXe.MaPhieuNo && x.SoTienDaTra > 0).ToList().Count > 0)
                        return "Đã có phiếu trả nợ bán xe";
                if (noNganHang.SoTienDaTra == 0)
                    if (_VNPTRepository.Table.Where(x => x.MaPhieuNo == noNganHang.MaPhieuNo && x.SoTienDaTra > 0).ToList().Count > 0)
                        return "Đã có phiếu giải ngân từ ngân hàng";
                string kiemTraRangBuocNgayDatCoc = KiemTraDanhSachCocTruocKhiInsert(phieuThu.MaLoaiPhieu, phieuThu, phieuThu.NgayThu, lstCoc);
                //kiem tra rang buoc ngay dat coc
                if (!string.IsNullOrEmpty(kiemTraRangBuocNgayDatCoc))
                    return kiemTraRangBuocNgayDatCoc;
                foreach (ChiTietKhuyenMai ctkm in lstCTKM)
                {
                    if (ctkm.MaPhieuThu != null)
                    {
                        if (ctkm.NguonKhuyenMai == "HH" && !string.IsNullOrEmpty(ctkm.MaPhieuNoHHBX))
                        {
                            //neu co phat sinh phieu chi no hoa hong ban xe thi khong cho luu tiep
                            if (_NPTRepository.Table.Where(x => x.MaPhieuNo == ctkm.MaPhieuNoHHBX
                                                            && x.SoTienDaTra > 0
                                                            && x.SoTienNo != ctkm.GiaVon).ToList().Count > 0)
                            {
                                return "Đã có phiếu chi hoa hồng bán xe";
                            }
                        }
                        if (ctkm.NguonKhuyenMai.Contains("BDTK") && !string.IsNullOrEmpty(ctkm.MaPhieuNoHHBX))
                        {
                            //neu co phat sinh phieu chi hoac su dung goi BDTK thi khong cho luu tiep
                            if (_NPTRepository.Table.Where(x => x.MaPhieuNo == ctkm.MaPhieuNoHHBX
                                                            && x.SoTienDaTra > 0
                                                            && x.SoTienNo != ctkm.GiaVon).ToList().Count > 0)
                            {
                                return "Đã sử dụng gói Bảo dưỡng tiết kiệm";
                            }
                        }
                    }
                }

                //Kiem tra truoc khi update phieu thu
                SqlParameter MPT = new SqlParameter("MaPhieuThu", phieuThu.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", phieuThu.NgayThu);//xử lý trước khi update
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuThu", MPT, Ngay,sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();

                if (lstCoc != null)
                {
                    foreach (ChiTietPhieuThu obj in lstCoc)
                    {
                        SqlParameter MPT1 = new SqlParameter("MaPhieuThuBanXe_DichVu", phieuThu.MaPhieuThu);//xử lý trước khi update
                        SqlParameter Message = new SqlParameter("Message", "");
                        Message.Size = 4000;
                        Message.Direction = System.Data.ParameterDirection.Output;
                        SqlParameter MaPhieuCoc = new SqlParameter("maPhieuCoc", obj.SoThamChieu);//xử lý trước khi update
                        MaPhieuCoc.Direction = System.Data.ParameterDirection.Input;
                        SqlParameter SoTienSuDungCoc = new SqlParameter("soTienSuDungCoc", obj.SoTienThanhToan);
                        SoTienSuDungCoc.Direction = System.Data.ParameterDirection.Input;
                        _dbContext.ExecuteStoredProcedure("sp_KiemTraCocTruocKhiUpdatePhieuThu", MPT1, Message, MaPhieuCoc, SoTienSuDungCoc);
                        if (Message.Value != null && !string.IsNullOrEmpty(Message.Value.ToString()))
                            return Message.Value.ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                
                    _log.WriteLog("PhieuThuService.KiemTraRangBuocTruocKhiUpdatePhieuThuBanXe: " ,ex);
               
                return null;
            }
            
        }


        public string KiemTraRangBuocTruocKhiUpdatePhieuThuDichVu(PhieuThu phieuThu, NoPhaiThu noDichVu, NoPhaiThu noBaoHiem, List<ChiTietPhieuThu> lstCoc, NoPhaiTra noHHTX, NoPhaiTra noGCN)
        {
            try
            {
                if (noDichVu.SoTienDaTra == 0)
                    if (_VNPTRepository.Table.Where(x => x.MaPhieuNo == noDichVu.MaPhieuNo && x.SoTienDaTra > 0).ToList().Count > 0)
                        return "Đã có phiếu trả nợ dịch vụ.";
                if (noBaoHiem.SoTienDaTra == 0)
                    if (_VNPTRepository.Table.Where(x => x.MaPhieuNo == noBaoHiem.MaPhieuNo && x.SoTienDaTra > 0).ToList().Count > 0)
                        return "Đã có phiếu giải ngân từ công ty bảo hiểm";
                if (noHHTX.SoTienDaTra == 0)
                    if (_NPTRepository.Table.Where(x => x.MaPhieuNo == noHHTX.MaPhieuNo && x.SoTienDaTra > 0).ToList().Count > 0)
                        return "Đã có phiếu trả nợ hoa hồng tài xế";
                if(noGCN != null)
                {
                    if (noGCN.SoTienDaTra == 0)
                        if (_NPTRepository.Table.Where(x => x.MaPhieuNo == noGCN.MaPhieuNo && x.SoTienDaTra > 0).ToList().Count > 0)
                            return "Đã có phiếu chi gia công ngoài";
                }
                //string kiemTraRangBuocNgayDatCoc = KiemTraDanhSachCocTruocKhiInsert(phieuThu.MaLoaiPhieu, phieuThu, phieuThu.NgayThu, lstCoc);
                //kiem tra rang buoc ngay dat coc
                //if (!string.IsNullOrEmpty(kiemTraRangBuocNgayDatCoc))
                //    return kiemTraRangBuocNgayDatCoc;

                //Kiem tra truoc khi update phieu thu
                SqlParameter MPT = new SqlParameter("MaPhieuThu", phieuThu.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", phieuThu.NgayThu);//xử lý trước khi update
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuThu", MPT,Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();

                if (lstCoc != null)
                {
                    foreach (ChiTietPhieuThu obj in lstCoc)
                    {
                        SqlParameter MPT1 = new SqlParameter("MaPhieuThuBanXe_DichVu", phieuThu.MaPhieuThu);//xử lý trước khi update
                        SqlParameter Message = new SqlParameter("Message", "");
                        Message.Size = 4000;
                        Message.Direction = System.Data.ParameterDirection.Output;
                        SqlParameter MaPhieuCoc = new SqlParameter("maPhieuCoc", obj.SoThamChieu);//xử lý trước khi update
                        MaPhieuCoc.Direction = System.Data.ParameterDirection.Input;
                        SqlParameter SoTienSuDungCoc = new SqlParameter("soTienSuDungCoc", obj.SoTienThanhToan);
                        SoTienSuDungCoc.Direction = System.Data.ParameterDirection.Input;
                        _dbContext.ExecuteStoredProcedure("sp_KiemTraCocTruocKhiUpdatePhieuThu", MPT1, Message, MaPhieuCoc, SoTienSuDungCoc);
                        if (Message.Value != null && !string.IsNullOrEmpty(Message.Value.ToString()))
                            return Message.Value.ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
             
                    _log.WriteLog("PhieuThuService.KiemTraRangBuocTruocKhiUpdatePhieuThuDichVu: " ,ex);                
                return null;
            }
            
        }

        public string UpdatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt, ref string sessionId, bool isXuLySauKhiUpdate = true,string giaTriTrenGiaoDien ="")
        {
            try
            {
                giaTriTrenGiaoDien = giaTriTrenGiaoDien.Replace("<formdata>", "<formdata>" + "<Browser>" + _clientBrowser + "</Browser>");
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == model.MaPhieuThu && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu","Không tìm thấy phiếu thu " + obj.MaPhieuThu + " để update.");
                    return "Không tồn tại phiếu";
                }
                if (obj.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss") != model.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss"))
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Đã có người cập nhật phiếu lúc. Giờ update trên form:" + model.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss") + "; giờ update database:" + obj.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss"));
                    return "Đã có người cập nhật phiếu lúc " + obj.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                if (obj.IsActive == false)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo");
                    return "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo";
                }
                if(CheckPhieuThuHHGP(model.MaPhieuThu) == true && model.HoaHong != obj.HoaHong)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo");
                    return " _Không thể cập nhật hoa hồng góp";
                }
                
                string kiemTraRangBuocNgayDatCoc = KiemTraDanhSachCocTruocKhiInsert(obj.MaLoaiPhieu, model, model.NgayThu, httt);
                if (!string.IsNullOrEmpty(kiemTraRangBuocNgayDatCoc))
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", kiemTraRangBuocNgayDatCoc);
                    return kiemTraRangBuocNgayDatCoc;
                }

                SqlParameter MPT1 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuThu", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                //lay gia tri sessionid trả ra sau xử lý
                sessionId = sessionid.Value.ToString();
                //xoa cac coc da su dung trong phieu nay truoc khi update
                if (obj.HoTen != model.HoTen
                               || obj.BienSo != model.BienSo
                               || obj.SoHopDong != model.SoHopDong
                               )
                {
                    SqlParameter pSessionId = new SqlParameter("SessionId", sessionid.Value);
                    SqlParameter pMaPhieuThu = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);
                    _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuThuSuDungCoc", pSessionId, pMaPhieuThu);
                }
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.BienSo = model.BienSo;
                obj.ConNo = model.ConNo;
                obj.CTBH = model.CTBH;
                obj.DiaChi = model.DiaChi;
                obj.DienThoai = model.DienThoai;
                obj.DoiTac = model.DoiTac;
                obj.DoiTacTraLai = model.DoiTacTraLai;
                obj.DVCV = model.DVCV;
                obj.GhiChu = model.GhiChu;
                obj.GiaBan = model.GiaBan;
                obj.NoKhuyenMai = model.NoKhuyenMai;
                if(obj.MaLoaiPhieu == "TBDTK")
                {
                    ViewGoiBDTK BDTK = GetListGBDTK(false).SingleOrDefault(x=>x.Ma == model.LoaiBaoHiem);
                    obj.GiaBan = BDTK == null ? model.GiaBan : BDTK.GiaBan;
                }    
                obj.GiamGia = model.GiamGia;
                obj.GiaVon = model.GiaVon;
                obj.HoTen = model.HoTen;
                if(model.SoChungTu == "CNHONDA")
                {
                    obj.MaPhieuLienQuan = model.MaPhieuLienQuan;
                }
                obj.KeToanTruong = model.KeToanTruong;
                obj.LoaiXe = model.LoaiXe;
                obj.MauXe = model.MauXe;
                obj.MaXe = model.MaXe;
                obj.NgayThu = model.NgayThu;
                obj.NgayHopDong = model.NgayHopDong;
                obj.ThongTinKhac = model.ThongTinKhac;
                obj.NguoiLapPhieu = model.NguoiLapPhieu;
                obj.NguoiNopTien = model.NguoiNopTien;
                obj.NgayHachToan = model.NgayThu;
                obj.SoHoaDon = model.SoHoaDon;
                obj.NguoiThuTien = model.NguoiThuTien;
                obj.NguoiUngTien = model.NguoiUngTien;
                obj.NhaCungCap = model.NhaCungCap;
                obj.NoiDung = model.NoiDung;
                obj.SoChungTu = model.SoChungTu;
                obj.SoHopDong = model.SoHopDong;
                obj.SoTienUng = model.SoTienUng;
                obj.TinhTrangPhieu = model.TinhTrangPhieu;
                if (obj.MaLoaiPhieu != "TBAXE")
                    obj.TongCong = model.TongCong;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.NgayHachToan = obj.NgayThu;
                obj.HoaHongTaiXe = model.HoaHongTaiXe;
                obj.NguoiDuyetHHTX = model.NguoiDuyetHHTX;
                obj.HoaHong = model.HoaHong;
                obj.TienTraBH = model.TienTraBH;
                obj.NguoiBaoLanh = model.NguoiBaoLanh;
                obj.SoKhung = model.SoKhung;
                obj.LoaiBaoHiem = model.LoaiBaoHiem;
                obj.NhanVienTuVan = model.NhanVienTuVan;
                double soTien = 0;
                foreach (var item in httt)
                {
                    ChiTietPhieuThu data = null;
                    if (item.MaNganHang != "NBDTK")
                        data = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id);
                    else
                        data = _ctptRepository.Table.FirstOrDefault(x => x.MaPhieuThu == item.MaPhieuThu && x.MaNganHang == "NBDTK" && x.IsDeleted == false);
                    if (!item.IsDeleted && !string.IsNullOrEmpty(item.Temp))
                    {
                        bool flag = true;
                        if (data == null)
                        {
                            data = new ChiTietPhieuThu();
                            data.Id = Guid.NewGuid();
                            data.IsDeleted = false;
                            data.IsActive = true;
                            data.CreatedBy = uid;
                            data.CreatedDate = d;
                            flag = false;
                        }
                        var a = item.Temp.Split('-');
                        data.MaPhieuThu = obj.MaPhieuThu;
                        data.HinhThucThanhToan = a[1];
                        data.MaNganHang = a[0];
                        data.SoThamChieu = item.SoThamChieu;
                        data.TinhTrang = item.TinhTrang;
                        //data.GhiChu = item.GhiChu;
                        data.SoTienThanhToan = item.SoTienThanhToan;
                        data.UpdatedBy = uid;
                        data.UpdatedDate = d;
                        if (item.TinhTrang) //neu phieu bi treo thi ghi nhan ngay treo
                            data.NgayTreoTien = model.NgayThu;
                        //else if (item.NgayTienVao == null) //neu phieuu nay truoc do chua bi treo tien hoặc chưa bị chuyển tình trạng hết treo   thi cap nhat ngay vao tien la ngay thu
                        //    item.NgayTienVao = model.NgayThu;

                        if (!flag)
                        {
                            _ctptRepository.Insert(data);
                        }
                        else
                        {
                            _ctptRepository.Update(data);
                        }

                        soTien += item.SoTienThanhToan;
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
                if (obj.TongCong == 0)
                    obj.TongCong = soTien;
                obj.TongCong = Math.Round(obj.TongCong);
                if (obj.MaLoaiPhieu == "TBAXE")
                {
                    obj.TongCong = obj.GiaBan.Value;
                }
                obj.SoTienThu = soTien;
                if (obj.MaLoaiPhieu != "TCODV" && obj.MaLoaiPhieu != "TCOBX" && obj.MaLoaiPhieu != "TCONO")
                {
                    obj.ConLai = model.ConLai;
                    obj.SoTienDaSuDung = model.SoTienDaSuDung;
                }
                else
                    obj.ConLai = obj.TongCong;//neu la phieu coc thi cap nhat so tien con lai = tong so tien coc. Vi neu phieu da su dung thi khong cho cap nhat
                obj.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                obj.Ext1 = model.Ext1;
                obj.Ext2 = model.Ext2;
                obj.Ext3 = model.Ext3;
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuThu", "Mã phiếu thu :" + obj.MaPhieuThu + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isXuLySauKhiUpdate)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý sau khi update
                    MPT2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);//xử lý sau khi update
                    pGiaTriTrenGiaoDien.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2, SessionId2,pGiaTriTrenGiaoDien,pIpClient,pHostNameClient,Error);
                    if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                    {
                        int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                        if (gioGuiEmailLast != GioGuiEmail)
                        {
                            MailHelper Mail = new MailHelper();
                            string Content = ContentMail(model.MaPhieuThu);
                            //Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            gioGuiEmailLast = GioGuiEmail;
                        }
                        else
                        {
                            if (Send == false)
                            {
                                MailHelper Mail = new MailHelper();
                                string Content = ContentMail(model.MaPhieuThu);
                                //Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                                //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
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
                
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu: " , e);
              
                return "ERROR:" + e.ToString();
            }
        }
        public string UpdatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt, List<ChiTietBanPhuKien> LPT, string type, ref string sessionId, bool isXuLySauKhiUpdate = true, string giaTriTrenGiaoDien = "")
        {
            try
            {

                giaTriTrenGiaoDien = giaTriTrenGiaoDien.Replace("<formdata>","<formdata>" + "<Browser>" + _clientBrowser + "</Browser>") ;
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == model.MaPhieuThu && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Không tìm thấy phiếu thu " + obj.MaPhieuThu + " để update.");
                    return "Không tồn tại phiếu";
                }
                if (obj.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss") != model.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss"))
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Đã có người cập nhật phiếu lúc. Giờ update trên form:" + model.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss") + "; giờ update database:" + obj.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss"));
                    return "Đã có người cập nhật phiếu lúc " + obj.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                if (obj.IsActive == false)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo");
                    return "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo";
                }
                string kiemTraRangBuocNgayDatCoc = KiemTraDanhSachCocTruocKhiInsert(obj.MaLoaiPhieu, model, model.NgayThu, httt);
                if (!string.IsNullOrEmpty(kiemTraRangBuocNgayDatCoc))
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", kiemTraRangBuocNgayDatCoc);
                    return kiemTraRangBuocNgayDatCoc;
                }


                SqlParameter MPT1 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuThu", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                //lay gia tri sessionid trả ra sau xử lý
                sessionId = sessionid.Value.ToString();
                if (obj.HoTen != model.HoTen
                                || obj.BienSo != model.BienSo
                                || obj.SoHopDong != model.SoHopDong
                                )
                {
                    SqlParameter pSessionId = new SqlParameter("SessionId", sessionid.Value);
                    SqlParameter pMaPhieuThu = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);
                    _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuThuSuDungCoc", pSessionId, pMaPhieuThu);
                }
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                
                double soTien = 0;
                if (type == "TPHKI")
                {
                    foreach (var item in httt)
                    {
                        var data = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id || (x.MaPhieuThu == item.MaPhieuThu && x.SoThamChieu == item.SoThamChieu && x.HinhThucThanhToan == "DC" && x.IsDeleted==false));
                        if (!item.IsDeleted && !string.IsNullOrEmpty(item.Temp))
                        {
                            bool flag = true;
                            if (data == null)
                            {
                                data = new ChiTietPhieuThu();
                                data.Id = Guid.NewGuid();
                                data.IsDeleted = false;
                                data.IsActive = true;
                                data.CreatedBy = uid;
                                data.CreatedDate = d;
                                flag = false;
                            }
                            //truong hop load cọc mà đã thay đổi họ tên/bsx/so hop dong thi xoa chi tiet dung coc cu va insert chi tiet moi
                            else if (item.HinhThucThanhToan == "DC")
                            {
                                if (obj.HoTen != model.HoTen
                                    || obj.BienSo != model.BienSo
                                    || obj.SoHopDong != model.SoHopDong
                                    )
                                    flag = true;
                            }
                            var a = item.Temp.Split('-');
                            data.MaPhieuThu = obj.MaPhieuThu;
                            data.HinhThucThanhToan = a[1];
                            data.MaNganHang = a[0];
                            data.SoThamChieu = item.SoThamChieu;
                            data.TinhTrang = item.TinhTrang;
                            data.SoTienThanhToan = item.SoTienThanhToan;
                            data.UpdatedBy = uid;
                            data.UpdatedDate = d;
                            if (item.TinhTrang) //neu phieu bi treo thi ghi nhan ngay treo
                                data.NgayTreoTien = model.NgayThu;
                            //else if (item.NgayTienVao == null) //neu phieuu nay truoc do chua bi treo tien hoặc chưa bị chuyển tình trạng hết treo   thi cap nhat ngay vao tien la ngay thu
                            //    item.NgayTienVao = model.NgayThu;

                            if (!flag)
                            {
                                _ctptRepository.Insert(data);
                            }
                            else
                            {
                                _ctptRepository.Update(data);
                            }

                            soTien += item.SoTienThanhToan;
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
                }
                obj.BienSo = model.BienSo;
                obj.SoKhung = model.SoKhung;
                obj.ConNo = model.ConNo;
                obj.CTBH = model.CTBH;
                obj.DiaChi = model.DiaChi;
                obj.DienThoai = model.DienThoai;
                obj.DoiTac = model.DoiTac;
                obj.DoiTacTraLai = model.DoiTacTraLai;
                obj.DVCV = model.DVCV;
                obj.GhiChu = model.GhiChu;
                obj.GiaBan = model.GiaBan;
                obj.GiamGia = model.GiamGia;
                obj.GiaVon = model.GiaVon;
                obj.HoTen = model.HoTen;
                obj.MaPhieuLienQuan = model.MaPhieuLienQuan;
                obj.KeToanTruong = model.KeToanTruong;
                obj.LoaiXe = model.LoaiXe;
                obj.MauXe = model.MauXe;
                obj.MaXe = model.MaXe;
                obj.NgayThu = model.NgayThu;
                obj.ThongTinKhac = model.ThongTinKhac;
                obj.NguoiLapPhieu = model.NguoiLapPhieu;
                obj.NguoiNopTien = model.NguoiNopTien;
                obj.NgayHachToan = model.NgayThu;
                obj.SoHoaDon = model.SoHoaDon;
                obj.NguoiThuTien = model.NguoiThuTien;
                obj.NguoiUngTien = model.NguoiUngTien;
                obj.NhaCungCap = model.NhaCungCap;
                obj.NoiDung = model.NoiDung;
                obj.SoChungTu = model.SoChungTu;
                obj.SoHopDong = model.SoHopDong;
                obj.SoTienUng = model.SoTienUng;
                obj.TinhTrangPhieu = model.TinhTrangPhieu;
                if (obj.MaLoaiPhieu != "TBAXE")
                    obj.TongCong = model.TongCong;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.NgayHachToan = obj.NgayThu;
                obj.HoaHongTaiXe = model.HoaHongTaiXe;
                obj.NguoiDuyetHHTX = model.NguoiDuyetHHTX;
                obj.HoaHong = model.HoaHong;
                obj.TienTraBH = model.TienTraBH;
                obj.SoTienThu = soTien;
                obj.TongCong = 0;
                obj.GiaBan = 0;
                obj.GiamGia = 0;
                obj.LoaiBaoHiem = model.LoaiBaoHiem;
                obj.NhanVienTuVan = model.NhanVienTuVan;
                // obj.NoKhuyenMai = model.NoKhuyenMai;
                double TongTien = 0;
                if (LPT != null)
                {
                    foreach (var temp in LPT)
                    {
                        obj.GiaBan += temp.GiaBan;
                        obj.GiamGia += temp.GiamGia;
                        obj.TongCong += temp.ThucThu;

                        TongTien += temp.GiaVon + temp.TienCong;
                    }
                }
                obj.ConLai = obj.TongCong - soTien;//neu la phieu coc thi cap nhat so tien con lai = tong so tien coc. Vi neu phieu da su dung thi khong cho cap nhat
                                                   //model.ConLai = model.TongCong - soTien;
                if (type == "TKMPHKI")
                {
                    obj.TongCong = 0;
                    obj.ConLai = 0;
                }
                if (type == "NKMPK")
                {
                    obj.TongCong = TongTien;
                }

                obj.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                obj.Ext1 = model.Ext1;
                obj.Ext2 = model.Ext2;
                obj.Ext3 = model.Ext3;
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuThu", "Mã phiếu thu :" + obj.MaPhieuThu + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isXuLySauKhiUpdate)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý sau khi update
                    MPT2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);//xử lý sau khi update
                    pGiaTriTrenGiaoDien.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2, SessionId2, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Error);
                    if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                    {
                        int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                        if (gioGuiEmailLast != GioGuiEmail)
                        {
                            MailHelper Mail = new MailHelper();
                            string Content = ContentMail(model.MaPhieuThu);
                            //Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                            //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                            Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                            gioGuiEmailLast = GioGuiEmail;
                        }
                        else
                        {
                            if (Send == false)
                            {
                                MailHelper Mail = new MailHelper();
                                string Content = ContentMail(model.MaPhieuThu);
                                //Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                               // Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                                Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
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
             
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu: " , e);

                return "ERROR:" + e.ToString();
            }
        }

        public string UpdatePhieuDichVuChoHoaHongTaiXe(string maPhieuThu, string nguoiDuyetHHTX, double? soTienHHTX )
        {
            try
            {
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == maPhieuThu && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                    return "Không tồn tại phiếu";
                if (obj.IsActive == false)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo");
                    return "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo";
                }
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.NguoiDuyetHHTX = nguoiDuyetHHTX;
                obj.HoaHongTaiXe = soTienHHTX;
                obj.UpdatedDate = DateTime.Now;
                obj.UpdatedBy = uid;                                
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, maPhieuThu, "", "UPDATE", "UpdatePhieuDichVuChoHoaHongTaiXe", "Người duyệt phiếu :" + nguoiDuyetHHTX, _authenticationService.GetAuthenticatedUser().UserId);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.UpdatePhieuDichVuChoHoaHongTaiXe: " ,ex);
             
                return ex.ToString();
            }
            return "";
        }
        public string DeletePhieuThu(string id, string type)
        {
            try
            {
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == id && x.MaLoaiPhieu == type && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                    return "Phiếu thu không tồn tại hoặc đã bị xóa";
                SqlParameter MPT1 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiDeletePhieuThu", MPT1, sessionid, message);

                //kiem tra neu hop le thi moi cho xoa
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                obj.IsDeleted = true;
                obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedDate = DateTime.Now;
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "DELETE", "DeletePhieuThu", "DeletePhieuThu", _authenticationService.GetAuthenticatedUser().UserId);
                SqlParameter MPT2 = new SqlParameter("MaPhieu", obj.MaPhieuThu);//xử lý sau khi update  
                SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                SessionId2.Direction = System.Data.ParameterDirection.Input;
                SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien"
                    , "<formdata>" + Environment.NewLine + "<Action>DeletePhieuThu</Action>"
                        + Environment.NewLine + "<MaPhieuThu>" + obj.MaPhieuThu + "</MaPhieuThu>"
                        + Environment.NewLine + "</formdata>"
                        );//xử lý sau khi update
                SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                Error.Direction = System.Data.ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2, SessionId2, pIpClient, pHostNameClient, pGiaTriTrenGiaoDien, Error);
                if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                {
                    int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                    if (gioGuiEmailLast != GioGuiEmail)
                    {
                        MailHelper Mail = new MailHelper();
                        string Content = ContentMail(id);
                        Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                       // Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                        Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                        gioGuiEmailLast = GioGuiEmail;
                    }
                    else
                    {
                        if (Send == false)
                        {
                            MailHelper Mail = new MailHelper();
                            string Content = ContentMail(id);
                            Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                           // Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                            Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                            gioGuiEmailLast = GioGuiEmail;
                            Send = true;
                        }
                    }
                }
                return Error.Value.ToString();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.DeletePhieuThu: " ,ex);
               
                return "Lỗi phát sinh:" + ex.Message;
            }
        }
        public string DeletePhieuThu(string id)
        {
            try
            {
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == id && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                    return "Phiếu thu không tồn tại hoặc đã bị xóa";
                //Kiểm tra đã có phiếu QT hứa tặng chưa
                var objQT = _phieuQTRepository.Table.FirstOrDefault(x => x.MaPhieuLienQuan == obj.MaPhieuLienQuan && x.IsDeleted == false && x.IsActive == true);
                if (objQT != null)
                {
                    return "Đã có phiếu quyết toán phụ kiện hứa tặng. Vui lòng xóa quyết toán trước.";
                }
                SqlParameter MPT1 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý trước khi update
                MPT1.DbType = System.Data.DbType.String;
                SqlParameter Message1 = new SqlParameter("Message", "");//xử lý trước khi update
                Message1.Size = 4000;
                Message1.Direction = System.Data.ParameterDirection.Output;
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update

                _dbContext.ExecuteStoredProcedure("[sp_XuLyTruocKhiDeletePhieuThu]", MPT1, sessionid, Message1);
                //kiem tra neu hop le thi moi cho xoa
                if (Message1.Value != null && !string.IsNullOrEmpty(Message1.Value.ToString()))
                    return Message1.Value.ToString();
                //cap nhat lai phieu nay IsDeleted = 1
                obj.IsDeleted = true;
                obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedDate = DateTime.Now;
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "DELETE", "DeletePhieuThu", "DeletePhieuThu", _authenticationService.GetAuthenticatedUser().UserId);
                SqlParameter MPT2 = new SqlParameter("MaPhieu", obj.MaPhieuThu);//xử lý sau khi update
                SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                SessionId2.Direction = System.Data.ParameterDirection.Input;
                SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                pIpClient.Direction = System.Data.ParameterDirection.Input;
                SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                pHostNameClient.Direction = System.Data.ParameterDirection.Input;
                SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien"
                    , "<formdata>" + Environment.NewLine + "<Action>DeletePhieuThu</Action>"
                        + Environment.NewLine + "<MaPhieuThu>" + obj.MaPhieuThu + "</MaPhieuThu>"
                        + Environment.NewLine + "</formdata>"
                        );//xử lý sau khi update
                pGiaTriTrenGiaoDien.Direction = System.Data.ParameterDirection.Input;
                SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                Error.Direction = System.Data.ParameterDirection.Output;
                Error.Size = 4000;
                SqlParameter Action = new SqlParameter("Action", "UPDATE");//xử lý sau khi update
                Action.Direction = System.Data.ParameterDirection.Input;
                _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2, SessionId2, Action, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Error);
                if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                {
                    int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                    if (gioGuiEmailLast != GioGuiEmail)
                    {
                        MailHelper Mail = new MailHelper();
                        string Content = ContentMail(id);
                        Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                        //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                        Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                        gioGuiEmailLast = GioGuiEmail;
                    }
                    else
                    {
                        if (Send == false)
                        {
                            MailHelper Mail = new MailHelper();
                            string Content = ContentMail(id);
                            Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                           // Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                            Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                            gioGuiEmailLast = GioGuiEmail;
                            Send = true;
                        }
                    }
                }
                return Error.Value.ToString();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.DeletePhieuThu: " ,ex);
               
                return "Lỗi phát sinh:" + ex.Message;
            }
        }
        public string UpdateSHDTCOBX(string id,string shd)
        {
            PhieuThu obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == id && x.IsDeleted == false);
            obj.SoHopDong = shd;
            obj.UpdatedDate = DateTime.Now;
            obj.UpdatedBy = CurrentUser.UserId;
            _phieuThuRepository.Update(obj);
            return "";
        }
        #endregion

        public string XuLySauKhiInsertPhieuThu(string maPhieuThu,string giaTriTrenGiaodien, bool isXuLyTinhToanTuiTien = true)
        {
            //SqlParameter MPT2 = new SqlParameter("MaPhieuThu", maPhieuThu);//xử lý sau khi update
            //_dbContext.ExecuteStoredProcedure("sp_XuLySauKhiInsertPhieuThu", MPT2);
            return XuLySauKhiUpdatePhieuThu(maPhieuThu,"", giaTriTrenGiaodien,"INSERT");
        }


        public string XuLySauKhiUpdatePhieuThu(string maPhieuThu, string sessionId = "",string giaTriTrenGiaoDien ="",string action = "UPDATE")
        {
            giaTriTrenGiaoDien = giaTriTrenGiaoDien.Replace("<formdata>", "<formdata>" + "<Browser>" + _clientBrowser + "</Browser>");
            SqlParameter MPT2 = new SqlParameter("MaPhieuThu", maPhieuThu);//xử lý sau khi update
            SqlParameter SessionId = new SqlParameter("SessionId", sessionId);//xử lý sau khi update
            SqlParameter Action = new SqlParameter("action", @action);//xử lý sau khi update
            SqlParameter paramGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);//xử lý sau khi update
            MPT2.Direction = System.Data.ParameterDirection.Input;
            SessionId.Direction = System.Data.ParameterDirection.Input;
            Action.Direction = System.Data.ParameterDirection.Input;           
            paramGiaTriTrenGiaoDien.Direction = System.Data.ParameterDirection.Input;
            paramGiaTriTrenGiaoDien.Size = 10000;
            paramGiaTriTrenGiaoDien.Value = giaTriTrenGiaoDien;
            paramGiaTriTrenGiaoDien.SqlValue = giaTriTrenGiaoDien;
            SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
            SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
            SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
            Error.Size = 4000;
            Error.Direction = System.Data.ParameterDirection.Output;
            _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2,SessionId,Action, paramGiaTriTrenGiaoDien,pIpClient,pHostNameClient,Error);
            if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
            {
                //int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                //if (gioGuiEmailLast != GioGuiEmail)
                //{
                //    MailHelper Mail = new MailHelper();
                //    //string Content = ContentMail(maPhieuThu);
                //    string Content = "Import dịch vụ làm mất cân. Do " + CurrentUser.UserName + " thao tác!";
                //    Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                //    Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                //    Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                //    gioGuiEmailLast = GioGuiEmail;
                //}
                //else
                //{
                //    if (Send == false)
                //    {
                //        MailHelper Mail = new MailHelper();
                //        //string Content = ContentMail(maPhieuThu);
                //        string Content = "Import dịch vụ làm mất cân. Do " + CurrentUser.UserName + " thao tác!";
                //        Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                //        Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                //        Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                //        //gioGuiEmailLast = GioGuiEmail;
                //        //Send = true;
                //    }
                //}
                try
                {
                    MailHelper Mail = new MailHelper();
                    string Content = "Import dịch vụ làm mất cân. Do " + CurrentUser.UserName + " thao tác! Vào lúc " + DateTime.Now + ". Mã Phiếu thu là " + maPhieuThu;
                    Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                    //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                    Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên TNK HonDa Quận 2", Content + " " + Error.Value.ToString());
                }
                catch(Exception ex)
                {
                    _log.WriteLog("Lỗi import excel file " + ex.ToString());
                }
                
            }
            return Error.Value.ToString();

        }
        /// <summary>
        /// Lay ty le gia von khuyen mai cho luoi khuyen mai ban xe
        /// </summary>
        /// <returns></returns>
        public double GetTyLeGiaVonKhuyenMai()
        {
            try
            {
                var obj = _configRepository.Table.FirstOrDefault(x => x.ConfigCode == "PHAN_TRAM_GIA_VON_KM");
                if (obj != null)
                    return double.Parse(obj.ConfigValue);
            }
            catch (Exception ex)
            {
              
                    _log.WriteLog("PhieuThuService.GetTyLeGiaVonKhuyenMai: " ,ex);
                return 1;
            }
            return 1;
        }

        public bool ChangeTypeTCOBX(string id, bool type)
        {
            try
            {
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == id);
                obj.TinhTrangPhieu = type ? "B" : "";
                obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedDate = DateTime.Now;
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "ChangeTypeTCOBX", "ChangeTypeTCOBX", "ChangeTypeTCOBX", _authenticationService.GetAuthenticatedUser().UserId);
                return true;
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.ChangeTypeTCOBX: " ,ex);
              
                return false;
            }
        }

        public List<PhieuThu> GetKMBaoHiem(string soHopDong)
        {
            try
            {
                SqlParameter pSoHopDong = new SqlParameter("soHopDong", soHopDong);
                //return _phieuThuRepository.Table.Where(x => x.SoHopDong == soHopDong && x.MaLoaiPhieu == "TBAHI" && x.TongCong == 0 && x.IsDeleted == false).ToList();
                var lst = _dbContext.ExecuteStoredProcedureList<PhieuThu>("sp_GetChiTietKhuyenMaiThuHo", pSoHopDong).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetKMBaoHiem: " ,ex);
                    
                return null;
            }
            
        }
        public List<ChiTietBanPhuKien> GetKMPKien(string soHopDong)
        {
            try
            {
                SqlParameter pSoHopDong = new SqlParameter("soHopDong", soHopDong);
                pSoHopDong.Size = 50;
                var list = _dbContext.ExecuteStoredProcedureList<ChiTietBanPhuKien>("sp_GetChiTietKhuyenMaiPhuKien",pSoHopDong).ToList();
                return list;
                // return _CTBPKRepository.Table.Where(x => x.SoHopDong == soHopDong && x.MaLoaiPhieu == "TPHKI" && x.TongCong == 0 && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetKMPKien: " ,ex);
              
                return null;
            }

        }

        public List<ViewNoPhaiThu> GetNoChietKhauThuongMai(string soKhung)
        {
            try
            {
                SqlParameter pSoHopDong = new SqlParameter("soKhung", soKhung);
                pSoHopDong.Size = 50;
                var list = _dbContext.ExecuteStoredProcedureList<ViewNoPhaiThu>("sp_GetNoChietKhauThuongMaiTheoSoKhung", pSoHopDong).ToList();
                return list;
                // return _CTBPKRepository.Table.Where(x => x.SoHopDong == soHopDong && x.MaLoaiPhieu == "TPHKI" && x.TongCong == 0 && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetKMPKien: ", ex);

                return null;
            }

        }
        public List<ViewTBDTK> GetBDTK(string soHopDong)
        {
            try
            {
                List<ViewTBDTK> lst = _VTBDTKRepository.Table.Where(x => x.SoHopDong == soHopDong && x.IsDeleted == false && (x.SoTienThu-x.GiamGia)==0).ToList();
                // return _CTBPKRepository.Table.Where(x => x.SoHopDong == soHopDong && x.MaLoaiPhieu == "TPHKI" && x.TongCong == 0 && x.IsDeleted == false).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetKMPKien: ", ex);

                return null;
            }

        }
        public double GetNKMPK(string soHopDong)
        {
            try
            {
                double nokm = 0;
                //var view = double.Parse( _phieuThuRepository.Table.Where(x => x.SoHopDong == soHopDong && x.IsDeleted == false && x.MaLoaiPhieu == "TPHKI").Sum(y =>y.NoKhuyenMai).ToString());
                // return _CTBPKRepository.Table.Where(x => x.SoHopDong == soHopDong && x.MaLoaiPhieu == "TPHKI" && x.TongCong == 0 && x.IsDeleted == false).ToList();
                var lst = _phieuThuRepository.Table.Where(x => x.SoHopDong == soHopDong && x.IsDeleted == false && x.MaLoaiPhieu == "TPHKI" && x.NoKhuyenMai > 0).ToList();
                foreach (var item in lst)
                {
                    var NPT = _NPTRepository.Table.SingleOrDefault(x => x.MaPhieuPhatSinh == item.MaPhieuThu && x.IsDeleted == false);
                    if (NPT != null)
                    {
                        nokm += GetNoKMConLai(NPT.MaPhieuNo);
                    }
                }
                return nokm;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetKMPKien: ", ex);

                return 0;
            }
        }
        //public List<PhieuThu> GetKMPKien(string soHopDong)
        //{
        //    try
        //    {
        //        return _phieuThuRepository.Table.Where(x => x.SoHopDong == soHopDong && x.MaLoaiPhieu == "TPHKI" && x.TongCong == 0 && x.IsDeleted == false).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.WriteLog("PhieuThuService.GetKMPKien: " ,ex);
        //        return null;
        //    }

        //}
        public List<ViewPhieuThu> GetListPhieuThu(DateTime from, DateTime to, int p, ref int total, int pageSize, string query,string maLoaiPhieu)
        {
            try
            {
                query = query.ToLower();
                var  list = _ViewphieuThuRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if(maLoaiPhieu != "")
                {
                    list = _ViewphieuThuRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.MaLoaiPhieu == maLoaiPhieu);
                }
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.SoHopDong.ToString().Contains(query.ToLower())
                    || x.MaPhieuThu.ToLower().Contains(query.ToLower())
                    || x.MaLoaiPhieu.ToLower().Contains(query.ToLower())
                    || x.ThongTinKhac.ToLower().Contains(query.ToLower())
                    || x.DoiTac.ToLower().Contains(query.ToLower())
                    || x.DVCV.ToLower().Contains(query.ToLower())
                    || x.Hoten.ToLower().Contains(query.ToLower())                    
                    || query.Contains(x.SoHopDong.ToString().ToLower())
                    || query.Contains(x.MaLoaiPhieu.ToLower())
                    || query.Contains(x.MaPhieuThu.ToLower())
                    //  || query.Contains(x.HoTen.ToLower())

                    );
                }
                var result = list.OrderBy(x => x.MaLoaiPhieu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetListPhieuThu: " ,ex);
             
                return null;
            }
            
        }
        //phieu thu hoan von, truc lam
        public List<ViewCCOMX> ListCocMuaXe(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _ViewCCOMXRepository.Table.Where(x => x.NgayChi >= from && x.NgayChi <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuChi.ToLower().Contains(query) && x.DoiTac.ToLower().Contains(query));
                }
                var result = q.OrderByDescending(x => x.NgayChi).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.ListCocMuaXe:" ,ex);
               
                return null;
            }
        }

        #region Phần về cập nhật hóa đơn
        public List<DanhMucDienGiai> ListLoaiPhieu(string ID_Loai)
        {
              return _DMDGRepository.Table.Where(x=>x.TableSuDung == ID_Loai && x.IsActive == true).ToList();
        }
       

        public bool Update_SoHoaDonNgayGhiNhanCongNo(string sMaPhieuNo, string sSoHoaDon,string sNgayXuatHoaDon, string sNgayGhiNhanCongNo,string sUser, string ghiChu)
        {
            try
            {
                SqlParameter ngayXacNhanCongNo = null;
                SqlParameter ngayXuatHoaDon = null;
                DateTime d = DateTime.Now;
                if (DateTime.TryParse(sNgayXuatHoaDon, out d))
                    ngayXuatHoaDon = new SqlParameter("ngayXuatHoaDon", d);
                else
                    ngayXuatHoaDon = new SqlParameter("ngayXuatHoaDon", DBNull.Value);

                if (DateTime.TryParse(sNgayGhiNhanCongNo, out d))
                    ngayXacNhanCongNo = new SqlParameter("ngayXacNhanCongNo", d);
                else
                    ngayXacNhanCongNo = new SqlParameter("ngayXacNhanCongNo", DBNull.Value);

                SqlParameter maPhieuNo = new SqlParameter("MaPhieuNo", sMaPhieuNo);
                SqlParameter soHoaDon = new SqlParameter("soHoaDon", sSoHoaDon);                
                SqlParameter user = new SqlParameter("user", sUser);
                SqlParameter pGhiChu = new SqlParameter("GhiChu", ghiChu);

                _dbContext.ExecuteStoredProcedure("sp_NoPhaiThu_GhiNhanSoHoaDon", maPhieuNo,soHoaDon, ngayXuatHoaDon, ngayXacNhanCongNo, user,pGhiChu);

                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.Update_SoHoaDonNgayGhiNhanCongNo:" + ex);
                return false;
            }
        }
        public bool Update_SoHoaDon(string maPhieuThu, string soHoaDon,string soChungTu)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var data = _phieuThuRepository.Table.FirstOrDefault(x=>x.MaPhieuThu == maPhieuThu && x.IsDeleted == false);
                if(data == null)
                {
                    return false;
                }
                else
                {
                    data.SoHoaDon = soHoaDon;
                    data.SoChungTu = soChungTu;
                    data.UpdatedBy = uid;
                    data.UpdatedDate = DateTime.Now;
                    _phieuThuRepository.Update(data);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, maPhieuThu, soHoaDon, "UPDATE", "Update_SoHoaDon", "Update_SoHoaDon", _authenticationService.GetAuthenticatedUser().UserId);
                    return true;
                }
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.Update:" + ex);
                return false;
            }
        }

        public bool Update_GhiChuPhieuThu(string sMaPhieuThu, string sghiChu, Guid sUser)
        {
            try
            {

                SqlParameter pMaPhieuThu = new SqlParameter("MaPhieuThu", sMaPhieuThu);
                SqlParameter pGhiChu = new SqlParameter("GiaiTrinh", sghiChu);
                SqlParameter pUser = new SqlParameter("user", sUser);
                
                _dbContext.ExecuteStoredProcedure("sp_NoBHDV_UpdateGhiChuPhieuThu", pMaPhieuThu, pGhiChu, pUser);

                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.Update_GiaiTrinh:" + ex);
                return false;
            }
        }

        public bool Update_GiaiTrinh(string sMaPhieuNo, string sgiaiTrinh,string sUser, string lyDoQuaHan)
        {
            try
            {
                              
                SqlParameter pMaPhieuNo = new SqlParameter("MaPhieuNo", sMaPhieuNo);
                SqlParameter pGiaiTrinh = new SqlParameter("GiaiTrinh", sgiaiTrinh);
                SqlParameter pUser = new SqlParameter("user", sUser);
                SqlParameter pLyDoQuaHan = new SqlParameter("LyDoQuaHan", lyDoQuaHan);

                _dbContext.ExecuteStoredProcedure("sp_NoBHDV_UpdateGiaiTrinh",pMaPhieuNo,pGiaiTrinh,pUser,pLyDoQuaHan);

                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.Update_GiaiTrinh:" + ex);
                return false;
            }
        }
        public bool Update_SalesNote(string MaPhieuNo, string Sales, string GhiChu)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var data = _NPThuRepository.Table.FirstOrDefault(x => x.MaPhieuNo == MaPhieuNo && x.IsDeleted == false);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    data.GhiChu = GhiChu;
                    data.Sales = Sales;
                    data.UpdatedBy = uid;
                    data.UpdatedDate = DateTime.Now;
                    _NPThuRepository.Update(data);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, MaPhieuNo, Sales, "UPDATE", "Update_SoHoaDon", "Update_SoHoaDon", _authenticationService.GetAuthenticatedUser().UserId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.Update:" + ex);
                return false;
            }
        }
        public bool Update_SHDByMaPhieuNo(string MaPhieuNo, string SoHoaDon)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var data = _NPThuRepository.Table.FirstOrDefault(x => x.MaPhieuNo == MaPhieuNo && x.IsDeleted == false);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    data.SoHoaDon = SoHoaDon;
                    data.UpdatedBy = uid;
                    data.UpdatedDate = DateTime.Now;
                    _NPThuRepository.Update(data);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, MaPhieuNo, SoHoaDon, "UPDATE", "Update_SoHoaDon", "Update_SoHopDong", _authenticationService.GetAuthenticatedUser().UserId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.Update:" + ex);
                return false;
            }
        }
        #endregion
        public List<PhieuThu> getPhieuThuCuoi(string soHopDong)
        {
            try
            {
                List<PhieuThu> item = _phieuThuRepository.Table.Where(x => x.SoHopDong == soHopDong && x.IsDeleted == false && x.MaLoaiPhieu == "TCOBX").OrderByDescending(x => x.NgayThu).Take(1).ToList();
                return item;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.getPhieuThuCuoi" + ex);
                if(ex.InnerException!= null)
                    _log.WriteLog("PhieuThuService.getPhieuThuCuoi" + ex.InnerException);
                return null;
            }
        }
        public List<ViewTCODV> getLoadBienSo(string BienSo)
        {
            try
            {
                List<ViewTCODV> result = _ViewTCODVRepository.Table.Where(x => x.BienSo == BienSo).ToList();
                return result;
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.getLoadBienSo" + ex);
                if(ex.InnerException!= null)
                    _log.WriteLog("PhieuThuService.getLoadBienSo:" + ex.InnerException);
                return null;
            }
        }
        public List<PhieuThu> getPhieuThuBSXCuoi(string bsx)
        {
            try
            {
                List<PhieuThu> item = _phieuThuRepository.Table.Where(x => x.BienSo == bsx && x.IsDeleted == false && x.MaLoaiPhieu == "TCODV").OrderByDescending(x => x.NgayHopDong).Take(1).ToList();
                return item;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.getPhieuThuBSXCuoi" + ex);
                if(ex.InnerException!=null)
                    _log.WriteLog("PhieuThuService.getPhieuThuBSXCuoi" + ex.InnerException);
                return null;
            }
        }
        public double TotalPT()
        {
            var listPT = _phieuThuRepository.Table.Where(x => x.IsDeleted == false && x.MaLoaiPhieu =="TGOVO" && x.DoiTac == "HDPT").ToList();
            if (listPT.Count > 0)
            {
                double total = listPT.Sum(x => x.SoTienThu);
                return total;
            }
            else
                return 0;
        }
        public double TotalMT()
        {
            var listMT = _phieuThuRepository.Table.Where(x => x.IsDeleted == false && x.MaLoaiPhieu == "TGOVO" && x.DoiTac == "HDMT").ToList();
            if (listMT.Count > 0)
            {
                double total = listMT.Sum(x=>x.SoTienThu);
                return total;
            }
            else
                return 0;
        }
        public List<ViewHinhThucThanhToan> ThuHoaHongNgay()
        {
            var list = _ViewHinhThucThanhToan.Table.Where(x => x.MaHinhThuc == "TM").ToList();
            return list;
        }
        //phan create ban phu kien
        public string CreateCTBanPhuKien(string MaPhieuThu, List<ChiTietBanPhuKien> list,PhieuThu temp,string type)
        {
            logger.Start("CreateCTBanPhuKien");
            try
            {
              
                if (list != null)
                {
                    var dem = _CTBPKRepository.Table.Count();
                    double ThucThu = 0;
                    foreach(var tt in list)
                    {
                        ThucThu += tt.ThucThu;
                    }
                    foreach (var item in list)
                    {
                        if (item.TenHang == null && item.GiaBan == 0 && item.GiamGia == 0 && item.ThucThu == 0 && type == "TPHKI")
                            continue;
                        dem = dem + 1;
                        KhoPhuTung kpt = _KPTRepository.Table.Where(x => x.MaKho == item.MaKho).FirstOrDefault();
                      
                        double? phanTramGiaVon = 0;
                        if (kpt != null)
                            phanTramGiaVon = kpt.PhanTramGiaVon;
                        item.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                        item.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                        item.IsDeleted = false;
                        item.CreatedDate = DateTime.Now;
                        item.UpdatedDate = DateTime.Now;
                        if (type != "TKMPHKI" && type != "NKMPK")
                        {
                            if (temp.SoHopDong != "" && ThucThu == 0)
                                item.GiaVon = item.GiaBan;
                            if (item.GiaVon == 0)
                            {
                                item.GiaVon = Math.Round(phanTramGiaVon.Value * item.GiaBan);
                            }
                            //else
                            //    item.GiaVon = item.GiaBan * Convert.ToDouble(PTGV);
                        }                        
                        item.ThucThu  = type == "TKMPHKI" ? 0 : item.ThucThu;
                        item.GiaBan = type == "TKMPHKI" ? 0 : item.GiaBan;


                        item.TienCong = item.TienCong;//type == "TKMPHKI" || type == "NKMPK"  ? item.TienCong : 0;                        
                        item.MaPhieuThu = MaPhieuThu;
                        item.NhaCungCap = "";
                        item.Ma = dem;
                        _CTBPKRepository.Insert(item);
                        logger.Info(">>CTBPK");
                        logger.Param("item.MaPhieuThu", item.MaPhieuThu);
                        logger.Param("item.MaKho", item.MaKho);
                        logger.Param("item.GiaBan", item.GiaBan);
                        logger.Param("item.GiamGia", item.GiamGia);
                        logger.Param("item.GiaVon", item.GiaVon);
                        logger.Info("<<CTBPK");
                    }
                }
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.CreateCTBanPhuKien" + ex);
                if(ex.InnerException!= null)
                    _log.WriteLog("PhieuThuService.CreateCTBanPhuKien" + ex.InnerException);
                logger.Error(ex);
                logger.End("CreateCTBanPhuKien");
                return "Lỗi thêm chi tiết bán phụ kiện";
            }
            logger.End("CreateCTBanPhuKien");
            return "";
        }
        public string EditCTBanPhuKien(string MaPhieuThu, List<ChiTietBanPhuKien> list, PhieuThu phieuThu,string type)
        {
            logger.Start("EditCTBanPhuKien");
            try
            {
                double ThucThu = 0;
                foreach (var tt in list)
                {
                    ThucThu += tt.ThucThu;
                }
                foreach (var item in list)
                {
                    var PTGV = _KPTRepository.Table.Where(x => x.MaKho == item.MaKho).Select(x => x.PhanTramGiaVon).FirstOrDefault();
                    item.MaPhieuThu = MaPhieuThu;
                    if (item.TenHang == null && item.GiaBan == 0 && item.GiamGia == 0 && item.ThucThu == 0 && type == "TPHKI")
                    {
                        ChiTietBanPhuKien item_Isdelete = _CTBPKRepository.Table.Where(x => x.MaPhieuThu == item.MaPhieuThu && x.MaKho == item.MaKho && x.IsDeleted == false).FirstOrDefault();
                        if(item_Isdelete != null)
                        {
                            item_Isdelete.IsDeleted = true;
                            _CTBPKRepository.Update(item_Isdelete);
                        }
                    }
                    else
                    {
                        ChiTietBanPhuKien temp = _CTBPKRepository.Table.Where(x => x.MaPhieuThu == MaPhieuThu && x.MaKho == item.MaKho && x.IsDeleted == false).FirstOrDefault();
                        if (temp == null)
                        {
                            if (item.TenHang == null && item.GiaBan == 0 && item.GiamGia == 0 && item.ThucThu == 0 && type == "TPHKI")
                                continue;
                            else
                            {
                                item.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                                item.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                                item.CreatedDate = DateTime.Now;
                                item.UpdatedDate = DateTime.Now;
                                item.IsDeleted = false;
                                if (phieuThu.SoHopDong != null && ThucThu == 0)
                                    item.GiaVon = item.GiaBan;
                                else if (item.GiaVon == 0)
                                    item.GiaVon = item.GiaBan * Convert.ToDouble(PTGV);

                                item.MaPhieuThu = MaPhieuThu;
                                _CTBPKRepository.Insert(item);
                                logger.Info(">>Insert CTBPK");
                                logger.Param("item.MaPhieuThu", item.MaPhieuThu);
                                logger.Param("item.MaKho", item.MaKho);
                                logger.Param("item.GiaBan", item.GiaBan);
                                logger.Param("item.GiamGia", item.GiamGia);
                                logger.Param("item.GiaVon", item.GiaVon);
                                logger.Info("<<Insert CTBPK");
                            }
                        }
                        else
                        {
                            temp.TenHang = item.TenHang;
                            temp.GiamGia = item.GiamGia;
                            if (type != "TKMPHKI" && type != "NKMPK")
                            {
                                if (phieuThu.SoHopDong != null && ThucThu == 0)
                                    temp.GiaVon = item.GiaBan;
                                else if (item.GiaVon == 0 || item.GiaBan != temp.GiaBan)//neu gia von co thay doi thi moi update lai theo ty le
                                {
                                    temp.GiaVon = Math.Round(item.GiaBan * Convert.ToDouble(PTGV));

                                }
                                else
                                    temp.GiaVon = item.GiaVon;
                            }
                            else if (item.GiaVon != temp.GiaVon)
                                temp.GiaVon = item.GiaVon;
                            temp.ThucThu = type == "TKMPHKI" ? 0 : item.ThucThu;
                            temp.GiaBan = type == "TKMPHKI" ? 0 : item.GiaBan;
                            temp.TienCong = type == "TKMPHKI" || type == "NKMPK" ? item.TienCong : 0;
                            temp.SoLuong = item.SoLuong;
                            temp.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                            temp.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                            temp.UpdatedDate = DateTime.Now;
                            temp.CreatedDate = DateTime.Now;
                            _CTBPKRepository.Update(temp);
                            logger.Info(">>Update CTBPK");
                            logger.Param("item.MaPhieuThu", item.MaPhieuThu);
                            logger.Param("item.MaKho", item.MaKho);
                            logger.Param("item.GiaBan", item.GiaBan);
                            logger.Param("item.GiamGia", item.GiamGia);
                            logger.Param("item.GiaVon", item.GiaVon);
                            logger.Info("<<Update CTBPK");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.CreateCTBanPhuKien" + ex);
                if(ex.InnerException != null)
                    _log.WriteLog("PhieuThuService.CreateCTBanPhuKien" + ex.InnerException);
                logger.Error(ex);
                logger.End("EditCTBanPhuKien");
                return "Lỗi cập nhật chi tiết bán phụ kiện";
            }
            logger.End("EditCTBanPhuKien");
            return "";
        }
        public string CreateCTBanPhuKien(string MaPhieuThu, List<ChiTietBanPhuKien> list, string type)
        {
            logger.Start("CreateCTBanPhuKien");
            try
            {
                if (list != null)
                {
                    var dem = _CTBPKRepository.Table.Count();
                    double ThucThu = 0;
                    foreach (var tt in list)
                    {
                        ThucThu += tt.ThucThu;
                    }
                    foreach (var item in list)
                    {
                        if (item.TenHang == null && item.GiaBan == 0 && item.GiamGia == 0 && item.ThucThu == 0 && type == "TPHKI")
                            continue;
                        dem = dem + 1;
                        var PTGV = _KPTRepository.Table.Where(x => x.MaKho == item.MaKho).Select(x => x.PhanTramGiaVon).FirstOrDefault();
                        item.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                        item.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                        item.IsDeleted = false;
                        item.CreatedDate = DateTime.Now;
                        item.UpdatedDate = DateTime.Now;
                        item.ThucThu = type == "TKMPHKI" ? 0 : item.ThucThu;
                        item.GiaBan = type == "TKMPHKI" ? 0 : item.GiaBan;
                        if (item.GiaVon == 0)
                        {
                            item.GiaVon = Math.Round( PTGV.Value * item.GiaBan);
                        }
                        
                        item.TienCong = type == "TKMPHKI" ? item.TienCong : 0;
                        item.MaPhieuThu = MaPhieuThu;
                        item.NhaCungCap = "";
                        item.Ma = dem;
                        _CTBPKRepository.Insert(item); 
                        logger.Info(">>Update CTBPK");
                        logger.Param("item.MaPhieuThu", item.MaPhieuThu);
                        logger.Param("item.MaKho", item.MaKho);
                        logger.Param("item.GiaBan", item.GiaBan);
                        logger.Param("item.GiamGia", item.GiamGia);
                        logger.Param("item.GiaVon", item.GiaVon);
                        logger.Info("<<Update CTBPK");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.CreateCTBanPhuKien" + ex);
                if (ex.InnerException != null)
                    _log.WriteLog("PhieuThuService.CreateCTBanPhuKien" + ex.InnerException);
                logger.End("CreateCTBanPhuKien");
                return "Lỗi thêm chi tiết bán phụ kiện";
            }
            logger.End("CreateCTBanPhuKien");
            return "";
        }
        public DataTable GetListCTBPK(string MaPhieuThu)
        {
            try
            {
                SqlParameter maPhieuThu = new SqlParameter("MaPhieuThu", MaPhieuThu);
                return  _dbContext.ExecuteStoredProcedureDataTable("sp_DanhSachPhuTungPTPK", maPhieuThu);
            }
            catch(Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetListCTBPK" + ex);
                return null;
            }
        }
        public DataTable GetListCTBPKKM(string MaPhieuThu)
        {
            try
            {
                SqlParameter maPhieuThu = new SqlParameter("MaPhieuThu", MaPhieuThu);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_DanhSachPhuTungPTPKKM", maPhieuThu);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetListCTBPK" + ex);
                return null;
            }
        }
        public List<ViewGoiBDTK> GetListGBDTK(bool isForInsert)
        {
            try
            {
                SqlParameter PrisForInsert = new SqlParameter("isForInsert", isForInsert);
                return _dbContext.ExecuteStoredProcedureList<ViewGoiBDTK>("sp_LayDanhSachBDTK", PrisForInsert).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetListBDTK" + ex);
                return null;
            }
        }
        public List<ViewChiTietBaoDuongTietKiem> GetListCTBDTK(string MaPhieuThu)
        {
            try
            {
                SqlParameter maPhieuThu = new SqlParameter("maphieuthu", MaPhieuThu);
                return _dbContext.ExecuteStoredProcedureList<ViewChiTietBaoDuongTietKiem>("sp_LayDanhSachCTBDTK", maPhieuThu).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetListCTBDTK" + ex);
                return null;
            }
        }
        public List<ViewChiTietBDTK> GetListCTBDTK_v2(string MaPhieuThu)
        {
            try
            {
                SqlParameter maPhieuThu = new SqlParameter("maphieuthu", MaPhieuThu);
                return _dbContext.ExecuteStoredProcedureList<ViewChiTietBDTK>("sp_LayDanhSachCTBDTK_v2", maPhieuThu).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetListCTBDTK_v2" + ex);
                return null;
            }
        }
        #region Phần kiểm tra trước 
        public string XuLyTruocKhiInsertPhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt, ref string sessionId, bool isXuLyTruocKhiInsert = true, string giaTriTrenGiaoDien = "")
        {
            return null;
        }

        public string KiemTraDanhSachCocTruocKhiInsert(string maLoaiPhieu, PhieuThu model, DateTime ngayThu, List<ChiTietPhieuThu> httt)
        {          
            //lay danh sach coc ra de kiem tra
            string lstCoc = "";
            try
            {
                //neu la phieu tra no thi kiem tra MaPhieuLienQuan cua phieu tra (ma phieu no NKMPK, NTAUN,...)
                if (maLoaiPhieu.Substring(0,1) == "N")
                    lstCoc = model.MaPhieuLienQuan;
                //neu la cac loai phieu coc thi lay danh sach cac chi tiet phieu thu co loai hinh thanh toan 'DC'
                if (httt != null)
                {
                    foreach (ChiTietPhieuThu ct in httt)
                    {
                        string hinhThucThanhToan = "";
                        if (ct.HinhThucThanhToan != null)
                            hinhThucThanhToan = ct.HinhThucThanhToan.ToUpper();
                        else
                        {
                            if (!string.IsNullOrEmpty(ct.Temp))
                            {
                                if (ct.Temp.Length >= 2)
                                    hinhThucThanhToan = ct.Temp.ToUpper().Substring(ct.Temp.Length - 2, 2);
                            }
                        }
                        // phieu insert thi chua co hinhthucthanhtoan ma chi co Temp: "QuyChinh-TM"
                        //if ((ct.HinhThucThanhToan != null ? ct.HinhThucThanhToan.ToUpper() : ct.Temp.ToLower().Substring(ct.Temp.Length - 3, 2)) == "DC")
                        if (hinhThucThanhToan == "DC" && ct.SoTienThanhToan > 0)
                        {
                            if (lstCoc != "")
                                lstCoc += ",";
                            lstCoc += ct.SoThamChieu;
                        }
                    }
                }
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay gia trị trả về trước khi insert
                SqlParameter Ngay = new SqlParameter("NgayThu", ngayThu);
                SqlParameter MaLoaiPhieu = new SqlParameter("MaLoaiPhieu", maLoaiPhieu);
                SqlParameter DanhSachPhieuCoc = new SqlParameter("DanhSachPhieuCoc", lstCoc);

                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuThu_KiemTraCoc", MaLoaiPhieu, Ngay, DanhSachPhieuCoc, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return "";
        }
        #endregion
        #region phần export excel
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="MaLoaiPhieu"></param>
        /// <param name="Query"></param>
        /// <param name="loai">
        ///     - PhieuThu
        ///     - PhieuNo
        /// </param>
        /// <returns></returns>
        public DataTable XuatExcel(DateTime FromDate,DateTime ToDate,string MaLoaiPhieu, string Query, string Loai = "PhieuThu")
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter maloaiphieu = new SqlParameter("maloaiphieu", MaLoaiPhieu);
                SqlParameter query = new SqlParameter("query", Query);
                SqlParameter loai = new SqlParameter("loai", Loai);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_PhieuThu", fromdate, todate,maloaiphieu, query,loai);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel(" + FromDate + "," + ToDate + "," + MaLoaiPhieu + "):" ,ex);
                return null;
            }
        }

        public DataTable XuatExcelReceipted(DateTime FromDate, DateTime ToDate, string MaLoaiPhieu, string Query)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter maloaiphieu = new SqlParameter("maloaiphieu", MaLoaiPhieu);
                SqlParameter query = new SqlParameter("query", Query);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_PhieuThu_DaThuNo", fromdate, todate, maloaiphieu, query);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcelReceipted(" + FromDate + "," + ToDate + "," + MaLoaiPhieu + "):", ex);
                return null;
            }
        }

        public DataTable XuatExcel_TCOBX(DateTime FromDate, DateTime ToDate, string Query)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter query = new SqlParameter("query", Query);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_PhieuThuTCOBX", fromdate, todate, query);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel(" + FromDate + "," + ToDate + "):", ex);
                return null;
            }
        }
        public DataTable XuatExcel_NTKHA(DateTime FromDate, DateTime ToDate, string Query)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter query = new SqlParameter("query", Query);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_NTKHA", fromdate, todate, query);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel_NTKHA(" + FromDate + "," + ToDate + "):", ex);
                return null;
            }

           
        }
        public DataTable XuatExcel_NHANG(DateTime FromDate, DateTime ToDate, string Query)
        {
            try
            {
                SqlParameter loaiPhieuNo = new SqlParameter("loaiPhieuNo", "NHANG");
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter query = new SqlParameter("query", Query);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_PhieuNo",loaiPhieuNo, fromdate, todate, query);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel_NTKHA(" + FromDate + "," + ToDate + "):", ex);
                return null;
            }


        }

        public DataTable XuatExcel_GiaiTrinhNBHDV(DateTime FromDate, DateTime ToDate, string Query,string TinhTrang, string dagiaitrinh)
        {
            try
            {
                Query = Query.ToLower();
                SqlParameter pFrom = new SqlParameter("from", FromDate);//xử lý trước khi update
                SqlParameter pTo = new SqlParameter("to", ToDate);

                SqlParameter pQuery = new SqlParameter("query", Query);//xử lý trước khi update
                SqlParameter pTinhTrang = new SqlParameter("tinhTrang", TinhTrang);
                SqlParameter pDaGiaiTrinh = new SqlParameter("dagiaitrinh", dagiaitrinh);
                var result = _dbContext.ExecuteStoredProcedureDataTable("sp_SelectNoPhaiThuNBHDV_GiaiTrinh", pTinhTrang, pFrom, pTo, pQuery,pDaGiaiTrinh);
                return result;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel_GiaiTrinhNBHDV:", ex);

                return null;
            }
        }
            public DataTable XuatExcel_NBHDV(DateTime FromDate, DateTime ToDate, string Query, string tinhtrang = "")
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter query = new SqlParameter("query", Query);
                SqlParameter status = new SqlParameter("status", tinhtrang);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_NBHDV", fromdate, todate, query,status);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel_NBHDV(" + FromDate + "," + ToDate + "):", ex);
                return null;
            }
        }
        public DataTable XuatExcel_NDIVU(DateTime FromDate, DateTime ToDate, string Query, string TinhTrang)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter query = new SqlParameter("query", Query);
                SqlParameter tinhtrang = new SqlParameter("tinhtrang", TinhTrang);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_NDIVU", fromdate, todate, query, tinhtrang);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel_NDIVU(" + FromDate + "," + ToDate + "):", ex);
                return null;
            }
        }
        public List<DanhMucDienGiai> LoaiPhieuThu()
        {
            try
            {
                return _DanhMucDienGiaiRepository.Table.Where(x => (x.TableSuDung == "PhieuThu" || x.TableSuDung == "NoPhaiThu") && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Ten).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.LoaiPhieuThu" ,ex);
                return null;
            }
        }
        #endregion
        //phần thêm phiếu thu TPHKI
        public string CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt, List<ChiTietBanPhuKien> LPT, string type, bool isGoiXuLySauKhiInsertPhieuThu = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                //kiem tra ngay ThuCoc co NgayThuPhieu dich vu/ban xe... khong
                string kiemTraDanhSachCoc = KiemTraDanhSachCocTruocKhiInsert(model.MaLoaiPhieu,model, model.NgayThu, httt);
                if (!string.IsNullOrEmpty(kiemTraDanhSachCoc))
                    return kiemTraDanhSachCoc;
                model.MaLoaiPhieu = model.MaLoaiPhieu == "TKMPHKI" || type == "NKMPK" ? "TPHKI" : model.MaLoaiPhieu;
                //kiểm tra trước khi insert Phiếu Thu
                SqlParameter MPT1 = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi insert
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay gia trị trả về trước khi insert
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                SqlParameter SoHopDong = new SqlParameter("SoHopDong", model.SoHopDong == null? "": model.SoHopDong);
                SqlParameter BienSo = new SqlParameter("BienSo", model.BienSo == null?"":model.BienSo);
                SqlParameter TongCong = new SqlParameter("TongCong", model.TongCong);
                SqlParameter SoTienThu = new SqlParameter("SoTienThu", model.SoTienThu);
                
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuThu", MPT1,Ngay, message,SoHopDong,BienSo,TongCong,SoTienThu);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaPhieuThu = _commonService.CreateId(model.MaLoaiPhieu);
                //if (model.NgayHachToan == null)
                model.NgayHachToan = model.NgayThu;
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                if (model.BienSo != null)
                {
                    model.BienSo = model.BienSo.Replace(" ", "").Replace(".", "");
                }
                if (model.SoHopDong != null)
                {
                    model.SoHopDong = model.SoHopDong.Replace(" ", "").Replace(".", "");
                }
                //xu ly tong so tien thu duoi database
                //model.SoTienThu = 0;
                double soTien = 0; model.TongCong = 0; model.SoTienThu = 0;
                if (type != "TKMPHKI" && type != "NKMPK")
                {
                    foreach (var item in httt)
                    {
                        if (!string.IsNullOrEmpty(item.Temp) && item.IsDeleted == false)
                        {
                            var a = item.Temp.Split('-');
                            if (a.Length == 2)
                            {
                                item.Id = Guid.NewGuid();
                                item.MaPhieuThu = model.MaPhieuThu;
                                item.HinhThucThanhToan = a[1];
                                item.MaNganHang = a[0];
                                item.IsActive = true;
                                item.IsDeleted = false;
                                item.CreatedBy = uid;
                                item.CreatedDate = DateTime.Now;
                                item.UpdatedDate = d;
                                item.UpdatedBy = uid;
                                if (item.TinhTrang) //neu phieu bi treo thi ghi nhan ngay treo
                                    item.NgayTreoTien = model.NgayThu;
                                //else if (item.NgayTienVao == null) //neu phieu nay truoc do chua bi treo tien hoặc chưa bị chuyển tình trạng hết treo   thi cap nhat ngay vao tien la ngay thu
                                //    item.NgayTienVao = model.NgayThu;
                                _ctptRepository.Insert(item);
                                //       model.SoTienThu += item.SoTienThanhToan;
                                soTien += item.SoTienThanhToan;
                            }
                        }
                    }
                    model.TongCong = 0;
                    model.SoTienThu = soTien;
                }
                double TongTien = 0;
                if (LPT != null)
                    {
                        foreach (var temp in LPT)
                        {
                            model.GiaBan += temp.GiaBan;
                            model.GiamGia += temp.GiamGia;
                            model.TongCong += temp.ThucThu;
                            TongTien += temp.GiaVon + temp.TienCong;
                        }
                    }
                    model.ConLai = model.TongCong - soTien;

                if(type == "TKMPHKI")
                {
                    model.TongCong = 0;
                    model.ConLai = 0;
                }
                if (type == "NKMPK")
                {
                    model.TongCong = TongTien;
                }
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                //Lưu phiếu thu
                _phieuThuRepository.Insert(model);
                //Lưu lịch sử thao tác
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuThu, model.MaLoaiPhieu, "Create", "CreatePhieuThu", "Mã phiếu thu :" + model.MaPhieuThu + " mã loại phiếu :" + model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuThu)
                    _sothuService.XuLySauKhiInsertPhieuThu(model.MaPhieuThu, giaTriTrenGiaoDien);
                return "";
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.PhieuThuService.CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt): " ,ex);
                     return "Lỗi tạo mới phiếu thu";
            }
        }
        //public string getMaPhieuPhatSinh(string MaPhieuNo)
        //{
        //    ViewNoPhaiThu item = _VNPTRepository.Table.Where(x=>x.MaPhieuNo == MaPhieuNo).FirstOrDefault();
        //    string MaPhieuThu = _phieuThuRepository.Table.Where(x => x.MaPhieuThu == item.MaPhieuThu).FirstOrDefault().MaPhieuThu.ToString();
        //    return MaPhieuThu;
        //}
        #region
        public List<ViewPhieuThuCNO> ListThuCongNo(DateTime from, DateTime to, int p, ref int total, int pageSize, string tinhtrang)
        {
            IQueryable<ViewPhieuThuCNO> queryable = _ViewphieuThuCNORepository.Table
                .Where(x => x.MaLoaiPhieu == "TCONO" && x.IsDeleted == false && x.NgayThu >= from && x.NgayThu <= to);

            if (!string.IsNullOrEmpty(tinhtrang))
            {
                queryable = queryable.Where(x => x.TinhTrang == tinhtrang);
            }

            total = queryable.Count();

            return queryable
                .OrderByDescending(x => x.NgayThu)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public List<PhieuThu> GetCongNo(string maPhieuThu,string NCC)
        {
            try
            {
                var q = _phieuThuRepository.Table.Where(x => x.DoiTac == NCC && x.MaLoaiPhieu == "TCONO" && x.IsDeleted == false);
                List<PhieuThu> list = q.OrderByDescending(x => x.NgayThu).ToList();
                foreach (var temp in list)
                {
                    List<ChiTietPhieuThu> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuThu == maPhieuThu
                    && x.SoThamChieu == temp.MaPhieuThu
                    && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai.Value + TongSoTien;
                    temp.SoTienThu = TongSoTien;
                }
                return list;
            }
            catch (Exception ex)
            {
              
                    _log.WriteLog("PhieuThuService.GetCongNo:" ,ex);
                return null;
            }
        }
        public double GetNoKMConLai(string MaPhieuNo)
        {
            try
            {
                string NoConLai = string.Empty;
                SqlParameter PrMaPhieuNo = new SqlParameter("maphieuno", MaPhieuNo);
                SqlParameter PrNoConLai = new SqlParameter("conlai", NoConLai);
                PrNoConLai.Direction = ParameterDirection.Output;
                PrNoConLai.Size = 400;
                _dbContext.ExecuteStoredProcedure("sp_GetNoKMConLai", PrMaPhieuNo, PrNoConLai);
                var rs = PrNoConLai.Value.ToString();
                return PrNoConLai.Value.ToString() == "" ? 0 : double.Parse(PrNoConLai.Value.ToString());
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public List<ViewCongNo> ListCongNo(string MaPhieuThu)
        {
            try
            {
                SqlParameter param = new SqlParameter("MaPhieuThu", MaPhieuThu);
                var data = _dbContext.ExecuteStoredProcedureList<ViewCongNo>("sp_DanhSachCongNo", param);
                return data.ToList();
            }
            catch (Exception ex)
            {
              
                    _log.WriteLog("PhieuThuService.ListCongNo:" ,ex);
                return null;
            }
            
        }
        public bool KiemTraTonTaiChungTu(string SoChungTu)
        {
            PhieuThu pt = _phieuThuRepository.Table.Where(x => x.SoChungTu == SoChungTu && x.IsDeleted == false && x.MaLoaiPhieu == "TBAHI").FirstOrDefault();
            if (pt != null)
                return false;
            return true;
        }
        public bool KiemTraSoQuyetToan(string SoQuyetToan)
        {
            PhieuThu pt = _phieuThuRepository.Table.Where(x => x.MaLoaiPhieu == "TDIVU" && x.SoHopDong == SoQuyetToan && x.IsDeleted == false).FirstOrDefault();
            if (pt != null)
                return false;
            return true;
        }
        public bool KiemTraTonTaiSoQuyetToan(string SoQuyetToan)
        {
            REPAIR_ORDER_PYS item  = _PDVTRepository.Table.Where(x => x.REPAIRORDERNO == SoQuyetToan).FirstOrDefault();
            if (item != null)
                return true;
            return false;
        }
        public bool CheckSoKhung(string SoKhung)
        {
            ViewKhoXe KX = _VKXRepository.Table.Where(x=>x.SoKhung == SoKhung && x.TinhTrang == "N").FirstOrDefault();
            if (KX != null)
                return true;
            return false;
        }
        public ViewKhoXe GetXe(string SoKhung)
        {
            return _VKXRepository.Table.Where(x => x.SoKhung == SoKhung && x.TinhTrang == "N").FirstOrDefault();
        }
        public bool CheckMauXe(string MauXe, string SoKhung)
        {
            ViewKhoXe KX = _VKXRepository.Table.Where(x => x.SoKhung == SoKhung && x.CodeMau == MauXe && x.TinhTrang == "N").FirstOrDefault();
            if (KX != null)
                return true;
            return false;
        }
        public bool CheckMaXe(string MaXe, string SoKhung)
        {
            ViewKhoXe KX = _VKXRepository.Table.Where(x => x.SoKhung == SoKhung && x.CodeModel == MaXe && x.TinhTrang == "N").FirstOrDefault();
            if (KX != null)
                return true;
            return false;
        }
        public PhieuThu PT_BanXe(string MaPhieuThu)
        {
            return _phieuThuRepository.Table.Where(x=>x.MaPhieuThu == MaPhieuThu && x.IsDeleted == false).FirstOrDefault();
        }
        #endregion

        #region phan coc xe
        public List<ViewGiaoDichCoc> ChiTietDungCocXe(string MaPhieuThu)
        {
            try
            {
                SqlParameter maPhieuThu = new SqlParameter("MaPhieuThu", MaPhieuThu);
                return _dbContext.ExecuteStoredProcedureList<ViewGiaoDichCoc>("sp_ListViewGiaoDichCoc", maPhieuThu).OrderByDescending(x=>x.NgayPhatSinh).ToList();
                //return _ViewGiaoDichCocRepository.Table.Where(x=>x.SoThamChieu == MaPhieuThu && x.SoTien > 0 && x.IsActive == true && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.ChiTietDungCocXe", ex);
                return null;
            }
        }

        public List<ViewTBHXH> getListThuBHXH(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var list = _ViewTBHXHRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.MaLoaiPhieu == "TBHXH");
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.SoChungTu.ToString().Contains(query)
                    || x.MaPhieuThu.ToLower().Contains(query)
                    || x.DoiTac.ToLower().Contains(query)

                    || query.Contains(x.SoChungTu.ToString().ToLower())
                    || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.DoiTac.ToLower())

                    );
                }
                var result = list.OrderBy(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.getListThuBHXH: ", ex);
                return null;
            }
        }
        public List<ViewTBHXH> getListThuGTXe(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var list = _ViewTBHXHRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.MaLoaiPhieu == "T2GTX");
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.SoChungTu.ToString().Contains(query.ToLower())
                    || x.MaPhieuThu.ToLower().Contains(query.ToLower())
                    || x.DoiTac.ToLower().Contains(query.ToLower())
                    || x.ThongTinKhac.ToLower().Contains(query.ToLower())
                    || x.DoiTac.ToLower().Contains(query.ToLower())

                    || query.Contains(x.SoChungTu.ToString().ToLower())
                    || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.DoiTac.ToLower())

                    );
                }
                var result = list.OrderBy(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.getListThuGTXe: ", ex);
                return null;
            }
        }

        public List<ViewTBHXH> getListThuHoKHAC(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                var list = _ViewTBHXHRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.MaLoaiPhieu == "T2THK");
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.SoChungTu.ToString().Contains(query)
                    || x.MaPhieuThu.ToLower().Contains(query)
                    || x.DoiTac.ToLower().Contains(query)

                    || query.Contains(x.SoChungTu.ToString().ToLower())
                    || query.Contains(x.MaPhieuThu.ToLower())
                    || query.Contains(x.DoiTac.ToLower())

                    );
                }
                var result = list.OrderBy(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.getListThuHoKHAC: ", ex);
                return null;
            }
        }
        public List<ViewPhieuThu> GetThuHo(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                query = query.ToLower();
                //var q = _ViewphieuThuRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.MaLoaiPhieu == id);
                var q = _ViewphieuThuRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.MaLoaiPhieu == id && (x.TongCong > 0 || (x.TongCong == 0 && string.IsNullOrEmpty(x.SoHopDong))));
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaPhieuThu.ToLower().Contains(query)
                    || x.SoChungTu.ToLower().Contains(query)
                    || x.Hoten.ToLower().Contains(query.ToLower())
                    || x.CTBH.ToLower().Contains(query)

                    || query.Contains(x.MaPhieuThu.ToString().ToLower())
                    || query.Contains(x.SoChungTu.ToLower())
                    || query.Contains(x.CTBH.ToString().ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();

            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetThuHo: ", ex);
                return null;
            }
        }
        public List<ViewNoPhaiThu> GetNoPhaiThuHo(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang)
        {
            try
            { 
                    var q = _VNPTRepository.Table.Where(x => x.NgayNo >= from && x.NgayNo <= to && x.LoaiPhieuNo == id);
                    if (!string.IsNullOrEmpty(query))
                    {
                        query = query.ToLower();
                            q = q.Where(x => (x.KhachHang.ToLower().Contains(query)
                           || x.ChungTuThu.ToLower().Contains(query)
                           || x.TinhTrang.ToLower().Contains(query)
                           || x.MaPhieuNo.ToLower().Contains(query)
                           || x.Hoten.ToLower().Contains(query.ToLower())
                           || x.MaPhieuPhatSinh.ToLower().Contains(query)
                           || x.GhiChu.ToLower().Contains(query) //sau nay bo phan nay de tranh bi cham, chi de thoi gian ban dau luc khoi tao
                           || x.SoHopDong.ToLower().Contains(query)
                           || x.NoiDung.ToLower().Contains(query)
                           || x.BienSo.ToLower().Contains(query)
                           || x.KhachHang.ToLower().Contains(query)
                           || x.DienThoai.ToLower().Contains(query)
                        //   || query.Contains(x.KhachHang.ToLower())
                        || query.Contains(x.ChungTuThu.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        || query.Contains(x.MaPhieuPhatSinh.ToLower())
                        || query.Contains(x.MaPhieuNo.ToLower())
                        //  || query.Contains(x.GhiChu.ToLower())
                        || query.Contains(x.SoHopDong.ToLower())
                        || query.Contains(x.BienSo.ToLower())
                           //   || query.Contains(x.NoiDung.ToLower())
                           )
                           && x.SoTienNo > 0
                           );
                        }
                    else
                        q = q.Where(x => x.SoTienNo > 0);
                if (tinhtrang == "C")
                {
                    q = q.Where(x => x.SoTienConLai != 0);
                }
                else if (tinhtrang == "H")
                {
                    q = q.Where(x => x.SoTienConLai == 0);
                }
                var result = q.OrderByDescending(x => x.SoHopDong).ToList();
                    total = result.Count;
                    return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
                }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoPhaiThuHo:", ex);
                return null;
            }
        }
        public List<ViewNoDaThu> GetNoDaThuHo(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            query = query.ToLower();
            try
            {
                var q = _VNDTRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to && x.LoaiPhieuNo == id);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.KhachHang.ToLower().Contains(query)
                    || x.ChungTuThu.ToLower().Contains(query)
                    || x.TinhTrang.ToLower().Contains(query)
                    || x.MaPhieuNo.ToLower().Contains(query)
                    || x.SoPhieuQuyetToan.ToLower().Contains(query)
                    || query.Contains(x.ChungTuThu.ToLower())
                    || query.Contains(x.MaPhieuNo.ToLower())
                    || query.Contains(x.BienSo.ToLower())
                    || query.Contains(x.SoPhieuQuyetToan.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.NgayThu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThuHo:", ex);
                return null;
            }
        }
        #endregion

        public double TienThucGop()
        {
            try
            {
                List<PhieuThu> list = _phieuThuRepository.Table.Where(x => x.MaLoaiPhieu == "TGOVO" && x.IsDeleted == false).ToList();
                double TongTien = 0;
                foreach(var item in list)
                {
                    TongTien += item.TongCong;
                }
                return TongTien;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.TienThucGop:", ex);
                return 0;
            }
        }

        public PhieuThu CongNoHonDa(string MaPhieuThu)
        {
            try
            {
                return _phieuThuRepository.Table.Where(x => x.IsDeleted == false && x.MaPhieuLienQuan == MaPhieuThu).FirstOrDefault();
            }
            catch(Exception ex)
            {
                _log.WriteLog("Error CongNoHonDa " + ex.ToString());
                return null;
            }
        }
        public string PhieuTKHACByCongNo(string MaPhieuThu)
        {
            try
            {
                return _phieuThuRepository.Table.Where(x => x.IsDeleted == false && x.MaPhieuLienQuan == MaPhieuThu).Select(x => x.MaPhieuThu).FirstOrDefault();
            }
            catch(Exception ex)
            {
                _log.WriteLog("Error PhieuTKHACByCongNo " + ex.ToString());
                return "";
            }
        }

        public DataTable KiemTraThongTinNhapSoHoaDonCongNoBHDV(string sLstMaPhieuNo)
        {
            try
            {
                SqlParameter lstMaPhieuNo = new SqlParameter("lstMaPhieuNo", sLstMaPhieuNo);                
                return _dbContext.ExecuteStoredProcedureDataTable("sp_KiemTraThongTinNhapSoHoaDonCongNoBHDV", lstMaPhieuNo);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.KiemTraThongTinNhapSoHoaDonCongNoBHDV("+ sLstMaPhieuNo +"):", ex);
                return null;
            }
        }

        public double LayGiaVonBDTK(string ma)
        {
            try
            {
                SqlParameter maBDTK = new SqlParameter("maBDTK", ma);
                SqlParameter giaVonBDTK = new SqlParameter("GiaVonBDTK", SqlDbType.Float);
                
                giaVonBDTK.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_LayGiaVonBDTK", maBDTK,giaVonBDTK);
                return float.Parse( giaVonBDTK.Value.ToString());
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.LayGiaVonBDTK(" + ma + "):", ex);
                return 0;
            }
        }
        public bool CheckPhieuThuHHGP(string MaPhieuBanXe)
        {
            try
            {
                string result = "";
                SqlParameter PrMaPhieuBanXe = new SqlParameter("MaPhieuBanXe", MaPhieuBanXe);
                SqlParameter Prresult = new SqlParameter("result", result);
                Prresult.Size = 4000;
                Prresult.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_CheckPhieuThuHHGP", PrMaPhieuBanXe, Prresult);
                var resulst = Prresult.Value.ToString();
                return resulst == "" ? false : true;
            }
            catch(Exception ex)
            { return false; }
        }
        public bool CheckChiNoKMPK(string id)
        {
            try
            {
                var lst = _phieuThuRepository.Table.Where(x => x.MaPhieuLienQuan == id).ToList();
                if (lst.Count > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            { return false; }
        }
        #region  Cập nhật hóa đơn xác nhận công nợ

        public List<ViewNoPhaiThuBHDV> GetListHDXNCN(DateTime from, DateTime to, string query, string type, int p, ref int total, int pageSize, string tinhtrang)
        {
            try
            {
                SqlParameter PrFromDate = new SqlParameter("FromDate ", from);
                SqlParameter PrToDate = new SqlParameter("ToDate ", to);
                SqlParameter PrQuery = new SqlParameter("Query", query);
                SqlParameter PrType = new SqlParameter("Type ", type);
                SqlParameter PrTinhtrang = new SqlParameter("Tinhtrang", tinhtrang);
                var q= _dbContext.ExecuteStoredProcedureList<ViewNoPhaiThuBHDV>("sp_GetListHDXNCN", PrFromDate, PrToDate, PrQuery, PrType, PrTinhtrang);

                // Lấy tổng số lượng bản ghi trước khi phân trang
                total = q.Count();

                // Sắp xếp, phân trang và lấy dữ liệu
                var result = q.OrderByDescending(x => x.LoaiPhieuNo)
                              .ThenByDescending(y => y.NgayThu)
                              .Skip((p - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThus:", ex);
                return new List<ViewNoPhaiThuBHDV>();
            }
        }

        public List<ViewNoPhaiThuBHDV> GetNoPhaiThu_GiaiTrinh(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang, string dagiaitrinh)
        {
            try
            {
                query = query.ToLower();
                SqlParameter pFrom = new SqlParameter("from", from);//xử lý trước khi update
                SqlParameter pTo = new SqlParameter("to", to);

                SqlParameter pQuery = new SqlParameter("query", query);//xử lý trước khi update
                SqlParameter pTinhTrang = new SqlParameter("tinhTrang", tinhtrang);
                SqlParameter pDaGiaiTrinh = new SqlParameter("dagiaitrinh", dagiaitrinh);
                var result = _dbContext.ExecuteStoredProcedureList<ViewNoPhaiThuBHDV>("sp_SelectNoPhaiThu_GiaiTrinh", pTinhTrang, pFrom, pTo, pQuery, pDaGiaiTrinh).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNoDaThus:", ex);

                return null;
            }
        }
        public DataTable XuatExcel_CapNhatHoaDonCongNo(DateTime FromDate, DateTime ToDate, string Type, string Query, string TinhTrang)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter type = new SqlParameter("type", Type);
                SqlParameter query = new SqlParameter("query", Query);
                SqlParameter status = new SqlParameter("status", TinhTrang);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_CapNhatHoaDonCongNo", fromdate, todate, type, query, status);
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel_NBHDV(" + FromDate + "," + ToDate + "):", ex);
                return null;
            }
        }
        public DataTable XuatExcel_GiaiTrinh(DateTime FromDate, DateTime ToDate, string Query, string TinhTrang, string dagiaitrinh)
        {
            try
            {
                Query = Query.ToLower();
                SqlParameter pFrom = new SqlParameter("from", FromDate);//xử lý trước khi update
                SqlParameter pTo = new SqlParameter("to", ToDate);

                SqlParameter pQuery = new SqlParameter("query", Query);//xử lý trước khi update
                SqlParameter pTinhTrang = new SqlParameter("tinhTrang", TinhTrang);
                SqlParameter pDaGiaiTrinh = new SqlParameter("dagiaitrinh", dagiaitrinh);
                var result = _dbContext.ExecuteStoredProcedureDataTable("sp_SelectNoPhaiThu_GiaiTrinh", pTinhTrang, pFrom, pTo, pQuery, pDaGiaiTrinh);
                return result;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.XuatExcel_GiaiTrinhNBHDV:", ex);

                return null;
            }
        }

        #endregion

        public string CreateCTBaoDuongTietKiem(string MaPhieuThu, List<ChiTietBaoDuongTietKiem> list)
        {
            try
            {
                var PT = _phieuThuRepository.Table.SingleOrDefault(x => x.MaPhieuThu == MaPhieuThu);
                list = list.Where(x => x.GiaNiemYet != 0).ToList();
                if (list != null && PT != null)
                {
                    var dem = _CTBPKRepository.Table.Count();
                    double ThucThu = 0;
                    foreach (var tt in list)
                    {
                        ThucThu += tt.ThucBan;
                    }
                    foreach (var item in list)
                    {

                        if (item.MaBDTK == null && item.GiaBan == 0 && item.GiamGia == 0 && item.ThucBan == 0)
                            continue;
                        dem = dem + 1;
                        item.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                        item.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                        item.SoTienSuDung = 0;
                        item.ConLai = item.GiaNiemYet;
                        //1 cap: 7 thang
                        //2-3 cap: 14 thang
                        //tren 4 cap: 26 thang
                        int soThangSuDung = 7;
                        soThangSuDung = list.Count < 2 ? 7 : list.Count < 4 ? 14 : 26;
                        //item.HanSuDung = list.Count < 2 ? PT.NgayThu.AddMonths(7) : list.Count < 4 ? PT.NgayThu.AddMonths(14) : PT.NgayThu.AddMonths(26);
                        item.HanSuDung = PT.NgayThu.AddMonths(soThangSuDung);
                        item.IsDeleted = false;
                        item.CreatedDate = DateTime.Now;
                        item.UpdatedDate = DateTime.Now;
                        item.MaPhieuThu = MaPhieuThu;

                        _CTBDTKRespository.Insert(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.CreateCTBaoDuongTietKiem" + ex);
                if (ex.InnerException != null)
                    _log.WriteLog("PhieuThuService.CreateCTBaoDuongTietKiem" + ex.InnerException);
                return "Lỗi thêm chi tiết bảo dưỡng tiết kiệm";
            }
            return "";
        }

        //Lộc thêm phiếu thu bảo dưỡng tiết kiệm
        public string CreatePhieuThuBDTK(PhieuThu model, List<ChiTietPhieuThu> httt, List<ChiTietBaoDuongTietKiem> CTBDTK
           , bool isGoiXuLySauKhiInsertPhieuThu = true
           , string giaTriTrenGiaoDien = ""
           )
        {
            try
            {

                //kiểm tra trước khi insert Phiếu Thu
                SqlParameter MPT1 = new SqlParameter("MaLoaiPhieu", model.MaLoaiPhieu);//xử lý trước khi insert
                SqlParameter message = new SqlParameter("Message", "");
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay gia trị trả về trước khi insert

                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiInsertPhieuThu", MPT1, Ngay, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();



                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.MaLoaiPhieu = "TBDTK";
                model.MaPhieuThu = _commonService.CreateId(model.MaLoaiPhieu);
                model.NgayHachToan = model.NgayThu;
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                if (model.BienSo != null)
                {
                    model.BienSo = model.BienSo.Replace(" ", "").Replace(".", "");
                }
                if (model.SoHopDong != null)
                {
                    model.SoHopDong = model.SoHopDong.Replace(" ", "").Replace(".", "");
                }
                //xu ly tong so tien thu duoi database
                //model.SoTienThu = 0;
                double soTien = 0;
                foreach (var item in httt)
                {
                    if (!string.IsNullOrEmpty(item.Temp) && item.IsDeleted == false)
                    {
                        var a = item.Temp.Split('-');
                        if (a.Length == 2)
                        {
                            item.Id = Guid.NewGuid();
                            item.MaPhieuThu = model.MaPhieuThu;
                            item.HinhThucThanhToan = a[1];
                            item.MaNganHang = a[0];
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = uid;
                            item.CreatedDate = DateTime.Now;
                            item.UpdatedDate = d;
                            item.UpdatedBy = uid;
                            if (item.TinhTrang) //neu phieu bi treo thi ghi nhan ngay treo
                                item.NgayTreoTien = model.NgayThu;
                            //else if (item.NgayTienVao == null) //neu phieu nay truoc do chua bi treo tien hoặc chưa bị chuyển tình trạng hết treo   thi cap nhat ngay vao tien la ngay thu
                            //    item.NgayTienVao = model.NgayThu;
                            _ctptRepository.Insert(item);
                            //       model.SoTienThu += item.SoTienThanhToan;
                            soTien += item.SoTienThanhToan;
                        }
                    }
                }
                // Tính Tổng gói bao dưỡng tiết kiệm
                double TongGiaBan = 0, TongThucThu = 0, TongGiaNiemYet = 0;
                foreach (ChiTietBaoDuongTietKiem item in CTBDTK)
                {
                    TongGiaBan += item.GiaBan;
                    //TongGiamGia += item.GiamGia;
                    TongGiaNiemYet += item.GiaNiemYet;
                    model.LoaiBaoHiem = item.MaBDTK + ",";
                }

                model.GiaBan = TongGiaBan;
                model.GiaNiemYet = TongGiaNiemYet;
                //model.GiamGia = TongGiamGia;
                model.SoTienThu = TongGiaBan - model.GiamGia.Value;
                model.TongCong = TongGiaBan - model.GiamGia.Value;
                model.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                _phieuThuRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaPhieuThu, model.MaLoaiPhieu, "Create", "CreatePhieuThu", "Mã phiếu thu :" + model.MaPhieuThu + " mã loại phiếu :" + model.MaLoaiPhieu + " Tổng cộng :" + model.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isGoiXuLySauKhiInsertPhieuThu)
                    _sothuService.XuLySauKhiInsertPhieuThu(model.MaPhieuThu, giaTriTrenGiaoDien);
                return "";
            }
            catch (Exception ex)
            {

                _log.WriteLog("PhieuThuService.PhieuThuService.CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt): ", ex);
                return "Lỗi tạo mới phiếu thu";
            }
        }
        public string UpdatePhieuThuBDTK(PhieuThu model, List<ChiTietPhieuThu> httt, List<ChiTietBaoDuongTietKiem> CTBDTK, ref string sessionId, bool isXuLySauKhiUpdate = true, string giaTriTrenGiaoDien = "")
        {
            try
            {
                giaTriTrenGiaoDien = giaTriTrenGiaoDien.Replace("<formdata>", "<formdata>" + "<Browser>" + _clientBrowser + "</Browser>");
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == model.MaPhieuThu && x.IsDeleted == false && x.IsActive == true);
                if (obj == null)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Không tìm thấy phiếu thu " + obj.MaPhieuThu + " để update.");
                    return "Không tồn tại phiếu";
                }
                if (obj.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss") != model.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss"))
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Đã có người cập nhật phiếu lúc. Giờ update trên form:" + model.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss") + "; giờ update database:" + obj.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss"));
                    return "Đã có người cập nhật phiếu lúc " + obj.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                if (obj.IsActive == false)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo");
                    return "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo";
                }
                if (CheckPhieuThuHHGP(model.MaPhieuThu) == true && model.HoaHong != obj.HoaHong)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo");
                    return " _Không thể cập nhật hoa hồng góp";
                }
                SqlParameter MPT1 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuThu", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                //lay gia tri sessionid trả ra sau xử lý
                sessionId = sessionid.Value.ToString();
                //neu la cac phieu co su dung coc thi phai chay lai coc
                if (obj.MaLoaiPhieu == "TBDTK" || obj.MaLoaiPhieu == "T2GTX" || obj.MaLoaiPhieu == "TGHBH" || obj.MaLoaiPhieu == "TBAHI")
                {
                    if (obj.HoTen != model.HoTen
                                    || obj.BienSo != model.BienSo
                                    || obj.SoHopDong != model.SoHopDong
                                    )
                    {
                        SqlParameter pSessionId = new SqlParameter("SessionId", sessionid.Value);
                        SqlParameter pMaPhieuThu = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);
                        _dbContext.ExecuteStoredProcedure("sp_XuLyXoaChiTietPhieuThuSuDungCoc", pSessionId, pMaPhieuThu);
                    }
                }
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.BienSo = model.BienSo;
                obj.ConNo = model.ConNo;
                obj.CTBH = model.CTBH;
                obj.DiaChi = model.DiaChi;
                obj.DienThoai = model.DienThoai;
                obj.DoiTac = model.DoiTac;
                obj.DoiTacTraLai = model.DoiTacTraLai;
                obj.DVCV = model.DVCV;
                obj.GhiChu = model.GhiChu;
                obj.GiaBan = model.GiaBan;
                obj.GiamGia = model.GiamGia;
                obj.GiaVon = model.GiaVon;
                obj.HoTen = model.HoTen;
                obj.KeToanTruong = model.KeToanTruong;
                obj.LoaiXe = model.LoaiXe;
                obj.MauXe = model.MauXe;
                obj.MaXe = model.MaXe;
                obj.NgayThu = model.NgayThu;
                obj.NgayHopDong = model.NgayHopDong;
                obj.ThongTinKhac = model.ThongTinKhac;
                obj.NguoiLapPhieu = model.NguoiLapPhieu;
                obj.NguoiNopTien = model.NguoiNopTien;
                obj.NgayHachToan = model.NgayThu;
                obj.SoHoaDon = model.SoHoaDon;
                obj.NguoiThuTien = model.NguoiThuTien == null ? uid : model.NguoiThuTien;
                obj.NguoiUngTien = model.NguoiUngTien;
                obj.NhaCungCap = model.NhaCungCap;
                obj.NoiDung = model.NoiDung;
                obj.SoChungTu = model.SoChungTu;
                obj.SoHopDong = model.SoHopDong;
                obj.SoTienUng = model.SoTienUng;
                obj.TinhTrangPhieu = model.TinhTrangPhieu;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.NgayHachToan = obj.NgayThu;
                obj.HoaHongTaiXe = model.HoaHongTaiXe;
                obj.NguoiDuyetHHTX = model.NguoiDuyetHHTX;
                obj.HoaHong = model.HoaHong;
                obj.TienTraBH = model.TienTraBH;
                obj.NguoiBaoLanh = model.NguoiBaoLanh;
                obj.SoKhung = model.SoKhung;
                obj.LoaiBaoHiem = model.LoaiBaoHiem;
                double soTien = 0;
                foreach (var item in httt)
                {
                    var data = _ctptRepository.Table.FirstOrDefault(x => x.Id == item.Id);
                    if (!item.IsDeleted && !string.IsNullOrEmpty(item.Temp))
                    {
                        bool flag = true;
                        if (data == null)
                        {
                            data = new ChiTietPhieuThu();
                            data.Id = Guid.NewGuid();
                            data.IsDeleted = false;
                            data.IsActive = true;
                            data.CreatedBy = uid;
                            data.CreatedDate = d;
                            flag = false;
                        }
                        var a = item.Temp.Split('-');
                        data.MaPhieuThu = obj.MaPhieuThu;
                        data.HinhThucThanhToan = a[1];
                        data.MaNganHang = a[0];
                        data.SoThamChieu = item.SoThamChieu;
                        data.TinhTrang = item.TinhTrang;
                        //data.GhiChu = item.GhiChu;
                        data.SoTienThanhToan = item.SoTienThanhToan;
                        data.UpdatedBy = uid;
                        data.UpdatedDate = d;
                        if (item.TinhTrang) //neu phieu bi treo thi ghi nhan ngay treo
                            data.NgayTreoTien = model.NgayThu;
                        //else if (item.NgayTienVao == null) //neu phieuu nay truoc do chua bi treo tien hoặc chưa bị chuyển tình trạng hết treo   thi cap nhat ngay vao tien la ngay thu
                        //    item.NgayTienVao = model.NgayThu;

                        if (!flag)
                        {
                            _ctptRepository.Insert(data);
                        }
                        else
                        {
                            _ctptRepository.Update(data);
                        }

                        soTien += item.SoTienThanhToan;
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
                double TongGiaBan = 0, TongGiamGia = 0, TongThucThu = 0, TongGiaNiemYet = 0;
                foreach (ChiTietBaoDuongTietKiem item in CTBDTK)
                {
                    TongGiaBan += item.GiaBan;
                    //TongGiamGia += item.GiamGia;
                    TongThucThu += item.ThucBan;
                    TongGiaNiemYet += item.GiaNiemYet;
                    model.LoaiBaoHiem = item.MaBDTK + ",";
                }
                obj.GiaNiemYet = TongGiaNiemYet;
                obj.GiaBan = TongGiaBan;
                //obj.GiamGia = TongGiamGia;
                model.SoTienThu = TongGiaBan - model.GiamGia.Value;
                model.TongCong = TongGiaBan - model.GiamGia.Value;
                obj.TongCong = model.TongCong;
                obj.LoaiBaoHiem = model.LoaiBaoHiem.Substring(0, (model.LoaiBaoHiem.Length - 1));
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuThu", "Mã phiếu thu :" + obj.MaPhieuThu + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                if (isXuLySauKhiUpdate)
                {
                    SqlParameter MPT2 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý sau khi update
                    MPT2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                    SessionId2.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);//xử lý sau khi update
                    pGiaTriTrenGiaoDien.Direction = System.Data.ParameterDirection.Input;
                    SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                    SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                    Error.Direction = System.Data.ParameterDirection.Output;
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2, SessionId2, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Error);
                    if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                    {
                        int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                        if (gioGuiEmailLast != GioGuiEmail)
                        {
                            MailHelper Mail = new MailHelper();
                            string Content = ContentMail(model.MaPhieuThu);
                            //Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HBH", Content + " " + Error.Value.ToString());
                            //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HBH", Content + " " + Error.Value.ToString());
                            gioGuiEmailLast = GioGuiEmail;
                        }
                        else
                        {
                            if (Send == false)
                            {
                                MailHelper Mail = new MailHelper();
                                string Content = ContentMail(model.MaPhieuThu);
                                //Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HBH", Content + " " + Error.Value.ToString());
                                //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                                Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HBH", Content + " " + Error.Value.ToString());
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

                _log.WriteLog("PhieuThuService.UpdatePhieuThu: ", e);

                return "ERROR:" + e.ToString();
            }
        }
        public string EditCTBDTK(string MaPhieuThu, List<ChiTietBaoDuongTietKiem> list)
        {
            try
            {
                double ThucThu = 0;
                foreach (var tt in list)
                {
                    ThucThu += tt.ThucBan;
                }
                var lstOld = _CTBDTKRespository.Table.Where(x => x.MaPhieuThu == MaPhieuThu).ToList();
                foreach (var item in lstOld)
                {
                    var obj = list.FirstOrDefault(x => x.MaBDTK == item.MaBDTK);
                    if (obj == null)
                    {
                        item.IsDeleted = true;
                        item.UpdatedDate = DateTime.Now;
                        item.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                        _CTBDTKRespository.Update(item);
                    }
                }
                var PT = _phieuThuRepository.Table.SingleOrDefault(x => x.MaPhieuThu == MaPhieuThu);
                list = list.Where(x => x.GiaNiemYet != 0).ToList();
                int HSD = list.Count * 6 + 1;
                foreach (var item in list)
                {
                    item.MaPhieuThu = MaPhieuThu;
                    if (item.MaBDTK == null && item.GiaBan == 0 && item.GiamGia == 0 && item.ThucBan == 0)
                    {
                        ChiTietBaoDuongTietKiem item_Isdelete = _CTBDTKRespository.Table.Where(x => x.MaPhieuThu == item.MaPhieuThu && x.MaBDTK == item.MaBDTK && x.IsDeleted == false).FirstOrDefault();
                        if (item_Isdelete != null)
                        {
                            item_Isdelete.IsDeleted = true;
                            _CTBDTKRespository.Update(item_Isdelete);
                        }
                    }
                    else
                    {
                        ChiTietBaoDuongTietKiem temp = _CTBDTKRespository.Table.Where(x => x.MaPhieuThu == MaPhieuThu && x.MaBDTK == item.MaBDTK && x.IsDeleted == false).FirstOrDefault();
                        if (temp == null)
                        {
                            if (item.MaBDTK == null && item.GiaBan == 0 && item.GiamGia == 0 && item.ThucBan == 0)
                                continue;
                            else
                            {
                                item.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                                item.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                                item.CreatedDate = DateTime.Now;
                                item.UpdatedDate = DateTime.Now;
                                item.SoTienSuDung = 0;
                                item.HanSuDung = list.Count < 2 ? item.HanSuDung : list.Count < 4 ? PT.NgayThu.AddMonths(18) : PT.NgayThu.AddMonths(30);
                                item.ConLai = item.GiaNiemYet;
                                item.IsDeleted = false;
                                item.MaPhieuThu = MaPhieuThu;
                                _CTBDTKRespository.Insert(item);
                            }
                        }
                        else
                        {
                            temp.MaBDTK = item.MaBDTK;
                            temp.GiaBan = item.GiaBan;
                            temp.GiamGia = item.GiamGia;
                            temp.ThucBan = item.ThucBan;
                            temp.HanSuDung = item.HanSuDung;
                            temp.SoTienSuDung = item.SoTienSuDung;
                            temp.ConLai = item.GiaNiemYet;
                            temp.HanSuDung = PT.NgayThu.AddMonths(HSD);
                            temp.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                            temp.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                            temp.UpdatedDate = DateTime.Now;
                            temp.CreatedDate = DateTime.Now;
                            _CTBDTKRespository.Update(temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.EditCTBDTK" + ex);
                if (ex.InnerException != null)
                    _log.WriteLog("PhieuThuService.EditCTBDTK" + ex.InnerException);
                return "Lỗi cập nhật chi tiết bảo dưỡng tiết kiệm";
            }
            return "";
        }
        public string UpdateBDTK(List<ChiTietBaoDuongTietKiem> lstBDTK, string MaPhieuThu)
        {
            if (lstBDTK == null)
                return "";
            try
            {
                foreach (var item in lstBDTK)
                {

                    ChiTietBaoDuongTietKiem temp = _CTBDTKRespository.Table.Where(x => x.MaPhieuThu == item.MaPhieuThu && x.MaBDTK == item.MaBDTK && x.IsDeleted == false).FirstOrDefault();
                    if (temp != null)
                    {
                        temp.MaBDTK = item.MaBDTK;
                        temp.SoTienSuDung = item.SoTienSuDung;
                        temp.ConLai = temp.GiaNiemYet - item.SoTienSuDung;
                        temp.MaPhieuSuDung = item.SoTienSuDung > 0 ? MaPhieuThu : "";
                        temp.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                        temp.UpdatedDate = DateTime.Now;
                        _CTBDTKRespository.Update(temp);

                    }
                }
                return "";
            }
            catch (Exception Ex)
            {
                return Ex.ToString();
            }
        }
        public string CheckThuNoHHGP(string MaPhieuNo, DateTime NgayNo)
        {
            try
            {
                SqlParameter PrMaPhieuNo = new SqlParameter("MaPhieuNo", MaPhieuNo);
                SqlParameter PrNgayNo = new SqlParameter("NgayNo", NgayNo); 
                SqlParameter PrUserId = new SqlParameter("UserId", CurrentUser.UserId);
                SqlParameter PrOutPut = new SqlParameter("Message", "");

                PrOutPut.Size = 4000;
                PrOutPut.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_CheckThuNoHHGP", PrMaPhieuNo, PrNgayNo, PrUserId, PrOutPut);
                return PrOutPut.Value.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string Update_SoTienNo_NoHHGP(string MaPhieuNo, DateTime NgayNo, double SoTienNo)
        {
            try
            {
                SqlParameter PrMaPhieuNo = new SqlParameter("MaPhieuNo", MaPhieuNo);
                SqlParameter PrNgayNo = new SqlParameter("NgayNo", NgayNo);
                SqlParameter PrSoTienNo = new SqlParameter("SoTienNo", SoTienNo);
                SqlParameter PrUser = new SqlParameter("User", CurrentUser.UserId);
                SqlParameter PrOutPut = new SqlParameter("Message", "");

                PrOutPut.Size = 4000;
                PrOutPut.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_Update_SoTienNo_NoHHGP", PrMaPhieuNo, PrNgayNo, PrSoTienNo, PrUser, PrOutPut);
                if (PrOutPut.Value.ToString() == "")
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, MaPhieuNo, "NHHGP", "UPDATE", "UpdateSoTienNoHHGP", "Mã phiếu nợ :" + MaPhieuNo + " mã loại phiếu : NHHGP, SoTienNo:" + SoTienNo, _authenticationService.GetAuthenticatedUser().UserId);
                return PrOutPut.Value.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string Update_CKTM(string SoKhung, double SoTienChietKhau, string TenChuongTrinh)
        {
            try
            {
                SqlParameter PrSoKhung = new SqlParameter("soKhung", SoKhung);
                SqlParameter PrSoTienChietKhau = new SqlParameter("SoTienChietKhau", SoTienChietKhau);
                SqlParameter PrTenChuongTrinh = new SqlParameter("tenChuongTrinh", TenChuongTrinh);
                SqlParameter PrOutPut = new SqlParameter("Message", "");
                PrOutPut.Size = 4000;
                PrOutPut.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_ImportChietKhauThuongMai", PrSoKhung, PrSoTienChietKhau, PrTenChuongTrinh, PrOutPut);
                return PrOutPut.Value.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string UpdatePhieuThuHH(PhieuThu model)
        {
            try
            {
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == model.MaPhieuThu && x.IsDeleted == false && x.IsActive == true);
                if (obj.HoaHong == model.HoaHong)
                    return "";
                if (CheckPhieuThuHHGP(model.MaPhieuThu) == true && model.HoaHong != obj.HoaHong)
                {
                    _log.WriteLog("PhieuThuService.UpdatePhieuThu", "Phiếu đã bị khóa do đã kết chuyển hoặc phiếu khởi tạo");
                    return " _Không thể cập nhật hoa hồng góp";
                }
                SqlParameter MPT1 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuThu", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                //lay gia tri sessionid trả ra sau xử lý
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.HoaHong = model.HoaHong;
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuThu", "Mã phiếu thu :" + obj.MaPhieuThu + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                SqlParameter MPT2 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý sau khi update
                MPT2.Direction = System.Data.ParameterDirection.Input;
                SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                SessionId2.Direction = System.Data.ParameterDirection.Input;
                SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", "");//xử lý sau khi update
                pGiaTriTrenGiaoDien.Direction = System.Data.ParameterDirection.Input;
                SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                Error.Direction = System.Data.ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2, SessionId2, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Error);
                if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                {
                    int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                    if (gioGuiEmailLast != GioGuiEmail)
                    {
                        MailHelper Mail = new MailHelper();
                        string Content = ContentMail(model.MaPhieuThu);
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
                            string Content = ContentMail(model.MaPhieuThu);
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

                _log.WriteLog("PhieuThuService.UpdatePhieuThu: ", e);

                return "ERROR:" + e.ToString();
            }
        }
        public string UpdatePhieuThuNoKhuyenMai(PhieuThu model)
        {
            try
            {
                var obj = _phieuThuRepository.Table.FirstOrDefault(x => x.MaPhieuThu == model.MaPhieuThu && x.IsDeleted == false && x.IsActive == true);
                if (obj.NoKhuyenMai == model.NoKhuyenMai)
                    return "";
                SqlParameter MPT1 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý trước khi update
                SqlParameter sessionid = new SqlParameter("SessionId", "");
                sessionid.Size = 20;
                sessionid.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter message = new SqlParameter("Message", "");
                message.Size = 4000;
                message.Direction = System.Data.ParameterDirection.Output;//lay chuoi sessionid de truyen vao lichSuGiaoDichTuiTien sau khi update
                SqlParameter Ngay = new SqlParameter("Ngay", model.NgayThu);
                _dbContext.ExecuteStoredProcedure("sp_XuLyTruocKhiUpdatePhieuThu", MPT1, Ngay, sessionid, message);
                if (message.Value != null && !string.IsNullOrEmpty(message.Value.ToString()))
                    return message.Value.ToString();
                //lay gia tri sessionid trả ra sau xử lý
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                obj.NoKhuyenMai = model.NoKhuyenMai;
                _phieuThuRepository.Update(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieuThu, obj.MaLoaiPhieu, "UPDATE", "UpdatePhieuThu", "Mã phiếu thu :" + obj.MaPhieuThu + " mã loại phiếu :" + obj.MaLoaiPhieu + " Tổng cộng :" + obj.TongCong, _authenticationService.GetAuthenticatedUser().UserId);
                SqlParameter MPT2 = new SqlParameter("MaPhieuThu", obj.MaPhieuThu);//xử lý sau khi update
                MPT2.Direction = System.Data.ParameterDirection.Input;
                SqlParameter SessionId2 = new SqlParameter("SessionId", sessionid.Value);//xử lý sau khi update
                SessionId2.Direction = System.Data.ParameterDirection.Input;
                SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", "");//xử lý sau khi update
                pGiaTriTrenGiaoDien.Direction = System.Data.ParameterDirection.Input;
                SqlParameter pIpClient = new SqlParameter("IPClient", _ipClient);
                SqlParameter pHostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                Error.Direction = System.Data.ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuThu", MPT2, SessionId2, pGiaTriTrenGiaoDien, pIpClient, pHostNameClient, Error);
                if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                {
                    int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                    if (gioGuiEmailLast != GioGuiEmail)
                    {
                        MailHelper Mail = new MailHelper();
                        string Content = ContentMail(model.MaPhieuThu);
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
                            string Content = ContentMail(model.MaPhieuThu);
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

                _log.WriteLog("PhieuThuService.UpdatePhieuThu: ", e);

                return "ERROR:" + e.ToString();
            }
        }
        public string Update_Insert_NoHHGP(string MaPhieuThuBX, double SoTienNo)
        {
            try
            {
                SqlParameter PrMaPhieuNo = new SqlParameter("MaPhieuBX", MaPhieuThuBX);
                SqlParameter PrSoTienNo = new SqlParameter("SoTienNo", SoTienNo);
                SqlParameter PrUser = new SqlParameter("User", CurrentUser.UserId);
                SqlParameter PrOutPut = new SqlParameter("Message", "");
                PrOutPut.SqlDbType = SqlDbType.NVarChar;
                PrOutPut.Size = 4000;
                PrOutPut.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_InsertUpdate_NoHHGP", PrMaPhieuNo, PrSoTienNo, PrUser, PrOutPut);
                if (PrOutPut.Value.ToString() == "")
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, MaPhieuThuBX, "NHHGP", "UPDATE_INSERT", "UpdateSoTienNoHHGP", "Mã phiếu nợ :" + MaPhieuThuBX + " mã loại phiếu : NHHGP, SoTienNo:" + SoTienNo, _authenticationService.GetAuthenticatedUser().UserId);
                return PrOutPut.Value.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string UpdatePhieuThuBH(string xmlPhieuThu)
        {
            try
            {
                SqlParameter _xmlPhieuThu = new SqlParameter("xmlObj", xmlPhieuThu);
                _xmlPhieuThu.SqlDbType = SqlDbType.Xml;
                SqlParameter PrUser = new SqlParameter("User", CurrentUser.UserId);
                SqlParameter PrOutPut = new SqlParameter("outMessg", "");
                PrOutPut.Size = 4000;
                PrOutPut.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_UpdatePhieuThuBH", _xmlPhieuThu, PrUser, PrOutPut);
                return PrOutPut.Value.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string UpdatePhieuThuSK(string xmlPhieuThu)
        {
            try
            {
                SqlParameter _xmlPhieuThu = new SqlParameter("xmlObj", xmlPhieuThu);
                _xmlPhieuThu.SqlDbType = SqlDbType.Xml;
                SqlParameter PrUser = new SqlParameter("User", CurrentUser.UserId);
                SqlParameter PrOutPut = new SqlParameter("outMessg", "");
                PrOutPut.Size = 4000;
                PrOutPut.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_UpdatePhieuThuBH_SK", _xmlPhieuThu, PrUser, PrOutPut);
                return PrOutPut.Value.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public double GetCNTT(string SoKhung)
        {
            try
            {
                double Count = _phieuThuRepository.Table.Where(x => x.MaLoaiPhieu == "TDIVU" && x.SoKhung == SoKhung && x.LoaiBaoHiem == "XKG" && x.IsDeleted == false).Sum(y => y.TongCong);
                return Count;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lay coc theo dieu kien Bien so xe + So hop dong
        /// </summary>
        /// <param name="maPhieuThu"></param>
        /// <param name="maSo"></param>
        /// <param name="HoTen"></param>
        /// <param name="isForInsert"></param>
        /// <returns></returns>
        /// thang.tran them ngay 23/5/2022 xu ly viec coc dich vu + coc ban xe duoc su dung chung o cac phieu co dung coc
        public List<ViewTCOC> GetTCOC_V2(string maPhieuThu, string bienSo, string soHopDong, string hoTen, bool isForInsert, string type = "ALL")
        {
            try
            {

                List<ViewTCOC> list = new List<ViewTCOC>();
                //if (isForInsert)
                //    list = _VTCOCRepository.Table.Where(x => maSo != "" && maSo != null && (x.BienSo == maSo || x.SoHopDong == maSo || x.HoTen.ToLower().Contains(HoTen.ToLower())) && x.TTPCode != "B").ToList();
                //else
                //    list = _VTCOCRepository.Table.Where(x => maSo != "" && maSo != null && (x.BienSo == maSo || x.SoHopDong == maSo || x.HoTen.ToLower().Contains(HoTen.ToLower()))).ToList();

                SqlParameter BienSo = new SqlParameter("BienSo", bienSo == null ? "" : bienSo);
                SqlParameter SoHopDong = new SqlParameter("SoHopDong", soHopDong == null ? "" : soHopDong);
                SqlParameter HoTen = new SqlParameter("HoTen", hoTen == null ? "" : hoTen);
                SqlParameter IsForInsert = new SqlParameter("isForInsert", isForInsert);
                SqlParameter MaPhieuThu = new SqlParameter("MaPhieuThu", maPhieuThu == null ? "" : maPhieuThu);
                SqlParameter Type = new SqlParameter("type", type == null ? "" : type);
                list = _dbContext.ExecuteStoredProcedureList<ViewTCOC>("sp_LoadDanhSachCocLenPhieuThu", BienSo, SoHopDong, HoTen, IsForInsert, MaPhieuThu, Type).ToList();

                foreach (var temp in list)
                {
                    List<ChiTietPhieuThu> CTPT = _ctptRepository.Table.Where(x => x.MaPhieuThu == maPhieuThu
                        && x.HinhThucThanhToan == "DC"
                        && x.SoThamChieu == temp.MaPhieuThu
                        && x.IsDeleted == false).ToList();
                    double TongSoTien = 0;//tinh tong so tien su dung coc
                    foreach (var item in CTPT)
                    {
                        TongSoTien = +item.SoTienThanhToan;
                    }
                    temp.TongCong = temp.ConLai + TongSoTien;
                    temp.SoTienDaSuDung = TongSoTien;
                }
                // Lấy cọc còn lại != 0 Lộc 200828
                // thang comment tai day 26/5/2022
                //list = list.Where(x => !(x.ConLai == 0 && x.).ToList();
                return list;
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetTCOC:", ex);
                return null;
            }
        }
        public List<ViewChiTietPhuKien> GetChiTietPhuKien(string MaXe, string Type, string isMultiTicket)
        {
            try
            {
                SqlParameter pMaXe = new SqlParameter("ma_xe", MaXe);
                SqlParameter pType = new SqlParameter("type", Type);
                SqlParameter pisMultiTicket = new SqlParameter("isMultiTicket", isMultiTicket);
                var list = _dbContext.ExecuteStoredProcedureList<ViewChiTietPhuKien>("sp_ChiTiet_PhuKien", pMaXe, pType, pisMultiTicket).ToList();
                //var list = new List<ViewChiTietPhuKien>();
                //var dt = _dbContext.ExecuteStoredProcedureDataTable("sp_ChiTiet_PhuKien", pMaXe, pType);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ViewTBAHITemp> GetTBAHITabTemp(DateTime from, DateTime to, string query, string typeTicketTemp, int p, ref int total, int pageSize, ref List<ViewTBAHITemp> listViewFull)
        {
            try
            {
                SqlParameter pfrom = new SqlParameter("from", from);
                SqlParameter pto = new SqlParameter("to", to);
                SqlParameter pquery = new SqlParameter("keyWord", query);
                SqlParameter ptypeTicketTemp = new SqlParameter("typeTicketTemp", typeTicketTemp);
                //var w2 = _dbContext.ExecuteStoredProcedureDataTable("sp_DanhSach_BaoHiem", pfrom, pto, pquery, ptypeTicketTemp);
                var list = _dbContext.ExecuteStoredProcedureList<ViewTBAHITemp>("sp_DanhSach_BaoHiem", pfrom, pto, pquery, ptypeTicketTemp).ToList();
                //if (query != "")
                //{
                //    list = list.Where(x =>x.TEN_KHACH_HANG.ToString().ToLower().Contains(query.ToLower())).ToList();
                //}
                // var list = new List<ViewTBAHITemp>();
                listViewFull = list;
                total = list.Count;
                if (query != "")
                {
                    return list.Take(pageSize).ToList();
                }
                return list.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ViewPhieuBDTKTamCybers> GetBDTKCyber(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string type)
        {
            try
            {
                query = query.ToLower();
                var q = _ViewPhieuBDTKTamCybersRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.SoChungTu.ToLower().Contains(query)
                    || x.HoTen.ToLower().Contains(query)
                    || query.Contains(x.SoChungTu.ToLower())
                    || query.Contains(x.BienSoXe.ToLower())
                    || query.Contains(x.SoKhung.ToLower())
                    );
                }
                var result = q.OrderByDescending(x => x.SoChungTu).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();

            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetBDTKCyberTabTemp:(" + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + ") ", ex);
                return null;
            }
        }
        public List<ViewChiTietBDTK> GetChiTietThuBDTKTemp(string SoChungTu, string Type)
        {
            try
            {
                SqlParameter pMaXe = new SqlParameter("soChungTu", SoChungTu);
                //SqlParameter pType = new SqlParameter("type", Type);
                var list = _dbContext.ExecuteStoredProcedureList<ViewChiTietBDTK>("sp_ChiTiet_BDTK_Cyber", pMaXe).ToList();
                //var list = new List<ViewChiTietPhuKien>();
                //var dt = _dbContext.ExecuteStoredProcedureDataTable("sp_ChiTiet_PhuKien", pMaXe, pType);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string TaoCongNoChietKhauThuongMai(PhieuThu objPhieuThu, string loaiNo,string lyDoThu, string noiDung, float soTienNo, string maDoiTacChietKhauThuongMai)
        {
            try
            {
                SqlParameter pMaPhieuThu = new SqlParameter("MaPhieuThu", objPhieuThu.MaPhieuThu);
                SqlParameter pLoaiNo = new SqlParameter("LoaiPhieuNo", loaiNo);
                SqlParameter pNoiDung = new SqlParameter("NoiDung", noiDung);
                SqlParameter pSoTienNo = new SqlParameter("SoTienNo", soTienNo);
                SqlParameter pLyDoThu = new SqlParameter("LyDoThu", lyDoThu);
                SqlParameter pSoKhung = new SqlParameter("SoKhung", objPhieuThu.SoKhung);
                SqlParameter pSoHopDong = new SqlParameter("SoHopDong", objPhieuThu.SoHopDong);
                SqlParameter pCreatedBy = new SqlParameter("CreatedBy", objPhieuThu.CreatedBy);
                SqlParameter pNgayThu = new SqlParameter("NgayThu", objPhieuThu.NgayThu);
                SqlParameter pMessage = new SqlParameter("Message", "");
                SqlParameter pMaDoiTacChietKhauThuongMai = new SqlParameter("MaDoiTacChietKhauThuongMai", maDoiTacChietKhauThuongMai);
             
                pMessage.Direction = ParameterDirection.Output;

                //SqlParameter pType = new SqlParameter("type", Type);
                _dbContext.ExecuteStoredProcedure("sp_TaoCongNoChietKhauThuongMai", pMaPhieuThu, pLoaiNo, pLyDoThu, pNoiDung, pSoTienNo, pSoKhung,pNgayThu, pSoHopDong, pCreatedBy,pMessage, pMaDoiTacChietKhauThuongMai);
                //var list = new List<ViewChiTietPhuKien>();
                //var dt = _dbContext.ExecuteStoredProcedureDataTable("sp_ChiTiet_PhuKien", pMaXe, pType);
                return pMessage.Value.ToString();
            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        public ViewNoPhaiThu LayCongNoChietKhauThuongMai(PhieuThu objPhieuThuBanXe)
        {
            SqlParameter pMaPhieuThuTBAXE = new SqlParameter("MaPhieuThuTBAXE", objPhieuThuBanXe.MaPhieuThu);
            //SqlParameter pType = new SqlParameter("type", Type);
            var list = _dbContext.ExecuteStoredProcedureList<ViewNoPhaiThu>("sp_LayCongNoChietKhauThuongMai", pMaPhieuThuTBAXE).ToList();
            //var list = new List<ViewChiTietPhuKien>();
            //var dt = _dbContext.ExecuteStoredProcedureDataTable("sp_ChiTiet_PhuKien", pMaXe, pType);
            if(list.Count>0)
                return list[0];
            return null;
        }

        public List<ViewPhieuThuCyber> GetListViewPhieuThuCyber(DateTime from, DateTime to, int p, ref int total, int pageSize, string query, string type)
        {
            try
            {
                query = query == null ? "" : query.ToLower();
                SqlParameter strQuery = new SqlParameter("Query", query);
                SqlParameter fromdate = new SqlParameter("Fromdate", from);
                SqlParameter todate = new SqlParameter("Todate", to);
                SqlParameter _type = new SqlParameter("type", type);
                var result = _dbContext.ExecuteStoredProcedureObjs<ViewPhieuThuCyber>("sp_getListPhieuThuCyberSoft", fromdate, todate, strQuery, _type).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();

            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetListViewPhieuThuCyber:", ex);

                return null;
            }
        }
        public List<ViewChiTietBaoDuongTietKiem> GetNBDTK_Cyber(string soRO)
        {
            try
            {
                SqlParameter SoRo = new SqlParameter("SoRo", string.IsNullOrEmpty(soRO) ? "" : soRO);
                return _dbContext.ExecuteStoredProcedureList<ViewChiTietBaoDuongTietKiem>("sp_SelectNBDTK_Cyber", SoRo).ToList();
                //}
            }
            catch (Exception ex)
            {
                _log.WriteLog("PhieuThuService.GetNBDTK_Cyber:", ex);
                return null;
            }

        }
    }
}
