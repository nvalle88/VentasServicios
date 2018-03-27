using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class RecuperarContrasenaRequest
    {
        public string Email { get; set; }

        public string Codigo { get; set; }
    }
}