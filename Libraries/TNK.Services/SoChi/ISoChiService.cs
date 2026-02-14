using System;
using System.Collections.Generic;
using System.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Model;

namespace TNK.Services.SoChi
{
    public interface ISoChiService
    {
        string CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, bool isGoiXuLySauKhiInsertPhieuChi = true, string giaTriTrenGiaoDien = "");
        string CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt,List<ChiTietPhieuChi> CTPC, bool isGoiXuLySauKhiInsertPhieuChi = true, string giaTriTrenGiaoDien = "");
        string CreatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, List<ChiTietNhapKhoPhuTung> ListCTNKPT,List<ChiTietPhieuChi> CTPC,bool isGoiXuLySauKhiInsertPhieuChi = true, string giaTriTrenGiaoDien = "");
        string CreatePhieuChi_ImportCMPT(PhieuChi model, List<ChiTietPhieuChi> httt, List<ChiTietNhapKhoPhuTung> ListCTNKPT, List<ChiTietPhieuChi> CTPC, bool isGoiXuLySauKhiInsertPhieuChi = true, string giaTriTrenGiaoDien = "");
        string UpdatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, ref string sessionId, bool isGoiXuLySauKhiUpdatePhieuChi = true, string giaTriTrenGiaoDien = "");

        string UpdatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt,List<ChiTietPhieuChi> CTPC, ref string sessionId, bool isGoiXuLySauKhiUpdatePhieuChi = true,  string giaTriTrenGiaoDien = "");

        string UpdatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, List<ChiTietNhapKhoPhuTung> ListCTNKPT, List<ChiTietPhieuChi> CTPC, ref string sessionId, bool isGoiXuLySauKhiUpdatePhieuChi = true, string giaTriTrenGiaoDien = "");
        string UpdatePhieuChi(PhieuChi model, List<ChiTietPhieuChi> httt, NoPhaiTra noPhaiTra, List<ChiTietPhieuChi> CTPC, ref string sessionId, bool isGoiXuLySauKhiUpdatePhieuChi = true, string giaTriTrenGiaoDien = "");
        string DeletePhieuChi(string id, bool isGoiXuLySauKhiUdatePhieuChi = true);
        string DeletePhieuThu(string id, bool isGoiXuLySauKhiUdatePhieuThu = true);
        ViewNoDaTra GetPhieuChiCompleteById(string id);
        List<ViewNoDaTra> GetPhieuChiComplete(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        PhieuChi GetPhieuChi(string id);
        List<DoiTac> GetDoiTac(string id);
        List<DoiTac> GetDoiTac_VM(string id);
        List<CategoryItem> GetDuAnDauTu(string id);
        List<CategoryItem> GetLyDoChi(string id);
        List<CategoryItem> GetTenTaiSan(string id);
        List<ViewHinhThucThanhToan> GetHTTT();
        List<ChiTietPhieuChi> GetCTPC(string id);
        List<ViewCHCPT> GetCHCPT(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewNoPhaiTraTuPhieuThu> GetNoPhaiTraTuPhieuThuList(string loaiPhieuNo, DateTime from, DateTime to);
        ViewNoPhaiTraTuPhieuThu GetNoPhaiTraTuPhieuThu(string maPhieuNo);
        string CreateId(string mlp);

        List<NoPhaiThu> GetNoPhaiThu(string id);
        List<NoPhaiTra> GetNoPhaiTra(string id);

        List<ViewPhieuChi> GetCMHTD();
        List<PhieuChi> GetPhieuChiList(string loaiPhieu, DateTime from, DateTime to);
        List<ViewCOCDT> GetCOCDT(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewNoPhaiTra> GetNPTs(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        ViewNoPhaiTra GetNPT(string id);
        ViewNoDaTra GetNoDaTra(string id);
        ViewNoDaTra GetNoDaTra_ThuHo(string id);
        List<ViewNoDaTra> GetNoDaTra(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);

      
        List<ViewPhieuChi> GetPhieuChiList(string loaiPhieu, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewPhieuChi> ListThuHoanCoc(string loaiPhieu, DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string maloaiphieu);
        //ListThuHoanCocComplete
        List<PhieuThu> ListThuHoanCocComplete(string loaiPhieu, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewPhieuChi> GetPhieuChiList(string loaiPhieu, DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang);
        List<ViewCTAUN> GetCTAUNList(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang);


        List<ChiTietNhapKhoPhuTung> GetCTNKPT(string id);
        bool UpdateCTNKPT(ChiTietNhapKhoPhuTung model);

        bool UpdateCTNKPT(List<ChiTietNhapKhoPhuTung> lst);
       
        bool InsertCTNKPT(ChiTietNhapKhoPhuTung info);
        string XuLySauKhiInsertPhieuChi(string maPhieuChi,string sessionId, string giaTriTrenGiaoDien = "");
        List<ViewListPhieuChi> GetListPhieuChi(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string maLoaiPhieu,string chonngay);
        List<TNK.Core.Domain.NhanVien> GetNhanVien();
        //bool DeleteHTD(string id);
        //phan truc them 
        List<ViewNoPhaiTra> GetNPTs1(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang);
        void SetClientBrowser(string browser);
        void SetIPClient(string ip);
        void SetHostNameClient(string host);
        string CreatePhieuChi(PhieuChi PhieuChi);
        List<ViewChiCoc> GetChiCoc();
        List<PhieuChi> GetChiCocMXTT(string maPhieuChi, string NCC);
        List<PhieuChi> GetChiCocMPTU(string maPhieuChi, string NCC);
        List<PhieuChi> GetChiCocMUTS(string maPhieuChi, string NCC);
        List<PhieuChi> GetChiCocGCN(string maPhieuChi, string NCC);
        List<PhieuChi> GetChiCocTH(string maPhieuChi, string LDC);
        List<ChiTietPhieuChi> getListCTPC(string id);
        List<PhieuChi> GetListPhieuChi(DateTime from, DateTime to, int p, ref int total, int pageSize, string query, string maLoaiPhieu);
        bool Update_SoHoaDon(string maPhieuChi, string soHoaDon);
        DataTable XuatExcel(DateTime FromDate, DateTime ToDate,string MaLoaiPhieu);
        List<DanhMucDienGiai> LoaiPhieuChi();
        double TongSoTienChi(string MaPhieuChi);
        List<ViewNoDaTra> GetPhieuChiComplete_NTHBH(string id, DateTime from, DateTime to, string query, int p,ref int total, int pageSize);
        //tim ma phieu nơ
        ViewNoDaTra getNoDaTraById(string id);
        //tìm ra các phiêu nợ trả nợ sau
        string getPhieuTraNo(ViewNoDaTra VNDT);
        string CheckNCC_CMPT_Import(string TenNCC);
        bool CheckKhoNhap(string KhoNhap);
        void CreateRoleInmenu(Guid id);        

        bool CreatePhieuChiKHDT(PhieuChi model, List<CHI_TIET_KHAU_HAO> lstCTKH, Guid UserId);
        bool CreatePhieuChiKHTS(PhieuChi model, List<ViewKhoTaiSan> TS, Guid UserID);
        string GetPhieuChiNo(string MaPhieuNo);
        List<ViewCKHHA> GetCKHDT(DateTime from, DateTime to, int p, ref int total, int pageSize, string query);
        double GetTongTienKhauHao(string type, string MaDuAn);
        List<ViewCKHHA> GetCKHTS(DateTime from, DateTime to, int p, ref int total, int pageSize, string query);
        List<ViewKhoTaiSan> GetListCTKH(string maphieuchi);
        List<ViewChiTietKhauHao> GetViewCTKH(string maphieuchi, string maloaiphieu);
        bool DeletedCTKH(string maphieuchi);
        List<LichSuChiNo> GetListLichSuChiNo(string MaPhieuNo);
        List<CategoryItem> GetDuAnDauTuKhauHau(string id);
        string ImportNoGCN(string soQuyetToan, string tenCongViec, double soTien);
        string ImportNoHHTXE(string soQuyetToan, double soTien, string nguoiDuyet);
        List<ViewCHCNO> GetCHCNO(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewChiMuaPTTemp> GetListCMPTUTemp(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        DataTableCollection GetListCMPTUChiTietTemp(string ConnectionString, string So_ct);
        /// <summary>
        /// SoChungTu = ma_hs + "-" + so_ct trong Cyber
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="soChungTu"></param>
        /// <returns></returns>
        DataTableCollection GetListCMPTUChoDongBoCyber(string ConnectionString, string soChungTu);

        List<ViewDanhSachPhieuNhapKhoPhuTung> GetListChiMuaPhuTung(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
    }
}
 