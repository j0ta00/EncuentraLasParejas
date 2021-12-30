using EncuentraLasParejas_DAL.UtilsQuery;
using EncuentraLasParejas_Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncuentraLasParejas_DAL.Gestora
{
    public class GestoraPuntuacion_DAL
    {
        public static int actualizarOInsertarPuntuacion(clsPuntuacion puntuacion){
            string query = "";
            int resultado = 0;
            clsUtilsQuery myUtilsQuery = new clsUtilsQuery();
            clsPuntuacion puntuacionYaAlmacenada = new clsPuntuacion();
            if ((puntuacionYaAlmacenada=Listados.ListadoPuntuacion_DAL.getUnaPuntuacion(puntuacion.NombreJugador))is null) {
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
