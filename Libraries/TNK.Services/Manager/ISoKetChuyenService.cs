using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.Manager
{
    public interface ISoKetChuyenService
    {
        string LayNgayKetChuyenKeTiep();
        string TaoKetChuyen(string strNgayKC, string strNguoiKC);
        DataTable LayDanhSachSoKetChuyen(string strTinhTrang);
        string HuyKetChuyen(Guid gdId, string strNguoiHuy, string strGhiChu);
        DateTime NgayKetChuyenCuoiCung();
    }
}
