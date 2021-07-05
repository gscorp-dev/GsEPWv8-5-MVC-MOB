using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
   public class ItemScanIN : BasicEntity
    {
        public string cmp_id { get; set; }
        public string itm_code { get; set; }
        public string itm_serial_num { get; set; }
        public string itm_serial_num_exist { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string status { get; set; }
        public string ib_doc_id { get; set; }
        public DateTime? ib_doc_dt { get; set; }
        public string ob_doc_id { get; set; }
        public DateTime? ob_doc_dt { get; set; }
        public string ib_user { get; set; }
        public string ob_user { get; set; }
        public string itm_name { get; set; }
        public string ppk { get; set; }
        public string ctn { get; set; }
        public string TotalQty { get; set; }

        public int balanceScan { get; set; }
    }
}
