using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Me_Spotify_App.CookieManager
{
    public interface ICookieManager
    {

        void AddCookie(HttpCookie cookie,HttpResponseBase response);

        HttpCookie GetCookie(string key, HttpRequestBase request);
    }
}
