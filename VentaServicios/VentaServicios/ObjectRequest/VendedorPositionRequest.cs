using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class VendedorPositionRequest
    {
        public int EmpresaId { get; set; }
        public int VendedorId { get; set; }
        public string Nombre { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public DateTime Fecha { get; set; }
        public string urlFoto { get; set; }
    }
}