using System;
using System.Collections.Generic;
using Dominio;
using ConexionBD;

namespace Negocio
{
    public class ReservasNegocio
    {      
        public List<Reserva> ObtenerTodasLasReservas()
        {
            var reservas = new List<Reserva>();
            var datos = new AccesoDatos();

            try
            {
                string sql = @"
SELECT
    r.ReservaID,                 -- 0
    r.UsuarioID,                 -- 1
    r.CanchaID,                  -- 2
    r.Fecha,                     -- 3 (DATE)
    r.HoraInicio,                -- 4 (TIME)
    r.HoraFin,                   -- 5 (TIME)
    r.PromocionID,               -- 6 (nullable)
    u.Nombre      AS UsuarioNombre,   -- 7
    u.Apellido    AS UsuarioApellido, -- 8
    c.Nombre      AS CanchaNombre,    -- 9
    e.Nombre      AS EstadoCancha,    -- 10
    p.Descripcion AS PromoDescripcion,-- 11 (nullable)
    p.Descuento   AS PromoDescuento   -- 12 (nullable, TINYINT)
FROM Reservas r
JOIN Usuarios u  ON u.UsuarioID = r.UsuarioID
JOIN Canchas  c  ON c.CanchaID  = r.CanchaID
JOIN Estados  e  ON e.EstadoID  = c.EstadoID
LEFT JOIN Promociones p ON p.PromocionID = r.PromocionID
ORDER BY r.Fecha, r.HoraInicio;";

                datos.setearConsulta(sql);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    var reserva = new Reserva
                    {
                        IdReserva = datos.Lector.GetInt32(0),
                        Usuario = new Usuario
                        {
                            UsuarioID = datos.Lector.GetInt32(1),
                            Nombre = datos.Lector.GetString(7),
                            Apellido = datos.Lector.GetString(8)
                        },
                        Cancha = new Cancha
                        {
                            CanchaID = datos.Lector.GetInt32(2),
                            Nombre = datos.Lector.GetString(9),
                            EstadoID = 0 
                        },
                        FechaReserva = datos.Lector.GetDateTime(3),
                        HoraInicio = datos.Lector.GetTimeSpan(4),
                        HoraFin = datos.Lector.GetTimeSpan(5),
                        Promocion = datos.Lector.IsDBNull(6) ? null : new Promocion
                        {
                            PromocionID = datos.Lector.GetInt32(6),
                            Descripcion = datos.Lector.IsDBNull(11) ? null : datos.Lector.GetString(11),
                            Descuento = datos.Lector.IsDBNull(12) ? (byte)0 : datos.Lector.GetByte(12)
                        }
                    };

                    reservas.Add(reserva);
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return reservas;
        }

        public Reserva BuscarReservaPorID(int idReserva)
        {
            Reserva reserva = null;
            var datos = new AccesoDatos();

            try
            {
                string sql = @"
SELECT
    r.ReservaID,
    r.UsuarioID,
    r.CanchaID,
    r.Fecha,
    r.HoraInicio,
    r.HoraFin,
    r.PromocionID,
    u.Nombre      AS UsuarioNombre,
    u.Apellido    AS UsuarioApellido,
    c.Nombre      AS CanchaNombre,
    e.Nombre      AS EstadoCancha,
    p.Descripcion AS PromoDescripcion,
    p.Descuento   AS PromoDescuento
FROM Reservas r
JOIN Usuarios u  ON u.UsuarioID = r.UsuarioID
JOIN Canchas  c  ON c.CanchaID  = r.CanchaID
JOIN Estados  e  ON e.EstadoID  = c.EstadoID
LEFT JOIN Promociones p ON p.PromocionID = r.PromocionID
WHERE r.ReservaID = @id;";

                datos.setearConsulta(sql);
                datos.setearParametro("@id", idReserva);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    reserva = new Reserva
                    {
                        IdReserva = datos.Lector.GetInt32(0),
                        Usuario = new Usuario
                        {
                            UsuarioID = datos.Lector.GetInt32(1),
                            Nombre = datos.Lector.GetString(7),
                            Apellido = datos.Lector.GetString(8)
                        },
                        Cancha = new Cancha
                        {
                            CanchaID = datos.Lector.GetInt32(2),
                            Nombre = datos.Lector.GetString(9),
                            EstadoID = 0
                        },
                        FechaReserva = datos.Lector.GetDateTime(3),
                        HoraInicio = datos.Lector.GetTimeSpan(4),
                        HoraFin = datos.Lector.GetTimeSpan(5),
                        Promocion = datos.Lector.IsDBNull(6) ? null : new Promocion
                        {
                            PromocionID = datos.Lector.GetInt32(6),
                            Descripcion = datos.Lector.IsDBNull(11) ? null : datos.Lector.GetString(11),
                            Descuento = datos.Lector.IsDBNull(12) ? (byte)0 : datos.Lector.GetByte(12)
                        }
                    };
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return reserva;
        }

        public int AgregarReserva(Reserva reserva)
        {
            var datos = new AccesoDatos();
            try
            {

                string sql = @"
EXEC SP_ReservasOK
    @UsuarioID   = @usuarioId,
    @CanchaID    = @canchaId,
    @Fecha       = @fecha,
    @HoraInicio  = @horaInicio,
    @HoraFin     = @horaFin,
    @PromocionID = @promoId;
";             

                datos.setearConsulta(sql);
                datos.setearParametro("@usuarioId", reserva.Usuario.UsuarioID);
                datos.setearParametro("@canchaId", reserva.Cancha.CanchaID);
                datos.setearParametro("@fecha", reserva.FechaReserva.Date);
                datos.setearParametro("@horaInicio", reserva.HoraInicio);
                datos.setearParametro("@horaFin", reserva.HoraFin);
                datos.setearParametro("@promoId", (object)reserva.Promocion?.PromocionID ?? DBNull.Value);

                object result = datos.ejecutarScalar();           
                return result == null || result == DBNull.Value ? -1 : Convert.ToInt32(result);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void EliminarReserva(int idReserva)
        {
            var datos = new AccesoDatos();
            try
            {
                string sql = "DELETE FROM Reservas WHERE ReservaID = @id;";
                datos.setearConsulta(sql);
                datos.setearParametro("@id", idReserva);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Cancha> ListarCanchasDisponibles(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            List<Cancha> canchas = new List<Cancha>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = @"
    SELECT 
        c.CanchaID, 
        c.Nombre, 
        c.SucursalID, 
        s.Nombre AS NombreSucursal, 
        c.EstadoID
    FROM Canchas c
    INNER JOIN Sucursales s ON c.SucursalID = s.SucursalID
    WHERE c.CanchaID NOT IN (
        SELECT r.CanchaID
        FROM Reservas r
        WHERE r.Fecha = @Fecha
        AND r.HoraInicio < @HoraFin
        AND r.HoraFin > @HoraInicio
    )
    ORDER BY s.Nombre, c.Nombre;";

                datos.setearConsulta(consulta);
                datos.setearParametro("@Fecha", fecha);
                datos.setearParametro("@HoraInicio", horaInicio);
                datos.setearParametro("@HoraFin", horaFin);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Cancha cancha = new Cancha
                    {
                        CanchaID = Convert.ToInt32(datos.Lector["CanchaID"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        SucursalID = Convert.ToInt32(datos.Lector["SucursalID"]),
                        EstadoID = Convert.ToInt32(datos.Lector["EstadoID"]),
                        NombreSucursal = datos.Lector["NombreSucursal"].ToString()
                    };

                    cancha.PrecioHora = 6000;
               
                    double duracionHoras = (horaFin - horaInicio).TotalHours;
                    cancha.PrecioHora *= (decimal)duracionHoras;

                    canchas.Add(cancha);
                }

                return canchas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar canchas disponibles: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Reserva> ListarPorUsuario(int idUsuario)
        {
            List<Reserva> lista = new List<Reserva>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
        SELECT
            r.ReservaID,
            r.UsuarioID,
            r.CanchaID,
            r.Fecha,
            r.HoraInicio,
            r.HoraFin,
            c.Nombre AS CanchaNombre,
            e.Nombre AS EstadoNombre,
            s.Nombre AS SucursalNombre
        FROM Reservas r
        INNER JOIN Canchas c   ON r.CanchaID = c.CanchaID
        INNER JOIN Estados e   ON c.EstadoID = e.EstadoID
        INNER JOIN Sucursales s ON c.SucursalID = s.SucursalID
        WHERE r.UsuarioID = @UsuarioID
        ORDER BY r.Fecha DESC, r.HoraInicio;");

                datos.setearParametro("@UsuarioID", idUsuario);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    try
                    {
                        Reserva reserva = new Reserva();

                        reserva.IdReserva = datos.Lector["ReservaID"] != DBNull.Value
                            ? Convert.ToInt32(datos.Lector["ReservaID"])
                            : 0;

                        reserva.FechaReserva = datos.Lector["Fecha"] != DBNull.Value
                            ? Convert.ToDateTime(datos.Lector["Fecha"])
                            : DateTime.MinValue;

                        // HoraInicio / HoraFin
                        reserva.HoraInicio = datos.Lector["HoraInicio"] != DBNull.Value
                            ? (datos.Lector["HoraInicio"] is TimeSpan t1 ? t1 : ((DateTime)datos.Lector["HoraInicio"]).TimeOfDay)
                            : TimeSpan.Zero;

                        reserva.HoraFin = datos.Lector["HoraFin"] != DBNull.Value
                            ? (datos.Lector["HoraFin"] is TimeSpan t2 ? t2 : ((DateTime)datos.Lector["HoraFin"]).TimeOfDay)
                            : TimeSpan.Zero;

                        reserva.Cancha = new Cancha
                        {
                            Nombre = datos.Lector["CanchaNombre"]?.ToString() ?? ""
                        };
                        reserva.Estado = new Estado
                        {
                            Nombre = datos.Lector["EstadoNombre"]?.ToString() ?? ""
                        };
                        reserva.Sucursal = new Sucursal
                        {
                            Nombre = datos.Lector["SucursalNombre"]?.ToString() ?? ""
                        };

                        lista.Add(reserva);
                    }
                    catch (Exception exFila)
                    {
                        throw new Exception("Error en fila: " +
                            string.Join(" | ",
                                "ReservaID=" + datos.Lector["ReservaID"],
                                "Fecha=" + datos.Lector["Fecha"],
                                "HoraInicio=" + datos.Lector["HoraInicio"],
                                "HoraFin=" + datos.Lector["HoraFin"]
                            ) + " -> " + exFila.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar reservas del usuario: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;


        }




    }

}
