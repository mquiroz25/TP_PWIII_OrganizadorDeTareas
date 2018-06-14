using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrganizadorDeTareas
{
    public class RegistroModel
    {
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessage = "Mail invalido")]
        [MaxLength(200, ErrorMessage = "Maximo 200 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(20, ErrorMessage = "Maximo 20 caracteres")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).+$", ErrorMessage = "No valido")]
        public string Contrasenia { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(20, ErrorMessage = "Maximo 20 caracteres")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).+$", ErrorMessage = "No valido")]
        public string vContrasenia { get; set; }
    }
}