using System;
using System.Collections.Generic;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Model;

namespace TNK.Services.Catalog
{
    public interface IGiaoDichTuiTienService
    {
        List<ViewGiaoDichTuiTien> GetDanhSachGiaoDich(string maTui, string loaiGiaoDich, DateTime from, DateTime to, int p, ref int total, int pageSize);
    }
}
 