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
    public class Cobranza : Conexion
    {
        public int insertarCobranza(string json)
        {
            DB_Cobranza cobranza = new JavaScriptSerializer().Deserialize<DB_Cobranza>(json);
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        cmd.Transaction = cmd.Connection.BeginTransaction();
                        this.insertarCobranza(cmd, cobranza);
                        cmd.Transaction.Commit();
                        try
                        {
                            new ProcedimientosCliente().ejecutarProcesoCobranza(cobranza.numsec);
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

        public string insertarCobranzaMasivo(string json)
        {
            List<DB_GestionResponse> dbGestionResponseList = new List<DB_GestionResponse>();
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            List<DB_Cobranza> dbCobranzaList = scriptSerializer.Deserialize<List<DB_Cobranza>>(json);
            using (SqlConnection sqlConnection = this.conectar())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    for (int index = 0; index < dbCobranzaList.Count; ++index)
                    {
                        DB_GestionResponse dbGestionResponse = new DB_GestionResponse();
                        dbGestionResponse.numsec = dbCobranzaList[index].numsec;
                        dbGestionResponse.estado = 1;
                        dbGestionResponse.mensaje = "";
                        try
                        {
                            cmd.Transaction = cmd.Connection.BeginTransaction();
                            this.insertarCobranza(cmd, dbCobranzaList[index]);
                            cmd.Transaction.Commit();
                            try
                            {
                                new ProcedimientosCliente().ejecutarProcesoCobranza(dbCobranzaList[index].numsec);
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

        private int insertarCobranza(SqlCommand cmd, DB_Cobranza cobranza)
        {
            try
            {
                cmd.CommandText = "usp_insertarCobranza";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                DateTime exact = DateTime.ParseExact(cobranza.fecha, "yyyy-MM-dd HH:mm:ss", (IFormatProvider)CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@numcobranza", SqlDbType.VarChar).Value = (object)cobranza.numsec;
                cmd.Parameters.Add("@codvendedor", SqlDbType.VarChar).Value = (object)cobranza.codven;
                cmd.Parameters.Add("@codcliente", SqlDbType.VarChar).Value = (object)cobranza.codcli;
                cmd.Parameters.Add("@coddoc", SqlDbType.VarChar).Value = (object)cobranza.coddoc;
                cmd.Parameters.Add("@numdoc", SqlDbType.VarChar).Value = (object)cobranza.numdoc;
                cmd.Parameters.Add("@importe", SqlDbType.VarChar).Value = (object)cobranza.importe;
                cmd.Parameters.Add("@codmoneda", SqlDbType.VarChar).Value = (object)cobranza.codmon;
                cmd.Parameters.Add("@codformapago", SqlDbType.VarChar).Value = (object)cobranza.codforpag;
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