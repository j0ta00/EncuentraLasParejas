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
        public List<Carta> ListaCartas { get; set; }
        public static int IdCartaPrevia{ get; set; }
        public static int Intentos { get; set; }
        public List<CartaImagenId> ListaCartasOptimizadas { get; set; }
        public ViewModelPartida(){
            llenarListaDeCartas();
        }

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

        private void barajarCartas()
        {
            var rand = new Random();
            ListaCartas = new List<Carta>(ListaCartas.Select(carta => new { Carta = carta, R = rand.Next() })
                 .OrderBy(x => x.R)
                 .Select(x => x.Carta)
                 .ToList());
        }

        public void llenarListaDeCartasOptimizadas(){
            ListaCartasOptimizadas = new List<CartaImagenId>();
            ListaCartas.ForEach(carta=>ListaCartasOptimizadas.Add(new CartaImagenId(carta)));
        }

    }
}
