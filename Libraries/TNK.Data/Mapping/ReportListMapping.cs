using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
  public partial class ReportListMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ReportList>
    {
        public ReportListMapping()
        {
            this.ToTable("ReportList");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.UIID);
        }
    }
}
