using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Model;

namespace TNK.Services.Common
{
    public interface ICommonService
    {
        string CreateId(string mlp);

        DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist);
        List<ChuongTrinhDichVuHang> getChuongTrinhDichVuHang();
    }
}
