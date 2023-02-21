using GROCERY.DAL.Core;
using System;
using System.Data;
namespace GROCERY.DAL.Managers
{
    public class ProductManager : DABase
    {
        public DataSet getAllProducts(int scID)
        {
            string q = @"SELECT top 1000  P.PRODUCT_ID, P.NAME, P.DESCRIPTION PRODUCT_DESCRIPTION, ISNULL(P.PRICE,0) PRICE, P.OLD_PRODUCT_ID,
                        case 
                        when P.HAS_IMAGE = 1 then 'Yes' 
                        else 'No'
                        end
                        AS HAS_IMAGE
                        , P.VENDOR_ID, P.SUB_CATEGORY_ID, P.IS_ACTIVE
                        FROM      PRODUCTS P 
                        {0}
                        GROUP	 BY  P.PRODUCT_ID, P.NAME, P.DESCRIPTION,  P.VENDOR_ID,P.PRICE,P.OLD_PRODUCT_ID, P.SUB_CATEGORY_ID,P.IS_ACTIVE,P.HAS_IMAGE";




            string where = " WHERE 1=1 ";
            if (scID != -1)
            {

                where += string.Format(" and   P.SUB_CATEGORY_ID = {0}", scID);
            }
            return ExecuteDataSet(string.Format(q, where));
        }
        public DataSet getAllMiniProducts(int scID)
        {
            string q = @"SELECT top 1000 P.PRODUCT_ID, P.NAME, P.DESCRIPTION PRODUCT_DESCRIPTION, ISNULL(P.PRICE,0) PRICE, P.OLD_PRODUCT_ID,
                        case 
                        when P.HAS_IMAGE = 1 then 'Yes' 
                        else 'No'
                        end
                        AS HAS_IMAGE
                        , P.VENDOR_ID, P.SUB_CATEGORY_ID, P.IS_ACTIVE
                        FROM      PRODUCTS P 
                        {0}
                        GROUP	 BY  P.PRODUCT_ID, P.NAME, P.DESCRIPTION,  P.VENDOR_ID,P.PRICE,P.OLD_PRODUCT_ID, P.SUB_CATEGORY_ID,P.IS_ACTIVE,P.HAS_IMAGE";



            //FOR MINI
            string where = " WHERE 1=1 and P.OLD_PRODUCT_ID >=1000000";
            if (scID != -1)
            {

                where += string.Format(" and   P.SUB_CATEGORY_ID = {0}", scID);
            }
            return ExecuteDataSet(string.Format(q, where));
        }
        public DataSet getAllCategories()
        {
            return ExecuteDataSet("select * from CATEGORIES");
        }
        public DataSet getAllSubCategories()
        {
            return ExecuteDataSet("select * from SUB_CATEGORIES");
        }
        public DataSet GetProductsBySubCat(int scID)
        {
            string q = @"SELECT top 1000  P.PRODUCT_ID, P.NAME, P.DESCRIPTION PRODUCT_DESCRIPTION, ISNULL(P.PRICE,0) PRICE, P.OLD_PRODUCT_ID,
                        case 
                        when P.HAS_IMAGE = 1 then 'Yes' 
                        else 'No'
                        end
                        AS HAS_IMAGE
                        , P.VENDOR_ID, P.SUB_CATEGORY_ID, P.IS_ACTIVE
                        FROM      PRODUCTS P 
                        {0}
                        GROUP	 BY  P.PRODUCT_ID, P.NAME, P.DESCRIPTION,  P.VENDOR_ID,P.PRICE,P.OLD_PRODUCT_ID, P.SUB_CATEGORY_ID,P.IS_ACTIVE,P.HAS_IMAGE";




            string where = " WHERE 1=1 ";
            if (scID != -1)
            {

                where += string.Format(" and   P.SUB_CATEGORY_ID = {0} and P.IS_ACTIVE = 1", scID);
            }
            return ExecuteDataSet(string.Format(q, where));
        }
        public DataSet getAllVendors()
        {
            return ExecuteDataSet("select * from VENDORS");
        }
        
        public DataSet GetProducts()
        {
            return ExecuteDataSet("select P.PRODUCT_ID, P.NAME, P.DESCRIPTION from PRODUCTS P WHERE P.IS_ACTIVE = 1");
        }

        public int deleteProduct(int pID)
        {
            string queryString = "UPDATE PRODUCTS SET IS_ACTIVE = 0, MODIFIED_DATE = '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', MODIFIED_BY = " + 1 + "  where PRODUCT_ID = " + pID;
            return ExecuteNonQuery(queryString);
        }

        public DataSet getProductImgPath(int pID)
        {
            string queryString = "SELECT ADMIN_IMAGE_PATH FROM PRODUCT_IMAGES WHERE PRODUCT_ID = " + pID;
            return ExecuteDataSet(queryString);
        }
        /*
                public bool updateProduct(int productID)
                {
                    string query = "UPDATE PRODUCTS SET NAME = " + pName + ", DESCRIPTION = " + pDesc + ", PRICE = " + pPrice + ", VENDOR_ID = " + pVendorID + ", SUB_CATEGORY_ID =" +
                                    ", IS_ACTIVE = " + pFlagActive + ", UPDATED_ON = " + System.DateTime.Now + ", UPDATED_BY = " + userID;
                    return ExecuteNonQuery();
                }*/

        public DataSet getGroups()
        {
            return ExecuteDataSet("select * from Groups");
        }

        public DataSet getAllProductsForBranchBarCodes(int scID, int bID)
        {
            string q = @"select top 1000 P.OLD_PRODUCT_ID AS PRODUCT_ID, P.DESCRIPTION PRODUCT_DESCRIPTION,
                        b.UNIT_PRICE as PRICE,b.DISC as DISCOUNT,b.UNIT,b.BAR_CODE,ISNULL((select case when QTY > P.THRESHOLD THEN 'YES' ELSE 'NO' END
                        from STOCK where BRANCH_ID = " + bID + @" and PRODUCT_ID = P.PRODUCT_ID),'NO') AS AVAILABLE,P.IS_EXEMPTED
                        from BARCODES  b
                        inner join PRODUCTS p on b.ITEM_CODE =p.OLD_PRODUCT_ID
                        {0} 
                        GROUP	 BY  P.PRODUCT_ID,P.OLD_PRODUCT_ID, P.DESCRIPTION,b.UNIT_PRICE, b.DISC,b.UNIT,b.BAR_CODE,P.THRESHOLD,P.IS_EXEMPTED";
            if (scID == -1 && bID == -1)
                return ExecuteDataSet(string.Format(q, " where 1=1 and and b.bDEFAULT =1 and b.IsActive = 1 "));
            else
            {
                return ExecuteDataSet(string.Format(q, "WHERE b.IsActive = 1 and b.bDEFAULT =1 and b.LOCNO = " + bID + "  "));

            }
        }
        public DataSet getAllProductsForBranchBarCodesView(int scID, int bID)
        {
            string q = @"select  P.OLD_PRODUCT_ID AS PRODUCT_ID, P.DESCRIPTION PRODUCT_DESCRIPTION,
                        b.UNIT_PRICE as PRICE,b.DISC as DISCOUNT,b.UNIT,b.BAR_CODE
                        from BARCODES  b
                        inner join PRODUCTS p on b.ITEM_CODE =p.OLD_PRODUCT_ID
                        {0} 
                        GROUP	 BY  P.OLD_PRODUCT_ID, P.DESCRIPTION,b.UNIT_PRICE, b.DISC,b.UNIT,b.BAR_CODE";
            if (scID == -1 && bID == -1)
                return ExecuteDataSet(string.Format(q, " where 1=1 and and b.bDEFAULT =1 b.IsActive = 1 "));
            else
            {
                return ExecuteDataSet(string.Format(q, "WHERE b.IsActive = 1 and b.bDEFAULT =1 and b.LOCNO = " + bID + "  "));

            }
        }
        public DataSet getFilteredProductsForBranchBarCodes(int scID, int bID,string key)
        {
            string filter = "";
            string[] keywords = key.Split(' ');
            foreach(string data in keywords)
            {
                filter += "AND P.NAME LIKE '%" + data + "%' "; 
            }

            string q = @"select  P.OLD_PRODUCT_ID AS PRODUCT_ID, P.DESCRIPTION PRODUCT_DESCRIPTION,
                        b.UNIT_PRICE as PRICE,b.DISC as DISCOUNT,b.UNIT,b.BAR_CODE,ISNULL((select case when QTY > P.THRESHOLD THEN 'YES' ELSE 'NO' END
                        from STOCK where BRANCH_ID = " + bID + @" and PRODUCT_ID = P.PRODUCT_ID),'NO') AS AVAILABLE,P.IS_EXEMPTED
                        from BARCODES b
                        inner join PRODUCTS p on b.ITEM_CODE =p.OLD_PRODUCT_ID
                        {0} {1}
                        GROUP BY  P.OLD_PRODUCT_ID,P.PRODUCT_ID, P.DESCRIPTION,b.UNIT_PRICE, b.DISC,b.UNIT,b.BAR_CODE,P.THRESHOLD,P.IS_EXEMPTED";
            if (scID == -1 && bID == -1)
                return ExecuteDataSet(string.Format(q, " where 1=1 and b.bDEFAULT =1 and b.IsActive = 1 ", filter));
            else
            {
                return ExecuteDataSet(string.Format(q, "WHERE b.IsActive = 1 and b.bDEFAULT =1 and b.LOCNO = " + bID + "  ", filter));

            }
        }

        public DataSet searchFilteredItems(int pID, int status, string @key)
        {
            string filter = "";

            if(key !=null && key != string.Empty)
            {
                string[] keywords = key.Split(' ');
                foreach (string data in keywords)
                {
                    filter += "AND P.NAME LIKE '%" + data + "%' ";
                }
            }
            string where = " where 1=1 " +filter;

            string q = @"SELECT  P.PRODUCT_ID, P.NAME, P.DESCRIPTION PRODUCT_DESCRIPTION, ISNULL(P.PRICE,0) PRICE, P.OLD_PRODUCT_ID,
                        case 
                        when P.HAS_IMAGE = 1 then 'Yes' 
                        else 'No'
                        end
                        AS HAS_IMAGE
                        , P.VENDOR_ID, P.SUB_CATEGORY_ID, P.IS_ACTIVE
                        FROM      PRODUCTS P 
                        {0}
                        GROUP	 BY  P.PRODUCT_ID, P.NAME, P.DESCRIPTION,  P.VENDOR_ID,P.PRICE,P.OLD_PRODUCT_ID, P.SUB_CATEGORY_ID,P.IS_ACTIVE,P.HAS_IMAGE";
            if (pID != -1 )
            {

                where += string.Format(" and   P.OLD_PRODUCT_ID = {0}", pID);
            }
            if(status != -1)
            {
                where += string.Format(" and   P.IS_ACTIVE = {0}",status);
                

            }
            return ExecuteDataSet(string.Format(q, where));
        }

        public DataSet searchFilteredMiniItems(int pID, int status, string @key)
        {
            string filter = "";

            if (key !=null && key != string.Empty)
            {
                string[] keywords = key.Split(' ');
                foreach (string data in keywords)
                {
                    filter += "AND P.NAME LIKE '%" + data + "%' ";
                }
            }
            //FOR MINI ITEMS
            string where = " where 1=1 AND P.OLD_PRODUCT_ID >=1000000 " + filter;

            string q = @"SELECT  P.PRODUCT_ID, P.NAME, P.DESCRIPTION PRODUCT_DESCRIPTION, ISNULL(P.PRICE,0) PRICE, P.OLD_PRODUCT_ID,
                        case 
                        when P.HAS_IMAGE = 1 then 'Yes' 
                        else 'No'
                        end
                        AS HAS_IMAGE
                        , P.VENDOR_ID, P.SUB_CATEGORY_ID, P.IS_ACTIVE
                        FROM      PRODUCTS P 
                        {0}
                        GROUP	 BY  P.PRODUCT_ID, P.NAME, P.DESCRIPTION,  P.VENDOR_ID,P.PRICE,P.OLD_PRODUCT_ID, P.SUB_CATEGORY_ID,P.IS_ACTIVE,P.HAS_IMAGE";
            if (pID != -1)
            {

                where += string.Format(" and   P.OLD_PRODUCT_ID = {0}", pID);
            }
            if (status != -1)
            {
                where += string.Format(" and   P.IS_ACTIVE = {0}", status);


            }
            return ExecuteDataSet(string.Format(q, where));
        }
    

        public DataSet getAllProductsForOrders(int scID, int bID)
        {

            string q = @"SELECT     P.OLD_PRODUCT_ID as PRODUCT_ID, P.NAME, P.DESCRIPTION PRODUCT_DESCRIPTION, ISNULL(P.PRICE,0) PRICE, ISNULL(P.DISCOUNT,0) DISCOUNT, 
                                    FLOOR(SUM(Stk.QTY)) QUANTITY
                                    FROM      PRODUCTS P 
                                    INNER JOIN STOCK Stk on P.OLD_PRODUCT_ID = Stk.PRODUCT_ID
                                    {0}
                                    GROUP	 BY  P.PRODUCT_ID, P.NAME, P.DESCRIPTION,  P.PRICE, P.DISCOUNT";
            if(scID == -1 && bID == -1)
                return ExecuteDataSet(string.Format(q, " where 1=1 "));
            else
            {
                return ExecuteDataSet(string.Format(q, "WHERE stk.Branch_ID = " + bID + "  "));

            }
            
        }
        public DataSet GetProductDetails(int productId,int branchId)
        {
            return ExecuteDataSet(string.Format(@"select PIMG.ADMIN_IMAGE_PATH, SC.DESCRIPTION AS SUBCATEGORY, C.DESCRIPTION AS CATEGORY, P.* 
                                                from PRODUCTS P
                                                INNER JOIN BARCODES B ON P.OLD_PRODUCT_ID = B.ITEM_CODE
                                                INNER JOIN PRODUCT_IMAGES PIMG ON P.PRODUCT_ID = PIMG.PRODUCT_ID
                                                INNER JOIN SUB_CATEGORIES SC ON P.SUB_CATEGORY_ID = SC.SUB_CATEGORY_ID
                                                INNER JOIN CATEGORIES C ON SC.CATEGORY_ID = C.CATEGORY_ID
                                                where P.PRODUCT_ID = {0} and B.LOCNO = {1}", productId,branchId));
        }

        public string GetProductName(int productId)
        {
            return ExecuteDataSet(string.Format("select * from products where product_id = {0}", productId)).Tables[0].Rows[0][2].ToString();
        }
        public int updateMiniProductBarcode(decimal price, double disc ,int user,int itemcode)
        {
            
            return ExecuteNonQuery(string.Format("UPDATE BARCODES SET UNIT_PRICE = {0}, DISC = {1}, MUSER = {2},MDATE = '{3}' WHERE ITEM_CODE = {4}"
                                    , price, disc, user, DateTime.Now,itemcode));
        }
        public bool hasImage(int productID)
        {
            object o = ExecuteScalar(string.Format("select ISNULL( COUNT(*),0) as IMAGE_COUNT from PRODUCT_IMAGES where PRODUCT_ID = {0}", productID));
            
            int has_image = o == null ? 0 : (int)o;
            if (has_image > 0)
                return true;
            else
                return false;
        }
    }
}