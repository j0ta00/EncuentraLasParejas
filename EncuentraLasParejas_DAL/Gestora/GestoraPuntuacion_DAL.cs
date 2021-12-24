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
            if (Listados.ListadoPuntuacion_DAL.getUnaPuntuacion(puntuacion.NombreJugador) is null) {
                query = "Insert into Puntuacion(nombreJugador,tiempoRealizado) values (@nombreJugador,@tiempoRealizado)";
            }
            else {
                query = "update puntuacion set nombreJugador=@nombreJugador,tiempoRealizado=@tiempoRealizado";
            }
           return new clsUtilsQuery().QueryActualizarOInsertarPuntuacion(query,puntuacion);
        }

    }
}
