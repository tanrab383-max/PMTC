using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Services.SoChi
{
    public interface IChiTietNhapKhoPhuTungService
    {
        bool UpdateCTNKPT(ChiTietNhapKhoPhuTung obj);
        bool InsertCTNKPT(ChiTietNhapKhoPhuTung ctkm);
        List<ChiTietNhapKhoPhuTung> GetCTNKPT(string id);
        List<ViewNguonKhuyenMai> GetNKM();
    }
}
