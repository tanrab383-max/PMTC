using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Data;
using TNK.Core.Domain.View;
namespace TNK.Services.TreoTiens
{
    public class TreoTienService : ITreoTienService
    {
        IRepository<ViewPhieuTreo> _phieuTreoRepository;

        public TreoTienService(IRepository<ViewPhieuTreo> _phieuTreoRepository)
        {
            this._phieuTreoRepository = _phieuTreoRepository;
        }

        public List<ViewPhieuTreo> Get(DateTime from, DateTime to, DateTime toHang, string query, int p, ref int total, int pageSize, string tinhtrang)
        {
            var q = _phieuTreoRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to);

            tinhtrang = string.IsNullOrEmpty(tinhtrang) ? "C" : tinhtrang;
            if (tinhtrang == "H")
            {
                q = q.Where(x => x.NgayTienVao != null && x.TinhTrang == false && x.NgayTienVao <= toHang);
            }
            else if (tinhtrang == "C")
            {
                q = q.Where(x => (x.TinhTrang == true) || (x.TinhTrang == false && x.NgayTienVao > toHang));
            }
            if (!string.IsNullOrEmpty(query))
            {
                q = q.Where(x => x.MaPhieuThu.Contains(query)
                || x.MaPhieuThu.Contains(query)
                || x.MaNganHang.ToLower().Contains(query)
                || x.TenNganHang.ToLower().Contains(query)
                || x.HoTen.ToLower().Contains(query));
            }
            var result = q.OrderByDescending(x => x.MaPhieuThu).ToList();
            total = result.Count;
            return result.Skip((p - 1) * pageSize).Take(pageSize).ToList();
        }




    }
}
