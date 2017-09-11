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
    public class Devolucion : Conexion
    {
        public int insertarDevolucion(string json)
        {
            DB_DevolucionCabecera devolucion = new JavaScriptSerializer().Deserialize<DB_DevolucionCabecera>(json);
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        cmd.Transaction = cmd.Connection.BeginTransaction();
                        if (this.insertarDevolucionCabecera(cmd, devolucion) == 1)
                            this.insertarDevolucionDetalle(cmd, devolucion.recojoDetalles);
                        cmd.Transaction.Commit();
                        try
                        {
                            new ProcedimientosCliente().ejecutarProcesoDevolucion(devolucion.numGestion);
                        }
                        catch
                        {
                        }
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

        public string insertarDevolucionMasivo(string json)
        {
            List<DB_GestionResponse> dbGestionResponseList = new List<DB_GestionResponse>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            List<DB_DevolucionCabecera> devolucionCabeceraList = scriptSerializer.Deserialize<List<DB_DevolucionCabecera>>(json);
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    for (int index = 0; index < devolucionCabeceraList.Count; ++index)
                    {
                        DB_GestionResponse dbGestionResponse = new DB_GestionResponse();
                        dbGestionResponse.numsec = devolucionCabeceraList[index].numGestion;
                        dbGestionResponse.estado = 1;
                        dbGestionResponse.mensaje = "";
                        try
                        {
                            cmd.Transaction = cmd.Connection.BeginTransaction();
                            if (this.insertarDevolucionCabecera(cmd, devolucionCabeceraList[index]) == 1)
                                this.insertarDevolucionDetalle(cmd, devolucionCabeceraList[index].recojoDetalles);
                            cmd.Transaction.Commit();
                            try
                            {
                                new ProcedimientosCliente().ejecutarProcesoDevolucion(devolucionCabeceraList[index].numGestion);
                            }
                            catch
                            {
                            }
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

        private int insertarDevolucionCabecera(SqlCommand cmd, DB_DevolucionCabecera devolucion)
        {
            try
            {
                cmd.CommandText = "usp_insertarDevolucionCabecera";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                DateTime exact = DateTime.ParseExact(devolucion.fecha, "yyyy-MM-dd HH:mm:ss", (IFormatProvider)CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@numdevolucion", SqlDbType.VarChar).Value = (object)devolucion.numGestion;
                cmd.Parameters.Add("@codvendedor", SqlDbType.VarChar).Value = (object)devolucion.codven;
                cmd.Parameters.Add("@codcliente", SqlDbType.VarChar).Value = (object)devolucion.codcli;
                cmd.Parameters.Add("@coddomicilio", SqlDbType.VarChar).Value = (object)devolucion.coddomic;
                cmd.Parameters.Add("@codmoneda", SqlDbType.VarChar).Value = (object)devolucion.codmon;
                cmd.Parameters.Add("@codmotivo", SqlDbType.VarChar).Value = (object)devolucion.codmot;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = (object)exact;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private int insertarDevolucionDetalle(SqlCommand cmd, List<DB_DevolucionDetalle> detalles)
        {
            try
            {
                for (int index = 0; index < detalles.Count<DB_DevolucionDetalle>(); ++index)
                {
                    cmd.CommandText = "usp_insertarDevolucionDetalle";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@numdevolucion", SqlDbType.VarChar).Value = (object)detalles[index].numGestion;
                    cmd.Parameters.Add("@cantidad", SqlDbType.VarChar).Value = (object)detalles[index].cantidad;
                    cmd.Parameters.Add("@codpro", SqlDbType.VarChar).Value = (object)detalles[index].codpro;
                    cmd.Parameters.Add("@item", SqlDbType.VarChar).Value = (object)detalles[index].item;
                    cmd.Parameters.Add("@codmotivo", SqlDbType.VarChar).Value = (object)detalles[index].codmot;
                    cmd.Parameters.Add("@fechavcto", SqlDbType.VarChar).Value = (object)detalles[index].fechavcto;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }
    }
}