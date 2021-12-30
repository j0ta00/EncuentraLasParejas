using EncuentraLasParejas_DAL.Gestora;
using EncuentraLasParejas_Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncuentraLasParejas_BL.Gestora
{
    public class GestoraPuntuacion_BL
    {
        /// <summary>
        /// Aplica las lógicas de negocio a los datos que vamos a insertar(gracias a la Capa DAL)
        /// </summary>
        /// <param name="puntuacion">clsPuntuacion puntuacion</param>
        /// <returns>int filasAfectadas</returns>
        public static int actualizarOInsertar(clsPuntuacion puntuacion){
            return GestoraPuntuacion_DAL.actualizarOInsertarPuntuacion(puntuacion);
        }
    }
}
