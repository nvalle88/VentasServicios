using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaServicios.Utils.GeoUtils;

namespace VentaServicios.ModeloDato
{
    public class NearClientRequest
    {
        public Position Position { get; set; }
        public int myId { get; set; }
        public double radio { get; set; }
    }
}