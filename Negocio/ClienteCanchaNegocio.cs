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

                // Determinar el EstadoID correspondiente a "Activo" (para mapear a bool)
                int activoId = 1; // fallback
                try
                {
                    // intentamos obtenerlo desde la BD (una vez)
                    AccesoDatos d2 = new AccesoDatos();
                    d2.setearConsulta("SELECT EstadoID FROM Estados WHERE Nombre = 'Activo'");
                    d2.ejecutarLectura();
                    if (d2.Lector.Read())
                        activoId = Convert.ToInt32(d2.Lector["EstadoID"]);
                    d2.cerrarConexion();
                }
                catch { /* si falla dejamos activoId = 1 */ }

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

                    // Mapeo lógico: Activa si EstadoID == activoId
                    cancha.Activa = (cancha.EstadoID == activoId);

                    // Calcular precio estimado (podes usar tu regla)
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
                string sql = @"
                EXEC SP_ReservasOK
                    @UsuarioID   = @usuarioId,
                    @CanchaID    = @canchaId,
                    @Fecha       = @fecha,
                    @HoraInicio  = @horaInicio,
                    @HoraFin     = @horaFin,
                    @PromocionID = NULL;";

                datos.setearConsulta(sql);
                datos.setearParametro("@usuarioId", usuarioId);
                datos.setearParametro("@canchaId", canchaId);
                datos.setearParametro("@fecha", fecha);
                datos.setearParametro("@horaInicio", horaInicio);
                datos.setearParametro("@horaFin", horaFin);

                object result = datos.ejecutarScalar();
                return result == null || result == DBNull.Value ? -1 : Convert.ToInt32(result);
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

       
        public decimal CalcularPrecio(TimeSpan horaInicio, TimeSpan horaFin)
        {
            decimal precioHora = 6000m;
            double duracion = (horaFin - horaInicio).TotalHours;
            return precioHora * (decimal)duracion;
        }
    }
}