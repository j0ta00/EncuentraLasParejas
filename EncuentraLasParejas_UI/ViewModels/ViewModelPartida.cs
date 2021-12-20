using EncuentraLasParejas.Models;
using EncuentraLasParejas_UI.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace EncuentraLasParejas_UI.ViewModels
{
    public class ViewModelPartida : clsVMBase
    {
        public ObservableCollection<Carta> ListaCartas { get; set; }
        public DelegateCommand Clicar { get; set; }
        private int numeroCartasVolteadas;//esto es un booleano pero no se me ocurria ningun nombre de booleano logico
        private Carta cartaSeleccionada;
        public Carta CartaSeleccionada { get { return cartaSeleccionada; } set {
                if (numeroCartasVolteadas == 1) {
                    CartaPrevia = cartaSeleccionada;
                }
                cartaSeleccionada = value;
                cartaSeleccionada.Descubierta = true;
                Clicar.Execute(Clicar);
            } }
        private Carta CartaPrevia { get; set; }
        private int puntuacion, intentos;
        public int Puntuacion {
            get { return puntuacion; }
            set {
                if (puntuacion > 5)
                {
                    imprimirResultado(true);
                }
                else {
                    puntuacion = value;
                }
            }
        }
        public int Intentos {
            get { return intentos; }
            set {
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
            Clicar = new DelegateCommand(ClicarEnCarta_Execute);
        }


        private void comprobarCartas() {
            if (CartaSeleccionada.Id == CartaPrevia.Id)
                {
                    Puntuacion++;
                    NotifyPropertyChanged("Puntuacion");
                }
                else {
                    Intentos--;
                    CartaSeleccionada.Descubierta = false;
                    CartaPrevia.Descubierta = false;
                    NotifyPropertyChanged("Intentos");
            }   
        }
        public void llenarListaDeCartas() {
            Carta carta;
            ListaCartas = new ObservableCollection<Carta>();
            string[] nombreImagenesCartas = Directory.GetFiles("./Assets/ImagenesDeLasCartas/");
            for (int i = 0; i < 9; i++) {
                carta = new Carta(i, false, nombreImagenesCartas[i].Remove(0, 1));
                ListaCartas.Add(carta);
                ListaCartas.Add(new Carta(carta));
            }
            barajarCartas();
            NotifyPropertyChanged("ListaCartas");
        }

        private void barajarCartas(){
            var rand = new Random();
            ListaCartas = new ObservableCollection<Carta>(ListaCartas.Select(x => new { X = x, R = rand.Next() })
                 .OrderBy(x => x.R)
                 .Select(x => x.X)
                 .ToList());
        }
        public void ClicarEnCarta_Execute() {
            if (numeroCartasVolteadas == 1)
            {
                comprobarCartas();
                numeroCartasVolteadas = 0;
            }
            else { ++numeroCartasVolteadas; }
        }

        private async void imprimirResultado(bool resultado) {
            ContentDialogResult resultadoContentDialog;
            ContentDialog resultadoDialog = null;
            if (resultado) {
                resultadoDialog = new ContentDialog() {
                    Title = "Victory!",
                    PrimaryButtonText = "Play again!"
            };
            } else {
                resultadoDialog = new ContentDialog()
                {
                    Title = "You Loose :(",
                    Content = "Try again!",
                    CloseButtonText = "Exit",
                    PrimaryButtonText = "Try again!"

                };

            }
            SalirOJugarDeNuevo(await resultadoDialog.ShowAsync());
        }
        private void SalirOJugarDeNuevo(ContentDialogResult resultado){
            if (resultado == ContentDialogResult.Primary)
            {
                volverAJugar();
            }
            else {
                
            }
        }

        private void volverAJugar(){
            this.Puntuacion=0;
            this.Intentos=6;
            ListaCartas.ToList().ForEach(carta=>carta.Descubierta=false);
            NotifyPropertyChanged("ListaCartas");
            NotifyPropertyChanged("Puntuacion");
            NotifyPropertyChanged("Intentos");
        }
    }
}

