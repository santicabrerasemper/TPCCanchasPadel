using ConexionBD;
using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class SucursalNegocio
    {
        public List<Sucursal> ListarSucursales()
        {
            List<Sucursal> lista = new List<Sucursal>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT SucursalID, Nombre, FotoUrl FROM Sucursales");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Sucursal suc = new Sucursal
                    {
                        SucursalID = (int)datos.Lector["SucursalID"],
                        Nombre = (string)datos.Lector["Nombre"],
                        FotoUrl = datos.Lector["FotoUrl"] != DBNull.Value
                ? (string)datos.Lector["FotoUrl"]
                : null


                    };
                    lista.Add(suc);
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

