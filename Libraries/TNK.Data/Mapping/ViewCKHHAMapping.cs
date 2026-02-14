using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain.View;

namespace TNK.Data.Mapping
{
    public class ViewCKHHAMapping : TNKEntityTypeConfiguration<ViewCKHHA>
    {
        public ViewCKHHAMapping()
        {
            this.ToTable("ViewChiKhauHao");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}
