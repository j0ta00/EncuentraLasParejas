using EncuentraLasParejas_DAL.Listados;
using EncuentraLasParejas_Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncuentraLasParejas_BL.Listados
{
    public class ListadoPuntuacion_BL
    {
        /// <summary>
        /// Aplica la lógica de negocio a los datos obtenidos gracias a la capa DAL
        /// </summary>
        /// <returns>List(clsPuntuacion)</returns>
        public static List<clsPuntuacion> getListadoCompletoPuntuacion(){
            return ListadoPuntuacion_DAL.getListadoCompleto();
        }

    }
}
