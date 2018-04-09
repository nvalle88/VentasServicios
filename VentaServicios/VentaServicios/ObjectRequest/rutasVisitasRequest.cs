using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class RutasVisitasRequest
    {
        public List<RutaRequest> ListaRutas { get; set; }
        public List<VisitaRecorrido >ListaVisitas { get; set; }
    }
}