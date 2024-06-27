using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DocWeb_Prueba.Util
{
    public class Conexion
    {
        private static SqlConnection objConexion;
        private static string error;

        public static SqlConnection getConexion()
        {
            if (objConexion != null)
                return objConexion;
            objConexion = new SqlConnection();
            objConexion.ConnectionString = "Server=LAPTOP-BP0R1IFM\\BDBORISHG;Database=db_docweb;User ID=sa;Password=nomames12345;MultipleActiveResultSets=true";
            try
            {
                objConexion.Open();
                return objConexion;
            }
            catch (Exception ex) 
            {
                error = ex.Message;
                return null;
            }
        }

        public static void cerrarConexion()
        {
            if (objConexion != null)
                objConexion.Close();
        }
    }
}
