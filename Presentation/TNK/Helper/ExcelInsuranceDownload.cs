using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TNK.Helper
{
    public class ExcelInsuranceDownload : ActionResult
    {
        public ExcelInsuranceDownload()
        {
        }

        public ExcelInsuranceDownload(ExcelPackage pck, string filename)
        {
            _pck = pck;
            _fileName = filename;
        }
        private ExcelPackage _pck
        {
            get;
            set;
        }
        private string _fileName
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpCookie cookie = new HttpCookie("fileDownload");
            cookie.Value = "true";
            cookie.Expires = DateTime.Now.AddMinutes(30);
            context.HttpContext.Response.Cookies.Add(cookie);
            context.HttpContext.Response.BinaryWrite(_pck.GetAsByteArray());
            context.HttpContext.Response.AddHeader("content-disposition", "attachment;  filename=" + _fileName);
            context.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        }
    }
}