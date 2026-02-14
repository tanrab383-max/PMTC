using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class DropDownListItem : BaseEntity
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public DropDownListItem(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}
