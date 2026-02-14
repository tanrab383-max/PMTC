using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTuiHangTrenDuongMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTuiHangTrenDuong>
    {
        public ViewTuiHangTrenDuongMapping()
        {
            this.ToTable("ViewTuiHangTrenDuong");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaTui);
        }
    }
}
