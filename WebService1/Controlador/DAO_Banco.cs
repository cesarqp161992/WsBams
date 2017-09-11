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
    public class DAO_Banco : ConexionCliente
    {
        public string obtenerBanco()
        {
            List<DB_Banco> dbBancoList = new List<DB_Banco>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_banco";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbBancoList.Add(new DB_Banco()
                            {
                                codbco = sqlDataReader.GetString(0),
                                codctacte = sqlDataReader.GetString(1),
                                nombco = sqlDataReader.GetString(2),
                                numctacte = sqlDataReader.GetString(3),
                                codmon = sqlDataReader.GetString(4)
                            });
                        return scriptSerializer.Serialize((object)dbBancoList);
                    }
                }
            }
        }
    }
}