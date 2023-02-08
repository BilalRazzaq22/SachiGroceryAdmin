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
        readonly string charsuDb = "data source=68.66.211.65;initial catalog=anytimea_GROCERY;user id=sa;password=:gRqlrv3wsR1lw;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        readonly string sachiChakkiDb = "data source=sachichakkiwh.mine.nu;initial catalog=BIZPRO_WH;user id=sa;password=golden@3864;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        readonly SqlConnection charsuConnection = null;
        readonly SqlConnection sachiChakkiConnection = null;
        private static DataSyncService _instance;
        public DataSyncService()
        {
            charsuConnection = new SqlConnection(charsuDb);
            sachiChakkiConnection = new SqlConnection(sachiChakkiDb);
            charsuConnection.Open();
            sachiChakkiConnection.Open();

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

        public void StartFullSyncing()
        {
            try
            {
                BarcodeSync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void BarcodeSync()
        {
            try
            {
                GetAllBarcodes();
            }
            catch (Exception ex)
            {
            }
            finally
            {
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

        public void StockSync()
        {
            try
            {

                GetAllStocks();
            }
            catch (Exception ex)
            {
            }
            finally
            {
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

        public void ProductSync()
        {
            try
            {

                GetAllProducts();
            }
            catch (Exception ex)
            {
            }
            finally
            {
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

        public void GetAllBarcodes()
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
            //BarcodeTable.Columns.Add("NeedsReplication", typeof(bool));
            //BarcodeTable.Columns.Add("NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P2NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P3NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P4NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P5NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P6NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P7NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P8NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P9NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1P10NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P2NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P3NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P4NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P5NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P6NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P7NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P8NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P9NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2P10NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L0NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L2NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L3NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L3P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L4NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L5NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L6NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L3P2NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L3P3NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L3P4NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L3P5NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L3P6NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L4P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L4P2NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L4P3NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L4P4NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L4P5NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L4P6NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L5P6NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L5P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L5P2NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L5P3NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L5P4NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L5P5NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L6P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L6P2NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L6P3NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L6P4NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L6P5NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L6P6NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L7NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L7P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L7P2NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L7P3NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L7P4NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L7P5NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L7P6NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L8NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L9NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L10NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L11NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L12NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L8P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L9P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L10P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L11P1NeedsUpdation", typeof(bool));
            //BarcodeTable.Columns.Add("L12P1NeedsUpdation", typeof(bool));

            try
            {
                //using (SqlConnection con = new SqlConnection(sachiChakkiDb))
                //{
                //    con.Open();

                if(sachiChakkiConnection.State == ConnectionState.Closed)
                {
                    sachiChakkiConnection.Open();
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
                            //if (dr["NeedsReplication"] != null)
                            //    barcodeRow["NeedsReplication"] = dr["NeedsReplication"];
                            //if (dr["NeedsUpdation"] != null)
                            //    barcodeRow["NeedsUpdation"] = dr["NeedsUpdation"];
                            //if (dr["L1P1NeedsUpdation"] != null)
                            //    barcodeRow["L1P1NeedsUpdation"] = dr["L1P1NeedsUpdation"];
                            //if (dr["L1P2NeedsUpdation"] != null)
                            //    barcodeRow["L1P2NeedsUpdation"] = dr["L1P2NeedsUpdation"];
                            //if (dr["L1P3NeedsUpdation"] != null)
                            //    barcodeRow["L1P3NeedsUpdation"] = dr["L1P3NeedsUpdation"];
                            //if (dr["L1P4NeedsUpdation"] != null)
                            //    barcodeRow["L1P4NeedsUpdation"] = dr["L1P4NeedsUpdation"];
                            //if (dr["L1P5NeedsUpdation"] != null)
                            //    barcodeRow["L1P5NeedsUpdation"] = dr["L1P5NeedsUpdation"];
                            //if (dr["L1P6NeedsUpdation"] != null)
                            //    barcodeRow["L1P6NeedsUpdation"] = dr["L1P6NeedsUpdation"];
                            //if (dr["L1P7NeedsUpdation"] != null)
                            //    barcodeRow["L1P7NeedsUpdation"] = dr["L1P7NeedsUpdation"];
                            //if (dr["L1P8NeedsUpdation"] != null)
                            //    barcodeRow["L1P8NeedsUpdation"] = dr["L1P8NeedsUpdation"];
                            //if (dr["L1P9NeedsUpdation"] != null)
                            //    barcodeRow["L1P9NeedsUpdation"] = dr["L1P9NeedsUpdation"];
                            //if (dr["L1P10NeedsUpdation"] != null)
                            //    barcodeRow["L1P10NeedsUpdation"] = dr["L1P10NeedsUpdation"];
                            //if (dr["L2P1NeedsUpdation"] != null)
                            //    barcodeRow["L2P1NeedsUpdation"] = dr["L2P1NeedsUpdation"];
                            //if (dr["L2P2NeedsUpdation"] != null)
                            //    barcodeRow["L2P2NeedsUpdation"] = dr["L2P2NeedsUpdation"];
                            //if (dr["L2P3NeedsUpdation"] != null)
                            //    barcodeRow["L2P3NeedsUpdation"] = dr["L2P3NeedsUpdation"];
                            //if (dr["L2P4NeedsUpdation"] != null)
                            //    barcodeRow["L2P4NeedsUpdation"] = dr["L2P4NeedsUpdation"];
                            //if (dr["L2P5NeedsUpdation"] != null)
                            //    barcodeRow["L2P5NeedsUpdation"] = dr["L2P5NeedsUpdation"];
                            //if (dr["L2P6NeedsUpdation"] != null)
                            //    barcodeRow["L2P6NeedsUpdation"] = dr["L2P6NeedsUpdation"];
                            //if (dr["L2P7NeedsUpdation"] != null)
                            //    barcodeRow["L2P7NeedsUpdation"] = dr["L2P7NeedsUpdation"];
                            //if (dr["L2P8NeedsUpdation"] != null)
                            //    barcodeRow["L2P8NeedsUpdation"] = dr["L2P8NeedsUpdation"];
                            //if (dr["L2P9NeedsUpdation"] != null)
                            //    barcodeRow["L2P9NeedsUpdation"] = dr["L2P9NeedsUpdation"];
                            //if (dr["L2P10NeedsUpdation"] != null)
                            //    barcodeRow["L2P10NeedsUpdation"] = dr["L2P10NeedsUpdation"];
                            //if (dr["L0NeedsUpdation"] != null)
                            //    barcodeRow["L0NeedsUpdation"] = dr["L0NeedsUpdation"];
                            //if (dr["L1NeedsUpdation"] != null)
                            //    barcodeRow["L1NeedsUpdation"] = dr["L1NeedsUpdation"];
                            //if (dr["L2NeedsUpdation"] != null)
                            //    barcodeRow["L2NeedsUpdation"] = dr["L2NeedsUpdation"];
                            //if (dr["L3NeedsUpdation"] != null)
                            //    barcodeRow["L3NeedsUpdation"] = dr["L3NeedsUpdation"];
                            //if (dr["L3P1NeedsUpdation"] != null)
                            //    barcodeRow["L3P1NeedsUpdation"] = dr["L3P1NeedsUpdation"];
                            //if (dr["L4NeedsUpdation"] != null)
                            //    barcodeRow["L4NeedsUpdation"] = dr["L4NeedsUpdation"];
                            //if (dr["L5NeedsUpdation"] != null)
                            //    barcodeRow["L5NeedsUpdation"] = dr["L5NeedsUpdation"];
                            //if (dr["L6NeedsUpdation"] != null)
                            //    barcodeRow["L6NeedsUpdation"] = dr["L6NeedsUpdation"];
                            //if (dr["L3P2NeedsUpdation"] != null)
                            //    barcodeRow["L3P2NeedsUpdation"] = dr["L3P2NeedsUpdation"];
                            //if (dr["L3P3NeedsUpdation"] != null)
                            //    barcodeRow["L3P3NeedsUpdation"] = dr["L3P3NeedsUpdation"];
                            //if (dr["L3P4NeedsUpdation"] != null)
                            //    barcodeRow["L3P4NeedsUpdation"] = dr["L3P4NeedsUpdation"];
                            //if (dr["L3P5NeedsUpdation"] != null)
                            //    barcodeRow["L3P5NeedsUpdation"] = dr["L3P5NeedsUpdation"];
                            //if (dr["L3P6NeedsUpdation"] != null)
                            //    barcodeRow["L3P6NeedsUpdation"] = dr["L3P6NeedsUpdation"];
                            //if (dr["L4P1NeedsUpdation"] != null)
                            //    barcodeRow["L4P1NeedsUpdation"] = dr["L4P1NeedsUpdation"];
                            //if (dr["L4P2NeedsUpdation"] != null)
                            //    barcodeRow["L4P2NeedsUpdation"] = dr["L4P2NeedsUpdation"];
                            //if (dr["L4P3NeedsUpdation"] != null)
                            //    barcodeRow["L4P3NeedsUpdation"] = dr["L4P3NeedsUpdation"];
                            //if (dr["L4P4NeedsUpdation"] != null)
                            //    barcodeRow["L4P4NeedsUpdation"] = dr["L4P4NeedsUpdation"];
                            //if (dr["L4P5NeedsUpdation"] != null)
                            //    barcodeRow["L4P5NeedsUpdation"] = dr["L4P5NeedsUpdation"];
                            //if (dr["L4P6NeedsUpdation"] != null)
                            //    barcodeRow["L4P6NeedsUpdation"] = dr["L4P6NeedsUpdation"];
                            //if (dr["L5P6NeedsUpdation"] != null)
                            //    barcodeRow["L5P6NeedsUpdation"] = dr["L5P6NeedsUpdation"];
                            //if (dr["L5P1NeedsUpdation"] != null)
                            //    barcodeRow["L5P1NeedsUpdation"] = dr["L5P1NeedsUpdation"];
                            //if (dr["L5P2NeedsUpdation"] != null)
                            //    barcodeRow["L5P2NeedsUpdation"] = dr["L5P2NeedsUpdation"];
                            //if (dr["L5P3NeedsUpdation"] != null)
                            //    barcodeRow["L5P3NeedsUpdation"] = dr["L5P3NeedsUpdation"];
                            //if (dr["L5P4NeedsUpdation"] != null)
                            //    barcodeRow["L5P4NeedsUpdation"] = dr["L5P4NeedsUpdation"];
                            //if (dr["L5P5NeedsUpdation"] != null)
                            //    barcodeRow["L5P5NeedsUpdation"] = dr["L5P5NeedsUpdation"];
                            //if (dr["L6P1NeedsUpdation"] != null)
                            //    barcodeRow["L6P1NeedsUpdation"] = dr["L6P1NeedsUpdation"];
                            //if (dr["L6P2NeedsUpdation"] != null)
                            //    barcodeRow["L6P2NeedsUpdation"] = dr["L6P2NeedsUpdation"];
                            //if (dr["L6P3NeedsUpdation"] != null)
                            //    barcodeRow["L6P3NeedsUpdation"] = dr["L6P3NeedsUpdation"];
                            //if (dr["L6P4NeedsUpdation"] != null)
                            //    barcodeRow["L6P4NeedsUpdation"] = dr["L6P4NeedsUpdation"];
                            //if (dr["L6P5NeedsUpdation"] != null)
                            //    barcodeRow["L6P5NeedsUpdation"] = dr["L6P5NeedsUpdation"];
                            //if (dr["L6P6NeedsUpdation"] != null)
                            //    barcodeRow["L6P6NeedsUpdation"] = dr["L6P6NeedsUpdation"];
                            //if (dr["L7NeedsUpdation"] != null)
                            //    barcodeRow["L7NeedsUpdation"] = dr["L7NeedsUpdation"];
                            //if (dr["L7P1NeedsUpdation"] != null)
                            //    barcodeRow["L7P1NeedsUpdation"] = dr["L7P1NeedsUpdation"];
                            //if (dr["L7P2NeedsUpdation"] != null)
                            //    barcodeRow["L7P2NeedsUpdation"] = dr["L7P2NeedsUpdation"];
                            //if (dr["L7P3NeedsUpdation"] != null)
                            //    barcodeRow["L7P3NeedsUpdation"] = dr["L7P3NeedsUpdation"];
                            //if (dr["L7P4NeedsUpdation"] != null)
                            //    barcodeRow["L7P4NeedsUpdation"] = dr["L7P4NeedsUpdation"];
                            //if (dr["L7P5NeedsUpdation"] != null)
                            //    barcodeRow["L7P5NeedsUpdation"] = dr["L7P5NeedsUpdation"];
                            //if (dr["L7P6NeedsUpdation"] != null)
                            //    barcodeRow["L7P6NeedsUpdation"] = dr["L7P6NeedsUpdation"];
                            //if (dr["L8NeedsUpdation"] != null)
                            //    barcodeRow["L8NeedsUpdation"] = dr["L8NeedsUpdation"];
                            //if (dr["L9NeedsUpdation"] != null)
                            //    barcodeRow["L9NeedsUpdation"] = dr["L9NeedsUpdation"];
                            //if (dr["L10NeedsUpdation"] != null)
                            //    barcodeRow["L10NeedsUpdation"] = dr["L10NeedsUpdation"];
                            //if (dr["L11NeedsUpdation"] != null)
                            //    barcodeRow["L11NeedsUpdation"] = dr["L11NeedsUpdation"];
                            //if (dr["L12NeedsUpdation"] != null)
                            //    barcodeRow["L12NeedsUpdation"] = dr["L12NeedsUpdation"];
                            //if (dr["L8P1NeedsUpdation"] != null)
                            //    barcodeRow["L8P1NeedsUpdation"] = dr["L8P1NeedsUpdation"];
                            //if (dr["L9P1NeedsUpdation"] != null)
                            //    barcodeRow["L9P1NeedsUpdation"] = dr["L9P1NeedsUpdation"];
                            //if (dr["L10P1NeedsUpdation"] != null)
                            //    barcodeRow["L10P1NeedsUpdation"] = dr["L10P1NeedsUpdation"];
                            //if (dr["L11P1NeedsUpdation"] != null)
                            //    barcodeRow["L11P1NeedsUpdation"] = dr["L11P1NeedsUpdation"];
                            //if (dr["L12P1NeedsUpdation"] != null)
                            //    barcodeRow["L12P1NeedsUpdation"] = dr["L12P1NeedsUpdation"];

                            BarcodeTable.Rows.Add(barcodeRow);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                try
                {

                    DeleteBarcode();

                    //using (SqlConnection con1 = new SqlConnection(charsuDb))
                    //{
                    //con.Open();
                    SqlCommand cmd1 = new SqlCommand("SP_InsertBarcode", charsuConnection);
                    cmd1.CommandText = "SP_InsertBarcode";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = cmd1.Parameters.AddWithValue("@BarcodeType", BarcodeTable);
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    cmd1.ExecuteNonQuery();
                    //}

                    //StockSync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void GetAllStocks()
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
                //using (SqlConnection con = new SqlConnection(sachiChakkiDb))
                //{
                //    con.Open();

                if (sachiChakkiConnection.State == ConnectionState.Closed)
                {
                    sachiChakkiConnection.Open();
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
                            throw ex;
                        }
                    }
                }
                try
                {
                    DeleteStock();

                    //using (SqlConnection con1 = new SqlConnection(charsuDb))
                    //{
                    //    con1.Open();
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

                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void GetAllProducts()
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
            ItemInfoTable.Columns.Add("SUPP_CODE", typeof(string));
            ItemInfoTable.Columns.Add("MAKE_CODE", typeof(int));
            ItemInfoTable.Columns.Add("SIZE_CODE", typeof(int));
            ItemInfoTable.Columns.Add("AUTH_CODE", typeof(int));
            ItemInfoTable.Columns.Add("PUB_CODE", typeof(int));
            ItemInfoTable.Columns.Add("EDITION", typeof(string));
            ItemInfoTable.Columns.Add("CLASS", typeof(string));
            ItemInfoTable.Columns.Add("ISBN", typeof(string));
            ItemInfoTable.Columns.Add("ITEM_DESC", typeof(string));
            ItemInfoTable.Columns.Add("ITEM_DESC_LONG", typeof(string));
            ItemInfoTable.Columns.Add("COST_MARGIN", typeof(decimal));
            ItemInfoTable.Columns.Add("SEX", typeof(string));
            ItemInfoTable.Columns.Add("SEASON", typeof(string));
            ItemInfoTable.Columns.Add("AGE", typeof(string));
            ItemInfoTable.Columns.Add("FABRIC", typeof(string));
            ItemInfoTable.Columns.Add("SPEED", typeof(string));
            ItemInfoTable.Columns.Add("PLY", typeof(string));
            ItemInfoTable.Columns.Add("PCD", typeof(string));
            ItemInfoTable.Columns.Add("HOLES", typeof(string));
            ItemInfoTable.Columns.Add("SPH", typeof(string));
            ItemInfoTable.Columns.Add("CYL", typeof(string));
            ItemInfoTable.Columns.Add("AXIS", typeof(string));
            ItemInfoTable.Columns.Add("ADDS", typeof(string));
            ItemInfoTable.Columns.Add("COMMENT", typeof(string));
            ItemInfoTable.Columns.Add("EXEMPT", typeof(bool));
            ItemInfoTable.Columns.Add("VAT", typeof(decimal));
            ItemInfoTable.Columns.Add("GST", typeof(decimal));
            ItemInfoTable.Columns.Add("OPEN_PRICE", typeof(bool));
            ItemInfoTable.Columns.Add("FRACTIONAL", typeof(bool));
            ItemInfoTable.Columns.Add("ALLOWQTY", typeof(bool));
            ItemInfoTable.Columns.Add("MATERIAL_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("INGR_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("PKG_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("LAB_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("OH_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("OTH_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("WASTAGE", typeof(decimal));
            ItemInfoTable.Columns.Add("COST_PRICE1", typeof(decimal));
            ItemInfoTable.Columns.Add("COST_PRICE2", typeof(decimal));
            ItemInfoTable.Columns.Add("COST_PRICE3", typeof(decimal));
            ItemInfoTable.Columns.Add("NET_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("AVG_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("AVG_COST1", typeof(decimal));
            ItemInfoTable.Columns.Add("AVG_COST2", typeof(decimal));
            ItemInfoTable.Columns.Add("AVG_COST3", typeof(decimal));
            ItemInfoTable.Columns.Add("F_PUR_PRICE", typeof(decimal));
            ItemInfoTable.Columns.Add("LAST_PUR_PRICE1", typeof(decimal));
            ItemInfoTable.Columns.Add("LAST_PUR_PRICE2", typeof(decimal));
            ItemInfoTable.Columns.Add("LAST_PUR_PRICE3", typeof(decimal));
            ItemInfoTable.Columns.Add("LAST_SUPP", typeof(string));
            ItemInfoTable.Columns.Add("LAST_SUPP1", typeof(string));
            ItemInfoTable.Columns.Add("LAST_SUPP2", typeof(string));
            ItemInfoTable.Columns.Add("FREE_QTY", typeof(decimal));
            ItemInfoTable.Columns.Add("FREE_QTY_AVG_COST", typeof(decimal));
            ItemInfoTable.Columns.Add("ITEM_DISC", typeof(decimal));
            ItemInfoTable.Columns.Add("bDISCOUNTED", typeof(bool));
            ItemInfoTable.Columns.Add("DISC_QTY", typeof(decimal));
            ItemInfoTable.Columns.Add("SALE_DISC", typeof(decimal));
            ItemInfoTable.Columns.Add("CDATE", typeof(DateTime));
            ItemInfoTable.Columns.Add("CUSER", typeof(int));
            ItemInfoTable.Columns.Add("MDATE", typeof(DateTime));
            ItemInfoTable.Columns.Add("MUSER", typeof(int));
            ItemInfoTable.Columns.Add("EMPTY", typeof(bool));
            ItemInfoTable.Columns.Add("bNEW", typeof(bool));
            ItemInfoTable.Columns.Add("NeedsReplication", typeof(bool));

            try
            {
                //using (SqlConnection con = new SqlConnection(sachiChakkiDb))
                //{
                //    con.Open();

                if (sachiChakkiConnection.State == ConnectionState.Closed)
                {
                    sachiChakkiConnection.Open();
                }

                SqlCommand cmd = new SqlCommand(@"select ITEM_CODE,HV_CODE,ITEM_TYPE,ITEM_ATTRIB,IsRECIPE,SERIALIZED,EXPIRY_ITEM,FEATURED,UOM,BATCH_QTY,DEPT_CODE,GRCODE,SUBGRCODE,
                                     CATCODE,BRAND_CODE,DESIGN_CD,CLR_CODE,SUPP_CODE,MAKE_CODE,SIZE_CODE,AUTH_CODE,PUB_CODE,EDITION,CLASS,ISBN,ITEM_DESC,ITEM_DESC_LONG,COST_MARGIN,SEX,SEASON,AGE,FABRIC,SPEED,
                                     PLY,PCD,HOLES,SPH,CYL,AXIS,ADDS,COMMENT,EXEMPT,VAT,GST,OPEN_PRICE,FRACTIONAL,ALLOWQTY,MATERIAL_COST,INGR_COST,PKG_COST,LAB_COST,OH_COST,OTH_COST,WASTAGE,COST_PRICE1,COST_PRICE2,
                                     COST_PRICE3,NET_COST,AVG_COST,AVG_COST1,AVG_COST2,AVG_COST3,F_PUR_PRICE,LAST_PUR_PRICE1,LAST_PUR_PRICE2,LAST_PUR_PRICE3,LAST_SUPP,LAST_SUPP1,LAST_SUPP2,FREE_QTY,FREE_QTY_AVG_COST,
                                     ITEM_DISC,bDISCOUNTED,DISC_QTY,SALE_DISC,CDATE,CUSER,MDATE,MUSER,EMPTY,bNEW,NeedsReplication from ITEMINFO", sachiChakkiConnection);
                //SqlCommand cmd3 = new SqlCommand("select Count(*) from ITEMINFO", con);
                //Int32 count = (Int32)cmd3.ExecuteScalar();
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
                            if (dr["ITEM_TYPE"] != null)
                                ItemInfoRow["ITEM_TYPE"] = dr["ITEM_TYPE"];
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
                            if (dr["SUPP_CODE"] != null)
                                ItemInfoRow["SUPP_CODE"] = dr["SUPP_CODE"];
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
                            if (dr["COST_MARGIN"] != null)
                                ItemInfoRow["COST_MARGIN"] = dr["COST_MARGIN"];
                            if (dr["SEX"] != null)
                                ItemInfoRow["SEX"] = dr["SEX"];
                            if (dr["SEASON"] != null)
                                ItemInfoRow["SEASON"] = dr["SEASON"];
                            if (dr["AGE"] != null)
                                ItemInfoRow["AGE"] = dr["AGE"];
                            if (dr["FABRIC"] != null)
                                ItemInfoRow["FABRIC"] = dr["FABRIC"];
                            if (dr["SPEED"] != null)
                                ItemInfoRow["SPEED"] = dr["SPEED"];
                            if (dr["PLY"] != null)
                                ItemInfoRow["PLY"] = dr["PLY"];
                            if (dr["PCD"] != null)
                                ItemInfoRow["PCD"] = dr["PCD"];
                            if (dr["HOLES"] != null)
                                ItemInfoRow["HOLES"] = dr["HOLES"];
                            if (dr["SPH"] != null)
                                ItemInfoRow["SPH"] = dr["SPH"];
                            if (dr["CYL"] != null)
                                ItemInfoRow["CYL"] = dr["CYL"];
                            if (dr["AXIS"] != null)
                                ItemInfoRow["AXIS"] = dr["AXIS"];
                            if (dr["ADDS"] != null)
                                ItemInfoRow["ADDS"] = dr["ADDS"];
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
                            if (dr["MATERIAL_COST"] != null)
                                ItemInfoRow["MATERIAL_COST"] = dr["MATERIAL_COST"];
                            if (dr["INGR_COST"] != null)
                                ItemInfoRow["INGR_COST"] = dr["INGR_COST"];
                            if (dr["PKG_COST"] != null)
                                ItemInfoRow["PKG_COST"] = dr["PKG_COST"];
                            if (dr["LAB_COST"] != null)
                                ItemInfoRow["LAB_COST"] = dr["LAB_COST"];
                            if (dr["OH_COST"] != null)
                                ItemInfoRow["OH_COST"] = dr["OH_COST"];
                            if (dr["OTH_COST"] != null)
                                ItemInfoRow["OTH_COST"] = dr["OTH_COST"];
                            if (dr["WASTAGE"] != null)
                                ItemInfoRow["WASTAGE"] = dr["WASTAGE"];
                            if (dr["COST_PRICE1"] != null)
                                ItemInfoRow["COST_PRICE1"] = dr["COST_PRICE1"];
                            if (dr["COST_PRICE2"] != null)
                                ItemInfoRow["COST_PRICE2"] = dr["COST_PRICE2"];
                            if (dr["COST_PRICE3"] != null)
                                ItemInfoRow["COST_PRICE3"] = dr["COST_PRICE3"];
                            if (dr["NET_COST"] != null)
                                ItemInfoRow["NET_COST"] = dr["NET_COST"];
                            if (dr["AVG_COST"] != null)
                                ItemInfoRow["AVG_COST"] = dr["AVG_COST"];
                            if (dr["AVG_COST1"] != null)
                                ItemInfoRow["AVG_COST1"] = dr["AVG_COST1"];
                            if (dr["AVG_COST2"] != null)
                                ItemInfoRow["AVG_COST2"] = dr["AVG_COST2"];
                            if (dr["AVG_COST3"] != null)
                                ItemInfoRow["AVG_COST3"] = dr["AVG_COST3"];
                            if (dr["F_PUR_PRICE"] != null)
                                ItemInfoRow["F_PUR_PRICE"] = dr["F_PUR_PRICE"];
                            if (dr["LAST_PUR_PRICE1"] != null)
                                ItemInfoRow["LAST_PUR_PRICE1"] = dr["LAST_PUR_PRICE1"];
                            if (dr["LAST_PUR_PRICE2"] != null)
                                ItemInfoRow["LAST_PUR_PRICE2"] = dr["LAST_PUR_PRICE2"];
                            if (dr["LAST_PUR_PRICE3"] != null)
                                ItemInfoRow["LAST_PUR_PRICE3"] = dr["LAST_PUR_PRICE3"];
                            if (dr["LAST_SUPP"] != null)
                                ItemInfoRow["LAST_SUPP"] = dr["LAST_SUPP"];
                            if (dr["LAST_SUPP1"] != null)
                                ItemInfoRow["LAST_SUPP1"] = dr["LAST_SUPP1"];
                            if (dr["LAST_SUPP2"] != null)
                                ItemInfoRow["LAST_SUPP2"] = dr["LAST_SUPP2"];
                            if (dr["FREE_QTY"] != null)
                                ItemInfoRow["FREE_QTY"] = dr["FREE_QTY"];
                            if (dr["FREE_QTY_AVG_COST"] != null)
                                ItemInfoRow["FREE_QTY_AVG_COST"] = dr["FREE_QTY_AVG_COST"];
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
                            if (dr["EMPTY"] != null)
                                ItemInfoRow["EMPTY"] = dr["EMPTY"];
                            if (dr["bNEW"] != null)
                                ItemInfoRow["bNEW"] = dr["bNEW"];
                            if (dr["NeedsReplication"] != null)
                                ItemInfoRow["NeedsReplication"] = dr["NeedsReplication"];

                            ItemInfoTable.Rows.Add(ItemInfoRow);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                try
                {

                    DeleteProduct();
                    //using (SqlConnection con1 = new SqlConnection(charsuDb))
                    //{
                    //    con1.Open();
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

                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
    }
}