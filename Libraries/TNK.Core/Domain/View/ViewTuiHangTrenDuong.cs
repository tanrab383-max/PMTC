using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTuiHangTrenDuong:BaseEntity
    {
      public string MaTui{get;set;}
      public string TenTui{get;set;}
      public double SoTienDangCo{get;set;}
      public string NhaCungCap{get;set;}

    }
}
