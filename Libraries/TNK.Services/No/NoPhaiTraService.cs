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
using TNK.Services.Log;

namespace TNK.Services.No
{
    public class NoPhaiTraService : INoPhaiTraService
    {

        IDbContext _dbContext;
        ILogger _log;
        ICacheManager _cacheManager;
        IRepository<NoPhaiTra> _noPhaiTraRepository;
        IRepository<ViewNoPhaiTra> _viewNoPhaiTraRepository;
        IAuthenticationService _authenticationService;
        public NoPhaiTraService(
            IRepository<NoPhaiTra> _noPhaiTraRepository
            , ICacheManager _cacheManager
            , IDbContext _dbContext
            , ILogger _log
            , IAuthenticationService _authenticationService
            , IRepository<ViewNoPhaiTra> _viewNoPhaiTraRepository
            )
        {
            this._noPhaiTraRepository = _noPhaiTraRepository;
            this._cacheManager = _cacheManager;
            this._authenticationService = _authenticationService;
            this._dbContext = _dbContext;
            this._log = _log;
            this._viewNoPhaiTraRepository = _viewNoPhaiTraRepository;
        }


        public List<NoPhaiTra> GetNoPhaiTra(string maPhieuPhatSinh, string type = "")
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                    return _noPhaiTraRepository.Table.Where(x => x.MaPhieuPhatSinh == maPhieuPhatSinh && x.IsDeleted == false).ToList();
                else
                    return _noPhaiTraRepository.Table.Where(x => x.MaPhieuPhatSinh == maPhieuPhatSinh && x.IsDeleted == false && x.LoaiPhieuNo.ToLower() == type.ToLower()).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("NoPhaiThuService.GetNoPhaiTra(" + maPhieuPhatSinh + ","+type+"): " + ex.Message.ToString());
                return null;
            }
        }
        public NoPhaiTra Get(string id)
        {
            try
            {
                return _noPhaiTraRepository.Table.FirstOrDefault(x => x.MaPhieuNo == id);
            }
            catch(Exception ex)
            {
                _log.WriteLog("NoPhaiThuService.Get(" + id + "): " + ex.Message.ToString());
                return null;
            }
        }
        public bool UpdateMaPhieuChi(string id, string maphieuchi)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                var obj = Get(id);
                obj.MaPhieuChi = maphieuchi;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                _noPhaiTraRepository.Update(obj);
                return true;
            }
            catch(Exception ex)
            {
                _log.WriteLog("NoPhaiTraService.UpdateMaPhieuChi(" + id + "," + maphieuchi + "): " + ex.Message.ToString());
                return false;
            }
        }

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
                    obj.SoTienConLai = obj.SoTienNo - obj.SoTienDaTra;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    obj.DonViNo = info.DonViNo;
                    _noPhaiTraRepository.Update(obj);
                }
                return true;
            }
            catch (Exception e)
            {
                //_log.WriteLog("UpdateNoPhaiTra: " + e.Message.ToString());
                _log.WriteLog("NoPhaiTraService.UpdateNoPhaiTra(" + info + "): " + e.Message.ToString());
                return false;
            }
        }

        public bool DeleteNoPhaiTra(NoPhaiTra info,string note)
        {

            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var obj = _noPhaiTraRepository.Table.FirstOrDefault(x => x.MaPhieuNo == info.MaPhieuNo);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.GhiChu = note;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    _noPhaiTraRepository.Update(obj);
                }
                return true;
            }
            catch (Exception e)
            {
                //_log.WriteLog("UpdateNoPhaiTra: " + e.Message.ToString());
                _log.WriteLog("NoPhaiTraService.DeleteNoPhaiTra(" + info +","+note+ "): " + e.Message.ToString());
                return false;
            }
        }
        public bool InsertNoPhaiTra(NoPhaiTra info, string maPhieuPhatSinh)
        {
            try
            {
                if (info.SoTienNo > 0)
                {
                    SqlParameter MPN = new SqlParameter("MaPhieuNo", System.Data.SqlDbType.NVarChar);
                    MPN.Size = 15;
                    MPN.Direction = System.Data.ParameterDirection.Output;
                    SqlParameter LPN = new SqlParameter("LoaiPhieuNo", info.LoaiPhieuNo);
                    SqlParameter MPPS = new SqlParameter("MaPhieuPhatSinh", maPhieuPhatSinh);
                    SqlParameter TTDT = new SqlParameter("ThongTinDoiTac", info.ThongTinDoiTac);
                    SqlParameter STN = new SqlParameter("SoTienNo", info.SoTienNo);
                    SqlParameter STDT = new SqlParameter("SoTienDaTra", info.SoTienDaTra);
                    SqlParameter STCL = new SqlParameter("SoTienConLai", info.SoTienConLai);
                    SqlParameter NBL = new SqlParameter("NguoiBaoLanh", info.NguoiBaoLanh==null?"":info.NguoiBaoLanh);
                    SqlParameter NPD = new SqlParameter("NguoiPheDuyet", info.NguoiPheDuyet ==null?"": info.NguoiPheDuyet);
                    SqlParameter GhiChu = new SqlParameter("GhiChu", info.GhiChu ==null?"":info.GhiChu);
                    _dbContext.ExecuteStoredProcedure("sp_InsertNoPhaiTra", MPN, LPN, MPPS, TTDT, STN, STDT, STCL, NBL, NPD,GhiChu);
                }
                return true;
            }
            catch (Exception e)
            {
                //_log.WriteLog("InsertNoPhaiTra: " + e.Message.ToString());
                _log.WriteLog("NoPhaiTraService.InsertNoPhaiTra(" + info + "," + maPhieuPhatSinh + "): " + e.Message.ToString());
                return false;
            }
        }
        public ViewNoPhaiTra getNoPhaiTra(string MaPhieuPhatSinh)
        {
            try
            {
                return _viewNoPhaiTraRepository.Table.Where(x => x.MaPhieuPhatSinh == MaPhieuPhatSinh).FirstOrDefault();
            }
            catch(Exception ex)
            {
                _log.WriteLog("NoPhaiTraService.getNoPhaiTra " + ex.Message.ToString());
                return null;
            }
        }
    }
}
