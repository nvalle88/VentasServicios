
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


    [RoutePrefix("api/Clientes")]
    public class ClientesController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();


        [HttpPost]
        [Route("ObtenerTipoClientePorEmpresa")]
        public async Task<List<TipoClienteRequest>> ObtenerTipoClientePorEmpresa(EmpresaActual empresaActual)
        {
            try
            {
                var lista = await db.TipoCliente.Where(x=>x.IdEmpresa==empresaActual.IdEmpresa).Select(x => new TipoClienteRequest
                {
                    idTipoCliente=x.idTipoCliente,
                    Tipo=x.Tipo,

                }).ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                return new List<TipoClienteRequest>();
            }
        }

        // GET: api/Clientes
        [HttpPost]
        [Route("ListarClientes")]
        public async Task<List<ClienteRequest>> ListarClientes(EmpresaActual empresaActual)
        {
            try
            {
                var lista= await db.Cliente.Where(x=>x.Vendedor.AspNetUsers.IdEmpresa==empresaActual.IdEmpresa).Select(x=>new ClienteRequest
                {
                    Apellido=x.Apellido,
                    ApellidosVendedor=x.Vendedor.AspNetUsers.Apellidos,
                    Email=x.Email,
                    Firma=x.Firma,
                    Foto=x.Foto,
                    IdCliente=x.idCliente,
                    IdTipoCliente=x.idTipoCliente,
                    IdVendedor=x.IdVendedor,
                    Latitud=x.Latitud,
                    Longitud=x.Longitud,
                    Nombre=x.Nombre,
                    NombresVendedor=x.Vendedor.AspNetUsers.Nombres,
                    Telefono=x.Telefono,
                    TipoCliente=x.TipoCliente.Tipo,
                    Identificacion=x.Identificacion,
                    Direccion=x.Direccion

                }).ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                return new List<ClienteRequest>();
            }
        }

        [HttpPost]
        [Route("ExisteClientePorEmpresa")]
        public async Task<Response> ExisteClientePorEmpresa(ClienteRequest clienteRequest)
        {
            try
            {
                var cliente = await db.Cliente.
                                             Where(x => x.Vendedor.AspNetUsers.IdEmpresa==clienteRequest.IdEmpresa 
                                                     && x.Identificacion==clienteRequest.Identificacion)
                                             .FirstOrDefaultAsync();

                if (cliente==null)
                {
                    return new Response { IsSuccess = false };
                }
                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }






        // POST: api/Clientes
        [HttpPost]
        [Route("InsertarCliente")]
        public async Task<Response> InsertarCliente(ClienteRequest clienteRequest)
        {

            var cliente = new Cliente
            {
                Apellido=clienteRequest.Apellido,
                Email=clienteRequest.Email,
                Foto=clienteRequest.Foto,
                Identificacion=clienteRequest.Identificacion,
                idTipoCliente=clienteRequest.IdTipoCliente,
                IdVendedor=clienteRequest.IdVendedor,
                Latitud=clienteRequest.Latitud,
                Longitud=clienteRequest.Longitud,
                Nombre=clienteRequest.Nombre,
                Telefono=clienteRequest.Telefono,
                TelefonoMovil=clienteRequest.TelefonoMovil,
            };

            try
            {
                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();
                return new Response {IsSuccess=true, };

            }
            catch (Exception )
            {
                return new Response {IsSuccess=false};

            }
            

        }

       

   



    }
}