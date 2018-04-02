using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class TipoCompromisoRequest
    {
        public int IdTipoCompromiso { get; set; }
        
        public string Descripcion { get; set; }


        // Este campo se usa para realizar el estadístico (No es parte de la tabla)
        public int? CantidadCompromiso { get; set; }
    }
}