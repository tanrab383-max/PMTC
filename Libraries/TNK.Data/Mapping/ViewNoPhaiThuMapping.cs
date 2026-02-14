using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewNoPhaiThuMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewNoPhaiThu>
    {
        public ViewNoPhaiThuMapping()
        {
            this.ToTable("ViewNoPhaiThu");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuNo);
        }
    }
}
