using EncuentraLasParejas_DAL.Gestora;
using EncuentraLasParejas_Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncuentraLasParejas_BL.Gestora
{
    public class GestoraPuntuacion_BL
    {

        public static int actualizarOInsertar(clsPuntuacion puntuacion){
            return GestoraPuntuacion_DAL.actualizarOInsertarPuntuacion(puntuacion);
        }
    }
}
