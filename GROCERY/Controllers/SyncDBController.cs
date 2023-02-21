using GROCERY.SyncService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GROCERY.Controllers
{
    public class SyncDBController : ApiController
    {
        [Route("api/SyncDB/FullSyncData")]
        [HttpGet]
        public string FullSyncData()
        {
            try
            {
                return DataSyncService.Instance.StartFullSyncing();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/SyncDB/StockSyncData")]
        [HttpGet]
        public string StockSyncData()
        {
            try
            {
                return DataSyncService.Instance.StockSync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/SyncDB/ProductSyncData")]
        [HttpGet]
        public string ProductSyncData()
        {
            try
            {
                return DataSyncService.Instance.ProductSync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
