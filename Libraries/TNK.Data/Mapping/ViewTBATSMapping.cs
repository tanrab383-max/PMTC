using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTBATSMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTBATS>
    {
        public ViewTBATSMapping()
        {
            this.ToTable("ViewTBATS");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}
