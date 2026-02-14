using TNK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class CategoryItemMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.CategoryItem>
    {
        public CategoryItemMapping()
        {
            this.ToTable("CategoryItem");
        }
    }
}
