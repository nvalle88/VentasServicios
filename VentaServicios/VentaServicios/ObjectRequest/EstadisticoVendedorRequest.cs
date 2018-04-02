using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class EstadisticoVendedorRequest
    {

        // Campos de la tabla vendedor
        public int IdVendedor { get; set; }

        public int? CalificacionPromedio { get; set; }

        public int? CompromisosCumplidos { get; set; }

        public int? CompromisosIncumplidos { get; set; }

        public List<TipoCompromisoRequest> ListaTipoCompromiso { get; set; }

    }
}