using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VentaServicios.ModeloDato;
using VentaServicios.ObjectRequest;
using VentaServicios.Utils;
namespace VentaServicios.Controllers.API
{
    [RoutePrefix("api/MapaCalor")]
    public class MapaCalorController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();

        [HttpPost]
        [Route("ObtenerTipoClienteTipoCompromisoPorEmpresa")]
        public async Task<MapaCalorRequest> ObtenerTipoClienteTipoCompromisoPorEmpresa(MapaCalorRequest mapaCalorRequest)
        {
            try
            {
                var mapacalor = new MapaCalorRequest();
                var tipocliente = new List<TipoClienteRequest>();
                var ticompromiso = new List<TipoCompromisoRequest>();
                tipocliente = db.TipoCliente.Where(x => x.IdEmpresa == mapaCalorRequest.IdEmpresa).Select(x => new TipoClienteRequest
                {
                    idTipoCliente = x.idTipoCliente,
                    Tipo = x.Tipo,

                }).ToList();


                ticompromiso = db.TipoCompromiso.Select(x => new TipoCompromisoRequest
                {
                    IdTipoCompromiso = x.IdTipoCompromiso,
                    Descripcion = x.Descripcion

                }).ToList();

                mapacalor.ListaTipoCliente = tipocliente;
                mapacalor.ListaTipoCompromiso = ticompromiso;
                return mapacalor;

            }
            catch (Exception ex)
            {
                return mapaCalorRequest;
            }
        }

        [HttpPost]
        [Route("ListarClientesPorTipoCliente")]
        public async Task<MapaCalorRequest> ListarClientesPorTipoCliente(MapaCalorRequest mapaCalorRequest)
        {
            var mapa = new MapaCalorRequest();
            var listaClientes = new List<ClienteRequest>();
            try
            {
                listaClientes = db.TipoCliente
                        .Join(db.Cliente
                            , tc => tc.idTipoCliente, cli => cli.idTipoCliente,
                            (tc, cli) => new { hm = tc, gh = cli })
                     .Where(ds =>
                        ds.gh.idTipoCliente == mapaCalorRequest.IdTipoCLiente
                      )
                      .Select(t => new ClienteRequest
                      {
                          IdTipoCliente = t.gh.idTipoCliente,
                          Latitud = t.gh.Latitud,
                          Longitud = t.gh.Longitud
                      })
                      .ToList();

                //db.Configuration.ProxyCreationEnabled = false;
                //listaClientes = db.Cliente.Select(x => new ClienteRequest
                //{
                //    IdCliente = x.idCliente,
                //    Longitud =x.Longitud,
                //    Latitud = x.Latitud
                //}

                //).Where(x => x.IdTipoCliente == mapaCalorRequest.IdTipoCLiente).ToList();

                mapa.ListaClientes = listaClientes;

                return mapa;
            }
            catch (Exception ex)
            {
                return mapa;
            }
        }
    }
}
