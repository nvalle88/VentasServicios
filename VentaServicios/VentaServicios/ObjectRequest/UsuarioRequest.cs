using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class UsuarioRequest
    {
        public int IdUsuario { get; set; }

        public string Foto { get; set; }

        public bool? Estado { get; set; }

        public string Contrasena { get; set; }

        public string Correo { get; set; }

        public string Direccion { get; set; }

        public string Identificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public int? IdEmpresa { get; set; }
    }
}