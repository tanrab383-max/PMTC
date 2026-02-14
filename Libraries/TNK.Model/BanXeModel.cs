using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class BanXeModel
    {
        public PhieuThu obj { get; set; }
        public List<User> Users { get; set; }
        public List<ViewTCOC> Cocs { get; set; }
        public List<ChiTietPhieuThu> CTPT { get; set; }
        public List<ViewHinhThucThanhToan> HTTT { get; set; }
        public List<DoiTac> NganHang { get; set; }
        public List<NoPhaiThu> KVNH { get; set; }
        public NoPhaiThu noPhaiThu { get; set; }
        public NoPhaiTra noHHBX { get; set; }
        public List<ViewNguonKhuyenMai> NKM { get; set; }
        public List<ViewChiTietKhuyenMai> CTKM { get; set; }     
        public List<ChiTietPhieuThu> ListCTPT { get; set; }
        public List<PhieuThu> ListKMBaoHiem { get; set; }
        public List<ChiTietBanPhuKien> ListKMPKien { get; set; }
        public List<ViewTBDTK> ListBDTK { get; set; }
        public List<DoiTac> DoiTac { get; set; }
        /// <summary>
        /// danh sach cac no khac lien quan chiet khau thuong mai cua hang
        /// </summary>
        public List<ViewNoPhaiThu> ListChietKhauHang { get; set; }
        public List<DropDownListItem> LoaiChietKhau { get; set; }
    }
}
