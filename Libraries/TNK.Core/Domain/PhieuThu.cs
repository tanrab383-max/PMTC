using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class PhieuThu:BaseEntity
    {
        public PhieuThu()
        {
            MaPhieuThu = "";
            MaLoaiPhieu = "";
            SoChungTu = "";
            NgayThu = DateTime.Now;
            HoTen = "";
            DienThoai = "";
            DiaChi = "";
            BienSo = "";
            SoHopDong = "";
            MaXe = "";
            MauXe = "";
            NoiDung = "";
            CTBH = "";
            DoiTac = "";
            LoaiXe = "";
            NhaCungCap = "";
            GiaVon = 0;
            GiaBan = 0;
            ConLai = 0;
            GiamGia = 0;
            SoTienChiTra = 0;
            DVCV = "";
            SoKhung = "";
            NguoiUngTien = "";
            SoTienThu = 0;
            SoTienDaSuDung = 0;
            SoTienUng = 0;
            ConNo = 0;
            DoiTacTraLai = "";
            GhiChu = "";
            KeToanTruong = Guid.Empty;
            NguoiLapPhieu = Guid.Empty;
            NguoiLapPhieu = Guid.Empty;
            NguoiThuTien = Guid.Empty;
            TongCong = 0;
            TinhTrangPhieu = "";
            IsDeleted = false;
            IsActive = true;
            IsActive = true;
            CreatedBy = Guid.Empty;
            CreatedDate = DateTime.Now;
            UpdatedBy = Guid.Empty;
            UpdatedDate = DateTime.Now;
            SoHoaDon = "";
            YeuCauKH = "";
            NgayHachToan = DateTime.Now;
            NgayHopDong = DateTime.Now;
            TenXe = "";
            NgayQuyetToan = DateTime.Now;
            ThongTinKhac = "";
            MaPhieuLienQuan = "";
            HoaHongTaiXe = 0;
            NguoiDuyetHHTX = "";
            DotGopVon = "";
            HoaHong = 0;
            TienTraBH = 0;
            ThuHoaHongNgay = "";
            Flag = false;
            Sales = "";
            LoaiBaoHiem = "";
            NoKhuyenMai = 0;
            NhanVienTuVan = "";
            CyberId = "";
            //NgaySuDungCoc = "";
            LyDoThu = "";
        }
        public string MaPhieuThu { get; set; }
        public string MaPhieuLienQuan { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string SoChungTu { get; set; }
        public DateTime NgayThu { get; set; }
        public DateTime NgayHachToan { get; set; }
        public DateTime? NgayHopDong { get; set; }
        public DateTime NgayQuyetToan { get; set; }
        public string TenXe { get; set; }
        public string HoTen { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string BienSo { get; set; }
        public string SoHopDong { get; set; }
        public string SoHoaDon { get; set; }
        public string YeuCauKH { get; set; }
        public string SoKhung { get; set; }
        public string MaXe { get; set; }
        public string MauXe { get; set; }
        public string NoiDung { get; set; }
        public string CTBH { get; set; }
        public string DoiTac { get; set; }
        public string LoaiXe { get; set; }
        public string NhaCungCap { get; set; }
        public double SoTienThu { get; set; }
        public double SoTienChiTra { get; set; }
        public double? SoTienDaSuDung { get; set; }
        public double? GiaVon { get; set; }
        public double? GiaBan { get; set; }
        public double? GiaNiemYet { get; set; }
        public double? GiamGia { get; set; }
        public double? ConLai { get; set; }
        public string DVCV { get; set; }
        public string NguoiUngTien { get; set; }
        public double SoTienUng { get; set; }
        public double ConNo { get; set; }
        public string DoiTacTraLai { get; set; }
        public string GhiChu { get; set; }
        public string ThongTinKhac { get; set; }
        public Guid? KeToanTruong { get; set; }
        public Guid? NguoiLapPhieu { get; set; }
        public Guid? NguoiNopTien { get; set; }
        public Guid? NguoiThuTien { get; set; }
        public double TongCong { get; set; }
        public string TinhTrangPhieu { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public double? HoaHongTaiXe { get; set; }
        public string NguoiDuyetHHTX { get; set; }
        public string NguoiBaoLanh { get; set; }
        public Guid? MaTaiSan { get; set; }
        public string DotGopVon { get; set; }
        public double? HoaHong { get; set; }
        public double? TienTraBH { get; set; }
        public string ThuHoaHongNgay { get; set; }
        public bool? Flag { get; set; }
        public string Sales { get; set; }
        public string LoaiBaoHiem { get; set; }
        public double? NoKhuyenMai { get; set; }

        public string NhanVienTuVan { get; set; }

        
              public string CyberId { get; set; }
        
        /// <summary>
        /// Cac truong mo rong Ext1,Ext2,Ext3 de danh khi can them thong tin
        /// </summary>
        public string Ext1 { get; set; }
        public string Ext2 { get; set; }
        public string Ext3 { get; set; }
        //public string NgaySuDungCoc { get; set; }
        public string LyDoThu{ get; set; }
    }
}
