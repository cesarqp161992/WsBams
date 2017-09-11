using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService1.Modelo
{
    public class DB_DevolucionCabecera
    {
        public string cndvta;
        public string codcli;
        public string codcnl;
        public string coddomic;
        public string codmon;
        public string codmot;
        public string codven;
        public string fecha;
        public string fecha_entrega;
        public string numGestion;
        public string observacion;
        public string tipoGestion;
        public List<DB_DevolucionDetalle> recojoDetalles;
    }
}