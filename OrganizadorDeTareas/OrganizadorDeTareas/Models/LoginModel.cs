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
        [RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessage = "Mail invalido")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string Contrasenia { get; set; }

        public string Recordar { get; set; }
    }
}