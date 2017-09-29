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
    public class DAO_Producto : ConexionCliente
    {
        public string obtenerProducto(string coduser)
        {
            List<DB_Producto> dbProductoList = new List<DB_Producto>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_producto";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CodVnd", (object)SqlDbType.VarChar).Value = (object)coduser;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbProductoList.Add(new DB_Producto()
                            {
                                codpro = sqlDataReader.GetString(0),
                                nompro = sqlDataReader.GetString(1),
                                prevta = sqlDataReader.GetString(2),
                                stock = sqlDataReader.GetString(3),
                                codimp = sqlDataReader.GetString(4),
                                flagfp = sqlDataReader.GetString(5),
                                estado = sqlDataReader.GetString(6),
                                porpcn = sqlDataReader.GetString(7),
                                tasisc = sqlDataReader.GetString(8),
                                cndvta = sqlDataReader.GetString(9),
                                codfam = sqlDataReader.GetString(10),
                                codsubfam = sqlDataReader.GetString(11)
                            });
                        return scriptSerializer.Serialize((object)dbProductoList);
                    }
                }
            }
        }
    }
}