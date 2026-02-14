using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Model
{
    public class LichSuThaoTacModel
    {
        public List<LichSuThaoTac> GetLichSuThaoTac { get; set; }
        public LichSuThaoTac Item { get; set; }
        public List<User> Users { get; set; }
        public List<TuiDinhKhoan> TuiDinhKhoan { get; set; }
    }
}
