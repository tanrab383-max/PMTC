using TNK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class CategoryMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.Category>
    {
        public CategoryMapping()
        {
            this.ToTable("Category");
            this.Ignore(u => u.Children);
        }
    }
}
