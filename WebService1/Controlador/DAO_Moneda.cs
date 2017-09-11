using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebService1.Modelo;

namespace WebService1.Controlador
{
    public class DAO_Moneda : ConexionCliente
    {
        public string obtenerMoneda()
        {
            List<DB_Moneda> dbMonedaList = new List<DB_Moneda>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_moneda";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbMonedaList.Add(new DB_Moneda()
                            {
                                codmon = sqlDataReader.GetString(0),
                                desmon = sqlDataReader.GetString(1)
                            });
                        return scriptSerializer.Serialize((object)dbMonedaList);
                    }
                }
            }
        }
    }
}