using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ChiTietKhuyenMai:BaseEntity
    {
        public ChiTietKhuyenMai()
        {
            NguonKhuyenMai = "";
        }
        public new  Guid Id { get; set; }
        public string MaPhieuThu { get; set; }
        public string LoaiKhuyenMai { get; set; }
        public string NguonKhuyenMai { get; set; }
        public string NoiDung { get; set; }
        public double GiaBan { get; set; }
        public double GiaVon { get; set; }
        public string GhiChu { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string MaPhieuNoHHBX { get; set; }
    }
}
