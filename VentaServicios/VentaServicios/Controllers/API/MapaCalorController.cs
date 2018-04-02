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

                mapa.ListaClientes = listaClientes;

                return mapa;
            }
            catch (Exception ex)
            {
                return mapa;
            }
        }

        [HttpPost]
        [Route("ListarVisitasPorTipoCompromiso")]
        public async Task<MapaCalorRequest> ListarVisitasPorTipoCompromiso(MapaCalorRequest mapaCalorRequest)
        {
            var mapa = new MapaCalorRequest();
            var listavistaporCompromiso = new List<VisitaRequest>();
            try
            {
                listavistaporCompromiso = db.Compromiso
                        .Join(db.Visita
                            , tc => tc.idVisita, cli => cli.idVisita,
                            (tc, cli) => new { hm = tc, gh = cli })
                     .Where(ds =>
                        ds.hm.IdTipoCompromiso == mapaCalorRequest.IdTipoCompromiso
                      )
                      
                      .Select(t => new VisitaRequest
                      {
                          //idCliente = t.Count(),
                          idVisita = t.gh.idVisita,
                          Latitud = t.gh.Latitud,
                          Longitud = t.gh.Longitud
                      })
                      .ToList();

                mapa.ListaVisitaCompromiso = listavistaporCompromiso;

                return mapa;
            }
            catch (Exception ex)
            {
                return mapa;
            }
        }
        [HttpPost]
        [Route("ListarVisitas")]
        public async Task<MapaCalorRequest> ListarVisitas(MapaCalorRequest mapaCalorRequest)
        {
            var mapa = new MapaCalorRequest();
            var listavista = new List<VisitaRequest>();
            try
            {
                listavista = db.Visita
                      .Select(t => new VisitaRequest
                      {
                          idVisita =t.idVisita,
                          Latitud = t.Latitud,
                          Longitud = t.Longitud
                      })
                      .ToList();

                mapa.ListaVisita = listavista;

                return mapa;
            }
            catch (Exception ex)
            {
                return mapa;
            }
        }
    }
}
