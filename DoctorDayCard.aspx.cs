using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BirthdayCard
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }
        protected static string getQuotations()
        {
            string sCon = "Data Source=10.40.14.228;User ID=sa;Password=rjlw510213;Initial Catalog=lwportalapp";
            SqlConnection con = new SqlConnection(sCon);
            //调用群发接口群发报表
            string s = "select TOP 1 * from Z_Quotations order by NEWID()";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            string Quotation = "";
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                Quotation += select_result.Rows[i][1].ToString().Replace("，", "，<br>").Replace("。", "。<br>") + "<br>" + select_result.Rows[i][2].ToString().Replace(",", ",<br>").Replace(".", ".<br>");
            }
            return Quotation;
        }
    }
}