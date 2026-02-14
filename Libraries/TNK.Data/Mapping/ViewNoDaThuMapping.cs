using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewNoDaThuMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewNoDaThu>
    {
        public ViewNoDaThuMapping()
        {
            this.ToTable("ViewNoDaThu");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.SoPhieuQuyetToan);
        }
    }
}
