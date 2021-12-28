using EncuentaLasParejas_UI_ASP.Models;
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
        [HttpGet]
        [HttpPost]
        public IActionResult Index(int posicion)
        {
            ViewModels.ViewModelPartida vm = new ViewModels.ViewModelPartida();
            Carta carta = null;
            if (posicion != 0)
            {
                for (int i = 0; i < vm.ListaCartas.Count(); i++)
                {
                    if (i == (posicion - 1))
                    {
                        (carta = vm.ListaCartas[i]).Descubierta = true;
                    }
                }
                if (!ViewModels.ViewModelPartida.ParejaVolteada)
                {
                    ViewModels.ViewModelPartida.IdCartaPrevia = carta.Id;
                    ViewModels.ViewModelPartida.ParejaVolteada = true;
                }
                else {
                    ViewModels.ViewModelPartida.ParejaVolteada = false;
                    if (ViewModels.ViewModelPartida.IdCartaPrevia == carta.Id)
                    {
                        ViewModels.ViewModelPartida.Puntuacion++;

                    }
                    else {
                        carta.Descubierta = false;
                        vm.ListaCartas.Find(carta=>carta.Id== ViewModels.ViewModelPartida.IdCartaPrevia).Descubierta=false;
                        ViewModels.ViewModelPartida.Intentos--;
                    }
                }
            }
            vm.llenarListaDeCartasOptimizadas();
            return View(vm);
        }

        private void inicializarPunutacionEIntentos()
        {
            if (ViewModels.ViewModelPartida.Intentos == 0 || ViewModels.ViewModelPartida.Puntuacion == 9){
                ViewModels.ViewModelPartida.Intentos =6;
                ViewModels.ViewModelPartida.Puntuacion =0;
            }
        }
    }
}
