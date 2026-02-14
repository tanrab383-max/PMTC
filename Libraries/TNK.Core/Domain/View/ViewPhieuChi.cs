using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewPhieuChi : BaseEntity
    {
        public ViewPhieuChi()
        {
            MaPhieuChi = "";
            MaLoaiPhieu = "";
            SoChungTu = "";
            NgayChi = DateTime.Now;
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
            TenDoiTac = "";
            LoaiXe = "";
            NhaCungCap = "";
            TenNhaCungCap = "";
            GiaVon = 0;
            GiaBan = 0;
            ConLaiPhaiThu = 0;
            ConLaiPhaiTra = 0;
            DVCV = "";
            SoKhung = "";
            NguoiUngTien = "";
            SoTienChi = 0;
            SoTienUng = 0;
            GhiChu = "";
            KeToanTruong = Guid.Empty;
            NguoiLapPhieu = Guid.Empty;
            //NguoiLapPhieu = Guid.Empty;
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
            TenXe = "";
            SoKhung = "";
            SoMay = "";
            NgayQuyetToan = DateTime.Now;
            DuAn = "";
            TenDuAn = "";
            LyDoChi = "";
            TenTaiSan = "";
            NguoiDuyet = "";
            ConLai = 0;
            SoTienHoanCoc = 0;
            NgayNhapKho = DateTime.Now;
        }
        public string MaPhieuChi { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string SoChungTu { get; set; }
        public DateTime NgayChi { get; set; }
        public DateTime NgayHachToan { get; set; }
        public DateTime? NgayQuyetToan { get; set; }
        public string TenXe { get; set; }
        public string HoTen { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string BienSo { get; set; }
        public string SoHopDong { get; set; }
        public string SoHoaDon { get; set; }
        public string YeuCauKH { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string MaXe { get; set; }
        public string MauXe { get; set; }
        public string NoiDung { get; set; }
        public string CTBH { get; set; }
        public string DoiTac { get; set; }
        public string TenDoiTac { get; set; }
        public string LoaiXe { get; set; }
        public string NhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; }
        public double SoTienChi { get; set; }
        public double GiaVon { get; set; }
        public double GiaBan { get; set; }
        public double ConLaiPhaiThu { get; set; }
        public double ConLaiPhaiTra { get; set; }
        public string DVCV { get; set; }
        public string NguoiUngTien { get; set; }
        public double SoTienUng { get; set; }
        public string GhiChu { get; set; }
        public Guid? KeToanTruong { get; set; }
        public Guid? NguoiLapPhieu { get; set; }
        public Guid? NguoiNopTien { get; set; }
        public Guid? NguoiThuTien { get; set; }
        public double TongCong { get; set; }
        public string TinhTrangPhieu { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DuAn { get; set; }
        public string TenDuAn { get; set; }
        public string LyDoChi { get; set; }
        public string TenTaiSan { get; set; }
        public string NguoiDuyet { get; set; }
        public double ConLai { get; set; }
        public double? SoTienHoanCoc { get; set; }
        public DateTime NgayNhapKho { get; set; }
    }
}
