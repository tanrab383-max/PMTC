using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace TNK.Core.Domain.View
{ 
    public class ViewPhieuBDTKTamCybers : BaseEntity
    {
        public string DonVi { get; set; }

        public string SoChungTu { get; set; }

        public string Stt_rec { get; set; }

        public DateTime NgayThu { get; set; }

        public string NguoiThu { get; set; }

        public DateTime? ThoiHanSuDung { get; set; }

        public double? GiaBan { get; set; }

        public double? GiamGia { get; set; }

        public double? TongCong { get; set; }

        public string HoTen { get; set; }

        public string SoKhung { get; set; }

        public string BienSoXe { get; set; }

        public string Loai { get; set; }
    }
}
