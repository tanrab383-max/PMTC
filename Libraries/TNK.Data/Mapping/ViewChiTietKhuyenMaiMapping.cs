using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewChiTietKhuyenMaiMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ViewChiTietKhuyenMai>
    {
        public ViewChiTietKhuyenMaiMapping()
        {
            this.ToTable("ViewChiTietKhuyenMai");
        }
    }
}
