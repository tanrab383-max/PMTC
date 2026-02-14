using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTBAXEMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTBAXE>
    {
        public ViewTBAXEMapping()
        {
            this.ToTable("ViewTBAXE");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.SoKhung);
        }
    }
}
