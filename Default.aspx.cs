using Eureka.Mobile360.Commons;
using Newtonsoft.Json;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.MailList;
using Senparc.Weixin.QY.AdvancedAPIs.Mass;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.Entities;
using SWX.Utils;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using ThirdParty.Json.LitJson;
using WeiPay;

namespace BirthdayCard
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string code = Request.QueryString["code"];
                //你的回发地址
                //string corpid = "ww18af6aae84975571";
                //string corpsecret = "weQWl6X4fYpS6hBJGhoFZhf_6xr1hjR9LrNYKRkAvfg";
                string appid = "ww18af6aae84975571";
                string redirect_uri = "https://dx.rjlwh.com.cn:443";
                if (string.IsNullOrEmpty(code))
                {
                    string code_url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=SCOPE&agentid=agentid&state=STATE#wechat_redirect  ", appid, redirect_uri);
                    //回发后会自带code参数
                    Response.Redirect(code_url);
                    return;
                }
                else
                {

                    string accesstoken;
                {
                    //code = Request.QueryString["code"].ToString();
                    //accesstoken = GetAccessToken();
                    accesstoken = JsCode2Session();
                    //accesstoken = "IO3cebiD7lAaOfv_0bka8dRXKZTRM9r281qhX9o5boTB4ZNXRA2mI8zeIz0D9L64Ukuuxw7OrbTO2_ivas2jCq8l27JPOpLhrZTG79bfcsBP8kWoK-ecEUWrd6NTn78fjuEAY6yYoAmpujWEtxqkn_xW91CZoE5RnDjiBvwCMeL0Rz_JOVRN73eswmyac3AcviyIj_Ry2qKt0vtjT7M2AQ";
                    //Label1.Text = accesstoken;
                    OAuth_userinfo Model = Get_userinfo(accesstoken, code);
                    if (Model.UserId != null && Model.UserId != "")
                    {

                        string info = getUserInfo(accesstoken, Model.UserId);
                        var DynamicObject = JsonConvert.DeserializeObject<UserInfo>(info);
                        //Response.Write("<span style='color:#FF0000;font-size:20px'>" + info + "</span>");
                        //Response.Write("<span style='color:#FF0000;font-size:20px'>" + DynamicObject.userid + "</span></br>");
                        //Response.Write("<span style='color:#FF0000;font-size:20px'>" + DynamicObject.name + "</span></br>");
                        //Response.Write("<span style='color:#FF0000;font-size:20px'>" + DynamicObject.mobile + "</span></br>");
                        Session["yggh"] = DynamicObject.userid;
                        Session["name"] = DynamicObject.name;
                        Session["mobile"] = DynamicObject.mobile;
                        Server.Transfer("~/AutoMessage.aspx");

                    }

                    else
                        Response.Write(Model.errcode + ":" + Model.errmsg);
                   }
                }
            }
                
        }
    
       
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
            }
            return returnText;
        }
        protected OAuth_userinfo Get_userinfo(string AccessToken, string Code)
        {

            string Str = GetJson("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token=" + AccessToken + "&code=" + Code + "&agentid=0");

            OAuth_userinfo Oauth_Token_userinfo = JsonHelper.ParseFromJson<OAuth_userinfo>(Str);
            return Oauth_Token_userinfo;
        }
        //public string GetAccessToken()
        //{
        //    WebClient webClient = new WebClient();
        //    Byte[] bytes = webClient.DownloadData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token=" + AccessToken + "&code=" + Code + "&agentid=0"));
        //    string result = Encoding.GetEncoding("utf-8").GetString(bytes);

        //    //JObject jObj = JObject.Parse(result);      
        //    //JToken token = jObj["access_token"];     
        //    //return token.ToString().Substring(1, token.ToString().Length - 2);  

        //    string[] temp = result.Split(',');
        //    string[] tp = temp[0].Split(':');
        //    return tp[1].ToString().Replace("\"", "").Trim().ToString();

        //}

        public class UserInfo
        {
            public string userid { get; set; }//用户的唯一标识
            public string name { get; set; }//用户姓名
            public string mobile { get; set; }//手机号码

            public UserInfo(string userid, string name, string mobile)
            {
                this.userid = userid;
                this.name = name;
                this.mobile = mobile;
            }

        }
       
        private string getUserInfo(string AccessToken, string userid)
        {
            string web_url = "https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token=" + AccessToken + "&userid=" + userid + "";
            WebRequest request = WebRequest.Create(web_url);
            //request.ContentType = "application/x-www-form-urlencoded";

            request.Method = "GET";

            WebResponse respone = request.GetResponse();

            Stream sr = respone.GetResponseStream();

            StreamReader sd = new StreamReader(sr);

            string mess = sd.ReadToEnd();

            //Response.Write(mess);
            //处理JSOn对象 转成自己需要的对象
            //JavaScriptSerializer js = new JavaScriptSerializer();

            //Json.JsonCode jg = js.Deserialize<Json.JsonCode>(mess);

            sd.Close();

            return mess;
        }
        

        
        public string JsCode2Session()
        {
            string appid = "ww18af6aae84975571";
            string secret = "rEggq1XBKskn8KYzcmqwcOT7Kotwpq-sH6vjYBHOybg";
            string JsCode2SessionUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
            var url = string.Format(JsCode2SessionUrl, appid, secret);
            var str = GetFunction(url);
            try
            {
                JsonData jo = JsonMapper.ToObject(str);
                string access_token = jo["access_token"].ToString();
                return access_token;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //public string Name(string accesstoken,string userid)

        //public string GetFunction(string url)
        //{
        //    string serviceAddress = url;
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
        //    request.Method = "GET";
        //    request.ContentType = "textml;charset=UTF-8";
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    Stream myResponseStream = response.GetResponseStream();
        //    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
        //    string retString = myStreamReader.ReadToEnd();
        //    myStreamReader.Close();
        //    myResponseStream.Close();
        //    //Response.Write(retString);
        //    return retString;
        //}
        public string GetFunction(string url)
        {

            string serviceAddress = url;
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public class OAuth_userinfo
        {
            public OAuth_userinfo()
            {

                //
                //TODO: 在此处添加构造函数逻辑
                //
            }

            public string _UserId;
            public string _errcode;
            public string _errmsg;


            public string errmsg
            {
                set { _errmsg = value; }
                get { return _errmsg; }
            }

            public string errcode
            {
                set { _errcode = value; }
                get { return _errcode; }
            }
            public string UserId
            {
                set { _UserId = value; }
                get { return _UserId; }
            }



        }

        public class JsonHelper
        {
            /// <summary>  
            /// 生成Json格式  
            /// </summary>  
            /// <typeparam name="T"></typeparam>  
            /// <param name="obj"></param>  
            /// <returns></returns>  
            public static string GetJson<T>(T obj)
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);
                    string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
                }
            }
            /// <summary>  
            /// 获取Json的Model  
            /// </summary>  
            /// <typeparam name="T"></typeparam>  
            /// <param name="szJson"></param>  
            /// <returns></returns>  
            public static T ParseFromJson<T>(string szJson)
            {
                T obj = Activator.CreateInstance<T>();
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)serializer.ReadObject(ms);
                }
            }
        }
    }
}