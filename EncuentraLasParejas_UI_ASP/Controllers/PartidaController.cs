using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncuentraLasParejas_UI_ASP.Controllers
{
    public class PartidaController : Controller
    {
        public IActionResult Index()
        {
            ViewModels.ViewModelPartida vm = new ViewModels.ViewModelPartida();
            return View();
        }
    }
}
