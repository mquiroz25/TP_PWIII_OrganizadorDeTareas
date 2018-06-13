using System.ComponentModel.DataAnnotations;

namespace OrganizadorDeTareas
{
    public class UsuarioMetaData

    {
        [Required(ErrorMessage = "Requerido<br/>")]
        [MaxLength(200,ErrorMessage = "Maximo 200 caracteres")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Requerido<br/>")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Nombre;

        [Required(ErrorMessage = "Requerido<br/>")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Apellido;



        [Required(ErrorMessage = "Requerido<br/>")]
        [MaxLength(20, ErrorMessage = "Maximo 20 caracteres")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).+$", ErrorMessage = "No valido")]
        public string Contrasenia { get; set; }

    }
}