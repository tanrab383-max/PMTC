using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class ConfigMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.Config>
    {
        public ConfigMapping()
        {
            this.ToTable("Config");
            this.HasKey(x => x.ConfigCode);
            this.Ignore(x => x.Id);
        }
    }
}
