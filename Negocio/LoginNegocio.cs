using System;
using System.Text.RegularExpressions;
using Dominio;
using ConexionBD;

namespace Negocio
{
    public class LoginNegocio
    {
        private static readonly Regex RxUser =
            new Regex(@"^(?![._-])(?:[A-Za-z0-9]|[._-](?![._-])){3,18}[A-Za-z0-9]$",
                      RegexOptions.Compiled);

        public Usuario Autenticar(string usuarioInput, string passwordInput, out string mensaje)
        {
            const string MSG = "Por favor, ingresá datos correctos.";
            mensaje = MSG;

            var usuario = (usuarioInput ?? string.Empty).Trim();
            var pass = passwordInput ?? string.Empty;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrEmpty(pass) || !RxUser.IsMatch(usuario))
                return null;

            var datos = new AccesoDatos();
            try
            {
                string sql = @"
SELECT UsuarioID, Usuario, Nombre, Apellido, Password, RolID
FROM Usuarios
WHERE Usuario = @u;";

                datos.setearConsulta(sql);
                datos.setearParametro("@u", usuario);
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                    return null;

                int usuarioId = datos.Lector.GetInt32(0);
                string userDb = datos.Lector.GetString(1);
                string nombre = datos.Lector.GetString(2);
                string apellido = datos.Lector.GetString(3);
                string passDb = datos.Lector.GetString(4);
                int rolId = datos.Lector.GetInt32(5);

                if (!ComparacionSegura(pass, passDb))
                    return null;

                return new Usuario
                {
                    UsuarioID = usuarioId,
                    NombreUsuario = userDb,    
                    Nombre = nombre,
                    Apellido = apellido,
                    RolID = rolId
                };
            }
            catch
            {     
                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private bool ComparacionSegura(string a, string b)
        {
            if (a == null || b == null) return false;
            int diff = a.Length ^ b.Length;
            int n = Math.Min(a.Length, b.Length);
            for (int i = 0; i < n; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
