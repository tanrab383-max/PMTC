using System;
using System.Collections.Generic;
using System.Data;
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
using TNK.Services.Log;
using TNK.Services.Manager;
namespace TNK.Services.KhoXe
{
    public class KhoPhuTungService : IKhoPhuTungService
    {
        IAuthenticationService _authenticationService;
        IRepository<KhoPhuTung> _KhoPhuTungRepository;
        IRepository<DieuChinhKhoPhuTung> _DieuChinhKhoPhuTungRepository;
        IRepository<View_DSDieuChinhKhoPhuTung> _View_DSDieuChinhKhoPhuTungRepository;
        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;
        ISoKetChuyenService _soKetChuyenService;
        public KhoPhuTungService(
                IAuthenticationService _authenticationService,
                IRepository<KhoPhuTung> _KhoPhuTungRepository,
              IRepository<DieuChinhKhoPhuTung> _DieuChinhKhoPhuTungRepository,
              IRepository<View_DSDieuChinhKhoPhuTung> _View_DSDieuChinhKhoPhuTungRepository,
        IDbContext _dbContext,
                ICacheManager _cacheManager,
                ILogger _log,
                ISoKetChuyenService _soKetChuyenService
            )
        {
            this._authenticationService = _authenticationService;
            this._KhoPhuTungRepository = _KhoPhuTungRepository;
            this._DieuChinhKhoPhuTungRepository = _DieuChinhKhoPhuTungRepository;
            this._View_DSDieuChinhKhoPhuTungRepository = _View_DSDieuChinhKhoPhuTungRepository;
            this._dbContext = _dbContext;
            this._cacheManager = _cacheManager;
            this._log = _log;
            this._soKetChuyenService = _soKetChuyenService;
        }
        public bool CreateKhoPhuTung(KhoPhuTung model)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;            
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                _KhoPhuTungRepository.Insert(model);
                
                return true;
            }
            catch (Exception e)
            {
                _log.WriteLog("KhoPhuTungService:CreatePhuTung:("+model+") " + e.Message.ToString());
                return false;
            }
        }
        public bool UpdateKhoPhuTung(KhoPhuTung model)
        {
            try
                  {
                var obj = _KhoPhuTungRepository.Table.FirstOrDefault(x => x.MaKho == model.MaKho && x.IsDeleted == false);
                if (obj == null)
                    return false;
               
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.PhanTramGiaVon = model.PhanTramGiaVon;
               // obj.GiaMua = model.GiaMua;
               // obj.SoTienKhoiTao = model.GiaTriConLai;
               // obj.HinhThucKhauHao = model.HinhThucKhauHao;
               // obj.IsDeleted = model.IsDeleted;
              //  obj.MaLoaiTS = model.MaLoaiTS;
               // obj.MaPhieuNhap = model.MaPhieuNhap;
               // obj.MaPhieuXuat = model.MaPhieuXuat;
               // obj.NgayNhapKho = model.NgayNhapKho;
              //  obj.NhaCungCap = model.NhaCungCap;
               // obj.TenPhuTung = model.TenPhuTung;
              //  obj.ThoiGianKhauHao = model.ThoiGianKhauHao;
                
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                
                _KhoPhuTungRepository.Update(obj);
               
                return true;
            }
            catch (Exception e)
            {
                _log.WriteLog("KhoPhuTungService:UpdatePhuTung: " + e.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// Xoa tai san (cap nhật isdeleted = 1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteKhoPhuTung(string id)
        {
            try
            {
                var obj = _KhoPhuTungRepository.Table.FirstOrDefault(x => x.MaKho == id && x.IsDeleted == false);
                if (obj == null)
                    return false;
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                _KhoPhuTungRepository.Update(obj);
            }
            catch (Exception e)
            {
                _log.WriteLog("KhoPhuTungService:DeletePhuTung: " + e.Message.ToString());
            }
            return false;
        }
        /// <summary>
        /// Tim kiem danh sach tai san trong kho
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="query"></param>
        /// <param name="p"></param>
        /// <param name="total"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<KhoPhuTung> GetKhoPhuTung(string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _KhoPhuTungRepository.Table.Where(x => x.IsDeleted == false);
                if (!string.IsNullOrEmpty(query))
                {
                    q = q.Where(x => x.MaKho.ToLower().Contains(query.ToLower()));
                }
                var result = q.OrderByDescending(x => x.MaKho).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("KhoPhuTungService:GetKhoPhuTung:("+query+","+ p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Lay tai san de edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public KhoPhuTung GetKhoPhuTung(string id)
        {
            try
            {
                return _KhoPhuTungRepository.Table.FirstOrDefault(x => x.MaKho == id && x.IsDeleted == false);
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoPhuTungService:GetKhoPhuTung:(" + id + ") " + ex.Message.ToString());
                return null;
            }
        }
        //phan truc edit kho phu tung
        public string EditKhoPhuTung(DieuChinhKhoPhuTung obj)
        {
            try
            {
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayDieuChinh).Days >= 0)
                    return "Không thể điều chỉnh kho trong khoảng thời gian đã kết chuyển(" + ngayKetChuyenCuoi.ToString("dd/MM/yyyy") + "). Vui lòng liên hệ Phòng Kiểm toán.";
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.CreatedBy = uid;
                obj.CreatedDate = DateTime.Now;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = DateTime.Now;
                obj.IsActive = true;
                obj.IsDeleted = false;
                obj.ID = Guid.NewGuid();
                _DieuChinhKhoPhuTungRepository.Insert(obj);
                SqlParameter idDieuChinhKho = new SqlParameter("idDieuChinhKho", obj.ID);
                _dbContext.ExecuteStoredProcedure("sp_XuLyCapNhatDieuChinhKho", idDieuChinhKho);
                return "";
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoPhuTungService:EditKhoPhuTung " + ex.Message.ToString());
                return ex.Message;
            }
        }
        public DataTable GetList()
        {
            try
            {
                DataTable result = _dbContext.ExecuteStoredProcedureDataTable("sp_ListDieuChinhKhoPhuTung");
                return result;
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoPhuTungService:GetList  " + ex.Message.ToString());
                return null;
            }
        }
        public string DeleteDieuChinhKho(Guid ID)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DieuChinhKhoPhuTung obj = _DieuChinhKhoPhuTungRepository.Table.Where(x => x.ID == ID).FirstOrDefault();
                if (obj == null)
                    return "Không tìm thấy mục điều chỉnh khoa hoặc mục này đã bị xóa trước đó";
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayDieuChinh).Days >= 0)
                    return "Không thể điều chỉnh kho trong khoảng thời gian đã kết chuyển(" + ngayKetChuyenCuoi.ToString("dd/MM/yyyy") + "). Vui lòng liên hệ Phòng Kiểm toán.";
                obj.IsDeleted = true;
                obj.IsActive = false;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = DateTime.Now;
                _DieuChinhKhoPhuTungRepository.Update(obj);
                SqlParameter idDieuChinhKho = new SqlParameter("idDieuChinhKho", obj.ID);
                _dbContext.ExecuteStoredProcedure("sp_XuLyCapNhatDieuChinhKho", idDieuChinhKho);
                return "";
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoPhuTungService:DeleteDieuChinhKho : '"+ ID +"'" + ex.Message.ToString());
                return ex.Message;
            }
        }
    }
}
