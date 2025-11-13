using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using ConexionBD;


namespace Negocio
{
    public class ClienteCanchaNegocio
    {

        public List<Cancha> ListarCanchasDisponibles(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int sucursalId)
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
                c.EstadoID,
                s.Nombre AS NombreSucursal,
                l.Nombre AS NombreLocalidad
            FROM Canchas c
            INNER JOIN Sucursales s ON c.SucursalID = s.SucursalID
            INNER JOIN Localidades l ON s.LocalidadID = l.LocalidadID
            WHERE c.SucursalID = @SucursalID
              AND c.CanchaID NOT IN (
                  SELECT r.CanchaID 
                  FROM Reservas r
                  WHERE r.Fecha = @Fecha
                    AND (
                        (@HoraInicio < r.HoraFin AND @HoraFin > r.HoraInicio)
                    )
              )
            ORDER BY c.Nombre";

                datos.setearConsulta(consulta);
                datos.setearParametro("@Fecha", fecha);
                datos.setearParametro("@HoraInicio", horaInicio);
                datos.setearParametro("@HoraFin", horaFin);
                datos.setearParametro("@SucursalID", sucursalId);
                datos.ejecutarLectura();

                int activoId = 1; 
                try
                {

                    AccesoDatos d2 = new AccesoDatos();
                    d2.setearConsulta("SELECT EstadoID FROM Estados WHERE Nombre = 'Activo'");
                    d2.ejecutarLectura();
                    if (d2.Lector.Read())
                        activoId = Convert.ToInt32(d2.Lector["EstadoID"]);
                    d2.cerrarConexion();
                }
                catch { }

                while (datos.Lector.Read())
                {
                    var cancha = new Cancha
                    {
                        CanchaID = datos.Lector["CanchaID"] != DBNull.Value ? Convert.ToInt32(datos.Lector["CanchaID"]) : 0,
                        Nombre = datos.Lector["Nombre"] != DBNull.Value ? datos.Lector["Nombre"].ToString() : string.Empty,
                        SucursalID = datos.Lector["SucursalID"] != DBNull.Value ? Convert.ToInt32(datos.Lector["SucursalID"]) : 0,
                        EstadoID = datos.Lector["EstadoID"] != DBNull.Value ? Convert.ToInt32(datos.Lector["EstadoID"]) : 0,
                        NombreSucursal = datos.Lector["NombreSucursal"] != DBNull.Value ? datos.Lector["NombreSucursal"].ToString() : string.Empty,
                        NombreLocalidad = datos.Lector["NombreLocalidad"] != DBNull.Value ? datos.Lector["NombreLocalidad"].ToString() : string.Empty,
                    };

                    cancha.Activa = (cancha.EstadoID == activoId);

                    decimal precioHora = 6000m;
                    double duracion = (horaFin - horaInicio).TotalHours;
                    cancha.TotalEstimado = precioHora * (decimal)duracion;

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




        public int ReservarCancha(int usuarioId, int canchaId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = @"
        INSERT INTO Reservas (UsuarioID, CanchaID, Fecha, HoraInicio, HoraFin, PromocionID)
        VALUES (@UsuarioID, @CanchaID, @Fecha, @HoraInicio, @HoraFin, @PromocionID);
        SELECT SCOPE_IDENTITY();";

                datos.setearConsulta(consulta);
                datos.setearParametro("@UsuarioID", usuarioId);
                datos.setearParametro("@CanchaID", canchaId);
                datos.setearParametro("@Fecha", fecha);
                datos.setearParametro("@HoraInicio", horaInicio);
                datos.setearParametro("@HoraFin", horaFin);
                datos.setearParametro("@PromocionID", DBNull.Value);

                object result = datos.ejecutarScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la reserva: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public string ValidarReservaCompleta(int canchaId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            
            string general = ValidarBusquedaGeneral(fecha, horaInicio, horaFin);
            if (general != null)
                return general;

            
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                SELECT TOP 1 HoraInicio, HoraFin 
                FROM Reservas 
                WHERE CanchaID = @CanchaID 
                  AND Fecha = @Fecha
                  AND (HoraInicio < @HoraFin AND HoraFin > @HoraInicio)
                ORDER BY HoraInicio;");

                datos.setearParametro("@CanchaID", canchaId);
                datos.setearParametro("@Fecha", fecha);
                datos.setearParametro("@HoraInicio", horaInicio);
                datos.setearParametro("@HoraFin", horaFin);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    TimeSpan horaOcupada = (TimeSpan)datos.Lector["HoraInicio"];
                    return $"⚠️ La cancha ya tiene una reserva desde las {horaOcupada:hh\\:mm}. Solo podés reservar antes de ese horario.";
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return null;
        }

        public string ValidarBusquedaGeneral(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            if (fecha.Year != 2025)
                return "Solo se permiten reservas dentro del año 2025.";

            if (fecha < DateTime.Today)
                return "No se pueden realizar reservas en fechas anteriores a hoy.";

            if (horaInicio < new TimeSpan(7, 0, 0) || horaFin > new TimeSpan(23, 59, 0))
                return "El horario debe estar entre las 07:00 y las 23:59.";

            if (horaInicio >= horaFin)
                return "La hora de inicio debe ser menor que la hora de finalización.";

            var duracion = horaFin - horaInicio;
            if (duracion.TotalMinutes < 30)
                return "La duración mínima de la reserva es de 30 minutos.";

          
            if (duracion.TotalHours > 4)
                return "La reserva no puede superar las 4 horas de duración.";

            return null;
        }




        public decimal CalcularPrecio(TimeSpan horaInicio, TimeSpan horaFin)
        {
            decimal precioHora = 6000m;
            double duracion = (horaFin - horaInicio).TotalHours;
            return precioHora * (decimal)duracion;
        }
    }
}