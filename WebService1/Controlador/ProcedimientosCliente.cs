using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebService1.Modelo;

namespace WebService1.Controlador
{
    public class ProcedimientosCliente : ConexionCliente
    {
        public void ejecutarProcesoPedido(string numpedido)
        {
            if (!ConfigurationManager.AppSettings["procedimientoPedidoCliente"].Equals("1"))
                return;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_procesarPedidoMovil";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@numpedido", (object)SqlDbType.VarChar).Value = (object)numpedido;
                    sqlCommand.Parameters.AddWithValue("@FlgSegundoPlano", (object)SqlDbType.VarChar).Value = (object)1;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void ejecutarProcesoPedidoStock(string numpedido, ref DB_GestionResponse response)
        {
            if (!ConfigurationManager.AppSettings["procedimientoPedidoStockCliente"].Equals("1"))
                return;
            List<DB_ProductoResponse> productoResponseList = new List<DB_ProductoResponse>();
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_procesarPedidoMovil";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@numpedido", (object)SqlDbType.VarChar).Value = (object)numpedido;
                    sqlCommand.Parameters.AddWithValue("@FlgSegundoPlano", (object)SqlDbType.VarChar).Value = (object)0;
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            productoResponseList.Add(new DB_ProductoResponse()
                            {
                                codpro = sqlDataReader.GetString(0),
                                stock = sqlDataReader.GetString(1)
                            });
                    }
                    response.estado = productoResponseList.Count > 0 ? -2 : 1;
                    response.data = productoResponseList;
                    if (response.estado == -2)
                        throw new Exception();
                }
            }
        }

        public void ejecutarProcesoCobranza(string numcobranza)
        {
            if (!ConfigurationManager.AppSettings["procedimientoCobranzaCliente"].Equals("1"))
                return;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_procesarCobranzaMovil";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@numcobranza", (object)SqlDbType.VarChar).Value = (object)numcobranza;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void ejecutarProcesoDevolucion(string numdevolucion)
        {
            if (!ConfigurationManager.AppSettings["procedimientoDevolucionCliente"].Equals("1"))
                return;
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_procesarDevolucionMovil";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@numdevolucion", (object)SqlDbType.VarChar).Value = (object)numdevolucion;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}