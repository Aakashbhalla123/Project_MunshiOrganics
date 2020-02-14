using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiDAL
{
    public static class DAL_RoleRightsBasedMenu
    {

        #region Constants..
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion

        #region Role Rights Based Module List
        public static DataSet ShowModules_RightsBased(string strConn, int SelectionType, int RoleId, Guid? CompanyId)
        {

            const string ProcName = "ShowModules_RightsBased";
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

                    param = command.Parameters.Add("@RoleId", SqlDbType.Int);
                    param.Value = RoleId;

                    param = command.Parameters.Add("@CompanyId", SqlDbType.UniqueIdentifier);
                    param.Value = CompanyId;

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
