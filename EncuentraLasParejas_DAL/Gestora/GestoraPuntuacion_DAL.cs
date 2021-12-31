using EncuentraLasParejas_DAL.UtilsQuery;
using EncuentraLasParejas_Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncuentraLasParejas_DAL.Gestora
{
    public class GestoraPuntuacion_DAL
    {

        /// <summary>
        /// Actualiza o inserta un objeto puntuacion en la bbdd
        /// </summary>
        /// <param name="puntuacion">clsPuntuacion puntuacion</param>
        /// <returns>int columnas afectadas</returns>
        public static int actualizarOInsertarPuntuacion(clsPuntuacion puntuacion){
            string query = "";
            int resultado = 0;
            clsUtilsQuery myUtilsQuery = new clsUtilsQuery();
            TimeSpan tiempoAlmacenado;
            if ((tiempoAlmacenado=Listados.ListadoPuntuacion_DAL.getUnaPuntuacion(puntuacion.NombreJugador)).TotalSeconds==0){
                query = "Insert into Puntuacion(nombreJugador,tiempoRealizado) values (@nombreJugador,@tiempoRealizado)";
                resultado = myUtilsQuery.QueryActualizarOInsertarPuntuacion(query, puntuacion);
            }
            else {
                if (tiempoAlmacenado.TotalSeconds > TimeSpan.Parse(puntuacion.Tiempo).TotalSeconds)//tengo que hacer este parseo porque si no al parsear de string a int me daba un fallo de formato
                {
                    query = "update puntuacion set nombreJugador=@nombreJugador,tiempoRealizado=@tiempoRealizado where nombreJugador=@nombreJugador";
                    resultado = myUtilsQuery.QueryActualizarOInsertarPuntuacion(query, puntuacion);
                }
            }
            return resultado;
        }

    }
}
