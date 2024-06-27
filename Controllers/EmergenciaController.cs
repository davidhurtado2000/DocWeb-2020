using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocWeb_Prueba.Models;
using DocWeb_Prueba.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocWeb_Prueba.Controllers
{
    public class EmergenciaController : Controller
    {
        public SqlConnection con;
        public SqlTransaction transaccion;
        public string error;
        private readonly IWebHostEnvironment _env;
        List<Cliente> listadoCliente;
        const string SessionUser = "_user";
        const string SessionPass = "_pass";

        [Obsolete]
        public EmergenciaController(IWebHostEnvironment env)
        {
            this.con = Conexion.getConexion();
            _env = env;
        }
        [HttpGet]
        public ActionResult LoginEmergencia()
        {
            return View();
        }

        public ActionResult RegistroEmer()
        {
            return View();
        }

        public ActionResult RegistroExito()
        {
            return View();
        }

        public ActionResult DatosCliente()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("Error");
            }
            else
            {
                VerificacionEmergencia(ViewBag.Name, ViewBag.Age);
                return View(mostrarClientes());
            }
        }

        [BindProperty]
        public Cliente cliente { get; set; }

        public ActionResult EmergenciaPage()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("Error");
            }
            else
            {
                VerificacionEmergencia(ViewBag.Name, ViewBag.Age);
                return View();
            }
        }

        [HttpPost]
        public ActionResult VerificacionEmergencia(string usuario, string contraseña)
        {
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from emple_emergencia where emer_user='" + usuario + "' and emer_pass='" + contraseña + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                Emergencia emp = new Emergencia();
                emp.Nombre_Empleado = dr.GetString(1);
                emp.Foto_Empleado = dr.GetString(7);
                ViewData["NombreCompleto"] = "Empleado: " + emp.Nombre_Empleado;
                ViewData["foto"] = emp.Foto_Empleado;

                HttpContext.Session.SetString(SessionUser, usuario);
                HttpContext.Session.SetString(SessionPass, contraseña);
                return View("EmergenciaPage");
            }
            else
            {
                ViewData["Error"] = "Usuario o contraseña incorrectos";
                return View("LoginEmergencia");
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

        public ActionResult datosClientesEspecificos(decimal dni)
        {
            listadoCliente = new List<Cliente>();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from cliente where CAST(dni AS VARCHAR) LIKE '%"+dni+"%'";
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
                return View("AccesoDenegado");
            }
            else
            {
                VerificacionEmergencia(ViewBag.Name, ViewBag.Age);
                return View("DatosCliente", listadoCliente);
            }
        }

        [HttpPost]
        public async Task<IActionResult> registrarEmpleado(Emergencia emp, IFormFile file)
        {
            string mensaje_Error = "";
            string imgext = Path.GetExtension(file.FileName);
            if (imgext == ".jpg" || imgext == ".png")
            {
                var dir = _env.WebRootPath;
                var saveimg = Path.Combine(dir, "img/Fotos", file.FileName);
                using (var stream = new FileStream(saveimg, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                SqlCommand com = new SqlCommand();

                com.Connection = con;
                com.CommandText = "INSERT INTO emple_emergencia (id_empemer, nombre_emple, dni, telef_emple, correo_emple, emer_user, emer_pass, emer_foto, clinica_id_clinica, id_sector)" +
                    " VALUES (@id,@nombre,@dni,@telefono,@correo,@user,@password,@foto,@clinica,@sector)";
                emp.Foto_Empleado = file.FileName.ToString();
                com.Parameters.AddWithValue("@id", emp.Id_Empleado);
                com.Parameters.AddWithValue("@nombre", emp.Nombre_Empleado);
                com.Parameters.AddWithValue("@dni", emp.DNI_Empleado);
                com.Parameters.AddWithValue("@telefono", emp.Telefono_Empleado);
                com.Parameters.AddWithValue("@correo",emp.Correo_Empleado);
                com.Parameters.AddWithValue("@user", emp.User_Empleado);
                com.Parameters.AddWithValue("@password", emp.Password_Empleado);
                com.Parameters.AddWithValue("@foto", emp.Foto_Empleado);
                com.Parameters.AddWithValue("@clinica", emp.Id_Clinica);
                com.Parameters.AddWithValue("@sector", emp.Id_Sector);
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.error = ex.Message;
                    mensaje_Error = "Error, Intente nuevamente";
                    ViewData["Error_Mensaje"] = mensaje_Error;
                    return View("RegistroEmer");
                }
                return View("RegistroExito");
            }
            else
            {
                mensaje_Error = "Error en la carga de imagen, intente nuevamente";
                ViewData["Error_Mensaje"] = mensaje_Error;
                return View("RegistroEmer");
            }

        }
        public ActionResult LogOut()
        {
            string usuario = "";
            string contraseña = "";
            HttpContext.Session.SetString(SessionUser, usuario);
            HttpContext.Session.SetString(SessionPass, contraseña);
            return View("LoginEmergencia");
        }

    }
}
