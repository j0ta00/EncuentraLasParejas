using System;
using System.Collections.Generic;
using System.Text;

namespace EncuentraLasParejas_Entities
{
    public class clsPuntuacion
    {
        public string NombreJugador{get; set;}
        public string Tiempo{ get; set; }

        public clsPuntuacion(string nombreJugador, string tiempo)
        {
            this.NombreJugador = nombreJugador;
            Tiempo = tiempo;
        }

        public clsPuntuacion()
        {
            this.NombreJugador = "Guess";
            Tiempo = "0";
        }
    }
}
