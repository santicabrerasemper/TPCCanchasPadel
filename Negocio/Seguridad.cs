using System;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace Negocio
{
    public static class Seguridad
    {
        public const int RolAdmin = 1;
        public const int RolCliente = 2;

        public static bool SesionActiva(HttpSessionState session)
            => session != null && session["UsuarioID"] != null;

        public static bool EsAdmin(HttpSessionState session)
        {
            var obj = session?["RolID"];
            return obj != null && Convert.ToInt32(obj) == RolAdmin;
        }

        public static void RequerirSesion(Page page)
        {
            if (page == null || page.Session == null || page.Session["UsuarioID"] == null)
            {
                var url = (page != null)
                    ? page.ResolveUrl("~/Login.aspx?exp=1")
                    : "~/Login.aspx?exp=1";
                HttpContext.Current.Response.Redirect(url, endResponse: false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }


        public static void RequerirAdmin(Page page)
        {
            if (!SesionActiva(page.Session) || !EsAdmin(page.Session))
            {
                page.Response.Redirect(page.ResolveUrl("~/Login.aspx"), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        public static void CerrarSesion(Page page)
        {
            page.Session.Clear();
            page.Session.Abandon();
            var cookie = page.Request.Cookies["ASP.NET_SessionId"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.UtcNow.AddDays(-1);
                page.Response.Cookies.Add(cookie);
            }
        }


        public static void NoCache(Page page)
        {
            var cache = page.Response.Cache;
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();
            cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetValidUntilExpires(false);
            cache.SetAllowResponseInBrowserHistory(false);
        }

    }
}
