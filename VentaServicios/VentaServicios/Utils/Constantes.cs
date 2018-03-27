using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace VentaServicios.Utils
{
    public static class Constantes
    {
        public static int CuotaInferiorCodigo { get; set; }
        public static int CuotaSuperiorCodigo { get; set; }
        public static MailAddress EmailAdmin { get; internal set; }
    }
}