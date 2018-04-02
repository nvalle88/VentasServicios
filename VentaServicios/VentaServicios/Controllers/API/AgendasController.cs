
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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


    [RoutePrefix("api/Agendas")]
    public class AgendasController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();



        
        // POST: api/Agendas
        [HttpPost]
        [Route("ListarEventosPorVendedor")]
        public async Task<List<EventoRequest>> ListarEventosPorVendedor(VendedorRequest vendedorRequest)
        {

            //Solo necesita el IdVendedor

            //Prioridad: 0 = baja (verde), 1 = media (naranja), 2 = alta (rojo) 

            var lista = new List<EventoRequest>();

            try
            {
                var listaAgenda = await db.Agenda.Select(x => new EventoRequest 
                {
                    idAgenda = x.idAgenda,
                    Prioridad = x.Prioridad,
                    FechaFin = x.FechaFin,
                    FechaVista = x.FechaVista,
                    Notas = x.Notas,
                    idCliente = x.idCliente,
                    IdVendedor = x.IdVendedor

                }
                    
                ).Where( y=> y.IdVendedor == vendedorRequest.IdVendedor).ToListAsync();


                var listaCompromiso = await db.Compromiso
                    .Join(db.Visita, com => com.idVisita, v => v.idVisita, (com, v) => new { tcom = com, tv = v })
                    .Join(db.Vendedor, conjunto => conjunto.tv.IdVendedor, ven => ven.IdVendedor, (conjunto, ven) => new { varConjunto = conjunto, tven = ven })
                .Where(y => y.tven.IdVendedor == vendedorRequest.IdVendedor)
                .Select(x => new EventoRequest
                {
                    idAgenda = 0,

                    IdCompromiso = x.varConjunto.tcom.IdCompromiso,
                    IdTipoCompromiso = x.varConjunto.tcom.IdTipoCompromiso,
                    idVisita = x.varConjunto.tcom.idVisita,
                    Descripcion = x.varConjunto.tcom.Descripcion,
                    Solucion = x.varConjunto.tcom.Solucion,

                    FechaVista = x.varConjunto.tv.Fecha,

                    IdVendedor = x.tven.IdVendedor

                }

                ).ToListAsync();

                lista = listaAgenda;

                for (int i=0;i<listaCompromiso.Count;i++)
                {
                    lista.Add(listaCompromiso.ElementAt(i));
                }


                return lista;
            }
            catch (Exception ex)
            {
                return lista;
            }
        }

        // POST: api/Agendas
        [HttpPost]
        [Route("VerEstadisticosVendedor")]
        public async Task<EstadisticoVendedorRequest> VerEstadisticosVendedor(VendedorRequest vendedorRequest)
        {

            EstadisticoVendedorRequest estadisticoVendedorRequest = new EstadisticoVendedorRequest();


            //Solo necesita el IdVendedor

            var promedio = 0;
            var listaVisitas = new List<Visita>();

            try
            {

                // Lógica sacar promedio de calificaciones
                listaVisitas = await db.Visita.Where(y => y.IdVendedor == vendedorRequest.IdVendedor).ToListAsync();

                
                for (int i = 0; i<listaVisitas.Count; i++) {
                    promedio = promedio + Convert.ToInt32(listaVisitas.ElementAt(i).Calificacion);
                }

                promedio = promedio / listaVisitas.Count;



                // Lógica para estadísticos pasteles (tipo de compromiso)
                var listaCompromiso = await db.Compromiso
                    .Join(db.TipoCompromiso, com => com.IdTipoCompromiso, tc => tc.IdTipoCompromiso, (com, tc) => new { tcom = com, ttc = tc })
                    .Join(db.Visita, conjunto1 => conjunto1.tcom.idVisita, visita => visita.idVisita, (conjunto1, visita) => new {  Aconjunto1 = conjunto1, Avis = visita })
                    .Join(db.Vendedor, conjunto2 => conjunto2.Avis.IdVendedor, ven => ven.IdVendedor, (conjunto2, ven) => new { AConjunto2 = conjunto2, Aven = ven })

                .Where(y => y.Aven.IdVendedor == vendedorRequest.IdVendedor)
                .Select(x => new TipoCompromisoRequest
                {
                    IdTipoCompromiso = x.AConjunto2.Aconjunto1.ttc.IdTipoCompromiso,
                    Descripcion = x.AConjunto2.Aconjunto1.ttc.Descripcion

                }

                ).GroupBy(z => z.Descripcion).ToListAsync();


                var listaTipoCompromisos = new List<TipoCompromisoRequest>();
                
                for (int i = 0; i < listaCompromiso.Count; i++)
                {
                    var num = listaCompromiso.ElementAt(i).Count();

                    listaTipoCompromisos.Add(
                        new TipoCompromisoRequest {
                            Descripcion = listaCompromiso.ElementAt(i).ElementAt(0).Descripcion,
                            CantidadCompromiso = num
                        }
                    );

                    
                }


                // Lógica para compromisos cumplidos - incumplidos

                var cumplidos = await db.Compromiso
                    .Join(db.Visita, com => com.idVisita, v => v.idVisita, (com, v) => new { tcom = com, tv = v })
                    .Join(db.Vendedor, conjunto => conjunto.tv.IdVendedor, ven => ven.IdVendedor, (conjunto, ven) => new { varConjunto = conjunto, tven = ven })
                    .Where(y => y.tven.IdVendedor == vendedorRequest.IdVendedor && !String.IsNullOrEmpty(y.varConjunto.tcom.Solucion) )
                    .ToListAsync();

                var incumplidos = await db.Compromiso
                    .Join(db.Visita, com => com.idVisita, v => v.idVisita, (com, v) => new { tcom = com, tv = v })
                    .Join(db.Vendedor, conjunto => conjunto.tv.IdVendedor, ven => ven.IdVendedor, (conjunto, ven) => new { varConjunto = conjunto, tven = ven })
                    .Where(y => y.tven.IdVendedor == vendedorRequest.IdVendedor && String.IsNullOrEmpty(y.varConjunto.tcom.Solucion) )
                    .ToListAsync();


                estadisticoVendedorRequest.IdVendedor = vendedorRequest.IdVendedor;
                estadisticoVendedorRequest.CalificacionPromedio = promedio;
                estadisticoVendedorRequest.ListaTipoCompromiso = listaTipoCompromisos;
                estadisticoVendedorRequest.CompromisosCumplidos = cumplidos.Count();
                estadisticoVendedorRequest.CompromisosIncumplidos = incumplidos.Count();

                return estadisticoVendedorRequest;
            }
            catch (Exception ex)
            {
                return estadisticoVendedorRequest;
            }
        }

    }
}