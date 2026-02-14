using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewThongKeKhoMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewThongKeKho>
    {
        public ViewThongKeKhoMapping()
        {
            this.ToTable("ViewThongKeKho");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaLoaiXe);
        }
    }
}
