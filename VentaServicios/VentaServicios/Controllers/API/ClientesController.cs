using bd.swth.entidades.Utils;
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

namespace VentaServicios.Controllers.API
{


    [RoutePrefix("api/Clientes")]
    public class ClientesController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();


        // GET: api/Clientes
        [HttpGet]
        [Route("ListarClientes")]
        public async Task<List<Cliente>> GetCliente()
        {
            try
            {
                return await db.Cliente.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Cliente>();
            }
        }




        /*

        // POST: api/Clientes
        [HttpPost]
        [Route("InsertarCliente")]
        public async Task<Response> InsertarCliente([FromBody] Cliente cliente)
        {

            Response response = new Response();

            if (!ModelState.IsValid)
            {

            }

            try
            {
                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                
            }

        }

        return response;

    }
    */
    }
}