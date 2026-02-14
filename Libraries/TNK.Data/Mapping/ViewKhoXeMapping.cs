using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewKhoXeMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewKhoXe>
    {
        public ViewKhoXeMapping()
        {
            this.ToTable("ViewKhoXe");
           // this.Ignore(x => x.Id);
            //this.HasKey(x => x.SoKhung);
        }
    }
}
