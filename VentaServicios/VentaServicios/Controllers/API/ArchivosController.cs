using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VentaServicios.ObjectRequest;
using VentaServicios.Utils;

namespace VentaServicios.Controllers.API
{
    [RoutePrefix("Api/Archivos")]
    public class ArchivosController : ApiController
    {
        /// <summary>
        /// Metodo para subir un archivo enviando un objeto de tipo ArchivoRequest que contiene 
        /// Id, Tipo y el Array con el cual podemos verificar si el archivo que se sube es una foto de perfil o una firma y segun el archivo la extension 
        /// </summary>
        /// <param name="archivoRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insertar")]
        public Response Insertar(ArchivoRequest archivoRequest)
        {
            var folder = "";
            var file = string.Format("");

            switch (archivoRequest.Tipo)
            {
                case 1:
                    folder = "~/Content/Usuario/Foto";
                    file = string.Format("{0}.jpg", archivoRequest.Id);
                    break;
                case 2:
                    folder = "~/Content/Usuario/Firma";
                    file = string.Format("{0}.png", archivoRequest.Id);
                    break;
                case 3:
                    folder = "~/Content/Usuario/Vendedor/Foto";
                    file = string.Format("{0}.jpg", archivoRequest.Id);
                    break;
            }

            var stream = new MemoryStream(archivoRequest.Array);            
            var fullPath = string.Format("{0}/{1}", folder, file);
            var response = FileHelper.UploadFoto(stream, folder, file);

            if (!response)
            {
                new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }

           
            return new Response
            {
                IsSuccess = true,
                Message = Mensaje.Satisfactorio,
                Resultado=fullPath
            };

        }

    }
}
