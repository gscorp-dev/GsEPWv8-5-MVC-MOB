using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GsEPWv8_5_MVC.Common;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Business.Implementation;

namespace GsEPWv8_4_MVC.Controllers
{
    public class InBoundScanInController : Controller
    {
        DataTable dtStockInq;
        int AvlCtn = 0;
        int AvlQty = 0;

        int AlocQty = 0;
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string Folderpath = string.Empty;
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
        public ActionResult DashboardInBoundScanIn(string FullFillType, string cmp, string screentitle)
        {
            string l_str_cmp_id = string.Empty;
            string whs = string.Empty;
            try
            {
                InBoundScanHeader objContainerArrival = new InBoundScanHeader();
                IContainerArrivalService ServiceObject = new ContainerArrivalServiceService();

                if (cmp == null)
                {
                    cmp = Session["g_str_cmp_id"].ToString().Trim();
                    objContainerArrival.cmp_id = cmp;
                    if (Session["g_str_whs_id"] != null)
                    {
                        whs = Session["g_str_whs_id"].ToString().Trim();
                        objContainerArrival.whs_id = whs;
                    }
                }
                else
                {
                    objContainerArrival.cmp_id = cmp;
                    if (Session["g_str_whs_id"] != null)
                    {
                        whs = Session["g_str_whs_id"].ToString().Trim();
                        objContainerArrival.whs_id = whs;
                    }

                }

                clsGlobal.AdjDocId = string.Empty;
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objContainerArrival.cmp_id != "" && FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objContainerArrival.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objContainerArrival.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }

                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);

                objContainerArrival.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                objCompany.cust_cmp_id = cmp;
                objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);

                objContainerArrival.ListLocPickDtl = objCompany.ListLocPickDtl;
                objContainerArrival.cmp_id = cmp;
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objContainerArrival.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objContainerArrival.tmp_cmp_id = objContainerArrival.cmp_id;
                objContainerArrival.p_str_company = objContainerArrival.cmp_id;
                objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);

                Mapper.CreateMap<InBoundScanHeader, InBoundScanHeaderModel>();
                InBoundScanHeaderModel objContainerArrivalModel = Mapper.Map<InBoundScanHeader, InBoundScanHeaderModel>(objContainerArrival);
                return View(objContainerArrivalModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("UserLogin", "Login");
            }
            finally
            {

            }
        }
        public JsonResult ItemXGetLocDtl(string term, string cmp_id)
        {
            StockChangeService ServiceObject = new StockChangeService();
            var List = ServiceObject.ItemXGetLocDetails(term.Trim(), cmp_id).LstItmxlocdtl.Select(x => new { label = x.loc_id, value = x.loc_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DashboardInBoundSerialGrid(string p_str_cmpid, string p_str_whsId, string p_str_contId, string p_str_event, string p_str_docid)
        {
            InBoundScanHeader objIBScanIn = new InBoundScanHeader();
            IContainerArrivalService ServiceObject = new ContainerArrivalServiceService();
            IIBScanInService ObjIBScanInService = new IBScanInService();
            objIBScanIn.cmp_id = p_str_cmpid;
            objIBScanIn.whs_id = p_str_whsId;
            objIBScanIn.cont_id = p_str_contId;
            objIBScanIn.mevent = p_str_event;
            objIBScanIn.ib_doc_id = p_str_docid;
            objIBScanIn.InboundInquiry = new InboundInquiry();
            objIBScanIn.InboundInquiry.cmp_id = p_str_cmpid;
            objIBScanIn.InboundInquiry.cont_id = p_str_contId;
            objIBScanIn.InboundInquiry.ib_doc_id = p_str_docid;
            objIBScanIn.InboundInquiry = ObjIBScanInService.GetInboundHdrDtl(objIBScanIn.InboundInquiry);
            //objContainerArrival = ServiceObject.GetContainerArrivalDetails(objContainerArrival);
            //objContainerArrival.ListGetCarrierDetails = ServiceObject.GetCarrierDetails(objContainerArrival);
            objIBScanIn.ListEamilDetail = ServiceObject.GetCMBbandCarrierEMailDetails(p_str_cmpid);


            int initInt = 0;
            string strEmaillist = "";
            for (int i = 0; i < objIBScanIn.ListEamilDetail.Count; i++)
            {
                if (objIBScanIn.ListEamilDetail.Count > 0)
                {
                    if (initInt == 0)
                    {
                        strEmaillist = objIBScanIn.ListEamilDetail[i].email;
                        strEmaillist = strEmaillist.Replace("CLIENT: ", "");
                        strEmaillist = strEmaillist.Replace("CSR: ", "");
                        strEmaillist = strEmaillist.Replace("CARRIER: ", "");
                    }
                    else
                    {
                        strEmaillist = strEmaillist + ";" + objIBScanIn.ListEamilDetail[i].email;
                        strEmaillist = strEmaillist.Replace("CLIENT: ", "");
                        strEmaillist = strEmaillist.Replace("CSR: ", "");
                        strEmaillist = strEmaillist.Replace("CARRIER: ", "");
                    }
                    initInt = initInt + 1;
                }
            }
            if (strEmaillist != "")
            {
                strEmaillist = strEmaillist.TrimEnd(';');
                strEmaillist = strEmaillist.Replace(";", ",");
                ViewBag.emailList = strEmaillist;
            }
            Mapper.CreateMap<InBoundScanHeader, InBoundScanHeaderModel>();
            InBoundScanHeaderModel objContainerArrivalModel = Mapper.Map<InBoundScanHeader, InBoundScanHeaderModel>(objIBScanIn);
            if (objContainerArrivalModel != null)
            {
                if (objContainerArrivalModel.ListGetContainerArrivalDetails.Count > 0)
                {
                    Session["ib_doc_id"] = objContainerArrivalModel.ListGetContainerArrivalDetails[0].ib_doc_id;
                    string strdate = objContainerArrivalModel.ListGetContainerArrivalDetails[0].ib_doc_dt.ToString();
                    strdate = strdate.Substring(0, 10);
                    ViewBag.ib_doc_dt = strdate;
                    ViewBag.SelectedType = objContainerArrivalModel.ListGetContainerArrivalDetails[0].type.ToString();
                }
            }

            return PartialView("_DashboardInBoundScanIn", objContainerArrivalModel);
        }

        public ActionResult CompanyAdd(string p_str_cmpid)
        {
            InBoundScanHeaderModel objContainerArrivalModel = new InBoundScanHeaderModel();
            //CompanyMaster objCompanyMaster = new CompanyMaster();
            //ICompanyMasterService ServiceObject = new CompanyMasterService();
            //objCompany.cust_of_cmp_id = string.Empty;
            //objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            //objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            //objCompanyMaster.last_chg_dt = DateTime.Now.ToString("MM/dd/yyyy");
            //objCompanyMaster.start_dt = DateTime.Now.ToString("MM/dd/yyyy");
            //objPick.cmp_id = (p_str_cmp_id == null) ? string.Empty : p_str_cmp_id.Trim();
            //objPick = ServiceObjectPick.GetCountryPick(objPick);
            //objCompanyMaster.ListCntryPick = objPick.ListCntryPick;
            //objPick = ServiceObjectPick.GetStatePick(objPick);
            //objCompanyMaster.ListStatePick = objPick.ListStatePick;
            //objLookUp.id = "16";
            //objLookUp.lookuptype = "CUSTOMERMASTER";
            //objLookUp = ServiceObjectLookUp.GetLookUpValue(objLookUp);
            //objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            //Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            //CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_AddNewCarrier", objContainerArrivalModel);
        }
        public ActionResult GetEmaildetials(string p_str_carrier_id, string p_str_cmp_id)
        {
            try
            {

                InBoundScanHeader objContainerArrival = new InBoundScanHeader();
                IContainerArrivalService ServiceObject = new ContainerArrivalServiceService();
                if (p_str_carrier_id != null)
                {
                    var Result = ServiceObject.GetCarrierEmailDetails(p_str_carrier_id, p_str_cmp_id);
                    if (Result != "")
                    {
                        return Json(Result, JsonRequestBehavior.AllowGet);
                    }
                    return null;
                }
                else
                {
                    return Json(objCustMaster.ListCheckExistCmpId.Count, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                string strException = ex.InnerException.ToString();
                return Json(null);
            }
        }
        public ActionResult AddCarrierMaster()
        {
            Carrier objCarrier = new Carrier();
            CarrierHdr objCarrierHdr = new CarrierHdr();
            objCarrier.tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            ICarrierService srvcObjCarrier = new CarrierService();

            if (Session["UserID"].ToString() != null)
            {
                objCarrierHdr.entered_by = Session["UserID"].ToString().Trim();
                objCarrierHdr.updated_by = Session["UserID"].ToString().Trim();
            }
            else
            {
                objCarrierHdr.entered_by = "Admin";
                objCarrierHdr.updated_by = "Admin";
            }
            objCarrierHdr.entered_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objCarrierHdr.updated_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objCarrier.option = "ADD";

            Mapper.CreateMap<Carrier, CarrierModel>();
            CarrierModel objCarrierModel = Mapper.Map<Carrier, CarrierModel>(objCarrier);
            return PartialView("_CarrierEntry", objCarrierModel);
        }

        public ActionResult LoadScanSerialDetails(string Id, string cmp_id, string Itm_Code, string Style, string Color, string Size, string itm_name, string ppk, string ctn, string TotalQty)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            //InboundInquiryService ServiceObject = new InboundInquiryService();
            //IContainerArrivalService ServiceObject = new ContainerArrivalServiceService();
            IIBScanInService ServiceObject = new IBScanInService();

            objInboundInquiry.CompID = cmp_id;
            objInboundInquiry.ib_doc_id = Id;
            objInboundInquiry.LineNum = 1;
            objInboundInquiry = ServiceObject.GetInboundHdrDtl(objInboundInquiry);
            objInboundInquiry.Container = (objInboundInquiry.ListAckRptDetails[0].Container == null || objInboundInquiry.ListAckRptDetails[0].Container == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].Container.Trim());
            objInboundInquiry.status = (objInboundInquiry.ListAckRptDetails[0].status == null || objInboundInquiry.ListAckRptDetails[0].status == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].status.Trim());
            objInboundInquiry.InboundRcvdDt = (objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == null || objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt.Trim());
            //objInboundInquiry.vend_id = objInboundInquiry.ListAckRptDetails[0].vend_id.Trim();

            objInboundInquiry.vend_id = (objInboundInquiry.ListAckRptDetails[0].vend_id == null || objInboundInquiry.ListAckRptDetails[0].vend_id == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_id.Trim());
            objInboundInquiry.vend_name = (objInboundInquiry.ListAckRptDetails[0].vend_name == null || objInboundInquiry.ListAckRptDetails[0].vend_name == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_name.Trim());
            objInboundInquiry.FOB = (objInboundInquiry.ListAckRptDetails[0].FOB == null || objInboundInquiry.ListAckRptDetails[0].FOB == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].FOB.Trim());
            objInboundInquiry.refno = (objInboundInquiry.ListAckRptDetails[0].refno == null || objInboundInquiry.ListAckRptDetails[0].refno == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].refno.Trim());

            //objInboundInquiry.ibdocid = Id;
            //objInboundInquiry = ServiceObject.GetDocEntryId(objInboundInquiry);
            //objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
            //objInboundInquiry.cmp_id = cmp_id;
            //objInboundInquiry = ServiceObject.GetInboundDtl(objInboundInquiry);
            objInboundInquiry.ItemScanIN = new ItemScanIN();
            objInboundInquiry.ItemScanIN.cmp_id = cmp_id;
            objInboundInquiry.ItemScanIN.ib_doc_id = Id;
            objInboundInquiry.ItemScanIN.itm_code = Itm_Code;
            objInboundInquiry.ItemScanIN.itm_num = Style;
            objInboundInquiry.ItemScanIN.itm_color = Color;
            objInboundInquiry.ItemScanIN.itm_size = Size;
            objInboundInquiry.ItemScanIN.itm_name = itm_name;
            objInboundInquiry.ItemScanIN.ppk = ppk;
            objInboundInquiry.ItemScanIN.ctn = ctn;
            objInboundInquiry.ItemScanIN.TotalQty = TotalQty;
            objInboundInquiry.ItemScanIN.ib_doc_dt = null;
            objInboundInquiry.ItemScanIN.ob_doc_dt = null;

            //objInboundInquiry.ListItemScanIN = ServiceObject.getScanInDetailsByItemCode(cmp_id, Itm_Code, string.Empty);
            // objInboundInquiry.ItemScanIN.balanceScan = Convert.ToInt32(objInboundInquiry.ItemScanIN.TotalQty) - objInboundInquiry.ListItemScanIN.Count();
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ScanSerialDetails", InboundInquiryModel);
        }
        public ActionResult SaveContainerArrival(string p_str_cmp_id, string p_str_doc_id, string p_str_adj_dt, string p_str_carrier_id, string p_str_whs_id, string p_str_type, string p_str_cont_id, string p_str_event, string p_str_Notes, string p_str_emailList)
        {
            string SelPkgIds = string.Empty;
            string ItemCode = string.Empty;
            string l_str_save_status = string.Empty;
            InBoundScanHeader objContainerArrival = new InBoundScanHeader();
            IContainerArrivalService ServiceObject = new ContainerArrivalServiceService();
            if (p_str_doc_id == null || p_str_doc_id == "")
            {
                p_str_doc_id = Session["ib_doc_id"].ToString().Trim();
            }
            l_str_save_status = ServiceObject.SaveContainerArrival(p_str_cmp_id, p_str_doc_id, p_str_adj_dt, p_str_carrier_id, p_str_whs_id, p_str_type, p_str_cont_id, p_str_event, p_str_Notes);
            var mBody = "Please note that container'" + p_str_cont_id + "'Arrived now";

            //if (p_str_event == "Add")
            //{
            //    SendMail(p_str_carrier_id, p_str_cont_id, p_str_cmp_id);
            //}

            if (p_str_event == "Add")
            {
                SendMailCust_Carrier_CSR(p_str_emailList, p_str_cont_id, p_str_cmp_id);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdItemTempDetails(string cmpId, string ibdocId, string rcvdate, string paletId, string lot_id, string frm_loc, string WhsId,
            string itm_num, string itm_color, string itm_size, int MoveQty, int MoveCtn, string Itemcode, int l_str_ppk, int tot_ctns, int tot_qty, string to_loc,
            string po_num, string cont_id)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            if (frm_loc.Trim() == to_loc.Trim())
            {
                int Tolocation = 1;
                return Json(Tolocation, JsonRequestBehavior.AllowGet);
            }
            if ((tot_qty) < (MoveQty))
            {
                int Tolocation = 2;
                return Json(Tolocation, JsonRequestBehavior.AllowGet);
            }
            objStockChange.cmp_id = cmpId.Trim();
            if (clsGlobal.AdjDocId == string.Empty)
            {
                objStockChange = objService.GetAdjustDocID(objStockChange);
                objStockChange.adj_doc_id = objStockChange.adj_doc_id;
                clsGlobal.AdjDocId = objStockChange.adj_doc_id;
            }

            else
            {
                objStockChange.adj_doc_id = clsGlobal.AdjDocId;
            }

            objStockChange.adj_date = DateTime.Now.ToString("MM/dd/yyyy");
            objStockChange.itm_code = Itemcode.Trim();
            objStockChange.whs_id = WhsId.Trim();
            objStockChange.ib_doc_id = ibdocId.Trim();
            objStockChange.rcvd_dt = rcvdate.Trim();
            objStockChange.cont_id = cont_id.Trim();
            objStockChange.lot_id = lot_id.Trim();
            objStockChange.lot_num = paletId.Trim();
            objStockChange.po_num = po_num.Trim();
            objStockChange.frm_loc = frm_loc.Trim();
            objStockChange.pkg_qty = l_str_ppk;
            objStockChange.tot_ctns = tot_ctns;
            objStockChange.to_loc = to_loc.Trim();
            objStockChange.move_ctns = MoveCtn;
            objStockChange.userId = Session["UserID"].ToString().Trim();
            objStockChange = objService.InsertTempStkMove(objStockChange);
            return Json("3", JsonRequestBehavior.AllowGet);
        }
        public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        {
            StockChangeService ServiceObject = new StockChangeService();
            var List = ServiceObject.ItemXGetitmDetails(term, cmp_id.Trim()).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SameLocID(string frm_loc, string to_loc)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            if (frm_loc.Trim() == to_loc.Trim())
            {
                int ResultCount = 1;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult STK_CHG_INQ_HDR_DATA(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_search_text, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_whs_id, string p_str_Date_fm, string p_str_Date_To, string p_str_po_num)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Session["str_cmp_id"] = p_str_cmp_id.Trim();
            Session["str_ib_doc_id"] = p_str_ib_doc_id.Trim();
            Session["str_itm_num"] = p_str_itm_num.Trim();
            Session["str_itm_color"] = p_str_itm_color.Trim();
            Session["str_itm_size"] = p_str_itm_size.Trim();
            Session["str_status"] = p_str_itm_name.Trim();
            Session["str_search_text"] = p_str_search_text.Trim();
            Session["str_cont_id"] = p_str_cont_id.Trim();
            Session["str_lot_id"] = p_str_lot_id.Trim();
            Session["str_loc_id"] = p_str_loc_id.Trim();
            Session["str_whs_id"] = p_str_whs_id.Trim();
            Session["str_rcvd_dt_fm"] = p_str_Date_fm.Trim();
            Session["str_rcvd_dt_to"] = p_str_Date_To.Trim();
            Session["str_po_num"] = p_str_po_num.Trim();
            return Json(objStockChange.ListGetStockChangeDetails, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DashboardStkChangeItemdtl(string p_str_cmp_id, string Itemcode)
        {
            try
            {
                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();
                objStockChange.cmp_id = p_str_cmp_id;
                objStockChange.itm_code = Itemcode;
                objStockChange = objService.GetItemMoveGridLoadItem(objStockChange);
                if (objStockChange.ListGetItemMoveDetails.Count > 0)
                {
                    objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[0].ib_doc_id;
                    objStockChange.lot_id = objStockChange.ListGetItemMoveDetails[0].lot_id;
                    objStockChange.date = Convert.ToDateTime(objStockChange.ListGetItemMoveDetails[0].rcvd_dt).ToString("MM/dd/yyyy");
                    objStockChange.whs_id = objStockChange.ListGetItemMoveDetails[0].whs_id;
                    objStockChange.frm_loc = objStockChange.ListGetItemMoveDetails[0].loc_id;
                    objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[0].ib_doc_id;
                    objStockChange.po_num = objStockChange.ListGetItemMoveDetails[0].po_num;
                    objStockChange.lot_num = objStockChange.ListGetItemMoveDetails[0].palet_id;
                }
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_DashboardItemMoveGrid", StockChangeModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("UserLogin", "Login");
            }

        }
        public EmptyResult CmpIdOnChange(string p_str_cmp_id)
        {
            Session["g_str_cmp_id"] = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
            return null;
        }
        public JsonResult GetItemCode(string l_str_cmp_id, string l_str_Itmdtl, string l_str_itm_color, string l_str_itm_size)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            objStockChange.cmp_id = l_str_cmp_id;
            objStockChange.itm_num = l_str_Itmdtl;
            objStockChange.itm_color = l_str_itm_color;
            objStockChange.itm_size = l_str_itm_size;
            objStockChange = objService.GetItemCode(objStockChange);
            if (objStockChange.LstItmCodetdtl.Count > 0)
            {
                objStockChange.ItmCode = objStockChange.LstItmCodetdtl[0].ItmCode;
                objStockChange.itm_name = objStockChange.LstItmCodetdtl[0].itm_name;
                return Json(new { data1 = "Y", data2 = objStockChange.ItmCode, data3 = objStockChange.itm_name }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data1 = "N", data2 = objStockChange.ItmCode, data3 = objStockChange.itm_name }, JsonRequestBehavior.AllowGet);
            //return Json(objOutboundInq.ItmCode, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailCust_Carrier_CSR(string p_str_emailList, string p_str_cont_id, string p_str_cmp_id)
        {
            InBoundScanHeader objContainerArrival = new InBoundScanHeader();
            IContainerArrivalService ServiceObjectCA = new ContainerArrivalServiceService();
            ForgotPassword ObjForgotPassword = new ForgotPassword();
            ForgotPasswordService ServiceObject = new ForgotPasswordService();
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                string strSMTPHost = string.Empty;
                string strSmtpUserId = string.Empty;
                string strSmtpPassword = string.Empty;
                string EmailMessage = string.Empty;
                string l_str_from_email_display_name = string.Empty;

                try
                {
                    strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();
                    strSmtpUserId = System.Configuration.ConfigurationManager.AppSettings["SMTPUserId"].ToString();
                    strSmtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPUserPwd"].ToString();
                    l_str_from_email_display_name = System.Configuration.ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                }
                catch
                {
                }
                //var p_str_email_id = ServiceObjectCA.GetCarrierEmailDetails(p_str_carrier_id, p_str_cmp_id);
                //p_str_emailList
                //var p_str_user_id = "pm";
                //No Email details Found
                if (p_str_emailList == "")
                {
                    p_str_emailList = "krishbala@gensoftcorp.com";
                }
                if (p_str_emailList != null && p_str_emailList != string.Empty)
                {
                    mail.To.Add(p_str_emailList.Trim());
                }

                mail.From = new MailAddress(strSmtpUserId, l_str_from_email_display_name);
                //Random randomnumber = new Random();
                //var randompassword = randomnumber.Next(0, 1000000).ToString("D10");
                //if (randompassword != null && randompassword != string.Empty)
                //{
                //    ObjForgotPassword.user_id = (p_str_user_id == null) ? string.Empty : p_str_user_id.Trim();
                //    ObjForgotPassword.new_pwd = (randompassword == null) ? string.Empty : randompassword.Trim();
                //    ObjForgotPassword = ServiceObject.UpdateUserAccPwd(ObjForgotPassword);
                //}
                mail.Subject = p_str_cont_id + " Arrived !!!" + " Date :" + DateTime.Now; ;
                EmailMessage = "Hi, " + "\n" + "\n" + "Your Container has been Arrived" + "\n" + "Container No : " + p_str_cont_id + "\n" + "Date :" + DateTime.Now;
                StringBuilder myBuilder = new StringBuilder();
                if (EmailMessage != null && EmailMessage != "")
                {
                    string[] lstr_message = EmailMessage.Split('\n');

                    for (int j = 0; j < lstr_message.Count(); j++)
                    {

                        myBuilder.Append(lstr_message[j].ToString());
                        myBuilder.Append("<br/>");
                        if (j == lstr_message.Count() - 1)
                        {
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("Thanks & Regards, ");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("CDC 3pl Team");
                        }
                        mail.Body = myBuilder.ToString();
                    }
                }
                else
                {
                    mail.Body = "";
                }
                mail.IsBodyHtml = true;
                string lstr_file_name = string.Empty;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = strSMTPHost;
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(strSmtpUserId, strSmtpPassword);
                smtp.EnableSsl = false;

                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    strSmtpPassword = ex.InnerException.ToString();
                }
                return Json("Email", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
        }
        public ActionResult SendMail(string p_str_carrier_id, string p_str_cont_id, string p_str_cmp_id)
        {
            InBoundScanHeader objContainerArrival = new InBoundScanHeader();
            IContainerArrivalService ServiceObjectCA = new ContainerArrivalServiceService();
            ForgotPassword ObjForgotPassword = new ForgotPassword();
            ForgotPasswordService ServiceObject = new ForgotPasswordService();
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                string strSMTPHost = string.Empty;
                string strSmtpUserId = string.Empty;
                string strSmtpPassword = string.Empty;
                string EmailMessage = string.Empty;
                string l_str_from_email_display_name = string.Empty;

                try
                {
                    strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();
                    strSmtpUserId = System.Configuration.ConfigurationManager.AppSettings["SMTPUserId"].ToString();
                    strSmtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPUserPwd"].ToString();
                    l_str_from_email_display_name = System.Configuration.ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                }
                catch
                {
                }
                var p_str_email_id = ServiceObjectCA.GetCarrierEmailDetails(p_str_carrier_id, p_str_cmp_id);
                //var p_str_user_id = "pm";
                if (p_str_email_id == "No Email details Found")
                {
                    p_str_email_id = "krishbala@gensoftcorp.com";
                }
                if (p_str_email_id != null && p_str_email_id != string.Empty)
                {
                    mail.To.Add(p_str_email_id.Trim());
                }

                mail.From = new MailAddress(strSmtpUserId, l_str_from_email_display_name);
                //Random randomnumber = new Random();
                //var randompassword = randomnumber.Next(0, 1000000).ToString("D10");
                //if (randompassword != null && randompassword != string.Empty)
                //{
                //    ObjForgotPassword.user_id = (p_str_user_id == null) ? string.Empty : p_str_user_id.Trim();
                //    ObjForgotPassword.new_pwd = (randompassword == null) ? string.Empty : randompassword.Trim();
                //    ObjForgotPassword = ServiceObject.UpdateUserAccPwd(ObjForgotPassword);
                //}
                mail.Subject = p_str_cont_id + " Arrived !!!" + " Date :" + DateTime.Now; ;
                EmailMessage = "Hi, " + "\n" + "\n" + "Your Container has been Arrived" + "\n" + "Container No : " + p_str_cont_id + "\n" + "Date :" + DateTime.Now;
                StringBuilder myBuilder = new StringBuilder();
                if (EmailMessage != null && EmailMessage != "")
                {
                    string[] lstr_message = EmailMessage.Split('\n');

                    for (int j = 0; j < lstr_message.Count(); j++)
                    {

                        myBuilder.Append(lstr_message[j].ToString());
                        myBuilder.Append("<br/>");
                        if (j == lstr_message.Count() - 1)
                        {
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("Thanks & Regards, ");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("CDC 3pl Team");
                        }
                        mail.Body = myBuilder.ToString();
                    }
                }
                else
                {
                    mail.Body = "";
                }
                mail.IsBodyHtml = true;
                string lstr_file_name = string.Empty;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = strSMTPHost;
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(strSmtpUserId, strSmtpPassword);
                smtp.EnableSsl = false;

                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    strSmtpPassword = ex.InnerException.ToString();
                }
                return Json("Email", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
        }


    }
}