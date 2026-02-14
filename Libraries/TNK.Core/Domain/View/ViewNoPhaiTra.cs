using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewNoPhaiTra:BaseEntity
    {
        public string MaPhieuNo { get; set; }       
        public DateTime NgayNo { get; set; }
        public string LoaiPhieuNo { get; set; }
        public string LyDoNo { get; set; }
        public string GhiChu { get; set; }
        public string ChungTuThu { get; set; }
        public string SoPhieuQuyetToan { get; set; }
        public string BienSo { get; set; }
        public string KhachHang { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string SoHopDong { get; set; }
        public DateTime NgayHopDong { get; set; }
        public string NguoiBaoLanh { get; set; }
        public double SoTienNo { get; set; }
        public double SoTienDaTra { get; set; }
        public double SoTienConLai { get; set; }
        public string ThongTinDonViNo { get; set; }
        public string DonViNo { get; set; }
        public string TinhTrang { get; set; }
        public string HSD { get; set; }
        public string HanSuDung { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MaPhieuPhatSinh { get; set; }
        public double? HoaHong { get; set; }
        public string DoiTac { get; set; }
        //public string SoChungTu { get; set; }
        public string Hoten { get; set; }
        public string TenDoiTac { get; set; }
        public DateTime NgayHachToan { get; set; }
        public DateTime? NgayChi { get; set; }
        public string ThongTinKhac { get; set; }
        public string TenTui { get; set; }
        public string LyDoChi { get; set; }
        public double? GiamGia { get; set; }
        public string TinhTrangQT { get; set; }
    }
}
