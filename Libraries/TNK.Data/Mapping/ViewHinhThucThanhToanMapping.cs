using TNK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewHinhThucThanhToanMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewHinhThucThanhToan>
    {
        public ViewHinhThucThanhToanMapping()
        {
            this.ToTable("ViewHinhThucThanhToan");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.GiaTri);
        }
    }
}
