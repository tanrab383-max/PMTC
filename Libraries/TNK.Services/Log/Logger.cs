using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Data;
using TNK.Services.Authentication;

namespace TNK.Services.Log
{
    public class Logger:ILogger
    {
        IAuthenticationService _authenticationService;
        IRepository<Logging> _logRepository;
        IDbContext _dbContext;
        
        public Logger(IRepository<Logging> _logRepository, IAuthenticationService _authenticationService, IDbContext _dbContext)
        {
            this._logRepository = _logRepository;
            this._authenticationService = _authenticationService;
            this._dbContext = _dbContext;
        }
        public void WriteLogAccessTime(string controller, string action, DateTime startTime, DateTime endTime, string error,string url)
        {
            string user = "";
            try
            {
                user = _authenticationService.GetAuthenticatedUser().UserName;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
            try
            {
                SqlParameter pBeginTime = new SqlParameter("StartTime", startTime);
                SqlParameter pEndTime = new SqlParameter("EndTime", endTime);
                SqlParameter pAction = new SqlParameter("action", action);
                SqlParameter pController = new SqlParameter("Controller", controller);
                SqlParameter pError = new SqlParameter("Error", error);
                SqlParameter pCreatedBy = new SqlParameter("CreatedBy", user);
                SqlParameter pUrl = new SqlParameter("Url", url);
                _dbContext.ExecuteStoredProcedure("sp_WriteActionLog", pController, pAction, pBeginTime, pEndTime, pCreatedBy, pError, pUrl);
            }
            catch (Exception exx)
            {
                //WriteLog(exx.Message);
            }

        }

        public void WriteLog(string message)
        {
            /* Logging obj = new Logging();
             obj.ID = Guid.NewGuid();
             obj.CreatedDate = DateTime.Now;            
             obj.UserId = _authenticationService.GetAuthenticatedUser().UserId;
             obj.ErrorMessage = message;
             _logRepository.Insert(obj);
            */
            try
            {
                SqlParameter pUserid = new SqlParameter("UserId", _authenticationService.GetAuthenticatedUser()?.UserId);
                SqlParameter pMessage = new SqlParameter("Message", message);
                _dbContext.ExecuteStoredProcedure("sp_InsertLogging", pUserid, pMessage);
                
            }
            catch (Exception exx)
            {
               // WriteLog(exx.Message);
            }
        }

        public void WriteLog(string action, object message)
        {
            try
            {
                if (message is Exception)
                {
                    Exception e = (Exception)message;
                    WriteLog(action + ":" + e.ToString());
                    TNK.Core.Common.WriteLogError(action, (Exception)message);
                    if (e.InnerException != null)
                        WriteLog(action + ":" + e.InnerException);

                    if (e.StackTrace != null)
                        WriteLog(action + ":" + e.StackTrace);

                }
                else
                {
                    WriteLog(action + ":" + message.ToString());
                }
            }
            catch (Exception ex)
            { 
            
            }
        }

    }
}
