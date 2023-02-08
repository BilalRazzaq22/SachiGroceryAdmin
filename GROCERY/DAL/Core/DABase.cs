using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Configuration;
namespace GROCERY.DAL.Core
{
    public class DABase
    {
        static Database db = DatabaseFactory.CreateDatabase();
        public static string connectionString;
        public static SqlConnection connection;
        public DABase()
        {
            connectionString = ConfigurationManager.ConnectionStrings["GROCERYDB"].ConnectionString;
            connection = new SqlConnection(connectionString);
        }
        public void OpenDBConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        public void CloseDBConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        protected int ExecuteNonQuery(string queryString)
        {
            try
            {
                return db.ExecuteNonQuery(db.GetSqlStringCommand(queryString));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        protected object ExecuteScalar(string queryString)
        {
            try
            {
                return db.ExecuteScalar(db.GetSqlStringCommand(queryString));
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                connection.Close();
            }
        }
        protected DataSet ExecuteDataSet(string queryString)
        {
            try
            {
                var cmd = db.GetSqlStringCommand(queryString);
                cmd.CommandTimeout = 180; // 0 = give it as much time as it needs to complete
                return db.ExecuteDataSet(cmd);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                connection.Close();
            }
        }
        public DataSet ExecuteStoredProcedure(string procedureName, List<StoredProcedureParams> parametersList)
        {
            DbCommand cmd = db.GetStoredProcCommand(procedureName);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var item in parametersList)
            {

                if (item.ParamType.Equals(DbType.DateTime) && (item.ParamValue == null || item.ParamValue.Length <= 0))
                {
                    item.ParamValue = DBNull.Value.ToString();
                }

                db.AddInParameter(cmd, item.ParamName, (DbType)item.ParamType, item.ParamValue);
            }

            DataSet ds = db.ExecuteDataSet(cmd);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Columns.Contains("ERROR_MESSAGE"))
                {
                    throw new Exception("Error Occured While executing the stored procedure at break point:" + ds.Tables[0].Rows[0]["ERROR_MESSAGE"].ToString());
                }
            }
            return ds;
        }

    }
}
