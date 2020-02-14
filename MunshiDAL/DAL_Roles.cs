using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiDAL
{
    public static class DAL_Roles
    {
        #region Constants..
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion

        #region Roles Insert Update
        public static int Roles_InsertUpdate(string strConn, int? RoleId, string RoleName, string RoleCode,int UnderPrecedingRoleId,
           bool AuthorizationRequired, Guid? CompanyId)
        {
            const string ProcName = "Roles_InsertUpdate()";
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

                    param = command.Parameters.Add("@RoleId", SqlDbType.Int);
                    param.Value = RoleId;

                    param = command.Parameters.Add("@RoleName", SqlDbType.VarChar);
                    param.Value = RoleName;

                    param = command.Parameters.Add("@RoleCode", SqlDbType.VarChar);
                    param.Value = RoleCode;

                    param = command.Parameters.Add("@UnderPrecedingRoleId", SqlDbType.Int);
                    param.Value = UnderPrecedingRoleId;

                    param = command.Parameters.Add("@AuthorizationRequired", SqlDbType.Bit);
                    param.Value = AuthorizationRequired;

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

        #region Roles  List
        public static DataSet Roles_List(string strConn, int SelectionType, string SearchBy, string SearchString, int? IntID,int PrecedingRoleId, Guid? CompanyId,
            int ItemsPerPage, int RequestPageNo, int CurrentPageNo)
        {

            const string ProcName = "Roles_Select";
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

                    param = command.Parameters.Add("@RoleId", SqlDbType.Int);
                    param.Value = IntID;

                    param = command.Parameters.Add("@PrecedingRoleId", SqlDbType.Int);
                    param.Value = PrecedingRoleId;

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
