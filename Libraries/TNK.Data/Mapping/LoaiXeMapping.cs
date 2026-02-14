using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class LoaiXeMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.LoaiXe>
    {
        public LoaiXeMapping()
        {
            this.ToTable("LoaiXe");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaLoaiXe);
        }
    }
}
