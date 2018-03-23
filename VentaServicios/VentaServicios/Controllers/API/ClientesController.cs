
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

                }).ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                return new List<ClienteRequest>();
            }
        }




        

        // POST: api/Clientes
        [HttpPost]
        [Route("InsertarCliente")]
        public async Task<Response> InsertarCliente([FromBody] Cliente cliente)
        {

            Response response = new Response();

            if (!ModelState.IsValid)
            {
                response = new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ModeloInvalido,
                    Resultado = null
                };

                return response;
            }

            try
            {
                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();

                response = new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = cliente
                };

                return response;

            }
            catch (Exception ex)
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