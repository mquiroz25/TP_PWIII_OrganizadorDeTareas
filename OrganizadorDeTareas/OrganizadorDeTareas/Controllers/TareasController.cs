using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

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
                TempData["regreso"] = "/Tareas/";
                return RedirectToAction("login", "home");
            }
        }

        public ActionResult nueva(int? id) {
            if (Session["usuarioid"] != null)
            {
                int sid = (int)Session["usuarioid"];
                if (id != null) {
                    Carpeta carpeta = ctx.Carpeta.FirstOrDefault(o => o.IdCarpeta == id);
                    ViewBag.Nombre = carpeta.Nombre;
                    
                    CrearTareaModel ct = new CrearTareaModel();
                    ct.carpetas = ctx.Carpeta.Where(o => o.IdUsuario == sid).ToList<Carpeta>();
                    ct.IdCarpeta= carpeta.IdCarpeta;
                    return View(ct);
                }
                else
                {
                    CrearTareaModel ct = new CrearTareaModel();
                    ct.carpetas = ctx.Carpeta.Where(o => o.IdUsuario == sid).ToList<Carpeta>();
                    return View(ct);
                }
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                TempData["regreso"] = "/Tareas/nueva/";
                if (id != null)
                {
                    TempData["regreso"] = TempData["regreso"] + id.ToString();
                }
                return RedirectToAction("login", "home");
            }
        }
        
        public ActionResult Listar(int? id)
        {


            if (Session["usuarioid"] != null) {
                int sid = (int)Session["usuarioid"];
                var query = ctx.Tarea.Where(t => t.IdUsuario == sid);

                ViewBag.carpeta = ctx.Carpeta.Where(c => c.IdUsuario == sid);
                if (id != null)
                {
                    ViewBag.idCarpeta = id;
                    query = query.Where(t => t.IdCarpeta == id);
                }

                if (Request["completadas"] == "C")
                {
                    query = query.Where(t => t.Completada == 1);
                }

                if (Request["completadas"] == "N")
                {
                    query = query.Where(t => t.Completada == 0);
                }
                List < Tarea > tareas = query.ToList();
                tareas.OrderByDescending(c => c.Prioridad);
                tareas.OrderBy(c => c.FechaFin);
                ViewBag.tarea = tareas;

                return View();

            }
            else
            {
                TempData["mensaje"] = "login requerido";
                TempData["regreso"] = "/Tareas/listar/";
                if (id != null)
                {
                    TempData["regreso"] = TempData["regreso"] + id.ToString();
                }
                return RedirectToAction("login", "home");
            }
        }

        public ActionResult Detalle(int id)
        {
            if (Session["usuarioid"] != null)
            {
                Tarea tarea = ctx.Tarea.Find(id);
                Carpeta carpeta = ctx.Carpeta.FirstOrDefault(o => o.IdCarpeta == tarea.IdCarpeta);

    
                ViewBag.carpeta = carpeta.Nombre;
                ViewBag.tarea = tarea;
                ViewBag.id = id;
                ViewBag.comentarios = ctx.ComentarioTarea.Where(o => o.IdTarea == id).ToList<ComentarioTarea>();
                ViewBag.adjuntos = ctx.ArchivoTarea.Where(o => o.IdTarea == id).ToList<ArchivoTarea>();
                return View();
            }
            else
            {
                TempData["mensaje"] = "login requerido";
                TempData["regreso"] = "/Tareas/detalle/" + id;
                return RedirectToAction("login", "home");
            }
        }

        [HttpPost]
        public ActionResult nueva(CrearTareaModel t)
        {
            if (ModelState.IsValid)
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

                return RedirectToAction("listar/" + t.IdCarpeta, "tareas");


            }
            else
            {
                int sid = (int)Session["usuarioid"];
                t.carpetas = ctx.Carpeta.Where(o => o.IdUsuario == sid).ToList<Carpeta>();
                return View(t);
            }
        }

        [HttpPost]
        public ActionResult Comentario(ComentarioTarea c)
        {
            ComentarioTarea nuevo = new ComentarioTarea();
            nuevo.IdTarea = c.IdTarea;
            nuevo.Texto = c.Texto;
            nuevo.FechaCreacion = DateTime.Now;
            ctx.ComentarioTarea.Add(nuevo);
            ctx.SaveChanges();
            return RedirectToAction("Detalle/" + c.IdTarea, "Tareas");
        }

        [HttpPost]
        public ActionResult Adjunto(HttpPostedFileBase file)
        {
            int idTarea = int.Parse(Request["idTarea"]);
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fname = Path.GetFileNameWithoutExtension(file.FileName)
                        + "_" + RandomString.Generar(5)
                        + Path.GetExtension(file.FileName);
                    if (!Directory.Exists(Server.MapPath("/Archivos/Tareas/" + idTarea + "/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/Archivos/Tareas/" + idTarea + "/"));
                    }
                    string path = Path.Combine(Server.MapPath("/Archivos/Tareas/"+idTarea+"/"),Path.GetFileName(fname));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    ArchivoTarea at = new ArchivoTarea();
                    at.IdTarea = idTarea;
                    at.RutaArchivo = "/Archivos/Tareas/" + idTarea + "/" + fname;
                    at.FechaCreacion = DateTime.Now;
                    ctx.ArchivoTarea.Add(at);
                    ctx.SaveChanges();
                    return RedirectToAction("Detalle/" + idTarea, "Tareas");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    return RedirectToAction("Detalle/" + idTarea, "Tareas");
                }
            }
            else
            {
                ViewBag.Message = "No has seleccionado un archivo";
                return RedirectToAction("index", "home");
            }
            
        }


        public ActionResult Completar (int id)
        {
            Tarea t = ctx.Tarea.SingleOrDefault(o => o.IdTarea == id);
            if (t != null)
            {
                t.Completada = 1;
                ctx.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }




    }
}