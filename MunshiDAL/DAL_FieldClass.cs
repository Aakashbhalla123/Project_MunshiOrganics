using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiDAL
{
    public static class DAL_FieldClass
    {
        #region Constants..
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion

        #region FieldClass Insert Update
        public static int FieldClass_InsertUpdate(string strConn, int? FieldClassId, string FieldClassName, string FieldClassCode, Guid? CompanyId)
        {
            const string ProcName = "FieldClass_InsertUpdate()";
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

                    param = command.Parameters.Add("@FieldClassId", SqlDbType.Int);
                    param.Value = FieldClassId;

                    param = command.Parameters.Add("@FieldClassName", SqlDbType.VarChar);
                    param.Value = FieldClassName;

                    param = command.Parameters.Add("@FieldClassCode", SqlDbType.VarChar);
                    param.Value = FieldClassCode;
                   
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

        #region FieldClass  List
        public static DataSet FieldClass_List(string strConn, int SelectionType, string SearchBy, string SearchString, int? IntID,Guid? CompanyId,
            int ItemsPerPage, int RequestPageNo, int CurrentPageNo)
        {

            const string ProcName = "FieldClass_Select";
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

                    param = command.Parameters.Add("@FieldClassId", SqlDbType.Int);
                    param.Value = IntID;

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
