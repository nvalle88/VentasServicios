using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class GerenteRequest
    {
        public int IdGerente { get; set; }

        public int? IdUsuario { get; set; }

        public string Direccion { get; set; }

        public string Identificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }
    }
}