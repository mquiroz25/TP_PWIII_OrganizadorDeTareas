using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrganizadorDeTareas
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string Contrasenia { get; set; }

        public string Recordar { get; set; }
    }
}