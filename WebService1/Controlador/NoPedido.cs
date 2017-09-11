using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebService1.Modelo;

namespace WebService1.Controlador
{
    public class NoPedido : Conexion
    {
        public int insertarNoPedido(string json)
        {
            DB_NoPedido noPedido = new JavaScriptSerializer().Deserialize<DB_NoPedido>(json);
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        cmd.Transaction = cmd.Connection.BeginTransaction();
                        this.insertarNoPedido(cmd, noPedido);
                        cmd.Transaction.Commit();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public string insertarNoPedidoMasivo(string json)
        {
            List<DB_GestionResponse> dbGestionResponseList = new List<DB_GestionResponse>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            List<DB_NoPedido> dbNoPedidoList = scriptSerializer.Deserialize<List<DB_NoPedido>>(json);
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    for (int index = 0; index < dbNoPedidoList.Count; ++index)
                    {
                        DB_GestionResponse dbGestionResponse = new DB_GestionResponse();
                        dbGestionResponse.numsec = dbNoPedidoList[index].numGestion;
                        dbGestionResponse.estado = 1;
                        dbGestionResponse.mensaje = "";
                        try
                        {
                            cmd.Transaction = cmd.Connection.BeginTransaction();
                            this.insertarNoPedido(cmd, dbNoPedidoList[index]);
                            cmd.Transaction.Commit();
                        }
                        catch
                        {
                            dbGestionResponse.estado = 0;
                            dbGestionResponse.mensaje = "Error";
                            cmd.Transaction.Rollback();
                        }
                        dbGestionResponseList.Add(dbGestionResponse);
                    }
                }
            }
            return scriptSerializer.Serialize((object)dbGestionResponseList);
        }

        private int insertarNoPedido(SqlCommand cmd, DB_NoPedido noPedido)
        {
            try
            {
                cmd.CommandText = "usp_insertarNoPedido";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                DateTime exact = DateTime.ParseExact(noPedido.fecha, "yyyy-MM-dd HH:mm:ss", (IFormatProvider)CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@numpedido", SqlDbType.VarChar).Value = (object)noPedido.numGestion;
                cmd.Parameters.Add("@codvendedor", SqlDbType.VarChar).Value = (object)noPedido.codven;
                cmd.Parameters.Add("@codcliente", SqlDbType.VarChar).Value = (object)noPedido.codcli;
                cmd.Parameters.Add("@coddomicilio", SqlDbType.VarChar).Value = (object)noPedido.coddomic;
                cmd.Parameters.Add("@codmot", SqlDbType.VarChar).Value = (object)noPedido.codmot;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = (object)exact;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}