using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain.View;

namespace TNK.Data.Mapping
{
    public class ViewCongNoMapping : TNKEntityTypeConfiguration<ViewCongNo>
    {
        public ViewCongNoMapping()
        {
            this.ToTable("ViewCongNo");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}
