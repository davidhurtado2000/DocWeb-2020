using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DocWeb_Prueba.Models
{
    public class Doctor
    {


        [Key]
        [DisplayName("Código de Doctor")]
        public string id_doctor { get; set; }

        [Required(ErrorMessage = "El campo de Nombre es obligatorio")]
        [DisplayName("Nombre")]
        [StringLength(50, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 3)]
        public string nombre_doctor { get; set; }

        [Required(ErrorMessage = "El campo de Dni es obligatorio")]
        [DisplayName("Dni")]
        [Range(10000000, 99999999, ErrorMessage = "El numero de dni debe tener 8 digitos")]
        public decimal Dni_Doctor { get; set; }

        [Required(ErrorMessage = "El campo de Telefono es obligatorio")]
        [DisplayName("Telefono")]
        [StringLength(12, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.")]
        public string telef_doctor { get; set; }

        [Required(ErrorMessage = "El campo de Correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Correo no es un correo valido")]
        [DisplayName("Correo")]
        public string CorreoDoctor { get; set; }

        [Required(ErrorMessage = "El campo de User es obligatorio")]
        [DisplayName("User")]
        [StringLength(100, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.")]
        public string user_doctor { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        [StringLength(100, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        public string doctor_password { get; set; }

        [Required(ErrorMessage = "El campo de Confirmación es obligatorio.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmación de Contraseña")]
        [Compare("doctor_password", ErrorMessage = "La contraseña y confirmación de contraseña no son lo mismo")]
        public string ConfirmPasword { get; set; }

        [Required(ErrorMessage = "El campo de Foto es obligatorio")]
        [DataType(DataType.Upload)]
        [DisplayName("Foto")]
        public string foto_doctor { get; set; }

        public IFormFile file { get; set; }

        [Required(ErrorMessage = "El campo de Id_Clinica es obligatorio")]
        [DisplayName("Código de Clinica")]
        public string id_clinica { get; set; }

        [ForeignKey("Horario")]
        [Required(ErrorMessage = "El campo de Horario es obligatorio")]
        [DisplayName("Código de Horario")]
        public string id_horario { get; set; }

        [ForeignKey("Especialidad")]
        [Required(ErrorMessage = "El campo de Id_Especialidad es obligatorio")]
        [DisplayName("Código de Especialidad")]
        public string Id_especialidad { get; set; }
    }
}
