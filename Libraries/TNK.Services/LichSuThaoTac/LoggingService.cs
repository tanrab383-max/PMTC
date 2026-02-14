using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Data;
using TNK.Services.Authentication;
using TNK.Services.Log;

namespace TNK.Services.LichSuThaoTac
{
    public class LoggingService :ILoggingService
    {
        IAuthenticationService _authenticationService;
        IRepository<TNK.Core.Domain.LichSuThaoTac> _LichSuThaoTacRepository;
        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;

        public LoggingService(IAuthenticationService _authenticationService,
                               IRepository<TNK.Core.Domain.LichSuThaoTac> _LichSuThaoTacRepository,
                               IDbContext _dbContext,
                               ICacheManager _cacheManager,
                               ILogger _log)
        {
            this._authenticationService = _authenticationService;
            this._LichSuThaoTacRepository = _LichSuThaoTacRepository;
            this._dbContext = _dbContext;
            this._cacheManager = _cacheManager;
            this._log = _log;
        }
        //phan lay danh sanh logging
        public DataTable getLogging(Guid UserID, DateTime FromDate, DateTime ToDate, string TuKhoa, ref int total, int pageSize, int p)
        {
            try
            {
                SqlParameter userID = new SqlParameter("UserID", UserID);
                SqlParameter fromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter toDate = new SqlParameter("ToDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_GetListLogging",userID,fromDate,toDate);
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoggingService.getLogging:(" + UserID + "," + FromDate + "," + ToDate + "," + TuKhoa + ") " + ex.Message.ToString());
                return null;
            }
        }
        //phan lich su dang nhap
        public List<TNK.Core.Domain.Login> getLogin(Guid UserID, DateTime FromDate, DateTime ToDate, string TuKhoa, ref int total, int pageSize, int p)
        {
            try
            {
                SqlParameter userID = new SqlParameter("UserID", UserID);
                SqlParameter fromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter toDate = new SqlParameter("ToDate", ToDate);
                SqlParameter tuKhoa = new SqlParameter("TuKhoa", TuKhoa);
                var list = _dbContext.ExecuteStoredProcedureList<TNK.Core.Domain.Login>("sp_getLichSuLogin", userID, fromDate, toDate,tuKhoa).ToList();
                total = list.Count;
                return list.Skip((p - 1) * pageSize).Take(pageSize).OrderByDescending(x=>x.CreatedDate).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoggingService.getLogin:(" + UserID + "," + FromDate + "," + ToDate + "," + TuKhoa + ") " + ex.Message.ToString());
                return null;
            }
        }
        //lịch sử dữ liệu
        public DataTable getDataHistory(Guid UserID, DateTime FromDate, DateTime ToDate, string TuKhoa, ref int total, int pageSize, int p)
        {
            try
            {
                SqlParameter userID = new SqlParameter("UserID", UserID);
                SqlParameter fromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter toDate = new SqlParameter("ToDate", ToDate);
                SqlParameter query = new SqlParameter("Query", TuKhoa);
                var list = _dbContext.ExecuteStoredProcedureDataTable("sp_getDataHistory", userID, fromDate, toDate,query);
                total = list.AsEnumerable().Count();
                DataView dv = list.DefaultView;
                dv.Sort = "ChangedDate desc";
                DataTable sortedDT = dv.ToTable();
                return sortedDT.AsEnumerable().Skip((p - 1) * pageSize).Take(pageSize).CopyToDataTable();
            }
            catch (Exception ex)
            {
                _log.WriteLog("LoggingService.getLogging:(" + UserID + "," + FromDate + "," + ToDate + "," + TuKhoa + ") " + ex.Message.ToString());
                return null;
            }
        }
        public DataTable getHistory(string SessionId, string TableName,string MaLoaiPhieu)
        {
            try
            {
                SqlParameter spSessionId = new SqlParameter("SessionId", SessionId);
                SqlParameter spTableName = new SqlParameter("TableName", TableName);
                SqlParameter spMaLoaiPhieu = new SqlParameter("MaLoaiPhieu", MaLoaiPhieu);
                return _dbContext.ExecuteStoredProcedureDataTable("spgetDataHistory_Old", spSessionId, spTableName,spMaLoaiPhieu);
            }
            catch (Exception ex)
            {
                _log.WriteLog("LoggingService.getHistory:(" + SessionId + "," + TableName +") " + ex.Message.ToString());
                return null;
            }
        }
        public DataTable getHistory_New(string Id)
        {
            try
            {
                Guid id = Guid.Parse(Id);
                SqlParameter spId = new SqlParameter("Id", id);
                return _dbContext.ExecuteStoredProcedureDataTable("spgetDataHistory_New", spId);
            }
            catch (Exception ex)
            {
                _log.WriteLog("LoggingService.getHistory_New:(" + Id + ") " + ex.Message.ToString());
                return null;
            }
        }

        public DataTable getDataHistory(string maPhieu, string tableName)
        {
            try
            {
                
                SqlParameter spMaPhieu = new SqlParameter("Id", maPhieu);
                SqlParameter spTableName = new SqlParameter("TableName", tableName );
                return _dbContext.ExecuteStoredProcedureDataTable("spgetDataHistoryCyber", spMaPhieu,spTableName);
            }
            catch (Exception ex)
            {
                _log.WriteLog("LoggingService.getHistory_New:(" + maPhieu + "," + tableName + ") " + ex.Message.ToString());
                return null;
            }
        }
    }
}
