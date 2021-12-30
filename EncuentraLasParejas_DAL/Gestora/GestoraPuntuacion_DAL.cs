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
            clsPuntuacion puntuacionYaAlmacenada = new clsPuntuacion();
            if ((puntuacionYaAlmacenada=Listados.ListadoPuntuacion_DAL.getUnaPuntuacion(puntuacion.NombreJugador))is null) {//aquí está lo que comentaba del tema del 0 si el tiempo fuera un int 0 y el tiempo en el que alguien lo completo es 0, entraría en la condición y eso sería erroneo
                query = "Insert into Puntuacion(nombreJugador,tiempoRealizado) values (@nombreJugador,@tiempoRealizado)";
                resultado = myUtilsQuery.QueryActualizarOInsertarPuntuacion(query, puntuacion);
            }
            else {
                if (TimeSpan.Parse(puntuacionYaAlmacenada.Tiempo).Seconds > int.Parse(puntuacion.Tiempo))
                {
                    query = "update puntuacion set nombreJugador=@nombreJugador,tiempoRealizado=@tiempoRealizado";
                    resultado = myUtilsQuery.QueryActualizarOInsertarPuntuacion(query, puntuacion);
                }
            }
            return resultado;
        }

    }
}
