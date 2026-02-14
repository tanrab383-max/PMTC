using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class KhoTaiSanModel
    {
        public List<DoiTac> ListNhaCungCap { get; set; }
        public KhoTaiSan Item { get; set; }
        public List<CategoryItem> ListTenTaiSan { get; set; }

    }
}
