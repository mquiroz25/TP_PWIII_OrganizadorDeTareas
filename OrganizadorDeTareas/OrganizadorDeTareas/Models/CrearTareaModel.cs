using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrganizadorDeTareas
{
    public class CrearTareaModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Nombre { get; set; }

        [MaxLength(200, ErrorMessage = "Maximo 50 caracteres")]
        public string Descripcion { get; set; }

        [Range(1 , 4, ErrorMessage = "Seleccione una opcion")]
        public short Prioridad { get; set; }

        public int IdCarpeta { get; set; }

        public Nullable<decimal> EstimadoHoras { get; set; }

        public Nullable<System.DateTime> FechaFin { get; set; }

        public List<Carpeta> carpetas { get; set; }

    }
}