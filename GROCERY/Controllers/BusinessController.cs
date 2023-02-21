using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using GROCERY.DAL.Managers;
namespace GROCERY.Controllers
{
        
    public class BusinessController 
    {
        ProductManager daProduct = new ProductManager();
        UserManager daUser = new UserManager();
        OrderManger daOrder = new OrderManger();
        StoreManager daStore = new StoreManager();
        CommonManager daCommon = new CommonManager();
        RiderManager riderManager = new RiderManager();

        public DataSet getPackages()
        {
            return daCommon.getPackages();
        }
        public DataSet getCustomers()
        {
            return daCommon.getCustomers();
        }
        
        public DataSet getAllProducts(int scID)
        {
            return daProduct.getAllProducts(scID);
        }
        public DataSet getAllMiniProducts(int scID)
        {
            return daProduct.getAllMiniProducts(scID);
        }

        public DataSet getAllProductsForCallOrders(int scID, int bID)
        {
            return daProduct.getAllProductsForOrders(scID, bID);
        }
        public DataSet getAllProductsForBranchBarCodes(int scID, int bID)
        {
            return daProduct.getAllProductsForBranchBarCodes(scID, bID);
        }
        public DataSet getAllProductsForBranchBarCodesView(int scID, int bID)
        {
            return daProduct.getAllProductsForBranchBarCodesView(scID, bID);
        }
        public DataSet getFilteredProductsForBranchBarCodes(int scID, int bID,string key)
        {
            return daProduct.getFilteredProductsForBranchBarCodes(scID, bID,key);
        }
        public DataSet searchFilteredItems(int pID, int status, string @key)
        {
            return daProduct.searchFilteredItems(pID,status, key);
        }
        public DataSet searchFilteredMiniItems(int pID, int status, string @key)
        {
            return daProduct.searchFilteredMiniItems(pID, status, key);
        }
        public DataSet getAllCategories()
        {
            return daProduct.getAllCategories();
        }

        public DataSet getAllSubCategories()
        {
            return daProduct.getAllSubCategories();
        }

        public DataSet GetProductsBySubCat(int scID)
        {
            return daProduct.GetProductsBySubCat(scID);
        }
        
        public DataSet GetProducts()
        {
            return daProduct.GetProducts();
        }

        public DataSet getAllVendors()
        {
            return daProduct.getAllVendors();
        }

        public int deleteProduct(int pID)
        {
            return daProduct.deleteProduct(pID);
        }

        public DataSet GetProductDetails(int productId, int branchId)
        {
            return daProduct.GetProductDetails(productId, branchId);
        }
        public int updateMiniProductBarcode(decimal price, double disc, int user, int itemcode)
        {
            return daProduct.updateMiniProductBarcode(price, disc, user, itemcode);
        }
        public bool hasImage(int productID)
        {
            return daProduct.hasImage(productID);
        }
        public DataSet getAllUserTypes()
        {
            return daUser.getUserTypes();
        }

        public DataSet getAllUsers()
        {
            return daUser.getUsers();
        }

        public DataSet getAllRiders()
        {
            return riderManager.getRiders();
        }
        
        public DataSet getRiderOrderJobs(int riderId)
        {
            return riderManager.getRiderOrderJobs(riderId);
        }

        public DataSet getUserscContacts()
        {
            return daUser.getUserscContacts();
        }
        public int deleteUser(int uID)
        {
            return daUser.deleteUser(uID);
        }

        public DataSet productImgPath(int pID)
        {
            return daProduct.getProductImgPath(pID);
        }

        public DataSet getAllOrderStatuses()
        {
            return daOrder.getAllOrderStatuses();
        }

        public DataSet getAllOrders(int oStID, string oDateFrom, string oDateTo)
        {
            return daOrder.getOrders(oStID, oDateFrom, oDateTo);
        }

        public DataSet getOrdersForExcel(int oStID, string oDateFrom, string oDateTo)
        {
            return daOrder.getOrdersForExcel(oStID, oDateFrom, oDateTo);
        }

        public DataSet getCustomerOrders(int uId)
        {
            return daOrder.getCustomerOrders(uId);
        }
        public DataSet allocateOrder(int oID)
        {
            return daOrder.allocateOrder(oID);
        }

        public DataSet dispatchOrder(int oID, int rID)
        {
            return daOrder.dispatchOrder(oID, rID);
        }
        
        public DataSet updateStock(int pID, int amountOrdered, int bID)
        {
            return daOrder.updateStock(pID, amountOrdered, bID);
        }

        public DataSet getBranches()
        {
            return daStore.getBranches();
        }
        
        public DataSet checkCustomerExistence(string mobNum)
        {
            return daUser.checkExistence(mobNum);
        }

        public DataSet getAllGroups()
        {
            return daProduct.getGroups();
        }

        public DataSet getAvailableRiders()
        {
            return daOrder.getAvailableRiders();
        }
        public DataSet getPaymentModes()
        {
            return daStore.getPaymentModes();
        }
        public DataSet getPrevAddress(int userId)
        {
            return daStore.getPrevAddress(userId);
        }
        public string GetProductName(int productId)
        {
            return daProduct.GetProductName(productId);
        }
        public DataSet getCouponInfo(string code)
        {
            return daStore.getCouponInfo(code);
        }

        public DataSet managerFCMToken(string branchId)
        {
            return daOrder.managerFCMToken(branchId);

        }
    }
}