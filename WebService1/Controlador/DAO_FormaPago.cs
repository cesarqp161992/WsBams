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
    public class DAO_FormaPago : ConexionCliente
    {
        public string obtenerFormaPago()
        {
            List<DB_FormaPago> dbFormaPagoList = new List<DB_FormaPago>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_formapago";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbFormaPagoList.Add(new DB_FormaPago()
                            {
                                codforpag = sqlDataReader.GetString(0),
                                desforpag = sqlDataReader.GetString(1)
                            });
                        return scriptSerializer.Serialize((object)dbFormaPagoList);
                    }
                }
            }
        }
    }
}