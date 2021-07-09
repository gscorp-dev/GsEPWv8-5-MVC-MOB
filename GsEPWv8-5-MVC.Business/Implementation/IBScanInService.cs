using GsEPWv8_4_MVC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_4_MVC.Data.Interface;
using GsEPWv8_4_MVC.Data.Implementation;

namespace GsEPWv8_4_MVC.Business.Implementation
{
    public class IBScanInService : IIBScanInService
    {
        IScanInRepository ObjScanInRepository = new ScanInRepository();
        public InboundInquiry GetInboundHdrDtl(InboundInquiry objInboundInquiry)
        {
            return ObjScanInRepository.GetInboundHdrDtl(objInboundInquiry);
        }

        public List<ItemScanIN> getScanInDetailsByItemCode(string cmpId, string itm_code, string itm_serial_num)
        {
            return ObjScanInRepository.getScanInDetailsByItemCode(cmpId, itm_code, itm_serial_num);
        }
        public void DeleteScanInDetails(InboundInquiry objInboundInquiry)
        {
            ObjScanInRepository.DeleteScanInDetails(objInboundInquiry);
        }
        public void InsertScanInDetails(InboundInquiry objInboundInquiry)
        {
            ObjScanInRepository.InsertScanInDetails(objInboundInquiry);
        }

        public void EditScanInDetails(InboundInquiry objInboundInquiry)
        {
            ObjScanInRepository.EditScanInDetails(objInboundInquiry);
        }
    }
}
