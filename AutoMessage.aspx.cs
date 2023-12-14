using System;
using System.Globalization;
using System.Net;
using System.Threading;
using WeiPay;

namespace BirthdayCard
{
    
    public partial class AutoMessage : System.Web.UI.Page
    {
        public static Thread threadAtuoSend;
        System.DateTime currentTime = System.DateTime.Now;

        #region 按某月第几个星期几
        private static WeekHoliday[] wHolidayInfo = new WeekHoliday[]{
            new WeekHoliday(5, 2, 1, "母亲节"),
            new WeekHoliday(6, 3, 1, "父亲节"),
            new WeekHoliday(11, 4, 5, "感恩节")
        };
        #endregion

        //C# 获取农历日期

        ///<summary>
        /// 实例化一个 ChineseLunisolarCalendar
        ///</summary>
        private static ChineseLunisolarCalendar ChineseCalendar = new ChineseLunisolarCalendar();
  
        ///<summary>
        /// 十天干
        ///</summary>
        private static string[] tg = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        ///<summary>
        /// 十二地支
        ///</summary>
        private static string[] dz = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        ///<summary>
        /// 十二生肖
        ///</summary>
        private static string[] sx = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        ///<summary>
        /// 返回农历天干地支年
        ///</summary>
        ///
        ///<summary>
        /// 农历月
        ///</summary>

        ///<return s></return s>
        private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二(腊)" };

        ///<summary>
        /// 农历日
        ///</summary>
        private static string[] days1 = { "初", "十", "廿", "三" };
        ///<summary>
        /// 农历日
        ///</summary>
        private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

        #region ConvertDayOfWeek
        /// <summary>
        /// 将星期几转成数字表示
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        private int ConvertDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 7;
                default:
                    return 0;
            }
        }
        #endregion


        #region CompareWeekDayHoliday
        /// <summary>
        /// 比较当天是不是指定的第周几
        /// </summary>
        /// <param name="date"></param>
        /// <param name="month"></param>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private bool CompareWeekDayHoliday(DateTime date, int month, int week, int day)
        {
            bool ret = false;

            if (date.Month == month) //月份相同
            {
                if (ConvertDayOfWeek(date.DayOfWeek) == day) //星期几相同
                {
                    DateTime firstDay = new DateTime(date.Year, date.Month, 1);//生成当月第一天
                    int i = ConvertDayOfWeek(firstDay.DayOfWeek);
                    int firWeekDays = 7 - ConvertDayOfWeek(firstDay.DayOfWeek) + 1; //计算第一周剩余天数

                    if (i > day)
                    {
                        if ((week - 1) * 7 + day + firWeekDays == date.Day)
                        {
                            ret = true;
                        }
                    }
                    else
                    {
                        if (day + firWeekDays + (week - 2) * 7 == date.Day)
                        {
                            ret = true;
                        }
                    }
                }
            }

            return ret;
        }
        #endregion
        #region WeekDayHoliday
        /// <summary>
        /// 按某月第几周第几日计算的节日
        /// </summary>
        public string WeekDayHoliday
        {
            get
            {
                string tempStr = "";
                foreach (WeekHoliday wh in wHolidayInfo)
                {
                    if (CompareWeekDayHoliday(currentTime, wh.Month, wh.WeekAtMonth, wh.WeekDay))
                    {
                        tempStr = wh.HolidayName;
                        break;
                    }
                }
                return tempStr;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
           
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            if (!IsPostBack)
            {
                
                MyUtility.CelebrateEmpBirthday2();
               
                //MyUtility.CelebrationForDragonFestival();
                if (currentTime.Month == 12 && currentTime.Day == 31)
                {
                    MyUtility.CelebrationForNewYear();
                }
                if (GetChineseDateTime(currentTime.AddDays(2)).IndexOf("正月初一") != -1)
                {
                    MyUtility.CelebrationForSpringFestival();
                }
                if (GetChineseDateTime(currentTime).IndexOf("正月十五") != -1)
                {
                    MyUtility.CelebrationForLanternFestival();
                }
                if (currentTime.Month == 3 && currentTime.Day == 8)
                {
                    MyUtility.CelebrationForWomen();
                }
                if (currentTime.Month == 3 && currentTime.Day == 12)
                {
                    MyUtility.CelebrationForPlantingTreeDay();
                }
                if(currentTime.Month==5 && currentTime.Day == 1)
                {
                    MyUtility.CelebrationForLaborDay();
                }
                if (currentTime.Month == 5 && currentTime.Day == 4)
                {
                    MyUtility.CelebrationForYouthDay();
                }
                if (WeekDayHoliday.Equals("母亲节"))
                {
                    MyUtility.CelebrationForMothers();
                }
                if(currentTime.Month == 5 && currentTime.Day == 12)
                {
                    MyUtility.CelebrationToNurses();
                }
                if (currentTime.Month == 6 && currentTime.Day == 1)
                {
                    MyUtility.CelebrationToChildren2();
                }
                if (WeekDayHoliday.Equals("父亲节"))
                {
                    MyUtility.CelebrateForFathers();
                }
                if (GetChineseDateTime(currentTime).IndexOf("五月初五")!=-1)
                {
                    MyUtility.CelebrationForDragonFestival();
                }
                if (currentTime.Month==7 && currentTime.Day==1)
                {
                    MyUtility.CelebrationForPartyBirthday();
                }
                if (currentTime.Month == 8 && currentTime.Day == 1)
                {
                    MyUtility.CelebrationForArmyDay();
                }
                if (GetChineseDateTime(currentTime).IndexOf("七月初七") != -1)
                {
                    MyUtility.CelebrationForDouble7thDay();
                }
                if (currentTime.Month == 8 && currentTime.Day ==19)
                {
                    MyUtility.CelebrationToDoctors();
                }
                if(currentTime.Month==9 && currentTime.Day == 10)
                {
                    MyUtility.CelebrationForTeachersDay();
                }
                if (GetChineseDateTime(currentTime).IndexOf("八月十五") != -1)
                {
                    MyUtility.CelebrationForMidAutumnFestival();
                }
                if (currentTime.Month==10 && currentTime.Day==1)
                {
                    MyUtility.CelebrationForNationalDay();
                }
                if (GetChineseDateTime(currentTime).IndexOf("九月初九") != -1)
                {
                    MyUtility.CelebrationForDouble9thDay();
                }
                //if (currentTime.Month == 10 && currentTime.Day == 31)
                //{
                //    MyUtility.CelebrationForHalloween();
                //}
                //if (WeekDayHoliday.Equals("感恩节"))
                //{
                //    MyUtility.CelebrationForThanksgivingDay();
                //}
                //if (currentTime.Month == 12 && currentTime.Day == 24)
                //{
                //    MyUtility.CelebrationForChristmas();
                //}
            }
        }

        protected void OnClick_Click(object sender, EventArgs e)
        {
            while (true)
            {
                //try
                //{

                //    if (threadAtuoSend == null)
                //    {
                //        threadAtuoSend = new Thread(new ThreadStart(Tick));
                //        threadAtuoSend.Start();
                //        Response.Write("提示：自动推送功能启动成功！");
                //    }
                //    else if (threadAtuoSend.ThreadState == ThreadState.Suspended)
                //    {
                //        threadAtuoSend.Resume();
                //        Response.Write("提示：自动推送功能启动成功！");
                //    }
                //    else if (threadAtuoSend.ThreadState == ThreadState.Running
                //        || threadAtuoSend.ThreadState == ThreadState.WaitSleepJoin)
                //    {
                //        Response.Write("提示：自动推送已经启动，无需重复启动！");
                //    }
                //    Thread.Sleep(60000);

                //}
                //catch (Exception ee)
                //{
                //    Response.Write("错误：" + ee.Message.ToString());
                //}
            }
        }
        //public void Tick()
        //{

        //    #region 运行时间记录

        //    int month, hour, min, sec, day;
        //    month = System.DateTime.Now.Month;
        //    day = System.DateTime.Now.Day;
        //    hour = System.DateTime.Now.Hour;
        //    min = System.DateTime.Now.Minute;
        //    sec = System.DateTime.Now.Second;
        //    //8点推送
        //    if (hour == 8
        //        && min == 0
        //        && sec == 0)
        //    {
        //        MyUtility.CelebrateEmpBirthday2();
        //    }

        //    #endregion
        //}

        ///<param name="year">农历年</param>
        ///<return s></return s>
        public static string GetLunisolarYear(int year)
        {
            if (year > 3)
            {
                int tgIndex = (year - 4) % 10;
                int dzIndex = (year - 4) % 12;

                return string.Concat(tg[tgIndex], dz[dzIndex], "[", sx[dzIndex], "]");
            }

            throw new ArgumentOutOfRangeException("无效的年份!");
        }

        ///<summary>
        /// 返回农历月
        ///</summary>
        ///<param name="month">月份</param>
        ///<return s></return s>
        public static string GetLunisolarMonth(int month)
        {
            if (month < 13 && month > 0)
            {
                return months[month - 1];
            }

            throw new ArgumentOutOfRangeException("无效的月份!");
        }

        ///<summary>
        /// 返回农历日
        ///</summary>
        ///<param name="day">天</param>
        ///<return s></return s>
        public static string GetLunisolarDay(int day)
        {
            if (day > 0 && day < 32)
            {
                if (day != 20 && day != 30)
                {
                    return string.Concat(days1[(day - 1) / 10], days[(day - 1) % 10]);
                }
                else
                {
                    return string.Concat(days[(day - 1) / 10], days1[1]);
                }
            }

            throw new ArgumentOutOfRangeException("无效的日!");
        }

        ///<summary>
        /// 根据公历获取农历日期
        ///</summary>
        ///<param name="datetime">公历日期</param>
        ///<return s></return s>
        public static string GetChineseDateTime(DateTime datetime)
        {
            int year = ChineseCalendar.GetYear(datetime);
            int month = ChineseCalendar.GetMonth(datetime);
            int day = ChineseCalendar.GetDayOfMonth(datetime);
            //获取闰月， 0 则表示没有闰月
            int leapMonth = ChineseCalendar.GetLeapMonth(year);

            bool isleap = false;

            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }

            return string.Concat(GetLunisolarYear(year), "年", isleap ? "闰" : string.Empty, GetLunisolarMonth(month), "月", GetLunisolarDay(day));
        }

    }
}