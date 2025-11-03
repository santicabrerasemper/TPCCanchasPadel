using System;
using System.Text.RegularExpressions;
using Dominio;
using ConexionBD;

namespace Negocio
{
    public class RegistroNegocio
    {
        private static readonly Regex RxUsuario =
            new Regex(@"^(?![._-])(?:[A-Za-z0-9]|[._-](?![._-])){3,18}[A-Za-z0-9]$", RegexOptions.Compiled);

        private static readonly Regex RxNombreApellido =
            new Regex(@"^(?! )(?!.* {2,})(?!.* $)[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:[ '\-][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$", RegexOptions.Compiled);

        private static readonly Regex RxPassword =
            new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*_+\-])[A-Za-z\d!@#$%^&*_+\-]{6,50}$", RegexOptions.Compiled);

        public bool IntentoRegistrar(Usuario nuevo, out string mensaje)
        {
            nuevo = nuevo ?? new Usuario();
            nuevo.NombreUsuario = (nuevo.NombreUsuario ?? string.Empty).Trim();
            nuevo.Nombre = ColapsarEspacios(nuevo.Nombre);
            nuevo.Apellido = ColapsarEspacios(nuevo.Apellido);
            var pass = nuevo.Password ?? string.Empty;

            if (!RxUsuario.IsMatch(nuevo.NombreUsuario) ||
                !RxNombreApellido.IsMatch(nuevo.Nombre ?? "") ||
                !RxNombreApellido.IsMatch(nuevo.Apellido ?? "") ||
                !RxPassword.IsMatch(pass))
            {
                mensaje = "Revisá los datos: usuario/nombre/apellido/contraseña no cumplen los requisitos.";
                return false;
            }

            if (nuevo.NombreUsuario.Length > 50 || (nuevo.Nombre?.Length ?? 0) > 50 || (nuevo.Apellido?.Length ?? 0) > 50)
            {
                mensaje = "Los campos no pueden superar 50 caracteres.";
                return false;
            }

            if (ExisteUsuario(nuevo.NombreUsuario))
            {
                mensaje = "El nombre de usuario ya existe.";
                return false;
            }

            const int RolClienteId = 2;

            var datos = new AccesoDatos();
            try
            {
                string sql = @"
EXEC SP_RegistrarUsuarios 
     @Usuario  = @u,
     @Nombre   = @n,
     @Apellido = @a,
     @Password = @p,
     @RolID    = @r;";

                datos.setearConsulta(sql);
                datos.setearParametro("@u", nuevo.NombreUsuario);
                datos.setearParametro("@n", nuevo.Nombre);
                datos.setearParametro("@a", nuevo.Apellido);
                datos.setearParametro("@p", pass);      
                datos.setearParametro("@r", RolClienteId);

                datos.ejecutarAccion();
                mensaje = "Usuario registrado exitosamente.";
                return true;
            }
            catch (Exception)
            {
                mensaje = "No se pudo registrar el usuario. Intentá nuevamente.";
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool ExisteUsuario(string nombreUsuario)
        {
            nombreUsuario = (nombreUsuario ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(nombreUsuario)) return false;

            var datos = new AccesoDatos();
            try
            {
                const string sql = @"SELECT TOP 1 1 FROM Usuarios WHERE Usuario = @u;";
                datos.setearConsulta(sql);
                datos.setearParametro("@u", nombreUsuario);
                var result = datos.ejecutarScalar();
                return !(result == null || result == DBNull.Value);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private string ColapsarEspacios(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            s = s.Trim();
            while (s.Contains("  ")) s = s.Replace("  ", " ");
            return s;
        }
    }
}
