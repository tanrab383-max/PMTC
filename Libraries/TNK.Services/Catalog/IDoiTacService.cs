using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain.View;

namespace TNK.Services.Catalog
{
    public interface IDoiTacService
    {
        List<ViewDoiTac> GetDoiTacList();
        DoiTac GetDoiTac(string maDoiTac);
        DoiTac CheckDoiTac(string maDoiTac);
        List<DoiTac> GetNCC();
        bool Update(DoiTac obj,Guid id);
        bool Create(DoiTac obj,Guid id);
        bool Delete(string maDoiTac,Guid id);
        bool Create(DoiTac obj);
        bool Update(DoiTac obj);
        bool Delete(DoiTac obj);
        List<ViewDoiTac> SearchDoiTacList(DoiTac objDoiTac, ref int total, int p = 1, int pageSize = 30);
        List<ViewDoiTac> GetNDT();
        List<ViewDoiTac> GetNDT1();
        bool getDoiTacById(string MaVietTat);
        bool getCTBHById(string MaVietTat);
        DoiTac getDoiTacTheoTen(string tenDoiTac);
    }
}
