using Examen.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Examen.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            PersonaModel modelo = new PersonaModel();
            return View(modelo);
        }

        public JsonResult Grabar(PersonaModel modelo)
        {
            int tipo = 0;
            string mensaje = "";
            try
            {
                OracleConnection bdConn= new OracleConnection("Data Source=ORCL; User ID=PRUEBA;Password=12345;Pooling=False;");
                OracleParameter[] bdParameters = new OracleParameter[2];
                bdParameters[0] = new OracleParameter("P_NOMBRE", OracleDbType.Varchar2) { Value = modelo.nombre };
                bdParameters[1] = new OracleParameter("P_APELLIDO", OracleDbType.Varchar2) { Value = modelo.apellido };
                bdParameters[2] = new OracleParameter("P_DIRECCION", OracleDbType.Varchar2) { Value = modelo.direccion };
                using (var bdCmd = new OracleCommand("SP_PERSONA_GRABAR", bdConn))
                {
                    bdCmd.CommandType = CommandType.StoredProcedure;
                    bdCmd.Parameters.AddRange(bdParameters);
                    bdCmd.ExecuteNonQuery();
                    bdConn.Close();
                    tipo = 1;
                    mensaje = "Registro guardado";
                }
            }
            catch (Exception ex)
            {
                tipo = 0;
                mensaje = ex.Message;
            }
            return Json(new { tipo = tipo, mensaje = mensaje });
        }
    }
}