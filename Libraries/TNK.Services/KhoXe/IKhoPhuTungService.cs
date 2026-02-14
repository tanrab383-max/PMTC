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
    public interface IKhoPhuTungService
    {
        bool CreateKhoPhuTung(KhoPhuTung model);
        bool UpdateKhoPhuTung(KhoPhuTung model);
        bool DeleteKhoPhuTung(string maKho);
        KhoPhuTung GetKhoPhuTung(string maKho);
        List<KhoPhuTung> GetKhoPhuTung(string query, int p, ref int total, int pageSize);
        string EditKhoPhuTung(DieuChinhKhoPhuTung obj);
        DataTable GetList();
        string DeleteDieuChinhKho(Guid ID);
    }
}
