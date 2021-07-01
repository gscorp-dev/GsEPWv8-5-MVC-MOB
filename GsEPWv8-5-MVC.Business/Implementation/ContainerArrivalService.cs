using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class ContainerArrivalServiceService : IContainerArrivalService
    {
        IContainerArrivalRepository objRepository = new ContainerArrivalRepository();
        public StockChange GetStockChangeDetails(StockChange objStockChange)
        {
            return objRepository.GetStockChangeDetails(objStockChange);
        }

        public ContainerArrival GetContainerArrivalDetails(ContainerArrival objContainerArrival)
        {
            return objRepository.GetContainerArrivalDetails(objContainerArrival);
        }

        public IList<CarrierHdr> GetCarrierDetails(ContainerArrival objContainerArrival)
        {
            return objRepository.GetCarrierDetails(objContainerArrival);
        }

        public string GetCarrierEmailDetails(string p_str_carrier_id,string p_str_cmp_id)
        {
            return objRepository.GetCarrierEmailDetails(p_str_carrier_id, p_str_cmp_id);
        }
        public IList<Email> GetCMBbandCarrierEMailDetails(string p_str_cmp_id)
        {
            return objRepository.GetCMBbandCarrierEMailDetails( p_str_cmp_id);
        }
        public string GetWhsCode(string p_str_cmp_id)
        {
            return objRepository.GetWhsCode(p_str_cmp_id);
        }
        public StockChange GetItemMoveGridLoadItem(StockChange objStockChange)
        {
            return objRepository.GetItemMoveGridLoadItem(objStockChange);
        }
        public StockChange GetItemMoveTotQty(StockChange objStockChange)
        {
            return objRepository.GetItemMoveTotQty(objStockChange);
        }
        public StockChange CheckLotStatus(StockChange objStockChange)
        {
            return objRepository.CheckLotStatus(objStockChange);
        }
        public StockChange GetAdjustDocID(StockChange objStockChange)
        {
            return objRepository.GetAdjustDocID(objStockChange);
        }
        public StockChange GetPalletId(StockChange objStockChange)
        {
            return objRepository.GetPalletId(objStockChange);
        }
        public StockChange Ck_Loc_Itm_Avail(StockChange objStockChange)
        {
            return objRepository.Ck_Loc_Itm_Avail(objStockChange);
        }
        public StockChange Update_Rcvd_Qty(StockChange objStockChange)
        {
            return objRepository.Update_Rcvd_Qty(objStockChange);
        }
        public StockChange Add_Iv_Itm_Trn_Hdr(StockChange objStockChange)
        {
            return objRepository.Add_Iv_Itm_Trn_Hdr(objStockChange);
        }
        public StockChange UPD_TRNIN_TRNHST_PART_TPW(StockChange objStockChange)
        {
            return objRepository.UPD_TRNIN_TRNHST_PART_TPW(objStockChange);
        }
        public StockChange UPD_LOTDTL_STK_XFER_PART_TPW(StockChange objStockChange)
        {
            return objRepository.UPD_LOTDTL_STK_XFER_PART_TPW(objStockChange);
        }
        public StockChange Validate_ItmCode(StockChange objStockChange)
        {
            return objRepository.Validate_ItmCode(objStockChange);
        }
        public StockChange InsertTempStkMove(StockChange objStockChange)
        {
            return objRepository.InsertTempStkMove(objStockChange);
        }
        public StockChange ItemXGetLocDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetLocDetails(term, cmp_id);
        }
        public StockChange ItemXGetitmDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetitmDetails(term, cmp_id);
        }
        //public StockChange SaveStkMove(StockChange objStockChange)
        //{
        //    return objRepository.SaveStkMove(objStockChange);
        //}
        public StockChange UpdateTempStkMove(StockChange objStockChange)
        {
            return objRepository.UpdateTempStkMove(objStockChange);
        }

        public StockChange GetItemCode(StockChange objStockChange)
        {
            return objRepository.GetItemCode(objStockChange);
        }

          public string SaveStkMove(string p_str_cmp_id, string p_str_user_id, DataTable p_dt_item_stock_move)
        {
            return objRepository.SaveStkMove(p_str_cmp_id, p_str_user_id, p_dt_item_stock_move);
        }
        public string SaveContainerArrival(string p_str_cmp_id, string p_str_doc_id, string p_str_adj_dt, string p_str_carrier_id, string p_str_whs_id, string p_str_type, string p_str_cont_id, string p_str_event, string p_str_Notes)
        {
            return objRepository.SaveContainerArrival(p_str_cmp_id,  p_str_doc_id,  p_str_adj_dt,  p_str_carrier_id,  p_str_whs_id,  p_str_type,  p_str_cont_id, p_str_event,p_str_Notes);
        }
    }
}
