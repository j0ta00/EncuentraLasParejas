using EncuentaLasParejas_UI_ASP.Models;
using EncuentraLasParejas_BL.Gestora;
using EncuentraLasParejas_Entities;
using EncuentraLasParejas_UI_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            Carta carta = null;
            ViewModels.ViewModelPartida.Tiempo = tiempoGlobal;
            if (volverAJugarONo == 0)
            {
                if (posicion != 0)
                {
                    for (int i = 0; i < ViewModels.ViewModelPartida.ListaCartas.Count(); i++)//Encuentro la carta clicada
                    {
                        if (i == (posicion - 1))
                        {
                            (carta = ViewModels.ViewModelPartida.ListaCartas[i]).Descubierta = true;//una vez encontrada dicha carta, voy a modificar sus propiedades
                        }
                    }
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
                            ViewModels.ViewModelPartida.ListaCartas.Find(carta => carta.Id == ViewModels.ViewModelPartida.IdCartaPrevia).Descubierta = true;
                            ViewModels.ViewModelPartida.Puntuacion++;

                        }
                        else
                        {
                            //carta.Descubierta = false;
                            //ViewModels.ViewModelPartida.ListaCartas.Find(carta => carta.Id == ViewModels.ViewModelPartida.IdCartaPrevia).Descubierta = false;                       
                            ViewModels.ViewModelPartida.ResultadoComprobado = true;
                            ViewModels.ViewModelPartida.IdCartaActual = carta.Id;
                        }
                    }
                }
            }
            else if (volverAJugarONo == 1)
            {
                restaurarElementos();
            } else if (volverAJugarONo==2){
                if (string.IsNullOrEmpty(nombreJugador)){
                    nombreJugador = "Guess";
                }
                GestoraPuntuacion_BL.actualizarOInsertar(new clsPuntuacion(nombreJugador,ViewModels.ViewModelPartida.Tiempo.ToString()));
            }
            return RedirectToAction("Index");
        }


        private void restaurarElementos()
        {            
            ViewModels.ViewModelPartida.ListaCartas = null;
        }
    }
}
