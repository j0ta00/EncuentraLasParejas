using EncuentraLasParejas.Models;
using EncuentraLasParejas_UI.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncuentraLasParejas_UI.ViewModels
{
    public class ViewModelPartida:clsVMBase
    {
        public ObservableCollection<Carta> ListaCartas { get; set; }
        private int numeroCartasVolteadas;//esto es un booleano pero no se me ocurria ningun nombre de booleano logico
        private Carta cartaSeleccionada;
        public Carta CartaSeleccionada { get { return cartaSeleccionada; } set{
                if (numeroCartasVolteadas == 1){
                    CartaPrevia = cartaSeleccionada;
                }
            } }
        private Carta CartaPrevia { get; set; }
        private int puntuacion, intentos;
        public int Puntuacion{
              get{return puntuacion;}
            set{
                if (puntuacion > 5)
                {
                    imprimirResultado(true);
                }
                else {
                    puntuacion = value;
                }
            }
            }
        public int Intentos{
            get{return intentos;}
            set{
                if (intentos == 0)
                {
                    imprimirResultado(false);
                }
                else {
                    intentos = value;
                }
            }
            }
        public ViewModelPartida() {
            puntuacion = 0;
            intentos = 6;
            numeroCartasVolteadas = 0;
            llenarListaDeCartas();
        }


        private void voltearCarta(){
            if (numeroCartasVolteadas == 1){
                if ((CartaSeleccionada.Id + 1) == CartaPrevia.Id)
                {
                    Puntuacion++;
                }
                else {
                    Intentos--;
                    CartaSeleccionada.Descubierta = false;
                    CartaPrevia.Descubierta = false;
                    NotifyPropertyChanged("CartaSeleccionada");
                    NotifyPropertyChanged("CartaPrevia");
                }
            } else{
                CartaSeleccionada.Descubierta = true;
                NotifyPropertyChanged("CartaSeleccionada");
            }
        }
        public void llenarListaDeCartas(){
            Carta carta;
            ListaCartas = new ObservableCollection<Carta>();
            string[] nombreImagenesCartas = Directory.GetFiles("./Assets/ImagenesDeLasCartas/");
            for(int i=0;i<9;i++){
                carta = new Carta(i,false, nombreImagenesCartas[i].Remove(0, 1));
                ListaCartas.Add(carta);
                ListaCartas.Add(new Carta(carta));
            }
            NotifyPropertyChanged("ListaCartas");
        }

        public void ClicarEnCarta_Execute(){
            if (numeroCartasVolteadas == 1)
            {
                numeroCartasVolteadas = 0;
                voltearCarta();
                
            }
            else {numeroCartasVolteadas++;}
        }

        private void imprimirResultado(bool resultado){
            if (resultado) { } else { }
        }
    }
}

