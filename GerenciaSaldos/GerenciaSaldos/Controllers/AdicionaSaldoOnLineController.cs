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
    public class AdicionaSaldoOnLineController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            HttpResponseMessage Result = new HttpResponseMessage();
            Result.StatusCode = HttpStatusCode.NotImplemented;
            return Result;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string value)
        {
            HttpResponseMessage Result = new HttpResponseMessage();
            Result.StatusCode = HttpStatusCode.NotImplemented;
            return Result;
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id,string value)
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
                    string RegQueue = string.Format("{0},{1},{2}", id, value, operacao);
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