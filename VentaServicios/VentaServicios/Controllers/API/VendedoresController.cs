
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



        
        // GET: api/Vendedores
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


        // GET: api/Vendedores
        [HttpPost]
        [Route("ListarClientesPorVendedor")]
        public async Task<VendedorRequest> ListarClientesPorVendedor(VendedorRequest vendedorRequest)
        {
            //Necesarios : idEmpresa e idVendedor
            // solo muestra vendedores con estado 1("Activado")

            var vendedor = new VendedorRequest();
            var listaClientes = new List<ClienteRequest>();

            int idEmpresa = Convert.ToInt32( vendedorRequest.idEmpresa );
            EmpresaActual empresaActual = new EmpresaActual { IdEmpresa = idEmpresa };

            ClientesController ctl = new ClientesController();
            listaClientes = await ctl.ListarClientes( empresaActual );

            


            try
            {
                vendedor = await db.Vendedor.Select(x => new VendedorRequest
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
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    Vendedor vendedor = new Vendedor();
                    vendedor.IdUsuario = vendedorRequest.IdUsuario;
                    vendedor.TiempoSeguimiento = vendedorRequest.TiempoSeguimiento;
                    vendedor.IdSupervisor = vendedorRequest.IdSupervisor;

                    db.Vendedor.Add(vendedor);

                    await db.SaveChangesAsync();
                    transaction.Commit();

                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.GuardadoSatisfactorio
                    };

                }

                catch (Exception ex)

                {

                    transaction.Rollback();

                    return new Response

                    {

                        IsSuccess = false,

                        Message = Mensaje.Error,

                    };

                }

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



        [HttpPost]
        [Route("obtenerSupervisorPorIdUsuario")]
        public async Task<Response> obtenerSupervisorPorIdUsuario(SupervisorRequest supervisorRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

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



    }
}