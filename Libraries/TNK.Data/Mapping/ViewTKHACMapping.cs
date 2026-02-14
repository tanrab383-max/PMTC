using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTKHACMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTKHAC>
    {
        public ViewTKHACMapping()
        {
            this.ToTable("ViewTKHAC");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}
