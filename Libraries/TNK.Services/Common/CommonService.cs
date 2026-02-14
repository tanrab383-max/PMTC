using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TNK.Data;
using TNK.Model;

namespace TNK.Services.Common
{
    public class CommonService : ICommonService
    {
        IDbContext _dbContext;
        public CommonService(IDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public string CreateId(string mlp)
        {
            SqlParameter MaLoaiPhieu = new SqlParameter("MaLoaiPhieu", mlp);
            SqlParameter MaPhieuThu = new SqlParameter();
            MaPhieuThu.Direction = System.Data.ParameterDirection.Output;
            MaPhieuThu.DbType = System.Data.DbType.String;
            MaPhieuThu.Size = 100;
            MaPhieuThu.ParameterName = "MaPhieuThu";
            _dbContext.ExecuteStoredProcedure("sp_GetLastId",
                MaLoaiPhieu,
                MaPhieuThu
                );
            return MaPhieuThu.Value.ToString();
        }

        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }
        public List<ChuongTrinhDichVuHang> getChuongTrinhDichVuHang()
        {
            SqlParameter date = new SqlParameter("date", null);
            List<ChuongTrinhDichVuHang> data = _dbContext.ExecuteStoredProcedureList<ChuongTrinhDichVuHang>("sp_DanhSachChuongTrinhDichVuHang").ToList();
            return data;
        }
    }
}
