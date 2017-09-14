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
                                FecInicio = sqlDataReader.GetString(0),
                                FecFinal = sqlDataReader.GetString(1),
                                General = sqlDataReader.GetInt32(2),
                                CodVnd = sqlDataReader.GetString(3),
                                Politica = sqlDataReader.GetString(4),
                                GiroNegocio = sqlDataReader.GetString(5),
                                NombrePromocion = sqlDataReader.GetString(6),
                                CodAlm = sqlDataReader.GetString(7),
                                ItemSalida = sqlDataReader.GetInt32(8),
                                CodSalida = sqlDataReader.GetString(9),
                                TipoUnidadSalida = sqlDataReader.GetInt32(10),
                                CantidadPromocion = sqlDataReader.GetInt32(11),
                                MaximoPedido = sqlDataReader.GetInt32(12),
                                GrupoCondicion = sqlDataReader.GetInt32(13),
                                Tipo = sqlDataReader.GetString(14),
                                Condicion = sqlDataReader.GetInt32(15),
                                TipoEntrada = sqlDataReader.GetString(16),
                                CodEntrada = sqlDataReader.GetString(17),
                                TipoUnidadEntrada = sqlDataReader.GetInt32(18),
                                CantidadCondicion = sqlDataReader.GetInt32(19),
                                MontoMinimo = sqlDataReader.GetDecimal(20),
                                MontoMaximo = sqlDataReader.GetDecimal(21),
                                Monto = sqlDataReader.GetDecimal(22)
                            });
                        return scriptSerializer.Serialize((object)dbBnfList);
                    }
                }
            }
        }
    }
}