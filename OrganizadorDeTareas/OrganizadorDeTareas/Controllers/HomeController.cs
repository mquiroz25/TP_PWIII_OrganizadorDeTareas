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
            
            return View();
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
            else
            {
                return View(u);
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

                    return RedirectToAction("index", "home");
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