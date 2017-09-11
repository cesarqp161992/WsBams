using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebService1.Modelo;

namespace WebService1.Controlador
{
    public class DAO_RegMovil : Conexion
    {
        public string obtenerDatosxImei(string coduser)
        {
            List<DB_RegMovil> dbRegMovilList = new List<DB_RegMovil>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_obtenerDatosxImei";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@Imei", (object)SqlDbType.VarChar).Value = (object)coduser;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbRegMovilList.Add(new DB_RegMovil()
                            {
                                IMEI = sqlDataReader.GetString(0),
                                Correo = sqlDataReader.GetString(1),
                                IpServidor = sqlDataReader.GetString(2),
                                NombreEmpresa = sqlDataReader.GetString(3)
                            });
                        return scriptSerializer.Serialize((object)dbRegMovilList);
                    }
                }
            }
        }

        public string validarApp(string imei_movil,string version,string variable)
        {
            List<Hashtable> dbList = new List<Hashtable>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_validarApp";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@Imei", (object)SqlDbType.VarChar).Value = (object)imei_movil;
                    sqlCommand.Parameters.AddWithValue("@Version", (object)SqlDbType.VarChar).Value = (object)version;
                    sqlCommand.Parameters.AddWithValue("@Variable", (object)SqlDbType.VarChar).Value = (object)variable;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable["Codigo"] = sqlDataReader.GetInt32(0);
                            hashtable["Mensaje"] = sqlDataReader.GetString(1);
                            dbList.Add(hashtable);
                        }
                        return scriptSerializer.Serialize((object)dbList);
                       
                    }
                }
            }
        }

    }
}