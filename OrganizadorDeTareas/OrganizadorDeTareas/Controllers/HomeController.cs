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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logueado()
        {
            return View();
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
            return RedirectToAction("Index", "Home");
        }
    }
}