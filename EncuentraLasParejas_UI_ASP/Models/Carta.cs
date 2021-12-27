using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EncuentraLasParejas_UI_ASP.Models
{
    public class Carta
    {
        #region propiedades autoimplementadas
        public int Id;
        private bool descubierta;
        private string cartaPorDetras = "/Assets/ImagenesDeLasCartas/cartaPorDetras.png";
        public bool Descubierta {
            get{ return descubierta;}
            set{ descubierta = value;
                if (descubierta) {
                    Imagen=UbicacionImagen;
                }else{
                    Imagen = cartaPorDetras;
                }
            }
        }
        public string UbicacionImagen;
        public string Imagen {get; set;}
        #endregion
        #region Constructores
        public Carta(int id, bool descubierta, string ubicacionImagen)
        {
            Id = id;
            Descubierta = descubierta;
            this.UbicacionImagen = ubicacionImagen;
        }
        public Carta()
        {
            Id = 0;
            Descubierta = true;
            this.UbicacionImagen = "";
        }
        public Carta(Carta carta)
        {
            Id=carta.Id;
            Descubierta= carta.Descubierta;
            UbicacionImagen= carta.UbicacionImagen;
        }
        #endregion

        

    }
}
