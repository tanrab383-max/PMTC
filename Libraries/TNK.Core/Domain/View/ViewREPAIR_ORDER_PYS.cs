using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewREPAIR_ORDER_PYS : BaseEntity
    {
        public string REPAIRORDERNO { get; set; }
        public string CVDV_NAME { get; set; }
        public int? CVDV_ID { get; set; }
        public DateTime? RO_DATE { get; set; }
        public string CAROWNERNAME { get; set; }
        public string CAROWNERADD { get; set; }
        public string DRIVERNAME { get; set; }
        public int? CUSTOMER_ID { get; set; }
        public string VEHICLE_TYPE { get; set; }
        public string REGISTERNO { get; set; }
        public string CM_CODE { get; set; }
        public string FRAMENO { get; set; }
        public string WORK_TYPE { get; set; }
        public string WTCODE { get; set; }
        public string JOBCODE { get; set; }
        public string JOBSNAME { get; set; }
        public string DESCRIPTIONS { get; set; }
        public int? QUANTITY { get; set; }
        public string UNIT_NAME { get; set; }
        public double COST { get; set; }
        public double DISCOUNT { get; set; }
        public int? TAX_AMOUNT5 { get; set; }
        public int? TAX_AMOUNT10 { get; set; }
        public int? LINETOTAL { get; set; }
        public int? DETAIL_ID { get; set; }
        public string IS_GET { get; set; }
        public string ORGNAME { get; set; }
        public string CAROWNERTEL { get; set; }
        public string CAROWNERFAX { get; set; }
        public string INR_COM_NAME { get; set; }
        public string EMP_TEL { get; set; }
        public string V_C_CODE { get; set; }
        public string KM { get; set; }
        public DateTime? DELIVERY_DATE { get; set; }
        public DateTime? MEETCUS { get; set; }
        public DateTime? CLOSERO_DATE { get; set; }
        public DateTime? TRANS_DATE { get; set; }
        public int? DLR_ID { get; set; }
        public string VIN { get; set; }
        public string CF_TYPE { get; set; }
        public string CM_NAME { get; set; }
        public string FULL_MODEL { get; set; }
        public string DRIVER_TEL { get; set; }
        public int? KM_PREVIOUS { get; set; }
        public DateTime? MAX_CREATE_DATE { get; set; }
        public string REQUEST_DESC { get; set; }
        public string NOTES { get; set; }
        public string EMP_CODE { get; set; }
        public DateTime? OPENRO_DATE { get; set; }
        public string TAXCODE { get; set; }
        public string ROTYPE { get; set; }
        public string IS_MA { get; set; }
        public int? NEXT_KM_MAINTENANCE { get; set; }
        public DateTime? SHIPDATE { get; set; }
        public string ENGINE_NO { get; set; }
        public string DISCOUNT_TYPE { get; set; }
        public string ROTYPE_DETAIL { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string GENUINE { get; set; }
        public string WORK_TYPE_DETAIL { get; set; }
        public string KPI_PART_TYPE { get; set; }
      
    }
}
