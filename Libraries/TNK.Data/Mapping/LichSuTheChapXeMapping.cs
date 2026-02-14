using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Data.Mapping
{
    public partial class LichSuTheChapXeMapping : TNKEntityTypeConfiguration<LichSuTheChapXe>
    {
        public LichSuTheChapXeMapping()
        {
            this.ToTable("LichSuTheChapXe");
            //this.HasKey(x => x.Id);
            //this.Ignore(x => x.Id);
        }
    }
}
