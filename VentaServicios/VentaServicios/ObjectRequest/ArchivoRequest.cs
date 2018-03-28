using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class ArchivoRequest
    {
        public string Id { get; set; }
        public int Tipo { get; set; }
        public byte[] Array { get; set; }
    }
}