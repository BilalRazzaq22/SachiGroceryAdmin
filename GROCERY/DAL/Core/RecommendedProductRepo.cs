using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class RecommendedProductRepo : DABase
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        public List<RECOMMENDED_PRODUCTS> GetRecommendedProducts()
        {
            DataSet ds = ExecuteStoredProcedure("SP_GETRECOMMENDEDPRODUCTS", new List<StoredProcedureParams>());
            List<RECOMMENDED_PRODUCTS> recommendedProductList = new List<RECOMMENDED_PRODUCTS>();

            foreach (DataRow dataRow in ds.Tables[0].AsEnumerable())
            {
                RECOMMENDED_PRODUCTS recommenedProduct = new RECOMMENDED_PRODUCTS();
                recommenedProduct.RECOMMENDED_PRODUCT_ID = dataRow.Field<int>("RECOMMENDED_PRODUCT_ID");
                recommenedProduct.PRODUCT_ID = dataRow.Field<int>("PRODUCT_ID");
                recommenedProduct.PRODUCT_NAME = dataRow.Field<string>("NAME");
                recommenedProduct.PACKING = dataRow.Field<string>("PACKING");
                if (dataRow.Field<decimal?>("PRICE") != null)
                    recommenedProduct.PRICE = dataRow.Field<decimal>("PRICE");
                recommenedProduct.TYPE = dataRow.Field<string>("TYPE");
                recommenedProduct.CREATED_ON = dataRow.Field<DateTime>("CREATED_ON");
                recommendedProductList.Add(recommenedProduct);
            }

            return recommendedProductList;
        }

        public void AddRecommendedProduct(RECOMMENDED_PRODUCTS rp)
        {
            try
            {
                PRODUCT p = gEnt.PRODUCTS.FirstOrDefault(x => x.PRODUCT_ID == rp.PRODUCT_ID);
                if (p != null)
                {
                    rp.PRODUCT_NAME = p.NAME;
                    rp.PRICE = p.PRICE;
                    rp.PACKING = p.PACKING;
                }
                gEnt.RECOMMENDED_PRODUCTS.Add(rp);
                gEnt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RECOMMENDED_PRODUCTS getRecommendedProductById(int recommendedProductId)
        {
            try
            {
                var q = from c in gEnt.RECOMMENDED_PRODUCTS
                        where c.RECOMMENDED_PRODUCT_ID == recommendedProductId
                        select c;
                if (q.Any())
                {
                    return q.FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteRecommendedProduct(int id)
        {
            RECOMMENDED_PRODUCTS rp = getRecommendedProductById(id);
            if (rp != null)
            {
                gEnt.RECOMMENDED_PRODUCTS.Remove(rp);
                gEnt.SaveChanges();
            }
        }
    }
}