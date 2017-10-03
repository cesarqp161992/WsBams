using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebService1.Controlador
{
    public class DAO_Familia : ConexionCliente
    {
        public string obtenerFamilia(string coduser)
        {
            List<Modelo.DB_Familia> dbList = new List<Modelo.DB_Familia>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_familia";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CodVnd", (object)SqlDbType.VarChar).Value = (object)coduser;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbList.Add(new Modelo.DB_Familia()
                            {
                                codfam = sqlDataReader.GetString(0),
                                desfam = sqlDataReader.GetString(1),
                                abrevfam = sqlDataReader.GetString(2)
                            });
                        return scriptSerializer.Serialize((object)dbList);
                    }
                }
            }
        }
    }
}