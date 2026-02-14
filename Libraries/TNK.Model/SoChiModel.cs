using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class SoChiModel
    {
        public PhieuChi Item { get; set; }
        public List<User> Users { get; set; }
        public List<ViewHinhThucThanhToan> HTTT { get; set; }
        public List<CategoryItem> MauXe { get; set; }
        public List<ViewLoaiXe> MaXe { get; set; }
        public List<ChiTietPhieuChi> CTPT { get; set; }
        public List<DoiTac> DoiTac { get; set; }
        public List<CategoryItem> DuAnDauTu { get; set; }
        public List<CategoryItem> LyDoChi { get; set; }
        public List<CategoryItem> PhongBanChi { get; set; }
        public List<CategoryItem> TenTaiSan { get; set; }
        public string LoaiPhieu { get; set; }
        public PhieuThu ItemPhieuThu { get; set; }
        public NoPhaiTra ItemNoPhaiTra { get; set; }
        public ViewNoPhaiTraTuPhieuThu ItemNoPhaiTraTuPhieuThu { get; set; }
        public List<ViewNoPhaiTraTuPhieuThu> ListNoPhaiTraTuPhieuThu { get; set; }
        public ViewNoPhaiTra VNPT { get; set; }
        public ViewNoDaTra VNDT { get; set; }
        /// <summary>
        /// Danh sách các kho phụ tùng 
        /// </summary>
        public List<ViewNguonKhuyenMai> KhoPhuTung { get; set; }
        /// <summary>
        /// Danh sách chi tiết nhập kho phụ tùng
        /// </summary>
        public List<ChiTietNhapKhoPhuTung> ListCTNKPT { get; set; }

        public CategoryItem MX { get; set; }
        public CategoryItem MAX { get; set; }

        public PhieuThu TCOBX { get; set; }
        public PhieuThu TCODV { get; set; }
        public List<NhanVien> GetNhanVien { get; set; }
        public List<NhanVien> GetHoTen { get; set; }
        public List<ViewChiCoc> ChiCoc { get; set; }
        public List<ChiTietPhieuChi> CTPC { get; set; }
        public List<ViewKhoTaiSan> KhoTaiSan { get; set; }
        public List<CategoryItem> PhanLoai { get; set; }
    }
}
