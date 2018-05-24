using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class DistanciaRequest
    {
        public int IdVendedor { get; set; }
        public double? DistanciaSeguimiento { get; set; }
        public bool isSet { get; set; }
    }
}