using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewLoaiXeMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewLoaiXe>
    {
        public ViewLoaiXeMapping()
        {
            this.ToTable("ViewLoaiXe");
            //this.Ignore(x => x.Id);
            //this.HasKey(x => x.MaLoaiXe);
        }
    }
}
