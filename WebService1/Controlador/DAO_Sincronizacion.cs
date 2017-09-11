using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebService1.Modelo;

namespace WebService1.Controlador
{
    public class DAO_Sincronizacion : ConexionCliente
    {
        public string obtenerSincronizacion(string coduser)
        {
            List<DB_Sincronizacion> dbSincronizacionList = new List<DB_Sincronizacion>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "select CAST(CAST(GETDATE() as date) as varchar) fecha, CONVERT(varchar, GETDATE(), 108) hora";
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbSincronizacionList.Add(new DB_Sincronizacion()
                            {
                                fecha = sqlDataReader.GetString(0),
                                hora = sqlDataReader.GetString(1)
                            });
                        return scriptSerializer.Serialize((object)dbSincronizacionList);
                    }
                }
            }
        }
    }
}