using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Model;

namespace TNK.Services.SoThu
{
    public interface IPhieuThuService
    {
        List<ViewTKHAC> GetTKHAC(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<PhieuThu> GetKMTMV(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTCOBX> GetTCOBX(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTCOBX> GetTCOBX(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang);
        List<ViewTBAHI> GetTBAHI(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTBAHI> GetTBAHITab(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tab);


        List<ViewTCODV> GetTCODV(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang);
        List<ViewTCODV> GetTCODV(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTHHHB> GetTHHHB(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTVAMU> GetTVAMU(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTLADT> GetTLADT(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTBAXE> GetTBAXE(DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang);
        List<ViewTPHKI> GetTPHKI(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTPHKI> GetTPHKITab(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTPHKITemp> GetTPHKITabTemp(DateTime from, DateTime to, string query, string typeTicketTemp, int p, ref int total, int pageSize, ref List<ViewTPHKITemp> listViewFull, string loai);
        List<ViewNKMPK> GetTNKMPK(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewPhieuThu> GetBDTK(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string type);
        List<ViewTBATS> GetTBATS(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewNoDaThu> GetNBHDV(string id,DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        ViewNoDaThu GetNBHDVById(string id);
        PhieuThu GetPhieuThuBySHD(string shd);
        PhieuThu GetPhieuThuTBAXEBySHD(string shd);
        PhieuThu GetPhieuThuTDIVUBySoDichVu(string soDichVu);
        List<ViewNoPhaiThu> GetNoPhaiThu();
        List<ViewNoPhaiThu> GetNoPhaiThus(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewNoPhaiThu> GetNoPhaiThus(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize,string tinhtrang);
        List<ViewNoPhaiThuBHDV> GetNoPhaiThuBHDVs(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewNoPhaiThuBHDV> GetNoPhaiThuBHDVs(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang, string loaiBH);
        List<ViewNoPhaiThuBHDV> GetNoPhaiThuBHDVs_GiaiTrinh(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang,string dagiaitrinh);
        List<ViewNoDaThu> GetNoDaThus(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        ViewNoDaThu GetNoDaThu(string id);
        ViewNoPhaiThu GetNoPhaiThu(string id);
        List<ViewTCOBX> GetTCOBXBySHD(string shd, bool isForInsert);
        List<ViewTCODV> GetTCODVByBienSo(string bienSoXe, bool isForInsert, string maPhieuThu = "");
        List<ViewTDIVU> GetTDIVU(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<PhieuDVTamCyber> GetPDVT(DateTime from, DateTime to, int p, ref int total, int pageSize, string query);
        DataTable GetPDVT_Datable(DateTime from, DateTime to, string query);
        PhieuDVTamCyber GetPDVTCybers(string id);
        List<CTPDVTamCyber> GetCTPDVTCybers(string id);
        DataTable GetListCTPDVTCybers(DateTime from, DateTime to, string query);
        List<REPAIR_ORDER_PYS> GetPDVT(string id);
        ViewTBAXE GetTBAXE(string id);
        List<ViewTBAXE> GetTBAXEBySoHopDong(string soHopDong);
        List<ViewPhieuThu> GetListPhieuThu(DateTime from, DateTime to, int p, ref int total, int pageSize, string query,string maLoaiPhieu);
        double GetTyLeGiaVonKhuyenMai();

        string DeletePhieuThu(string id);
        string UpdateSHDTCOBX(string id,string shd);
        string DeletePhieuThu(string id, string type);
        string UpdatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt, ref string sessionId, bool isXuLySauKhiUpdate = true,string giaTriTrenGiaoDien = "");
        string UpdatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt,List<ChiTietBanPhuKien> LPT,string type, ref string sessionId, bool isXuLySauKhiUpdate = true, string giaTriTrenGiaoDien = "");
        string CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt,bool isGoiXuLySauKhiInsertPhieuThu = true, string giaTriTrenGiaoDien = "");
        string CreatePhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt, List<ChiTietBanPhuKien> LPT, string type, bool isGoiXuLySauKhiInsertPhieuThu = true, string giaTriTrenGiaoDien = "");

        bool CreatePhieuThu(PhieuThu model);
        PhieuThu GetPhieuThu(string id);
        ViewPhieuThu GetViewPhieuThu(string id, string type);
        bool ChangeTreoTien(Guid id, bool check);
        bool InsertCTPT(ChiTietPhieuThu info);
        bool UpdateCTPT(List<ChiTietPhieuThu> lst,PhieuThu obj);
        bool UpdateCTPT(ChiTietPhieuThu model,PhieuThu obj);
        List<ChiTietPhieuThu> GetCTPT(string id);
        List<ChiTietPhieuThu> GetCTPT_BHDV(string id);
        List<ChiTietPhieuThu> GetCTPT_CongNo(string id);
        List<ChiTietPhieuThu> GetCTPTByBienSo(string bienso);
        bool CreateChiTietPhieuThu(string maPhieuThu, List<ChiTietPhieuThu> httt);
        string XuLySauKhiInsertPhieuThu(string maPhieuThu,string giaTriTrenGiaoDien ="", bool isXuLyTinhToanTuiTien = true);

        string XuLySauKhiUpdatePhieuThu(string maPhieuThu,string sessionId ="",string giaTriTrenGiaoDien ="",string action ="UPDATE");

        bool UpdateCTBTS(ChiTietBanTaiSan ctbts);
        bool InsertCTBTS(ChiTietBanTaiSan ctbts);
        ChiTietBanTaiSan GetCTBTS(Guid id);
        List<ChiTietBanTaiSan> GetCTBTS(string id);
        bool ChangeTypeTCOBX(string id, bool type);
        //List<ViewTCOC> GetTCOC(string maSo, bool isForInsert);//GetTCOC_New
        List<ViewTCOC> GetTCOC(string maPhieuThu,string maSo,string HoTen, bool isForInsert);
        List<ViewTCOC> GetTCOC_V2(string maPhieuThu, string bienSo, string soHopDong, string hoTen, bool isForInsert);
        List<PhieuThu> GetKMBaoHiem(string soHopDong);
        // List<PhieuThu> GetKMPKien(string soHopDong);
        List<ChiTietBanPhuKien> GetKMPKien(string soHopDong);

        List<ViewNoPhaiThu> GetNoChietKhauThuongMai(string soKhung);
        List<ViewTBDTK> GetBDTK(string soHopDong);
        double GetNKMPK(string soHopDong);

        string KiemTraCocTruocKhiUpdatePhieuThu(List<ChiTietPhieuThu> lstCoc, string maPhieuThu);
        string KiemTraRangBuocTruocKhiUpdatePhieuThuBanXe(PhieuThu phieuThu, NoPhaiThu noBanXe, NoPhaiThu noNganHang, List<ChiTietPhieuThu> lstCoc,List<ChiTietKhuyenMai> CTKM);
        string KiemTraRangBuocTruocKhiUpdatePhieuThuDichVu(PhieuThu phieuThu, NoPhaiThu noDichVu, NoPhaiThu noBaoHiem, List<ChiTietPhieuThu> lstCoc, NoPhaiTra noHHTX, NoPhaiTra noGCN);
        /// <summary>
        /// Cap nhat no hoa hong tai xe cho phieu dich vu
        /// Khong cap nhat các giá trị khác, chỉ cập nhật: HoaHongTaiXe,NguoiDuyetHHTX,UpdatedBy,UpdatedDate
        /// </summary>
        /// <param name="maPhieuThu"></param>
        /// <param name="nguoiDuyetHHTX"></param>
        /// <param name="soTienHHTX"></param>
        /// <returns></returns>
        string UpdatePhieuDichVuChoHoaHongTaiXe(string maPhieuThu, string nguoiDuyetHHTX, double? soTienHHTX);
        string LayNgayUpdateCuaCacPhieuNo(string lstMaPhieuNo);
        void SetClientBrowser(string browser);
        void SetIPClient(string ip);
        void SetHostNameClient(string host);
        //string getMaPhieuPhatSinh(string MaPhieuNo);
        List<ViewCCOMX> ListCocMuaXe(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<DanhMucDienGiai> ListLoaiPhieu(string ID_Loai);
        bool Update_SoHoaDon(string maPhieuThu, string soHoaDon, string soChungTu);
        bool Update_SoHoaDonNgayGhiNhanCongNo(string sMaPhieuNo, string sSoHoaDon, string sNgayXuatHoaDon, string sNgayGhiNhanCongNo, string sUser,string ghiChu);
        bool Update_GiaiTrinh(string sMaPhieuNo, string giaiTrinh,string sUser,string lyDoQuaHan);
        bool Update_GhiChuPhieuThu(string sMaPhieuNo, string ghiChu, Guid sUser);
        bool Update_SalesNote(string MaPhieuNo, string Sales, string GhiChu);
        bool Update_SHDByMaPhieuNo(string MaPhieuNo, string SoHoaDon);
        List<PhieuThu> getPhieuThuCuoi(string soHopDong);
        List<PhieuThu> getPhieuThuBSXCuoi(string bsx);
        List<ViewHinhThucThanhToan> ThuHoaHongNgay();
        double TotalPT();
        double TotalMT();
        #region phần xử lý kiểm tra trước khi insert phiếu thu
        string XuLyTruocKhiInsertPhieuThu(PhieuThu model, List<ChiTietPhieuThu> httt, ref string sessionId, bool isXuLyTruocKhiInsert = true, string giaTriTrenGiaoDien = "");
        #endregion
        #region phan xua excel
        DataTable XuatExcel(DateTime FromDate, DateTime ToDate,string MaLoaiPhieu,string Query,string loai ="");
        DataTable XuatExcelReceipted(DateTime FromDate, DateTime ToDate, string MaLoaiPhieu, string Query);
        DataTable XuatExcel_TCOBX(DateTime FromDate, DateTime ToDate, string Query);
        DataTable XuatExcel_NTKHA(DateTime FromDate, DateTime ToDate, string Query);
        DataTable XuatExcel_NHANG(DateTime FromDate, DateTime ToDate, string Query);        
        DataTable XuatExcel_NBHDV(DateTime FromDate, DateTime ToDate, string Query, string tinhtrang = "");
        DataTable XuatExcel_NDIVU(DateTime FromDate, DateTime ToDate, string Query, string TinhTrang);
        List<ViewTCODV> getLoadBienSo(string BienSo);
        List<DanhMucDienGiai> LoaiPhieuThu();
        bool SaveTienTra(Guid id, string ngaytientra, string maPhieuThu);
        //phan luu chi tiet ban phu tung
        string CreateCTBanPhuKien(string MaPhieuThu,List<ChiTietBanPhuKien> list,PhieuThu item, string type);
        string CreateCTBanPhuKien(string MaPhieuThu, List<ChiTietBanPhuKien> list, string type);
        DataTable GetListCTBPK(string MaPhieuThu);
        DataTable GetListCTBPKKM(string MaPhieuThu);
        List<ViewGoiBDTK> GetListGBDTK(bool isForInsert);
        string EditCTBanPhuKien(string MaPhieuThu, List<ChiTietBanPhuKien> list, PhieuThu item, string type);
        #endregion

        #region
        List<ViewPhieuThuCNO> ListThuCongNo(DateTime from, DateTime to, int p, ref int total, int pageSize, string tinhtrang);
        #endregion
        List<PhieuThu> GetCongNo(string maPhieuThu, string NCC);
        List<PhieuThu> ListCongNo(string maPhieuThu,string donViNo, bool isForInsert);
        string CreatePhieuThu_ThuNo(PhieuThu model, List<ChiTietPhieuThu> httt
            , List<ChiTietPhieuThu> Coc
            , bool isGoiXuLySauKhiInsertPhieuThu = true
            , string giaTriTrenGiaoDien = ""
            );
        List<ViewCongNo> ListCongNo(string MaPhieuThu);
        bool KiemTraTonTaiChungTu(string SoChungTu);
        bool KiemTraSoQuyetToan(string SoQuyetToan);
        bool KiemTraTonTaiSoQuyetToan(string SoQuyetToan);
        bool CheckSoKhung(string SoKhung);
        bool CheckMauXe(string MauXe,string SoKhung);
        bool CheckMaXe(string MaXe,string SoKhung);
        ViewKhoXe GetXe(string SoKhung);
        PhieuThu PT_BanXe(string MaPhieuThu);
        List<ViewGiaoDichCoc> ChiTietDungCocXe(string MaPhieuThu);
        List<ViewTBHXH> getListThuBHXH(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);

        List<ViewTBHXH> getListThuGTXe(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ViewTBHXH> getListThuHoKHAC(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);

        List<ViewPhieuThu> GetThuHo(string id,DateTime from, DateTime to, string query, int p, ref int total, int pageSize);

        List<ViewNoPhaiThu> GetNoPhaiThuHo(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang);
        List<ViewNoDaThu> GetNoDaThuHo(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        ViewTBAXE GetCocXe(string soHopDong);

        double TienThucGop();
        PhieuThu CongNoHonDa(string MaPhieuThu);
        string PhieuTKHACByCongNo(string MaPhieuThu);

        DataTable KiemTraThongTinNhapSoHoaDonCongNoBHDV(string lstMaPhieuNo);
        DataTable XuatExcel_GiaiTrinhNBHDV(DateTime FromDate, DateTime ToDate, string Query, string TinhTrang,string dagiaitrinh);
        List<ViewChiTietBaoDuongTietKiem> GetNBDTK(string bienSoXe, string soKhung, bool isForInsert, string maPhieuThu = "");
        double GetNoKMConLai(string MaPhieuNo);
        double LayGiaVonBDTK(string ma);
        bool CheckPhieuThuHHGP(string MaPhieuBanXe);
        bool CheckChiNoKMPK(string id);
        List<ViewNoPhaiThuBHDV> GetListHDXNCN(DateTime from, DateTime to, string query, string type, int p, ref int total, int pageSize, string tinhtrang);
        List<ViewNoPhaiThuBHDV> GetNoPhaiThu_GiaiTrinh(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang, string dagiaitrinh);
        DataTable XuatExcel_CapNhatHoaDonCongNo(DateTime FromDate, DateTime ToDate, string Type, string Query, string TinhTrang);
        DataTable XuatExcel_GiaiTrinh(DateTime FromDate, DateTime ToDate, string Query, string TinhTrang, string dagiaitrinh);

        List<ViewChiTietBaoDuongTietKiem> GetListCTBDTK(string MaPhieuThu);
        string CreatePhieuThuBDTK(PhieuThu model, List<ChiTietPhieuThu> httt, List<ChiTietBaoDuongTietKiem> CTBDTK, bool isGoiXuLySauKhiInsertPhieuThu = true, string giaTriTrenGiaoDien = "");
        string CreateCTBaoDuongTietKiem(string MaPhieuThu, List<ChiTietBaoDuongTietKiem> list);
        string UpdatePhieuThuBDTK(PhieuThu model, List<ChiTietPhieuThu> httt, List<ChiTietBaoDuongTietKiem> CTBDTK, ref string sessionId, bool isXuLySauKhiUpdate = true, string giaTriTrenGiaoDien = "");
        string EditCTBDTK(string MaPhieuThu, List<ChiTietBaoDuongTietKiem> list);
        string UpdateBDTK(List<ChiTietBaoDuongTietKiem> lstBDTK, string MaPhieuThu);
        string CheckThuNoHHGP(string MaPhieuNo, DateTime NgayNo);
        string Update_SoTienNo_NoHHGP(string MaPhieuNo, DateTime NgayNo, double SoTienNo);
        string Update_Insert_NoHHGP(string MaPhieuThuBX, double SoTienNo);
        string Update_CKTM(string SoKhung, double SoTienChietKhau, string TenChuongTrinh);
        string UpdatePhieuThuHH(PhieuThu model);
        string UpdatePhieuThuBH(string xmlPhieuThu);
        string UpdatePhieuThuSK(string xmlPhieuThu);
        double GetCNTT(string SoKhung);
        List<ViewTCOC> GetTCOC_V2(string maPhieuThu, string bienSo, string soHopDong, string hoTen, bool isForInsert, string type = "ALL");
        List<ViewChiTietPhuKien> GetChiTietPhuKien(string MaXe, string Type, string isMultiTicket);
        List<ViewTBAHITemp> GetTBAHITabTemp(DateTime from, DateTime to, string query, string typeTicketTemp, int p, ref int total, int pageSize, ref List<ViewTBAHITemp> listViewFull);
        List<ViewPhieuBDTKTamCybers> GetBDTKCyber(DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string type);
        List<ViewChiTietBDTK> GetChiTietThuBDTKTemp(string SoChungTu, string Type);
        List<ViewChiTietBDTK> GetListCTBDTK_v2(string MaPhieuThu);

        string TaoCongNoChietKhauThuongMai(PhieuThu objPhieuThu, string loaiNo, string lyDoThu, string noiDung, float soTienNo,string maDoiTacChietKhauThuongMai);
        ViewNoPhaiThu LayCongNoChietKhauThuongMai(PhieuThu objPhieuThuBanXe);

        List<ViewPhieuThuCyber> GetListViewPhieuThuCyber(DateTime from, DateTime to, int p, ref int total, int pageSize, string query, string type);
        string UpdatePhieuThuNoKhuyenMai(PhieuThu model);
        List<ViewChiTietBaoDuongTietKiem> GetNBDTK_Cyber(string soRO);
    }
}
