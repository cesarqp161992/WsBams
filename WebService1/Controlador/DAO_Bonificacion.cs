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
    public class DAO_Bonificacion : ConexionCliente
    {
        public string obtenerBonificacion(string coduser)
        {
            List<DB_Bonificacion> dbBnfList = new List<DB_Bonificacion>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "Uspm_bonificaciones";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CodVnd", (object)SqlDbType.VarChar).Value = (object)coduser;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            dbBnfList.Add(new DB_Bonificacion()
                            {
                                Id = sqlDataReader.GetInt32(0),
                                FecInicio = sqlDataReader.GetString(1),
                                FecFinal = sqlDataReader.GetString(2),
                                General = sqlDataReader.GetInt32(3),
                                CodVnd = sqlDataReader.GetString(4),
                                Politica = sqlDataReader.GetString(5),
                                GiroNegocio = sqlDataReader.GetString(6),
                                NombrePromocion = sqlDataReader.GetString(7),
                                CodAlm = sqlDataReader.GetString(8),
                                ItemSalida = sqlDataReader.GetInt32(9),
                                CodSalida = sqlDataReader.GetString(10),
                                TipoUnidadSalida = sqlDataReader.GetInt32(11),
                                CantidadPromocion = sqlDataReader.GetInt32(12),
                                MaximoPedido = sqlDataReader.GetInt32(13),
                                GrupoCondicion = sqlDataReader.GetInt32(14),
                                Tipo = sqlDataReader.GetString(15),
                                Condicion = sqlDataReader.GetInt32(16),
                                TipoEntrada = sqlDataReader.GetString(17),
                                CodEntrada = sqlDataReader.GetString(18),
                                TipoUnidadEntrada = sqlDataReader.GetInt32(19),
                                CantidadCondicion = sqlDataReader.GetInt32(20),
                                MontoMinimo = sqlDataReader.GetDecimal(21),
                                MontoMaximo = sqlDataReader.GetDecimal(22),
                                Monto = sqlDataReader.GetDecimal(23),
                                Itemgrupocondicion = sqlDataReader.GetInt32(24)
                            });
                        return scriptSerializer.Serialize((object)dbBnfList);
                    }
                }
            }
        }
    }
}