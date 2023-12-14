using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BirthdayCard
{
    public partial class UserMessage : System.Web.UI.Page
    {
        static string sCon = "Data Source=10.40.14.228;User ID=sa;Password=rjlw510213;Initial Catalog=lwportalapp";
        static SqlConnection con = new SqlConnection(sCon);
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region 添加消息日志
        public static void insertUserMsg(string words)
        {
            string sql = "insert into Z_Message(MsgContent,SendTime,IsClicked) values('" + words + "','" + DateTime.Now + "',0)";
            if (con.State != System.Data.ConnectionState.Open) //判断当前数据库连接状态,若不为打开,则打开
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            if (con.State != System.Data.ConnectionState.Closed) //判断当前数据库连接状态,若不为关闭,则关闭
                con.Close();
        }
        #endregion
        #region 拉取消息信息
        public static UserMsg getUserMsg(int msgid)
        {
            string sql = "select * from Z_Message where Id='" + msgid + "'";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, sql).Tables[0];
            string message = select_result.Rows[1].ToString();
            string sendtime = select_result.Rows[2].ToString();
            int isclicked = Convert.ToInt32(select_result.Rows[3].ToString());
            string clicktime = select_result.Rows[4].ToString();
            UserMsg userMsg = new UserMsg(msgid, message,sendtime,isclicked,clicktime);
            return userMsg;
        }

        #endregion
        
        public class UserMsg
        {
            public int msgid { get; set; }//消息ID(主键)
            public string content { get; set; }//消息内容
            public string sendtime { get; set; }//发送时间
            public int isclicked { get; set; }//是否被点击的标志
            public string clicktime { get; set; }//点击时间
            public UserMsg(int msgid, string content, string sendtime, int isclicked, string clicktime)
            {
                this.msgid = msgid;
                this.content = content;
                this.sendtime = sendtime;
                this.isclicked = isclicked;
                this.clicktime = clicktime;
            }
        }
        
    }
}