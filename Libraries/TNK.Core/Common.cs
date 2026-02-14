using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Net;
using System.Data;
using System.Xml;
using System.IO;
using AF.Library;

namespace TNK.Core
{
    public class Common
    {

        public static void WriteLogError(string strClassName,Exception ex)
        {
            try
            {
                AF.Library.Logger _log4Net = new Logger(strClassName);
                 _log4Net.Error(ex);
                if (ex.StackTrace != null)
                    _log4Net.Error(ex.StackTrace);
                if (ex.InnerException != null)
                    _log4Net.Error(ex.InnerException);
            }
            catch (Exception exx)
            { 
            }
        }
        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static bool IsDate(object Expression)
        {
            DateTime dt;
            bool isDate = true;
            isDate = DateTime.TryParse(Expression.ToString(), out dt);
            return isDate;
        }
        /// <summary>
        /// Kiem tra chuỗi có phải là định dạng ngày dd/MM/yyyy không
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsVNDate(object Expression)
        {
            DateTime dt = ToVNDate(Expression.ToString());
            if (dt.Year > 1970)
                return true;
            return false;
        }
        /// <summary>
        /// Neu dung dinh dang thi tra ve ngay, neu khong dung dinh dang thi tra ve 1970-01-01
        /// </summary>
        /// <param name="date">theo dinh dang dd/mm/yyyy</param>
        /// <returns></returns>
        public static DateTime ToVNDate(string date)
        {
            DateTime dt = new DateTime(1970, 1, 1);
            if (date.Length >= 10)
            {
                date = date.Substring(0, 10);
                string[] ls = date.Split('/');
                if (ls.Length >= 3)
                {
                    string sDay = ls[0];
                    string sMonth = ls[1];
                    string sYear = ls[2];

                    if (!IsNumeric(sDay) || !IsNumeric(sMonth) || !IsNumeric(sYear))
                        return dt;
                    try
                    {
                        dt = new DateTime(int.Parse(sYear), int.Parse(sMonth), int.Parse(sDay));
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        return dt;
                    }

                }
                else
                {
                    return dt;
                }
            }
            return dt;
        }

        public static string ToCurrencyString(double val)
        {
            return val.ToString("N0", CultureInfo.CreateSpecificCulture("en-US"));
        }

        public static string ToCurrencyString_New(string val)
        {
            double val1 = Convert.ToDouble(val);
            return val1.ToString("N0", CultureInfo.CreateSpecificCulture("en-US"));
        }
        public static string ConvertListToXMLString(Array lst)
        {
            string xml = "<data>";
            foreach (object obj in lst)
            {
                xml += ConvertObjectToXMLString(obj);
            }
            xml = xml + "</data>";
            return xml;
        }
        /// <summary>
        /// Chuyen object sang chuoi XML
        /// </summary>
        /// <returns></returns>
        public static string ConvertObjectToXMLString(object obj)
        {
            string xml = Environment.NewLine;
            if (obj == null)
                return "";
            if (
                obj.GetType() == typeof(String)
                || obj.GetType() == typeof(int)
                || obj.GetType() == typeof(double)
                || obj.GetType() == typeof(float)
                || obj.GetType() == typeof(DateTime)
                )
                return "<data>" + obj.ToString() + "</data>";
            if (!IsList(obj))
            {
                List<PropertyInfo> lstInfo = obj.GetType().GetRuntimeProperties().ToList().OrderBy(x => x.Name).ToList();
                xml = Environment.NewLine + "<" + obj.GetType().Name + ">";
                foreach (PropertyInfo info in lstInfo)
                {
                    object val = info.GetValue(obj);
                    if (val != null)
                    {
                        xml += Environment.NewLine + "<" + info.Name + ">";
                        xml += val;
                        xml += "</" + info.Name + ">";
                    }
                }
                xml += Environment.NewLine + "</" + obj.GetType().Name + ">";
                //var stringwriter = new System.IO.StringWriter();
                //var serializer = new XmlSerializer(valueType);
                //serializer.Serialize(stringwriter, obj);
                //xml = stringwriter.ToString();                
            }
            //else
            //{                

            //    foreach (object o in (List<object>) obj)
            //    {
            //        xml += ConvertObjectToXMLString(o);
            //    }                
            //}           
            return xml;
        }
        /// <summary>
        /// Chuyen chuoi XML sang object
        /// </summary>
        /// <param name="xmlText"></param>
        /// <param name="obj"></param>
        public static void LoadFromXMLString(string xmlText, ref object obj)
        {
            var stringReader = new System.IO.StringReader(xmlText);
            var serializer = new XmlSerializer(obj.GetType());
            obj = serializer.Deserialize(stringReader) as object;
        }

        /// <summary>
        /// Chuyen chuoi XML sang object
        /// </summary>
        /// <param name="xmlText"></param>
        /// <param name="obj"></param>
        public static DataSet LoadDataSetFromXMLString(string xmlText)
        {
            DataSet ds = new DataSet();
            try
            {

                ds.ReadXml(new XmlTextReader(new StringReader(xmlText)));
                return ds;
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public static string DataTableToHtmlVertical(DataTable tbl, string tableClass = "", string thClass = "", string tdClass = "")
        {
            try
            {
                string html = "";
                string header = "";
                string body = "";
                for (int i = 0; i < tbl.Columns.Count; i++)
                    header += "<th class ='" + thClass + "'>" + tbl.Columns[i].ColumnName + "</th>" + Environment.NewLine;
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    body += "<tr>";
                    for (int j = 0; j < tbl.Columns.Count; j++)
                    {
                        string value = tbl.Rows[i][j].ToString();
                        if (tbl.Columns[j].DataType == typeof(float)
                            || tbl.Columns[j].DataType == typeof(double)
                            )
                            value = ToCurrencyString(double.Parse(tbl.Rows[i][j].ToString()));
                        else if (tbl.Columns[j].DataType == typeof(DateTime))
                            value = DateTime.Parse(tbl.Rows[i][j].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        body += "<td class ='" + tdClass + "'>" + value + "</td>" + Environment.NewLine;
                    }
                    body += "</tr>";
                    body += Environment.NewLine;
                }
                html = "<table class ='" + tableClass + "'>" + Environment.NewLine
                       + header
                       + Environment.NewLine
                       + body
                       + "</table>"
                       ;
                return html;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string DataTableToHtmlHorizontal(DataTable tbl, string title = "", string tableClass = "", string thClass = "", string tdClass = "")
        {
            try
            {
                string html = "";
                string body = "";
                if (!string.IsNullOrEmpty(title))
                    title = "<tr> <th style='text-align:center;text-transform: uppercase;' class ='" + thClass + "' colspan = '" + (tbl.Rows.Count + 1) + "'>" + title + "</th> </tr>";
                for (int i = 0; i < tbl.Columns.Count; i++)
                {
                    body += "<tr>";
                    body += "<td> <b>" + tbl.Columns[i].ColumnName + "</b></td>";
                    for (int j = 0; j < tbl.Rows.Count; j++)
                    {
                        string value = tbl.Rows[j][i].ToString();
                        if (tbl.Columns[i].DataType == typeof(float)
                            || tbl.Columns[i].DataType == typeof(double)
                            )
                            value = ToCurrencyString(double.Parse(tbl.Rows[j][i].ToString()));
                        else if (tbl.Columns[i].DataType == typeof(DateTime))
                            value = DateTime.Parse(tbl.Rows[j][i].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                        body += "<td class ='" + tdClass + "'>" + value + "</td>" + Environment.NewLine;
                    }
                    body += "</tr>";
                    body += Environment.NewLine;
                }
                html = "<table class ='" + tableClass + "'>"
                       + Environment.NewLine
                       + title
                       + Environment.NewLine
                       + body
                       + "</table>"
                       ;
                return html;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Get all the properties
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var prop in properties)
            {
                // Define the type of the data column, to be nullable if the property is nullable
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    ? Nullable.GetUnderlyingType(prop.PropertyType)
                    : prop.PropertyType;

                // Add columns to the DataTable
                dataTable.Columns.Add(prop.Name, type);
            }

            foreach (var item in items)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public static string EncodeHtmlUrl(string url)
        {
            url = url.Replace(".","11____11");
            url = url.Replace("&", "22____22");
            url = url.Replace("$", "33____33");
            url = url.Replace("/", "44____44");
            return url;
        }

        public static string DecodeHtmlUrl(string url)
        {
            url = url.Replace("11____11", ".");
            url = url.Replace("22____22", "&");
            url = url.Replace("33____33", "$");
            url = url.Replace("44____44", "/");
            //url = url.Replace(".", "11____11");
            return url;
        }
    }
}

