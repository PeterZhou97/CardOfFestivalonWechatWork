using BirthdayCard;
using Senparc.Weixin;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.MailList;
using Senparc.Weixin.QY.AdvancedAPIs.Mass;
using Senparc.Weixin.QY.CommonAPIs;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WeiPay
{
    public class MyUtility
    {
        #region XML序列化方法
        /// <summary>
        /// 将实体类转换成XML方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            try
            {
                string result = string.Empty;
                using (MemoryStream output = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(output, obj);
                    result = System.Text.Encoding.UTF8.GetString(output.ToArray());
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString() + "执行XML序列化方法失败。");
            }
        }

        #endregion //XML序列化方法

        #region XML反序列化方法
        /// <summary>
        /// 把XML转换成相应的实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static T DESerializer<T>(string strXML) where T : class
        {
            try
            {
                T result = null;
                using (MemoryStream input = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strXML)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    result = serializer.Deserialize(input) as T;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString() + "执行XML反序列化方法失败。");
            }
        }

        #endregion //XML反序列化方法
        /// <summary>
        /// 名称：获取员工工号科室对象方法
        /// </summary>
        /// <param name="userid_para"></param>
        /// <returns></returns>
        //public static Entity.UseridAndKsdm GetUseridAndKsdm(string userid_para)
        //{
        //    try
        //    {
        //        Entity.UseridAndKsdm useridAndKsdm = new Entity.UseridAndKsdm();
        //        Entity.queryParaForUseridAndKsdm para = new Entity.queryParaForUseridAndKsdm(userid_para);
        //        string xmlPara = MyUtility.XmlSerialize<Entity.queryParaForUseridAndKsdm>(para);
        //        string xmlQueryResult = "";
        //        StringReader reader = new StringReader(xmlQueryResult);
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(reader);
        //        XmlNode node = doc.SelectSingleNode("/queryResult/resultCode");
        //        string resultCode = node.InnerText;
        //        node = doc.SelectSingleNode("/queryResult/message");
        //        string message = node.InnerText;
        //        node = doc.SelectSingleNode("/queryResult/errorMsg");
        //        string errorMsg = node.InnerText;
        //        if (resultCode == "0")
        //        {
        //            useridAndKsdm.userid = userid_para;
        //            useridAndKsdm.ksdm = "0";
        //        }
        //        else
        //            useridAndKsdm = MyUtility.DESerializer<Entity.UseridAndKsdm>(message);

        //        return useridAndKsdm;

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("执行获取员工工号科室对象方法失败。" + e.Message.ToString());
        //    }
        //}
        
        public static void CelebrationForNewYear()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5'); ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您元旦快乐！工作一切顺利！请点击下方链接查看您的元旦贺卡：https://dx.rjlwh.com.cn:443/NewYearDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022",words , 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送元旦快乐信息
        public static void CelebrationForSpringFestival()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5'); ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您春节快乐！工作一切顺利！请点击下方链接查看您的春节贺卡：https://dx.rjlwh.com.cn:443/SpringFestivalCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送春节快乐信息

        public static void CelebrationForLanternFestival()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5'); ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您元宵节快乐！工作一切顺利！请点击下方链接查看您的元宵节贺卡：https://dx.rjlwh.com.cn:443/LanternFestivalCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送元宵节快乐信息

        public static void CelebrationForWomen()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5') and EMPSEX=1; ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您妇女节快乐！工作一切顺利！请点击下方链接查看您的妇女节贺卡：https://dx.rjlwh.com.cn:443/WomenDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022",words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送妇女节快乐信息

        public static void CelebrationForPlantingTreeDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5'); ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您植树节快乐！工作一切顺利！请点击下方链接查看您的植树节贺卡：https://dx.rjlwh.com.cn:443/PlantingTreeDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送植树节快乐信息

        public static void CelebrationForLaborDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5'); ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您劳动节快乐！工作一切顺利！请点击下方链接查看您的劳动节贺卡：https://dx.rjlwh.com.cn:443/LaborDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022",words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送劳动节快乐信息

        public static void CelebrationForYouthDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5'); ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您青年节快乐！工作一切顺利！请点击下方链接查看您的青年节贺卡：https://dx.rjlwh.com.cn:443/YouthDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送青年节快乐信息

        public static void CelebrationForMothers()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5'); ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您母亲节快乐！工作一切顺利！请点击下方链接查看您的母亲节贺卡：https://dx.rjlwh.com.cn:443/MotherDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送母亲节快乐信息

        //public static void CelebrateEmpBirthday(string userid, string username)
        //{
        //    //1. 获取accessToken
        //    string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

        //    //2. 获取taglist（标签列表）
        //    GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

        //    //1. 获取微信消息推送接口参数
        //    string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
        //    SqlConnection con = new SqlConnection(sCon);
        //    #region 2. 遍历标签，获取推送报表内容，并推送报表
        //    //调用群发接口群发报表
        //    string s = "select * from org_employee where month(EMPBIRTH) = month(getdate()) and day(EMPBIRTH) = day(getdate())";
        //    DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
        //    for (int i = 0; i < select_result.Rows.Count; i++)
        //    {
        //        if (select_result.Rows[i][4].ToString() == userid && select_result.Rows[i][3].ToString() == username)
        //        {
        //            MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您生日快乐！工作一切顺利！请点击下方链接查看您的生日贺卡：https://dx.rjlwh.com.cn:7007/MobileCard.aspx", 0, 5000);
        //        }

        //    }
        //    ////记录错误日志
        //    //Console.WriteLine(result.errcode + "\n" + result.errmsg + "\n" + result.invalidparty + "\n" + result.invaliduser + "\n" + result.invalidtag);

        //}

       
        public static void CelebrationToChildren2()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');  ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您儿童节快乐！工作一切顺利！请点击下方链接查看您的儿童节贺卡：https://dx.rjlwh.com.cn:443/ChildrenCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送儿童节快乐信息
        //public static void CelebrationToChildren(string userid,string username)
        //{
        //    //1. 获取accessToken
        //    string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

        //    //2. 获取taglist（标签列表）
        //    GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

        //    //1. 获取微信消息推送接口参数
        //    string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
        //    SqlConnection con = new SqlConnection(sCon);
        //    #region 2. 遍历标签，获取推送报表内容，并推送报表
        //    //调用群发接口群发报表
        //    string s = "select * from org_employee;";
        //    DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
        //    for (int i = 0; i < select_result.Rows.Count; i++)
        //    {
                
        //            MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您儿童节快乐！工作一切顺利！请点击下方链接查看您的儿童节贺卡：http://dx.rjlwh.com.cn:7007/ChildrenCard.aspx", 0, 5000);
                
                
        //    }
        //}
        //#endregion //发送儿童节快乐信息


        public static void CelebrateEmpBirthday2()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where month(EMPBIRTH) = month(getdate()) and day(EMPBIRTH) = day(getdate()) and userisdeleted=0 and person_type_id in ('1','2','5')";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            //发送消息并插入日志
            
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                    string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您生日快乐！工作一切顺利！请点击下方链接查看您的生日贺卡：https://dx.rjlwh.com.cn:443/MobileCard.aspx";
                    MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                    UserMessage.insertUserMsg(words);
            }
            ////记录错误日志
            //Console.WriteLine(result.errcode + "\n" + result.errmsg + "\n" + result.invalidparty + "\n" + result.invaliduser + "\n" + result.invalidtag);

        }
        #endregion //发送生日快乐信息

        public static void CelebrationToNurses()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5') and EMPPOSITION='护理' OR EMPNUMBER in ('71080','71081','71082');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您护士节快乐！工作一切顺利！请点击下方链接查看您的护士节贺卡：https://dx.rjlwh.com.cn:443/NurseDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022",words , 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送护士节快乐信息

        public static void CelebrationForDragonFestival()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您端午节快乐！工作一切顺利！请点击下方链接查看您的端午节贺卡：https://dx.rjlwh.com.cn:443/DragonFestivalCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送端午节快乐信息
        public static void CelebrateForFathers()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5'); ";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您父亲节快乐！工作一切顺利！请点击下方链接查看您的父亲节贺卡：https://dx.rjlwh.com.cn:443/FatherDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送父亲节快乐信息
        public static void CelebrationForPartyBirthday()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您建党节快乐！工作一切顺利！请点击下方链接查看您的建党节贺卡：https://dx.rjlwh.com.cn:443/PartyBirthdayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送建党节快乐信息

        public static void CelebrationForArmyDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您建军节快乐！工作一切顺利！请点击下方链接查看您的建军节贺卡：https://dx.rjlwh.com.cn:443/ArmyDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送建军节快乐信息

        public static void CelebrationForDouble7thDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您七夕节快乐！工作一切顺利！请点击下方链接查看您的七夕节贺卡：https://dx.rjlwh.com.cn:443/Double7thDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送七夕节快乐信息

        public static void CelebrationToDoctors()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表,面向对象：全院医师
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5') and EMPPOSITION in ('医疗','医技') and EMPNUMBER not in ('sys','71080','71081','71082');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您医师节快乐！工作一切顺利！请点击下方链接查看您的医师节贺卡：https://dx.rjlwh.com.cn:443/DoctorDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送医师节快乐信息

        public static void CelebrationForTeachersDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您教师节快乐！工作一切顺利！请点击下方链接查看您的教师节贺卡：https://dx.rjlwh.com.cn:443/TeachersDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送教师节快乐信息

        public static void CelebrationForMidAutumnFestival()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您中秋节快乐！工作一切顺利！请点击下方链接查看您的中秋节贺卡：https://dx.rjlwh.com.cn:443/MidAutumnCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送中秋节快乐信息

        public static void CelebrationForNationalDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您国庆节快乐！工作一切顺利！请点击下方链接查看您的国庆节贺卡：https://dx.rjlwh.com.cn:443/NationalDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送国庆节快乐信息
        public static void CelebrationForDouble9thDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                string words = "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您重阳节快乐！工作一切顺利！请点击下方链接查看您的重阳节贺卡：https://dx.rjlwh.com.cn:443/Double9thDayCard.aspx";
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", words, 0, 5000);
                UserMessage.insertUserMsg(words);
            }
        }
        #endregion //发送重阳节快乐信息

        public static void CelebrationForHalloween()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您万圣节快乐！工作一切顺利！请点击下方链接查看您的万圣节贺卡：https://dx.rjlwh.com.cn:443/HalloweenCard.aspx", 0, 5000);

            }
        }
        #endregion //发送感恩节快乐信息
        public static void CelebrationForThanksgivingDay()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您感恩节快乐！工作一切顺利！请点击下方链接查看您的感恩节贺卡：https://dx.rjlwh.com.cn:443/ThanksgivingDayCard.aspx", 0, 5000);

            }
        }
        #endregion //发送感恩节快乐信息

        public static void CelebrationForChristmas()
        {
            //1. 获取accessToken
            string accesstoken = AccessTokenContainer.TryGetToken("Your WeixinWork AppID", "Your WeixinWork Corscret");

            //2. 获取taglist（标签列表）
            GetTagListResult tagList = MailListApi.GetTagList(accesstoken);

            //1. 获取微信消息推送接口参数
            string sCon = ConfigurationManager.AppSettings["Conn"].ToString().Trim();
            SqlConnection con = new SqlConnection(sCon);
            #region 2. 遍历标签，获取推送报表内容，并推送报表
            //调用群发接口群发报表
            string s = "select * from org_employee where userisdeleted=0 and person_type_id in ('1','2','5');";
            DataTable select_result = LZX.SqlDAL.SqlHelper.ExecuteDataset(sCon, CommandType.Text, s).Tables[0];
            for (int i = 0; i < select_result.Rows.Count; i++)
            {
                MassResult result = MassApi.SendText(accesstoken, select_result.Rows[i][4].ToString(), string.Empty, "1", "1000022", "亲爱的" + select_result.Rows[i][3].ToString() + "，祝您圣诞节快乐！工作一切顺利！请点击下方链接查看您的圣诞节贺卡：https://dx.rjlwh.com.cn:443/ChristmasCard.aspx", 0, 5000);

            }
        }
        #endregion //发送圣诞节快乐信息
    }//class

}//namespace





