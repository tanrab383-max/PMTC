using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ReportCongNoMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.Report.ReportCongNo>
    {
        public ReportCongNoMapping()
        {
            this.ToTable("ReportCongNo");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}
