using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewDanhSachPhieuNhapKhoPhuTung : BaseEntity
    {
        public ViewDanhSachPhieuNhapKhoPhuTung()
        {
            MaPhieuChi = "";           
            SoChungTu = "";
            NgayChi = DateTime.Now;          
            NoiDung = "";
            TenDoiTac = "";
            DoiTac = "";           
            ConLaiPhaiTra = 0;
            SoTienChi = 0;
            GhiChu = "";
            TongCong = 0;
            NgayNhapKho = DateTime.Now;
            MaKho = "";
        }
        public string MaPhieuChi { get; set; }
        public string SoChungTu { get; set; }
        public DateTime NgayChi { get; set; }
        public string NoiDung { get; set; }
        public string DoiTac { get; set; }
        public string TenDoiTac { get; set; }
        public double SoTienChi { get; set; }
        public double ConLaiPhaiTra { get; set; }
        public string GhiChu { get; set; }
        public double TongCong { get; set; }
        public DateTime NgayNhapKho { get; set; }
        public string MaKho { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
