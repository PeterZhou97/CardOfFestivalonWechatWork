using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BirthdayCard
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;

    //引入的命名空间
    using System.IO;

    /// <summary>
    /// 判断IP归属地类
    /// </summary>
    public class IpSearch
    {
        private static object lockHelper = new object();

        static PHCZIP pcz = new PHCZIP();

        static string filePath = "";

        static bool fileIsExsit = true;

        static IpSearch()
        {
            filePath = HttpContext.Current.Server.MapPath("~/ipdata.config");
            pcz.SetDbFilePath(filePath);
        }

        /// <summary>
        /// 返回IP查找结果
        /// </summary>
        /// <param name="IPValue">要查找的IP地址</param>
        /// <returns></returns>
        public static string GetAddressWithIP(string IPValue)
        {
            lock (lockHelper)
            {
                string result = pcz.GetAddressWithIP(IPValue.Trim());

                if (fileIsExsit)
                {
                    if (result.IndexOf("IANA") >= 0)
                    {
                        return "";
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

    }
}