using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizadorDeTareas.Controllers
{
    public class TareasController : Controller
    {
        PW3TP_20181C_TareasEntities ctx = new PW3TP_20181C_TareasEntities();
        // GET: Tareas
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

        public ActionResult nueva(int id) {
            if (Session["usuarioid"] != null)
            {
                Carpeta carpeta = ctx.Carpeta.FirstOrDefault(o => o.IdCarpeta == id);
                return View(carpeta);
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }
        }


        public ActionResult Listar(int id)
        {
            if (Session["usuarioid"] != null) {
                ViewBag.tareas = ctx.Tarea.Where(o => o.IdCarpeta == id).ToList<Tarea>();
                ViewBag.nombreCarpeta = ctx.Carpeta.Find(id).Nombre;
                ViewBag.idCarpeta = id;
                return View();
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }
        }

        public ActionResult Detalle(int id)
        {
            if (Session["usuarioid"] != null)
            {
                Tarea tarea = ctx.Tarea.Find(id);
                ViewBag.carpeta = ctx.Carpeta.Find(tarea.IdCarpeta).Nombre;
                ViewBag.tarea = tarea;
                return View();
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                return RedirectToAction("login", "home");
            }
        }

        [HttpPost]
        public ActionResult Crear(Tarea t)
        {
            Tarea nueva = new Tarea();
            nueva.IdUsuario = (int)Session["usuarioid"];
            nueva.IdCarpeta = t.IdCarpeta;
            nueva.FechaCreacion = DateTime.Now;
            nueva.Nombre = t.Nombre;
            nueva.Descripcion = t.Descripcion;
            nueva.EstimadoHoras = t.EstimadoHoras;
            nueva.FechaFin = t.FechaFin;
            nueva.Prioridad = t.Prioridad;
            nueva.Completada = 0;
            ctx.Tarea.Add(nueva);
            ctx.SaveChanges();
            return RedirectToAction("listar", t.IdCarpeta + "tareas/" );
        }
    }
}