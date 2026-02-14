using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class SoThuModel
    {
        public PhieuThu Item { get; set; }
        public List<User> Users { get; set; }
        public List<ViewHinhThucThanhToan> HTTT { get; set; }
        public List<CategoryItem> MauXe { get; set; }
        public List<CategoryItem> MaXe { get; set; }
        public List<ChiTietPhieuThu> CTPT { get; set; }
        public List<DoiTac> DoiTac { get; set; }
        public ViewNoPhaiThu NPT { get; set; }
        public ViewNoDaThu NDT { get; set; }
        public CategoryItem MX { get; set; }
        public CategoryItem MAX { get; set; }
        public List<ChiTietBanTaiSan> CTBTS { get; set; }
        public List<ChiTietBanPhuKien> CTBPK { get; set; }
        public List<ViewTCOC> Coc { get; set; }
        public List<PhieuThu> CocCongNo { get; set; }
        public List<ChiTietPhieuThu> ListCongNo { get; set; }
        public List<KhoPhuTung> ListPhuTung { get; set; }
        //public List<ViewMaLoaiPhieuThu> MaLoaiPhieuThu { get; set; }
        public List<ChiTietBanPhuKien> ListBanPhuKien { get; set; }
        public ViewPhieuThu ViewPhieuThu { get; set; }
        public List<ViewCongNo> DSCongNo { get; set; }
        public List<ViewLoaiXe> MaLoaiXe { get; set; }
        public List<DropDownListItem> ListItem { get; set; }
        public List<DropDownListItem> LoaiBH { get; set; }
        public string SoKhung { get; set; }
        public List<ViewGoiBDTK> GoiBDTK { get; set; }
        //public List<ViewChiTietBaoDuongTietKiem> CTBDTK {get;set;}
        public List<ViewChiTietBDTK> CTBDTK { get; set; }
        public List<ViewTCOC> Cocs { get; set; }
        public ViewTPHKITemp ViewTPHKITemp { get; set; }
        public ViewTBAHITemp ViewTBAHITemp { get; set; }
        public List<ViewChiTietPhuKien> ViewChiTietPhuKien { get; set; }
        public ViewPhieuBDTKTamCybers ViewPhieuBDTKTamCybers { get; set; }
        public List<CategoryItem> LyDoThu { get; set; }
        public List<ChuongTrinhDichVuHang> LHTT { get; set; }
    }
}
