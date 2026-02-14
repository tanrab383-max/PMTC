using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Services.No
{
    public interface INoPhaiTraService
    {
        bool InsertNoPhaiTra(NoPhaiTra info, string maPhieuPhatSinh);
        bool UpdateNoPhaiTra(NoPhaiTra info);
        List<NoPhaiTra> GetNoPhaiTra(string maPhieuPhatSinh, string type ="");
        bool DeleteNoPhaiTra(NoPhaiTra info, string note);
        bool UpdateMaPhieuChi(string id, string maphieuchi);
        ViewNoPhaiTra getNoPhaiTra(string MaPhieuPhatSinh);
        NoPhaiTra Get(string id);
    }
}
