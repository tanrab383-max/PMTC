using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{

    public class TuiDinhKhoanModel
    {
        public TuiDinhKhoan Item { get; set; }
        public List<TuiDinhKhoan> DanhSachTuiDinhKhoan { get; set; }
       
    }
}
