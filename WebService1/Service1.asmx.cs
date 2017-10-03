using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebService1.Controlador;

namespace WebService1
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
        
        [WebMethod]
        public string validarApp(string imei_movil, string version, string variable)
        {
            return new DAO_RegMovil().validarApp(imei_movil, version, variable);
        }

        [WebMethod]
        public string obtenerSincronizacion(string coduser)
        {
            return new DAO_Sincronizacion().obtenerSincronizacion(coduser);
        }

        [WebMethod]
        public string obtenerUsuario(string cadena)
        {
            return new DAO_Usuario().obtenerUsuario(cadena);
        }

        [WebMethod]
        public string obtenerBanco(string coduser)
        {
            return new DAO_Banco().obtenerBanco();
        }

        [WebMethod]
        public string obtenerCanal(string coduser)
        {
            return new DAO_Canal().obtenerCanal();
        }

        [WebMethod]
        public string obtenerCliente(string coduser)
        {
            return new DAO_Cliente().obtenerCliente(coduser);
        }

        [WebMethod]
        public string obtenerFormaPago(string coduser)
        {
            return new DAO_FormaPago().obtenerFormaPago();
        }

        [WebMethod]
        public string obtenerImpuesto(string coduser)
        {
            return new DAO_Impuesto().obtenerImpuesto();
        }

        [WebMethod]
        public string obtenerMoneda(string coduser)
        {
            return new DAO_Moneda().obtenerMoneda();
        }

        [WebMethod]
        public string obtenerMotivo(string coduser)
        {
            return new DAO_Motivo().obtenerMotivo();
        }

        [WebMethod]
        public string obtenerProducto(string coduser)
        {
            return new DAO_Producto().obtenerProducto(coduser);
        }

        [WebMethod]
        public string obtenerFamilia(string coduser)
        {
            return new DAO_Familia().obtenerFamilia(coduser);
        }

        [WebMethod]
        public string obtenerTerminoPago(string coduser)
        {
            return new DAO_TerminoPago().obtenerTerminoPago();
        }

        [WebMethod]
        public string obtenerCobranza(string coduser)
        {
            return new DAO_Cobranza().obtenerCobranza(coduser);
        }

        [WebMethod]
        public string obtenerEmpresasxImei(string imei_movil)
        {
            return new DAO_Empresa().obtenerEmpresasxImei(imei_movil);
        }

        [WebMethod]
        public string obtenerBonificacion(string coduser)
        {
            return new DAO_Bonificacion().obtenerBonificacion(coduser);
        }

        [WebMethod]
        public string guardarPedido(string cadena)
        {
            return new Pedido().insertarPedido(cadena);
        }

        [WebMethod]
        public string guardarPedidoMasivo(string coduser, string cadena)
        {
            return new Pedido().insertarPedidoMasivo(coduser, cadena);
        }

        [WebMethod]
        public int guardarDevolucion(string cadena)
        {
            return new Devolucion().insertarDevolucion(cadena);
        }

        [WebMethod]
        public string guardarDevolucionMasivo(string cadena)
        {
            return new Devolucion().insertarDevolucionMasivo(cadena);
        }

        [WebMethod]
        public int guardarCobranza(string cadena)
        {
            return new Cobranza().insertarCobranza(cadena);
        }

        [WebMethod]
        public string guardarCobranzaMasivo(string cadena)
        {
            return new Cobranza().insertarCobranzaMasivo(cadena);
        }

        [WebMethod]
        public int guardarNoPedido(string cadena)
        {
            return new NoPedido().insertarNoPedido(cadena);
        }

        [WebMethod]
        public string guardarNoPedidoMasivo(string cadena)
        {
            return new NoPedido().insertarNoPedidoMasivo(cadena);
        }
    }
}