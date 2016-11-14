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
        public HttpResponseMessage Put(int id, decimal Valor, string IdOperacao)
        {
            HttpResponseMessage Result = new HttpResponseMessage();
            Result.StatusCode = HttpStatusCode.OK;

            decimal SaldoRetornado = 0;
            string Comando = "";

            using (STONEEntities StoneDB = new STONEEntities())
            {
                Comando = string.Format("Select Saldo from SaldoAtual where idCliente = {0}", id);
                SaldoRetornado = StoneDB.Database.SqlQuery<decimal>(Comando).SingleOrDefault();
                if (SaldoRetornado < Valor)
                {
                    Result.ReasonPhrase = "O Valor é maior que o saldo disponível";
                    Result.StatusCode = HttpStatusCode.Forbidden;
                }
                else
                {
                    string Comando1 = string.Format("INSERT INTO SaldoHistorico(IdCliente, Valor, TipoOperacao, idOperacao) VALUES({0}, {1}, '{2}', '{3}')", id, Valor, "-", IdOperacao);
                    int retorno;
                    retorno = StoneDB.Database.ExecuteSqlCommand(Comando1);
                }

            }
            return Result;
        }
    }
}