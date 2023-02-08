using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using System.Text;
namespace GROCERY.Controllers
{
    public class ProductController : ApiController
    {
        BusinessController controller = new BusinessController();
        
        [Route("api/Product/Products")]
        [HttpGet]
        public HttpResponseMessage GetAllProducts(int scID)
        {

            DataSet obj = controller.getAllProducts(scID);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);

        }
        [Route("api/Product/miniProducts")]
        [HttpGet]
        public HttpResponseMessage GetAllMiniProducts(int scID)
        {

            DataSet obj = controller.getAllMiniProducts(scID);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);

        }
        //[Route("api/Product/Items")]
        //[HttpGet]
        //public HttpResponseMessage GetAllProducts(int scID)
        //{

        //    DataSet obj = controller.getAllProducts(scID);
        //    string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        //    return Request.CreateResponse(HttpStatusCode.OK, json);

        //}
        [Route("api/Product/ProductsQ")]
        [HttpGet]
        public HttpResponseMessage GetAllProductsForCallOrders(int scID, int bID)
        {
            DataSet obj = controller.getAllProductsForCallOrders(scID, bID);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);

        }
        [Route("api/Product/ProductsBarCodes")]
        [HttpGet]
        public HttpResponseMessage getAllProductsForBranchBarCodes(int scID, int bID)
        {
            DataSet obj = controller.getAllProductsForBranchBarCodes(scID, bID);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/Product/ProductsBarCodesView")]
        [HttpGet]
        public HttpResponseMessage getAllProductsForBranchBarCodesView(int scID, int bID)
        {
            DataSet obj = controller.getAllProductsForBranchBarCodesView(scID, bID);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/Product/searchit")]
        [HttpGet]
        public HttpResponseMessage searchFilteredProducts(int scID,int bID,string @key)
        {
            DataSet obj = controller.getFilteredProductsForBranchBarCodes(scID, bID, key);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/Product/searchitems")]
        [HttpGet]
        public HttpResponseMessage searchFilteredItems(int pID,int status, string @key)
        {
            DataSet obj = controller.searchFilteredItems(pID,status, key);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/Product/searchminiitems")]
        [HttpGet]
        public HttpResponseMessage searchFilteredMiniItems(int pID, int status, string @key)
        {
            DataSet obj = controller.searchFilteredMiniItems(pID, status, key);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }




        [Route("api/Product/Categories")]
        [HttpGet]
        public HttpResponseMessage GetCategories()
        {
            DataSet obj = controller.getAllCategories();
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/Product/SubCategories")]
        [HttpGet]
        public HttpResponseMessage GetSubCategories()
        {
            DataSet subCat = controller.getAllSubCategories();
            string json = JsonConvert.SerializeObject(subCat, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }


        [Route("api/Product/Vendors")]
        [HttpGet]
        public HttpResponseMessage GetVendors()
        {
            DataSet vendors = controller.getAllVendors();
            string json = JsonConvert.SerializeObject(vendors, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/Product/deleteproduct")]
        [HttpGet]
        public HttpResponseMessage deleteProduct(int pID)
        {
            int flag = controller.deleteProduct(pID);
            string json = JsonConvert.SerializeObject(flag, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/Product/imgPath")]
        [HttpGet]
        public HttpResponseMessage getProductImagePath(int pID)
        {
            DataSet flag = controller.productImgPath(pID);
            string json = JsonConvert.SerializeObject(flag, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [Route("api/Product/Groups")]
        [HttpGet]
        public HttpResponseMessage GetGroups()
        {
            DataSet vendors = controller.getAllGroups();
            string json = JsonConvert.SerializeObject(vendors, Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [Route("api/Product/Details")]
        [HttpGet]
        public HttpResponseMessage GetProductDetails(int pID,int BID)
        {
            DataSet products = controller.GetProductDetails(pID,BID);
            products.Tables[0].TableName = "PRODUCTS";
            return Request.CreateResponse(HttpStatusCode.OK, products);
        }
        [Route("api/Product/Name")]
        [HttpGet]
        public HttpResponseMessage GetProductName(int pID)
        {
            string products = controller.GetProductName(pID);
            return Request.CreateResponse(HttpStatusCode.OK, products);
        }
    }
}