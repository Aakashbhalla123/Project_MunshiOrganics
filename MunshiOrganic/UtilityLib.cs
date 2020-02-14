using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace AOneNutsWeb 
{
    public class UtilityLib
    {
        public enum selectiontype
        {
            All = 0 ,
            Load = 1,
            DDl = 101
        }

        public static string FormatString(object value)
        {
            string outputString = string.Empty;

            if (value == null || value.Equals(DBNull.Value)) value = "";
            outputString = value.ToString().Trim();

            if (outputString.IndexOf("\r\n") > 0)
            {
                return outputString;
            }
            Regex regEx = new Regex(@"\s+");
            outputString = regEx.Replace(outputString, @" ");

            return outputString;
        }

        public static Guid? FormatGuid(string value)
        {
            Guid? output = null;
            if (value != null)
            {
                output = Guid.Parse(value);
                return (Guid)output;
            }
            return output;
        }

        public static int FormatNumber(object value)
        {
            int temp;
            if (Int32.TryParse(FormatString(value), out temp) == true)
                return Convert.ToInt32(FormatString(value));
            else
                return 0;
        }

        public static long FormatLongNumber(object value)
        {
            long temp;
            if (Int64.TryParse(FormatString(value), out temp) == true)
                return Convert.ToInt64(FormatString(value));
            else
                return 0;
        }

        public static decimal FormatDecimal(object value)
        {
            decimal temp;
            if (Decimal.TryParse(FormatString(value), out temp) == true)
                return Convert.ToDecimal(FormatString(value));
            else
                return 0.00M;
        }

        public static string FormatDecimalN2(object value)
        {
            decimal temp;
            if (Decimal.TryParse(FormatString(value), out temp) == true)
                return Convert.ToDecimal(FormatString(value)).ToString("n2");
            else
                return "0.00";
        }

        public static string FormatDecimalN2WithoutComma(object value)
        {
            decimal temp;
            if (Decimal.TryParse(FormatString(value), out temp) == true)
                return Convert.ToDecimal(FormatString(value)).ToString("n2").Replace(",", "");
            else
                return "0.00";
        }

        public static string FormatDecimalEmpty(object value)
        {
            decimal temp;
            if (Decimal.TryParse(FormatString(value), out temp) == true)
            {
                if (Convert.ToDecimal(FormatString(value)).ToString("n2") == "0.00")
                {
                    return string.Empty;
                }
                else
                {
                    return Convert.ToDecimal(FormatString(value)).ToString("n2").Replace(",", "");
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string FormatDecimalN2OrDash(object value)
        {
            decimal temp;
            string result = " - ";
            if (Decimal.TryParse(FormatString(value), out temp) == true)
            {
                result = Convert.ToDecimal(FormatString(value)).ToString("n2");
            }
            else
            {
                result = " - &nbsp;&nbsp; ";
            }
            if (result == "0.00")
            {
                result = "  -  &nbsp;&nbsp; ";
            }
            return result;


        }

        public static string FormatDecimalN2OrZero(object value)
        {
            decimal temp;
            string result = " 0.00 ";
            if (Decimal.TryParse(FormatString(value), out temp) == true)
            {
                result = Convert.ToDecimal(FormatString(value)).ToString("n2");
            }
            else
            {
                result = "0.00";
            }
            if (result == "0.00")
            {
                result = "  0.00 ";
            }
            return result;
        }


        public static float FormatFloat(object value)
        {
            float temp;
            if (float.TryParse(FormatString(value), out temp) == true)
                return temp;
            else
                return 0.0f;
        }

        public static DateTime FormatDate(object value)
        {
            if (value != null && value.Equals(DBNull.Value) == false && value.ToString().Length > 0)
                return DateTime.Parse(UtilityLib.FormatString(value));
            else
                return DateTime.MinValue;
        }

        public static DateTime? FormatDateForNullable(object value)
        {
            if (value != null && value.Equals(DBNull.Value) == false && value.ToString().Length > 0)
                return DateTime.Parse(UtilityLib.FormatString(value));
            else
                return null;
        }

        public static Boolean FormatBoolean(object value)
        {
            if (value != null && value.Equals(DBNull.Value) == false && value.ToString().Length > 0)
                return Boolean.Parse(UtilityLib.FormatString(value));
            else
                return false;
        }

        /// <summary>
        /// Determines whether the specified value is number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is number; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumber(string value)
        {
            Regex _isNumber = new Regex("[^0-9]");
            if (!_isNumber.IsMatch(value))
            {
                try
                {
                    Convert.ToInt32(value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool IsDateWithinLowerRange(DateTime date)
        {
            if (date.Date < new DateTime(2000, 1, 1))
            {
                return false;
            }
            return true;
        }

        public static DateTime GetMonthStartDate(DateTime date)
        {
            if (date != null && date != DateTime.MinValue)
            {
                return new DateTime(date.Year, date.Month, 1);

            }
            else
            {
                DateTime todayDt;
                todayDt = DateTime.Today;
                return new DateTime(todayDt.Year, todayDt.Month, 1);

            }

        }

        //public static string FormatString(object value)
        //{
        //    string outputString = string.Empty;

        //    if (value == null || value.Equals(DBNull.Value)) value = "";
        //    outputString = value.ToString().Trim();

        //    if (outputString.IndexOf("\r\n") > 0)
        //    {
        //        return outputString;
        //    }
        //    Regex regEx = new Regex(@"\s+");
        //    outputString = regEx.Replace(outputString, @" ");

        //    return outputString;
        //}


        public static Guid? FormatGuid(object value)
        {
            Guid temp;
            if (Guid.TryParse(FormatString(value), out temp) == true)
                return (Guid)value;
            else
                return null;
        }

        /// <summary>
        /// Determines whether the specified the value is decimal.
        /// </summary>
        /// <param name="theValue">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified the value is decimal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDecimal(string theValue)
        {
            Regex _isDecimal = new Regex(@"^-?[0-9]+(\.[0-9]{1,2})?$");
            return !_isDecimal.IsMatch(theValue);
        }


        public static bool IsDecimalNumber(string theValue)
        {
            theValue = theValue.Replace(".", "");
            return IsNumber(theValue);
            //Regex _isDecimal = new Regex(@"^-?[0-9]+(\.[0-9]{1,2})?$");
            //return !_isDecimal.IsMatch(theValue);
        }

        public static string GetMacAddress()
        {
            string macAddress = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        macAddress = nic.GetPhysicalAddress().ToString();
                    }
                }
            }
            return macAddress;
        }

        public static string GetConnectionString()
        {
            string serverName = System.Configuration.ConfigurationManager.AppSettings["ServerName"].ToString();
            string databaseName = System.Configuration.ConfigurationManager.AppSettings["Database"].ToString();
            string userId = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
            string password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
            string strResult = serverName + ":" + databaseName + ":" + userId + ":" + password;
            return strResult;
        }

        public static string SendMail(string toAddress, string subject, string body)
        {
            string msg = string.Empty;
            try
            {
                string fromAddress = System.Configuration.ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                string password = System.Configuration.ConfigurationManager.AppSettings["Emailpassword"].ToString();
                string smptHost = System.Configuration.ConfigurationManager.AppSettings["Emailhost"].ToString();
                int portNo = UtilityLib.FormatNumber(System.Configuration.ConfigurationManager.AppSettings["Emailport"].ToString());
                bool sslEnabled = UtilityLib.FormatBoolean(System.Configuration.ConfigurationManager.AppSettings["EmailsslEnabled"].ToString());
                bool UseDefault = true;

                MailAddress fromMailAddress = new MailAddress(fromAddress);
                MailAddress toMailAddress = new MailAddress(toAddress);
                SmtpClient smtp = new SmtpClient
                {
                    Host = smptHost,  //"smtp.gmail.com",
                    Port = portNo, //587, //465
                    EnableSsl = sslEnabled,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = UseDefault,
                    Credentials = new NetworkCredential(fromMailAddress.Address, password)
                };
                using (MailMessage message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })

                    smtp.Send(message);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw ex;
            }
            return msg;
        }

        public static string SendSMS(string mobileNo, string message)
        {

            string SentResult = String.Empty;
            string StatusCode = String.Empty;
            string url = string.Empty;
            try
            {
                if (mobileNo.Length > 0)
                {
                    if (mobileNo.Substring(0, 1) != "+")
                    {
                        mobileNo = '+' + mobileNo;
                    }
                }

                string smsUser = System.Configuration.ConfigurationManager.AppSettings["SmsUser"].ToString();
                string smsPassword = System.Configuration.ConfigurationManager.AppSettings["SmsPassword"].ToString();
                string smsSenderID = System.Configuration.ConfigurationManager.AppSettings["SmsSenderID"].ToString();

                url = "http://bulksms.smsforbulk.com/sendsms.jsp?user=" + smsUser.Trim() + "&password="+ smsPassword.Trim() + "&mobiles=" + mobileNo.Trim() + "&sms=" + message.Trim() + "&senderid="+ smsSenderID.Trim();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader responseReader = new StreamReader(response.GetResponseStream());

                String resultmsg = responseReader.ReadToEnd();
                responseReader.Close();

                int StartIndex = 0;
                int LastIndex = resultmsg.Length;

                if (LastIndex > 0)
                    SentResult = resultmsg.Substring(StartIndex, LastIndex);

                responseReader.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SentResult;
        }

        //public static string FormatDateDDMMYYYY(object value)
        //{
        //    string returnValue = string.Empty;
        //    DateTime dt;
        //    try
        //    {
        //        if (value != null && value.Equals(DBNull.Value) == false && value.ToString().Length > 0)
        //        {
        //            dt = DateTime.Parse(Utility.FormatString(value), new CultureInfo("en-GB", false));
        //            returnValue = dt.ToString("dd/MM/yyyy");
        //        }
        //        else
        //        {
        //            returnValue = string.Empty;
        //        }
        //    }
        //    catch
        //    {
        //        return returnValue;
        //    }
        //    return returnValue;
        //}

    }
}