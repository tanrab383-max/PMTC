using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Data;
using TNK.Services.Authentication;
using TNK.Services.Common;
using TNK.Services.Log;
using TNK.Services.Catalog;
using TNK.Services.LichSuThaoTac;
using TNK.Services.Manager;
using System.Data;

namespace TNK.Services.KhoXe
{
    public class KhoXeService : IKhoXeService
    {
        IAuthenticationService _authenticationService;
        IRepository<ViewKhoXe> _viewKhoXeRepository;
        IRepository<TNK.Core.Domain.KhoXe> _khoXeRepository;
        IRepository<ViewTuiHangTrenDuong> _viewTuiHangTrenDuongXeRepository;
        IRepository<ViewThongKeKho> _ViewThongKeKhoRepository;
        IRepository<PhieuNhapKho> _phieuNhapKhoRepository;
        IRepository<LichSuTheChapXe> _LichSuTheChapXeRepository;
        IRepository<LoaiXe> _LoaiXeRepository;
        IRepository<NoPhaiThu> _NoPhaiThuRepository;
        ILichSuThaoTacService _lichSuThaoTacService;
        ILoaiXeService _loaiXeService;
        ICommonService _commonService;
        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;
        ISoKetChuyenService _soKetChuyenService;
        string _ipClient = "";
        string _hostNameClient = "";
        public KhoXeService(
                IAuthenticationService _authenticationService,
                IRepository<ViewKhoXe> _viewKhoXeRepository,
                IRepository<ViewTuiHangTrenDuong> _viewTuiHangTrenDuongXeRepository,
                IRepository<ViewThongKeKho> _ViewThongKeKhoRepository,
                IRepository<PhieuNhapKho> _phieuNhapKhoRepository,
                IRepository<LichSuTheChapXe> _LichSuTheChapXeRepository,
                IRepository<TNK.Core.Domain.KhoXe> _khoXeRepository,
                IRepository<NoPhaiThu> _NoPhaiThuRepository,
                IRepository<LoaiXe> _LoaiXeRepository,
                ILoaiXeService _loaiXeService,
                ICommonService _commonService,
                IDbContext _dbContext,
                ICacheManager _cacheManager,
                ILogger _log,
                ILichSuThaoTacService _lichSuThaoTacService,
                ISoKetChuyenService _soKetChuyenService
            )
        {
            this._authenticationService = _authenticationService;
            this._viewKhoXeRepository = _viewKhoXeRepository;
            this._khoXeRepository = _khoXeRepository;
            this._loaiXeService = _loaiXeService;
            this._viewTuiHangTrenDuongXeRepository = _viewTuiHangTrenDuongXeRepository;
            this._ViewThongKeKhoRepository = _ViewThongKeKhoRepository;
            this._commonService = _commonService;
            this._phieuNhapKhoRepository = _phieuNhapKhoRepository;
            this._LichSuTheChapXeRepository = _LichSuTheChapXeRepository;
            this._dbContext = _dbContext;
            this._cacheManager = _cacheManager;
            this._log = _log;
            this._lichSuThaoTacService = _lichSuThaoTacService;
            this._NoPhaiThuRepository = _NoPhaiThuRepository;
            this._LoaiXeRepository = _LoaiXeRepository;
            this._soKetChuyenService = _soKetChuyenService;
        }

        public List<ViewKhoXe> GetDanhSachXe()
        {
            return _viewKhoXeRepository.Table.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<ViewKhoXe> GetDanhSachXe(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string TinhTrang, string chonngay)
        {
            if(chonngay == "NN")
            {
                var list = _viewKhoXeRepository.Table.Where(x => x.IsDeleted == false && x.NgayNhapKho >= from & x.NgayNhapKho <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.MaPhieuNhap.ToLower().Contains(query.ToLower())
                    || x.SoKhung.ToLower().Contains(query.ToLower())
                    || x.SoMay.ToLower().Contains(query.ToLower())
                    || x.SoTMSS.ToLower().Contains(query.ToLower())
                    || x.SoHoaDon.ToLower().Contains(query.ToLower())
                    || x.MaLoaiXe.ToLower().Contains(query.ToLower())
                    || x.NhaCungCap.ToLower().Contains(query.ToLower()));
                }
                if (TinhTrang == "N")
                {
                    list = list.Where(x => x.TinhTrang == TinhTrang);
                }
                else if (TinhTrang == "X")
                {
                    list = list.Where(x => x.TinhTrang == TinhTrang);
                }
                else if (TinhTrang == "TC")
                {
                    list = list.Where(x => x.TinhTrang == TinhTrang);
                }
                var result = list.OrderByDescending(x => x.NgayNhapKho).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                var list = _viewKhoXeRepository.Table.Where(x => x.IsDeleted == false && x.NgayXuatKho >= from & x.NgayXuatKho <= to);
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.MaPhieuNhap.ToLower().Contains(query.ToLower())
                    || x.SoKhung.ToLower().Contains(query.ToLower())
                    || x.SoMay.ToLower().Contains(query.ToLower())
                    || x.SoTMSS.ToLower().Contains(query.ToLower())
                    || x.SoHoaDon.ToLower().Contains(query.ToLower())
                    || x.MaLoaiXe.ToLower().Contains(query.ToLower())
                    || x.NhaCungCap.ToLower().Contains(query.ToLower()));
                }
                if (TinhTrang == "N")
                {
                    list = list.Where(x => x.TinhTrang == TinhTrang);
                }
                else if (TinhTrang == "X")
                {
                    list = list.Where(x => x.TinhTrang == TinhTrang);
                }
                else if (TinhTrang == "TC")
                {
                    list = list.Where(x => x.TinhTrang == TinhTrang);
                }
                var result = list.OrderByDescending(x => x.NgayXuatKho).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }         
        }

        public List<ViewKhoXe> GetDanhSachXeTon( DateTime to, string query, int p, ref int total, int pageSize)
        {
            try { 
            var list = _viewKhoXeRepository.Table.Where(x => x.IsDeleted == false && x.NgayNhapKho <= to && x.NgayXuatKho == null || x.NgayXuatKho > to);
            if(!string.IsNullOrEmpty(query))
            {
                list = list.Where(x => x.MaPhieuNhap.ToLower().Contains(query.ToLower())
                || x.SoKhung.ToLower().Contains(query.ToLower())
                || x.SoMay.ToLower().Contains(query.ToLower())
                || x.SoTMSS.ToLower().Contains(query.ToLower())
                || x.SoHoaDon.ToLower().Contains(query.ToLower())
                || x.MaLoaiXe.ToLower().Contains(query.ToLower())
                || x.NhaCungCap.ToLower().Contains(query.ToLower()));
            }
            var result = list.OrderByDescending(x => x.CreatedDate).ToList();
            total = result.Count;
            return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<ViewTuiHangTrenDuong> GetDanhSachKhoHangTrenDuong()
        {
            return _viewTuiHangTrenDuongXeRepository.Table.ToList();
        }
        #region thống kê kho
        public List<ViewThongKeKho> ListDanhSach(DateTime from,DateTime to)
        {
            SqlParameter param = new SqlParameter("DateFrom", from);
            SqlParameter param1 = new SqlParameter("DateTo", to);
            return _dbContext.ExecuteStoredProcedureList<ViewThongKeKho>("ThongKeKhoXe", param,param1).ToList();
            //return _ViewThongKeKhoRepository.Table.Where(x => x.NgayNhap >= from && x.NgayNhap <= to).ToList();

        }
        #endregion
        public ViewTuiHangTrenDuong GetTuiHangTrenDuong(string maNCC)
        {
            List<ViewTuiHangTrenDuong> lst = _viewTuiHangTrenDuongXeRepository.Table.Where(x => x.NhaCungCap == maNCC).ToList();
            if (lst.Count > 0)
                return lst[0];
            return null;

        }
        /// <summary>
        /// Kiem tra xe da co trong kho chưa
        /// </summary>
        /// <param name="soKhung">so khung</param>
        /// <returns></returns>
        public bool CheckExists(string soKhung)
        {
            if (_khoXeRepository.Table.Where(x => x.SoKhung == soKhung && x.IsDeleted == false).ToList().Count > 0)
                return true;
            return false;
        }

        public string NhapKhoXe(TNK.Core.Domain.KhoXe item, PhieuNhapKho phieuNhapKho)
        {
            try
            {
                DateTime _ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (_ngayKetChuyenCuoi.Subtract(phieuNhapKho.NgayNhap).Days >= 0)
                    return "Không thể nhập xe vào khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán.";
                //kiem tra xe này đã có trong kho chưa
                if (CheckExists(item.SoKhung))
                    return "Xe đã có trong kho";
                //tao phieu nhap kho
                phieuNhapKho.MaPN = _commonService.CreateId("PNKHO");
                //phieuNhapKho.NgayNhap = DateTime.Now;
                phieuNhapKho.NguoiNhap = _authenticationService.GetAuthenticatedUser().UserName;
                phieuNhapKho.PhanLoaiNhap = "HTD";
                phieuNhapKho.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                phieuNhapKho.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                phieuNhapKho.CreatedDate = DateTime.Now;
                phieuNhapKho.UpdatedDate = DateTime.Now;
                phieuNhapKho.SoTienHTD = item.GiaVon.Value;//item.GiaHoaDon.Value : thay đổi ngày 20.08.2018
                phieuNhapKho.SoTienKMTMV = 0;//item.GiaVon.Value - item.GiaHoaDon.Value
                _phieuNhapKhoRepository.Insert(phieuNhapKho);
                //insert vao kho xe
                item.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                item.MaPhieuNhap = phieuNhapKho.MaPN;
                LoaiXe loai = _loaiXeService.GetLoaiXe(item.MaLoaiXe);
                item.GiaNiemYet = loai.GiaNiemYet.Value;
                item.CreatedDate = DateTime.Now;
                item.UpdatedDate = DateTime.Now;
                item.TinhTrang = "N";
                item.Id = Guid.NewGuid();
                item.SoKhung = item.SoKhung.Trim();
                _khoXeRepository.Insert(item);


                //SqlParameter maTui = new SqlParameter("maTui", item.NhaCungCap);
                //maTui.Direction = System.Data.ParameterDirection.Input;
                //SqlParameter log = new SqlParameter("log", "");
                //log.Direction = System.Data.ParameterDirection.Output;
                //SqlParameter loaiTui = new SqlParameter("loaiTui", "HTD");
                //loaiTui.Direction = System.Data.ParameterDirection.Input;
                //SqlParameter sessionId = new SqlParameter("SessionId", DateTime.Now.ToString("yyMMddhhmmss"));
                //loaiTui.Direction = System.Data.ParameterDirection.Input;
                //_dbContext.ExecuteStoredProcedure("sp_XuLyCapNhatTienTrongTuiDinhKhoan", maTui, log, loaiTui, sessionId);

                //SqlParameter maTui1 = new SqlParameter("maTui", "KHO_XE");
                //maTui.Direction = System.Data.ParameterDirection.Input;
                //SqlParameter log1 = new SqlParameter("log", "");
                //log.Direction = System.Data.ParameterDirection.Output;
                //SqlParameter loaiTui1 = new SqlParameter("loaiTui", "");
                //loaiTui.Direction = System.Data.ParameterDirection.Input;
                //SqlParameter sessionId1 = new SqlParameter("SessionId", DateTime.Now.ToString("yyMMddhhmmss"));
                //sessionId1.Direction = System.Data.ParameterDirection.Input;
              
                ////tinh toan lai kho xe
                //_dbContext.ExecuteStoredProcedure("sp_XuLyCapNhatTienTrongTuiDinhKhoan", maTui1, log1, loaiTui1, sessionId1);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, phieuNhapKho.MaPN, "NKHXE", "INSERT", "Nhập kho xe HTD:" + item.SoKhung + " -NCC:" + item.NhaCungCap, "", _authenticationService.GetAuthenticatedUser().UserId);
                SqlParameter MaPhieuNhap = new SqlParameter("MaPhieuNhap", item.MaPhieuNhap);
                SqlParameter IPClient = new SqlParameter("IPClient", _ipClient);
                SqlParameter HostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                SqlParameter CreatedBy = new SqlParameter("CreatedBy", phieuNhapKho.CreatedBy);
                _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiNhapKhoXe", MaPhieuNhap,IPClient,HostNameClient,CreatedBy);
            }
            catch (Exception ex)
            {
                _log.WriteLog("KhoXeService.NhapKhoXe:" + ex);
                return "Error:" + ex;
            }
            //xu ly lai goi store nhap kho xe   
            return "";
        }
        //danh sach phieu nhap kho
        public DataTable GetDanhSachPhieuNhap(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string TinhTrang)
        {
            SqlParameter FromDate = new SqlParameter("fromdate", from);
            SqlParameter ToDate = new SqlParameter("todate", to);
            SqlParameter tinhtrang = new SqlParameter("tinhtrang", TinhTrang);
            SqlParameter Query = new SqlParameter("query", query);
            var list =  _dbContext.ExecuteStoredProcedureDataTable("sp_DanhSachPhieuNhap", FromDate, ToDate, tinhtrang,Query);
            total = list.Rows.Count;
            return list;
        }
        public bool Update(TNK.Core.Domain.KhoXe objKhoXe)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var data = _khoXeRepository.Table.FirstOrDefault(x => x.SoKhung == objKhoXe.SoKhung&& x.IsDeleted == false);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    data.Id = objKhoXe.Id;
                    data.SoMay = objKhoXe.SoMay;
                    data.SoHoaDon = objKhoXe.SoHoaDon;
                    data.SoTMSS = objKhoXe.SoTMSS;
                    data.MaLoaiXe = objKhoXe.MaLoaiXe;
                    if(data.TheChap == null)
                    {
                        data.TheChap = objKhoXe.TheChap;
                        if(objKhoXe.TheChap != null)
                        {
                            data.NgayTheChap = objKhoXe.NgayTheChap;
                            data.NgayHetTheChap = null;
                        }
                    }
                    else 
                    {
                        if(objKhoXe.NgayHetTheChap != null)
                        {
                            data.TheChap = null;
                            data.NgayTheChap = null;
                            data.NgayHetTheChap = objKhoXe.NgayHetTheChap;
                        }
                    }
                    data.UpdatedBy = uid;
                    data.UpdatedDate = DateTime.Now;
                    _khoXeRepository.Update(data);
                    SqlParameter SoKhung = new SqlParameter("soKhung", objKhoXe.SoKhung);
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdateKhoXe", SoKhung);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("KhoXeService.Update:" + ex);
                return false;
            }
        }


        public string Delete(Guid id)
        {
            try
            {                
                var obj = _khoXeRepository.Table.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                                
                if (obj == null)
                {
                    return "Không tìm thấy thông tin xe này. Xe đã bị xóa trước đó.";
                }
                else
                {
                    var objNgayNhap = _viewKhoXeRepository.Table.FirstOrDefault(x => x.Id == id);
                    DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                    if (ngayKetChuyenCuoi.Subtract(objNgayNhap.NgayNhapKho).Days >= 0
                      )
                        return "Không thể xóa xe trong khoảng thời gian đã kết chuyển (" + ngayKetChuyenCuoi.ToString("dd/MM/yyyy") + "). Vui lòng liên hệ Phòng Kiểm toán.";
                    
                    obj.IsDeleted = true;
                    obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                    obj.UpdatedDate = DateTime.Now;
                    _khoXeRepository.Update(obj);
                    SqlParameter Id = new SqlParameter("id", id );
                    SqlParameter IPClient = new SqlParameter("IPClient", _ipClient);
                    SqlParameter HostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiDeleteXeTrongKho", Id,IPClient,HostNameClient);
                   
                    return "";
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog("KhoXeService.Delete:" + ex );
                return ex.Message;
            }
        }
        public double SoTienConLai()
        {
            return _NoPhaiThuRepository.Table.Where(x => x.MaPhieuNo == "NOTMV1701010001").Select(x => x.SoTienConLai).FirstOrDefault();
        }
        public void SetIPClient(string ip)
        {
            this._ipClient = ip;
        }
        public void SetHostNameClient(string host)
        {
            this._hostNameClient = host;
        }
        public string NhapTheChap(LichSuTheChapXe Data)
        {
            try
            {
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if ( ngayKetChuyenCuoi.Subtract(Data.NgayTheChap).Days >= 0
                    || (Data.NgayHetTheChap != null && ngayKetChuyenCuoi.Subtract(Data.NgayHetTheChap.Value).Days > 0)
                  )
                    return "Không thể tạo thế chấp trong khoảng thời gian đã kết chuyển (" + ngayKetChuyenCuoi.ToString("dd/MM/yyyy") + "). Vui lòng liên hệ Phòng Kiểm toán.";
                
                Data.CreatedBy = _authenticationService.GetAuthenticatedUser().UserName;
                Data.CreatedDate = DateTime.Now;
                Data.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserName;
                Data.UpdatedDate = DateTime.Now;
                Data.Id = Guid.NewGuid();
                //Data.NgayHetTheChap = null;
                Data.IsActive = true;
                _LichSuTheChapXeRepository.Insert(Data);
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoXeService.NhapTheChap:" + ex);
                return "Error:" + ex;
            }
            return "";
        }
        public List<LichSuTheChapXe> List(string SoKhung)
        {
            List<LichSuTheChapXe> list = _LichSuTheChapXeRepository.Table.Where(x => x.IsDeleted == false && x.IsActive == true && x.SoKhung == SoKhung).OrderByDescending(x=>x.NgayTheChap).ToList();
            return list;
        }
        public string EditTheChap(LichSuTheChapXe Data)
        {
            try
            {
                var obj = _LichSuTheChapXeRepository.Table.Where(x => x.Id == Data.Id).FirstOrDefault();
                if (obj == null)                    
                    return "Không tìm thấy thông tin thế chấp hoặc dữ liệu đã bị thay đổi trước đó.";

                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if ( ngayKetChuyenCuoi.Subtract(obj.NgayTheChap).Days >= 0
                    || (obj.NgayHetTheChap != null && ngayKetChuyenCuoi.Subtract(obj.NgayHetTheChap.Value).Days > 0)
                    || ngayKetChuyenCuoi.Subtract(Data.NgayTheChap).Days >= 0
                    || (Data.NgayHetTheChap != null && ngayKetChuyenCuoi.Subtract(Data.NgayHetTheChap.Value).Days > 0)
                  )
                    return "Không thể điều chỉnh thế chấp trong khoảng thời gian đã kết chuyển (" + ngayKetChuyenCuoi.ToString("dd/MM/yyyy") + "). Vui lòng liên hệ Phòng Kiểm toán.";

                obj.NganHangTheChap = Data.NganHangTheChap;
                obj.NgayTheChap = Data.NgayTheChap;
                obj.NgayHetTheChap = Data.NgayHetTheChap;
                obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserName;
                obj.UpdatedDate = DateTime.Now;
                _LichSuTheChapXeRepository.Update(obj);
                return "";
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoXeService.EditTheChap:" + ex);
                return ex.Message;
            }
        }
        public string DeleteTheChap(LichSuTheChapXe Data)
        {
            try
            {
                var obj = _LichSuTheChapXeRepository.Table.Where(x => x.Id == Data.Id).FirstOrDefault();
                if (obj == null)
                    return "Không tìm thấy thông tin thế chấp hoặc dữ liệu đã bị thay đổi trước đó.";
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayTheChap).Days >= 0
                    || (obj.NgayHetTheChap != null && ngayKetChuyenCuoi.Subtract(obj.NgayHetTheChap.Value).Days > 0)
                    || ngayKetChuyenCuoi.Subtract(Data.NgayTheChap).Days >= 0
                    || (Data.NgayHetTheChap != null && ngayKetChuyenCuoi.Subtract(Data.NgayHetTheChap.Value).Days > 0)
                  )
                    return "Không thể điều chỉnh thế chấp trong khoảng thời gian đã kết chuyển (" + ngayKetChuyenCuoi.ToString("dd/MM/yyyy") + "). Vui lòng liên hệ Phòng Kiểm toán.";

                obj.IsActive = false;
                obj.IsDeleted = true;
                _LichSuTheChapXeRepository.Update(obj);
                return "";
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoXeService.DeleteTheChap:" + ex);
                return ex.Message;
            }
        }
        public DataTable GetListDanhSachXe(DateTime from, DateTime to, string chonngay)
        {
            try
            {

                SqlParameter FromDate = new SqlParameter("FromDate", from);
                SqlParameter ToDate = new SqlParameter("ToDate", to);
                SqlParameter ChonNgay = new SqlParameter("ChonNgay", chonngay);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_getListViewKhoXe", FromDate, ToDate,ChonNgay);
            }
            catch (Exception ex)
            {
                _log.WriteLog("KhoXeService.GetListDanhSachXe:" + ex);
                return null;
            }
        }
        public DataTable GetListDanhSachXeTon(DateTime to)
        {
            try
            {
                SqlParameter ToDate = new SqlParameter("ToDate", to); 
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_getListViewDSXeTon",ToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("KhoXeService.GetListDanhSachXeTon:" + ex);
                return null;
            }
        }
        public TNK.Core.Domain.KhoXe GetXebySoKhung(string SoKhung)
        {
            try
            {
                return _khoXeRepository.Table.Where(x => x.SoKhung == SoKhung && x.TinhTrang == "N" && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.WriteLog("KhoXeService.GetXebySoKhung:" + ex);
                return null;
            }
        }
    }

}
