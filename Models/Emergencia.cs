using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocWeb_Prueba.Models
{
    public class Emergencia
    {
        [Key]
        public string Id_Empleado { get; set; }

        [Required(ErrorMessage = "El campo de Nombre es obligatorio")]
        [DisplayName("Nombre")]
        [StringLength(50, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 3)]
        public string Nombre_Empleado { get; set; }

        [Required(ErrorMessage = "El campo de Dni es obligatorio")]
        [DisplayName("Dni")]
        [Range(10000000, 99999999, ErrorMessage = "El numero de dni debe tener 8 digitos")]
        public int DNI_Empleado { get; set; }

        [Required(ErrorMessage = "El campo de Telefono es obligatorio")]
        [DisplayName("Telefono")]
        [StringLength(12, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.")]
        public string Telefono_Empleado { get; set; }

        [Required(ErrorMessage = "El campo de Correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Correo no es un correo valido")]
        [DisplayName("Correo")]
        public string Correo_Empleado { get; set; }

        [Required(ErrorMessage = "El campo de User es obligatorio")]
        [DisplayName("User")]
        [StringLength(100, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.")]
        public string User_Empleado { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        [StringLength(100, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        public string Password_Empleado { get; set; }

        [Required(ErrorMessage = "El campo de Confirmación es obligatorio.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmación de Contraseña")]
        [Compare("Password_Empleado", ErrorMessage = "La contraseña y confirmación de contraseña no son lo mismo")]
        public string Confirmar_Pass_Emple { get; set; }

        [Required(ErrorMessage = "El campo de Foto es obligatorio")]
        [DataType(DataType.Upload)]
        [DisplayName("Foto")]
        public string Foto_Empleado { get; set; }

        public IFormFile file { get; set; }

        [Required(ErrorMessage = "El campo de Id clinica es obligatorio")]
        [DisplayName("Id clinica")]
        public string Id_Clinica { get; set; }

        [Required(ErrorMessage = "El campo de Id sector es obligatorio")]
        [DisplayName("Id sector")]
        public string Id_Sector { get; set; }
    }

}
