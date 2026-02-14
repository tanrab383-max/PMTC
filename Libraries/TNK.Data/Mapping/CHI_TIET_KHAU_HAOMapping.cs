using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Data.Mapping
{
    public class CHI_TIET_KHAU_HAOMapping :TNKEntityTypeConfiguration<CHI_TIET_KHAU_HAO>
    {
        public CHI_TIET_KHAU_HAOMapping()
        {
            this.ToTable("CHI_TIET_KHAU_HAO");
            this.HasKey(x => x.MaId);
            this.Ignore(x => x.Id);
        }
    }
}
