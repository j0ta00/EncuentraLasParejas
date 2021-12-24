using EncuentraLasParejas_DAL.UtilsQuery;
using EncuentraLasParejas_Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EncuentraLasParejas_DAL.Listados
{
    public class ListadoPuntuacion_DAL
    {

        public static List<clsPuntuacion> getListadoCompleto(){
            List<clsPuntuacion> listadoPuntuacion = new List<clsPuntuacion>();
            clsUtilsQuery utilsMyReader = new clsUtilsQuery();
            utilsMyReader.QuerySelect("SELECT * FROM Puntuacion");
            if (utilsMyReader.MyReader.HasRows){
                while (utilsMyReader.MyReader.Read()){
                    listadoPuntuacion.Add(leerUnaPuntuacion(utilsMyReader.MyReader));                
                }
            }
            utilsMyReader.closeConnection();
            return listadoPuntuacion;
        }

        private static clsPuntuacion leerUnaPuntuacion(SqlDataReader reader){
            clsPuntuacion oPuntuacion = new clsPuntuacion();
            oPuntuacion.NombreJugador=(string)reader["nombreJugador"];
            oPuntuacion.Tiempo = (string)reader["tiempoRealizado"].ToString();
            return oPuntuacion;
        }
        public static clsPuntuacion getUnaPuntuacion(string nombreJugador)
        {
            clsPuntuacion oPuntuacion = new clsPuntuacion();
            clsUtilsQuery utilsMyReader = new clsUtilsQuery();
            return utilsMyReader.QuerySelectWithValueString("SELECT * FROM Puntuacion Where nombreJugador=@nombreJugador",nombreJugador);
        }
    }
}
