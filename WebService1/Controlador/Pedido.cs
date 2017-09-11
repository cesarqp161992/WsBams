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
    public class Pedido : Conexion
    {
        public string insertarPedido(string json)
        {
            DB_GestionResponse response = new DB_GestionResponse();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            DB_PedidoCabecera pedido = scriptSerializer.Deserialize<DB_PedidoCabecera>(json);
            response.numsec = pedido.numGestion;
            response.estado = 1;
            response.mensaje = "";
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        this.insertarJson(cmd, "G_PEDIDO",json);
                        if (this.insertarPedidoCabecera(cmd, pedido) == 1)
                            this.insertarPedidoDetalle(cmd, pedido.pedidoDetalles);
                        new ProcedimientosCliente().ejecutarProcesoPedidoStock(pedido.numGestion, ref response);
                    }
                    catch (Exception ex)
                    {
                        this.eliminarPedido(cmd, pedido);
                        if (response.estado == 1)
                        {
                            response.estado = 0;
                            response.mensaje = ex.Message;
                        }
                    }
                    return scriptSerializer.Serialize((object)response);
                }
            }
        }

        public string insertarPedidoMasivo(string coduser, string json)
        {
            List<DB_GestionResponse> dbGestionResponseList = new List<DB_GestionResponse>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            List<DB_PedidoCabecera> dbPedidoCabeceraList = scriptSerializer.Deserialize<List<DB_PedidoCabecera>>(json);
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    this.insertarJson(cmd, "G_PED_MAS", json);
                    int idTask = -1;
                    try
                    {
                        idTask = this.insertarBackgroundTask(cmd, coduser);
                    }
                    catch
                    {
                    }
                    for (int index = 0; index < dbPedidoCabeceraList.Count; ++index)
                    {
                        DB_GestionResponse dbGestionResponse = new DB_GestionResponse();
                        dbGestionResponse.numsec = dbPedidoCabeceraList[index].numGestion;
                        dbGestionResponse.estado = 1;
                        dbGestionResponse.mensaje = "";
                        try
                        {
                            cmd.Transaction = cmd.Connection.BeginTransaction();
                            if (this.insertarPedidoCabeceraBackground(cmd, dbPedidoCabeceraList[index], idTask) == 1)
                                this.insertarPedidoDetalle(cmd, dbPedidoCabeceraList[index].pedidoDetalles);
                            cmd.Transaction.Commit();
                            try
                            {
                                new ProcedimientosCliente().ejecutarProcesoPedido(dbPedidoCabeceraList[index].numGestion);
                            }
                            catch
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            dbGestionResponse.estado = 0;
                            dbGestionResponse.mensaje = ex.Message;
                            cmd.Transaction.Rollback();
                        }
                        dbGestionResponseList.Add(dbGestionResponse);
                    }
                    try
                    {
                        this.actualizarBackgroundTask(cmd, idTask, scriptSerializer.Serialize((object)dbGestionResponseList));
                    }
                    catch
                    {
                    }
                }
            }
            return scriptSerializer.Serialize((object)dbGestionResponseList);
        }

        private int insertarBackgroundTask(SqlCommand cmd, string coduser)
        {
            try
            {
                cmd.CommandText = "usp_insertarBackgroundTask";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@coduser", SqlDbType.VarChar).Value = (object)coduser;
                cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = (object)1;
                cmd.Parameters.Add("@idTask", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@idTask"].Value;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private void actualizarBackgroundTask(SqlCommand cmd, int idTask, string data)
        {
            try
            {
                cmd.CommandText = "usp_actualizarBackgroundTask";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@idTask", SqlDbType.VarChar).Value = (object)idTask;
                cmd.Parameters.Add("@data", SqlDbType.VarChar).Value = (object)data;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        private int eliminarPedido(SqlCommand cmd, DB_PedidoCabecera pedido)
        {
            try
            {
                cmd.CommandText = "usp_eliminarPedidoCabecera";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@numpedido", SqlDbType.VarChar).Value = (object)pedido.numGestion;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int insertarPedidoCabecera(SqlCommand cmd, DB_PedidoCabecera pedido)
        {
            try
            {
                cmd.CommandText = "usp_insertarPedidoCabecera";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                DateTime exact1 = DateTime.ParseExact(pedido.fecha, "yyyy-MM-dd HH:mm:ss", (IFormatProvider)CultureInfo.InvariantCulture);
                DateTime exact2 = DateTime.ParseExact(pedido.fecha_entrega, "yyyy-MM-dd", (IFormatProvider)CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@numpedido", SqlDbType.VarChar).Value = (object)pedido.numGestion;
                cmd.Parameters.Add("@codvendedor", SqlDbType.VarChar).Value = (object)pedido.codven;
                cmd.Parameters.Add("@codcliente", SqlDbType.VarChar).Value = (object)pedido.codcli;
                cmd.Parameters.Add("@coddomicilio", SqlDbType.VarChar).Value = (object)pedido.coddomic;
                cmd.Parameters.Add("@condventa", SqlDbType.VarChar).Value = (object)pedido.cndvta;
                cmd.Parameters.Add("@codmoneda", SqlDbType.VarChar).Value = (object)pedido.codmon;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = (object)exact1;
                cmd.Parameters.Add("@fechaEntrega", SqlDbType.Date).Value = (object)exact2;
                cmd.Parameters.Add("@idTask", SqlDbType.VarChar).Value = (object)null;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int insertarPedidoCabeceraBackground(SqlCommand cmd, DB_PedidoCabecera pedido, int idTask)
        {
            try
            {
                cmd.CommandText = "usp_insertarPedidoCabecera";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                DateTime exact1 = DateTime.ParseExact(pedido.fecha, "yyyy-MM-dd HH:mm:ss", (IFormatProvider)CultureInfo.InvariantCulture);
                DateTime exact2 = DateTime.ParseExact(pedido.fecha_entrega, "yyyy-MM-dd", (IFormatProvider)CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@numpedido", SqlDbType.VarChar).Value = (object)pedido.numGestion;
                cmd.Parameters.Add("@codvendedor", SqlDbType.VarChar).Value = (object)pedido.codven;
                cmd.Parameters.Add("@codcliente", SqlDbType.VarChar).Value = (object)pedido.codcli;
                cmd.Parameters.Add("@coddomicilio", SqlDbType.VarChar).Value = (object)pedido.coddomic;
                cmd.Parameters.Add("@condventa", SqlDbType.VarChar).Value = (object)pedido.cndvta;
                cmd.Parameters.Add("@codmoneda", SqlDbType.VarChar).Value = (object)pedido.codmon;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = (object)exact1;
                cmd.Parameters.Add("@fechaEntrega", SqlDbType.Date).Value = (object)exact2;
                cmd.Parameters.Add("@idTask", SqlDbType.VarChar).Value = (object)idTask;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int insertarPedidoDetalle(SqlCommand cmd, List<DB_PedidoDetalle> detalles)
        {
            try
            {
                for (int index = 0; index < detalles.Count<DB_PedidoDetalle>(); ++index)
                {
                    cmd.CommandText = "usp_insertarPedidoDetalle";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@numpedido", SqlDbType.VarChar).Value = (object)detalles[index].numGestion;
                    cmd.Parameters.Add("@cantidad", SqlDbType.VarChar).Value = (object)detalles[index].cantidad;
                    cmd.Parameters.Add("@codpro", SqlDbType.VarChar).Value = (object)detalles[index].codpro;
                    cmd.Parameters.Add("@dscto", SqlDbType.VarChar).Value = (object)detalles[index].dscto;
                    cmd.Parameters.Add("@dscto1", SqlDbType.VarChar).Value = (object)detalles[index].dscto1;
                    cmd.Parameters.Add("@item", SqlDbType.VarChar).Value = (object)detalles[index].item;
                    cmd.Parameters.Add("@precio", SqlDbType.VarChar).Value = (object)detalles[index].precio;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return 1;
        }

        private int insertarJson(SqlCommand cmd,string tip_envio, string json)
        {
            try
            {
                cmd.CommandText = "usp_guardar_json";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@tipo_envio", SqlDbType.VarChar).Value = (object)tip_envio;
                cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = (object)json;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}