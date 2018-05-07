using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using VentaServicios.ModeloDato;
using VentaServicios.Utils;

namespace VentaServicios.Controllers.API
{
    [RoutePrefix("api/Informacion")]
    public class InfoController : ApiController
    {
        [Route("Generar")]
        public async Task<Response> GenerarInformacion(InfoRequest informe)
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
                //Send email to Admin  
                WebMail.Send(to: "info@digitalstrategy.com.ec", cc:"bmolina@digitalstrategy.com.ec", subject: "Informacion SaleOut: " , body: "e-mail: " + informe.Mail+" "+ informe.Mensaje, isBodyHtml: true);
                //Send email to User
                var a = new Utils.Template.infoMail();
                string b = a.htmldata();
                WebMail.Send(to: informe.Mail, subject: "Hemos recibido tu correo ", body: b, isBodyHtml: true);

                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
