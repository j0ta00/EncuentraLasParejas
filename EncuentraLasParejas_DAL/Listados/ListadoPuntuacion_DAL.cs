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
        /// <summary>
        /// Devuelve el listado de objetos puntuacion almacenado en la bbdd
        /// </summary>
        /// <returns>List(clsPuntuacion) </returns>
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
        /// <summary>
        /// Lee un objeto puntuacion que se encontraba almacenado en la bbdd
        /// </summary>
        /// <param name="reader">SqlDataReader reader</param>
        /// <returns>clsPuntuacion oPuntuacion</returns>
        private static clsPuntuacion leerUnaPuntuacion(SqlDataReader reader)
        {
            clsPuntuacion oPuntuacion = null;
            if (reader.HasRows)
            {
                oPuntuacion = new clsPuntuacion();
                oPuntuacion.NombreJugador = (string)reader["nombreJugador"];
                oPuntuacion.Tiempo = reader["tiempoRealizado"].ToString();
            }
            return oPuntuacion;
        }
        /// <summary>
        /// Lee una puntuacion de la bbdd
        /// </summary>
        /// <param name="nombreJugador">string nombreJugador</param>
        /// <returns>clsPuntuacion oPuntuacion</returns>
        public static TimeSpan getUnaPuntuacion(string nombreJugador)
        {
            clsUtilsQuery utilsMyReader = new clsUtilsQuery();
            return utilsMyReader.QuerySelectWithValueString("SELECT tiempoRealizado FROM Puntuacion Where nombreJugador=@nombreJugador", nombreJugador);
        }
    }
}
