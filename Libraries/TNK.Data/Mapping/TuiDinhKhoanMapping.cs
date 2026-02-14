using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class TuiDinhKhoanMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.TuiDinhKhoan>
    {
        public TuiDinhKhoanMapping()
        {
            this.ToTable("TuiDinhKhoan");            
            this.HasKey(x => x.MaTui);
            this.Ignore(x => x.Id);
        }
    }
}
