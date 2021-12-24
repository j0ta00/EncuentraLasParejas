using EncuentraLasParejas_DAL.Listados;
using EncuentraLasParejas_Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncuentraLasParejas_BL.Listados
{
    public class ListadoPuntuacion_BL
    {
        public static List<clsPuntuacion> getListadoCompletoPuntuacion(){
            return ListadoPuntuacion_DAL.getListadoCompleto();
        }

    }
}
