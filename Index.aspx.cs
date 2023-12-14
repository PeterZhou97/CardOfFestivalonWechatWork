using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiPay;

namespace BirthdayCard
{
    public partial class Index : System.Web.UI.Page
    {
        //System.DateTime currentTime = System.DateTime.Now;
        public static Thread threadAtuoSend;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //OnClick_Click(sender, e);
                //MyUtility.CelebrateEmpBirthday(Session["yggh"].ToString(), Session["name"].ToString());
            }
        }

        protected void OnClick_Click(object sender, EventArgs e)
        {

            while (true)
            {
                try
                {

                    if (threadAtuoSend == null)
                    {
                        threadAtuoSend = new Thread(new ThreadStart(Tick));
                        threadAtuoSend.Start();
                        Response.Write("提示：自动推送功能启动成功！");
                    }
                    else if (threadAtuoSend.ThreadState == ThreadState.Suspended)
                    {
                        threadAtuoSend.Resume();
                        Response.Write("提示：自动推送功能启动成功！");
                    }
                    else if (threadAtuoSend.ThreadState == ThreadState.Running
                        || threadAtuoSend.ThreadState == ThreadState.WaitSleepJoin)
                    {
                        Response.Write("提示：自动推送已经启动，无需重复启动！");
                    }
                    Thread.Sleep(60000);

                }
                catch (Exception ee)
                {
                    Response.Write("错误：" + ee.Message.ToString());
                }
            }
        }

        public void Tick()
        {

            #region 运行时间记录

            int month,hour, min, sec, day;
            month = System.DateTime.Now.Month;
            day = System.DateTime.Now.Day;
            hour = System.DateTime.Now.Hour;
            min = System.DateTime.Now.Minute;
            sec = System.DateTime.Now.Second;
            //8点推送
            if (hour == 8
                && min == 0
                && sec == 0)
            {
                //MyUtility.CelebrateEmpBirthday(Session["yggh"].ToString(), Session["name"].ToString());
            }

            //6月1日推送
            if (month == 6
                && day == 1
                && hour == 8
                && min == 0
                && sec == 0)
            {
               // MyUtility.CelebrationToChildren(Session["yggh"].ToString(), Session["name"].ToString());
            }

            #endregion
        }
    }
}