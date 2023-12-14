using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

/// <summary>
///Tools 的摘要说明
/// </summary>
public class Tools :IRequiresSessionState
{
    public Tools()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public static string GetJsonValue(string jsonStr, string key)
    {
        string result = string.Empty;
        if (!string.IsNullOrEmpty(jsonStr))
        {
            key = "\"" + key.Trim('"') + "\"";
            int index = jsonStr.IndexOf(key) + key.Length + 1;
            if (index > key.Length + 1)
            {
                //先截逗号，若是最后一个，截“｝”号，取最小值
                int end = jsonStr.IndexOf(',', index);
                if (end == -1)
                {
                    end = jsonStr.IndexOf('}', index);
                }

                result = jsonStr.Substring(index, end - index);
                result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
            }
        }
        return result;
    }






    /// <summary> 
    /// 替换html中的特殊字符 
    /// </summary> 
    /// <param name="theString">需要进行替换的文本。</param> 
    /// <returns>替换完的文本。</returns> 
    public static string HtmlEncode(string theString)
    {

       // theString = theString.Replace(">", "&gt;");
       // theString = theString.Replace("<", "&lt;");
        theString = theString.Replace("'", "&acute;");
        theString = theString.Replace("insert", "");
        theString = theString.Replace("update", "");
        theString = theString.Replace("select", "");
        theString = theString.Replace("or", "");
        theString = theString.Replace("and", "");
        theString = theString.Replace("%", "");
        theString = theString.Replace("&", "&amp;");
        theString = theString.Replace("=", "");
        theString = theString.Replace("count", "");
        theString = theString.Replace("\n", "");
        return theString;
    }
    /// <summary> 
    /// 恢复html中的特殊字符 
    /// </summary> 
    /// <param name="theString">需要恢复的文本。</param> 
    /// <returns>恢复好的文本。</returns> 
    public static string HtmlDiscode(string theString)
    {
        theString = theString.Replace("&gt;", ">");
        theString = theString.Replace("&lt;", "<");
        theString = theString.Replace("&amp;", "&");
        theString = theString.Replace("&acute;", "'");
        theString = theString.Replace("&nbsp;", " ");
        return theString;
    }
    /// <summary>
    /// 检查服务是否正确
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public static string checkServer(string server)
    {
        string newserver = HtmlEncode(server);
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("1", "1");
        dic.Add("2", "2");
        dic.Add("3", "3");
        dic.Add("4", "4");
        dic.Add("5", "5");
        dic.Add("6", "6");
        dic.Add("7", "7");
        dic.Add("8", "8");
        dic.Add("9", "9");
        dic.Add("10", "10");
        if (dic.ContainsKey(newserver))
        {
            return dic[newserver];
        }
        else
        {
            return "1";
        }
    }
    public static void ismember()
    {
        if (System.Web.HttpContext.Current.Session["member_username"] == null || System.Web.HttpContext.Current.Session["member_username"].ToString() == "")
        {
           System.Web.HttpContext.Current.Response.Redirect("index.aspx");
        }
    }
    /// <summary>
    /// 检查婚宴配套是否正确
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public static string checkSupport(string server)
    {
        string newserver = HtmlEncode(server);
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("1", "1");
        dic.Add("2", "2");
        dic.Add("3", "3");
        dic.Add("4", "4");
        dic.Add("5", "5");
        if (dic.ContainsKey(newserver))
        {
            return dic[newserver];
        }
        else
        {
            return "1";
        }
    }
    public static bool IsValidDate(string strIn)
    {
        System.Text.RegularExpressions.Regex reg1  = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
        bool ok= reg1.IsMatch(strIn);
        return ok;
    }
}