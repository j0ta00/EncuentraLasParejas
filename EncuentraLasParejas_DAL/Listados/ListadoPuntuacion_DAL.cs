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
        {//no he podido usar execute scalar ya que ambos parametros es decir tanto el nombre como el tiempo me interesan a la hora de construir el objeto, y porque debería usar el objeto y no el tiempo a secas,
         //pues porqué no podría saber si ha habido un resultado o no ,ya aunque pueda parecer imposible técnicamente si eres tan rapido que el contador ni avanza lo cual teniendo en cuenta que tarda un segundo en arrancar no es tan descabellado,
         //podrías tener un tiempo de 0 y entonces fallaría el programa, ya que entraría como una inserción y pues no sería una inserción si no que no se debería hacer nada, quizás no me he explicado pero mirando el código se entiende.
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
        public static clsPuntuacion getUnaPuntuacion(string nombreJugador)
        {
            clsPuntuacion oPuntuacion = null;
            clsUtilsQuery utilsMyReader = new clsUtilsQuery();
            utilsMyReader.QuerySelectWithValueString("SELECT * FROM Puntuacion Where nombreJugador=@nombreJugador", nombreJugador);
            if (utilsMyReader.MyReader.HasRows)
            {
                utilsMyReader.MyReader.Read();
                oPuntuacion = leerUnaPuntuacion(utilsMyReader.MyReader);
            }
            return oPuntuacion;
        }
    }
}
