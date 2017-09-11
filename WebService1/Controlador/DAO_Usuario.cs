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
    public class DAO_Usuario : Conexion
    {
        public string obtenerUsuario(string value)
        {
            List<Usuario> usuarioList = new List<Usuario>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_usuario";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@value", (object)SqlDbType.VarChar).Value = (object)value;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            usuarioList.Add(new Usuario()
                            {
                                coduser = sqlDataReader.GetString(0),
                                pass = sqlDataReader.GetString(1),
                                tipouser = sqlDataReader.GetString(2)
                            });
                        return scriptSerializer.Serialize((object)usuarioList);
                    }
                }
            }
        }
    }
}