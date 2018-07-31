//
//文件名：    AuthHeader.aspx.cs
//功能描述：  通过通过SOAP Header身份验证
//创建时间：  2015/10/15
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace ServiceInterface.Common
{
    public class AuthHeader : SoapHeader
    {
        //用户名
        private string userName;
        //密码
        private string passWord;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }     

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        public AuthHeader()
        {
            userName = string.Empty;
            passWord = string.Empty;
        }

        public bool ValideUser(string userName, string passWord)
        {
            if ((userName == "dzsw") && (passWord == "MR5A*#90MDeC/c0IklEXg="))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}