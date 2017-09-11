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
    public class DAO_Impuesto : ConexionCliente
    {
        public string obtenerImpuesto()
        {
            List<DB_Impuesto> dbImpuestoList = new List<DB_Impuesto>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_impuesto";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbImpuestoList.Add(new DB_Impuesto()
                            {
                                codimp = sqlDataReader.GetString(0),
                                desimp = sqlDataReader.GetString(1),
                                tasimp = sqlDataReader.GetString(2)
                            });
                        return scriptSerializer.Serialize((object)dbImpuestoList);
                    }
                }
            }
        }
    }
}