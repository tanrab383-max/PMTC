using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTBAHIMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTBAHI>
    {
        public ViewTBAHIMapping()
        {
            this.ToTable("ViewTBAHI");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}
