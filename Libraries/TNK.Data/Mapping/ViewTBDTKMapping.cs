using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTBDTKMapping :  TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTBDTK>
    {
          public ViewTBDTKMapping()
            {
                this.ToTable("ViewTBDTK");
                this.Ignore(x => x.Id);
                this.HasKey(x => x.MaPhieuThu);
            }
    }
}
