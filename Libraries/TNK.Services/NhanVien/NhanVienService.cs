using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Data;
using TNK.Services.Authentication;
using TNK.Services.Log;

namespace TNK.Services.NhanVien
{
    public class NhanVienService : INhanVienService
    {
        IAuthenticationService _authenticationService;
        IRepository<TNK.Core.Domain.NhanVien> _NhanVienRepository;

        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;

        public NhanVienService(IAuthenticationService _authenticationService,
                                IRepository<TNK.Core.Domain.NhanVien> _NhanVienRepository,
                                IDbContext _dbContext,
                                ICacheManager _cacheManager,
                                ILogger _log)
        {
            this._authenticationService = _authenticationService;
            this._NhanVienRepository = _NhanVienRepository;
            this._dbContext = _dbContext;
            this._cacheManager = _cacheManager;
            this._log = _log;
        }
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateNhanVien(TNK.Core.Domain.NhanVien model)
        {
            try {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                model.CreatedBy = Convert.ToString(uid);
                model.UpdatedBy = Convert.ToString(uid);
                model.CreatedDate = d;
                model.IsDeleted = false;
                model.IsActive = true;
                model.MaNhanVien = Guid.NewGuid();
                _NhanVienRepository.Insert(model);
                return true;
            }
            catch(Exception e)
            {
                _log.WriteLog("NhanVienService.CreateNhanVien: " + e.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// Xóa nhân viên(update IsDelete = 1)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteNhanVien(Guid MaNhanVien)
        {
            try {
                var obj = _NhanVienRepository.Table.FirstOrDefault(x=>x.MaNhanVien == MaNhanVien && x.IsDeleted == false);
                if (obj == null)
                    return false;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedBy = Convert.ToString(uid);
                obj.IsDeleted = true;
                _NhanVienRepository.Update(obj);
                return true;
            }
            catch(Exception e)
            {
                _log.WriteLog("NhanVienService.DeleteNhanVien: " + e.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// Lấy một nhân viên để update
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Core.Domain.NhanVien GetNhanVien(Guid MaNhanVien)
        {
            try
            {
                return _NhanVienRepository.Table.FirstOrDefault(x => x.MaNhanVien == MaNhanVien && x.IsDeleted == false);
            }
            catch(Exception ex)
            {
                _log.WriteLog("NhanVienService.GetNhanVien: " + ex.Message.ToString());
                return null;
            }
            
        }
        /// <summary>
        /// Xuất danh sách nhân viên
        /// </summary>
        /// <param name="query"></param>
        /// <param name="p"></param>
        /// <param name="total"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<TNK.Core.Domain.NhanVien> GetNhanVien(string query, int p, ref int total, int pageSize)
        {
            try
            {
                var list = _NhanVienRepository.Table.Where(x => x.IsDeleted == false).ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(x => x.HoTen.ToLower().Contains(query.ToLower()) && x.IsActive == true).ToList();
                }
                var result = list.OrderByDescending(x => x.HoTen).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("NhanVienService.GetNhanVien:(" + query + "," +p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Cập nhật nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateNhanVien(TNK.Core.Domain.NhanVien model)
        {
            try
            {
                var obj = _NhanVienRepository.Table.FirstOrDefault(x => x.MaNhanVien == model.MaNhanVien && x.IsDeleted == false);
                if (obj == null)
                    return false;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.GioiTinh = model.GioiTinh;
                obj.HoTen = model.HoTen;
                obj.IsActive = model.IsActive;
                obj.UpdatedBy = Convert.ToString(uid);
                obj.UpdatedDate = DateTime.Now;
                _NhanVienRepository.Update(obj);
                return true;
            }
            catch(Exception e)
            {
                _log.WriteLog("NhanVienService.UpdateNhanVien: " + e.Message.ToString());
                return false;
            }
        }
    }
}
