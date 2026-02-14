using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain.View;

namespace TNK.Services.TreoTiens
{
    public interface ITreoTienService
    {
        List<ViewPhieuTreo> Get(DateTime from, DateTime to, DateTime toHang, string query, int p, ref int total, int pageSize, string tinhtrang);
    }
}
