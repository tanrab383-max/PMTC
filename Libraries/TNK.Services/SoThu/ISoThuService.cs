using System;
using System.Collections.Generic;
using System.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Model;

namespace TNK.Services.SoThu
{
    public interface ISoThuService
    {
        List<DoiTac> GetDoiTac(string id);
        List<ViewHinhThucThanhToan> GetHTTT();     
        bool InsertChiTietPhieuDichVu(List<ChiTietPhieuDichVu> model, string id);
        bool InsertChiTietPhieuDichVu_Import(List<ChiTietPhieuDichVu> model, string id);
        bool UpdateChiTietPhieuDichVu(List<ChiTietPhieuDichVu> model);
        List<ChiTietPhieuDichVu> GetCTPDV(string id);
        string XuLySauKhiInsertPhieuThu(string maPhieuThu,string giaTriTrenGiaoDien ="",string ipClient="",string hostNameClient ="");
        bool UpdateChiTietPhieuDichVu_New(List<ChiTietPhieuDichVu> model,string maPhieuThu);
        List<KhoPhuTung> ListKPT();
        //double TongTienHoanCoc(string MaPhieuThu);
        List<ViewLoaiXe> ListLoaiXe();
        bool UpdateChiTietPhieuDichVu_v1(List<ChiTietPhieuDichVu> model, string id);
    }
}
 