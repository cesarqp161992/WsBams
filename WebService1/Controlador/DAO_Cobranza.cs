using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebService1.Controlador
{
    public class DAO_Cobranza : ConexionCliente
    {
        public string obtenerCobranza(string coduser)
        {
            List<Modelo.Cobranza> cobranzaList = new List<Modelo.Cobranza>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "uspm_cobranza";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@CodVnd", (object)SqlDbType.VarChar).Value = (object)coduser;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            cobranzaList.Add(new Modelo.Cobranza()
                            {
                                coddoc = sqlDataReader.GetString(0),
                                numdoc = sqlDataReader.GetString(1),
                                codcli = sqlDataReader.GetString(2),
                                coddomic = sqlDataReader.GetString(3),
                                codven = sqlDataReader.GetString(4),
                                codmon = sqlDataReader.GetString(5),
                                importe = sqlDataReader.GetString(6),
                                saldo = sqlDataReader.GetString(7),
                                fecemision = sqlDataReader.GetString(8),
                                fecvcto = sqlDataReader.GetString(9)
                            });
                        return scriptSerializer.Serialize((object)cobranzaList);
                    }
                }
            }
        }
    }
}