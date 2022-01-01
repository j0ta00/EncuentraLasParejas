using EncuentraLasParejas_BL.Gestora;
using EncuentraLasParejas_Entities;
using EncuentraLasParejas_UI.Models;
using EncuentraLasParejas_UI.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
        #region propiedades
        private int tiempo;
        private DispatcherTimer Timer = new DispatcherTimer();
        public ObservableCollection<Carta> ListaCartas { get; set; }
        public DelegateCommand Clicar { get; set; }
        public DelegateCommand VolverAEmpezar { get; set; }
        public DelegateCommand VolverAMenu { get; set; }
        private bool numeroMaximoCartasVolteadas;
        private Carta cartaSeleccionada;
        public string Tiempo { get; set; }
        public Carta CartaSeleccionada { get { return cartaSeleccionada; } set {
                if (CartaPrevia != null && CartaSeleccionada != null && (CartaSeleccionada.Id != CartaPrevia.Id) && (CartaSeleccionada.Descubierta || CartaPrevia.Descubierta)){//Con esta línea me aseguro que si el jugador es más rapido clicando que los 500 milisegundo de espera
                   CartaSeleccionada.Descubierta = false;                                                                          // aun así las cartas se volteen, esto se debe a que el task.delay no duerme el hilo principal por lo que
                    CartaPrevia.Descubierta = false;                                                                                // el jugador puede seguir clicando, me parecia lo óptimo ya que  optar por dormir el hilo principal queda bastante abrupto y obligas a estar esperando al jugador aunque sean milesimas
                }
                cartaSeleccionada = value!=null && value.Descubierta==true?null:value;//esto es para evitar que clicando en cartas ya descubiertas consigas puntos
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
                if (intentos == 0)
                {
                    imprimirResultado(false);
                }
            }
        }
        #endregion
        #region constructor
        public ViewModelPartida() {
            puntuacion = 0;
            intentos = 9;
            numeroMaximoCartasVolteadas = false;
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
        #endregion
        #region metodos y commands
        /// <summary>
        /// Command asociado a un boton que llamará a una funcion la cual se encargará de hacer lo que le botón haga, en este caso jugar de nuevo
        /// </summary>
        private void jugarDeNuevo(){
            SalirOJugarDeNuevo(ContentDialogResult.Primary);
        }
        /// <summary>
        /// Command asociado a un boton que llamará a una funcion la cual se encargará de hacer lo que le botón haga, en este caso volver al menú principal
        /// </summary>
        private void volverAlMenu(){

            SalirOJugarDeNuevo(ContentDialogResult.None);
        }
        /// <summary>
        /// Comprueba el resultado de las cartas, es decir si coinciden aumenta la puntuacion y si no bajan los intentos
        /// </summary>
        private void comprobarCartas() {
            if (CartaSeleccionada.Id == CartaPrevia.Id)
                {               
                    Puntuacion++;
                    CartaSeleccionada = null;
                    CartaPrevia = null;
                    NotifyPropertyChanged("Puntuacion");
                }
                else {
                    Intentos--;
                    esperarYDarLaVuelta();
                    NotifyPropertyChanged("Intentos");
            }   
        }
        /// <summary>
        /// Voltea las cartas si estas no han coincidido dando unas milesimas para que el usuario pueda recordar donde estaban
        /// </summary>
        private async void esperarYDarLaVuelta(){
            await Task.Delay(400);
            CartaSeleccionada.Descubierta = false;
            CartaPrevia.Descubierta = false;
        }
        /// <summary>
        /// Llena una lista con los objetos carta necesarios para el juego
        /// </summary>
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
        /// <summary>
        /// Baraja las cartas haciendo que su disposicion sea completamente aleatoria
        /// </summary>
        private void barajarCartas(){
            var rand = new Random();
            ListaCartas = new ObservableCollection<Carta>(ListaCartas.Select(carta => new { Carta = carta, R = rand.Next() })
                 .OrderBy(x => x.R)
                 .Select(x => x.Carta)
                 .ToList());
        }
        /// <summary>
        /// En función del numero de cartas volteadas guardará el id de la carta actual y aumentará el numero de carta volteadas o en el caso
        /// de que ya hubiera 1 carta volteada comprobará si estas coinciden o no llamando a una función para ello
        /// </summary>
        private void clicarEnCarta_Execute() {
            if (!cartaSeleccionada.Descubierta)
            {
                cartaSeleccionada.Descubierta = true;
                if (numeroMaximoCartasVolteadas)
                {
                    comprobarCartas();
                    numeroMaximoCartasVolteadas = false;
                }
                else {
                    CartaPrevia = cartaSeleccionada;
                    numeroMaximoCartasVolteadas=true; }
            }
        }

        //private bool clicarEnCarta_CanExecute(){
        //    return !(CartaSeleccionada is null);       
        //}//por alguna razon el can execute no me funcionaba lo cual no tiene sentido nignuno
        /// <summary>
        /// En funcion del resultado de la partida imprime un mensaje u otro con el que interactuará el usuario
        /// </summary>
        /// <param name="resultado"></param>
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
        /// <summary>
        /// LLama a l bl para que esta se encargue de aplicar las lógicas de negocio y llamar a las capas necesarias para que se acabe almacenando la puntuacion obtenida en una bbdd
        /// </summary>
        /// <param name="nombreJugador"></param>
        private void guardarResultado(string nombreJugador){
            try
            {
                GestoraPuntuacion_BL.actualizarOInsertar(new clsPuntuacion(nombreJugador,TimeSpan.Parse(Tiempo).TotalSeconds.ToString()));
            }
            catch (SqlException){
                ViewModelMainPage.mostrarContentDialogErrorSql();//esto es básicamente para no tener dos veces el mismo método, podría haberlo metido en una clase utilidades o algo así pero siendo un único método me parecía un poco innecesario
            }
        }
        /// <summary>
        /// En funcion de lo elegido por el usuario llama a las funciones necesarias para volver al menú principal o para empezar una nueva partida
        /// </summary>
        /// <param name="resultado"></param>
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
        /// <summary>
        /// Restaura los elementos necesarios para que el juego vuelva a empezar
        /// </summary>
        private void volverAJugar(){
            this.Puntuacion=0;
            this.Intentos=9;
            numeroMaximoCartasVolteadas = false;
            ListaCartas.ToList().ForEach(carta=>carta.Descubierta=false);
            Timer.Start();
            tiempo=0;
            barajarCartas();
            NotifyPropertyChanged("Tiempo");
            NotifyPropertyChanged("ListaCartas");
            NotifyPropertyChanged("Puntuacion");
            NotifyPropertyChanged("Intentos");
        }
        #endregion
    }
}

