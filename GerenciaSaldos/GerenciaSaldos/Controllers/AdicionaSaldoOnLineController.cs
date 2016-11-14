using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using System.Diagnostics;

namespace GerenciaSaldos.Controllers
{
    public class AdicionaSaldoOnLineController : ApiController
    {




        // GET api/<controller>
        public HttpResponseMessage Get() // criado para testar a operação do Serviço.
        {
            HttpResponseMessage Result = new HttpResponseMessage();
            Result.StatusCode = HttpStatusCode.NotModified;


            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "SaldoOnLine", durable: false, exclusive: false, autoDelete: false, arguments: null);
                RabbitMQ.Client.BasicGetResult Mensagem;
                try
                {
                    uint MensagensDisponiveis = channel.MessageCount("SaldoOnLine");
                    for (int Msgs = 1; Msgs <= MensagensDisponiveis; Msgs++)
                    {
                        Mensagem = channel.BasicGet("SaldoOnLine", true);
                        string MensagemAGravar = Encoding.UTF8.GetString(Mensagem.Body);
                        var ArrayRetorno = MensagemAGravar.Split(','); // Id,Valor,Operacao
                        
                        string Comando1 = string.Format("INSERT INTO SaldoHistorico(IdCliente, Valor, TipoOperacao, idOperacao) VALUES({2}, {1}, '{0}', '{3}')", ArrayRetorno[2], ArrayRetorno[1], ArrayRetorno[0], ArrayRetorno[3]);
                        using (var StoneDB = new STONEEntities())
                        {
                            int retorno;
                            retorno = StoneDB.Database.ExecuteSqlCommand(Comando1);
 
                        }

                    }
                }
                catch (Exception Ex1)
                {
                    Debug.WriteLine(Ex1.Message);
                    Debug.WriteLine("Finalizando ...");
                };
            }


            return Result;
        }

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string value)
        {
            HttpResponseMessage Result = new HttpResponseMessage();
            Result.StatusCode = HttpStatusCode.NotImplemented;
            return Result;
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, decimal Valor,string IdOperacao)
        {
            HttpResponseMessage Result = new HttpResponseMessage();
            Result.StatusCode = HttpStatusCode.OK;
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "SaldoOnLine", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    string operacao = "+";
                    string RegQueue = string.Format("{0},{1},{2},{3}", id, Valor, operacao, IdOperacao);
                    var body = Encoding.UTF8.GetBytes(RegQueue);
                    channel.BasicPublish(exchange: "", routingKey: "SaldoOnLine", basicProperties: null, body: body);
                }
            }
            catch
            {
                Result.StatusCode = HttpStatusCode.BadRequest;
            }
            return Result;
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}