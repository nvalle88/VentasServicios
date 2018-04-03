using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaServicios.ModeloDato;

namespace VentaServicios.ObjectRequest
{
    public class CheckinRequest
    {
        public List<Compromiso> compromisos { get; set; }
        public Visita visita { get; set; }
    }
}