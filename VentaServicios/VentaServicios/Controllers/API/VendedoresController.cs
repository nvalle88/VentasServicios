
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
        public async Task<List<VendedorRequest>> ListarVendedores([FromBody] Vendedor Vendedor)
        {

            var listaVendedores = new List<VendedorRequest>();

            try
            {
               // listaVendedores = await db.Vendedor.Select(x => new VendedorRequest 
                //{
                //    IdVendedor = x.IdVendedor,
                //    TiempoSeguimiento = x.TiempoSeguimiento,
                //    IdSupervisor = 0 + (int)(x.IdSupervisor),
                //    IdUsuario = x.AspNetUsers.Id,
                //    TokenContrasena = x.AspNetUsers.TokenContrasena,
                //    Foto = x.AspNetUsers.Foto,
                //    Estado = x.AspNetUsers.Estado,
                //    Contrasena = x.AspNetUsers.Contrasena,
                //    Correo = x.AspNetUsers.Correo,
                //    Direccion = x.AspNetUsers.Direccion,
                //    Identificacion = x.AspNetUsers.Identificacion,
                //    Nombres = x.AspNetUsers.Nombres,
                //    Apellidos = x.AspNetUsers.Apellidos,
                //    Telefono = x.AspNetUsers.Telefono                    


                //}
                    
                //).ToListAsync();



                return listaVendedores;
            }
            catch (Exception ex)
            {
                return listaVendedores;
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