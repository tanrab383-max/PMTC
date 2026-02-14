using System;

namespace TNK.Core.Domain.View

{
    public partial class ViewMenuAction : BaseEntity
    {
      
        public Nullable<int> MenuId { get; set; }
        public Nullable<int> MenuActionId { get; set; }
        public string MenuName { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
