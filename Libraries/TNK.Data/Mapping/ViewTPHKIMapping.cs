using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTPHKIMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTPHKI>
    {
        public ViewTPHKIMapping()
        {
            this.ToTable("ViewTPHKI");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}
