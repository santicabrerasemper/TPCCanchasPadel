using ConexionBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Negocio
{
    public class CanchaNegocio
    {
        public List<Cancha> ListarPorSucursal(int idSucursal)
        {
            List<Cancha> lista = new List<Cancha>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT CanchaID, Nombre FROM Canchas WHERE IdSucursal = @idSucursal");
                datos.setearParametro("@idSucursal", idSucursal);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Cancha cancha = new Cancha
                    {
                        CanchaID = (int)datos.Lector["CanchaID"],
                        Nombre = (string)datos.Lector["Nombre"]
                    };
                    lista.Add(cancha);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
