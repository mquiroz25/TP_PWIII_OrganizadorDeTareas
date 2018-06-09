using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;


namespace OrganizadorDeTareas.Controllers
{
    public class CarpetasController : Controller
    {
        PW3TP_20181C_TareasEntities CTX = new PW3TP_20181C_TareasEntities();
        // GET: carpetas
        public ActionResult index()
        {
            return View();

        }

        public ActionResult crear()
        {
            return View();

        }

        [HttpPost]
        public ActionResult crear(Carpeta carpeta)

        {
            carpeta.FechaCreacion = DateTime.Now;
            CTX.Carpeta.Add(carpeta);
            CTX.SaveChanges();

            return RedirectToAction("Listar");

        }

        public ActionResult Editar(int id)
        {
            Carpeta carpeta = CTX.Carpeta.FirstOrDefault(o => o.IdCarpeta == id);

            if (carpeta != null)
            {
                return View(carpeta);

            }
            return RedirectToAction("Listar");
        }

        [HttpPost]
        public ActionResult Editar(Carpeta carpeta)

        {
            Carpeta carpActual = CTX.Carpeta.FirstOrDefault(o => o.IdCarpeta == carpeta.IdCarpeta);

            if (carpActual != null)
            {
                carpActual.Nombre = carpeta.Nombre;
                carpActual.Descripcion = carpeta.Descripcion;

                CTX.SaveChanges();
                return RedirectToAction("Listar");
            }

            return View(carpeta);
        }


        public ActionResult Borrar(int id)
        {
            Carpeta carpeta = CTX.Carpeta.FirstOrDefault(o => o.IdCarpeta == id);

            if (carpeta != null)
            {
                CTX.Carpeta.Remove(carpeta);
                CTX.SaveChanges();
            }

            return RedirectToAction("Listar");
        }

        public ActionResult Listar()
        {

            List<Carpeta> carpetas = CTX.Carpeta.ToList();

            return View(carpetas);
        }

    }
}