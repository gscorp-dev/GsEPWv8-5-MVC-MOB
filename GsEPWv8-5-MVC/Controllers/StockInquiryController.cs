using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GsEPWv8_5_MVC.Common;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace GsEPWv8_5_MVC.Controllers
{
    public class StockInquiryController : Controller
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
        public ActionResult DashboardStockInquiry(string FullFillType, string cmp, string screentitle)
        {
            string l_str_cmp_id = string.Empty;
            string  whs = string.Empty;
            try
            {
                StockChange objStockChange = new StockChange();
                IStockChangeService ServiceObject = new StockChangeService();

                if (cmp == null)
                {
                    cmp = Session["g_str_cmp_id"].ToString().Trim();
                    objStockChange.cmp_id = cmp;
                    if (Session["g_str_whs_id"] != null)
                    {
                        whs = Session["g_str_whs_id"].ToString().Trim();
                        objStockChange.whs_id = whs;
                    }
                }
                else
                {
                    objStockChange.cmp_id = cmp;
                    if (Session["g_str_whs_id"] !=null)
                    {
                        whs = Session["g_str_whs_id"].ToString().Trim();
                        objStockChange.whs_id = whs;
                    }
                   
                }

                clsGlobal.AdjDocId = string.Empty;
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objStockChange.cmp_id != "" && FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }

                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);

                objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                objCompany.cust_cmp_id = cmp;
                objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);

                objStockChange.ListLocPickDtl = objCompany.ListLocPickDtl;
                objStockChange.cmp_id = cmp;
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objStockChange.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objStockChange.tmp_cmp_id = objStockChange.cmp_id;
                objStockChange.p_str_company = objStockChange.cmp_id;
                objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);

                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel objStockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return View(objStockChangeModel);
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
        public ActionResult DashboardStockInquiryGrid(string p_str_cmpid, string p_str_LocId,
            string p_str_style, string p_str_Color, string p_str_Size)
         {
            string l_str_stkcmp_id = string.Empty;
            string l_str_stkitm_num = string.Empty;
            string l_str_stkitm_color = string.Empty;
            string l_str_stkitm_size = string.Empty;
            string l_str_stkitm_desc = string.Empty;
            string l_str_stklot_id = string.Empty;
            string l_str_stkpalet_id = string.Empty;
            string l_str_stkcont_id = string.Empty;
            string l_str_stkwhs_id = string.Empty;
            string l_str_stkloc_id = string.Empty;
            string l_str_stkpkg_type = string.Empty;
            string l_str_stkpo_num = string.Empty;
            string l_str_stkibdoc_id = string.Empty;
            string l_str_stkrcvd_dt = string.Empty;
            int l_str_AsrtItm = 0;
            int l_str_stpkg_qty = 0;
            int l_str_stkitm_qty = 0;
            int l_str_stktot_ctn = 0;
            int l_str_stktot_qty = 0;
            int l_str_aloc_ctn = 0;
            int l_str_aloc_qty = 0;
            string l_str_aloccmp_id = string.Empty;
            string l_str_alocitm_num = string.Empty;
            string l_str_alocitm_color = string.Empty;
            string l_str_alocitm_size = string.Empty;
             string l_str_alocibdoc_id = string.Empty;
            string l_str_aloclot_id = string.Empty;
            string l_str_alocpalet_id = string.Empty;
            string l_str_aloccont_id = string.Empty;
            string l_str_alocloc_id = string.Empty;
            string l_str_alocpkg_type = string.Empty;
            string l_str_alocpo_num = string.Empty;
            string l_str_alocwhs = string.Empty;
            int l_str_alocitm_qty = 0;
            int l_str_alocpkg_qty = 0;
            int l_str_count1 = 0;
            int l_str_count2 = 0;
            dtStockInq = new DataTable();
            List<StockInquiryDtl> li = new List<StockInquiryDtl>();
            //2: Initialize a object of type DataRow
            DataRow drStk;

            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colib_doc_id = new DataColumn("ib_doc_id", typeof(string));
            DataColumn colrcvd_dt = new DataColumn("rcvd_dt", typeof(string));
            DataColumn colcont_id = new DataColumn("cont_id", typeof(string));
            DataColumn colitm_num = new DataColumn("itm_num", typeof(string));
            DataColumn colitm_color = new DataColumn("itm_color", typeof(string));
            DataColumn colitm_size = new DataColumn("itm_size", typeof(string));
            DataColumn colitm_name = new DataColumn("itm_name", typeof(string));
            DataColumn colloc_id = new DataColumn("loc_id", typeof(string));
            DataColumn collot_id = new DataColumn("lot_id", typeof(string));
            DataColumn colpalet_id = new DataColumn("palet_id", typeof(string));
            DataColumn colpo_num = new DataColumn("po_num", typeof(string));
            DataColumn colTOT_CTN = new DataColumn("TOT_CTN", typeof(int));
            DataColumn colpkg_qty = new DataColumn("pkg_qty", typeof(int));
            DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
            DataColumn colitm_qty = new DataColumn("itm_qty", typeof(int));
            DataColumn coltot_qty = new DataColumn("tot_qty", typeof(int));
            DataColumn colAlocCtn = new DataColumn("AlocCtn", typeof(int));
            DataColumn colAlocqty = new DataColumn("Alocqty", typeof(int));
            DataColumn colpkg_type = new DataColumn("pkg_type", typeof(string));
            DataColumn colkit_id = new DataColumn("kit_id", typeof(string));
            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtStockInq.Columns.Add(colCmpId);
            dtStockInq.Columns.Add(colib_doc_id);
            dtStockInq.Columns.Add(colrcvd_dt);
            dtStockInq.Columns.Add(colcont_id);
            dtStockInq.Columns.Add(colitm_num);
            dtStockInq.Columns.Add(colitm_color);
            dtStockInq.Columns.Add(colitm_size);
            dtStockInq.Columns.Add(colitm_name);
            dtStockInq.Columns.Add(colloc_id);
            dtStockInq.Columns.Add(collot_id);
            dtStockInq.Columns.Add(colpalet_id);
            dtStockInq.Columns.Add(colpo_num);
            dtStockInq.Columns.Add(colTOT_CTN);
            dtStockInq.Columns.Add(colpkg_qty);
            dtStockInq.Columns.Add(colwhs_id);
            dtStockInq.Columns.Add(colitm_qty);
            dtStockInq.Columns.Add(coltot_qty);
            dtStockInq.Columns.Add(colAlocCtn);
            dtStockInq.Columns.Add(colAlocqty);
            dtStockInq.Columns.Add(colpkg_type);
            dtStockInq.Columns.Add(colkit_id);

            StockInquiryDtl objStackInquiry = new StockInquiryDtl();
            IStockInquiryService ServiceObject = new StockInquiryService();
            objStackInquiry.cmp_id = p_str_cmpid;
            objStackInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
            objStackInquiry.ib_doc_id = "";
            objStackInquiry.Status = "SEARCH_EXACT";
            objStackInquiry.itm_num = p_str_style;
            objStackInquiry.itm_color = p_str_Color;
            objStackInquiry.itm_size = p_str_Size;
            objStackInquiry.itm_name = "";
            objStackInquiry.cont_id = "";
            objStackInquiry.loc_id = p_str_LocId;
            objStackInquiry.lot_id = "";
            objStackInquiry.grn_id = "";
            objStackInquiry.whs_id = "";
            objStackInquiry.Date_fm = "";
            objStackInquiry.Date_to = "";
            objStackInquiry.po_num = "";
            objStackInquiry = ServiceObject.GetStockInquiryDetails(objStackInquiry);
            objStackInquiry = ServiceObject.GetStockInquiryDetailAloc(objStackInquiry);
            l_str_count1 = objStackInquiry.ListStockInquiry.Count();
            l_str_count2 = objStackInquiry.LstStockInquirystock.Count();
            for (int i = 0; i < l_str_count1; i++)
            {
                l_str_stkitm_num = "";
                l_str_stkitm_color = "";
                l_str_stkitm_size = "";
                l_str_stklot_id = "";
                l_str_stkpalet_id = "";
                l_str_stkwhs_id = "";
                l_str_stpkg_qty = 0;
                l_str_stpkg_qty = 0;
                l_str_stktot_ctn = 0;
                l_str_stktot_qty = 0;
                l_str_aloc_ctn = 0;
                l_str_aloc_qty = 0;
                l_str_stkcmp_id = objStackInquiry.ListStockInquiry[i].cmp_id;
                l_str_AsrtItm = Convert.ToInt32(objStackInquiry.ListStockInquiry[i].kit_id);
                l_str_stkitm_num = objStackInquiry.ListStockInquiry[i].itm_num;
                l_str_stkitm_color = objStackInquiry.ListStockInquiry[i].itm_color;
                l_str_stkitm_size = objStackInquiry.ListStockInquiry[i].itm_size;
                l_str_stkitm_desc = objStackInquiry.ListStockInquiry[i].itm_name;
                l_str_stklot_id = objStackInquiry.ListStockInquiry[i].lot_id;
                l_str_stkpalet_id = objStackInquiry.ListStockInquiry[i].palet_id;
                l_str_stkcont_id = objStackInquiry.ListStockInquiry[i].cont_id;
                l_str_stkloc_id = objStackInquiry.ListStockInquiry[i].loc_id;
                l_str_stkwhs_id = objStackInquiry.ListStockInquiry[i].whs_id;
                l_str_stkpkg_type = objStackInquiry.ListStockInquiry[i].pkg_type;
                l_str_stkpo_num = objStackInquiry.ListStockInquiry[i].po_num;
                l_str_stkibdoc_id = objStackInquiry.ListStockInquiry[i].ib_doc_id;
                l_str_stpkg_qty = objStackInquiry.ListStockInquiry[i].pkg_qty;
                l_str_stkitm_qty = Convert.ToInt32(objStackInquiry.ListStockInquiry[i].itm_qty);
                l_str_stktot_qty = objStackInquiry.ListStockInquiry[i].AvlQty;
                l_str_stktot_ctn = objStackInquiry.ListStockInquiry[i].AvlCtn;
                l_str_stkrcvd_dt = objStackInquiry.ListStockInquiry[i].rcvd_dt;

                for (int j = 0; j < l_str_count2; j++)
                {
                    l_str_aloccmp_id = objStackInquiry.LstStockInquirystock[j].cmp_id;
                    l_str_alocitm_num = objStackInquiry.LstStockInquirystock[j].itm_num;
                    l_str_alocitm_color = objStackInquiry.LstStockInquirystock[j].itm_color;
                    l_str_alocitm_size = objStackInquiry.LstStockInquirystock[j].itm_size;
                    l_str_alocibdoc_id = objStackInquiry.LstStockInquirystock[j].ib_doc_id;
                    l_str_aloclot_id = objStackInquiry.LstStockInquirystock[j].lot_id;
                    l_str_alocpalet_id = objStackInquiry.LstStockInquirystock[j].palet_id;
                    l_str_aloccont_id = objStackInquiry.LstStockInquirystock[j].cont_id;
                    l_str_alocloc_id = objStackInquiry.LstStockInquirystock[j].loc_id;
                    l_str_alocpkg_type = objStackInquiry.LstStockInquirystock[j].pkg_type;
                    l_str_alocpo_num = objStackInquiry.LstStockInquirystock[j].po_num;
                    l_str_alocwhs = objStackInquiry.LstStockInquirystock[j].whs_id;
                    l_str_alocitm_qty = Convert.ToInt32(objStackInquiry.LstStockInquirystock[j].itm_qty);
                    l_str_alocpkg_qty = objStackInquiry.LstStockInquirystock[j].pkg_qty;

                    if (l_str_stkcmp_id == l_str_aloccmp_id && l_str_stkpalet_id == l_str_alocpalet_id && l_str_stkpo_num == l_str_alocpo_num && l_str_stklot_id == l_str_aloclot_id && l_str_stkwhs_id == l_str_alocwhs
                    && l_str_stpkg_qty == l_str_alocpkg_qty && l_str_stkitm_num == l_str_alocitm_num && l_str_stkitm_color == l_str_alocitm_color && l_str_stkitm_size == l_str_alocitm_size && l_str_stkloc_id == l_str_alocloc_id && l_str_stkpkg_type == l_str_alocpkg_type)
                    {
                        l_str_aloc_ctn = Convert.ToInt32(objStackInquiry.LstStockInquirystock[j].AlocCtn);
                        l_str_aloc_qty = objStackInquiry.LstStockInquirystock[j].AlocQty;
                        l_str_stktot_ctn = l_str_stktot_ctn - l_str_aloc_ctn;
                        l_str_stktot_qty = l_str_stktot_qty - l_str_aloc_qty;
                        break;
                    }

                }
                if (l_str_stktot_qty <= 0)
                {
                    continue;
                }
                objStackInquiry.cmp_id = l_str_stkcmp_id;
                objStackInquiry.ib_doc_id = l_str_stkibdoc_id;
                objStackInquiry.rcvd_dt = l_str_stkrcvd_dt;
                objStackInquiry.cont_id = l_str_stkcont_id;
                objStackInquiry.itm_num = l_str_stkitm_num;
                objStackInquiry.itm_color = l_str_stkitm_color;
                objStackInquiry.itm_size = l_str_stkitm_size;
                objStackInquiry.itm_name = l_str_stkitm_desc;
                objStackInquiry.loc_id = l_str_stkloc_id;
                objStackInquiry.lot_id = l_str_stklot_id;
                objStackInquiry.palet_id = l_str_stkpalet_id;
                objStackInquiry.po_num = l_str_stkpo_num;
                objStackInquiry.TOT_CTN = l_str_stktot_ctn;
                objStackInquiry.pkg_qty = l_str_stpkg_qty;
                objStackInquiry.whs_id = l_str_stkwhs_id;
                objStackInquiry.itm_qty = l_str_stkitm_qty;
                objStackInquiry.tot_qty = l_str_stktot_qty;
                objStackInquiry.AlocCtn = Convert.ToString(l_str_aloc_ctn);
                objStackInquiry.Alocqty = Convert.ToString(l_str_aloc_qty);
                objStackInquiry.pkg_type = l_str_stkpkg_type;
                objStackInquiry.kit_id = Convert.ToString(l_str_AsrtItm);
                drStk = dtStockInq.NewRow();

                dtStockInq.Rows.Add(drStk);
                dtStockInq.Rows[lintCount][colCmpId] = objStackInquiry.cmp_id.ToString();
                dtStockInq.Rows[lintCount][colib_doc_id] = objStackInquiry.ib_doc_id.ToString();
                dtStockInq.Rows[lintCount][colrcvd_dt] = objStackInquiry.rcvd_dt.ToString();
                dtStockInq.Rows[lintCount][colcont_id] = objStackInquiry.cont_id.ToString();
                dtStockInq.Rows[lintCount][colitm_num] = objStackInquiry.itm_num.ToString();
                dtStockInq.Rows[lintCount][colitm_color] = objStackInquiry.itm_color.ToString();
                dtStockInq.Rows[lintCount][colitm_size] = objStackInquiry.itm_size.ToString();
                dtStockInq.Rows[lintCount][colitm_name] = objStackInquiry.itm_name.ToString();
                dtStockInq.Rows[lintCount][colloc_id] = objStackInquiry.loc_id.ToString();
                dtStockInq.Rows[lintCount][collot_id] = objStackInquiry.lot_id.ToString();
                dtStockInq.Rows[lintCount][colpalet_id] = objStackInquiry.palet_id.ToString();
                dtStockInq.Rows[lintCount][colpo_num] = objStackInquiry.po_num.ToString();
                dtStockInq.Rows[lintCount][colTOT_CTN] = objStackInquiry.TOT_CTN.ToString();
                dtStockInq.Rows[lintCount][colpkg_qty] = objStackInquiry.pkg_qty.ToString();
                dtStockInq.Rows[lintCount][colwhs_id] = objStackInquiry.whs_id.ToString();
                dtStockInq.Rows[lintCount][colitm_qty] = objStackInquiry.itm_qty.ToString();
                dtStockInq.Rows[lintCount][coltot_qty] = objStackInquiry.tot_qty.ToString();
                dtStockInq.Rows[lintCount][colAlocCtn] = objStackInquiry.AlocCtn.ToString();
                dtStockInq.Rows[lintCount][colAlocqty] = objStackInquiry.Alocqty.ToString();
                dtStockInq.Rows[lintCount][colpkg_type] = objStackInquiry.pkg_type.ToString();
                dtStockInq.Rows[lintCount][colkit_id] = objStackInquiry.kit_id.ToString();
                StockInquiryDtl objStockInquiryDtltemp = new StockInquiryDtl();
                objStockInquiryDtltemp.cmp_id = objStackInquiry.cmp_id;
                objStockInquiryDtltemp.ib_doc_id = objStackInquiry.ib_doc_id;
                objStockInquiryDtltemp.rcvd_dt = objStackInquiry.rcvd_dt;
                objStockInquiryDtltemp.cont_id = objStackInquiry.cont_id;
                objStockInquiryDtltemp.itm_num = objStackInquiry.itm_num;
                objStockInquiryDtltemp.itm_color = objStackInquiry.itm_color;
                objStockInquiryDtltemp.itm_size = objStackInquiry.itm_size;
                objStockInquiryDtltemp.itm_name = objStackInquiry.itm_name;
                objStockInquiryDtltemp.loc_id = objStackInquiry.loc_id;
                objStockInquiryDtltemp.lot_id = objStackInquiry.lot_id;
                objStockInquiryDtltemp.palet_id = objStackInquiry.palet_id;
                objStockInquiryDtltemp.po_num = objStackInquiry.po_num;
                objStockInquiryDtltemp.TOT_CTN = objStackInquiry.TOT_CTN;
                objStockInquiryDtltemp.pkg_qty = objStackInquiry.pkg_qty;
                objStockInquiryDtltemp.whs_id = objStackInquiry.whs_id;
                objStockInquiryDtltemp.itm_qty = objStackInquiry.itm_qty;
                objStockInquiryDtltemp.tot_qty = objStackInquiry.tot_qty;
                objStockInquiryDtltemp.AlocCtn = objStackInquiry.AlocCtn;
                objStockInquiryDtltemp.Alocqty = objStackInquiry.Alocqty;
                objStockInquiryDtltemp.pkg_type = objStackInquiry.pkg_type;
                objStockInquiryDtltemp.kit_id = objStackInquiry.kit_id;
                li.Add(objStockInquiryDtltemp);
                lintCount++;
                Session["tempAlocQty"] = 0;
                AvlCtn = AvlCtn + l_str_stktot_ctn;
                AvlQty = AvlQty + l_str_stktot_qty;
                AlocQty = AlocQty + l_str_aloc_qty;
                Session["tempAlocQty"] = objStackInquiry.AlocQty;
            }
            objStackInquiry.AvlCtn = AvlCtn;
            objStackInquiry.AvlQty = AvlQty;
            objStackInquiry.AlocQty = AlocQty;
           objStackInquiry.ListStockInquiryGrid = li;
                      
            Mapper.CreateMap<StockInquiryDtl, StockInquiryDtlModel>();
            StockInquiryDtlModel objStockInquiryModel = Mapper.Map<StockInquiryDtl, StockInquiryDtlModel>(objStackInquiry);
            return PartialView("_DashboardStockInquiryGrid", objStockInquiryModel);
        }
            
          

        public ActionResult SaveItemMove(string p_str_cmp_id, List<cls_temp_iv_stk_move> ListItemStockMove)
        {
            string SelPkgIds = string.Empty;
            string ItemCode = string.Empty;
            string l_str_save_status = string.Empty;
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            DataTable dt_item_stock_move = new DataTable();
            dt_item_stock_move = Utility.ConvertListToDataTable(ListItemStockMove);
            l_str_save_status = objService.SaveStkMove(p_str_cmp_id, Session["UserID"].ToString().Trim(), dt_item_stock_move);
            return Json("1", JsonRequestBehavior.AllowGet);
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
            return Json("",JsonRequestBehavior.AllowGet);
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

        public ActionResult DashboardStkChangeItemdtl (string p_str_cmp_id, string Itemcode)
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
        }

    }
}