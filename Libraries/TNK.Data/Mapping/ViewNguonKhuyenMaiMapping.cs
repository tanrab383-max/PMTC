using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewNguonKhuyenMaiMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewNguonKhuyenMai>
    {
        public ViewNguonKhuyenMaiMapping()
        {
            this.ToTable("ViewNguonKhuyenMai");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.NguonKhuyenMai);
        }
    }
}
