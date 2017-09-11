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
    public class DAO_Empresa : Conexion
    {
        public string obtenerEmpresasxImei(string imei_movil)
        {
            List<DB_Empresa> dbRegEmpresaList = new List<DB_Empresa>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_obtenerEmpresasxImei";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@Imei", (object)SqlDbType.VarChar).Value = (object)imei_movil;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbRegEmpresaList.Add(new DB_Empresa()
                            {
                                IdEmpresa = sqlDataReader.GetInt32(0),
                                NombreEmpresa = sqlDataReader.GetString(1),
                                RUC = sqlDataReader.GetString(2),
                                IpServidor = sqlDataReader.GetString(3),
                                Estado = sqlDataReader.GetString(4)
                            });
                        return scriptSerializer.Serialize((object)dbRegEmpresaList);
                    }
                }
            }
        }

    }
}