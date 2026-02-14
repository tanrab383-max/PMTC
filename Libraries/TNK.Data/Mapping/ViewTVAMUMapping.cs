using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTVAMUMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTVAMU>
    {
        public ViewTVAMUMapping()
        {
            this.ToTable("ViewTVAMU");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}
