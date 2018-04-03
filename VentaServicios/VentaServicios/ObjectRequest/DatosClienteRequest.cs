using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaServicios.ModeloDato;

namespace VentaServicios.ObjectRequest
{
    public class DatosClienteRequest
    {
        public Cliente cliente { get; set; }
        public List<Compromiso> comprimisos { get; set; }
    }
}