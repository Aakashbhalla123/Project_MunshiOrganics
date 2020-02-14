using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiDAL
{
    public static class DAL_UOMMaster
    {

        #region Constants..
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion
        #region UOM Master Insert Update
        public static int UOMMaster_InsertUpdate(string strConn, int UOMId, int UOMTypeId, string UOMName, string UOMCode,
        int DecimalPoints, bool BaseUnit, decimal ConversionRation, Guid? CreatedBy, Guid? ModifiedBy, Guid? CompanyId)
        {
            const string ProcName = "UOMMaster_InsertUpdate()";
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

                    param = command.Parameters.Add("@UOMId", SqlDbType.Int);
                    param.Value = UOMId;

                    param = command.Parameters.Add("@UOMTypeId", SqlDbType.Int);
                    param.Value = UOMTypeId;

                    param = command.Parameters.Add("@UOMName", SqlDbType.VarChar);
                    param.Value = UOMName;

                    param = command.Parameters.Add("@UOMCode", SqlDbType.VarChar);
                    param.Value = UOMCode;

                    param = command.Parameters.Add("@DecimalPoints", SqlDbType.Int);
                    param.Value = DecimalPoints;

                    param = command.Parameters.Add("@BaseUnit", SqlDbType.Bit);
                    param.Value = BaseUnit;

                    param = command.Parameters.Add("@ConversionRation", SqlDbType.Decimal);
                    param.Value = ConversionRation;

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

        #region UOMMaster  List
        public static DataSet UOMMaster_List(string strConn, int SelectionType, string SearchBy, string SearchString,
          int? IntID, Guid? CompanyId, int ItemsPerPage, int RequestPageNo, int CurrentPageNo)
        {

            const string ProcName = "UOMMaster_Select";
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

                    param = command.Parameters.Add("@UOMId", SqlDbType.Int);
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

        #region UOM Conversion
        public static DataSet UOMConversion_Get(string strConn, int SourceUOMId, int DestinationUOMId, Guid? CompanyId)
        {

            const string ProcName = "UOMConversion_Get";
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

                    param = command.Parameters.Add("@SourceUOMId", SqlDbType.Int);
                    param.Value = SourceUOMId;

                    param = command.Parameters.Add("@DestinationUOMId", SqlDbType.Int);
                    param.Value = DestinationUOMId;

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
