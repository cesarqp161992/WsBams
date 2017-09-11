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
    public class DAO_Cliente : ConexionCliente
    {
        public string obtenerCliente(string coduser)
        {
            List<DB_Cliente> dbClienteList = new List<DB_Cliente>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_carteraclientes";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@CodVnd", (object)SqlDbType.VarChar).Value = (object)coduser;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbClienteList.Add(new DB_Cliente()
                            {
                                codcli = sqlDataReader.GetString(0),
                                coddomic = sqlDataReader.GetString(1),
                                razon = sqlDataReader.GetString(2),
                                direccion = sqlDataReader.GetString(3),
                                codcnl = sqlDataReader.GetString(4),
                                codven = sqlDataReader.GetString(5),
                                cndvta = sqlDataReader.GetString(6),
                                codmon = sqlDataReader.GetString(7),
                                numdoc_id = sqlDataReader.GetString(8),
                                tipo_cliente = sqlDataReader.GetString(9)
                            });
                        return scriptSerializer.Serialize((object)dbClienteList);
                    }
                }
            }
        }
    }
}