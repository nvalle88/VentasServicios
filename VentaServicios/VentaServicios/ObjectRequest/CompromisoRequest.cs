using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class CompromisoRequest
    {
        public int IdCompromiso { get; set; }

        public int IdTipoCompromiso { get; set; }

        public int  idVisita { get; set; }
        public DateTime Fecha { get; set; }
        public string tipocompromiso { get; set; }

        public string Descripcion { get; set; }

        
        public string Solucion { get; set; }
    }
}