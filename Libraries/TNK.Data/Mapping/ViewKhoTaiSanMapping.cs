using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewKhoTaiSanMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewKhoTaiSan>
    {
        public ViewKhoTaiSanMapping()
        {
            this.ToTable("ViewKhoTaiSan");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.KId);
           
        }
    }
}
