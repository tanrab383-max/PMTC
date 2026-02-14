using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Data;
using TNK.Services.Log;

namespace TNK.Services.SoQuyetToan
{
    public class SoQuyetToanService : ISoQuyetToanService
    {
        IDbContext _dbContext;
        ILogger _log;
        public SoQuyetToanService(IDbContext _dbContext, ILogger _log)
        {
            this._dbContext = _dbContext;
            this._log = _log;
        }
        public string Create(string maPhieuNos, Guid nguoiLapPhieu)
        {
            try
            {
                try
                {
                    SqlParameter maPhieuNo = new SqlParameter("maPhieuNo", maPhieuNos);
                    SqlParameter nguoiLapPhieu_ = new SqlParameter("nguoiLapPhieu", nguoiLapPhieu);
                    return _dbContext.ExecuteStoredProcedureToString("sp_InsertQuyetToan", maPhieuNo, nguoiLapPhieu_);
                }
                catch (Exception ex)
                {
                    _log.WriteLog("SoQuyetToanService.Create", ex);
                    return null;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
        public List<ViewNoPhaiTra> GetDanhSachNKMPK(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang)
        {
            try
            {
                SqlParameter _from = new SqlParameter("from", from);
                SqlParameter _to = new SqlParameter("to", to);
                SqlParameter _query = new SqlParameter("query", query);
                SqlParameter _tinhtrang = new SqlParameter("tinhtrang", tinhtrang);
                var result = _dbContext.ExecuteStoredProcedureList<ViewNoPhaiTra>("sp_GetDanhSachNKMPK", _from, _to, _tinhtrang, _query);
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
                _log.WriteLog("SoChiService.GetDanhSachNKMPK(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):", ex);
                return null;
            }
        }
        public List<PhieuQTModel> GetDanhSachPhieuQT(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang, string loai)
        {
            try
            {
                SqlParameter _from = new SqlParameter("from", from);
                SqlParameter _to = new SqlParameter("to", to);
                SqlParameter _query = new SqlParameter("query", query);
                SqlParameter _loai = new SqlParameter("loai", loai);
                var result = _dbContext.ExecuteStoredProcedureList<PhieuQTModel>("sp_GetDanhSachPhieuQuyetToan", _from, _to, _loai, _query);
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
                _log.WriteLog("SoChiService.GetDanhSachPhieuQT(" + id + "," + from + "," + to + "," + query + "," + p + "," + total + "," + pageSize + "):", ex);
                return null;
            }
        }
        public string Delete(string id, Guid nguoiLapPhieu)
        {
            try
            {
                SqlParameter maPhieu = new SqlParameter("maPhieu", id);
                SqlParameter user = new SqlParameter("user", nguoiLapPhieu);
                _dbContext.ExecuteStoredProcedureList<PhieuQTModel>("sp_DeletePhieuQT", maPhieu, user);
                return string.Empty;
            }
            catch (Exception e)
            {
                _log.WriteLog("SoChiService.DeletePhieuChi: ", e);
                return "ERROR:" + e.ToString();
            }
        }
    }
}
public class PhieuQTModel : BaseEntity
{
    public string MaPhieuNo { get; set; }
    public DateTime NgayNo { get; set; }
    public string LoaiPhieuNo { get; set; }
    public string LyDoNo { get; set; }
    public string GhiChu { get; set; }
    public string ChungTuThu { get; set; }
    public string BienSo { get; set; }
    public string KhachHang { get; set; }
    public string DienThoai { get; set; }
    public string DiaChi { get; set; }
    public string SoKhung { get; set; }
    public string SoMay { get; set; }
    public string SoHopDong { get; set; }
    public double SoTienNo { get; set; }
    public double SoTienDaTra { get; set; }
    public double SoTienConLai { get; set; }
    public string ThongTinDonViNo { get; set; }
    public string DonViNo { get; set; }
    public string TinhTrang { get; set; }
    public string HSD { get; set; }
    public string HanSuDung { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string MaPhieuPhatSinh { get; set; }
    public string DoiTac { get; set; }
    public string Hoten { get; set; }
    public DateTime NgayQuyetToan { get; set; }
    public string MaPhieuQuyetToan { get; set; }
    public double? SoTienQuyetToan { get; set; }
}