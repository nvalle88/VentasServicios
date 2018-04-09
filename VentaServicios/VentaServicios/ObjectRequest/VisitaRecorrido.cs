using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaServicios.ModeloDato;

namespace VentaServicios.ObjectRequest
{
    public class VisitaRecorrido
    {
        public int IdVisita { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public TimeSpan TiempoRecorrido { get; set; }
        public ClienteRequest ClienteRequest { get; set; }
        public List<CompromisosRecorrido> ListaCompromisos { get; set; }
    }
}