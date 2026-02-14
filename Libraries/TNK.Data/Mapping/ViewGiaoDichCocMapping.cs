using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewGiaoDichCocMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewGiaoDichCoc>
    {
        public ViewGiaoDichCocMapping()
        {
            this.ToTable("ViewGiaoDichCoc");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.ID);
        }
            
    }
}
