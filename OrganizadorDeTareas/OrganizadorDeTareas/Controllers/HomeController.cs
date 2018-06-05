using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OrganizadorDeTareas.Controllers
{
    public class HomeController : Controller
    {
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

        public ActionResult login()
        {
            return View();
        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("index", "home");
        }
    }
}