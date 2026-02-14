using System.Collections.Generic;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class DichVuEditModel
    {
        public List<ChiTietPhieuDichVu> CTPDV { get; set; }
        public PhieuThu obj { get; set; }
        public List<NoPhaiThu> noPhaiThu { get; set; }        
        public List<ChiTietPhieuThu> CTPT { get; set; }
        public List<ViewTCODV> Coc { get; set; }
        public List<NoPhaiTra> noPhaiTra { get; set; }
        public List<DoiTac> CTBH { get; set; }
        public List<User> Users { get; set; }
        public List<ViewHinhThucThanhToan> HTTT { get; set; }
        /// <summary>
        /// Nợ công ty bảo hiểm 
        /// </summary>
        public List<NoPhaiThu> noBaoHiem { get; set; }
        /// <summary>
        /// Nợ gia công ngoài
        /// </summary>
        public List<NoPhaiTra> noGCN { get; set; }
        public List<ViewChiTietBaoDuongTietKiem> NBDTK { get; set; }
        /// <summary>
        /// Doi tac lam dich vu. Vi du doi tac noi bo, Honda,...
        /// </summary>
        public List<DoiTac> DoiTac { get; set; }
        public List<ChuongTrinhDichVuHang> LHTT { get; set; }
        public List<NoPhaiThu> noHang { get; set; }
    }
}
