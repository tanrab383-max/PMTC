using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Data;
using TNK.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Services.Authentication;
using TNK.Core.Caching;
using TNK.Services.Log;

namespace TNK.Services.Catalog
{
    public class GiaoDichTuiTienService : IGiaoDichTuiTienService
    {
        IRepository<ViewGiaoDichTuiTien> _giaoDichTuiTienRepository;
       
        IAuthenticationService _authenticationService;

        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;
        public GiaoDichTuiTienService(IRepository<ViewGiaoDichTuiTien> _giaoDichTuiTienRepository
            , IAuthenticationService _authenticationService
            , IDbContext _dbContext          
            , ICacheManager _cacheManager
            , ILogger _log
            )
        {

            this._giaoDichTuiTienRepository = _giaoDichTuiTienRepository;
            this._dbContext = _dbContext;  
            this._authenticationService = _authenticationService;    
            this._cacheManager = _cacheManager;
            this._log = _log;

        }
        
        public List<ViewGiaoDichTuiTien> GetDanhSachGiaoDich(string maTui, string loaiGiaoDich, DateTime from, DateTime to, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _giaoDichTuiTienRepository.Table.Where(x => x.CreatedDate >= from && x.CreatedDate <= to);
                //if (!string.IsNullOrEmpty(loaiGiaoDich))
                //{
                //    q = q.Where(x => x.LoaiThuChi == loaiGiaoDich);
                //}
                if (!string.IsNullOrEmpty(maTui))
                {
                    q = q.Where(x => x.MaTuiTien == maTui);
                }
                var result = q.OrderByDescending(x => x.CreatedDate).ToList();
                total = result.Count;
                return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("GiaoDichTuiTienService.GetDanhSachGiaoDich" + ex);
            }
            return null;
        }
    }
}
