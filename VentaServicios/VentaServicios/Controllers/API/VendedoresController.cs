
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
using VentaServicios.Utils;

namespace VentaServicios.Controllers.API
{


    [RoutePrefix("api/Vendedores")]
    public class VendedoresController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();


        // GET: api/Vendedores
        [HttpGet]
        [Route("ListarVendedores")]
        public async Task<List<Vendedor>> ListarVendedores()
        {
            try
            {
                return await db.Vendedor.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Vendedor>();
            }
        }




        

        // POST: api/Vendedores
        [HttpPost]
        [Route("InsertarVendedor")]
        public async Task<Response> InsertarVendedor([FromBody] Vendedor Vendedor)
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
                db.Vendedor.Add(Vendedor);
                await db.SaveChangesAsync();

                response = new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = Vendedor
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