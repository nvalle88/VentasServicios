
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
                    idEmpresa = vendedorRequest.idEmpresa

                }

                ).Where(x => x.Correo== vendedorRequest.Correo).FirstOrDefaultAsync();


                return Vendedor;
            }
            catch (Exception ex)
            {
                return null;
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
                        IdSupervisor = 0 + (int)(x.IdSupervisor),
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

            var vendedor = new Vendedor();
            vendedor.IdVendedor =  vendedorRequest.IdVendedor;
            vendedor.TiempoSeguimiento = vendedorRequest.TiempoSeguimiento;
            vendedor.IdSupervisor = vendedorRequest.IdSupervisor;
            vendedor.IdUsuario = vendedorRequest.IdUsuario;


            try
            {

                db.Entry(vendedor).State = EntityState.Modified;
                await db.SaveChangesAsync();

                response = new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.GuardadoSatisfactorio,
                    Resultado = vendedor
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





    }
}