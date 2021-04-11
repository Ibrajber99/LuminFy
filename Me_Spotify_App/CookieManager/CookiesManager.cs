using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.CookieManager
{
    public class CookiesManager : ICookieManager
    {
        public void AddCookie(HttpCookie cookie, HttpResponseBase response)
        {
            response.Cookies.Add(cookie);
        }

        public HttpCookie GetCookie(string key, HttpRequestBase request)
        {
            var cookieRes = request.Cookies.Get(key);
            return cookieRes;
        }
    }
}