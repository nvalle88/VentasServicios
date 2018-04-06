using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using VentaServicios.ModeloDato;
using VentaServicios.ObjectRequest;
using VentaServicios.Utils;

namespace VentaServicios.Controllers.API
{

    [RoutePrefix("api/Usuarios")]
    public class AspNetUsersController : ApiController
    {
        private ModelVentas db = new ModelVentas();

        // GET: api/AspNetUsers
        public IQueryable<AspNetUsers> GetAspNetUsers()
        {
            return db.AspNetUsers;
        }

        public int GenerarCodigo(int cuotaInferior, int cuotaSuperior)
        {

            var rdm = new Random();
           var numeroAleatorio= rdm.Next(cuotaInferior,cuotaSuperior);
            return numeroAleatorio;

        }

        [Route("VerificarCodigo")]
        public async Task<Response> VerificarCodigo(RecuperarContrasenaRequest recuperarContrasenaRequest)
        {
            var aspNetUsers = await db.AspNetUsers.Where(x => x.Email == recuperarContrasenaRequest.Email && x.TokenContrasena==recuperarContrasenaRequest.Codigo).FirstOrDefaultAsync();

            if (aspNetUsers == null)
            {
                return new Response { IsSuccess = false };
            }

            aspNetUsers.TokenContrasena = null;
            db.Entry(aspNetUsers).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return new Response { IsSuccess = true };
        }

        [Route("EnviarContrasena")]
        public async Task<Response> EnviarContrasena(RecuperarContrasenaRequest recuperarContrasenaRequest)
        {
            try
            {

                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = CorreoUtil.SmtpServer;
                //gmail port to send emails  
                WebMail.SmtpPort = Convert.ToInt32(CorreoUtil.Port);
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = CorreoUtil.UserName;
                WebMail.Password = CorreoUtil.Password;

                //Sender email address.  
                WebMail.From = CorreoUtil.UserName;

                //Send email  
                WebMail.Send(to: recuperarContrasenaRequest.Email, subject: "Contraseña: " + recuperarContrasenaRequest.Codigo, body: "Contraseña: " + recuperarContrasenaRequest.Codigo, isBodyHtml: true);
                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [Route("GenerarCodigo")]
        public async Task<Response> GenerarCodigo(RecuperarContrasenaRequest recuperarContrasenaRequest)
        {
            try
            {
                var aspNetUsers = await db.AspNetUsers.Where(x => x.Email == recuperarContrasenaRequest.Email).FirstOrDefaultAsync();

                if (aspNetUsers == null)
                {
                    return new Response { IsSuccess = false };
                }

                aspNetUsers.TokenContrasena = Convert.ToString(GenerarCodigo(Constantes.CuotaInferiorCodigo, Constantes.CuotaSuperiorCodigo));
                db.Entry(aspNetUsers).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = CorreoUtil.SmtpServer;
                //gmail port to send emails  
                WebMail.SmtpPort = Convert.ToInt32(CorreoUtil.Port);
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = CorreoUtil.UserName;
                WebMail.Password = CorreoUtil.Password;

                //Sender email address.  
                WebMail.From = CorreoUtil.UserName;

                //Send email  
                WebMail.Send(to: aspNetUsers.Email, subject: "Código: "+aspNetUsers.TokenContrasena, body: "Código: " + aspNetUsers.TokenContrasena,isBodyHtml: true);
                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // PUT: api/AspNetUsers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAspNetUsers(string id, AspNetUsers aspNetUsers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aspNetUsers.Id)
            {
                return BadRequest();
            }

            db.Entry(aspNetUsers).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AspNetUsers
        [ResponseType(typeof(AspNetUsers))]
        public async Task<IHttpActionResult> PostAspNetUsers(AspNetUsers aspNetUsers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUsers.Add(aspNetUsers);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AspNetUsersExists(aspNetUsers.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aspNetUsers.Id }, aspNetUsers);
        }

        // DELETE: api/AspNetUsers/5
        [ResponseType(typeof(AspNetUsers))]
        public async Task<IHttpActionResult> DeleteAspNetUsers(string id)
        {
            AspNetUsers aspNetUsers = await db.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspNetUsers);
            await db.SaveChangesAsync();

            return Ok(aspNetUsers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUsersExists(string id)
        {
            return db.AspNetUsers.Count(e => e.Id == id) > 0;
        }
    }
}