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
            if (!SesionActiva(page.Session))
            {
                page.Response.Redirect(page.ResolveUrl("~/Login.aspx"), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        public static void RequerirAdmin(Page page)
        {
            if (!SesionActiva(page.Session) || !EsAdmin(page.Session))
            {
                page.Response.Redirect(page.ResolveUrl("~/Home.aspx"), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        public static void CerrarSesion(Page page)
        {
            page.Session.Clear();
            page.Session.Abandon();
        }
    }
}
