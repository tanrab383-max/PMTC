using TNK.Core;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using TNK.Services.Authentication;
using TNK.Core.Domain.View;

namespace TNK.Services.Catalog
{
    public class DoiTacService : IDoiTacService
    {
        IRepository<DoiTac> _doiTacRepository;
        IRepository<ViewDoiTac> _viewDoiTacRepository;
        
         ICacheManager _cacheManager;
        IAuthenticationService _authenticationService;
        public DoiTacService(IRepository<DoiTac> _doiTacRepository
            , ICacheManager _cacheManager
            , IAuthenticationService _authenticationService
            , IRepository<ViewDoiTac> _viewDoiTacRepository)
        {
            this._doiTacRepository = _doiTacRepository;
            this._cacheManager = _cacheManager;
            this._authenticationService = _authenticationService;
            this._viewDoiTacRepository = _viewDoiTacRepository;
        }
        public bool Create(DoiTac obj)
        {
            try
            {
                obj.MaDoiTac = obj.MaDoiTac.Replace("-",".");
                obj.MaVietTat = obj.MaDoiTac.Replace("-", ".");
                _doiTacRepository.Insert(obj);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(DoiTac obj)
        {
            try
            {
                //obj.MaDoiTac = obj.MaDoiTac.Replace("-", ".");
                obj.MaVietTat = obj.MaDoiTac.Replace("-", ".");
                _doiTacRepository.Update(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(DoiTac obj)
        {
            try
            {
                _doiTacRepository.Delete(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<ViewDoiTac> GetDoiTacList()
        {
            var list = _viewDoiTacRepository.Table.ToList();
            return list;
        }

        public DoiTac GetDoiTac(string maDoiTac)
        {
            List<DoiTac> lst = _doiTacRepository.Table.Where(x => x.MaDoiTac ==maDoiTac && x.IsDeleted == false && x.IsActive == true).ToList();
            if (lst.Count > 0)
                return lst[0];
            return null;
        }
        //DoiTac CheckDoiTac(string maDoiTac)
        public DoiTac CheckDoiTac(string maDoiTac)
        {
            List<DoiTac> lst = _doiTacRepository.Table.Where(x => x.MaDoiTac == maDoiTac && x.IsDeleted == true && x.IsActive == false).ToList();
            if (lst.Count > 0)
                return lst[0];
            return null;
        }
        public List< DoiTac> GetNCC()
        {
            List<DoiTac> lst = _doiTacRepository.Table.Where(x => x.LoaiDoiTac == "NCC").ToList();
            return lst;
        }
        //lay nha dau tu
        public List<ViewDoiTac> GetNDT()
        {
            List<ViewDoiTac> lst = _viewDoiTacRepository.Table.Where(x => x.LoaiDoiTac == "NDT").OrderByDescending(x=>x.madoitac).ToList();
            return lst;
        }
        public List<ViewDoiTac> GetNDT1()
        {
            List<ViewDoiTac> lst = _viewDoiTacRepository.Table.Where(x => x.LoaiDoiTac == "NDT").OrderBy(x => x.madoitac).ToList();
            return lst;
        }
        public bool getDoiTacById(string MaVietTat)
        {
            DoiTac dt =  _doiTacRepository.Table.Where(x=>x.MaVietTat == MaVietTat && x.IsDeleted == false && x.LoaiDoiTac == "NH").FirstOrDefault();
            if (dt != null)
                return true;
            return false;
        }
        public bool getCTBHById(string MaVietTat)
        {
            DoiTac dt = _doiTacRepository.Table.Where(x => x.MaDoiTac.Trim() == MaVietTat && x.IsDeleted == false && x.LoaiDoiTac == "BH").FirstOrDefault();
            if (dt != null)
                return true;
            return false;
        }
        public DoiTac getDoiTacTheoTen(string tenDoiTac)
        {
            try
            {
                return _doiTacRepository.Table.Where(x => x.TenDoiTac == tenDoiTac.Trim() && x.IsDeleted == false && x.LoaiDoiTac == "NCC").FirstOrDefault();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public bool Create(DoiTac obj, Guid id)
        {
            try
            {
                var data = _doiTacRepository.Table.FirstOrDefault(x => x.MaDoiTac == obj.MaDoiTac);
                if (data != null)
                    return false;
                DateTime d = DateTime.Now;
                obj.IsDeleted = false;
                obj.IsActive = true;
                obj.CreatedBy = id;
                obj.CreatedDate = d;
                obj.UpdatedBy = id;
                obj.UpdatedDate = d;
                _doiTacRepository.Insert(obj);               
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(string maDoiTac,Guid userId)
        {
            try
            {
                var obj = _doiTacRepository.Table.FirstOrDefault(x => x.MaDoiTac ==maDoiTac);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.IsActive = false;
                    obj.UpdatedBy = userId;
                    obj.UpdatedDate = DateTime.Now;
                    _doiTacRepository.Update(obj);                  
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Update(DoiTac obj, Guid id)
        {
            try
            {
                var data = _doiTacRepository.Table.FirstOrDefault(x => x.MaDoiTac == obj.MaDoiTac);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    var i = _doiTacRepository.Table.FirstOrDefault(x => x.MaDoiTac == obj.MaDoiTac);
                    if (i != null && i.Id != obj.Id)
                    {
                        return false;
                    }
                    obj.MaDoiTac = obj.MaDoiTac.Replace("-", ".");
                    obj.MaVietTat = obj.MaDoiTac.Replace("-", ".");
                    data.MaVietTat = obj.MaVietTat;
                    data.TenDoiTac = obj.TenDoiTac;
                    data.GhiChu = obj.GhiChu;                   
                    data.UpdatedBy = id;
                    data.UpdatedDate = DateTime.Now;
                    data.IsActive = true;
                    data.IsDeleted = false;
                    _doiTacRepository.Update(obj);
                   
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<ViewDoiTac> SearchDoiTacList(DoiTac objDoiTac, ref int total, int p = 1, int pageSize = 30)
        {
            List<ViewDoiTac> ls = _viewDoiTacRepository.Table.Where(x =>
            (null == objDoiTac.MaDoiTac || x.madoitac.Contains(objDoiTac.MaDoiTac.Trim()))
            && (null == objDoiTac.MaVietTat || x.maviettat.Contains(objDoiTac.MaVietTat.Trim()))
            && (null == objDoiTac.TenDoiTac || x.TenDoiTac.Contains(objDoiTac.TenDoiTac.Trim()))
            && (null == objDoiTac.LoaiDoiTac || x.LoaiDoiTac.Contains(objDoiTac.LoaiDoiTac.Trim()))
            ).ToList();
            total = ls.Count;
            return ls.Skip((p - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
