using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace MunshiDAL
{
    public static class DL_ProcessMaster
    {
        #region Constants..
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion

        #region Insert_Upadte_Delete
        public static int ProcessInsert(string strConn, int  ?ProcessId, string ProcessName, bool QuantityFlag,
           bool PackingFlag,String ProcessDuration,float ProcessVolume,bool WastageFlag,int ProcessUnit, Guid ?CompanyId,bool byProduct)
        {
            const string ProcName = "ProcessMaster_InserUpdate()";
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

                    param = command.Parameters.Add("@ProcessId", SqlDbType.Int);
                    param.Value = ProcessId;


                    param = command.Parameters.Add("@ProcessName", SqlDbType.VarChar);
                    param.Value = ProcessName;


                    param = command.Parameters.Add("@QuantityFlag", SqlDbType.Bit);
                    param.Value = QuantityFlag;

                    param = command.Parameters.Add("@PackingFlag", SqlDbType.Bit);
                    param.Value = PackingFlag;
                    param = command.Parameters.Add("@ProcessDuration", SqlDbType.Time);
                    param.Value = ProcessDuration;
                    param = command.Parameters.Add("@WastageFlag", SqlDbType.Bit);
                    param.Value = WastageFlag;
                    param = command.Parameters.Add("@QuantityFlag", SqlDbType.Bit);
                    param.Value = QuantityFlag;

                    param = command.Parameters.Add("@ByProduct", SqlDbType.Bit);
                    param.Value = byProduct;
                    param = command.Parameters.Add("@ProcessVolume", SqlDbType.Float);
                    param.Value = ProcessVolume;

                    param = command.Parameters.Add("@CompanyId", SqlDbType.UniqueIdentifier);
                    param.Value = CompanyId;

                    param = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    param.Direction = ParameterDirection.ReturnValue;
                    param = command.Parameters.Add("@ProcessUnit", SqlDbType.Int);
                    param.Value = ProcessUnit;

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
        public static DataSet Process_List(string strConn, int SelectionType, string SearchBy, string SearchString, int ProcessId, Guid ?CompanyId,
               int ItemsPerPage, int RequestPageNo, int CurrentPageNo)
        {
            const string ProcName = "ProcessMaster_Select";
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
                    param = command.Parameters.Add("@ProcessId", SqlDbType.Int);
                    param.Value = ProcessId;
 

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

        #region for Delete Record
        public static int  Delete_Process(string strConn,int ProcessId)
        {
            const string ProcName = "PROCESS_Delete()";
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

                    param = command.Parameters.Add("@ProcessId", SqlDbType.Int);
                    param.Value = ProcessId;

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
    }
}
#endregion