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
    public class RemoveSaldoOnLineController : ApiController
    {

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, decimal value)
        {
            HttpResponseMessage Result = new HttpResponseMessage();
            Result.StatusCode = HttpStatusCode.OK;

            decimal SaldoRetornado = 0;
            string Comando = "";

            using (STONEEntities db = new STONEEntities())
            {
                Comando = string.Format("Select Saldo from SaldoAtual where idCliente = {0}", id);
                SaldoRetornado = db.Database.SqlQuery<decimal>(Comando).SingleOrDefault(); 
                if (SaldoRetornado < value)
                {
                    Result.ReasonPhrase = "O Valor é maior que o saldo disponível";
                    Result.StatusCode = HttpStatusCode.Forbidden;
                }
                else
                {
                    try
                    {
                        Comando = string.Format("Update SaldoAtual set Saldo = (Saldo - {0}) where idCliente = {1}", value, id);
                        int retorno = db.Database.ExecuteSqlCommand(Comando);
                    }
                    catch (Exception ex)
                    {
                        Result.ReasonPhrase = ex.Message;
                        Result.StatusCode = HttpStatusCode.BadRequest;
                    }
                }

            }
            return Result;
        }
    }
}