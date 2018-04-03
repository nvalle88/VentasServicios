using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class EstadisticoSupervisorRequest
    {
        public int IdSupervisor { get; set; }

        public int? CalificacionPromedio { get; set; }

        public int? CompromisosCumplidos { get; set; }

        public int? CompromisosIncumplidos { get; set; }

        public List<TipoCompromisoRequest> ListaTipoCompromiso { get; set; }
    }
}