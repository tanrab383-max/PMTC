using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core
{
    public class StoreService
    {
        private static readonly Dictionary<string, string> StoreConnectionStrings = new Dictionary<string, string>
        {
            { "HLAN", "Data Source=172.165.0.20;Initial Catalog=PMTC_HLAN;User ID=sa_tnk;Password=Sv@2015#;Connection Timeout=90000" },
            { "HPTN", "Data Source=172.165.0.20;Initial Catalog=PMTC_HPTN;User ID=sa_tnk;Password=Sv@2015#;Connection Timeout=90000" },
            { "HTNH", "Data Source=172.165.0.20;Initial Catalog=PMTC_HTNH;User ID=sa_tnk;Password=Sv@2015#;Connection Timeout=90000" },
            { "HBHA", "Data Source=172.165.0.20;Initial Catalog=PMTC_HBHA;User ID=sa_tnk;Password=Sv@2015#;Connection Timeout=90000" },
            { "TTG", "Data Source=172.165.0.20;Initial Catalog=PMTC_TTG;User ID=sa_tnk;Password=Sv@2015#;Connection Timeout=90000" },
            { "TBTR", "Data Source=172.165.0.20;Initial Catalog=PMTC_TBTR;User ID=sa_tnk;Password=Sv@2015#;Connection Timeout=90000" },
            { "TNK", "Data Source=115.75.99.65;Initial Catalog=PMTC_TNK;User ID=tnk;Password=Tnk@2019Toyota;Connection Timeout=90000" },
            { "MVL", "Data Source=172.165.0.20;Initial Catalog=PMTC_MVL;User ID=sa_tnk;Password=Sv@2015#;Connection Timeout=90000" },
            { "DEMO", "Data Source=172.165.0.20;Initial Catalog=PMTC_DEMO2;User ID=sa_tnk;Password=Sv@2015#;Connection Timeout=90000" },
        };

        public static readonly Dictionary<string, string> DealerList = new Dictionary<string, string>
        {
            { "HBHA", "Honda Oto Biên Hòa" },
            { "HPTN", "Honda Oto Phát Tiến" },
            { "HLAN", "Honda Oto Long An" },
            { "HTNH", "Honda Oto Tây Ninh" },
            { "TNK", "Toyota Ninh Kiều" },
            { "TTG", "Toyota Tiền Giang" },
            { "TBTR", "Toyota Bến Tre" },            
            { "MVL", "Mitsubishi Vĩnh Long" },
            { "DEMO", "Demo" },
        };

        public static Dictionary<string, string> GetDealerByDomain(string domainName)
        {
            domainName = domainName.ToLower();
            string dealerId = "";

            dealerId = GetDealerIdByDomain(domainName);
            if (!string.IsNullOrEmpty(dealerId))
            {
                return DealerList.Where(x => x.Key == dealerId).ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                return DealerList;
            }

        }
        public static string GetDealerIdByDomain(string domainName)
        {
            Dictionary<string, string> lstDomain = new Dictionary<string, string>
            {
                { "pmtc-hlan.phattien.net", "HLAN" },
                { "pmtc-hptn.phattien.net", "HPTN" },
                { "pmtc-htnh.phattien.net","HTNH" },
                { "pmtc-hbha.phattien.net","HBHA"},
                { "pmtc-ttg.phattien.net","TTG" },
                { "pmtc-tbtr.phattien.net", "TBTR"},
                { "pmtc-tnk.phattien.net","TNK" },
                { "pmtc-mvl.phattien.net","MVL"},
                { "pmtc-demo.phattien.net","DEMO" },
            };
            domainName = domainName.ToLower();            
            if(lstDomain.ContainsKey(domainName))
                return lstDomain[domainName];            
            return "";
        }



        private const string DefaultConnectionString = "Data Source=172.165.0.20;Initial Catalog=PMTC_DEMO2;User ID=sa_tnk;Password=Sv@2015#";
        public string GetConnectionStringForStore(string storeId)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return DefaultConnectionString;
            }

            string connectionString;
            if (StoreConnectionStrings.TryGetValue(storeId, out connectionString))
            {
                return connectionString;
            }

            throw new KeyNotFoundException($"Chuỗi kết nối DB '{storeId}' không tìm thấy.");
        }

    }
}
