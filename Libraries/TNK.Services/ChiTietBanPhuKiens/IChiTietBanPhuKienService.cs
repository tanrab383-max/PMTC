using TNK.Core.Domain;
using System;
using System.Collections.Generic;

namespace TNK.Services.ChiTietBanPhuKiens
{
    public interface IChiTietBanPhuKienService
    {
        List<ChiTietBanPhuKien> GetAll();
		ChiTietBanPhuKien GetById(int id);
		bool Create(ChiTietBanPhuKien obj); 
		bool Update(ChiTietBanPhuKien obj); 
		bool Delete(ChiTietBanPhuKien obj);
        List<ChiTietBanPhuKien> GetByMaPhieuThu(string id);
        List<ChiTietBanPhuKien> GetBySoHopDong(string id);
    }
}
