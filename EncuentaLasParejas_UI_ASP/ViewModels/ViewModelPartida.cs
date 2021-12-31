using EncuentaLasParejas_UI_ASP.Models;
using EncuentraLasParejas_UI_ASP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EncuentaLasParejas_UI_ASP.ViewModels
{
    public class ViewModelPartida
    {
        #region propiedades
        private static int puntuacion, intentos;
        public static bool ResultadoComprobado { get; set; }
        public static bool VoltearCartas { get; set; }
        public static int Resultado { get; set; }
        public static int Tiempo { get; set; }
        public static List<Carta> ListaCartas { get; set; }        
        public static int IdCartaPrevia{ get; set; }
        public static int IdCartaActual { get; set; }
        public static int Intentos { get { return intentos; } set {
                intentos = value;
                if (intentos == 0){
                    Resultado = 1;
                }
            } }
        public static int Puntuacion { get { return puntuacion; } set {
                puntuacion = value;
                if (puntuacion > 8){
                    Resultado = 2;
                }
            } }
        public static bool ParejaVolteada { get; set; }
        public List<CartaImagenId> ListaCartasOptimizadas { get; set; }
        #endregion
        #region constructor
        public ViewModelPartida(){
            if (ListaCartas is null)
            {
                llenarListaDeCartas();
                Intentos = 9;
                Puntuacion = 0;
                IdCartaActual = 0;
                Tiempo = 0;
                IdCartaPrevia = 0;
                Resultado = 0;
            }
              
        }
        #endregion
        #region metodos
        /// <summary>
        /// Llena una lista con los objetos carta necesarios para el juego
        /// </summary>
        private void llenarListaDeCartas()
        {
            Carta carta;
            ListaCartas = new List<Carta>();
            string[] nombreImagenesCartas = new string[] { "https://i.ibb.co/02s6VG7/c.png", "https://i.ibb.co/p3j0Nfz/php.png", "https://i.ibb.co/D82kRyv/perl.png",
            "https://i.ibb.co/tz1Fbh5/java-Script.png","https://i.ibb.co/VJVXDq1/java.png","https://i.ibb.co/GMMZZzb/delphi.png","https://i.ibb.co/NxZGCXP/cSharp.png",
            "https://i.ibb.co/m6XpgXz/Cobol.png","https://i.ibb.co/swcf21W/c.png"};
            for (int i = 0; i < 9; i++)
            {
                carta = new Carta(i, false, nombreImagenesCartas[i]);
                ListaCartas.Add(carta);
                ListaCartas.Add(new Carta(carta));
            }
                barajarCartas();
        }
        /// <summary>
        /// Baraja las cartas haciendo que su disposicion sea completamente aleatoria
        /// </summary>
        private void barajarCartas()
        {
            var rand = new Random();
            ListaCartas = ListaCartas.Select(carta => new {Carta = carta, R = rand.Next()})
                  .OrderBy(x => x.R)
                  .Select(x => x.Carta).ToList();
        }
        /// <summary>
        /// Llena la lista de cartas con el modelo de las cartas unicamente con su imagen e id que es lo único que necesita mi vista de las cartas, para que sea lo más liviano posible en la parte del cliente
        /// </summary>
        public void llenarListaDeCartasOptimizadas(){
            ListaCartasOptimizadas = new List<CartaImagenId>();
            ListaCartas.ForEach(carta=>ListaCartasOptimizadas.Add(new CartaImagenId(carta)));
        }
        #endregion
    }
}
