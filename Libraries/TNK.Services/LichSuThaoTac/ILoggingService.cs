using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.LichSuThaoTac
{
    public interface ILoggingService
    {
        DataTable getLogging(Guid UserID, DateTime FromDate, DateTime ToDate, string TuKhoa,ref int total, int pageSize, int p);
        List<TNK.Core.Domain.Login> getLogin(Guid UserID, DateTime FromDate, DateTime ToDate, string TuKhoa, ref int total, int pageSize, int p);
        //getDataHistory
        DataTable getDataHistory(Guid UserID, DateTime FromDate, DateTime ToDate, string TuKhoa, ref int total, int pageSize, int p);
        DataTable getHistory(string SessionId, string TableName,string MaLoaiPhieu);
        DataTable getHistory_New(string Id);
        /// <summary>
        /// Lay so lieu cho history cua cac phieu dong bo Cyber
        /// </summary>
        /// <param name="maPhieu"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataTable getDataHistory(string maPhieu, string tableName);
    }
}
