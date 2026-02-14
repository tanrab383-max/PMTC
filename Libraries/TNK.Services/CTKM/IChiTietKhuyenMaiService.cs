using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Services.CTKM
{
    public interface IChiTietKhuyenMaiService
    {
        bool UpdateCTKM(ChiTietKhuyenMai obj);
        bool InsertCTKM(ChiTietKhuyenMai ctkm);
        List<ViewChiTietKhuyenMai> GetCTKM(string id);
        List<ViewNguonKhuyenMai> GetNKM(bool isForInsert = true);
    }
}
