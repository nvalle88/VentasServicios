
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


    [RoutePrefix("api/Vendedores")]
    public class VendedoresController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();



        
        // POST: api/Vendedores
        [HttpPost]
        [Route("ListarVendedores")]
        public async Task<List<VendedorRequest>> ListarVendedores(VendedorRequest vendedorRequest)
        {

            //Solo necesita el IdEmpresa
            // solo muestra vendedores con estado 1("Activado")

            var listaVendedores = new List<VendedorRequest>();

            try
            {
                listaVendedores = await db.Vendedor.Select(x => new VendedorRequest 
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,

                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = vendedorRequest.idEmpresa
                    
                }
                    
                ).Where( x=> x.idEmpresa == vendedorRequest.idEmpresa && x.Estado == 1).ToListAsync();

                return listaVendedores;
            }
            catch (Exception ex)
            {
                return listaVendedores;
            }
        }

        // POST: api/Vendedores
        [HttpPost]
        [Route("ListarVendedoresConUbicacionPorSupervisor")]
        public async Task<List<UbicacionPersonaRequest>> ListarVendedoresConUbicacionPorSupervisor(VendedorRequest vendedorRequest)
        {

            // Necesarios: IdEmpresa E idSupervisor
            // solo muestra vendedores con estado 1("Activado")
            

            var listaVendedores = new List<UbicacionPersonaRequest>();

            
            //var vLongitud = db.LogRutaVendedor.Where(y => y.IdVendedor == 16 && y.Fecha == DateTime.Today).OrderByDescending(y => y.Fecha).FirstOrDefault().Longitud;

            try
            {
                listaVendedores = await db.Vendedor.Select(x => new UbicacionPersonaRequest
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,

                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = vendedorRequest.idEmpresa,

                    //ListaUbicaciones = db.LogRutaVendedor.Where(y => y.IdVendedor == x.IdVendedor).ToList(),

                    Latitud = db.LogRutaVendedor.Where(y => y.IdVendedor == x.IdVendedor).OrderByDescending(y => y.Fecha).FirstOrDefault().Latitud,
                    Longitud = db.LogRutaVendedor.Where(y => y.IdVendedor == x.IdVendedor).OrderByDescending(y => y.Fecha).FirstOrDefault().Longitud
                }


                ).Where(x => 
                    x.idEmpresa == vendedorRequest.idEmpresa
                    && x.IdSupervisor == vendedorRequest.IdSupervisor
                    && x.Estado == 1
                ).ToListAsync();

                



                return listaVendedores;
            }
            catch (Exception ex)
            {
                return listaVendedores;
            }
        }


        [HttpPost]
        [Route("VendedorbyEmail")]
        public async Task<VendedorRequest> VendedorByEmail(VendedorRequest vendedorRequest)
        {
            var Vendedor = new VendedorRequest();
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                //var resultAgente = await db.AspNetUsers.Where(x => x.Email == vendedorRequest.Correo).FirstOrDefaultAsync();


                Vendedor = await db.Vendedor.Select(x => new VendedorRequest
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,
                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = x.AspNetUsers.IdEmpresa

                }

                ).Where(x => x.Correo== vendedorRequest.Correo).FirstOrDefaultAsync();


                return Vendedor;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        [Route("ObtenerVendedor")]
        public async Task<Response> ObtenerVendedor(VendedorRequest vendedorRequest)
        {

           
            var vendedor =await db.Vendedor.Where(x => x.IdVendedor == vendedorRequest.IdVendedor)
                        .Select(y=>new VendedorRequest
                        {
                            Identificacion=y.AspNetUsers.Identificacion,
                            NombreApellido=y.AspNetUsers.Nombres +" "+ y.AspNetUsers.Apellidos,
                            Correo=y.AspNetUsers.Email,
                            Telefono=y.AspNetUsers.Telefono,
                            Foto=y.AspNetUsers.Foto,
                            IdVendedor=y.IdVendedor,
                        })
                .FirstOrDefaultAsync();

            if (vendedor!=null)
            {
                return new Response { IsSuccess = true, Resultado = vendedor };
            }
            return new Response { IsSuccess = false};
        }

        // POST: api/Vendedores
        [HttpPost]
        [Route("ListarVendedoresPorSupervisor")]
        public async Task<List<VendedorRequest>> ListarVendedoresPorSupervisor(VendedorRequest vendedorRequest)
        {

            //Necesarios : idEmpresa e idSupervisor
            // solo muestra vendedores con estado 1("Activado")
            var super = db.Supervisor.Where(x => x.IdUsuario == vendedorRequest.IdUsuario).FirstOrDefault();

            var listaVendedores = new List<VendedorRequest>();

            try
            {
                listaVendedores = await db.Vendedor.Select(x => new VendedorRequest
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,

                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = vendedorRequest.idEmpresa

                }

                ).Where(x => 
                    x.idEmpresa == vendedorRequest.idEmpresa 
                    && x.IdSupervisor == vendedorRequest.IdSupervisor
                    && x.Estado == 1
                ).ToListAsync();



                return listaVendedores;
            }
            catch (Exception ex)
            {
                return listaVendedores;
            }
        }

        [HttpPost]
        [Route("ListarVendedoresGerente")]
        public async Task<SupervisorRequest> ListarVendedoresGerente(SupervisorRequest supervisorRequest)
        {
           
            var listaVendedores = new List<VendedorRequest>();

            try
            {
                listaVendedores = await db.Vendedor.Select(x => new VendedorRequest
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,
                    NombreApellido = x.AspNetUsers.Nombres + " " + x.AspNetUsers.Apellidos,
                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = supervisorRequest.IdEmpresa

                }

                ).Where(x =>  x.idEmpresa == supervisorRequest.IdEmpresa
                    && x.Estado == 1
                ).ToListAsync();

                supervisorRequest.ListaVendedores = listaVendedores;

                return supervisorRequest;
            }
            catch (Exception ex)
            {
                return supervisorRequest;
            }
        }
        [HttpPost]
        [Route("ListarVendedoresSupervisor")]
        public async Task<SupervisorRequest> ListarVendedoresSupervisor(SupervisorRequest supervisorRequest)
        {

            var listaVendedores = new List<VendedorRequest>();

            try
            {
                listaVendedores = await db.Vendedor.Select(x => new VendedorRequest
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,
                    NombreApellido = x.AspNetUsers.Nombres + " " + x.AspNetUsers.Apellidos,
                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = supervisorRequest.IdEmpresa

                }

                ).Where(x => x.IdSupervisor == supervisorRequest.IdSupervisor
                    && x.Estado == 1
                ).ToListAsync();

                supervisorRequest.ListaVendedores = listaVendedores;

                return supervisorRequest;
            }
            catch (Exception ex)
            {
                return supervisorRequest;
            }
        }


        // POST: api/Vendedores
        [HttpPost]
        [Route("ListarClientesPorVendedor")]
        public async Task<VendedorRequest> ListarClientesPorVendedor(VendedorRequest vendedorRequest)
        {
            //Necesarios : idEmpresa e idVendedor
            // solo muestra vendedores con estado 1("Activado")

            var vendedor = new VendedorRequest();
            var listaClientes = new List<ClienteRequest>();

            int idEmpresa = Convert.ToInt32( vendedorRequest.idEmpresa );
            //EmpresaActual empresaActual = new EmpresaActual { IdEmpresa = idEmpresa };

            ClientesController ctl = new ClientesController();
            listaClientes = await ctl.ListarClientesPorVendedor( vendedorRequest );

            try
            {
                vendedor = await db.Vendedor.Select(x => new VendedorRequest
                    {
                        IdVendedor = x.IdVendedor,
                        TiempoSeguimiento = x.TiempoSeguimiento,
                        IdSupervisor = x.IdSupervisor,
                        IdUsuario = x.AspNetUsers.Id,
                        NombreApellido = x.AspNetUsers.Nombres+" "+x.AspNetUsers.Apellidos,
                        TokenContrasena = x.AspNetUsers.TokenContrasena,
                        Foto = x.AspNetUsers.Foto,
                        Estado = x.AspNetUsers.Estado,
                        Correo = x.AspNetUsers.Email,
                        Direccion = x.AspNetUsers.Direccion,
                        Identificacion = x.AspNetUsers.Identificacion,
                        Nombres = x.AspNetUsers.Nombres,
                        Apellidos = x.AspNetUsers.Apellidos,
                        Telefono = x.AspNetUsers.Telefono,
                        idEmpresa = vendedorRequest.idEmpresa
                    }

                ).Where(x => x.idEmpresa == vendedorRequest.idEmpresa 
                    && x.IdVendedor == vendedorRequest.IdVendedor
                    && x.Estado == 1
                ).FirstOrDefaultAsync();

               
                vendedor.ListaClientes = listaClientes;
               

                return vendedor;
            }
            catch (Exception ex)
            {
                return vendedor;
            }
        }



        // POST: api/Vendedores
        [HttpPost]
        [Route("InsertarVendedor")]
        public async Task<Response> InsertarVendedor( VendedorRequest vendedorRequest )
        {
            
                try
                {
                
                    Vendedor vendedor = new Vendedor();
                    vendedor.IdUsuario = vendedorRequest.IdUsuario;
                    vendedor.TiempoSeguimiento = vendedorRequest.TiempoSeguimiento;
                    vendedor.IdSupervisor = vendedorRequest.IdSupervisor;

                    db.Vendedor.Add(vendedor);

                    await db.SaveChangesAsync();

                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.GuardadoSatisfactorio,
                        Resultado = vendedor.IdVendedor
                    };

                }

                catch (Exception ex)
                {
                    
                    return new Response

                    {

                        IsSuccess = false,

                        Message = Mensaje.Error,

                    };

                }
            

        }

        // PUT: api/Vendedores

        //[HttpPut("{id}")]
        [Route("EditarVendedor")]
        public async Task<Response> EditarVendedor(VendedorRequest vendedorRequest)
        {
            Response response = new Response();

            db.Configuration.ProxyCreationEnabled = false;


            try
            {
                var modelo = await db.Vendedor.Where(x => x.IdVendedor == vendedorRequest.IdVendedor).FirstOrDefaultAsync();

                if (modelo.TiempoSeguimiento != vendedorRequest.TiempoSeguimiento || modelo.IdSupervisor != vendedorRequest.IdSupervisor) {
                    
                    modelo.TiempoSeguimiento = vendedorRequest.TiempoSeguimiento;
                    modelo.IdSupervisor = vendedorRequest.IdSupervisor;

                    db.Entry(modelo).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                response = new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.GuardadoSatisfactorio
                };

                return response;

            }
            catch(Exception ex)
            {
                response = new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Excepcion + ex,
                    Resultado = null
                };

                return response;

            }
        }


        // POST: api/Vendedores
        [HttpPost]
        [Route("obtenerSupervisorPorIdUsuario")]
        public async Task<Response> obtenerSupervisorPorIdUsuario(SupervisorRequest supervisorRequest)
        {
            try
            {
                
                var supervisor = await db.Supervisor.Where(x => x.AspNetUsers.IdEmpresa == supervisorRequest.IdEmpresa && x.IdUsuario == supervisorRequest.IdUsuario).Select(x => new SupervisorRequest
                {
                    IdUsuario = x.AspNetUsers.Id,
                    IdSupervisor = x.IdSupervisor,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Direccion = x.AspNetUsers.Direccion,
                    Telefono = x.AspNetUsers.Telefono,
                    Correo = x.AspNetUsers.Email,
                    IdEmpresa = x.AspNetUsers.IdEmpresa,
                    IdGerente = x.IdGerente

                }).SingleOrDefaultAsync();


                if (supervisor == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado
                    };

                }
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = supervisor

                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Excepcion
                };
            }

        }

        // POST: api/Vendedores
        [HttpPost]
        [Route("ListarClientesPorSupervisor")]
        public async Task<List<ClienteRequest>> ListarClientesPorSupervisor(VendedorRequest vendedorRequest)
        {

            //Necesita: IdSupervisor
            // solo muestra vendedores con estado 1("Activado")

            var lista = new List<ClienteRequest>();

            try
            {

                lista = await db.Vendedor
                    .Join(db.Cliente
                        , rta => rta.IdVendedor, ind => ind.IdVendedor,
                        (rta, ind) => new { hm = rta, gh = ind })
                        .Join(db.Supervisor
                            , ind_1 => ind_1.hm.IdSupervisor, valor => valor.IdSupervisor,
                            (ind_1, valor) => new { ca = ind_1, rt = valor })
                 .Where(ds =>
                    ds.rt.IdSupervisor == vendedorRequest.IdSupervisor
                    && ds.ca.gh.Estado == 1
                  )
                  .Select(t => new ClienteRequest
                  {
                        IdVendedor=t.ca.hm.IdVendedor,
                        IdCliente=t.ca.gh.idCliente,

                        Nombre = t.ca.gh.Nombre,
                        Apellido = t.ca.gh.Apellido,

                        Latitud = t.ca.gh.Latitud,
                        Longitud = t.ca.gh.Longitud
                  })
                  .ToListAsync();


                return lista;
            }
            catch (Exception ex)
            {
                return lista;
            }
        }


        // POST: api/Vendedores
        [HttpPost]
        [Route("ListarRutaVendedores")]
        public async Task<List<RutaRequest>> ListarRutaVendedores(VendedorRequest vendedorRequest)
        {

            // Necesarios: IdEmpresa e IdVendedor
            // solo muestra vendedores con estado 1("Activado")

            var lista = new List<RutaRequest>();
            var lista2 = new List<RutaRequest>();

            DateTime hoy = DateTime.Now;

            try
            {
                lista = await db.LogRutaVendedor
                    .Join(db.Vendedor, lrv => lrv.IdVendedor, v => v.IdVendedor,(lrv, v) => new { tlrv = lrv, tv = v })
                    .Join(db.AspNetUsers, conjunto => conjunto.tv.IdUsuario, asp => asp.Id,(conjunto, asp) => new { varConjunto = conjunto, tAsp = asp })
                .Where(y => y.tAsp.IdEmpresa == vendedorRequest.idEmpresa && y.tAsp.Estado == 1 && y.varConjunto.tv.IdVendedor == vendedorRequest.IdVendedor)
                .Select(x => new RutaRequest
                {
                    IdLogRutaVendedor = x.varConjunto.tlrv.IdLogRutaVendedor,
                    IdVendedor = x.varConjunto.tlrv.IdVendedor,
                    Fecha = x.varConjunto.tlrv.Fecha,
                    Latitud = x.varConjunto.tlrv.Latitud,
                    Longitud = x.varConjunto.tlrv.Longitud

                }

                ).OrderBy(or =>or.Fecha).ToListAsync();

                
                lista2 = await db.Visita
                    .Join(db.Vendedor, lrv => lrv.IdVendedor, v => v.IdVendedor, (lrv, v) => new { tlrv = lrv, tv = v })
                    .Join(db.AspNetUsers, conjunto => conjunto.tv.IdUsuario, asp => asp.Id, (conjunto, asp) => new { varConjunto = conjunto, tAsp = asp })
                .Where(y => y.tAsp.IdEmpresa == vendedorRequest.idEmpresa && y.tAsp.Estado == 1 && y.varConjunto.tv.IdVendedor == vendedorRequest.IdVendedor)
                .Select(x => new RutaRequest
                {
                    IdLogRutaVendedor = 0,
                    IdVendedor = x.varConjunto.tlrv.IdVendedor,
                    Fecha = x.varConjunto.tlrv.Fecha,
                    Latitud = x.varConjunto.tlrv.Latitud,
                    Longitud = x.varConjunto.tlrv.Longitud

                }

                ).OrderBy(or => or.Fecha).ToListAsync();

                for (int i = 0; i<lista2.Count; i++)
                {
                    lista.Add(lista2.ElementAt(i));
                }

                lista2.Clear();

                for (int i = 0; i<lista.Count; i++)
                {
                    if ( Convert.ToDateTime(lista.ElementAt(i).Fecha).Day == hoy.Day )
                    {
                        lista2.Add(lista.ElementAt(i));
                    }
                }

                
                var lista3 = lista2.OrderBy(t => t.Fecha).ToList();
                
                return lista3;
            }
            catch (Exception ex)
            {
                return lista;
            }
        }




        [HttpPost]
        [Route("BuscarUsuariosVendedoresPorEmpresaEIdentificacion")]
        public async Task<List<VendedorRequest>> BuscarUsuariosVendedoresPorEmpresaEIdentificacion(VendedorRequest vendedorRequest)
        {

            //Necesarios el IdEmpresa e Identificacion

            var listaVendedores = new List<VendedorRequest>();

            try
            {
                listaVendedores = await db.Vendedor.Select(x => new VendedorRequest
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,

                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = vendedorRequest.idEmpresa

                }

                ).Where(x => x.idEmpresa == vendedorRequest.idEmpresa && x.Identificacion == vendedorRequest.Identificacion).ToListAsync();

                return listaVendedores;
            }
            catch (Exception ex)
            {
                return listaVendedores;
            }
        }

    }
}