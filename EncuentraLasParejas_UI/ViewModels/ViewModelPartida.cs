using EncuentraLasParejas_BL.Gestora;
using EncuentraLasParejas_Entities;
using EncuentraLasParejas_UI.Models;
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
        public DelegateCommand VolverAEmpezar { get; set; }
        public DelegateCommand VolverAMenu { get; set; }
        private int numeroCartasVolteadas;//esto es un booleano pero no se me ocurria ningun nombre de booleano logico
        private Carta cartaSeleccionada;
        public string Tiempo { get; set; }
        public Carta CartaSeleccionada { get { return cartaSeleccionada; } set {
                if (CartaPrevia != null && CartaSeleccionada != null && (CartaSeleccionada.Id != CartaPrevia.Id) && (CartaSeleccionada.Descubierta || CartaPrevia.Descubierta)){//Con esta línea me aseguro que si el jugador es más rapido clicando que los 500 milisegundo de espera
                   CartaSeleccionada.Descubierta = false;                                                                          // aun así las cartas se volteen, esto se debe a que el task.delay no duerme el hilo principal por lo que
                    CartaPrevia.Descubierta = false;                                                                                // el jugador puede seguir clicando, me parecia lo óptimo ya que  optar por dormir el hilo principal queda bastante abrupto y obligas a estar esperando al jugador aunque sean milesimas
                }
                cartaSeleccionada = value;
                if (!(cartaSeleccionada is null) && (!cartaSeleccionada.Descubierta || !CartaPrevia.Descubierta))
                {
                    Clicar.Execute(Clicar);
                }
            } }
        private Carta CartaPrevia { get; set; }
        private int puntuacion, intentos;
        public int Puntuacion {
            get { return puntuacion; }
            set {
                puntuacion = value;
                if (puntuacion > 8)
                {
                    imprimirResultado(true);
                }
            }
        }
        public int Intentos {
            get { return intentos; }
            set {
                intentos = value;
                if (intentos == -1000)
                {
                    imprimirResultado(false);
                }
            }
        }
        public ViewModelPartida() {
            puntuacion = 0;
            intentos = 6;
            numeroCartasVolteadas = 0;
            llenarListaDeCartas();
            Tiempo = "0";
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += (a, b) => {
                tiempo++;
                TimeSpan tiempoConvertido = TimeSpan.FromSeconds(tiempo);
                Tiempo = tiempoConvertido.ToString("hh':'mm':'ss");
                NotifyPropertyChanged("Tiempo");
            };
            VolverAMenu = new DelegateCommand(volverAlMenu);
            VolverAEmpezar = new DelegateCommand(jugarDeNuevo);
            Timer.Start();
            Clicar = new DelegateCommand(clicarEnCarta_Execute);
        }
        private void jugarDeNuevo(){
            SalirOJugarDeNuevo(ContentDialogResult.Primary);
        }
        private void volverAlMenu(){

            SalirOJugarDeNuevo(ContentDialogResult.None);
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
            await Task.Delay(400);
            CartaSeleccionada.Descubierta = false;
            CartaPrevia.Descubierta = false;
        } 

        private void llenarListaDeCartas() {
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
                else {
                    CartaPrevia = cartaSeleccionada;
                    ++numeroCartasVolteadas; }
            }
        }
        //private bool clicarEnCarta_CanExecute(){
        //    return !(CartaSeleccionada is null);       
        //}
        private async void imprimirResultado(bool resultado) {
            ContentDialog dialog;
            ContentDialogResult resultadoDialog;
            string nombreJugador="";
            Timer.Stop();
            if (resultado) {
                dialog = new ContentDialog() {
                    Title = "Victory! :D",
                    CloseButtonText = "Exit",
                    Content = new TextBox() {
                        PlaceholderText="INTRODUCE YOUR NAME",
                        AcceptsReturn = false,
                        Width = 300,
                        MaxLength = 15
                    },
                    PrimaryButtonText = "Play again!",
                    SecondaryButtonText="Save Score"
            };
                while ((resultadoDialog = await dialog.ShowAsync()) == ContentDialogResult.Secondary)
                {
                    nombreJugador = (dialog.Content as TextBox).Text;
                    if (string.IsNullOrEmpty(nombreJugador))
                    {
                        nombreJugador = "Guess";
                    }
                    guardarResultado(nombreJugador);
                    dialog.Content = new TextBox().Text = "Score saved!";
                    dialog.IsSecondaryButtonEnabled = false;
                }
            } else {
                    dialog = new ContentDialog()
                {
                    Title = "You Lose :(",
                    Content = "Try again!",
                    CloseButtonText = "Exit",
                    PrimaryButtonText = "Try again!"
                };
                resultadoDialog = await dialog.ShowAsync();
            }
            SalirOJugarDeNuevo(resultadoDialog);
        }

        private void guardarResultado(string nombreJugador){
            GestoraPuntuacion_BL.actualizarOInsertar(new clsPuntuacion(nombreJugador,Tiempo));
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
            Timer.Start();
            tiempo=0;
            barajarCartas();
            NotifyPropertyChanged("Tiempo");
            NotifyPropertyChanged("ListaCartas");
            NotifyPropertyChanged("Puntuacion");
            NotifyPropertyChanged("Intentos");
        }
    }
}

