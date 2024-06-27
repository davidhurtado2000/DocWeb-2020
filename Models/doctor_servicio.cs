using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DocWeb_Prueba.Models
{
    public class doctor_servicio
    {
        [ForeignKey("Doctor")]
        public Doctor id_doctor { get; set; }

        [ForeignKey("Servicio")]
        public Servicio id_servicio { get; set; }


    }

}
