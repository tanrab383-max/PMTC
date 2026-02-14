using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Services.NhanVien
{
    public interface INhanVienService
    {
        bool CreateNhanVien(TNK.Core.Domain.NhanVien model);
        bool UpdateNhanVien(TNK.Core.Domain.NhanVien model);
        bool DeleteNhanVien(Guid Id);
        TNK.Core.Domain.NhanVien GetNhanVien(Guid Id);
        List<TNK.Core.Domain.NhanVien> GetNhanVien(string query, int p, ref int total, int pageSize);
    }
}
