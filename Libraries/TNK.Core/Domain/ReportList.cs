using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ReportList : BaseEntity
    {
        public Guid UIID { get; set; }
        public string ReportName { get; set; }

        public string TabId { get; set; }

        public string Href { get; set; }

        public string Url { get; set; }

        public string Method { get; set; }

        public int? SortOrder { get; set; }
        public string SheetName { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ExpirDate { get; set; }
    }
}
