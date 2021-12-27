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
        public IActionResult Index(int posicion,int numeroCartas)
        {
            ViewModels.ViewModelPartida vm = new ViewModels.ViewModelPartida();
            Carta carta=null;
            if (posicion != 0)
            {
                for (int i = 0; i < vm.ListaCartas.Count(); i++)
                {
                    if (i == (posicion - 1))
                    {
                        (carta = vm.ListaCartas[i]).Descubierta = true;
                    }
                }
                if (numeroCartas == 0)
                {                   
                    ViewModels.ViewModelPartida.IdCartaPrevia = carta.Id;
                    numeroCartas++;
                }
                else {
                    numeroCartas=0;
                    if (ViewModels.ViewModelPartida.IdCartaPrevia == carta.Id){
                        
                    }
                }
                
            }
            vm.llenarListaDeCartasOptimizadas();
            return View(vm);
        }
    }
}
