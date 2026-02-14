using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Services.KhoXe
{
    public interface IKhoXeService
    {
        List<ViewKhoXe> GetDanhSachXe();
        List<ViewTuiHangTrenDuong> GetDanhSachKhoHangTrenDuong();
        ViewTuiHangTrenDuong GetTuiHangTrenDuong(string maNCC);
        string NhapKhoXe(TNK.Core.Domain.KhoXe item, PhieuNhapKho phieuNhapKho);
        bool CheckExists(string soKhung);
        bool Update(TNK.Core.Domain.KhoXe objKhoXe);
        string Delete(Guid id);
        List<ViewKhoXe> GetDanhSachXe(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string TinhTrang, string chonngay);
        DataTable GetDanhSachPhieuNhap(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string TinhTrang);
        List<ViewThongKeKho> ListDanhSach(DateTime from,DateTime to);
        void SetIPClient(string ip);
        void SetHostNameClient(string host);
        double SoTienConLai();
        string NhapTheChap(LichSuTheChapXe Data);
        List<LichSuTheChapXe> List(string SoKhung);
        string EditTheChap(LichSuTheChapXe Data);
        string DeleteTheChap(LichSuTheChapXe Data);
        DataTable GetListDanhSachXe(DateTime from, DateTime to, string chonngay);
        DataTable GetListDanhSachXeTon(DateTime to);
        List<ViewKhoXe> GetDanhSachXeTon(DateTime to, string query, int p, ref int total, int pageSize);
        TNK.Core.Domain.KhoXe GetXebySoKhung(string SoKhung);
    }
}
