using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizadorDeTareas.Controllers
{
    public class TareasController : Controller
    {
        // GET: Tareas
        public ActionResult index()
        {
            return View();
        }
    }
}