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

namespace TNK.Services.KhoXe
{
    public class KhoTaiSanService : IKhoTaiSanService
    {
        IAuthenticationService _authenticationService;
        IRepository<KhoTaiSan> _khoTaiSanRepository;
        IRepository<ViewKhoTaiSan> _viewKhoTaiSanRepository;
        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;
        public KhoTaiSanService(
                IAuthenticationService _authenticationService,
                IRepository<KhoTaiSan> _khoTaiSanRepository,
                 IRepository<ViewKhoTaiSan> _viewKhoTaiSanRepository,
                    IDbContext _dbContext,
                ICacheManager _cacheManager,
                ILogger _log
            )
        {
            this._authenticationService = _authenticationService;
            this._khoTaiSanRepository = _khoTaiSanRepository;
            this._viewKhoTaiSanRepository = _viewKhoTaiSanRepository;
            this._dbContext = _dbContext;
            this._cacheManager = _cacheManager;
            this._log = _log;
        }
        public bool CreateTaiSan(KhoTaiSan model)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.Id = Guid.NewGuid();
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                _khoTaiSanRepository.Insert(model);

                return true;
            }
            catch (Exception e)
            {
                _log.WriteLog("KhoTaiSanService : CreateTaiSan: " + e.Message.ToString());
                return false;
            }
        }
        public bool UpdateTaiSan(KhoTaiSan model)
        {
            try
            {
                var obj = _khoTaiSanRepository.Table.FirstOrDefault(x => x.Id == model.Id && x.IsDeleted == false);
                if (obj == null)
                    return false;

                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.GhiChu = model.GhiChu;
                // obj.GiaMua = model.GiaMua;
                obj.GiaTriConLai = model.GiaTriConLai;
                // obj.HinhThucKhauHao = model.HinhThucKhauHao;
                // obj.IsDeleted = model.IsDeleted;
                obj.MaLoaiTS = model.MaLoaiTS;
                // obj.MaPhieuNhap = model.MaPhieuNhap;
                // obj.MaPhieuXuat = model.MaPhieuXuat;
                // obj.NgayNhapKho = model.NgayNhapKho;
                obj.NhaCungCap = model.NhaCungCap;
                // obj.TenTaiSan = model.TenTaiSan;
                //  obj.ThoiGianKhauHao = model.ThoiGianKhauHao;

                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;

                _khoTaiSanRepository.Update(obj);

                return true;
            }
            catch (Exception e)
            {
                _log.WriteLog("KhoTaiSanService : UpdateTaiSan: " + e.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// Xoa tai san (cap nhật isdeleted = 1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTaiSan(Guid id)
        {
            try
            {
                var obj = _khoTaiSanRepository.Table.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                if (obj == null)
                    return false;
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                _khoTaiSanRepository.Update(obj);
            }
            catch (Exception e)
            {
                _log.WriteLog("KhoTaiSanService : DeleteTaiSan: " + e.Message.ToString());
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
        public List<ViewKhoTaiSan> GetTaiSan(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _viewKhoTaiSanRepository.Table.Where(x => x.NgayNhapKho >= from && x.NgayNhapKho <= to);
                if(query != null)
                {
                    q = q.Where(x => x.MaPhieuNhap.ToLower().Contains(query) ||
                                 x.TenLoaiTS.ToLower().Contains(query) ||
                                 x.TenTaiSan.ToLower().Contains(query));
                }
                var result = q.OrderByDescending(x => x.NgayNhapKho).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoTaiSanService : GetTaiSan: " + ex.Message.ToString());
                return null;
            }
        }
        public List<ViewKhoTaiSan> GetListAll(DateTime from, DateTime to, string query)
        {
            try
            {

                var q = _viewKhoTaiSanRepository.Table.Where(x => x.NgayNhapKho >= from && x.NgayNhapKho <= to);
                if (query != null)
                {
                    q = q.Where(x => x.MaPhieuNhap.ToLower().Contains(query) ||
                                 x.TenLoaiTS.ToLower().Contains(query) ||
                                 x.TenTaiSan.ToLower().Contains(query));
                }
                var result = q.OrderByDescending(x => x.NgayNhapKho).ToList();
               
                return result;
            }
            catch (Exception ex)
            {
                _log.WriteLog("KhoTaiSanService.GetListAll:" + ex);
                return null;
            }
        }
        /// <summary>
        /// Lay tai san de edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public KhoTaiSan GetTaiSan(Guid id)
        {
            try
            {
                return _khoTaiSanRepository.Table.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoTaiSanService : GetTaiSan:("+id+") " + ex.Message.ToString());
                return null;
            }
        }

        public List<ViewKhoTaiSan> GetTaiSan()
        {
            try
            {
                return _viewKhoTaiSanRepository.Table.Where(x=>x.TinhTrang == "N").ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoTaiSanService : GetTaiSan " + ex.Message.ToString());
                return null;
            }
        }
        public List<ViewKhoTaiSan> GetTaiSan_Edit()
        {
            try
            {
                return _viewKhoTaiSanRepository.Table.ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("KhoTaiSanService : GetTaiSan_Edit " + ex.Message.ToString());
                return null;
            }
        }
    }
}
