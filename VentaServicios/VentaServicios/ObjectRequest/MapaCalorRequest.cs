using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaServicios.ObjectRequest
{
    public class MapaCalorRequest
    {
        public int IdTipoCLiente { get; set; }
        public int IdEmpresa { get; set; }

        public string DescripcionTipoCLiente { get; set; }

        public int IdTipoCompromiso { get; set; }

        public int Latitud { get; set; }

        public int Logintud { get; set; }
        public List<ClienteRequest> ListaClientes { get; set; }
        public List<TipoClienteRequest> ListaTipoCliente { get; set; }
        public List<TipoCompromisoRequest> ListaTipoCompromiso { get; set; }
        public List<VisitaRequest> ListaVisita { get; set; }
        public List<VisitaRequest> ListaVisitaCompromiso { get; set; }
        
    }
}