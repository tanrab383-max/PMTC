using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Data;
using TNK.Services.Authentication;
using TNK.Services.Log;

namespace TNK.Services.No
{
    public class NoPhaiThuService : INoPhaiThuService
    {
        IRepository<NoPhaiThu> _noPhaiThuRepository;
        IAuthenticationService _authenticationService;
        IDbContext _dbContext;
        ILogger _log;
        public NoPhaiThuService(
            IRepository<NoPhaiThu> _noPhaiThuRepository,
            IDbContext _dbContext,
            ILogger _log,
            IAuthenticationService _authenticationService
            )
        {
            this._noPhaiThuRepository = _noPhaiThuRepository;
            this._authenticationService = _authenticationService;
            this._dbContext = _dbContext;
            this._log = _log;
        }
        public List<NoPhaiThu> GetNoPhaiThu(string maPhieuPhatSinh, string type = "")
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    List<NoPhaiThu> lst = _noPhaiThuRepository.Table.Where(x => x.MaPhieuPhatSinh == maPhieuPhatSinh && x.IsDeleted == false).ToList();
                    return lst;
                }
                else
                    return _noPhaiThuRepository.Table.Where(x => x.MaPhieuPhatSinh == maPhieuPhatSinh && x.IsDeleted == false && x.LoaiPhieuNo.ToLower() == type.ToLower()).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("NoPhaiThuService.GetNoPhaiThu(" + maPhieuPhatSinh + "," + type + "): " + ex.Message.ToString());
                return null;
            }
        }

        public NoPhaiThu Get(string id)
        {
            try
            {
                return _noPhaiThuRepository.Table.FirstOrDefault(x => x.MaPhieuNo == id);
            }
            catch(Exception ex)
            {
                _log.WriteLog("NoPhaiThuService.GetNoPhGetaiThu(" + id + "): " + ex.Message.ToString());
                return null;
            }
        }

        public bool UpdateMaPhieuThu(string id, string maphieuthu)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                var obj = Get(id);
                obj.MaPhieuThu = maphieuthu;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = d;
                _noPhaiThuRepository.Update(obj);
                return true;
            }
            catch(Exception ex)
            {
                _log.WriteLog("NoPhaiThuService.UpdateMaPhieuThu("+id+","+ maphieuthu+"): " + ex.Message.ToString());
                return false;
            }
        }

        public bool UpdateNoPhaiThu(NoPhaiThu model)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                var obj = Get(model.MaPhieuNo);
                if (obj != null)
                {
                    if (model.IsDeleted == true)
                    {
                        obj.IsDeleted = true;
                    }
                    else
                    {
                        obj.SoTienNo = model.SoTienNo;
                        obj.NguoiBaoLanh = model.NguoiBaoLanh;
                    }
                    if (!string.IsNullOrEmpty(model.DonViNo))
                        obj.DonViNo = model.DonViNo;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    obj.GhiChu = model.GhiChu;
                    _noPhaiThuRepository.Update(obj);
                    XuLyCapNhatNoPhaiThu(obj.MaPhieuNo);
                }
                return true;
            }
            catch (Exception e)
            {
               // _log.WriteLog("NoPhaiThuService.UpdateNoPhaiThu: " + e.Message.ToString());
                _log.WriteLog("NoPhaiThuService.UpdateNoPhaiThu(" + model +"): " + e.Message.ToString());
                return false;
            }
        }
        public bool InsertNoPhaiThu(string loaiphieu, string maphieuthu, string nguoibaolanh, double sotienno, string donvino, string ghichu)
        {
            try
            {
                SqlParameter MPN = new SqlParameter("MaPhieuNo", System.Data.SqlDbType.Char);
                MPN.Size = 15;
                MPN.Direction = System.Data.ParameterDirection.Output;
                SqlParameter LPN = new SqlParameter("LoaiPhieuNo", loaiphieu);
                SqlParameter MPPS = new SqlParameter("MaPhieuPhatSinh", maphieuthu);
                SqlParameter TTDT = new SqlParameter("NguoiBaoLanh", nguoibaolanh == null ? "" : nguoibaolanh);
                SqlParameter STN = new SqlParameter("SoTienNo", sotienno);
                SqlParameter DVN = new SqlParameter("DonViNo", donvino);
                SqlParameter GC = new SqlParameter("GhiChu", ghichu);
                _dbContext.ExecuteStoredProcedure("sp_InsertNoPhaiThu", MPN, LPN, MPPS, TTDT, STN, DVN, GC);
                return true;
            }
            catch (Exception e)
            {
                _log.WriteLog("NoPhaiThuService.InsertNoPhaiThu: " + e.Message.ToString());
                return false;
            }
        }

        public bool XuLyCapNhatNoPhaiThu(string maPhieuNo)
        {
            try
            {
                SqlParameter MPN = new SqlParameter("MaPhieuNo", System.Data.SqlDbType.Char);
                MPN.Size = 20;
                MPN.Direction = System.Data.ParameterDirection.Input;
                MPN.Value = maPhieuNo;            
                _dbContext.ExecuteStoredProcedure("sp_XuLyTinhToanLaiPhieuNoPhaiThu", MPN);
                return true;
            }
            catch (Exception e)
            {
                _log.WriteLog("NoPhaiThuService.XuLyCapNhatNoPhaiThu: " + e.Message.ToString());
                return false;
            }
        }

    }
}
