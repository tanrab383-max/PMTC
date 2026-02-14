using System;
using System.Collections.Generic;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Model;

namespace TNK.Services.Catalog
{
    public interface ITuiDinhKhoanService
    {
        
        List<TuiDinhKhoan> GetTuiDinhKhoanList(string loai = "");
        TuiDinhKhoan GetTuiDinhKhoan(string maTui );
        List<ViewNguonKhuyenMai> GetKhoNhapPhuTung(bool isForInsert = true);
        List<ViewNoPhaiTra> GetListNoPhaiTra(string query, DateTime from, DateTime to, int p, ref int total, int pageSize);
        List<ViewNoPhaiThu> GetListNoPhaiThu(string query, DateTime from, DateTime to, int p, ref int total, int pageSize);
        List<ViewChiTietXuatNhapKhoPhuTung> GetListChiTietXuatNhapKhoPhuTung(string query,string maKho, DateTime from, DateTime to, int p, ref int total, int pageSize);
        List<ViewPhieuChi> GetListHangTrenDuong(string query, DateTime from, DateTime to, int p, ref int total, int pageSize);
        List<PhieuThu> GetListPhieuThu(string query,string maTui, DateTime from, DateTime to, int p, ref int total, int pageSize);
        //List<PhieuThu> GetListPhieuDoanhThu(string query,string maTui, DateTime from, DateTime to, int p, ref int total, int pageSize);
        List<TNK.Core.Domain.TuiDinhKhoan> ThongKeDinhKhoan();
        List<TuiDinhKhoan> getByParent_tien(string id);
        List<TuiDinhKhoan> getByParent_vay(string id);
        List<TuiDinhKhoan> getByParent_taisan(string id);
        List<TuiDinhKhoan> getByParent_nguonvon(string id);
        TuiDinhKhoan get_tien();
        TuiDinhKhoan get_vay();
        TuiDinhKhoan get_taisan();
        TuiDinhKhoan get_nguonvon();
        string Create(TuiDinhKhoan tuiDinhKhoan);
    }
}
 