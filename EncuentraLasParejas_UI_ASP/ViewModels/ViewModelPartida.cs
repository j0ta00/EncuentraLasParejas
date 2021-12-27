using EncuentraLasParejas_UI_ASP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EncuentraLasParejas_UI_ASP.ViewModels
{
    public class ViewModelPartida
    {
        public List<Carta> ListaCartas{get;set;}

       public ViewModelPartida(){
            llenarListaDeCartas();       
        }

        public void llenarListaDeCartas()
        {
            Carta carta;
            ListaCartas = new List<Carta>();
            string[] nombreImagenesCartas = Directory.GetFiles("./Assets/ImagenesDeLasCartas/").Where(nombreFichero => nombreFichero != "./Assets/ImagenesDeLasCartas/cartaPorDetras.png").ToArray();
            for (int i = 0; i < 9; i++)
            {
                carta = new Carta(i, false, nombreImagenesCartas[i].Remove(0, 1));
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
    }
}

