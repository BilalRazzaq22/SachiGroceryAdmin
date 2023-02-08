using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;

namespace GROCERY.Controllers
{
    public class CommonController : ApiController
    {
        BusinessController controller = new BusinessController();
        [Route("api/common/packages")]
        public HttpResponseMessage getPackages()
        {
            DataSet packages = controller.getPackages();
            string json = JsonConvert.SerializeObject(packages, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/common/customers")]
        public HttpResponseMessage getCustomers()
        {
            DataSet customers = controller.getCustomers();
            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
    }
}
