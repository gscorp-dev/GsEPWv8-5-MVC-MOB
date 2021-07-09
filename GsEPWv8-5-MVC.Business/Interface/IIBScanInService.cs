using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_4_MVC.Business.Interface
{
    public interface IIBScanInService
    {
        InboundInquiry GetInboundHdrDtl(InboundInquiry objInboundInquiry);
        void InsertScanInDetails(InboundInquiry objInboundInquiry);
        List<ItemScanIN> getScanInDetailsByItemCode(string cmpId, string itm_code, string itm_serial_num);
        void DeleteScanInDetails(InboundInquiry objInboundInquiry);
        void EditScanInDetails(InboundInquiry objInboundInquiry);
    }
}
