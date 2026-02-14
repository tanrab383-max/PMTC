using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ViewChiTietBaoDuongTietKiem : BaseEntity
    {
        public int Ma { get; set; }
        public string MaPhieuThu { get; set; }
        public string MaBDTK { get; set; }
        public string CapBDTK { get; set; }
        public double GiaNiemYet { get; set; }
        public double GiaBan { get; set; }
        public double GiamGia { get; set; }
        public double ThucBan { get; set; }
        public double SoTienSuDung { get; set; }
        public double ConLai { get; set; }
        public DateTime? HanSuDung { get; set; }
        public string MaPhieuSuDung { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Id_Cyber { get; set; }
    }
}
