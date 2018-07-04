using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;



namespace OrganizadorDeTareas.Controllers
{
    public class HomeController : Controller
    {

        PW3TP_20181C_TareasEntities ctx = new PW3TP_20181C_TareasEntities();
        // GET: Home
        public ActionResult index()
        {
            
            if (Session["usuarioid"] == null)
            {
                if (HttpContext.User.Identity.IsAuthenticated == true)
                {
                    Session["usuarioid"] = int.Parse(HttpContext.User.Identity.Name);
                }
            }

            if (Session["usuarioid"] == null)
            {
                return View();
            }
            else
            {
                int sid = (int)Session["usuarioid"];
                ViewBag.nombre = ctx.Usuario.SingleOrDefault(u => u.IdUsuario == sid).Nombre;
                ViewBag.carpeta = ctx.Carpeta.Where(c => c.IdUsuario == sid);
                List<Tarea> tareas = ctx.Tarea.Where(t => t.IdUsuario == sid && t.Completada==0).OrderByDescending(c => c.Prioridad).ToList();
                tareas.OrderBy(c => c.FechaFin);
                ViewBag.tarea = tareas;
                return View("indexlog");
            }
        }


        public ActionResult logueado(Usuario u)
        {
            Session["mail"] = u.Email;
            return RedirectToAction("index", "home"); 
        }

        public ActionResult registracion()
        {
            RegistroModel rm = new RegistroModel();
            return View(rm);
        }

        [HttpPost]
        public ActionResult registracion(RegistroModel u)
        {
            if (ModelState.IsValid)
            {
                if (!this.IsCaptchaValid("verifique el CAPTCHA"))
                {
                    TempData["mensaje"] = "verifique el CAPTCHA";
                    return View(u);
                }

                if (u.Contrasenia != u.vContrasenia)
                {
                    TempData["mensaje"] = "Las contraseñas no coinciden";
                    return View(u);
                }

                if (ctx.Usuario.FirstOrDefault(o => o.Email == u.Email) != null)
                {
                    TempData["mensaje"] = "El usuario ya existe";
                    return View(u);
                }


                Usuario nuevo = new Usuario();
                nuevo.Nombre = u.Nombre;
                nuevo.Apellido = u.Apellido;
                nuevo.Contrasenia = u.Contrasenia;
                nuevo.Email = u.Email;
                nuevo.FechaRegistracion = DateTime.Now;
                nuevo.CodigoActivacion = RandomString.Generar(12);
                nuevo.Activo = 0;
                ctx.Usuario.Add(nuevo);
                ctx.SaveChanges();

                return View("UsuarioCreado",nuevo);
            }
            else
            {
                return View(u);
            }
        }

        public ActionResult Activar(string usuarioId, string codigoActivacion)
        {
            int uid = int.Parse(usuarioId);
            Usuario a = ctx.Usuario.Find(uid);
            if (a != null)
            {
                if (a.Activo == 0)
                {
                    if (a.CodigoActivacion == codigoActivacion)
                    {
                        a.Activo = 1;
                        a.FechaActivacion = DateTime.Now;
                        ctx.SaveChanges();
                        TempData["mensaje"] = "Usuario activado";
                        return RedirectToAction("login", "home");
                    }
                    else
                    {
                        TempData["error"] = "El codigo de activacion no es correcto";
                        return View("error");
                    }
                }
                else
                {
                    TempData["error"] = "Este usuario ya esta activado";
                    return View("error");
                }
            }
            else
            {
                TempData["error"] = "El usuario no existe";
                return View("error");
            }

        }

        public ActionResult login()
        {
            
            if (Session["usuarioid"] == null)
            {
                if (HttpContext.User.Identity.IsAuthenticated == true)
                {
                    Session["usuarioid"] = int.Parse(HttpContext.User.Identity.Name);
                    return RedirectToAction("index", "home");
                }
                else {
                    LoginModel lm = new LoginModel();
                    if (TempData["regreso"] != null)
                    {
                        lm.Regreso = TempData["regreso"].ToString();
                    }
                    return View(lm);
                }
            }
            else
            {
                return RedirectToAction("index", "home");
            }

            
        }

        [ActionName("login")]
        [HttpPost]
        public ActionResult validarLogin(LoginModel lm)
        {
            if (!this.IsCaptchaValid("verifique el CAPTCHA"))
            {
                TempData["mensaje"] = "verifique el CAPTCHA";
                return View();
            }

            if (ModelState.IsValid)
            {
                String loginMail = lm.Email;
                String loginPass = lm.Contrasenia;
                Usuario u = ctx.Usuario.SingleOrDefault(o => o.Email == loginMail && o.Contrasenia == loginPass);
                if (!Object.ReferenceEquals(null, u))
                {
                    if (u.Activo == 1)
                    {
                        Session["usuarioid"] = u.IdUsuario;
                        bool record;
                        if (lm.Recordar == "S")
                        {
                            record = true;
                        }
                        else
                        {
                            record = false;
                        }

                        FormsAuthentication.SetAuthCookie(u.IdUsuario.ToString(), record);

                        if (lm.Regreso != null)
                        {
                            return Redirect(lm.Regreso);
                        }
                        else
                        {
                            return RedirectToAction("index","home");
                        }
                        
                    }
                    else
                    {
                        TempData["mensaje"] = "usuario no activado";
                        return View(lm);
                    }
                }
                else
                {
                    TempData["mensaje"] = "usuario o contraseña incorrecto";
                    return View(lm);
                }


            }else
            {
                return View(lm);
            }

        }

        public ActionResult logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }
    }
}