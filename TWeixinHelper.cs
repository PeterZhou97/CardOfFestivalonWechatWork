using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WeiXinEnterprise
{
    public class WXQYHHelper
    {
        private WXQYHHelper() { }

        static string CORPID;
        static string SECRET;

        /// <summary>
        /// .Ctor
        /// </summary>
        static WXQYHHelper()
        {
            //企业ID 企业微信唯一
            CORPID = ConfigurationManager.AppSettings["CorpID"];
            SECRET = ConfigurationManager.AppSettings["ticketSecret"];
        }

        /// <summary>
        /// ACCESS_TOKEN最后一次更新时间
        /// </summary>
        static DateTime _lastGetTimeOfAccessToken = DateTime.Now.AddSeconds(-7201);

        /// <summary>
        /// 存储微信访问凭证
        /// </summary>
        static string _AccessToken;

        /// <summary>
        /// 获取微信访问凭证
        /// </summary>
        public static string GetAccessToken()
        {
            try
            {
                if (_lastGetTimeOfAccessToken < DateTime.Now)
                {

                    string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", CORPID, SECRET);
                    string responseText = GetBase(url); // 封装的get请求
                    /*
                        API：http://qydev.weixin.qq.com/wiki/index.php?title=%E4%B8%BB%E5%8A%A8%E8%B0%83%E7%94%A8#.E8.8E.B7.E5.8F.96AccessToken
                        正确的Json返回示例:
                        {
                           "access_token": "accesstoken000001",
                           "expires_in": 7200
                        }
                        错误的Json返回示例:
                        {
                           "errcode": 43003,
                           "errmsg": "require https"
                        }
                    */
                    var rsEntity = new { access_token = "", expires_in = 0, errcode = 0, errmsg = "" };
                    dynamic en = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType<object>(responseText, rsEntity); // Newtonsoft.Json提供的匿名类反序列化
                    _lastGetTimeOfAccessToken = DateTime.Now.AddSeconds((double)en.expires_in - 1);
                    _AccessToken = en.access_token;
                }
                return _AccessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Get(string url)
        {
            string baseUrl = ConfigurationManager.AppSettings["baseUrl"];// "http://192.168.103.200:8001/eps/api/interface/";
            var request = (HttpWebRequest)WebRequest.Create(baseUrl + url);
            var response = (HttpWebResponse)request.GetResponse();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public static string GetTicket(string url)
        {
            string baseUrl = ConfigurationManager.AppSettings["ticketUrl"];// "http://192.168.40.44:8080/fzt/api/ticketinterface/";
            var request = (HttpWebRequest)WebRequest.Create(baseUrl + url);
            var response = (HttpWebResponse)request.GetResponse();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public static string GetBase(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public static string PostDate(string url, string sparams)
        {
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ticketUrl"] + url);
            //Post请求方式  
            request.Method = "POST";
            // 内容类型  

            request.ContentType = "application/x-www-form-urlencoded";
            byte[] payload;
            //将URL编码后的字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(sparams);
            //设置请求的 ContentLength   
            request.ContentLength = payload.Length;
            //获得请 求流  
            System.IO.Stream writer = request.GetRequestStream();
            //将请求参数写入流  
            writer.Write(payload, 0, payload.Length);
            // 关闭请求流  
            writer.Close();
            System.Net.HttpWebResponse response;
            // 获得响应流  
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();
            return responseText;
        }
    }
}