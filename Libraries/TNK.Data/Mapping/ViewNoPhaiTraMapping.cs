using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewNoPhaiTraMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewNoPhaiTra>
    {
        public ViewNoPhaiTraMapping()
        {
            this.ToTable("ViewNoPhaiTra");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuNo);
        }
    }
}
