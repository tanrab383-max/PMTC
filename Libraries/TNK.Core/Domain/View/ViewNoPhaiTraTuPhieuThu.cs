using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewNoPhaiTraTuPhieuThu : BaseEntity
    {
        public ViewNoPhaiTraTuPhieuThu()
        {

            MaPhieuNo = "";
            LoaiPhieuNo = "";
            MaPhieuPhatSinh = "";
            SoTienNo = 0;
            SoTienDaTra = 0;
            SoTienConLai = 0;
            NguoiBaoLanh = "";
            NguoiPheDuyet = "";
            ThongTinDoiTac = "";
            GhiChu = "";
            IsActive = true;
            IsDeleted = false;
            CreatedBy = Guid.Empty;
            CreatedDate = DateTime.Now;
            UpdatedBy = Guid.Empty;
            UpdatedDate = DateTime.Now;
            BienSo = "";
            Hoten = "";
            DoiTac = "";
            DienThoai = "";
            DiaChi = "";
            MaXe = "";
            ChungTuPhieuThu = "";
            LoaiXe = "";
            MaLoaiPhieu = "";
            NgayThu = DateTime.Now;
            
        }
        public string MaPhieuNo { get; set; }
        public string LoaiPhieuNo { get; set; }
        public string MaPhieuPhatSinh { get; set; }
        public DateTime NgayThu { get; set; }
        public double SoTienNo { get; set; }
        public double SoTienDaTra { get; set; }
        public double SoTienConLai { get; set; }
        public string NguoiBaoLanh { get; set; }
        public string NguoiPheDuyet { get; set; }
        public string ThongTinDoiTac { get; set; }
        public string GhiChu { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string BienSo { get; set; }
        public string Hoten { get; set; }
        public string DoiTac { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string MaXe { get; set; }
        public string ChungTuPhieuThu { get; set; }
        public string LoaiXe { get; set; }
        public string MaLoaiPhieu { get; set; }
    }
}
