using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Helpers
{
    public class HttpUtils
    {
        public static string Site(HttpRequest Request)
        {
            return (Request.IsHttps ? "https://" : "http://") + Request.Host.Value;
        }

        public static string Front(HttpRequest Request)
        {
            return "";
        }

        public static string Api(HttpRequest Request)
        {
            return (Request.IsHttps ? "https://" : "http://") + Request.Host.Value;
        }

        public static string GetApi(HttpRequest Request)
        {
            return string.Format("{0}://{1}", (Request.IsHttps ? "https" : "http"), Request.Host.Value);
        }

        public static string GetToken(HttpRequest Request)
        {
            string token = Request.Headers["token"].ToString();
            string Authorization = Request.Headers["Authorization"].ToString();

            Authorization = Authorization.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            if (!string.IsNullOrEmpty(Authorization))
            {
                return Authorization;
            }

            return "";
        }

        public static string GetAuthorization(HttpRequest Request)
        {
            try
            {
                if (Request.Headers == null)
                {
                    return string.Empty;
                }
                else
                {
                    return GetToken(Request);
                }
            }
            catch
            {
                return "";
            }
        }

        //public static Repository.Model.JwtUser GetUser(HttpRequest Request)
        //{
          //  return Domain.Helper.TokenHelper.GetUser(GetAuthorization(Request));
        //}

    }
}
