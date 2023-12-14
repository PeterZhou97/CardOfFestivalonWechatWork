<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpringFestivalCard.aspx.cs" Inherits="BirthdayCard.SpringFestivalCard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<script type="text/javascript" src="/js/jquery.min.js"></script>
     <script type="text/javascript">
         var phoneWidth = parseInt(window.screen.width);
         var phoneScale = phoneWidth / 640;
         var ua = navigator.userAgent;
         if (/Android (\d+\.\d+)/.test(ua)) {
             var version = parseFloat(RegExp.$1);
             if (version > 2.3) {
                 document.write('<meta name="viewport" content="width=640, minimum-scale = ' + phoneScale + ', maximum-scale = ' + phoneScale + ', target-densitydpi=device-dpi">');
             } else {
                 document.write('<meta name="viewport" content="width=640, target-densitydpi=device-dpi">');
             }
         } else {
             document.write('<meta name="viewport" content="width=640, user-scalable=no, target-densitydpi=device-dpi">');
         }
     </script>
	<title>瑞金医院卢湾分院春节贺卡推送</title>
    
    <style type="text/css">
        #app{
            border:2px solid #ff6a00; width:650px;height:1315px;  
            display:table;  
            background-position: center;
            background-size: cover; 
            background-repeat:no-repeat;
            background-image:url("media/SpringFestival/9.fw.png")
        }
        .word{
            display:table-cell; font-family: "楷体","楷体_GB2312";
            vertical-align:middle; font-size:30px; text-align:center; color:#f5eaf0;
        }
        #showtime table{border:2px solid #f5eaf0; text-align:center; font-family: "楷体","楷体_GB2312";}
    </style> 
    <script type="text/javascript">
        //首页自动更换背景特效开始============================================
        window.onload = changeImg;
        var timeInterval = 10000;
        //定义一个存放照片位置的数组，可以放任意个，在这里放3个
        var myPix = new Array("media/SpringFestival/1.fw.png", "media/SpringFestival/2.fw.png",
            "media/SpringFestival/3.fw.png", "media/SpringFestival/4.fw.png",
            "media/SpringFestival/5.fw.png", "media/SpringFestival/6.fw.png",
            "media/SpringFestival/7.fw.png", "media/SpringFestival/8.fw.png",
            "media/SpringFestival/9/fw/png");
        setInterval(changeImg, timeInterval);
        function changeImg() {

            //获得id名为d1的对象
            var obj = document.getElementById("app");
            var randomNum = Math.floor((Math.random() * myPix.length));
            //设置d1的背景图片
            let img = new Image();
            img.src = myPix[randomNum];
            // 确定图片加载完成后再进行背景图片切换
            img.onload = function () {
                obj.style.backgroundImage = "url(" + img.src + ")";
            }
            //显示对应的图片
        }
    </script>
   <script type="text/javascript">
       var obj = new Date();
       var year = obj.getFullYear();
       var month = obj.toDateString().split(" ")[1];
       var date = obj.getDate();
       function time() {
           //获得显示时间的div
           t_div = document.getElementById('showtime');
           //替换div内容
           t_div.innerHTML = "<br><table><tr><td>" + date + "</td></tr><tr><td>" + month + "</td></tr><tr><td>" + year + "</td></tr></table>";
           //等待一秒钟后调用time方法，由于settimeout在time方法内，所以可以无限调用
       }
       setInterval(time, 4000);
   </script>
     <script type="text/javascript">
         var html = document.querySelector('html');
         var rem = html.offsetWidth / 6.4;
         html.style.fontSize = rem + "px";
         var imgnum = 9 //设置要显示的图片数，本例总共是55张。
         function dispimg() //将几乎整个js脚本定义成一个函数dispimg()，为的是可以在<body>与<body>间随意地调用脚本、安排图片的显示位置。
         {
             var caution = false
             function setCookie(name, value, expires, path, domain, secure) {
                 var curCookie = name + "=" + escape(value) +
                     ((expires) ? "; expires=" + expires.toGMTString() : "") +
                     ((path) ? "; path=" + path : "") +
                     ((domain) ? "; domain=" + domain : "") +
                     ((secure) ? "; secure" : "")
                 if (!caution || (name + "=" + escape(value)).length <= 4000)
                     document.cookie = curCookie
                 else if (confirm("Cookie exceeds 4KB and will be cut!"))
                     document.cookie = curCookie
             }

             function getCookie(name) {
                 var prefix = name + "="
                 var cookieStartIndex = document.cookie.indexOf(prefix)
                 if (cookieStartIndex == -1)
                     return null
                 var cookieEndIndex = document.cookie.indexOf(";", cookieStartIndex + prefix.length)
                 if (cookieEndIndex == -1)
                     cookieEndIndex = document.cookie.length
                 return unescape(document.cookie.substring(cookieStartIndex + prefix.length, cookieEndIndex))
             }

             function deleteCookie(name, path, domain) {
                 if (getCookie(name)) {
                     document.cookie = name + "=" +
                         ((path) ? "; path=" + path : "") +
                         ((domain) ? "; domain=" + domain : "") + "; expires=Thu, 01-Jan-70 00:00:01 GMT"
                 }
             }

             function fixDate(date) {
                 var base = new Date(0)
                 var skew = base.getTime()
                 if (skew > 0)
                     date.setTime(date.getTime() - skew)
             }
             var now = new Date()
             fixDate(now)
             now.setTime(now.getTime() + 365 * 24 * 60 * 60 * 1000)
             var visits = getCookie("counter")
             if (!visits)
                 visits = 1
             else {
                 visits = parseInt(visits) + 1
             };
             visits = ((visits > imgnum) ? visits = 1 : visits);
             setCookie("counter", visits, now)
             var quote = "<%=getQuotations()%>";
             document.write("<div id=" + "app" + "><span class=" + "word" + ">" + quote + "<div id=" + "showtime" + " style=" + "font-size:" + "22px" + ";color:#f5eaf0" + "; align=" + "center" + "></div></span></div>");
             deleteCookie("counter", visits, now)
             //上面有document...的这一行在不清楚js语法时，切不可随意打回车键擅自排版，以免破坏js语句的连续性导致脚本出错。
         } //函数dispimg()结束。
     </script> 
</head>
<body onload="time()">
     <script type="text/javascript">
         dispimg();
     </script>
    <script src="js/jquery.min.js"></script>
    <script src="js/main.js"></script>
</body>
</html>

