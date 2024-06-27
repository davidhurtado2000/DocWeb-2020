using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DocWeb_Prueba.Models
{
    public class Servicio
    {
        public string id_servicio { get; set; }
        public string nombre_esp { get; set; }
        public decimal precio { get; set; }

        [ForeignKey("Categoria")]
        public Categoria categoria_idcategoria { get; set; }


    }
}
