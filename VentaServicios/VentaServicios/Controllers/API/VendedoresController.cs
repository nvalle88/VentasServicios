
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
                    //var user = new  AspNetUsers();
                    //user.IdEmpresa = supervisorRequest.IdEmpresa;
                    //user.Identificacion = supervisorRequest.Identificacion;
                    //user.Apellidos = supervisorRequest.Apellidos;
                    //user.Nombres = supervisorRequest.Nombres;
                    //user.Direccion = supervisorRequest.Direccion;
                    //user.Telefono = supervisorRequest.Telefono;
                    //user.Email = supervisorRequest.Correo;
                    //user.UserName = supervisorRequest.Correo;
                    //user.PasswordHash = "123";
                    //db.AspNetUsers.Add(user);
                    //await db.SaveChangesAsync();

                    /*
                    var gerente = db.Gerente.Where(x => x.AspNetUsers.IdEmpresa == supervisorRequest.IdEmpresa);
                    if (gerente != null)
                    {
                        var super = new Supervisor();
                        super.IdGerente = gerente.FirstOrDefault().IdGerente;
                        super.IdUsuario = supervisorRequest.IdUsuario;
                        db.Supervisor.Add(super);

                    }
                    */

                    Vendedor vendedor = new Vendedor();
                    vendedor.IdUsuario = vendedorRequest.IdUsuario;
                    vendedor.TiempoSeguimiento = vendedorRequest.TiempoSeguimiento;

                    db.Vendedor.Add(vendedor);

                    await db.SaveChangesAsync();
                    transaction.Commit();

                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio
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
        public async Task<Response> EditarVendedor([FromBody] Vendedor Vendedor )
        {
            Response response = new Response();

            try
            {

                db.Entry(Vendedor).State = EntityState.Modified;
                await db.SaveChangesAsync();

                response = new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = Vendedor
                };

                return response;

            }
            catch(Exception ex)
            {
                response = new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Excepcion,
                    Resultado = null
                };

                return response;

            }
        }





    }
}