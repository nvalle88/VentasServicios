using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class LivePositionRequest
    {
        public int EmpresaId { get; set; }
        public int AgenteId { get; set; }
        public string Nombre { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public DateTime fecha { get; set; }

    }
}