using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewGiaoDichTuiTienMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewGiaoDichTuiTien>
    {
        public ViewGiaoDichTuiTienMapping()
        {
            this.ToTable("ViewGiaoDichTuiTien");
           
        }
    }
}
