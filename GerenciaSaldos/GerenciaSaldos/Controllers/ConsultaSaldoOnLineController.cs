using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RabbitMQ.Client;
using System.Text;

namespace GerenciaSaldos.Controllers
{
    public class ConsultaSaldoOnLineController : ApiController
    {

        // GET api/<controller>/5
        public string Get(int id)
        {
            decimal SaldoRetornado = 0;


            using (STONEEntities db = new STONEEntities())
            {
                string Comando = string.Format("Select saldo from SaldoAtual where idCliente = {0}", id);
                 SaldoRetornado = db.Database.SqlQuery<decimal>(Comando).SingleOrDefault(); ;
            }
            return SaldoRetornado.ToString();
        }

 
    }
}