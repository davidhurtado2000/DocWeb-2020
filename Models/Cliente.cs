using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DocWeb_Prueba.Models
{

    
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Cliente { get; set; }

        [Required(ErrorMessage = "El campo de Nombre es obligatorio")]
        [DisplayName("Nombre")]
        [StringLength(50, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 3)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo de Apellidos es obligatorio")]
        [DisplayName("Apellidos")]
        [StringLength(50, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 3)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo de Dni es obligatorio")]
        [DisplayName("Dni")]
        [Range(10000000, 99999999, ErrorMessage = "El numero de dni debe tener 8 digitos")]
        public decimal Dni_Cliente { get; set; }

        [Required(ErrorMessage = "El campo de Telefono es obligatorio")]
        [DisplayName("Telefono")]
        [StringLength(12, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.")]
        public string telef_Cliente { get; set; }

        [Required(ErrorMessage = "El campo de Correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Correo no es un correo valido")]
        [DisplayName("Correo")]
        public string CorreoCliente { get; set; }

        [Required(ErrorMessage = "El campo de User es obligatorio")]
        [DisplayName("User")]
        [StringLength(100, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.")]
        public string user_cliente { get; set; } 

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        [StringLength(100, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        public string user_password { get; set; }

        [Required(ErrorMessage = "El campo de Confirmación es obligatorio.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmación de Contraseña")]
        [Compare("user_password", ErrorMessage = "La contraseña y confirmación de contraseña no son lo mismo")]
        public string ConfirmPasword { get; set; }

        [Required(ErrorMessage = "El campo de Foto es obligatorio")]
        [DataType(DataType.Upload)]
        [DisplayName("Foto")]
        public string foto_cliente { get; set; }

        public IFormFile file { get; set; }

        [Required(ErrorMessage = "El campo de Fecha de Nacimiento es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Nacimiento")]
        public DateTime fecha_nacimiento { get; set; }

        [Required(ErrorMessage = "El campo de Seguro es obligatorio")]
        [DisplayName("Seguro")]
        public string seguro { get; set; }

        [Required(ErrorMessage = "El campo de Edad es obligatorio")]
        [Range(18,100, ErrorMessage = "El registro es solo para personas mayores de 18 años")]
        [DisplayName("Edad")]
        public int edad { get; set; }
    
    }

}
