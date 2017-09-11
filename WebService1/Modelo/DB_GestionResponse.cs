using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService1.Modelo
{
    public class DB_GestionResponse
    {
        public string numsec;
        public int estado;
        public string mensaje;
        public List<DB_ProductoResponse> data;

        public DB_GestionResponse()
        {
            this.data = new List<DB_ProductoResponse>();
        }
    }
}