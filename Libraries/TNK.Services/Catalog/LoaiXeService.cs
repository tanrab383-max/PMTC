using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Data;
using TNK.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Services.Authentication;
using TNK.Core.Caching;
using TNK.Services.Log;
using TNK.Services.LichSuThaoTac;

namespace TNK.Services.Catalog
{
    public class LoaiXeService : ILoaiXeService
    {
        IRepository<ViewLoaiXe> _viewLoaiXeRepository;
        IRepository<LoaiXe> _loaiXeRepository;
        IRepository<LichSuTheChapXe> _lichSuTheChapXeRepository;
        IRepository<CategoryItem> _categoryItemRepository;
        IRepository<Category> _categoryRepository;
        IRepository<DoiTac> _DoiTacRepository;
        IAuthenticationService _authenticationService;
        ILichSuThaoTacService _lichSuThaoTacService;
        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;
        string _ipClient = "";
        string _hostNameClient = "";
        IRepository<ViewKhoXe> _viewKhoXerepository;
        public LoaiXeService(IRepository<ViewLoaiXe> _viewLoaiXeRepository
            , IRepository<LoaiXe> _loaiXeRepository
            , IRepository<LichSuTheChapXe> _lichSuTheChapXeRepository
            , IAuthenticationService _authenticationService
            , IRepository<CategoryItem> _categoryItemRepository
            , IRepository<DoiTac> _DoiTacRepository
            , IRepository<Category> _categoryRepository
            , ILichSuThaoTacService _lichSuThaoTacService
            , IDbContext _dbContext
            , ICacheManager _cacheManager
            , ILogger _log
            , IRepository<ViewKhoXe> _viewKhoXerepository
            )
        {
            this._viewLoaiXeRepository = _viewLoaiXeRepository;
            this._loaiXeRepository = _loaiXeRepository;
            this._lichSuTheChapXeRepository = _lichSuTheChapXeRepository;
            this._categoryItemRepository = _categoryItemRepository;
            this._categoryRepository = _categoryRepository;
            this._DoiTacRepository = _DoiTacRepository;
            this._lichSuThaoTacService = _lichSuThaoTacService;
            this._dbContext = _dbContext;
            this._authenticationService = _authenticationService;
            this._cacheManager = _cacheManager;
            this._log = _log;
            this._viewKhoXerepository = _viewKhoXerepository;

        }
        public void SetIPClient(string ip)
        {
            this._ipClient = ip;
        }
        public void SetHostNameClient(string host)
        {
            this._hostNameClient = host;
        }
        public List<ViewLoaiXe> GetViewLoaiXeList()
        {
            try
            {
                List<ViewLoaiXe> ls = _viewLoaiXeRepository.Table.Where(x => x.IsDeleted == false).OrderBy(x => x.MaLoaiXe).ToList();
                return ls;
            }
            catch(Exception ex)
            {
                _log.WriteLog("TKKNService.GetViewLoaiXeList:" + ex.Message.ToString());
                return null;
            }
        }

        public LoaiXe GetLoaiXe(string maLoaiXe)
        {
            try
            {
                LoaiXe loaixe = _loaiXeRepository.Table.Where(x => x.MaLoaiXe == maLoaiXe && x.IsDeleted == false).FirstOrDefault();
                return loaixe;
            }
            catch(Exception ex)
            {
                _log.WriteLog("TKKNService.GetLoaiXe("+maLoaiXe+"):" + ex.Message.ToString());
                return null;
            }
            
        }

        public List<CategoryItem> GetCategoryItemByParent(string parent)
        {
            try
            {
                int p = _categoryRepository.Table.Where(x => x.Code == parent).ToList()[0].Id;
                return _categoryItemRepository.Table.Where(x => x.Parent == p && x.IsDeleted == false).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("TKKNService.GetCategoryItemByParent(" + parent + "):" + ex.Message.ToString());
                return null;
            }
        }
        public string Insert(LoaiXe model)
        {
            try
            {
                if (CheckExistsLoaiXe(model.MaModel, model.MaMau,model.DoiXe,model.XuatXu,model.GiaNiemYet.Value))
                    return "Đã tồn tại loại xe này.";
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                string maLoaiXe = "";
                CategoryItem nameDoiXe = _categoryItemRepository.Table.Where(x => x.Id == model.DoiXe).FirstOrDefault();
                maLoaiXe += GetCategoryItem(model.MaModel);
               // maLoaiXe += "-" + GetCategoryItem(model.Grad);
               // maLoaiXe += "-" + GetCategoryItem(model.DoiXe);
                maLoaiXe += "-" + GetCategoryItem(model.MaMau);
                model.MaLoaiXe = maLoaiXe + model.XuatXu + nameDoiXe.Code;
                model.CreatedBy = uid;
                model.UpdatedBy = uid;
                model.CreatedDate = DateTime.Now;
                if (model.GiaNiemYet == null)
                    model.GiaNiemYet = 0;
                _loaiXeRepository.Insert(model);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, model.MaLoaiXe, Convert.ToString(model.MaModel), "Insert", "Insert Loại Xe", "Mã loại xe :" + model.MaLoaiXe + " mã model :" + model.MaModel, _authenticationService.GetAuthenticatedUser().UserId);
                return "";
            }
            catch (Exception e)
            {
                _log.WriteLog("LoaiXeService.Insert: " + e.Message.ToString());
                return e.ToString();
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var obj = _loaiXeRepository.Table.FirstOrDefault(x => x.Id == id);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = DateTime.Now;
                    _loaiXeRepository.Update(obj);
                    return true;
                }
            }
            catch (Exception e)
            {
                _log.WriteLog("LoaiXeService.Delete(" + id+ "): " + e.Message.ToString());
                return false;
            }
        }

        public string Delete(string maLoaiXe)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var obj = _loaiXeRepository.Table.FirstOrDefault(x => x.MaLoaiXe == maLoaiXe);
                var checkIsUsed = _viewKhoXerepository.Table.Where(s => s.MaLoaiXe == maLoaiXe && s.IsDeleted==false).Count();
                if (obj == null)
                {
                    return "N";//Not exist
                }
                if (checkIsUsed > 0)
                {
                    return "I";//In used
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.UpdatedBy = uid;
                    obj.UpdatedDate = DateTime.Now;
                    _loaiXeRepository.Update(obj);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.MaLoaiXe, Convert.ToString(obj.MaModel), "Delete", "Delete Loại Xe", "Mã loại xe :" + obj.MaLoaiXe + " mã model :" + obj.MaModel, _authenticationService.GetAuthenticatedUser().UserId);
                    return "S";//Success
                }
            }
            catch(Exception e)
            {
                _log.WriteLog("LoaiXeService.Delete(" +maLoaiXe + "): " + e.Message.ToString());
                return "F";//False
            }
        }

        public bool Update(LoaiXe obj)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var data = _loaiXeRepository.Table.FirstOrDefault(x => x.MaLoaiXe == obj.MaLoaiXe);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    var i = _loaiXeRepository.Table.FirstOrDefault(x => x.MaLoaiXe == obj.MaLoaiXe);
                    if (i != null && i.Id != obj.Id)
                    {
                        return false;
                    }
                    data.MaMau = obj.MaMau;
                    data.MaModel = obj.MaModel;
                    data.Grad = obj.Grad;
                    data.DoiXe = obj.DoiXe;
                    data.GhiChu = obj.GhiChu;
                    data.GiaNiemYet = obj.GiaNiemYet;
                    data.Hang = obj.Hang;
                    data.UpdatedBy = uid;
                    data.UpdatedDate = DateTime.Now;
                    _loaiXeRepository.Update(obj);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, data.MaLoaiXe, Convert.ToString(data.MaModel), "UPDATE", "Update Loại Xe", "Mã loại xe :" + data.MaLoaiXe + " mã model :" + data.MaModel, _authenticationService.GetAuthenticatedUser().UserId);
                    SqlParameter maLoai = new SqlParameter("MaLoaiXe", data.MaLoaiXe);
                    SqlParameter GNY = new SqlParameter("GiaNiemYet", data.GiaNiemYet);
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdateLoaiXe", maLoai, GNY);

                    return true;
                }
            }
            catch(Exception e)
            {
                _log.WriteLog("LoaiXeService.Update: " + e.Message.ToString());
                return false;
            }
        }
        public bool Update_New(LoaiXe obj, string maLoaiXe, double GNYOLD)
        {
            try
            {
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                var user = _authenticationService.GetAuthenticatedUser().UserName;
                var data = _loaiXeRepository.Table.FirstOrDefault(x => x.MaLoaiXe == obj.MaLoaiXe);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    var i = _loaiXeRepository.Table.FirstOrDefault(x => x.MaLoaiXe == obj.MaLoaiXe);
                    if (i != null && i.Id != obj.Id)
                    {
                        return false;
                    }
                    data.MaMau = obj.MaMau;
                    data.MaModel = obj.MaModel;
                    data.Grad = obj.Grad;
                    data.DoiXe = obj.DoiXe;
                    data.GhiChu = obj.GhiChu;
                    data.GiaNiemYet = obj.GiaNiemYet;
                    data.Hang = obj.Hang;
                    data.UpdatedBy = uid;
                    data.UpdatedDate = DateTime.Now;
                    //data.MaLoaiXe = maLoaiXe;
                    _loaiXeRepository.Update(obj);
                    //SqlParameter MaLoaiXe_old = new SqlParameter("maLoaiXe_old", data.MaLoaiXe);
                    SqlParameter MaLoaiXe = new SqlParameter("maLoaiXe", maLoaiXe);
                    //SqlParameter GiaNiemYet = new SqlParameter("giaNiemYet", obj.GiaNiemYet);
                    _dbContext.ExecuteStoredProcedure("sp_UpdateMaLoaiXe", MaLoaiXe);

                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient
                        , data.MaLoaiXe, Convert.ToString(data.MaModel), "UPDATE", "Update Loại Xe"
                        , "Mã loại xe :" + data.MaLoaiXe + " đã được user " + user + " thay đổi giá niêm yết cũ là : " + GNYOLD + " thành giá niêm yết mới là : " + data.GiaNiemYet
                        , _authenticationService.GetAuthenticatedUser().UserId);
                    SqlParameter maLoai = new SqlParameter("MaLoaiXe", data.MaLoaiXe);
                    SqlParameter GNY = new SqlParameter("GiaNiemYet", obj.GiaNiemYet);
                    _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiUpdateLoaiXe", maLoai, GNY);

                    return true;
                }
            }
            catch (Exception e)
            {
                _log.WriteLog("LoaiXeService.Update: " + e.Message.ToString());
                return false;
            }
        }
        public string GetCategoryItem(int id)
        {
            try
            {
                return _categoryItemRepository.GetById(id).Code;
            }
            catch (Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetCategoryItem:(" + id + ") " + ex.Message.ToString());
                return null;
            }
        }
        public List<ViewLoaiXe> SearchViewLoaiXeList(LoaiXe objLoaiXe, ref int total, int p, int pageSize)
        {
            try
            {
                List<ViewLoaiXe> ls = new List<ViewLoaiXe>();
                if(objLoaiXe.XuatXu != null)
                {
                    ls = _viewLoaiXeRepository.Table.Where(x => x.IsDeleted == false
                    && (null == objLoaiXe.MaLoaiXe || x.MaLoaiXe.Contains(objLoaiXe.MaLoaiXe))
                    && (objLoaiXe.MaModel < 1 || x.MaModel == objLoaiXe.MaModel)
                    && (objLoaiXe.Grad < 1 || x.Grad == objLoaiXe.Grad)
                    && (objLoaiXe.DoiXe < 1 || x.DoiXe == objLoaiXe.DoiXe)
                    && (objLoaiXe.MaMau < 1 || x.MaMau == objLoaiXe.MaMau)
                    && (x.XuatXu == objLoaiXe.XuatXu)
                    && (null == objLoaiXe.Hang || x.Hang.Contains(objLoaiXe.Hang))
                    ).OrderBy(x => x.MaLoaiXe).ToList();
                }
                else
                {
                    ls = _viewLoaiXeRepository.Table.Where(x => x.IsDeleted == false
                    && (null == objLoaiXe.MaLoaiXe || x.MaLoaiXe.Contains(objLoaiXe.MaLoaiXe))
                    && (objLoaiXe.MaModel < 1 || x.MaModel == objLoaiXe.MaModel)
                    && (objLoaiXe.Grad < 1 || x.Grad == objLoaiXe.Grad)
                    && (objLoaiXe.DoiXe < 1 || x.DoiXe == objLoaiXe.DoiXe)
                    && (objLoaiXe.MaMau < 1 || x.MaMau == objLoaiXe.MaMau)
                    //&& (x.XuatXu == objLoaiXe.XuatXu)
                    && (null == objLoaiXe.Hang || x.Hang.Contains(objLoaiXe.Hang))
                    ).OrderBy(x => x.MaLoaiXe).ToList();
                }
                total = ls.Count;
                return ls.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.SearchViewLoaiXeList:(" + objLoaiXe + ") " + ex.Message.ToString());
                return null;
            }
        }
        public List<DoiTac> ListThueChap()
        {
            List<DoiTac> list = _DoiTacRepository.Table.Where(x=>x.LoaiDoiTac == "NH" && x.IsDeleted == false).ToList();
            return list;
        }
        public LichSuTheChapXe XeTCCuoiCung(string SoKhung)
        {
            LichSuTheChapXe Xe = _lichSuTheChapXeRepository.Table.Where(x => x.SoKhung == SoKhung && x.IsDeleted == false && x.IsActive == true).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
            return Xe;
        }
        public int CheckLSTC(string SoKhung, DateTime From, DateTime? To)
        {
            if(To == null)
            {
                To = new DateTime(2050,01,01);
            }
            SqlParameter prid = new SqlParameter("soKhung", SoKhung);
            SqlParameter prFr = new SqlParameter("From", From);
            SqlParameter prTo = new SqlParameter("To", To);
            List<LichSuTheChapXe> data = _dbContext.ExecuteStoredProcedureList<LichSuTheChapXe>("sp_CheckLichSuTheChap", prFr,prTo,prid).ToList();
            return data.Count;
        }
        public int CheckTheChap(string SoKhung, DateTime NgayHienTai)
        {
            SqlParameter prNHT = new SqlParameter("NgayHienTai", NgayHienTai);
            SqlParameter prSK = new SqlParameter("soKhung", SoKhung);
            List<LichSuTheChapXe> data = _dbContext.ExecuteStoredProcedureList<LichSuTheChapXe>("sp_CheckTheChapXe", prNHT, prSK).ToList();
            return data.Count;
        }
        public bool HHHB(string SoKhung, DateTime NgayHienTai)
        {
            SqlParameter prNHT = new SqlParameter("NgayHienTai", NgayHienTai);
            SqlParameter prSK = new SqlParameter("soKhung", SoKhung);
            NoPhaiThu data = _dbContext.ExecuteStoredProcedureList<NoPhaiThu>("sp_CheckHHHB", prNHT, prSK).FirstOrDefault();
            if(data != null)
            {
                if (data.SoTienDaTra > 0)
                    return false;
            }
            return true;
        }
        public bool CheckExistsLoaiXe(int maModel, int maMau,int DoiXe,string XuatXu, double GiaNiemYet)
        {
            try
            {
                List<ViewLoaiXe> ls = _viewLoaiXeRepository.Table.Where(x => x.IsDeleted == false
                && x.MaModel == maModel
                && x.MaMau == maMau
                && x.DoiXe == DoiXe
                && x.XuatXu == XuatXu
                //&& x.GiaNiemYet == GiaNiemYet
                ).ToList();
                if (ls.Count > 0)
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.CheckExistsLoaiXe:(" + maModel + ","+maMau+") " + ex.Message.ToString());
                return false;
            }
        }
        public CategoryItem NamDoiXe(string namDoiXe)
        {
            try
            {
                CategoryItem NamDoiXe = _categoryItemRepository.Table.Where(x => x.Code == namDoiXe).FirstOrDefault();
                return NamDoiXe;
            }
            catch (Exception ex)
            {
                _log.WriteLog("LoaiXeService.NamDoiXe:(" + namDoiXe + ") " + ex.Message.ToString());
                return null;
            }
        }
        public CategoryItem NamDoiXe_Edit(string Id)
        {
            try
            {
                int DoiXe = Convert.ToInt32(Id);
                CategoryItem NamDoiXe = _categoryItemRepository.Table.Where(x => x.Id == DoiXe).FirstOrDefault();
                return NamDoiXe;
            }
            catch (Exception ex)
            {
                _log.WriteLog("LoaiXeService.NamDoiXe_Edit:(" + Id + ") " + ex.Message.ToString());
                return null;
            }
        }
    }
}
