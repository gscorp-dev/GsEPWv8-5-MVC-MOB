using Dapper;
using GsEPWv8_4_MVC.Data.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_4_MVC.Data.Implementation
{
    public class ScanInRepository : IScanInRepository
    {
        public InboundInquiry GetInboundHdrDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_mvcmob_inbound_scanin_details";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                            @Cntr_id = objInboundInquiry.cont_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListAckRptDetails = ListEShip.ToList();
                    if (objInboundInquiry.ListAckRptDetails.Any())
                    {
                        objInboundInquiry.ib_doc_id = objInboundInquiry.ListAckRptDetails.Select(x => x.ib_doc_id).Max();
                        //objInboundInquiry.ib_doc_id = objInboundInquiry.ListAckRptDetails.FirstOrDefault().ib_doc_id;
                        objInboundInquiry = GetInboundDtl(objInboundInquiry);

                        //objInboundInquiry = ServiceObject.GetItmCode(objInboundInquiry);
                        foreach (InboundInquiry item in objInboundInquiry.ListAckRptDetails)
                        {
                            var inboundItemCode = GetItmCodeByByItemMaster(item);
                            item.cmp_id = objInboundInquiry.cmp_id;
                            item.ib_doc_id = objInboundInquiry.ib_doc_id;
                            item.Itm_Code = inboundItemCode.ListGetItmhdr.FirstOrDefault().itm_code;
                        }
                        // objInboundInquiry.ListItemScanIN = getScanInDetailsByItemCode(objInboundInquiry.cmp_id, objInboundInquiry.Itm_Code, string.Empty);
                        // objInboundInquiry.ListItemScanIN = getScanInDetailsByItemCode(objInboundInquiry.cmp_id,);
                    }
                    return objInboundInquiry;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetInboundDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_webmvc_Inbound_receive_details";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListAckRptDetails = ListEShip.ToList();

                    return objInboundInquiry;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public List<ItemScanIN> getScanInDetailsByItemCode(string cmpId, string itm_code, string itm_serial_num)
        {

            try
            {
                InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    //InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_mvcweb_scan_by_itm_code";
                    IEnumerable<ItemScanIN> ListEShip = connection.Query<ItemScanIN>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = cmpId,
                            @itm_code = itm_code,
                            @itm_serial_num = itm_serial_num

                        },
                        commandType: CommandType.StoredProcedure);
                    //objOrderLifeCycleCategory.ListItemScanIN = ListEShip.ToList();

                    return ListEShip.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        public InboundInquiry GetItmCodeByByItemMaster(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_itemhdr";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.CompID,
                            @p_str_itm_num = objInboundInquiry.Style,
                            @p_str_itm_color = objInboundInquiry.Color,
                            @p_str_itm_size = objInboundInquiry.Size,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGetItmhdr = ListInq.ToList();

                    return objInboundInquiry;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void DeleteScanInDetails(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_delete_itm_scan_serial_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objInboundInquiry.ItemScanIN.cmp_id,
                        @itm_code = objInboundInquiry.ItemScanIN.itm_code,
                        @itm_serial_num = objInboundInquiry.ItemScanIN.itm_serial_num,

                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public void EditScanInDetails(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_update_itm_scan_serial_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objInboundInquiry.ItemScanIN.cmp_id,
                        @itm_code = objInboundInquiry.ItemScanIN.itm_code,
                        @itm_serial_num = objInboundInquiry.ItemScanIN.itm_serial_num,
                        @itm_serial_num_exist = objInboundInquiry.ItemScanIN.itm_serial_num_exist

                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public void InsertScanInDetails(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_add_itm_scan_serial_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objInboundInquiry.ItemScanIN.cmp_id,

                        @itm_code = objInboundInquiry.ItemScanIN.itm_code,
                        @itm_serial_num = objInboundInquiry.ItemScanIN.itm_serial_num,
                        @itm_num = objInboundInquiry.ItemScanIN.itm_num,
                        @itm_color = objInboundInquiry.ItemScanIN.itm_color,
                        @itm_size = objInboundInquiry.ItemScanIN.itm_size,
                        @status = objInboundInquiry.ItemScanIN.status,
                        @ib_doc_id = objInboundInquiry.ItemScanIN.ib_doc_id,
                        @ib_doc_dt = objInboundInquiry.ItemScanIN.ib_doc_dt,
                        @ob_doc_id = objInboundInquiry.ItemScanIN.ob_doc_id,
                        @ob_doc_dt = objInboundInquiry.ItemScanIN.ob_doc_dt,
                        @ib_user = objInboundInquiry.ItemScanIN.ib_user ?? "",
                        @ob_user = objInboundInquiry.ItemScanIN.ob_user ?? ""
                    }, commandType: CommandType.StoredProcedure);

            }
        }
    }
}
