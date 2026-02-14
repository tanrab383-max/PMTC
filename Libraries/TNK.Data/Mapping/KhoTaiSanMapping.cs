using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class KhoTaiSanMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.KhoTaiSan>
    {
        public KhoTaiSanMapping()
        {
            this.ToTable("KhoTaiSan");
            
        }
    }
}
