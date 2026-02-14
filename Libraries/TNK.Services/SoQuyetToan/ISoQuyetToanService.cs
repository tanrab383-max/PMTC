using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Services.SoQuyetToan
{
    public interface ISoQuyetToanService
    {
        string Create(string maPhieuNos,Guid nguoiLapPhieu);
        List<ViewNoPhaiTra> GetDanhSachNKMPK(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang);
        List<PhieuQTModel> GetDanhSachPhieuQT(string id, DateTime from, DateTime to, string query, int p, ref int total, int pageSize, string tinhtrang, string loai);
        string Delete(string id, Guid nguoiLapPhieu);
    }
}
