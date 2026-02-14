using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.Log
{
    public interface ILogger
    {
        void WriteLog(string message);
        void WriteLog(string action, object message);
        void WriteLogAccessTime(string controller, string action, DateTime startTime, DateTime endTime, string error,string url);
    }
    
}
