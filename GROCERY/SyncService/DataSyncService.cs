using GROCERY.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace GROCERY.SyncService
{
    public class DataSyncService
    {
        readonly string charsuDb = "data source=192.185.10.110;initial catalog=sachiery_GroceryApps;user id=sachiery_admin;password=bkY000o$3;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        //readonly string charsuDb = "data source=68.66.211.65;initial catalog=anytimea_GROCERY;user id=sa;password=:gRqlrv3wsR1lw;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        readonly string sachiChakkiDb = "data source=sachichakkiwh.mine.nu;initial catalog=BIZPRO_WH;user id=sa;password=golden@3864;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        readonly SqlConnection charsuConnection = null;
        readonly SqlConnection sachiChakkiConnection = null;
        private static DataSyncService _instance;
        public DataSyncService()
        {
            charsuConnection = new SqlConnection(charsuDb);
            sachiChakkiConnection = new SqlConnection(sachiChakkiDb);
            charsuConnection.Open();
            //sachiChakkiConnection.Open();

        }

        public static DataSyncService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataSyncService();
                }
                return _instance;
            }
        }

        public string StartFullSyncing()
        {
            try
            {
                return GetAllBarcodes();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DeleteBarcode()
        {
            SqlCommand cmd = new SqlCommand("delete from BARCODES", charsuConnection);
            cmd.CommandText = "delete from BARCODES";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
            }
        }

        public string StockSync()
        {
            try
            {

                return GetAllStocks();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DeleteStock()
        {
            SqlCommand cmd = new SqlCommand("delete from PS000", charsuConnection);
            cmd.CommandText = "delete from PS000";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
            }
        }

        public string ProductSync()
        {
            try
            {

                return GetAllProducts();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DeleteProduct()
        {
            SqlCommand cmd = new SqlCommand("delete from ITEMINFO", charsuConnection);
            cmd.CommandText = "delete from ITEMINFO";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
            }
        }

        public string GetAllBarcodes()
        {

            DataTable BarcodeTable = new DataTable();
            BarcodeTable.Columns.Add("LOCNO", typeof(Int16));
            BarcodeTable.Columns.Add("ITEM_CODE", typeof(int));
            BarcodeTable.Columns.Add("PACK_CODE", typeof(Int16));
            BarcodeTable.Columns.Add("PACK_DESC", typeof(string));
            BarcodeTable.Columns.Add("UNIT", typeof(decimal));
            BarcodeTable.Columns.Add("bDEFAULT", typeof(bool));
            BarcodeTable.Columns.Add("BAR_CODE", typeof(string));
            BarcodeTable.Columns.Add("BARTYPE", typeof(byte));
            BarcodeTable.Columns.Add("UNIT_PRICE", typeof(decimal));
            BarcodeTable.Columns.Add("DEALER_PRICE1", typeof(decimal));
            BarcodeTable.Columns.Add("DEALER_PRICE2", typeof(decimal));
            BarcodeTable.Columns.Add("RF_MARGIN", typeof(decimal));
            BarcodeTable.Columns.Add("CUSER", typeof(int));
            BarcodeTable.Columns.Add("CDATE", typeof(DateTime));
            BarcodeTable.Columns.Add("MUSER", typeof(int));
            BarcodeTable.Columns.Add("MDATE", typeof(DateTime));
            BarcodeTable.Columns.Add("bRandomDisc", typeof(bool));
            BarcodeTable.Columns.Add("DiscStartDate", typeof(DateTime));
            BarcodeTable.Columns.Add("DiscEndDate", typeof(DateTime));
            BarcodeTable.Columns.Add("DiscStartTime", typeof(DateTime));
            BarcodeTable.Columns.Add("DiscEndTime", typeof(DateTime));
            BarcodeTable.Columns.Add("DISC_TYPE", typeof(Int16));
            BarcodeTable.Columns.Add("DISC", typeof(decimal));
            BarcodeTable.Columns.Add("LOYALTY_DISC", typeof(decimal));
            BarcodeTable.Columns.Add("STAFF_DISC", typeof(decimal));
            BarcodeTable.Columns.Add("IsActive", typeof(bool));

            try
            {
                try
                {
                    if (sachiChakkiConnection.State == ConnectionState.Closed)
                    {
                        sachiChakkiConnection.Open();
                    }
                }
                catch (Exception ex)
                {
                    return "Unable to connect with Sachi Chakki DB, Please try again.";
                }

                SqlCommand cmd = new SqlCommand(@"select LOCNO,ITEM_CODE,PACK_CODE,PACK_DESC,UNIT,bDEFAULT,BAR_CODE,BARTYPE,UNIT_PRICE,
                                 DEALER_PRICE1,DEALER_PRICE2,RF_MARGIN,CUSER,CDATE,MUSER,MDATE,bRandomDisc,DiscStartDate,DiscEndDate,DiscStartTime,
                                 DiscEndTime,DISC_TYPE,DISC,LOYALTY_DISC,STAFF_DISC,IsActive from BARCODES WHERE IsActive = 1 AND bDEFAULT = 1", sachiChakkiConnection);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        try
                        {
                            var barcodeRow = BarcodeTable.NewRow();
                            if (dr["LOCNO"] != null)
                                barcodeRow["LOCNO"] = dr["LOCNO"];
                            if (dr["ITEM_CODE"] != null)
                                barcodeRow["ITEM_CODE"] = dr["ITEM_CODE"];
                            if (dr["PACK_CODE"] != null)
                                barcodeRow["PACK_CODE"] = dr["PACK_CODE"];
                            if (dr["PACK_DESC"] != null)
                                barcodeRow["PACK_DESC"] = dr["PACK_DESC"];
                            if (dr["UNIT"] != null)
                                barcodeRow["UNIT"] = dr["UNIT"];
                            if (dr["bDEFAULT"] != null)
                                barcodeRow["bDEFAULT"] = dr["bDEFAULT"];
                            if (dr["BAR_CODE"] != null)
                                barcodeRow["BAR_CODE"] = dr["BAR_CODE"];
                            if (dr["BARTYPE"] != null)
                                barcodeRow["BARTYPE"] = dr["BARTYPE"];
                            if (dr["UNIT_PRICE"] != null)
                                barcodeRow["UNIT_PRICE"] = dr["UNIT_PRICE"];
                            if (dr["DEALER_PRICE1"] != null)
                                barcodeRow["DEALER_PRICE1"] = dr["DEALER_PRICE1"];
                            if (dr["DEALER_PRICE2"] != null)
                                barcodeRow["DEALER_PRICE2"] = dr["DEALER_PRICE2"];
                            if (dr["RF_MARGIN"] != null)
                                barcodeRow["RF_MARGIN"] = dr["RF_MARGIN"];
                            if (dr["CUSER"] != null)
                                barcodeRow["CUSER"] = dr["CUSER"];
                            if (dr["CDATE"] != null)
                                barcodeRow["CDATE"] = dr["CDATE"];
                            if (dr["MUSER"] != null)
                                barcodeRow["MUSER"] = dr["MUSER"];
                            if (dr["MDATE"] != null)
                                barcodeRow["MDATE"] = dr["MDATE"];
                            if (dr["bRandomDisc"] != null)
                                barcodeRow["bRandomDisc"] = dr["bRandomDisc"];
                            if (dr["DiscStartDate"] != null)
                                barcodeRow["DiscStartDate"] = dr["DiscStartDate"];
                            if (dr["DiscEndDate"] != null)
                                barcodeRow["DiscEndDate"] = dr["DiscEndDate"];
                            if (dr["DiscStartTime"] != null)
                                barcodeRow["DiscStartTime"] = dr["DiscStartTime"];
                            if (dr["DiscEndTime"] != null)
                                barcodeRow["DiscEndTime"] = dr["DiscEndTime"];
                            if (dr["DISC_TYPE"] != null)
                                barcodeRow["DISC_TYPE"] = dr["DISC_TYPE"];
                            if (dr["DISC"] != null)
                                barcodeRow["DISC"] = dr["DISC"];
                            if (dr["LOYALTY_DISC"] != null)
                                barcodeRow["LOYALTY_DISC"] = dr["LOYALTY_DISC"];
                            if (dr["STAFF_DISC"] != null)
                                barcodeRow["STAFF_DISC"] = dr["STAFF_DISC"];
                            if (dr["IsActive"] != null)
                                barcodeRow["IsActive"] = dr["IsActive"];

                            BarcodeTable.Rows.Add(barcodeRow);
                        }
                        catch (Exception ex)
                        {
                            return "During syncing barcodes Sachi Chakki DB connection closed, Please try again.";
                        }
                    }
                }
                try
                {

                    DeleteBarcode();

                    try
                    {
                        SqlCommand cmd1 = new SqlCommand("SP_InsertBarcode", charsuConnection);
                        cmd1.CommandText = "SP_InsertBarcode";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        SqlParameter sqlParameter = cmd1.Parameters.AddWithValue("@BarcodeType", BarcodeTable);
                        sqlParameter.SqlDbType = SqlDbType.Structured;
                        cmd1.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return "Unable to save barcodes in Chaarsu DB, Please try again.";
                    }
                    //}

                    //StockSync();
                }
                catch (Exception ex)
                {
                    return "Unable to connect with Sachi Chakki DB, Please try again.";
                }
            }
            catch (Exception ex)
            {
                return "Unable to connect with Sachi Chakki DB, Please try again.";
            }

            return "Success";
        }

        public string GetAllStocks()
        {
            DataTable StockTable = new DataTable();
            StockTable.Columns.Add("LOCNO", typeof(Int16));
            StockTable.Columns.Add("ITEM_CODE", typeof(int));
            StockTable.Columns.Add("QTY", typeof(decimal));
            StockTable.Columns.Add("WH_QTY", typeof(decimal));
            StockTable.Columns.Add("T_QTY", typeof(decimal));
            StockTable.Columns.Add("FLOOR_NO", typeof(Int16));
            StockTable.Columns.Add("COUNTER", typeof(int));
            StockTable.Columns.Add("SHELF", typeof(int));
            StockTable.Columns.Add("SHELF_QTY", typeof(decimal));
            StockTable.Columns.Add("SHELF_FACING", typeof(decimal));
            StockTable.Columns.Add("LSDATE", typeof(DateTime));
            StockTable.Columns.Add("LPDATE", typeof(DateTime));
            StockTable.Columns.Add("ALLOW_NEGATIVE", typeof(bool));
            StockTable.Columns.Add("AreaCovered", typeof(decimal));
            StockTable.Columns.Add("IsActive", typeof(bool));
            StockTable.Columns.Add("MAX_QTY", typeof(decimal));
            StockTable.Columns.Add("MIN_QTY", typeof(decimal));
            StockTable.Columns.Add("OP_STOCK", typeof(decimal));
            StockTable.Columns.Add("OP_STOCK_VAL", typeof(decimal));
            StockTable.Columns.Add("NeedsReplication", typeof(bool));
            StockTable.Columns.Add("NeedsUpdation", typeof(bool));

            try
            {
                try
                {
                    if (sachiChakkiConnection.State == ConnectionState.Closed)
                    {
                        sachiChakkiConnection.Open();
                    }
                }
                catch (Exception ex)
                {
                    return "Unable to connect with Sachi Chakki DB, Please try again.";
                }

                SqlCommand cmd = new SqlCommand(@"select LOCNO,ITEM_CODE,QTY,WH_QTY,T_QTY,FLOOR_NO,COUNTER,SHELF,SHELF_QTY,SHELF_FACING,LSDATE,LPDATE,ALLOW_NEGATIVE,
                                     AreaCovered,IsActive,MAX_QTY,MIN_QTY,OP_STOCK,OP_STOCK_VAL,NeedsReplication,NeedsUpdation from PS000 WHERE IsActive = 1", sachiChakkiConnection);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        try
                        {
                            var stockRow = StockTable.NewRow();
                            if (dr["LOCNO"] != null)
                                stockRow["LOCNO"] = dr["LOCNO"];
                            if (dr["ITEM_CODE"] != null)
                                stockRow["ITEM_CODE"] = dr["ITEM_CODE"];
                            if (dr["QTY"] != null)
                                stockRow["QTY"] = dr["QTY"];
                            if (dr["WH_QTY"] != null)
                                stockRow["WH_QTY"] = dr["WH_QTY"];
                            if (dr["T_QTY"] != null)
                                stockRow["T_QTY"] = dr["T_QTY"];
                            if (dr["FLOOR_NO"] != null)
                                stockRow["FLOOR_NO"] = dr["FLOOR_NO"];
                            if (dr["COUNTER"] != null)
                                stockRow["COUNTER"] = dr["COUNTER"];
                            if (dr["SHELF"] != null)
                                stockRow["SHELF"] = dr["SHELF"];
                            if (dr["SHELF_QTY"] != null)
                                stockRow["SHELF_QTY"] = dr["SHELF_QTY"];
                            if (dr["SHELF_FACING"] != null)
                                stockRow["SHELF_FACING"] = dr["SHELF_FACING"];
                            if (dr["LSDATE"] != null)
                                stockRow["LSDATE"] = dr["LSDATE"];
                            if (dr["LPDATE"] != null)
                                stockRow["LPDATE"] = dr["LPDATE"];
                            if (dr["ALLOW_NEGATIVE"] != null)
                                stockRow["ALLOW_NEGATIVE"] = dr["ALLOW_NEGATIVE"];
                            if (dr["AreaCovered"] != null)
                                stockRow["AreaCovered"] = dr["AreaCovered"];
                            if (dr["IsActive"] != null)
                                stockRow["IsActive"] = dr["IsActive"];
                            if (dr["MAX_QTY"] != null)
                                stockRow["MAX_QTY"] = dr["MAX_QTY"];
                            if (dr["MIN_QTY"] != null)
                                stockRow["MIN_QTY"] = dr["MIN_QTY"];
                            if (dr["OP_STOCK"] != null)
                                stockRow["OP_STOCK"] = dr["OP_STOCK"];
                            if (dr["OP_STOCK_VAL"] != null)
                                stockRow["OP_STOCK_VAL"] = dr["OP_STOCK_VAL"];
                            if (dr["NeedsReplication"] != null)
                                stockRow["NeedsReplication"] = dr["NeedsReplication"];
                            if (dr["NeedsUpdation"] != null)
                                stockRow["NeedsUpdation"] = dr["NeedsUpdation"];

                            StockTable.Rows.Add(stockRow);
                        }
                        catch (Exception ex)
                        {
                            return "During syncing stocks Sachi Chakki DB connection closed, Please try again.";
                        }
                    }
                }
                try
                {
                    DeleteStock();

                    try
                    {
                        SqlCommand cmd1 = new SqlCommand("SP_InsertStock", charsuConnection);
                        cmd1.CommandText = "SP_InsertStock";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        SqlParameter sqlParameter = cmd1.Parameters.AddWithValue("@StockType", StockTable);
                        sqlParameter.SqlDbType = SqlDbType.Structured;
                        cmd1.ExecuteNonQuery();

                        SqlCommand cmd2 = new SqlCommand("SP_UpdateStock", charsuConnection);
                        cmd2.CommandText = "SP_UpdateStock";
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return "Unable to save stocks in Chaarsu DB, Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    return "Unable to connect with Sachi Chakki DB, Please try again.";
                }
            }
            catch (Exception ex)
            {
                return "Unable to connect with Sachi Chakki DB, Please try again.";
            }

            return "Success";
        }

        public string GetAllProducts()
        {
            DataTable ItemInfoTable = new DataTable();
            ItemInfoTable.Columns.Add("ITEM_CODE", typeof(int));
            ItemInfoTable.Columns.Add("HV_CODE", typeof(string));
            ItemInfoTable.Columns.Add("ITEM_TYPE", typeof(int));
            ItemInfoTable.Columns.Add("ITEM_ATTRIB", typeof(int));
            ItemInfoTable.Columns.Add("IsRECIPE", typeof(bool));
            ItemInfoTable.Columns.Add("SERIALIZED", typeof(bool));
            ItemInfoTable.Columns.Add("EXPIRY_ITEM", typeof(bool));
            ItemInfoTable.Columns.Add("FEATURED", typeof(bool));
            ItemInfoTable.Columns.Add("UOM", typeof(string));
            ItemInfoTable.Columns.Add("BATCH_QTY", typeof(decimal));
            ItemInfoTable.Columns.Add("DEPT_CODE", typeof(int));
            ItemInfoTable.Columns.Add("GRCODE", typeof(int));
            ItemInfoTable.Columns.Add("SUBGRCODE", typeof(int));
            ItemInfoTable.Columns.Add("CATCODE", typeof(int));
            ItemInfoTable.Columns.Add("BRAND_CODE", typeof(int));
            ItemInfoTable.Columns.Add("DESIGN_CD", typeof(int));
            ItemInfoTable.Columns.Add("CLR_CODE", typeof(int));
            //ItemInfoTable.Columns.Add("SUPP_CODE", typeof(string));
            ItemInfoTable.Columns.Add("MAKE_CODE", typeof(int));
            ItemInfoTable.Columns.Add("SIZE_CODE", typeof(int));
            ItemInfoTable.Columns.Add("AUTH_CODE", typeof(int));
            ItemInfoTable.Columns.Add("PUB_CODE", typeof(int));
            ItemInfoTable.Columns.Add("EDITION", typeof(string));
            ItemInfoTable.Columns.Add("CLASS", typeof(string));
            ItemInfoTable.Columns.Add("ISBN", typeof(string));
            ItemInfoTable.Columns.Add("ITEM_DESC", typeof(string));
            ItemInfoTable.Columns.Add("ITEM_DESC_LONG", typeof(string));
            //ItemInfoTable.Columns.Add("COST_MARGIN", typeof(decimal));
            ItemInfoTable.Columns.Add("SEX", typeof(string));
            ItemInfoTable.Columns.Add("SEASON", typeof(string));
            ItemInfoTable.Columns.Add("AGE", typeof(string));
            ItemInfoTable.Columns.Add("FABRIC", typeof(string));
            //ItemInfoTable.Columns.Add("SPEED", typeof(string));
            //ItemInfoTable.Columns.Add("PLY", typeof(string));
            //ItemInfoTable.Columns.Add("PCD", typeof(string));
            //ItemInfoTable.Columns.Add("HOLES", typeof(string));
            //ItemInfoTable.Columns.Add("SPH", typeof(string));
            //ItemInfoTable.Columns.Add("CYL", typeof(string));
            //ItemInfoTable.Columns.Add("AXIS", typeof(string));
            //ItemInfoTable.Columns.Add("ADDS", typeof(string));
            ItemInfoTable.Columns.Add("COMMENT", typeof(string));
            ItemInfoTable.Columns.Add("EXEMPT", typeof(bool));
            ItemInfoTable.Columns.Add("VAT", typeof(decimal));
            ItemInfoTable.Columns.Add("GST", typeof(decimal));
            ItemInfoTable.Columns.Add("OPEN_PRICE", typeof(bool));
            ItemInfoTable.Columns.Add("FRACTIONAL", typeof(bool));
            ItemInfoTable.Columns.Add("ALLOWQTY", typeof(bool));
            //ItemInfoTable.Columns.Add("MATERIAL_COST", typeof(decimal));
            //ItemInfoTable.Columns.Add("INGR_COST", typeof(decimal));
            //ItemInfoTable.Columns.Add("PKG_COST", typeof(decimal));
            //ItemInfoTable.Columns.Add("LAB_COST", typeof(decimal));
            //ItemInfoTable.Columns.Add("OH_COST", typeof(decimal));
            //ItemInfoTable.Columns.Add("OTH_COST", typeof(decimal));
            //ItemInfoTable.Columns.Add("WASTAGE", typeof(decimal));
            //ItemInfoTable.Columns.Add("COST_PRICE1", typeof(decimal));
            //ItemInfoTable.Columns.Add("COST_PRICE2", typeof(decimal));
            //ItemInfoTable.Columns.Add("COST_PRICE3", typeof(decimal));
            //ItemInfoTable.Columns.Add("NET_COST", typeof(decimal));
            //ItemInfoTable.Columns.Add("AVG_COST", typeof(decimal));
            //ItemInfoTable.Columns.Add("AVG_COST1", typeof(decimal));
            //ItemInfoTable.Columns.Add("AVG_COST2", typeof(decimal));
            //ItemInfoTable.Columns.Add("AVG_COST3", typeof(decimal));
            //ItemInfoTable.Columns.Add("F_PUR_PRICE", typeof(decimal));
            //ItemInfoTable.Columns.Add("LAST_PUR_PRICE1", typeof(decimal));
            //ItemInfoTable.Columns.Add("LAST_PUR_PRICE2", typeof(decimal));
            //ItemInfoTable.Columns.Add("LAST_PUR_PRICE3", typeof(decimal));
            //ItemInfoTable.Columns.Add("LAST_SUPP", typeof(string));
            //ItemInfoTable.Columns.Add("LAST_SUPP1", typeof(string));
            //ItemInfoTable.Columns.Add("LAST_SUPP2", typeof(string));
            //ItemInfoTable.Columns.Add("FREE_QTY", typeof(decimal));
            //ItemInfoTable.Columns.Add("FREE_QTY_AVG_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("ITEM_DISC", typeof(decimal));
            ItemInfoTable.Columns.Add("bDISCOUNTED", typeof(bool));
            ItemInfoTable.Columns.Add("DISC_QTY", typeof(decimal));
            ItemInfoTable.Columns.Add("SALE_DISC", typeof(decimal));
            ItemInfoTable.Columns.Add("CDATE", typeof(DateTime));
            ItemInfoTable.Columns.Add("CUSER", typeof(int));
            ItemInfoTable.Columns.Add("MDATE", typeof(DateTime));
            ItemInfoTable.Columns.Add("MUSER", typeof(int));
            //ItemInfoTable.Columns.Add("EMPTY", typeof(bool));
            ItemInfoTable.Columns.Add("bNEW", typeof(bool));
            ItemInfoTable.Columns.Add("NeedsReplication", typeof(bool));

            try
            {
                try
                {
                    if (sachiChakkiConnection.State == ConnectionState.Closed)
                    {
                        sachiChakkiConnection.Open();
                    }
                }
                catch (Exception ex)
                {
                    return "Unable to connect with Sachi Chakki DB, Please try again.";
                }

                SqlCommand cmd = new SqlCommand(@"select ITEM_CODE,HV_CODE,ITEM_ATTRIB,IsRECIPE,SERIALIZED,EXPIRY_ITEM,FEATURED,UOM,BATCH_QTY,DEPT_CODE,GRCODE,SUBGRCODE,CATCODE,BRAND_CODE,DESIGN_CD,CLR_CODE,
                                                  MAKE_CODE,SIZE_CODE,AUTH_CODE,PUB_CODE,EDITION,CLASS,ISBN,ITEM_DESC,ITEM_DESC_LONG,SEX,SEASON,AGE,FABRIC,COMMENT,EXEMPT,VAT,GST,OPEN_PRICE,FRACTIONAL,
                                                  ALLOWQTY,ITEM_DISC,bDISCOUNTED,DISC_QTY,SALE_DISC,CDATE,CUSER,MDATE,MUSER,bNEW,NeedsReplication from ITEMINFO", sachiChakkiConnection);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        try
                        {
                            var ItemInfoRow = ItemInfoTable.NewRow();
                            if (dr["ITEM_CODE"] != null)
                                ItemInfoRow["ITEM_CODE"] = dr["ITEM_CODE"];
                            if (dr["HV_CODE"] != null)
                                ItemInfoRow["HV_CODE"] = dr["HV_CODE"];
                            if (dr["ITEM_ATTRIB"] != null)
                                ItemInfoRow["ITEM_ATTRIB"] = dr["ITEM_ATTRIB"];
                            if (dr["IsRECIPE"] != null)
                                ItemInfoRow["IsRECIPE"] = dr["IsRECIPE"];
                            if (dr["SERIALIZED"] != null)
                                ItemInfoRow["SERIALIZED"] = dr["SERIALIZED"];
                            if (dr["EXPIRY_ITEM"] != null)
                                ItemInfoRow["EXPIRY_ITEM"] = dr["EXPIRY_ITEM"];
                            if (dr["FEATURED"] != null)
                                ItemInfoRow["FEATURED"] = dr["FEATURED"];
                            if (dr["UOM"] != null)
                                ItemInfoRow["UOM"] = dr["UOM"];
                            if (dr["BATCH_QTY"] != null)
                                ItemInfoRow["BATCH_QTY"] = dr["BATCH_QTY"];
                            if (dr["DEPT_CODE"] != null)
                                ItemInfoRow["DEPT_CODE"] = dr["DEPT_CODE"];
                            if (dr["GRCODE"] != null)
                                ItemInfoRow["GRCODE"] = dr["GRCODE"];
                            if (dr["SUBGRCODE"] != null)
                                ItemInfoRow["SUBGRCODE"] = dr["SUBGRCODE"];
                            if (dr["CATCODE"] != null)
                                ItemInfoRow["CATCODE"] = dr["CATCODE"];
                            if (dr["BRAND_CODE"] != null)
                                ItemInfoRow["BRAND_CODE"] = dr["BRAND_CODE"];
                            if (dr["DESIGN_CD"] != null)
                                ItemInfoRow["DESIGN_CD"] = dr["DESIGN_CD"];
                            if (dr["CLR_CODE"] != null)
                                ItemInfoRow["CLR_CODE"] = dr["CLR_CODE"];
                            //if (dr["SUPP_CODE"] != null)
                            //    ItemInfoRow["SUPP_CODE"] = dr["SUPP_CODE"];
                            if (dr["MAKE_CODE"] != null)
                                ItemInfoRow["MAKE_CODE"] = dr["MAKE_CODE"];
                            if (dr["SIZE_CODE"] != null)
                                ItemInfoRow["SIZE_CODE"] = dr["SIZE_CODE"];
                            if (dr["AUTH_CODE"] != null)
                                ItemInfoRow["AUTH_CODE"] = dr["AUTH_CODE"];
                            if (dr["PUB_CODE"] != null)
                                ItemInfoRow["PUB_CODE"] = dr["PUB_CODE"];
                            if (dr["EDITION"] != null)
                                ItemInfoRow["EDITION"] = dr["EDITION"];
                            if (dr["CLASS"] != null)
                                ItemInfoRow["CLASS"] = dr["CLASS"];
                            if (dr["ISBN"] != null)
                                ItemInfoRow["ISBN"] = dr["ISBN"];
                            if (dr["ITEM_DESC"] != null)
                                ItemInfoRow["ITEM_DESC"] = dr["ITEM_DESC"];
                            if (dr["ITEM_DESC_LONG"] != null)
                                ItemInfoRow["ITEM_DESC_LONG"] = dr["ITEM_DESC_LONG"];
                            if (dr["SEX"] != null)
                                ItemInfoRow["SEX"] = dr["SEX"];
                            if (dr["SEASON"] != null)
                                ItemInfoRow["SEASON"] = dr["SEASON"];
                            if (dr["AGE"] != null)
                                ItemInfoRow["AGE"] = dr["AGE"];
                            if (dr["FABRIC"] != null)
                                ItemInfoRow["FABRIC"] = dr["FABRIC"];
                            if (dr["COMMENT"] != null)
                                ItemInfoRow["COMMENT"] = dr["COMMENT"];
                            if (dr["EXEMPT"] != null)
                                ItemInfoRow["EXEMPT"] = dr["EXEMPT"];
                            if (dr["VAT"] != null)
                                ItemInfoRow["VAT"] = dr["VAT"];
                            if (dr["GST"] != null)
                                ItemInfoRow["GST"] = dr["GST"];
                            if (dr["OPEN_PRICE"] != null)
                                ItemInfoRow["OPEN_PRICE"] = dr["OPEN_PRICE"];
                            if (dr["FRACTIONAL"] != null)
                                ItemInfoRow["FRACTIONAL"] = dr["FRACTIONAL"];
                            if (dr["ALLOWQTY"] != null)
                                ItemInfoRow["ALLOWQTY"] = dr["ALLOWQTY"];
                            if (dr["ITEM_DISC"] != null)
                                ItemInfoRow["ITEM_DISC"] = dr["ITEM_DISC"];
                            if (dr["bDISCOUNTED"] != null)
                                ItemInfoRow["bDISCOUNTED"] = dr["bDISCOUNTED"];
                            if (dr["DISC_QTY"] != null)
                                ItemInfoRow["DISC_QTY"] = dr["DISC_QTY"];
                            if (dr["SALE_DISC"] != null)
                                ItemInfoRow["SALE_DISC"] = dr["SALE_DISC"];
                            if (dr["CDATE"] != null)
                                ItemInfoRow["CDATE"] = dr["CDATE"];
                            if (dr["CUSER"] != null)
                                ItemInfoRow["CUSER"] = dr["CUSER"];
                            if (dr["MDATE"] != null)
                                ItemInfoRow["MDATE"] = dr["MDATE"];
                            if (dr["MUSER"] != null)
                                ItemInfoRow["MUSER"] = dr["MUSER"];
                            if (dr["bNEW"] != null)
                                ItemInfoRow["bNEW"] = dr["bNEW"];
                            if (dr["NeedsReplication"] != null)
                                ItemInfoRow["NeedsReplication"] = dr["NeedsReplication"];

                            ItemInfoTable.Rows.Add(ItemInfoRow);
                        }
                        catch (Exception ex)
                        {
                            return "During syncing products Sachi Chakki DB connection closed, Please try again.";
                        }
                    }
                }
                try
                {

                    DeleteProduct();
                    try
                    {
                        SqlCommand cmd1 = new SqlCommand("SP_InsertItemInfo", charsuConnection);
                        cmd1.CommandText = "SP_InsertItemInfo";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        SqlParameter sqlParameter = cmd1.Parameters.AddWithValue("@ItemInfoType", ItemInfoTable);
                        sqlParameter.SqlDbType = SqlDbType.Structured;
                        cmd1.ExecuteNonQuery();

                        SqlCommand cmd2 = new SqlCommand("SP_InsertProductFromItemInfo", charsuConnection);
                        cmd2.CommandText = "SP_InsertProductFromItemInfo";
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return "Unable to save products in Chaarsu DB, Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    return "Unable to connect with Sachi Chakki DB, Please try again.";
                }
            }
            catch (Exception ex)
            {
                return "Unable to connect with Sachi Chakki DB, Please try again.";
            }

            return "Success";
        }
    }
}