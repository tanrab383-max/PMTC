using System;
using System.Collections.Generic;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using System.Linq;
using TNK.Services.Authentication;
using TNK.Services.Common;
using System.Data.SqlClient;
using TNK.Data;
using TNK.Services.Log;
using TNK.Services.LichSuThaoTac;
using System.Data;
using TNK.Services.Manager;
namespace TNK.Services.ChuyenTien
{
    public class ChuyenTienService : IChuyenTienService
    {
        IRepository<ViewChiTietPhieuChuyenTienNoiBo> _viewRepository;
        IRepository<PhieuChuyenTienNoiBo> _PCTNBRepository;
        IAuthenticationService _authenticationService;
        IRepository<ChiTietPhieuChuyenTienNoiBo> _CTPCTNBRepository;
        IRepository<ChiTietPhieuThu> _CTPTRepository;
        ICommonService _commonService;
        IRepository<ViewTaiKhoanTien_NganHang> VTKTNH;
        IDbContext _dbContext;
        ILogger _log;
        ILichSuThaoTacService _lichSuThaoTacService;
        ISoKetChuyenService _soKetChuyenService;
        string _ipClient = "";
        string _hostNameClient = "";
        public ChuyenTienService(
            IRepository<ViewChiTietPhieuChuyenTienNoiBo> _viewRepository,
            IRepository<PhieuChuyenTienNoiBo> _PCTNBRepository,
            IRepository<ChiTietPhieuChuyenTienNoiBo> _CTPCTNBRepository,
            IRepository<ChiTietPhieuThu> _CTPTRepository,
            IAuthenticationService _authenticationService,
            ICommonService _commonService,
            IRepository<ViewTaiKhoanTien_NganHang> VTKTNH,
            IDbContext _dbContext,
            ILogger _log,
            ILichSuThaoTacService _lichSuThaoTacService,
            ISoKetChuyenService _soKetChuyenService
            )
        {
            this._viewRepository = _viewRepository;
            this._PCTNBRepository = _PCTNBRepository;
            this._CTPCTNBRepository = _CTPCTNBRepository;
            this._CTPTRepository = _CTPTRepository;
            this._authenticationService = _authenticationService;
            this._commonService = _commonService;
            this.VTKTNH = VTKTNH;
            this._dbContext = _dbContext;
            this._log = _log;
            this._lichSuThaoTacService = _lichSuThaoTacService;
            this._soKetChuyenService = _soKetChuyenService;
        }

        public List<ViewChiTietPhieuChuyenTienNoiBo> GetVCTPCTNB(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            var q = _viewRepository.Table.Where(x => x.NgayChuyen >= from && x.NgayChuyen <= to);
            if (!string.IsNullOrEmpty(query))
            {
                //q = q.Where(x => x.TaiKhoanNguon.ToLower().Contains(query.ToLower()) || x.TaiKhoanDich.ToLower().Contains(query.ToLower()) || x.SoChungTu.ToLower().Contains(query.ToLower()));
                q = q.Where(x=>x.MaPhieu.Contains(query.ToLower()) || x.SoChungTu.Contains(query.ToLower()) || x.TaiKhoanNguon.Contains(query.ToLower()) || x.TaiKhoanDich.Contains(query.ToLower()));
            }
            var result = q.OrderByDescending(x => x.NgayChuyen).ToList();
            total = result.Count;
            return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
        }


        public string CreatePhieu(PhieuChuyenTienNoiBo obj, List<ChiTietPhieuChuyenTienNoiBo> httt,string giaTriTrenGiaoDien)
        {
            try
            {
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayChuyen).Days >= 0)
                    return "Không thực hiện được thao tác chuyển tiền trong khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán.";
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.NgayHachToan = obj.NgayChuyen;
                obj.MaPhieu = _commonService.CreateId(obj.MaLoaiPhieu);
                obj.IsDeleted = false;
                obj.IsActive = true;
                obj.CreatedBy = uid;
                obj.CreatedDate = DateTime.Now;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = DateTime.Now;
                double soTien = 0;
                foreach (var item in httt)
                {
                    if (item.IsDeleted == false && item.TaiKhoanNguon != "")
                    {
                        var temp = item.TaiKhoanNguon.Split('-');
                        item.TaiKhoanNguon = temp[0];
                        item.HinhThucThanhToan = temp[1];
                        item.Id = Guid.NewGuid();
                        item.MaPhieu = obj.MaPhieu;
                        item.IsActive = true;
                        item.IsDeleted = false;
                        item.CreatedBy = uid;
                        item.CreatedDate = DateTime.Now;
                      //  item.UpdatedDate = DateTime.Now;
                      //  item.UpdatedBy = uid;
                        _CTPCTNBRepository.Insert(item);
                        soTien += item.SoTienThanhToan;
                        if (item.TinhTrang)
                            item.NgayTreoTien = obj.NgayChuyen;
                    }
                }
                _PCTNBRepository.Insert(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaPhieu, obj.MaLoaiPhieu, "INSERT", "Create Phieu Chuyen Tien Noi Bo", "Mã phiếu :" + obj.MaPhieu + " mã loại phiếu :" + obj.MaLoaiPhieu, _authenticationService.GetAuthenticatedUser().UserId);
                XuLySauKhiCapNhatPhieuChuyenTien(obj.MaPhieu, "INSERT", giaTriTrenGiaoDien, obj.CreatedBy);
                return "";
            }
            catch (Exception e)
            {
                _log.WriteLog("ChuyenTienService.CreatePhieu", e.ToString());
                return e.Message;
            }
        }

        public void XuLySauKhiCapNhatPhieuChuyenTien(string maPhieu,string action, string giaTriTrenGiaoDien, Guid updatedBy)
        {
            SqlParameter paramMaPhieu = new SqlParameter("MaPhieu", maPhieu);//xử lý sau khi update          
            SqlParameter paramAction = new SqlParameter("action", action);//xử lý sau khi update
            SqlParameter paramGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);//xử lý sau khi update

            SqlParameter paramUpdatedBy = new SqlParameter("UpdatedBy", updatedBy);//xử lý sau khi update          
            SqlParameter paramIpClient = new SqlParameter("IpClient", _ipClient);//xử lý sau khi update
            SqlParameter paramHostName = new SqlParameter("hostNameClient", _hostNameClient);//xử lý sau khi update
            
            _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdatePhieuChuyenTienNoiBo", paramMaPhieu, paramAction, paramGiaTriTrenGiaoDien, paramUpdatedBy, paramIpClient, paramHostName);

        }

        public PhieuChuyenTienNoiBo GetPhieuChuyenTienNoiBo(string id)
        {
            return _PCTNBRepository.Table.FirstOrDefault(x => x.MaPhieu == id && x.IsDeleted == false && x.IsActive == true);
        }

        public List<ChiTietPhieuChuyenTienNoiBo> GetChiTietPhieuChuyenTienNoiBo(string id)
        {
            return _CTPCTNBRepository.Table.Where(x => x.MaPhieu == id && x.IsDeleted == false && x.IsActive == true).ToList();
        }

        public string UpdatePhieu(PhieuChuyenTienNoiBo obj, List<ChiTietPhieuChuyenTienNoiBo> httt)
        {
            try
            {
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayChuyen).Days >= 0)
                    return "Không thực hiện được thao tác chuyển tiền trong khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán.";

                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var model = GetPhieuChuyenTienNoiBo(obj.MaPhieu);
                if (model == null)
                    return "Không tìm thấy phiếu chuyển tiền";
                model.SoChungTu = obj.SoChungTu;
                model.NgayChuyen = obj.NgayChuyen;
                model.NgayHachToan = obj.NgayChuyen;
                model.NoiDung = obj.NoiDung;
                model.GhiChu = obj.GhiChu;
                model.KeToanTruong = obj.KeToanTruong;
                model.NguoiLapPhieu = obj.NguoiLapPhieu;
                model.NguoiNopTien = obj.NguoiNopTien;
                model.NguoiThuTien = obj.NguoiThuTien;
                model.UpdatedBy = uid;
                model.UpdatedDate = DateTime.Now;
                foreach (var item in httt)
                {
                    var data = _CTPCTNBRepository.Table.FirstOrDefault(x => x.Id == item.Id);
                    if (!item.IsDeleted)
                    {
                        bool flag = true;
                        if (data == null)
                        {
                            data = new ChiTietPhieuChuyenTienNoiBo();
                            data.Id = Guid.NewGuid();
                            data.IsDeleted = false;
                            data.IsActive = true;
                            data.CreatedBy = uid;
                            data.CreatedDate = DateTime.Now;
                            flag = false;
                        }
                        var temp = item.TaiKhoanNguon.Split('-');
                        item.TaiKhoanNguon = temp[0];
                        item.HinhThucThanhToan = temp[1];
                        data.TaiKhoanDich = item.TaiKhoanDich;
                        //data.TaiKhoanNguon = item.TaiKhoanNguon;
                        data.SoThamChieu = item.SoThamChieu;
                        data.TinhTrang = item.TinhTrang;
                        data.SoTienThanhToan = item.SoTienThanhToan;
                        data.UpdatedBy = uid;
                        data.UpdatedDate = DateTime.Now;
                        if (!flag)
                        {
                            _CTPCTNBRepository.Insert(data);
                            _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, data.MaPhieu, data.SoThamChieu, "INSERT", "Create Phieu Chuyen Tien Noi Bo", "Mã phiếu :" + data.MaPhieu + " mã số tham chiếu :" + data.SoThamChieu, _authenticationService.GetAuthenticatedUser().UserId);
                        }
                        else
                        {
                            _CTPCTNBRepository.Update(data);
                            _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, data.MaPhieu, data.SoThamChieu, "UPDATE", "Update Phieu Chuyen Tien Noi Bo", "Mã phiếu :" + data.MaPhieu + " mã loại phiếu :" + data.SoThamChieu, _authenticationService.GetAuthenticatedUser().UserId);
                        }

                    }
                    else
                    {
                        if (data != null)
                        {
                            data.IsDeleted = true;
                            data.UpdatedDate = DateTime.Now;
                            data.UpdatedBy = uid;
                            _CTPCTNBRepository.Update(data);
                        }
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeletePhieu(string id)
        {
            try
            {
                var obj = GetPhieuChuyenTienNoiBo(id);
                if (obj == null)
                    return "Phiếu đã bị xóa trước đó.";
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayChuyen).Days >= 0)
                    return "Không thực hiện được thao tác chuyển tiền trong khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán.";

                obj.IsDeleted = true;
                obj.UpdatedDate = DateTime.Now;
                obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
                _PCTNBRepository.Update(obj);
                XuLySauKhiCapNhatPhieuChuyenTien(obj.MaPhieu, "DELETE", "",obj.CreatedBy);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<ViewTaiKhoanTien_NganHang> GetVTKTNH()
        {
            return VTKTNH.Table.ToList();
        }

        public void SetIPClient(string ip)
        {
            this._ipClient = ip;
        }
        public void SetHostNameClient(string host)
        {
            this._hostNameClient = host;
        }
        public DataTable XuatExcel(DateTime FromDate, DateTime ToDate, string Query)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                SqlParameter query = new SqlParameter("query", Query);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_XuatExcel_PhieuThuCTNB", fromdate, todate, query);
            }
            catch (Exception ex)
            {
                _log.WriteLog("ChuyenTienService.XuatExcel(" + FromDate + "," + ToDate + "):", ex);
                return null;
            }
        }
    }
}
