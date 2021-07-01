using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface ICustMasterService
    {
        CustMaster GetCustMasterDetails(CustMaster objCustMaster);
        CustMaster GetCustHdrDetails(CustMaster objCustMaster);
        CustMaster GetCustConfigDetails(CustMaster objCustMaster);
        CustMaster GetDftWhs(CustMaster objCustMaster);
        CustMaster GetDftCmpWhs(CustMaster objCustMaster);
        void SaveCustMaster(CustMaster objCustMaster);
        void SaveCustMasterConfig(CustMaster objCustMaster);
        void SaveCmpHdr(CustMaster objCustMaster);
        void UpdateCmpHdr(CustMaster objCustMaster);
        void UpdateCustMasterDtl(CustMaster objCustMaster);
        CustMaster GetCustDetails(CustMaster objCustMaster);
        void DeleteCmpHdr(CustMaster objCustMaster);
        void SaveWhsMaster(CustMaster objCustMaster);
        void UpdateCustMasterConfigDir(CustMaster objCustMaster);

        void SaveCustMasterConfigDir(CustMaster objCustMaster);
        void SaveCustMasterDtl(CustMaster objCustMaster);
        void UpdateCustMasterConfig(CustMaster objCustMaster);
        void DeleteCust(CustMaster objCustMaster);
        void UpdateCustMaster(CustMaster objCustMaster);
        void SaveUserXCmp(CustMaster objCustMaster);
        CustMaster GetTableEntityValueCount(CustMaster objCustMaster);
        void InsertTableEntityValueByCust(CustMaster objCustMaster);
        CustMaster GetTableEntityValueCountByRMA_DocId(CustMaster objCustMaster);
        CustMaster GetCheckExistCmpId(CustMaster objCustMaster);
        CustMaster GetCustomerLogo(CustMaster objCustMaster);
        CustMaster CheckCustomerExist(CustMaster objCustMaster);
        CustMaster GetCustMasterRptDetails(CustMaster objCustMaster);

    }
}
