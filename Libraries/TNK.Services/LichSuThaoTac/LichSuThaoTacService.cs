using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Data;
using TNK.Services.Authentication;
using TNK.Services.Log;

namespace TNK.Services.LichSuThaoTac
{
    public class LichSuThaoTacService : ILichSuThaoTacService
    {
        IAuthenticationService _authenticationService;
        IRepository<TNK.Core.Domain.LichSuThaoTac> _LichSuThaoTacRepository;
        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;

        public LichSuThaoTacService(IAuthenticationService _authenticationService,
                                IRepository<TNK.Core.Domain.LichSuThaoTac> _LichSuThaoTacRepository,
                                IDbContext _dbContext,
                                ICacheManager _cacheManager,
                                ILogger _log)
        {
            this._authenticationService = _authenticationService;
            this._LichSuThaoTacRepository = _LichSuThaoTacRepository;
            this._dbContext = _dbContext;
            this._cacheManager = _cacheManager;
            this._log = _log;
        }

        // lay danh sach lich su thao tac 
        public List<TNK.Core.Domain.View.ViewGetListLichSuThaoTac> GetLichSuThaoTac(Guid CreatedBy, DateTime FromDate, DateTime ToDate, string TuKhoa, ref int total, int pageSize,int p,string action)
        {
            try
            {
                SqlParameter createdBy = new SqlParameter("CreatedBy", CreatedBy);
                SqlParameter fromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter toDate = new SqlParameter("ToDate", ToDate);
                SqlParameter tuKhoa = new SqlParameter("TuKhoa", TuKhoa);
                SqlParameter paction = new SqlParameter("action", action);
                var list =  _dbContext.ExecuteStoredProcedureList<TNK.Core.Domain.View.ViewGetListLichSuThaoTac>("GetListLichSuThaoTac", createdBy, fromDate, toDate, tuKhoa, paction).ToList();
                total = list.Count;
                return list.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.GetLichSuThaoTac:(" + CreatedBy + "," + FromDate + "," + ToDate + "," + TuKhoa + ") " + ex.Message.ToString());
                return null;
            }
        }
        public List<TNK.Core.Domain.LichSuThayDoiTuiDinhKhoan> getListLichSuTuiDinhKhoan(DateTime fromDate,DateTime toDate,string query,int p)
        {
            try
            {
                SqlParameter FromDate = new SqlParameter("fromDate", fromDate);
                SqlParameter ToDate = new SqlParameter("toDate", toDate);
                SqlParameter Query = new SqlParameter("query", query);
                return _dbContext.ExecuteStoredProcedureList<TNK.Core.Domain.LichSuThayDoiTuiDinhKhoan>("getListChiTiet", FromDate, ToDate, Query).ToList();

            }
            catch(Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.getListLichSuTuiDinhKhoan:(" + fromDate + "," + toDate + "," + query + "," + p + ") " + ex.Message.ToString());
                return null;
            }
        }
        public List<TNK.Core.Domain.LichSuThayDoiTuiDinhKhoan> getLichSuTuiTien (DateTime fromDate, DateTime toDate, string maTui, int p)
        {
            try
            {
                SqlParameter FromDate = new SqlParameter("fromDate", fromDate);
                SqlParameter ToDate = new SqlParameter("toDate", toDate);
                SqlParameter MaTui = new SqlParameter("query", maTui);
                return _dbContext.ExecuteStoredProcedureList<TNK.Core.Domain.LichSuThayDoiTuiDinhKhoan>("getLichSuTuiTien", FromDate, ToDate, MaTui).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.getLichSuTuiTien:(" + fromDate + "," + toDate + "," + maTui + "," + p + ") " + ex.Message.ToString());
                return null;
            }
        }
        public List<TNK.Core.Domain.DataHistory> getHistoryChange(DateTime fromDate, DateTime toDate, string query, int p)
        {
            try
            {
                SqlParameter FromDate = new SqlParameter("fromDate", fromDate);
                SqlParameter ToDate = new SqlParameter("toDate", toDate);
                SqlParameter Query = new SqlParameter("query", query);
                var lst =  _dbContext.ExecuteStoredProcedureList<TNK.Core.Domain.DataHistory>("getHistoryChange",FromDate,ToDate,Query).ToList();
                return lst;
            }
            catch(Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.getHistoryChange :(" + fromDate + ", " + toDate + ", " + query + ", " + p + ")" ,ex) ;
                return null;
            }
        }
        public List<TNK.Core.Domain.LichSuThaoTac> GetLichSuThaoTac(string sessionId)
        {
            return _LichSuThaoTacRepository.Table.Where(x => x.SessionId == sessionId).ToList();
        }

        public DataTable GetThayDoiTuiTtien(string sessionId)
        {
            try
            {
                SqlParameter pSessionId = new SqlParameter("sessionId", sessionId);
                pSessionId.Size = 100;             
                DataTable tbl = _dbContext.ExecuteStoredProcedureDataTable("sp_GetThayDoiTuiTien", pSessionId);
                tbl.TableName = "TuiDinhKhoan";

               
                tbl.Columns.Add("ThayDoi");
                if (tbl.Rows.Count < 2)
                    tbl.Rows.Add(tbl.Rows[0].ItemArray);

             
                DataTable tblBefore = TNK.Core.Common.LoadDataSetFromXMLString("<TuiDinhKhoan>" + tbl.Rows[1]["XmlValue"].ToString() + "</TuiDinhKhoan>").Tables[0];
                DataTable tblAfter = TNK.Core.Common.LoadDataSetFromXMLString("<TuiDinhKhoan>" + tbl.Rows[0]["XmlValue"].ToString() + "</TuiDinhKhoan>").Tables[0];

                tblBefore.Columns.Add("ThayDoi");
                tblAfter.Columns.Add("ThayDoi");
                for (int i = 0; i < tblAfter.Rows.Count; i++)
                {
                    DataRow rAfter = tblAfter.Rows[i];
                    DataRow[] rBefore = tblBefore.Select("MaTui = '" + rAfter["MaTui"] + "'");
                    if (rBefore.Length > 0)
                        rAfter["SoTienDangCoLanTruoc"] = rBefore[0]["SoTienDangCo"];
                    else
                        rAfter["SoTienDangCoLanTruoc"] = "0";
                    //if (rAfter["SoTienDangCoLanTruoc"].ToString() != rAfter["SoTienDangCo"].ToString())
                    //    rAfter["ThayDoi"] = "True";
                    if (TNK.Core.Common.IsNumeric(rAfter["SoTienDangCoLanTruoc"])
                        && TNK.Core.Common.IsNumeric(rAfter["SoTienDangCo"])
                        )
                        rAfter["ThayDoi"] = double.Parse(rAfter["SoTienDangCo"].ToString()) - double.Parse(rAfter["SoTienDangCoLanTruoc"].ToString());
                    else
                        rAfter["ThayDoi"] = 0;

                }

                             
                for (int i = tblAfter.Columns.Count-1;i>=0;i--) 
                {
                    DataColumn col = tblAfter.Columns[i];
                    if (!
                        ((col.ColumnName.ToUpper() == "MATUI")
                        || (col.ColumnName.ToUpper() == "MATUICHA")
                        || (col.ColumnName.ToUpper() == "TENTUI")
                        || (col.ColumnName.ToUpper() == "SOTIENDANGCOLANTRUOC")
                        || (col.ColumnName.ToUpper() == "SOTIENDANGCO")
                        || (col.ColumnName.ToUpper() == "THAYDOI")
                        )
                       )
                        tblAfter.Columns.Remove(col);
                }
                
                tblAfter.DefaultView.Sort = "ThayDoi desc,MaTuiCha,MaTui";

                tblAfter.TableName = "TuiDinhKhoan";
                return tblAfter.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.GetThayDoiTuiTtien :(" + sessionId + ")" , ex);
                return null;
            }
        }

        public void WriteLichSuThaoTac(string sessionId, string IPClient, string HostNameClient, string MaPhieu, string LoaiPhieu, string HanhDong, string GhiChu, string ChiTiet, Guid CreatedBy)
        {
            try
            {
                SqlParameter pSessionId = new SqlParameter("sessionId", sessionId);
                SqlParameter pIPClient = new SqlParameter("IPClient", IPClient);
                SqlParameter pHostNameClient = new SqlParameter("HostNameClient", HostNameClient);
                SqlParameter pMaPhieu = new SqlParameter("MaPhieu", MaPhieu == null? "": MaPhieu);
                SqlParameter pLoaiPhieu = new SqlParameter("LoaiPhieu", LoaiPhieu == null ? "" : LoaiPhieu);
                SqlParameter pHanhDong = new SqlParameter("HanhDong", HanhDong);
                SqlParameter pGhiChu = new SqlParameter("GhiChu", GhiChu);
                SqlParameter pChiTiet = new SqlParameter("ChiTiet", ChiTiet);
                SqlParameter pCreatedBy = new SqlParameter("CreatedBy", CreatedBy);
                _dbContext.ExecuteStoredProcedure("sp_WriteLichSuThaoTac"
                    , pSessionId
                    , pIPClient
                    , pHostNameClient
                    , pMaPhieu
                    , pLoaiPhieu
                    , pHanhDong
                    , pGhiChu
                    , pChiTiet
                    , pCreatedBy
                    );
            }
            catch (Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.WriteLichSuThaoTac", ex);
                
            }
            
        }
        public DataTable LichSuThayDoiTuiTien(DateTime FromDate, DateTime ToDate, string MaTuiTien,int p)
        {
            try
            {
                int i = 0;
                int dem = (20 * p) - 1;
                SqlParameter pfromDate = new SqlParameter("from", FromDate);
                SqlParameter pToDate = new SqlParameter("to", ToDate);
                SqlParameter pMaTuiTien = new SqlParameter("maTui", MaTuiTien);
                DataTable db = new DataTable();
                db.Columns.Add("SessionId", typeof(string));
                db.Columns.Add("MaPhieu", typeof(string));
                db.Columns.Add("MaTui", typeof(string));
                db.Columns.Add("SoTienThayDoi", typeof(double));
                db.Columns.Add("CreatedDate", typeof(DateTime));
                db.Columns.Add("IPClient", typeof(string));
                DataTable tbl = _dbContext.ExecuteStoredProcedureDataTable("sp_LichSuThayDoiTuiDinhKhoan", pfromDate, pToDate, pMaTuiTien);
                if(tbl.Rows.Count > 0)
                {
                    for (i = (p - 1) * 20; i <= dem; i++)
                    {

                        DataRow dr = db.NewRow();
                        dr["SessionId"] = tbl.Rows[i]["SessionId"];
                        dr["MaPhieu"] = tbl.Rows[i]["MaPhieu"];
                        dr["MaTui"] = tbl.Rows[i]["MaTui"];
                        dr["SoTienThayDoi"] = tbl.Rows[i]["SoTienThayDoi"];
                        dr["CreatedDate"] = tbl.Rows[i]["CreatedDate"];
                        dr["IPClient"] = tbl.Rows[i]["IPClient"];
                        db.Rows.Add(dr);
                    }
                }
                
                return db;
            }
            catch (Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.LichSuThayDoiTuiTien", ex);
                return null;
            }
        }


        public DataTable getLichSuThayDoiTuiTien(DateTime FromDate, DateTime ToDate, string MaTuiTien)
        {
            try
            {
                SqlParameter pfromDate = new SqlParameter("from", FromDate);
                SqlParameter pToDate = new SqlParameter("to", ToDate);
                SqlParameter pMaTuiTien = new SqlParameter("maTui", MaTuiTien);
                DataTable tbl = _dbContext.ExecuteStoredProcedureDataTable("sp_LichSuThayDoiTuiDinhKhoan", pfromDate, pToDate, pMaTuiTien);
                return tbl;
            }
            catch (Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.getLichSuThayDoiTuiTien", ex);
                return null;
            }
        }
        public TNK.Core.Domain.LichSuThaoTac GetLSTTGNY(string MaPhieu)
        {
            try
            {
                return _LichSuThaoTacRepository.Table.Where(x => x.MaPhieu == MaPhieu && x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.WriteLog("LichSuThaoTacService.GetLSTTGNY", ex);
                return null;
            }
        }
    }
}
