using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class LoaiXeModel
    {
        public LoaiXe Item { get; set; }
        public List<CategoryItem> ListModelXe { get; set; }
        public List<CategoryItem> ListDoiXe { get; set; }
        public List<CategoryItem> ListMauXe { get; set; }
        public List<CategoryItem> ListGrad { get; set; }
        public List<DoiTac> ListHang { get; set; }
    }
}
