using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Data;
using TNK.Services.Authentication;

namespace TNK.Services.Manager
{
    public class SoKetChuyenService : ISoKetChuyenService
    {
        IAuthenticationService _authenticationService;
        IDbContext _dbContext;
        public SoKetChuyenService(IAuthenticationService _authenticationService
            , IDbContext _dbContext
            )
        {
            this._authenticationService = _authenticationService;
            this._dbContext = _dbContext;
        }

        public string LayNgayKetChuyenKeTiep()
        {
            SqlParameter opNgayKetChuyen = new SqlParameter("NgayKetChuyen", SqlDbType.VarChar, 10);
            opNgayKetChuyen.Direction = ParameterDirection.Output;
            return _dbContext.ExecuteStoredProcedureToString("sp_LayNgayKetChuyenKeTiep", opNgayKetChuyen);
        }

        public string TaoKetChuyen(string strNgayKC, string strNguoiKC)
        {
            SqlParameter ngayKC = new SqlParameter("NgayKetChuyen", DBNull.Value);
            SqlParameter nguoiKC = new SqlParameter("NguoiKetChuyen", strNguoiKC);

            SqlParameter opMessage = new SqlParameter("message", SqlDbType.NVarChar, 1000);
            opMessage.Direction = ParameterDirection.Output;
            _dbContext.ExecuteStoredProcedureToString("sp_TaoKetChuyen", opMessage, ngayKC, nguoiKC);
            return opMessage.Value.ToString();
        }

        public DataTable LayDanhSachSoKetChuyen(string strTinhTrang)
        {
            SqlParameter tinhTrang = new SqlParameter("TinhTrang", strTinhTrang);

            return _dbContext.ExecuteStoredProcedureDataTable("sp_LayDanhSachSoKetChuyen", tinhTrang);
        }

        public string HuyKetChuyen(Guid gdId, string strNguoiHuy, string strGhiChu)
        {
            SqlParameter id = new SqlParameter("Id", gdId);
            SqlParameter nguoiHuy = new SqlParameter("NguoiHuy", strNguoiHuy);
            SqlParameter ghiChu = new SqlParameter("ghiChu", strGhiChu);

            SqlParameter opMessage = new SqlParameter("message", SqlDbType.NVarChar, 1000);
            opMessage.Direction = ParameterDirection.Output;

            _dbContext.ExecuteStoredProcedureToString("sp_HuyKetChuyen", opMessage, id, nguoiHuy, ghiChu);
            return opMessage.Value.ToString();
        }

        public DateTime NgayKetChuyenCuoiCung()
        {
            SqlParameter ngayKetChuyenCuoi = new SqlParameter("@ngayKetChuyenCuoi", SqlDbType.DateTime);
            ngayKetChuyenCuoi.Direction = ParameterDirection.Output;
            _dbContext.ExecuteStoredProcedureToString("sp_LayNgayKetChuyenCuoi", ngayKetChuyenCuoi);
            return DateTime.Parse( ngayKetChuyenCuoi.Value.ToString());
        }
    }
}
