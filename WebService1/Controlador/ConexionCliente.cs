using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebService1.Controlador
{
    public class ConexionCliente
    {
        public SqlConnection conectar()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["cnxCliente"].ConnectionString);
        }
    }
}