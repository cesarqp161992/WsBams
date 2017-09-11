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
    public class DAO_TerminoPago : ConexionCliente
    {
        public string obtenerTerminoPago()
        {
            List<DB_TerminoPago> dbTerminoPagoList = new List<DB_TerminoPago>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_terminopago";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbTerminoPagoList.Add(new DB_TerminoPago()
                            {
                                cndvta = sqlDataReader.GetString(0),
                                descripcion = sqlDataReader.GetString(1)
                            });
                        return scriptSerializer.Serialize((object)dbTerminoPagoList);
                    }
                }
            }
        }
    }
}