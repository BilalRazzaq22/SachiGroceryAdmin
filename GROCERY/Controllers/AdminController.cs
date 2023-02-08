using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GROCERY.Controllers
{
    public class AdminController : ApiController
    {
        BusinessController controller = new BusinessController();

        [Route("api/admin/usertypes")]
        [HttpGet]
        public HttpResponseMessage GetAllUserTypes()
        {

            DataSet obj = controller.getAllUserTypes();
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);

        }

        [Route("api/admin/users")]
        [HttpGet]
        public HttpResponseMessage GetAllUsers()
        {

            DataSet obj = controller.getAllUsers();
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/admin/deleteuser")]
        [HttpGet]
        public HttpResponseMessage deleteProduct(int uID)
        {
            int flag = controller.deleteUser(uID);
            string json = JsonConvert.SerializeObject(flag, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/admin/branches")]
        [HttpGet]
        public HttpResponseMessage getBranchesList()
        {
            string json = JsonConvert.SerializeObject(controller.getBranches() , Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/admin/customercheck")]
        [HttpGet]
        public HttpResponseMessage checkCustomerExistence(string mobNo)
        {
            string json = JsonConvert.SerializeObject(controller.checkCustomerExistence(mobNo), Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/admin/modes")]
        [HttpGet]
        public HttpResponseMessage getPaymentModes()
        {
            string json = JsonConvert.SerializeObject(controller.getPaymentModes(), Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/admin/PrevAddress")]
        [HttpGet]
        public HttpResponseMessage getPrevAddress(int userId)
        {
            string json = JsonConvert.SerializeObject(controller.getPrevAddress(userId), Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/admin/couponInfo")]
        [HttpGet]
        public HttpResponseMessage getCouponInfo(string code)
        {

            string json = JsonConvert.SerializeObject(controller.getCouponInfo(code), Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
    }
}