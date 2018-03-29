﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [RoutePrefix("api/Vista")]
    public class VisitaController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();
        [HttpPost]
        [Route("ListarVisitas")]
        public async Task<SupervisorRequest> ListarVisitas(SupervisorRequest supervisorRequest)
        {
            try
            {
                var listacompromiso = new List<CompromisoRequest>();
            listacompromiso = db.Compromiso
                                .Join(db.Visita
                                    , rta => rta.idVisita, ind => ind.idVisita,
                                    (rta, ind) => new { hm = rta, gh = ind })

                             .Where(ds =>
                                ds.gh.idCliente == supervisorRequest.IdCliente
                                && ds.gh.IdVendedor == supervisorRequest.IdVendedor
                                && ds.gh.Fecha >= supervisorRequest.FechaInicio
                                && ds.gh.Fecha <= supervisorRequest.FechaFin
                              )
                              .Select(t => new CompromisoRequest
                              {
                                  IdCompromiso = t.hm.IdCompromiso,
                                  idVisita = t.hm.idVisita,
                                  Descripcion = t.hm.Descripcion,
                                  Solucion = t.hm.Solucion,
                                  Fecha = t.gh.Fecha
                              })
                              .ToList();


                supervisorRequest.Listarcompromiso = listacompromiso;
                return supervisorRequest;
            }
            catch (Exception ex)
            {
                return supervisorRequest;
            }
        }
    }
}
