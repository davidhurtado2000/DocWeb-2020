using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DocWeb_Prueba.Models
{
    public class Reserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_reserva { get; set; }

        [Required]
        public string hora { get; set; }


        [ForeignKey("Doctor")]
        public Doctor id_doctor { get; set; }

        public string id_servicio { get; set; }

        public int id_cliente { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public string fecha_reserva { get; set; }

        public string nombreCliente { get; set; }
        public string nombreServicio { get; set; }
    }
}
