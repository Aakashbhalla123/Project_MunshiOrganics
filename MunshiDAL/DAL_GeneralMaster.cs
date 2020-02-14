using System;
using System.Data;
using System.Data.SqlClient;


namespace MunshiDAL
{
  public static class DAL_GeneralMaster
    {
        #region Constants..
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion
        #region General Master Insert Update
        public static int GeneralMaster_InsertUpdate(string strConn, int GeneralMasterId, int GeneralMasterClassId, string GeneralMasterName, string GeneralMasterCode,
        string GeneralMasterSquence,Guid? CreatedBy,Guid? ModifiedBy, Guid? CompanyId)
        {
            const string ProcName = "GeneralMaster_InsertUpdate()";
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

                    param = command.Parameters.Add("@GeneralMasterId", SqlDbType.Int);
                    param.Value = GeneralMasterId;

                    param = command.Parameters.Add("@GeneralMasterClassId", SqlDbType.Int);
                    param.Value = GeneralMasterClassId;

                    param = command.Parameters.Add("@GeneralMasterName", SqlDbType.VarChar);
                    param.Value = GeneralMasterName;

                    param = command.Parameters.Add("@GeneralMasterCode", SqlDbType.VarChar);
                    param.Value = GeneralMasterCode;

                    param = command.Parameters.Add("@GeneralMasterSquence", SqlDbType.VarChar);
                    param.Value = GeneralMasterSquence;

                    param = command.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier);
                    param.Value = CreatedBy;

                    param = command.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier);
                    param.Value = ModifiedBy;

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

        #region GeneralMaster  List
        public static DataSet GeneralMaster_List(string strConn, int SelectionType, string SearchBy, string SearchString, 
          int? IntID, int GeneralMasterClassId,Guid? CompanyId, int ItemsPerPage, int RequestPageNo, int CurrentPageNo)
        {

            const string ProcName = "GeneralMaster_Select";
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

                    param = command.Parameters.Add("@GeneralMasterId", SqlDbType.Int);
                    param.Value = IntID;

                    param = command.Parameters.Add("@GeneralMasterClassId", SqlDbType.Int);
                    param.Value = GeneralMasterClassId;

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
        #endregion
    }
}
