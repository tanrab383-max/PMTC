using TNK.Core.Data;
using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using TNK.Data;
using System.Data.SqlClient;

namespace TNK.Services.ChiTietBanPhuKiens
{
    public class ChiTietBanPhuKienService : IChiTietBanPhuKienService
    {
        private IRepository<ChiTietBanPhuKien> _ChiTietBanPhuKienRepository;
        IDbContext _dbContext;
        public ChiTietBanPhuKienService(IRepository<ChiTietBanPhuKien> _ChiTietBanPhuKienRepository, IDbContext _dbContext)
        {
            this._ChiTietBanPhuKienRepository = _ChiTietBanPhuKienRepository;
            this._dbContext = _dbContext;
        }
        public List<ChiTietBanPhuKien> GetAll()
        {
            return _ChiTietBanPhuKienRepository.Table.ToList();
        }
        public ChiTietBanPhuKien GetById(int id)
        {
            return _ChiTietBanPhuKienRepository.Table.FirstOrDefault(x => x.Id == id);
        }
        public List<ChiTietBanPhuKien> GetBySoHopDong(string id)
        {
            SqlParameter pid = new SqlParameter("id", id);
            return _dbContext.ExecuteStoredProcedureList<ChiTietBanPhuKien>("getCTBPKBySoHopDong", pid).ToList();
        }
        public List<ChiTietBanPhuKien> GetByMaPhieuThu(string id)
        {
            return _ChiTietBanPhuKienRepository.Table.Where(x => x.MaPhieuThu == id).ToList();
        }
        public bool Create(ChiTietBanPhuKien obj)
        {
            try
            {
                _ChiTietBanPhuKienRepository.Insert(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(ChiTietBanPhuKien obj)
        {
            try
            {
                _ChiTietBanPhuKienRepository.Update(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(ChiTietBanPhuKien obj)
        {
            try
            {
                _ChiTietBanPhuKienRepository.Delete(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}