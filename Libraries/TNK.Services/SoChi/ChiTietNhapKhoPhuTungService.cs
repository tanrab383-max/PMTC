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

namespace TNK.Services.SoChi
{
    public class ChiTietNhapKhoPhuTungService : IChiTietNhapKhoPhuTungService
    {
        IRepository<ChiTietNhapKhoPhuTung> _ctnkptRepository;
        IAuthenticationService _authenticationService;
        ILogger _log;
        IDbContext _dbContext;
        public ChiTietNhapKhoPhuTungService(
            IRepository<ChiTietNhapKhoPhuTung> _ctnkptRepository,
            ILogger _log,
            IAuthenticationService _authenticationService
            , IDbContext _dbContext
            )
        {
            this._ctnkptRepository = _ctnkptRepository;
            this._log = _log;
            this._authenticationService = _authenticationService;
            this._dbContext = _dbContext;
        }

        public List<ViewNguonKhuyenMai> GetNKM()
        {
            try
            {
                SqlParameter isForInsert = new SqlParameter("isForInsert", true);
                return _dbContext.ExecuteStoredProcedureList<ViewNguonKhuyenMai>("sp_SelectDanhSachNhapKhoPhuTung", isForInsert).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("ChiTietNhapKhoPhuTungService.GetNKM: " + ex.Message.ToString());
                return null;
            }
        }
        public List<ChiTietNhapKhoPhuTung> GetCTNKPT(string id)
        {
            try
            {
                return _ctnkptRepository.Table.Where(x => x.MaPhieuChi == id && x.IsDeleted == false && x.IsActive == true).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("ChiTietNhapKhoPhuTungService.GetCTNKPT: " + ex.Message.ToString());
                return null;
            }
        }
        public bool InsertCTNKPT(ChiTietNhapKhoPhuTung ctkm)
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
                _ctnkptRepository.Insert(ctkm);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdateCTNKPT(ChiTietNhapKhoPhuTung obj)
        {
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var item = _ctnkptRepository.Table.FirstOrDefault(x => x.Id == obj.Id);
                if (item != null)
                {
                    if (obj.IsDeleted == true)
                    {
                        item.IsDeleted = true;
                    }
                    else
                    {
                        item.MaKho = obj.MaKho;
                        item.SoTien = obj.SoTien;
                        item.GhiChu = obj.GhiChu;
                    }
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = d;
                    _ctnkptRepository.Update(item);
                }
                return true;

            }
            catch (Exception e)
            {
                _log.WriteLog("UpdateCTNKPT: " + e.Message.ToString());
                return false;
            }
        }
    }
}
