using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace MunshiDAL
{
    public static class DL_ProductMaster
    {
        #region Constants..
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion

        #region Insert_Upadte_Delete
     
        public static int ProductInsert(string strConn, int? Productid, string ProductName,int UOM,
            int Colour,int Texture,Guid CreatedBy,string  Processid,int BuyProductId,int BuyProductPacking,int Catagory,
            Guid CompanyId)
        {
            const string ProcName = "ProductMaster_InsertUpdate()";
            SqlConnection connection = null;
            SqlParameter param;
            DateTime startDate = DateTime.Now;
            Guid LoginID = Guid.Empty;
            string strMessage = string.Empty;
            DataTable dt = new DataTable();
            int rowsEffected;
            int returnValue;
            try
            {
              
                 connection = DataSource.GetConnection(strConn);
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = ProcName.Substring(0, ProcName.Length - 2);
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    param = command.Parameters.Add("@Productid", SqlDbType.Int);
                    param.Value = Productid;

                    param = command.Parameters.Add("@ProductName", SqlDbType.VarChar);
                    param.Value = ProductName;

                    param = command.Parameters.Add("@UOM", SqlDbType.Int);
                    param.Value = UOM;

                    

                    param = command.Parameters.Add("@Colour", SqlDbType.Int);
                    param.Value = Colour;

                    param = command.Parameters.Add("@Texture", SqlDbType.Int);
                    param.Value = Texture;

                    param = command.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier);
                    param.Value = CreatedBy;
                    
                    param = command.Parameters.Add("@ProcessId", SqlDbType.VarChar);
                    param.Value = Processid;

                    
                    
                    param = command.Parameters.Add("@BuyProductId", SqlDbType.Int);
                    param.Value = BuyProductId;
                    param = command.Parameters.Add("@Catagory", SqlDbType.Int);
                    param.Value = Catagory;

                    param = command.Parameters.Add("@BuyProductPacking", SqlDbType.Int);
                    param.Value = BuyProductPacking;

                    param = command.Parameters.Add("@CompanyId", SqlDbType.UniqueIdentifier);
                    param.Value = CompanyId;
                    param = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    param.Direction = ParameterDirection.ReturnValue;


                    rowsEffected = command.ExecuteNonQuery();
                    returnValue = (int)command.Parameters["@ReturnValue"].Value;



                }
            }
            finally
            {
                DataSource.CloseConnection(connection);

                TimeSpan timeSpan = DateTime.Now.Subtract(startDate);
            }
            return returnValue;

        }

        #endregion
        #region Process_List
        public static DataSet Product_List(string strConn, int SelectionType, string SearchBy, string SearchString, int? Productid, Guid CompanyId,
               int ItemsPerPage, int RequestPageNo, int CurrentPageNo)
        {
            const string ProcName = "ProductMaster_Select";
            SqlConnection connection = new SqlConnection();
            SqlParameter param;
            DataSet outDS = null;
            try
            {
                connection = DataSource.GetConnection(strConn);
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = ProcName;
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;


                    param = command.Parameters.Add("@SelectionType", SqlDbType.Int);
                    param.Value = SelectionType;
                    param = command.Parameters.Add("@SearchBy", SqlDbType.VarChar);
                    if (SearchBy != null)
                        param.Value = SearchBy;
                    else
                        param.Value = DBNull.Value;
                    param = command.Parameters.Add("@SearchString", SqlDbType.VarChar);
                    if (SearchBy != null)
                        param.Value = SearchString;
                    else
                        param.Value = "";
                    param = command.Parameters.Add("@Productid", SqlDbType.Int);
                    param.Value = Productid;


                    param = command.Parameters.Add("@CompanyId", SqlDbType.UniqueIdentifier);
                    param.Value = CompanyId;

                    param = command.Parameters.Add("@ItemsPerPage", SqlDbType.Int);
                    param.Value = ItemsPerPage;

                    param = command.Parameters.Add("@RequestPageNo", SqlDbType.Int);
                    param.Value = RequestPageNo;

                    param = command.Parameters.Add("@CurrentPageNo", SqlDbType.Int);
                    param.Value = CurrentPageNo;

                    param = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    param.Direction = ParameterDirection.ReturnValue;

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        outDS = new DataSet();
                        da.Fill(outDS);
                    }
                }
            }
            finally
            {
                DataSource.CloseConnection(connection);


            }
            return outDS;
        }
    }
}
#endregion