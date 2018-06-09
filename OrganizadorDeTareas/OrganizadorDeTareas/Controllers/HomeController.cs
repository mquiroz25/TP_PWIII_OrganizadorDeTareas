using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OrganizadorDeTareas.Controllers
{
    public class HomeController : Controller
    {

        PW3TP_20181C_TareasEntities ctx = new PW3TP_20181C_TareasEntities();
        // GET: Home
        public ActionResult index()
        {
            return View();
        }


        public ActionResult logueado(Usuario u)
        {
            Session["mail"] = u.Email;
            return RedirectToAction("index", "home"); 
        }

        public ActionResult registracion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(Usuario u)
        {
            Usuario nuevo = new Usuario();
            nuevo.Nombre = u.Nombre;
            nuevo.Apellido = u.Apellido;
            nuevo.Contrasenia = u.Contrasenia;
            nuevo.Email = u.Email;
            nuevo.FechaRegistracion = DateTime.Now;
            nuevo.FechaActivacion = DateTime.Now;
            nuevo.CodigoActivacion = "aaaaa";
            nuevo.Activo = 1;
            ctx.Usuario.Add(nuevo);
            ctx.SaveChanges();

            return RedirectToAction("index", "home");
        }

        public ActionResult login()
        {
            return View();
        }

        [ActionName("login")]
        [HttpPost]
        public ActionResult validarLogin()
        {
            String loginMail = Request["Email"];
            String loginPass = Request["Contrasenia"];
            Usuario u = ctx.Usuario.SingleOrDefault(o => o.Email == loginMail && o.Contrasenia == loginPass);
            if (!Object.ReferenceEquals(null, u))
            {
                Session["mail"] = u.Email;
                Session["usuarioid"] = u.IdUsuario;
                return RedirectToAction("index", "home");
            }
            else
            {
                TempData["mensaje"] = "usuario o contraseña incorrecto";
                return View();
            }
            
        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("index", "home");
        }
    }
}