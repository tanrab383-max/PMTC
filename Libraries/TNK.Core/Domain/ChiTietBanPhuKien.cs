using System;
using System.ComponentModel.DataAnnotations;

namespace TNK.Core.Domain
{
    public partial class ChiTietBanPhuKien : BaseEntity
    {
        public  int Ma { get; set; }
        public string MaPhieuThu { get; set; }
        //[Display(Name ="TenHang")]
		public string TenHang { get; set; }
		//[Display(Name ="NhaCungCap")]
		public string NhaCungCap { get; set; }
		//[Display(Name ="GiaVon")]
		public double GiaVon { get; set; }
		//[Display(Name ="GiaBan")]
		public double GiaBan { get; set; }
        public double GiamGia { get; set; }
        public double ThucThu { get; set; }
        public double TienCong { get; set; }
        public string MaKho { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public double SoLuong { get; set; }
    }
}
