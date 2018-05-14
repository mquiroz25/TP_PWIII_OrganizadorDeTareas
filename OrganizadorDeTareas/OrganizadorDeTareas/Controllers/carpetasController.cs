using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OrganizadorDeTareas.Models;

namespace OrganizadorDeTareas.Controllers
{
    public class CarpetasController : Controller
    {
        // GET: carpetas
        public ActionResult Crear()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Crear(Carpeta carpeta)
        {
         
               var path= Server.MapPath("~/MisCarpetas/"+carpeta.Nombre);

            Directory.CreateDirectory(path);

            return RedirectToAction("MisCarpetasCreadas", "carpetas");//accion,controller
        }

           public ActionResult MisCarpetasCreadas()
        {
        
            return View();

        }
    }
}