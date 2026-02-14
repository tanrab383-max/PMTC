using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ChiTietKhuyenMaiMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ChiTietKhuyenMai>
    {
        public ChiTietKhuyenMaiMapping()
        {
            this.ToTable("ChiTietKhuyenMai");
        }
    }
}
