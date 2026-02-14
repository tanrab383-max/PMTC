using TNK.Core.Domain;
using System.Collections.Generic;

namespace TNK.Models
{
    public partial class MenuDetailModel
    {
        public Menu Detail { get; set; }
        public List<MenuAction> Types { get; set; }
    }
}