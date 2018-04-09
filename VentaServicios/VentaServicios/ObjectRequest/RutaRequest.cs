using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class RutaRequest
    {

        // Campos de la tabla LogRutaVendedor
        public int IdLogRutaVendedor { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }
        
        public DateTime? Fecha { get; set; }

        public int IdVendedor { get; set; }

        public ClienteRequest ClienteRequest { get; set; }
    }
}