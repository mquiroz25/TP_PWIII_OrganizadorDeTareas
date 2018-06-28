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
            if (Session["usuarioid"] != null)
            {
                return View();
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }

        }

        public ActionResult crear()
        {
            if (Session["usuarioid"] != null)
            {
                CrearCarpetaModel cc = new CrearCarpetaModel();
                return View(cc);
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }

        }

        [HttpPost]
        public ActionResult crear(CrearCarpetaModel cc)

        {
            if (Session["usuarioid"] != null)
            {
                if (ModelState.IsValid)
                {
                    Carpeta carpeta = new Carpeta();
                    carpeta.Nombre = cc.Nombre;
                    carpeta.Descripcion = cc.Descripcion;
                    carpeta.FechaCreacion = DateTime.Now;
                    carpeta.IdUsuario = (int)Session["usuarioid"];
                    CTX.Carpeta.Add(carpeta);
                    CTX.SaveChanges();
                    return RedirectToAction("Listar");
                }
                else
                {
                    return View(cc);
                }
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }
        }

        public ActionResult Editar(int id)
        {
            if (Session["usuarioid"] != null)
            {
                Carpeta carpeta = CTX.Carpeta.FirstOrDefault(o => o.IdCarpeta == id);

                if (carpeta != null)
                {
                    return View(carpeta);

                }
                return RedirectToAction("Listar");
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }
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
            if (Session["usuarioid"] != null)
            {
                Carpeta carpeta = CTX.Carpeta.FirstOrDefault(o => o.IdCarpeta == id);

                if (carpeta != null)
                {
                    if (carpeta.IdUsuario == (int)Session["usuarioid"])
                    {
                        CTX.Carpeta.Remove(carpeta);
                        CTX.SaveChanges();
                    }
                }

                return RedirectToAction("Listar");
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }



        }

        public ActionResult Listar()
        {
            if (Session["usuarioid"] != null)
            {
                List<Carpeta> carpetas = CTX.Carpeta.OrderBy(c => c.Nombre).ToList();
                carpetas = carpetas.Where(o => o.IdUsuario == (int)Session["usuarioid"]).ToList<Carpeta>();

                return View(carpetas);
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }

        }

    }
}