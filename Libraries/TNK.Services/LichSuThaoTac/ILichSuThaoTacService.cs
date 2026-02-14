using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.LichSuThaoTac
{
    public interface ILichSuThaoTacService
    {
        List<TNK.Core.Domain.View.ViewGetListLichSuThaoTac> GetLichSuThaoTac(Guid CreatedBy,DateTime FromDate,DateTime ToDate,string TuKhoa,ref int total, int pageSize,int p,string action);
        List<TNK.Core.Domain.LichSuThaoTac> GetLichSuThaoTac(string sessionId);
        DataTable GetThayDoiTuiTtien(string sessionId);
        List<TNK.Core.Domain.LichSuThayDoiTuiDinhKhoan> getListLichSuTuiDinhKhoan(DateTime fromDate, DateTime toDate, string query, int p);
        List<TNK.Core.Domain.LichSuThayDoiTuiDinhKhoan> getLichSuTuiTien(DateTime fromDate, DateTime toDate, string maTui, int p);
        List<TNK.Core.Domain.DataHistory> getHistoryChange(DateTime fromDate, DateTime toDate, string query, int p);
        void WriteLichSuThaoTac(string sessionId, string IPClient, string HostNameClient, string MaPhieu, string LoaiPhieu, string HanhDong, string GhiChu, string ChiTiet,Guid CreatedBy);
        DataTable LichSuThayDoiTuiTien(DateTime FromDate, DateTime ToDate, string MaTuiTien,int p);
        DataTable getLichSuThayDoiTuiTien(DateTime FromDate, DateTime ToDate, string MaTuiTien);
        TNK.Core.Domain.LichSuThaoTac GetLSTTGNY(string MaPhieu);
    }
}
