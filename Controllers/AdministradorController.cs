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
    public class AdministradorController : Controller
    {
        //SqlConnection con = new SqlConnection();
        //SqlCommand com = new SqlCommand();
        //SqlDataReader dr;

        public SqlConnection con;
        public SqlTransaction transaccion;
        public string error;
        private readonly IWebHostEnvironment _env;
        List<Administrador> listado;
        List<Cliente> listadoCliente;
        List<Doctor> listadoDoctor;
        const string SessionUser = "_user";
        const string SessionPass = "_pass";


        [Obsolete]
        public AdministradorController(IWebHostEnvironment env)
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

        public ActionResult DatosPersonalesAdmin()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {
                string user = HttpContext.Session.GetString(SessionUser);
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con;
                com.CommandText = "select * from administrador where admin_user='" + user + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {

                    ViewData["Nombres"] = dr.GetString(1);
                    ViewData["Dni_Admin"] = dr.GetDecimal(2);
                    ViewData["telef_Admin"] = dr.GetString(3);
                    ViewData["CorreoAdmin"] = dr.GetString(4);
                    Verificacion1(ViewBag.Name, ViewBag.Age);
                    return View();
                }
                else
                {
                    return View("AccesoDenegado");
                }
            }
        }
        public ActionResult ModificarAdministrador()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {
                listado = new List<Administrador>();
                string user = HttpContext.Session.GetString(SessionUser);
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con;
                com.CommandText = "select * from administrador where admin_user='" + ViewBag.Name + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    ViewData["Nombres"] = dr.GetString(1);
                    ViewData["Dni_Admin"] = dr.GetDecimal(2);
                    ViewData["telef_Admin"] = dr.GetString(3);
                    ViewData["CorreoAdmin"] = dr.GetString(4);
                    ViewData["user"] = dr.GetString(5);
                }
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View();
            }
        }

        public ActionResult ListarClientes()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("Error");
            }
            else
            {
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View(mostrarClientes());
            }
        }

        public ActionResult ListarDoctores()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("Error");
            }
            else
            {
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View(mostrarDoctores());
            }
        }


        public ActionResult AdministradorPage()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View();
            }
        }


        [HttpPost]
        public ActionResult Verificacion1(string usuario, string contraseña)
        {
            //connectionString();
            //con.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from administrador where admin_user='" + usuario + "' and admin_pass='" + contraseña + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                Administrador admin = new Administrador();
                //Asignamos los valores de la base de datos a los ViewData
                admin.Nombres = dr.GetString(1);
                admin.foto_admin = dr.GetString(7);
                ViewData["NombreCompleto"] = "Administrador: " + admin.Nombres;
                ViewData["foto"] = admin.foto_admin;

                //con.Close();
                HttpContext.Session.SetString(SessionUser, usuario);
                HttpContext.Session.SetString(SessionPass, contraseña);
                return View("AdministradorPage");
            }
            else
            {
                //con.Close();
                ViewData["Error"] = "Usuario o contraseña incorrectos";
                return View("Login");
            }
        }




        //Metodo para el listado

        public List<Administrador> listarAdministrador()
        {
            listado = new List<Administrador>();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from administrador";
            dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Administrador admin = new Administrador();
                    admin.Nombres = dr.GetString(1);
                    admin.id_clinica = dr.GetString(8);
                    listado.Add(admin);
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

        [HttpPost]
        public IActionResult ModificoAdministrador(Administrador administrador)
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
                com.CommandText = "UPDATE administrador  SET nombre_admin = @nombre, dni= @dni, telef_admin = @telefono, correo_admin = @correo, " +
                    "admin_user= @user, admin_pass = @password" +
                    " where admin_user = '" + user + "' and admin_pass = '" + contraseña + "'";


                com.Parameters.AddWithValue("@nombre", administrador.Nombres);
                com.Parameters.AddWithValue("@dni", administrador.Dni_Admin);
                com.Parameters.AddWithValue("@telefono", administrador.telef_Admin);
                com.Parameters.AddWithValue("@correo", administrador.CorreoAdmin);
                com.Parameters.AddWithValue("@user", administrador.user_admin);
                com.Parameters.AddWithValue("@password", administrador.admin_password);

                //Try Catch para detectar algun error en el registro
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.error = ex.Message;
                    ViewData["Mensaje_error"] = error;
                    return View("ModificarAdministrador");
                }
                Verificacion1(user, contraseña);
                return View("AdministradorPage");
            }
        }

        public List<Cliente> mostrarClientes()
        {
            listadoCliente = new List<Cliente>();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from cliente";
            dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Cliente client = new Cliente();
                    client.Nombres = dr.GetString(1);
                    client.Apellidos = dr.GetString(2);
                    client.Dni_Cliente = dr.GetDecimal(3);
                    client.telef_Cliente = dr.GetString(4);
                    client.CorreoCliente = dr.GetString(5);
                    client.fecha_nacimiento = dr.GetDateTime(9);
                    client.seguro = dr.GetString(10);
                    client.edad = dr.GetInt32(11);
                    listadoCliente.Add(client);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return listadoCliente;

        }

        public List<Doctor> mostrarDoctores()
        {
            listadoDoctor = new List<Doctor>();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from doctor";
            dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Doctor doc = new Doctor();
                    doc.nombre_doctor = dr.GetString(1);
                    doc.Dni_Doctor = dr.GetDecimal(2);
                    doc.telef_doctor = dr.GetString(3);
                    doc.CorreoDoctor = dr.GetString(4);
                    doc.id_clinica = dr.GetString(8);
                    doc.id_horario = dr.GetString(9);
                    doc.Id_especialidad = dr.GetString(10);
                    listadoDoctor.Add(doc);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return listadoDoctor;

        }

        public ActionResult datosClientesEspecificos(decimal dni)
        {
            listadoCliente = new List<Cliente>();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from cliente where  CAST(dni AS VARCHAR) LIKE '%" + dni + "%'";
            dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Cliente client = new Cliente();
                    client.Nombres = dr.GetString(1);
                    client.Apellidos = dr.GetString(2);
                    client.Dni_Cliente = dr.GetDecimal(3);
                    client.telef_Cliente = dr.GetString(4);
                    client.CorreoCliente = dr.GetString(5);
                    client.fecha_nacimiento = dr.GetDateTime(9);
                    client.seguro = dr.GetString(10);
                    client.edad = dr.GetInt32(11);
                    listadoCliente.Add(client);
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
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View("ListarClientes", listadoCliente);
            }
        }

        public ActionResult datosDoctoresEspecificos(string nombre)
        {
            listadoDoctor = new List<Doctor>();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from doctor where nombre_doctor LIKE '%"+nombre+"%'";
            dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Doctor doctor = new Doctor();
                    doctor.nombre_doctor = dr.GetString(1);
                    doctor.Dni_Doctor = dr.GetDecimal(2);
                    doctor.telef_doctor = dr.GetString(3);
                    doctor.CorreoDoctor = dr.GetString(4);
                    doctor.id_clinica = dr.GetString(8);
                    doctor.id_horario = dr.GetString(9);
                    doctor.Id_especialidad = dr.GetString(10);
                    listadoDoctor.Add(doctor);
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
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View("ListarDoctores", listadoDoctor);
            }
        }

        [BindProperty]
        public Cliente Cliente { get; set; }


        [HttpPost]
        public async Task<IActionResult> registrarAdministrador(Administrador admin, IFormFile file)
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
                com.CommandText = "INSERT INTO administrador (id_admin, nombre_admin, dni, telef_admin, correo_admin, admin_user, admin_pass, admin_foto, clinica_id_clinica)" +
                    " VALUES (@id,@nombre,@dni,@telefono,@correo,@user,@password,@foto,@clinica)";
                admin.foto_admin = file.FileName.ToString();
                com.Parameters.AddWithValue("@id", admin.Id_Admin);
                com.Parameters.AddWithValue("@nombre", admin.Nombres);
                com.Parameters.AddWithValue("@dni", admin.Dni_Admin);
                com.Parameters.AddWithValue("@telefono", admin.telef_Admin);
                com.Parameters.AddWithValue("@correo", admin.CorreoAdmin);
                com.Parameters.AddWithValue("@user", admin.user_admin);
                com.Parameters.AddWithValue("@password", admin.admin_password);
                com.Parameters.AddWithValue("@foto", admin.foto_admin);
                com.Parameters.AddWithValue("@clinica", admin.id_clinica);

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


        public ActionResult LogOut()
        {
            string usuario = "";
            string contraseña = "";
            HttpContext.Session.SetString(SessionUser, usuario);
            HttpContext.Session.SetString(SessionPass, contraseña);
            return View("Login");
        }

        //[HttpPost]
        //public async Task<IActionResult> SingleFile(IFormFile file)
        //{
        //var dir = _env.WebRootPath;
        //var saveimg = Path.Combine(dir, "img/Fotos", file.FileName);
        //var stream = new FileStream(saveimg, FileMode.Create);
        //await file.CopyToAsync(stream);
        //    return RedirectToAction("registrarCliente");
        //}
    }
}
