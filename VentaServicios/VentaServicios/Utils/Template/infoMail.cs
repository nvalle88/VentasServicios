using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Web;

namespace VentaServicios.Utils.Template
{
    public class infoMail
    {
        public string htmldata()
        {
            //  var direccion = Path.Combine(Environment.CurrentDirectory, "\\Utils\\Template\\InfoMail.html");
            string path = System.Web.HttpContext.Current.Request.MapPath("~\\Utils\\Template\\InfoMail.html");
            string readText = File.ReadAllText(path);
            Console.WriteLine(readText);
            return readText;
        }
    }
}