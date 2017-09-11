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
    public class DAO_Canal : ConexionCliente
    {
        public string obtenerCanal()
        {
            List<DB_Canal> dbCanalList = new List<DB_Canal>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_canal";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbCanalList.Add(new DB_Canal()
                            {
                                codcnl = sqlDataReader.GetString(0),
                                nomcnl = sqlDataReader.GetString(1)
                            });
                        return scriptSerializer.Serialize((object)dbCanalList);
                    }
                }
            }
        }
    }
}