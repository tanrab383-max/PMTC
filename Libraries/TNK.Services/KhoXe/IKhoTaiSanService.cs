using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Services.KhoXe
{
    public interface IKhoTaiSanService
    {
        bool CreateTaiSan(KhoTaiSan model);
        bool UpdateTaiSan(KhoTaiSan model);
        bool DeleteTaiSan(Guid id);
        KhoTaiSan GetTaiSan(Guid id);
        List<ViewKhoTaiSan> GetTaiSan();
        List<ViewKhoTaiSan> GetTaiSan_Edit();
        List<ViewKhoTaiSan> GetTaiSan(DateTime from, DateTime to,string query, int p, ref int total, int pageSize);
        List<ViewKhoTaiSan> GetListAll(DateTime from, DateTime to, string query);
    }
}
