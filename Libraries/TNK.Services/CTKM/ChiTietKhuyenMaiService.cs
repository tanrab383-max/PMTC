using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Data;
using TNK.Services.Authentication;
using TNK.Services.Log;

namespace TNK.Services.CTKM
{
    public class ChiTietKhuyenMaiService : IChiTietKhuyenMaiService
    {
        IRepository<ChiTietKhuyenMai> _ctkmRepository;
        IRepository<ViewChiTietKhuyenMai> _viewCtkmRepository;
        IAuthenticationService _authenticationService;
        ILogger _log;
        IDbContext _dbContext;
        public ChiTietKhuyenMaiService(
            IRepository<ChiTietKhuyenMai> _ctkmRepository,
             IRepository<ViewChiTietKhuyenMai> _viewCtkmRepository,
            ILogger _log,
            IAuthenticationService _authenticationService
            , IDbContext _dbContext
            )
        {
            this._ctkmRepository = _ctkmRepository;
            this._viewCtkmRepository = _viewCtkmRepository;
            this._log = _log;
            this._authenticationService = _authenticationService;
            this._dbContext = _dbContext;
        }

        public List<ViewNguonKhuyenMai> GetNKM(bool isForInsert = true)
        {
            try
            {
                SqlParameter pIsForInsert = new SqlParameter("isForInsert", isForInsert);
                return _dbContext.ExecuteStoredProcedureList<ViewNguonKhuyenMai>("sp_SelectDanhSachKhuyenMai", pIsForInsert).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("ChiTietKhuyenMaiService.GetNKM " + ex.Message.ToString());
                return null;
            }
        }
        public List<ViewChiTietKhuyenMai> GetCTKM(string id)
        {
            try
            {
                return _viewCtkmRepository.Table.Where(x => x.MaPhieuThu == id && x.IsDeleted == false && x.IsActive == true).OrderBy(x=>x.TenChuongTrinh).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetCTKM " + ex.Message.ToString());
                return null;
            }
        }
        public List<ViewChiTietKhuyenMai> GetBDTK(string id)
        {
            try
            {
                return _viewCtkmRepository.Table.Where(x => x.MaPhieuThu == id && x.IsDeleted == false && x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetCTKM " + ex.Message.ToString());
                return null;
            }
        }
        public bool InsertCTKM(ChiTietKhuyenMai ctkm)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                ctkm.Id = Guid.NewGuid();
                ctkm.CreatedBy = uid;
                ctkm.UpdatedBy = uid;
                ctkm.CreatedDate = d;
                ctkm.UpdatedDate = d;
                ctkm.IsDeleted = false;
                ctkm.IsActive = true;
                _ctkmRepository.Insert(ctkm);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdateCTKM(ChiTietKhuyenMai obj)
        {
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var item = _ctkmRepository.Table.FirstOrDefault(x => x.Id == obj.Id);
                if (item != null)
                {
                    if (obj.IsDeleted == true)
                    {
                        item.IsDeleted = true;
                    }
                    else
                    {
                        item.NguonKhuyenMai = obj.NguonKhuyenMai;
                        item.NoiDung = obj.NoiDung;
                        item.GiaBan = obj.GiaBan;
                        item.GiaVon = obj.GiaVon;
                        item.GhiChu = obj.GhiChu;
                    }
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    _ctkmRepository.Update(item);
                }
                return true;

            }
            catch (Exception e)
            {
                _log.WriteLog("UpdateNoPhaiTra: " + e.Message.ToString());
                return false;
            }
        }
    }
}
