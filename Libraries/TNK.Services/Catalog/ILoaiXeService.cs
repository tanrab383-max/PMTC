using System;
using System.Collections.Generic;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Model;

namespace TNK.Services.Catalog
{
    public interface ILoaiXeService
    {        
        List<ViewLoaiXe> GetViewLoaiXeList();
        LoaiXe GetLoaiXe(string maLoaiXe);
        List<CategoryItem> GetCategoryItemByParent(string parent);
        string Insert(LoaiXe obj);
        bool Update(LoaiXe obj);
        bool Update_New(LoaiXe obj,string maLoaiXe, double GNYOLD);
        string Delete(string maLoaiXe);
        bool Delete(int id);
        List<ViewLoaiXe> SearchViewLoaiXeList(LoaiXe objLoaiXe, ref int total, int p = 1, int pageSize = 30);
        List<DoiTac> ListThueChap();
        LichSuTheChapXe XeTCCuoiCung(string SoKhung);
        int CheckLSTC(string SoKhung,DateTime From,DateTime? To);
        int CheckTheChap(string SoKhung, DateTime NgayHienTai);
        CategoryItem NamDoiXe(string namDoiXe);
        CategoryItem NamDoiXe_Edit(string Id);
        bool HHHB(string SoKhung, DateTime NgayHienTai);
    }
}
 