using EncuentaLasParejas_UI_ASP.Models;
using EncuentraLasParejas_BL.Gestora;
using EncuentraLasParejas_BL.Listados;
using EncuentraLasParejas_Entities;
using EncuentraLasParejas_UI_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EncuentaLasParejas_UI_ASP.Controllers
{
    public class PartidaController : Controller
    {
        public IActionResult Index()
        {
            ViewModels.ViewModelPartida vm = new ViewModels.ViewModelPartida();
            if (ViewModels.ViewModelPartida.VoltearCartas)
            {
                ViewModels.ViewModelPartida.ResultadoComprobado = false;
                ViewModels.ViewModelPartida.Intentos--;
                ViewModels.ViewModelPartida.ListaCartas.ForEach(carta =>
                {
                    if ((carta.Id == ViewModels.ViewModelPartida.IdCartaPrevia || carta.Id == ViewModels.ViewModelPartida.IdCartaActual) && carta.Descubierta)
                    {
                        carta.Descubierta = false;
                    }
                });
                ViewModels.ViewModelPartida.VoltearCartas = false;
            }
            if (ViewModels.ViewModelPartida.ResultadoComprobado)
            { ViewModels.ViewModelPartida.VoltearCartas = true; }            
            vm.llenarListaDeCartasOptimizadas();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(int posicion, String nombreJugador, int volverAJugarONo, int tiempoGlobal)
        {
            IActionResult view = null;
            Carta carta = null;
            if (tiempoGlobal != 0)
            {
                ViewModels.ViewModelPartida.Tiempo = tiempoGlobal;
            }
            if (volverAJugarONo == 0)
            {
                if (posicion != 0)
                {
                    carta = averiguarCarta(posicion);
                    comprobarResultado(carta);
                }
                view = RedirectToAction("Index");
            }
            else if (volverAJugarONo == 1)
            {
                restaurarElementos();
                view = RedirectToAction("Index");
            } else if (volverAJugarONo == 2) {
                if (string.IsNullOrEmpty(nombreJugador)) {
                    nombreJugador = "Guess";
                }
                GestoraPuntuacion_BL.actualizarOInsertar(new clsPuntuacion(nombreJugador, ViewModels.ViewModelPartida.Tiempo.ToString()));
                ViewModels.ViewModelPartida.Resultado = 3;
                view = RedirectToAction("Index");
            } else if (volverAJugarONo == 3){
                view = RedirectToAction("Menu");
            }
            return view;
        }

        /// <summary>
        /// Se encarga de comprobar si la carta clicada es la primera o segunda y en el caso de que sea la segunda comprueba el resultado viendo si son pareja o no
        /// </summary>
        /// <param name="carta">Carta carta</param>
        private void comprobarResultado(Carta carta){
            if (carta != null)
            {
                if (!ViewModels.ViewModelPartida.ParejaVolteada)
                {//si no hay dos cartas ya volteadas
                    ViewModels.ViewModelPartida.IdCartaPrevia = carta.Id;
                    ViewModels.ViewModelPartida.ParejaVolteada = true;
                }
                else
                {
                    ViewModels.ViewModelPartida.ParejaVolteada = false;
                    if (ViewModels.ViewModelPartida.IdCartaPrevia == carta.Id)
                    {
                        ViewModels.ViewModelPartida.ListaCartas.Find(cartaLista => cartaLista.Id == ViewModels.ViewModelPartida.IdCartaPrevia).Descubierta = true;//le pongo  cartaLista por que es la carta que pertenece a la lista y que así el nombre no sea el mismo que el del parámetro carta
                        ViewModels.ViewModelPartida.Puntuacion++;

                    }
                    else
                    {
                        ViewModels.ViewModelPartida.ResultadoComprobado = true;
                        ViewModels.ViewModelPartida.IdCartaActual = carta.Id;
                    }
                }
            }
        }


        /// <summary>
        /// Con el fin de ahorrarme el envio de un objeto al metodos post y así que el cliente no tuviese que cargar un objeto completo, ya que lo que recibe el cliente es un modelo con menos atributos,
        /// una vez tengo la posicion del objeto clicado lo obtendré de la lista, al ser una lista de tan solo 18 elementos, es más optimo a mi parecer hacer esto, que hacer que el cliente tuviese los 18 objetos cargados completos,
        /// y estos fueran al post cuando se clicasen en cada uno de ellos.
        /// Ahora sí, el proposito de este método es buscar en la lista el objeto que ha sido clicado por el usuario
        /// </summary>
        /// <param name="posicion">int posicion del objeto clicado</param>
        /// <returns>Carta cartaClicada</returns>
        private Carta averiguarCarta(int posicion){
            Carta carta = null;
            for (int i = 0; i < ViewModels.ViewModelPartida.ListaCartas.Count(); i++)//Encuentro la carta clicada
            {
                if (i == (posicion - 1) && !ViewModels.ViewModelPartida.ListaCartas[i].Descubierta)
                {
                    (carta = ViewModels.ViewModelPartida.ListaCartas[i]).Descubierta = true;//una vez encontrada dicha carta, voy a modificar sus propiedades
                }
            }
            return carta;
        }

        public IActionResult Menu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Menu(int eleccionMenu)
        {
            IActionResult view = null;
            if (eleccionMenu == 1)
            {
                view = RedirectToAction("Index");
            }
            else {
                view = RedirectToAction("Scoreboard");
            }
            return view;
        }

        public IActionResult Scoreboard()
        {
            List<clsPuntuacion> listado=null;
            IActionResult view = null;
            try
            {
                listado = ListadoPuntuacion_BL.getListadoCompletoPuntuacion().OrderBy(puntuacion => puntuacion.Tiempo).ToList();
                view = View(listado);
            }
            catch (SqlException){
                view = View("Error");
            }
            return view;
        }

        [HttpPost]
        public IActionResult Scoreboard(bool retroceder)
        {
            IActionResult view = null;
            if (retroceder){
                view = RedirectToAction("Menu");
            }
            return view;
        }
        /// <summary>
        /// Restaura los elementos necesarios para que la partida vuelva a empezar de nuevo
        /// </summary>
        private void restaurarElementos()
        {            
            ViewModels.ViewModelPartida.ListaCartas = null;
        }
    }
}
