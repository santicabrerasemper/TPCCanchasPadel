using ConexionBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;

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

        public void AgregarSucursalCompleta(string nombreSucursal, string nombreLocalidad, string foto)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT ISNULL(MAX(LocalidadID), 0) + 1 FROM Localidades");
                datos.ejecutarLectura();
                datos.Lector.Read();
                int nuevoLocalidadId = (int)datos.Lector[0];
                datos.cerrarConexion();

                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO Localidades (LocalidadID, Nombre) VALUES (@id, @nombre)");
                datos.setearParametro("@id", nuevoLocalidadId);
                datos.setearParametro("@nombre", nombreLocalidad);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                datos = new AccesoDatos();
                datos.setearConsulta("SELECT ISNULL(MAX(SucursalID), 0) + 1 FROM Sucursales");
                datos.ejecutarLectura();
                datos.Lector.Read();
                int nuevoSucursalId = (int)datos.Lector[0];
                datos.cerrarConexion();

                datos = new AccesoDatos();
                datos.setearConsulta(
                    @"INSERT INTO Sucursales (SucursalID, Nombre, LocalidadID, FotoUrl)
              VALUES (@id, @nombre, @loc, @foto)");
                datos.setearParametro("@id", nuevoSucursalId);
                datos.setearParametro("@nombre", nombreSucursal);
                datos.setearParametro("@loc", nuevoLocalidadId);
                datos.setearParametro("@foto", foto);
                datos.ejecutarAccion();
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

