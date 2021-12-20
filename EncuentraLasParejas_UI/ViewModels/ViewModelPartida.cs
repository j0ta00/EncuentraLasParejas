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
using System.Timers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EncuentraLasParejas_UI.ViewModels
{
    public class ViewModelPartida : clsVMBase
    {
        private int tiempo;
        private DispatcherTimer Timer = new DispatcherTimer();
        public ObservableCollection<Carta> ListaCartas { get; set; }
        public DelegateCommand Clicar { get; set; }
        private int numeroCartasVolteadas;//esto es un booleano pero no se me ocurria ningun nombre de booleano logico
        private Carta cartaSeleccionada;
        public string Tiempo { get; set; }
    public Carta CartaSeleccionada { get { return cartaSeleccionada; } set {
                if (numeroCartasVolteadas == 1) {
                    CartaPrevia = cartaSeleccionada;
                }                
                cartaSeleccionada = value;
                if (!(cartaSeleccionada is null))
                {
                    Clicar.Execute(Clicar);
                }
            } }
        private Carta CartaPrevia { get; set; }
        private int puntuacion, intentos;
        public int Puntuacion {
            get { return puntuacion; }
            set {
                if (puntuacion > 7)
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
                if (intentos == 1)
                {
                    imprimirResultado(false);
                }
                    intentos = value;
            }
        }
        public ViewModelPartida() {
            puntuacion = 0;
            intentos = 6;
            numeroCartasVolteadas = 0;
            llenarListaDeCartas();
            Tiempo = "0";
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += (a,b)=> {
                tiempo++;
                TimeSpan tiempoConvertido = TimeSpan.FromSeconds(tiempo);
                Tiempo = tiempoConvertido.ToString("hh':'mm':'ss");
                NotifyPropertyChanged("Tiempo");
            };
            Timer.Start();
            Clicar = new DelegateCommand(clicarEnCarta_Execute);           
        }       
        private void comprobarCartas() {
            if (CartaSeleccionada.Id == CartaPrevia.Id)
                {               
                    Puntuacion++;
                    NotifyPropertyChanged("Puntuacion");
                }
                else {
                    Intentos--;
                    esperarYDarLaVuelta();
                    NotifyPropertyChanged("Intentos");
            }   
        }
        private async void esperarYDarLaVuelta(){
            await Task.Delay(500);
            CartaSeleccionada.Descubierta = false;
            CartaPrevia.Descubierta = false;
        } 

        public void llenarListaDeCartas() {
            Carta carta;
            ListaCartas = new ObservableCollection<Carta>();
            string[] nombreImagenesCartas = Directory.GetFiles("./Assets/ImagenesDeLasCartas/").Where(nombreFichero=> nombreFichero != "./Assets/ImagenesDeLasCartas/cartaPorDetras.png").ToArray();
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
            ListaCartas = new ObservableCollection<Carta>(ListaCartas.Select(carta => new { Carta = carta, R = rand.Next() })
                 .OrderBy(x => x.R)
                 .Select(x => x.Carta)
                 .ToList());
        }
        private void clicarEnCarta_Execute() {
            if (!cartaSeleccionada.Descubierta)
            {
                cartaSeleccionada.Descubierta = true;
                if (numeroCartasVolteadas == 1)
                {
                    comprobarCartas();
                    numeroCartasVolteadas = 0;
                }
                else { ++numeroCartasVolteadas; }
            }
        }
        //private bool clicarEnCarta_CanExecute(){
        //    return !(CartaSeleccionada is null);       
        //}
        private async void imprimirResultado(bool resultado) {
            ContentDialog resultadoDialog;
            if (resultado) {
                resultadoDialog = new ContentDialog() {
                    Title = "Victory! :D",
                    CloseButtonText = "Exit",
                    PrimaryButtonText = "Play again!"
            };
            } else {
                resultadoDialog = new ContentDialog()
                {
                    Title = "You Lose :(",
                    Content = "Try again!",
                    CloseButtonText = "Exit",
                    PrimaryButtonText = "Try again!"
                };
            }
            SalirOJugarDeNuevo(await resultadoDialog.ShowAsync());
        }
        private void SalirOJugarDeNuevo(ContentDialogResult resultado){
            Frame rootFrame = null;
            if (resultado == ContentDialogResult.Primary)
            {
                volverAJugar();
            }
            else {
                rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
            }
        }

        private void volverAJugar(){
            this.Puntuacion=0;
            this.Intentos=6;
            numeroCartasVolteadas = 0;
            ListaCartas.ToList().ForEach(carta=>carta.Descubierta=false);
            tiempo=0;
            barajarCartas();
            NotifyPropertyChanged("Tiempo");
            NotifyPropertyChanged("ListaCartas");
            NotifyPropertyChanged("Puntuacion");
            NotifyPropertyChanged("Intentos");
        }
    }
}

