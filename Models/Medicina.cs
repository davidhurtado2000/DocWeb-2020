using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DocWeb_Prueba.Models
{
    public class Medicina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id_medicina { get; set; }

        [Required]
        public string nombre_medicina { get; set; }

        [Required]
        public string receta_medica { get; set; }

        [Required]
        public int cantidad { get; set; }

        [Required]
        public decimal precio { get; set; }

        [Required]
        public int id_tipomedicina { get; set; }

        [Required]
        public string id_clinica { get; set; }

        public string tipoNombre { get; set; }

     
}
}