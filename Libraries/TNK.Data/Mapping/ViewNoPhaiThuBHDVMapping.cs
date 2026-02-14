using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewNoPhaiThuBHDVMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewNoPhaiThuBHDV>
    {
        public ViewNoPhaiThuBHDVMapping()
        {
            this.ToTable("ViewNoPhaiThuBHDV");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuNo);
        }
    }
}
