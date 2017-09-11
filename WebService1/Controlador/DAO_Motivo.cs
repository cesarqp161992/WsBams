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
    public class DAO_Motivo : ConexionCliente
    {
        public string obtenerMotivo()
        {
            List<DB_Motivo> dbMotivoList = new List<DB_Motivo>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_motivo";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbMotivoList.Add(new DB_Motivo()
                            {
                                tipmot = sqlDataReader.GetString(0),
                                codmot = sqlDataReader.GetString(1),
                                desmot = sqlDataReader.GetString(2)
                            });
                        return scriptSerializer.Serialize((object)dbMotivoList);
                    }
                }
            }
        }
    }
}