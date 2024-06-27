using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DocWeb_Prueba.Models;
using System.Data.SqlClient;
using DocWeb_Prueba.Util;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DocWeb_Prueba.Controllers
{
    public class DoctorController : Controller
    {
        public SqlConnection con;
        public SqlTransaction transaccion;
        public string error;
        private readonly IWebHostEnvironment _env;
        List<Reserva> listado;
        List<Reserva> ListadoReserva;
        List<Doctor> ListadoDoctor;
        List<Medicina> ListadoMedicina;
        const string SessionUser = "_user";
        const string SessionPass = "_pass";
        const string SessionIdDoctor = "_doctor";

        [Obsolete]
        public DoctorController(IWebHostEnvironment env)
        {
            this.con = Conexion.getConexion();
            _env = env;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        public ActionResult RegistroExito()
        {
            return View();
        }

        public ActionResult Listado()
        {
            return View(ListarReserva());
        }

        public ActionResult DoctorPage()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            ViewBag.Id = HttpContext.Session.GetString(SessionIdDoctor);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {
                Verificacion2(ViewBag.Name, ViewBag.Age);
                return View();
            }
        }

        public ActionResult ListarReserva()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            ViewBag.Id = HttpContext.Session.GetString(SessionIdDoctor);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == "" || ViewBag.Age == "" || ViewBag.Id == null)
            {
                return View("AccesoDenegado");
            }
            else
            {
                Verificacion2(ViewBag.Name, ViewBag.Age);
                return View(mostrarReservaDelDia());
            }
        }

        public ActionResult ListarMedicina()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {
                Verificacion2(ViewBag.Name, ViewBag.Age);
                return View(mostrarMedicina());
            }
        }

        public ActionResult DatosPersonalesDoctor()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {
                string user = HttpContext.Session.GetString(SessionUser);
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con;
                com.CommandText = "select * from doctor where doc_user='" + user + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {

                    ViewData["Nombres"] = dr.GetString(1);
                    ViewData["Dni_Doctor"] = dr.GetDecimal(2);
                    ViewData["telef_doctor"] = dr.GetString(3);
                    ViewData["CorreoDoctor"] = dr.GetString(4);
                    ViewData["user"] = dr.GetString(5); 
                    Verificacion2(ViewBag.Name, ViewBag.Age);
                    return View();
                }
                else
                {
                    return View("AccesoDenegado");
                }
            }
        }

        public ActionResult ModificarDoctor()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {
                string user = HttpContext.Session.GetString(SessionUser);
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con;
                com.CommandText = "select * from doctor where doc_user='" + ViewBag.Name + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    ViewData["Nombres"] = dr.GetString(1);
                    ViewData["Dni_Doctor"] = dr.GetDecimal(2);
                    ViewData["telef_doctor"] = dr.GetString(3);
                    ViewData["CorreoDoctor"] = dr.GetString(4);
                    ViewData["user"] = dr.GetString(5);
                }
                Verificacion2(ViewBag.Name, ViewBag.Age);
                return View();
            }
        }

        [HttpPost]
        public IActionResult ModificoDoctor(Doctor doctor)
        {
            string user = HttpContext.Session.GetString(SessionUser);
            string contraseña = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {

                //Aqui empieza la secuencia para insertar a la base de datos
                SqlCommand com = new SqlCommand();

                com.Connection = con;
                com.CommandText = "UPDATE doctor  SET nombre_doctor = @nombre, dni= @dni, telef_doc = @telefono, correo_doc = @correo, " +
                    "doc_user= @user, doc_pass = @password" +
                    " where doc_user = '" + user + "' and doc_pass = '" + contraseña + "'";


                com.Parameters.AddWithValue("@nombre", doctor.nombre_doctor);
                com.Parameters.AddWithValue("@dni", doctor.Dni_Doctor);
                com.Parameters.AddWithValue("@telefono", doctor.telef_doctor);
                com.Parameters.AddWithValue("@correo", doctor.CorreoDoctor);
                com.Parameters.AddWithValue("@user", doctor.user_doctor);
                com.Parameters.AddWithValue("@password", doctor.doctor_password);

                //Try Catch para detectar algun error en el registro
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.error = ex.Message;
                    ViewData["Mensaje_error"] = error;
                    return View("ModificarDoctor");
                }
                Verificacion2(user, contraseña);
                return View("DoctorPage");
            }
        }

        [HttpPost]
        public ActionResult Verificacion(Doctor doctor)
        {
           
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from doctor where doc_user='" + doctor.user_doctor + "' and doc_pass='" + doctor.doctor_password + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                //Asignamos los valores de la base de datos a los ViewData
                doctor.id_doctor = dr.GetString(0);
                doctor.nombre_doctor = dr.GetString(1);
                doctor.foto_doctor = dr.GetString(7);
                ViewData["NombreCompleto"] = "Doctor: " + doctor.nombre_doctor + "Id " + doctor.id_doctor;
                ViewData["foto"] = doctor.foto_doctor;

                //con.Close();
                HttpContext.Session.SetString(SessionUser, doctor.user_doctor);
                HttpContext.Session.SetString(SessionPass, doctor.doctor_password);
                return View("DoctorPage");
            }
            else
            {
                //con.Close();
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Verificacion2(string usuario, string contraseña)
        {
           
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from doctor where doc_user='" + usuario + "' and doc_pass='" + contraseña + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                Doctor doctor = new Doctor();

                doctor.id_doctor = dr.GetString(0);
                doctor.nombre_doctor = dr.GetString(1);
                doctor.foto_doctor = dr.GetString(7);
                ViewData["NombreCompleto"] = "Doctor: " + doctor.nombre_doctor;
                ViewData["foto"] = doctor.foto_doctor;


                HttpContext.Session.SetString(SessionUser, usuario);
                HttpContext.Session.SetString(SessionPass, contraseña);
                HttpContext.Session.SetString(SessionIdDoctor, doctor.id_doctor);
                return View("DoctorPage");
            }
            else
            {
                ViewData["Error"] = "Usuario o contraseña incorrectos";
                return View("Login");
            }
        }




       public List<Reserva> mostrarReservaDelDia()
         {
            string id_doctor = ViewBag.Id;
             listado = new List<Reserva>();
             SqlCommand com = new SqlCommand();
             SqlDataReader dr;
             com.Connection = con;
             com.CommandText = "select id_reserva, fecha_reserva, hora, c.nombres_cliente, s.nombre_esp from reserva r JOIN cliente c ON r.cliente_id_cliente = c.id_cliente " +
                "JOIN servicio_especialidad s ON r.id_servicio = s.id_servicio " +
                " where fecha_reserva = (select convert(varchar, getdate(), 1)) and doctor_id_doctor='"+id_doctor+"' order by hora asc";
             dr = com.ExecuteReader();
             try
             {
                 while (dr.Read())
                 {
                     Reserva reserva = new Reserva();
                     reserva.id_reserva = dr.GetInt32(0);
                     reserva.fecha_reserva = dr.GetDateTime(1).ToString("dd-MM-yyyy");
                     reserva.hora = dr.GetString(2);
                     reserva.nombreCliente = dr.GetString(3);
                     reserva.nombreServicio = dr.GetString(4);
                     listado.Add(reserva);
                 }
             }
             catch (Exception e)
             {
                this.error = e.Message;
                ViewData["Error"] = error;
                return listado;
             }
             return listado;

         }
        public List<Medicina> mostrarMedicina()
        {
            ListadoMedicina = new List<Medicina>();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select id_medicina, nombre_medicina, receta_medica, cantidad, precio ,tp.caracterisitica, clinica_id_clinica from medicina m " +
                " JOIN tipo_medicina tp ON m.id_tipomedicina = tp.id_tipomedicina";
            dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Medicina med = new Medicina();
                    med.id_medicina = dr.GetString(0);
                    med.nombre_medicina = dr.GetString(1);
                    med.receta_medica = dr.GetString(2);
                    med.cantidad = dr.GetInt32(3);
                    med.precio = dr.GetDecimal(4);
                    med.tipoNombre = dr.GetString(5);
                    med.id_clinica = dr.GetString(6);
                    ListadoMedicina.Add(med);
                }
            }
            catch (Exception e)
            {
                this.error = e.Message;
                ViewData["Error"] = error;
                return ListadoMedicina;
            }
            return ListadoMedicina;
        }



            [HttpPost]

            public async Task<IActionResult> registrarDoctor(Doctor doctor, IFormFile file)
            {
                string mensaje_Error = "";
                string imgext = Path.GetExtension(file.FileName);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    //Codigo que permite registrar la imagen en la carpeta de Foto en root
                    var dir = _env.WebRootPath;
                    var saveimg = Path.Combine(dir, "img/Fotos", file.FileName);
                    using (var stream = new FileStream(saveimg, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    //stream.Flush();

                    //Asignamos el nombre de la carpeta

                    //Aqui empieza la secuencia para insertar a la base de datos
                    SqlCommand com = new SqlCommand();

                    com.Connection = con;
                    com.CommandText = "INSERT INTO doctor (id_doctor, nombre_doctor, dni, telef_doc, correo_doc, doc_user, doc_pass, doc_foto, clinica_id_clinica, horario_id_horario, id_especialidad)" +
                        " VALUES (@id,@nombre,@dni,@telefono,@correo,@user,@password,@foto,@clinica,@horario,@especialidad)";
                    doctor.foto_doctor = file.FileName.ToString();
                    com.Parameters.AddWithValue("@id", doctor.id_doctor);
                    com.Parameters.AddWithValue("@nombre", doctor.nombre_doctor);
                    com.Parameters.AddWithValue("@dni", doctor.Dni_Doctor);
                    com.Parameters.AddWithValue("@telefono", doctor.telef_doctor);
                    com.Parameters.AddWithValue("@correo", doctor.CorreoDoctor);
                    com.Parameters.AddWithValue("@user", doctor.user_doctor);
                    com.Parameters.AddWithValue("@password", doctor.doctor_password);
                    com.Parameters.AddWithValue("@foto", doctor.foto_doctor);
                    com.Parameters.AddWithValue("@clinica", doctor.id_clinica);
                    com.Parameters.AddWithValue("@horario", doctor.id_horario);
                    com.Parameters.AddWithValue("@especialidad", doctor.Id_especialidad);
                    //Try Catch para detectar algun error en el registro
                    try
                    {
                        com.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        this.error = ex.Message;
                        mensaje_Error = "Error, Intente nuevamente";
                        ViewData["Error_Mensaje"] = mensaje_Error + error;
                        return View("Registro");
                    }
                    return View("RegistroExito");
                }
                else
                {
                    mensaje_Error = "Error en la carga de imagen, intente nuevamente";
                    ViewData["Error_Mensaje"] = mensaje_Error;
                    return View("Registro");
                }
            }

        public ActionResult datosMedicinaEspecificos(string nombreMedicina)
        {
            ListadoMedicina = new List<Medicina>();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select id_medicina, nombre_medicina, receta_medica, cantidad, precio ,tp.caracterisitica, clinica_id_clinica from medicina m " +
                " JOIN tipo_medicina tp ON m.id_tipomedicina = tp.id_tipomedicina where nombre_medicina LIKE '%"+nombreMedicina+"%'";
            dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Medicina med = new Medicina();
                    med.id_medicina = dr.GetString(0);
                    med.nombre_medicina = dr.GetString(1);
                    med.receta_medica = dr.GetString(2);
                    med.cantidad = dr.GetInt32(3);
                    med.precio = dr.GetDecimal(4);
                    med.tipoNombre = dr.GetString(5);
                    med.id_clinica = dr.GetString(6);
                    ListadoMedicina.Add(med);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("Error");
            }
            else
            {
                Verificacion2(ViewBag.Name, ViewBag.Age);
                return View("ListarMedicina", ListadoMedicina);
            }
        }
        public ActionResult LogOut()
        {
            string usuario = "";
            string contraseña = "";
            HttpContext.Session.SetString(SessionUser, usuario);
            HttpContext.Session.SetString(SessionPass, contraseña);
            return View("Login");
        }
    }
}

