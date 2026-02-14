using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class KhoXeMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.KhoXe>
    {
        public KhoXeMapping()
        {
            this.ToTable("KhoXe");
            //this.Ignore(x => x.Id);
            //this.HasKey(x => x.Id);
        }
    }
}
