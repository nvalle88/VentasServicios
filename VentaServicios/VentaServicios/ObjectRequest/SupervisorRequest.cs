using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class SupervisorRequest
    {
        public int IdSupervisor { get; set; }

        public string IdUsuario { get; set; }

        public int IdGerente { get; set; }
        public bool? Estado { get; set; }
        public string Correo { get; set; }

        public string Direccion { get; set; }

        public string Identificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }
        public int IdEmpresa { get; set; }
    }
}