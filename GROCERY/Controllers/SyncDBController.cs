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
        public void FullSyncData()
        {
            DataSyncService.Instance.StartFullSyncing();
        }

        [Route("api/SyncDB/StockSyncData")]
        [HttpGet]
        public void StockSyncData()
        {
            DataSyncService.Instance.StockSync();
        }

        [Route("api/SyncDB/ProductSyncData")]
        [HttpGet]
        public void ProductSyncData()
        {
            DataSyncService.Instance.ProductSync();
        }
    }
}
