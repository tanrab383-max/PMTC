using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class DichVuModel
    {
        public List<REPAIR_ORDER_PYS> lst { get; set; }
        public List<User> Users { get; set; }
        public PhieuThu obj { get; set; }
        public List<ViewHinhThucThanhToan> HTTT { get; set; }
        public List<ChiTietPhieuThu> CTPT { get; set; }
        public NoPhaiTra noPhaiTra { get; set; }
        public List<DoiTac> CTBH { get; set; }
        public List<ViewTCODV> Cocs { get; set; }
        public List<ChiTietPhieuDichVu> PDV { get; set; }
        public NoPhaiThu NPT { get; set; }
        public List<ViewChiTietBaoDuongTietKiem> NBDTK { get; set; }
        /// <summary>
        /// Doi tac lam dich vu. Vi du doi tac noi bo, Honda,...
        /// </summary>
        public List<DoiTac> DoiTac { get; set; }
        public List<CategoryItem> LHTT { get; set; }
    }
}
