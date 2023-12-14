using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;


namespace BirthdayCard
{
    public class Global : System.Web.HttpApplication
    {
        static string sCon = "Data Source=10.40.14.228;User ID=sa;Password=rjlw510213;Initial Catalog=lwportalapp";
        public System.Data.SqlClient.SqlConnection sqlcon = new System.Data.SqlClient.SqlConnection(sCon);

        protected void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            Application["counter"] = 0;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            Application.Lock();
            Application["counter"] = (int)Application["counter"] + 1;
            Application.UnLock();
            if (Session["IP"] != null)
            {

                //将当前访问者状态修改为在线(生日)
                string statussql = "update Z_Message set IsClicked=1,ClickTime='" + DateTime.Now + "' where CharIndex('" + Session["name"] + "',MsgContent)>0 and CharIndex('生日',MsgContent)>0 and SendTime>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                //定义SQL语句
                string updatetxt = "update Z_BrowseCount set IsOnline = 'true' where BrowseID = '" + Session["BrowseID"] + "'";

                if (sqlcon.State != System.Data.ConnectionState.Open) //判断当前数据库连接状态,若不为打开,则打开
                    sqlcon.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(updatetxt, sqlcon);
                System.Data.SqlClient.SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand(statussql, sqlcon);
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                if (sqlcon.State != System.Data.ConnectionState.Closed) //判断当前数据库连接状态,若不为关闭,则关闭
                    sqlcon.Close();
            }
            else
            {
                Session.Timeout = 10; //设置会话超时时间为10分钟
                Session["IP"] = Request.UserHostAddress;//获取访问者IP
                string ClientArea = IpSearch.GetAddressWithIP(Request.UserHostAddress.ToString().Trim());
                Session["BrowseTime"] = DateTime.Now;//获取访问时间
                Session["Browser"] = Request.Browser.Browser;//获取访问者使用的浏览器
                Session["OS"] = Request.Browser.Platform;//获取访问者使用的操作系统
                Random randobj = new Random(); //随机数生成器
                Session["BrowseID"] = DateTime.Now.ToString("HHmmss") + randobj.Next(1, 10000).ToString(); //生成一个访问编号,以便用户退出修改在线属性时作为数据修改的条件



                //将当前访问者信息添加至数据库

                //定义SQL语句
                string instxt = "insert into Z_BrowseCount(client_ip,client_time,client_area,client_browser,client_os,BrowseID,IsOnline) values('" + Session["IP"] + "','" + Session["BrowseTime"] + "','" + ClientArea + "','" + Session["Browser"] + "','" + Session["OS"] + "','" + Session["BrowseID"] + "','')";
                //更改日志表访问标志
                
                if (sqlcon.State != System.Data.ConnectionState.Open) //判断当前数据库连接状态,若不为打开,则打开
                    sqlcon.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(instxt, sqlcon);
                cmd.ExecuteNonQuery();
                if (sqlcon.State != System.Data.ConnectionState.Closed) //判断当前数据库连接状态,若不为关闭,则关闭
                    sqlcon.Close();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
            // 或 SQLServer，则不会引发该事件。
            Application.Lock();
            Application["counter"] = (int)Application["counter"] - 1;
            Application.UnLock();

            if (Session["IP"] != null)
            {
                //将当前访问者状态修改为不在线

                //定义SQL语句
                string updatetxt = "update Z_BrowseCount set IsOnline = 'false' where BrowseID = '" + Session["BrowseID"] + "'";

                if (sqlcon.State != System.Data.ConnectionState.Open) //判断当前数据库连接状态,若不为打开,则打开
                    sqlcon.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(updatetxt, sqlcon);
                cmd.ExecuteNonQuery();
                if (sqlcon.State != System.Data.ConnectionState.Closed) //判断当前数据库连接状态,若不为关闭,则关闭
                    sqlcon.Close();
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}