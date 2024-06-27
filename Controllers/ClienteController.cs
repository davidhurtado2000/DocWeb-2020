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
using Microsoft.AspNetCore.Html;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DocWeb_Prueba.Controllers
{
    public class ClienteController : Controller
    {
        //SqlConnection con = new SqlConnection();
        //SqlCommand com = new SqlCommand();
        //SqlDataReader dr;

        public SqlConnection con;
        public SqlTransaction transaccion;
        public string error;
        private readonly IWebHostEnvironment _env;
        List<Cliente> listado;
        const string SessionUser = "_user";
        const string SessionPass = "_pass";
        const string SessionIdServicio = "_id_servicio";
        const string SessionIdDoctor = "_id_doctor";
        const string SessionIdCliente = "_id_cliente";
        const string SessionFecha = "_id_fecha";



        [Obsolete]
        public ClienteController(IWebHostEnvironment env)
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

        public ActionResult ExitoReserva()
        {
            string usuario = "";
            string contraseña = "";
            HttpContext.Session.SetString(SessionUser, usuario);
            HttpContext.Session.SetString(SessionPass, contraseña);
            return View();
        }

        public ActionResult ModificarCliente()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == "" || ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else { 
                listado = new List<Cliente>();
            string user = HttpContext.Session.GetString(SessionUser);
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from cliente where cliente_user='" + ViewBag.Name + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                ViewData["Nombres"] = dr.GetString(1);
                ViewData["Apellidos"] = dr.GetString(2);
                ViewData["Dni_Cliente"] = dr.GetDecimal(3);
                ViewData["telef_Cliente"] = dr.GetString(4);
                ViewData["CorreoCliente"] = dr.GetString(5);
                ViewData["fecha_nacimiento"] = dr.GetDateTime(9).ToString("yyyy-MM-dd");
                ViewData["user"] = dr.GetString(6);
                ViewData["seguro"] = dr.GetString(10);
                ViewData["edad"] = dr.GetInt32(11);
            }
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View();
            }
        }

        public ActionResult ConsultarDisponibilidad()
        {
            
            ViewBag.Servicio = HttpContext.Session.GetString(SessionIdServicio);
            ViewBag.Doctor = HttpContext.Session.GetString(SessionIdDoctor);
            ViewBag.IdCliente = HttpContext.Session.GetInt32(SessionIdCliente);
            ViewBag.Fecha_Reserva = HttpContext.Session.GetString(SessionFecha);
            if (ViewBag.IdCliente == null || ViewBag.Servicio == "" || ViewBag.Servicio == null || ViewBag.Doctor == "" || ViewBag.Doctor == null
                || ViewBag.Fecha_Reserva == "" || ViewBag.Fecha_Reserva == null)
            {
                return View("AccesoDenegado");
            }
            else
            {
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View();
            }
        }

        public ActionResult Listado()
        {
            return View(listarClientes());
        }

        public ActionResult ClientePage()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            ViewBag.IdCliente = HttpContext.Session.GetInt32(SessionIdCliente);
            if (ViewBag.Name == null || ViewBag.Age == null || ViewBag.Name == ""|| ViewBag.Age == "")
            {
                return View("AccesoDenegado");
            }
            else
            {
                Verificacion1(ViewBag.Name, ViewBag.Age);
                return View();
            }
        }

        public ActionResult PrimerRequisitoReserva()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionUser);
            ViewBag.Age = HttpContext.Session.GetString(SessionPass);
            ViewBag.IdCliente = HttpContext.Session.GetInt32(SessionIdCliente);
            object numero = ViewBag.IdCliente;
            if (ViewBag.Name == "" || ViewBag.Age == "" || ViewBag.IdCliente == null)
            {
                return View("AccesoDenegado");
            }
            else
            {
                Verificacion1(ViewBag.Name, ViewBag.Age);
                ViewBag.CategoriaSelectList = new SelectList(listaCategoria(), "id_categoria", "nombre_categoria");
                return View();
            }
        }


        public ActionResult DatosPersonales()
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
                com.CommandText = "select * from cliente where cliente_user='" + user + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {

                    ViewData["Nombres"] = dr.GetString(1);
                    ViewData["Apellidos"] = dr.GetString(2);
                    ViewData["Dni_Cliente"] = dr.GetDecimal(3);
                    ViewData["telef_Cliente"] = dr.GetString(4);
                    ViewData["CorreoCliente"] = dr.GetString(5);
                    ViewData["fecha_nacimiento"] = dr.GetDateTime(9).ToString("yyyy-MM-dd");
                    ViewData["seguro"] = dr.GetString(10);
                    ViewData["edad"] = dr.GetInt32(11);
                    Verificacion1(ViewBag.Name, ViewBag.Age);
                    return View();
                }
                else
                {
                    return View("AccesoDenegado");
                }
            }
        }



        public List<Categoria> listaCategoria()
        {
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from categoria";
            dr = com.ExecuteReader();
            var lista = new List<Categoria>();
            while (dr.Read())
            {
                lista.Add(new Categoria() { id_categoria = dr.GetInt32(0), nombre_categoria = dr.GetString(1)});
            }
            return lista;
        }

        public ActionResult listarServicio(int id_categoria)
        {
            
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from servicio_especialidad where categoria_id_categoria="+id_categoria;
            dr = com.ExecuteReader();
            var lista = new List<Servicio>();
            while (dr.Read())
            {
                lista.Add(new Servicio() { id_servicio = dr.GetString(0), nombre_esp = dr.GetString(1)});
            }
            ViewBag.ServicioSelectList = new SelectList(lista, "id_servicio", "nombre_esp");
            return PartialView("DisplayServicios");
        }


        public ActionResult listarDoctor(string id_servicio)
        {
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "Select id_doctor, nombre_doctor from servicio_doctor s JOIN doctor d ON s.doctor_id_doctor = d.id_doctor where id_servicio ='"+id_servicio+"'";
            dr = com.ExecuteReader();
            var lista = new List<Doctor>();
            while (dr.Read())
            {
                lista.Add(new Doctor() { id_doctor = dr.GetString(0), nombre_doctor = dr.GetString(1) });
            }
            ViewBag.DoctorSelectList = new SelectList(lista, "id_doctor", "nombre_doctor");
            return PartialView("DisplayDoctores");
        }

        //void connectionString()
        //{
        //    con.ConnectionString = "Server=LAPTOP-BP0R1IFM\\BDBORISHG;Database=db_prueba_docweb;User ID=sa;Password=nomames12345;MultipleActiveResultSets=true";

        //}





        [HttpPost]
        public ActionResult Verificacion(Cliente cliente)
        {
            //connectionString();
            //con.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = "select * from cliente where cliente_user='"+cliente.user_cliente+"' and cliente_pass='"+cliente.user_password+"'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                //Asignamos los valores de la base de datos a los ViewData
                cliente.Nombres = dr.GetString(1);
                cliente.Apellidos = dr.GetString(2);
                cliente.foto_cliente = dr.GetString(8);
                ViewData["NombreCompleto"] = "Cliente: " + cliente.Nombres + " " + cliente.Apellidos;
                ViewData["foto"] = cliente.foto_cliente;

                //con.Close();
                HttpContext.Session.SetString(SessionUser, cliente.user_cliente);
                HttpContext.Session.SetString(SessionPass, cliente.user_password);
                return View("ClientePage");
            }
            else
            {
                //con.Close();
                return View("Error");
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
            com.CommandText = "select * from cliente where cliente_user='" + usuario + "' and cliente_pass='" + contraseña + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                Cliente cliente = new Cliente();
                //Asignamos los valores de la base de datos a los ViewData
                cliente.Id_Cliente = dr.GetInt32(0);
                cliente.Nombres = dr.GetString(1);
                cliente.Apellidos = dr.GetString(2);
                cliente.foto_cliente = dr.GetString(8);
                ViewData["NombreCompleto"] = "Cliente: " + cliente.Nombres + " " + cliente.Apellidos;
                ViewData["foto"] = cliente.foto_cliente;

                //con.Close();
                HttpContext.Session.SetString(SessionUser, usuario);
                HttpContext.Session.SetString(SessionPass, contraseña);
                HttpContext.Session.SetInt32(SessionIdCliente, cliente.Id_Cliente);
                return View("ClientePage");
            }
            else
            {
                //con.Close();
                ViewData["Error"] = "Usuario o contraseña incorrectos";
                return View("Login");
            }
        }




        //Metodo para el listado

        public List<Cliente> listarClientes()
        {
            //La variable que se recupera
            listado = new List<Cliente>();
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
                    client.fecha_nacimiento = dr.GetDateTime(9);
                    listado.Add(client);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return listado;

        }

        [BindProperty]
        public Cliente Cliente { get; set; }


        [HttpPost]
        public async Task<IActionResult> registrarCliente(Cliente cliente, IFormFile file)
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
                com.CommandText = "INSERT INTO cliente (nombres_cliente, apellidos_cliente, dni, telef_cliente, correo_cliente, cliente_user, cliente_pass, cliente_foto, fecha_nacimiento, seguro, edad)" +
                    " VALUES (@nombre,@apellido,@dni,@telefono,@correo,@user,@password,@foto,@fecha,@seguro,@edad)";
                cliente.foto_cliente = file.FileName.ToString();
                com.Parameters.AddWithValue("@nombre", cliente.Nombres);
                com.Parameters.AddWithValue("@apellido", cliente.Apellidos);
                com.Parameters.AddWithValue("@dni", cliente.Dni_Cliente);
                com.Parameters.AddWithValue("@telefono", cliente.telef_Cliente);
                com.Parameters.AddWithValue("@correo", cliente.CorreoCliente);
                com.Parameters.AddWithValue("@user", cliente.user_cliente);
                com.Parameters.AddWithValue("@password", cliente.user_password);
                com.Parameters.AddWithValue("@foto", cliente.foto_cliente);
                com.Parameters.AddWithValue("@fecha", cliente.fecha_nacimiento);
                com.Parameters.AddWithValue("@seguro", cliente.seguro);
                com.Parameters.AddWithValue("@edad", cliente.edad);

                //Try Catch para detectar algun error en el registro
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.error = ex.Message;
                    mensaje_Error = "Error, Intente nuevamente";
                    ViewData["Error_Mensaje"] = mensaje_Error;
                    return View("Registro");
                }
                return View("RegistroExito");
            }
            else
            {
                mensaje_Error = "Error en la carga de imange, intente nuevamente";
                ViewData["Error_Mensaje"] = mensaje_Error;
                return View("Registro");
            }
        }


        public ActionResult pasarDatosPrincipales(string id_servicio, string id_doctor)
        {
            HttpContext.Session.SetString(SessionIdServicio, id_servicio);
            HttpContext.Session.SetString(SessionIdDoctor, id_doctor);
            ViewBag.IdCliente = HttpContext.Session.GetInt32(SessionIdCliente);

            return View("ConsultarDisponibilidad");
        }


        public ActionResult dispinibilidadDoctor(DateTime fecha)
        {
            string id_doctor = HttpContext.Session.GetString(SessionIdDoctor); ;
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            string[] dias = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };
            DateTime fecha_Actual = fecha;
            string fecha_TIPODATE = "";
            string fecha_reserva = fecha.ToString("dddd");
            for (int i = 0; i < days.Length; i++)
            {
                if (fecha_reserva == days[i])
                {
                    fecha_reserva = dias[i];
                }
            }
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con;
            com.CommandText = " SELECT nombre_doctor ,telef_doc,dia1,dia2,dia3,dia4,dia5,dia6 FROM horario a JOIN doctor d ON a.id_horario=d.horario_id_horario WHERE " +
                "(a.dia1 = '"+fecha_reserva+ "' OR a.dia2 = '" + fecha_reserva + "' OR a.dia3 = '" + fecha_reserva + "' OR a.dia4 = '" + fecha_reserva + "' OR a.dia5 = '" + fecha_reserva + "' OR a.dia6 = '" + fecha_reserva + "' )" +
                "AND d.id_doctor = '"+id_doctor+"'";
            dr = com.ExecuteReader();
            List<Doctor> infoDisponilibidad = new List<Doctor>();
            while (dr.Read())
            {
                fecha_TIPODATE = fecha_Actual.ToString("yyyy-MM-dd");
                Doctor doctor = new Doctor();
                doctor.nombre_doctor = dr.GetString(0);
                doctor.telef_doctor = dr.GetString(1);
                infoDisponilibidad.Add(doctor);
            }
            
            HttpContext.Session.SetString(SessionFecha, fecha_TIPODATE);
            return View("ConsultarDisponibilidad", infoDisponilibidad);
        }

        [Route("{controller}/{action}/{hora}")]
        public ActionResult registrarReserva(string hora)
        {
            int id_cliente = (int) HttpContext.Session.GetInt32(SessionIdCliente);
            string id_doctor = HttpContext.Session.GetString(SessionIdDoctor)
                , id_servicio = HttpContext.Session.GetString(SessionIdServicio)
                , fecha = HttpContext.Session.GetString(SessionFecha);
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandText = "INSERT INTO reserva (fecha_reserva, hora, doctor_id_doctor, cliente_id_cliente, id_servicio)" +
                " VALUES (@fecha,@hora,@doctor,@cliente,@servicio)";
            com.Parameters.AddWithValue("@fecha", fecha);
            com.Parameters.AddWithValue("@hora", hora.ToString());
            com.Parameters.AddWithValue("@doctor", id_doctor);
            com.Parameters.AddWithValue("@cliente", id_cliente);
            com.Parameters.AddWithValue("@servicio", id_servicio);
            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                ViewData["Mensaje_de_Error"] = error;
                return View("ConsultarDisponibilidad");
            }
            return View("ExitoReserva");
        }

        [HttpPost]
        public IActionResult ModificoCliente(Cliente cliente)
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
                com.CommandText = "UPDATE cliente  SET nombres_cliente =  @nombre, apellidos_cliente = @apellido, dni= @dni, telef_cliente = @telefono, correo_cliente = @correo, " +
                    "cliente_user= @user, cliente_pass = @password, fecha_nacimiento = @fecha, seguro = @seguro, edad = @edad" +
                    " where cliente_user = '" + user + "' and cliente_pass = '" + contraseña + "'";


                com.Parameters.AddWithValue("@nombre", cliente.Nombres);
                com.Parameters.AddWithValue("@apellido", cliente.Apellidos);
                com.Parameters.AddWithValue("@dni", cliente.Dni_Cliente);
                com.Parameters.AddWithValue("@telefono", cliente.telef_Cliente);
                com.Parameters.AddWithValue("@correo", cliente.CorreoCliente);
                com.Parameters.AddWithValue("@user", cliente.user_cliente);
                com.Parameters.AddWithValue("@password", cliente.user_password);
                com.Parameters.AddWithValue("@fecha", cliente.fecha_nacimiento);
                com.Parameters.AddWithValue("@seguro", cliente.seguro);
                com.Parameters.AddWithValue("@edad", cliente.edad);

                //Try Catch para detectar algun error en el registro
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.error = ex.Message;
                    ViewData["Mensaje_error"] = error;
                    return View("ModificarCliente");
                }
                Verificacion1(user, contraseña);
                return View("ClientePage");
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

    //public static HtmlString getCategoria()
    //{
    //    SqlConnection con = Conexion.getConexion();
    //    SqlCommand com = new SqlCommand();
    //    SqlDataReader dr;
    //    com.Connection = con;
    //    com.CommandText = "select * from categoria";
    //    dr = com.ExecuteReader();
    //    string categoria = "<option value='0'>Elige una Categoria</option>";

    //    while (dr.Read())
    //    {
    //        Categoria cat = new Categoria();
    //        cat.id_categoria = dr.GetInt32(0);
    //        cat.nombre_categoria = dr.GetString(1);
    //        categoria = string.Concat(categoria, "<option value='" + cat.id_categoria + "'>" + cat.nombre_categoria + "</option>");

    //    }
    //        return new HtmlString(categoria.ToString());
    //}


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

