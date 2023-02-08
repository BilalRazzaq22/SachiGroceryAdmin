using GROCERY.DAL.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GROCERY.Controllers
{
    public class OrderApiController : ApiController
    {
        BusinessController controller = new BusinessController();

        [Route("api/Order/Statuses")]
        [HttpGet]
        public HttpResponseMessage GetAllOrderStatuses()
        {
            DataSet obj = controller.getAllOrderStatuses();
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/Order/Orders")]
        [HttpGet]
        public HttpResponseMessage GetAllOrders(int oStID, string oDateFrom, string oDateTo)
        {
            DataSet obj = controller.getAllOrders(oStID, oDateFrom, oDateTo);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/Order/customerOrders")]
        [HttpGet]
        public HttpResponseMessage getCustomerOrders(int uId)
        {
            DataSet obj = controller.getCustomerOrders(uId);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }


        [Route("api/Order/Allocate")]
        [HttpGet]
        public HttpResponseMessage AllocateOrders(int oID)
        {
            DataSet obj = controller.allocateOrder(oID);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/Order/Dispatch")]
        [HttpGet]
        public HttpResponseMessage DispatchOrder(int orderID, int riderID)
        {
            DataSet obj = controller.dispatchOrder(orderID, riderID);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/Order/UpdateStock")]
        [HttpGet]
        public HttpResponseMessage DispatchOrder(int pID, int amountOrdered, int bID)
        {
            DataSet obj = controller.updateStock(pID, amountOrdered, bID);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/Order/Riders")]
        [HttpGet]
        public HttpResponseMessage GetRiders()
        {
            DataSet obj = controller.getAvailableRiders();
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        
    }

    

   
}
