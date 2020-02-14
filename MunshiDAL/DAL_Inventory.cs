using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiDAL
{
    public class DAL_Inventory
    {
        #region Constants..
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion
        #region Insert_Upadte_Delete
        public static int InventoryInsert(string strConn,int FarmerId, int ReciptNo, int LoginId, String PersonName, int Quantity, String Unit, Guid? CompanyId, string RawMaterial, int storage,DateTime CreatedDate)
        {
            const string ProcName = "Inventory_InsertUpdate";
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
                    command.CommandText = ProcName;
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    param = command.Parameters.Add("@ReciptNo", SqlDbType.Int);
                    param.Value = ReciptNo;
                    //param = command.Parameters.Add("@LoginId", SqlDbType.Int);
                    // param.Value = LoginId;
                    param = command.Parameters.Add("@FarmerId", SqlDbType.Int);
                    param.Value = FarmerId;
                    param = command.Parameters.Add("@PersonName", SqlDbType.NVarChar);
                    param.Value = PersonName;
                    //param = command.Parameters.Add("@Place", SqlDbType.NVarChar);
                    //param.Value = Place;
                    param = command.Parameters.Add("@Qunatity", SqlDbType.Int);
                    param.Value = Quantity;
                    param = command.Parameters.Add("@Unit", SqlDbType.NVarChar);
                    param.Value = Unit;
                    param = command.Parameters.Add("@CompanyId", SqlDbType.UniqueIdentifier);
                    param.Value = CompanyId;
                    
                    param = command.Parameters.Add("@RawMaterial", SqlDbType.NVarChar);
                    param.Value = RawMaterial;
                    param = command.Parameters.Add("@Storage", SqlDbType.Int);
                    param.Value = storage;
                    param = command.Parameters.Add("@CreatedDate", SqlDbType.DateTime);
                    param.Value = CreatedDate;
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
        #region Inventory_List
        public static DataSet Inventory_List(string strConn, int SelectionType, string SearchBy, string SearchString, int ReciptNo, Guid? CompanyId,
               int ItemsPerPage, int RequestPageNo, int CurrentPageNo)
        {
            const string ProcName = "Inventory_Select";
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
                    param = command.Parameters.Add("@ReciptNo", SqlDbType.Int);
                    param.Value = ReciptNo;


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

        public static DataSet GetRecipt(string strCon)
        {
            const string ProcName = "GenrateRecipt";
            SqlConnection connection = new SqlConnection();
            
            SqlParameter param;
            
            DataSet outDS = null;
            try
            {
               
                connection = DataSource.GetConnection(strCon);
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = ProcName;
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                   
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
    

