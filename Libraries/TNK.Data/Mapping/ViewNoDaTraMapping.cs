using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewNoDaTraMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewNoDaTra>
    {
        public ViewNoDaTraMapping()
        {
            this.ToTable("ViewNoDaTra");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}
