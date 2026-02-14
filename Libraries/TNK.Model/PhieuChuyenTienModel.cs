using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class PhieuChuyenTienModel
    {
        public PhieuChuyenTienModel()
        {
            Item = new PhieuChuyenTienNoiBo();
            HTTT = new List<ViewHinhThucThanhToan>();
            Users = new List<User>();
            CTPT = new List<ChiTietPhieuChuyenTienNoiBo>();
        }
        public PhieuChuyenTienNoiBo Item { get; set; }
        public List<ViewHinhThucThanhToan> HTTT { get; set; }
        public List<User> Users { get; set; }
        public List<ChiTietPhieuChuyenTienNoiBo> CTPT { get; set; }
        public List<ViewTaiKhoanTien_NganHang> VTKTNH { get; set; }
    }
}
