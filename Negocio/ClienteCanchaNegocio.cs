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
                SELECT c.CanchaID, c.Nombre, c.SucursalID, c.EstadoID, s.Nombre AS NombreSucursal
                FROM Canchas c
                INNER JOIN Sucursales s ON c.SucursalID = s.SucursalID
                WHERE (@SucursalID = 0 OR c.SucursalID = @SucursalID)
                AND c.CanchaID NOT IN (
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
                datos.setearParametro("@SucursalID", sucursalId);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    var cancha = new Cancha
                    {
                        CanchaID = (int)datos.Lector["CanchaID"],
                        Nombre = (string)datos.Lector["Nombre"],
                        SucursalID = (int)datos.Lector["SucursalID"],
                        EstadoID = (int)datos.Lector["EstadoID"],
                        NombreSucursal = (string)datos.Lector["NombreSucursal"]
                    };

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