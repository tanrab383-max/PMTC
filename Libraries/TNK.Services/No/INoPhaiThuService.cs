using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Services.No
{
    public interface INoPhaiThuService
    {
        bool InsertNoPhaiThu(string loaiphieu, string maphieuthu, string nguoibaolanh, double sotienno, string donvino, string ghichu);
        bool UpdateNoPhaiThu(NoPhaiThu model);
        List<NoPhaiThu> GetNoPhaiThu(string maPhieuPhatSinh, string type = "");
        bool UpdateMaPhieuThu(string id, string maphieuthu);
    }
}
