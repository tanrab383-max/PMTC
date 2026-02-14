using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Services.ChuyenTien
{
    public interface IChuyenTienService
    {
        List<ViewChiTietPhieuChuyenTienNoiBo> GetVCTPCTNB(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        string CreatePhieu(PhieuChuyenTienNoiBo obj, List<ChiTietPhieuChuyenTienNoiBo> httt, string giaTriTrenGiaoDien);
        string UpdatePhieu(PhieuChuyenTienNoiBo obj, List<ChiTietPhieuChuyenTienNoiBo> httt);
        PhieuChuyenTienNoiBo GetPhieuChuyenTienNoiBo(string id);
        List<ChiTietPhieuChuyenTienNoiBo> GetChiTietPhieuChuyenTienNoiBo(string id);
        string DeletePhieu(string id);
        List<ViewTaiKhoanTien_NganHang> GetVTKTNH();
        void SetIPClient(string ip);
        void SetHostNameClient(string host);
        DataTable XuatExcel(DateTime FromDate, DateTime ToDate ,string Query);
    }
}
