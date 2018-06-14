using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrganizadorDeTareas
{
    public class CrearCarpetaModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Nombre { get; set; }

        [MaxLength(200, ErrorMessage = "Maximo 50 caracteres")]
        public string Descripcion { get; set; }
    }
}