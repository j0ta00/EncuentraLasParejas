using EncuentraLasParejas_UI_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncuentaLasParejas_UI_ASP.Models
{
    public class CartaImagenId
    {
        public CartaImagenId(Carta carta)
        {
            Imagen=carta.Imagen;
        }
        public CartaImagenId(){
        
        }
        public string Imagen {get; set;}
    }
}
