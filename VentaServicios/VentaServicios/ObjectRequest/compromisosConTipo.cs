using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class compromisosConTipo
    {
        public int Hora { get; set; }

        public string TipoCompromiso { get; set; }

        public string Detalle { get; set; }

        public string Solucion { get; set; }

        public TimeSpan TiempoRecorrido { get; set; }
    }
}