using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Model
{
    public class DataHistoryModel
    {
        public DataHistory Item { get; set; }
        public List<DataHistory> DataHistory { get; set; }
    }
}
