using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace ServiceStoneQueuePersist
{
    public partial class ServiceMQPersist : ServiceBase
    {
        private System.Timers.Timer timer = null;

        public ServiceMQPersist()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Int32 TimerSec = 1000;
            this.timer = new System.Timers.Timer(TimerSec); 
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Enabled = true;

        }
        protected override void OnStop()
        {
            this.timer.Enabled = false;
            this.timer.Stop();
            this.timer = null;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.timer.Enabled = false;

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

                            string Comando1 = string.Format("INSERT INTO SaldoHistorico (IdCliente, Valor, TipoOperacao, idOperacao) VALUES({2}, {1}, '{0}', '{3}')", ArrayRetorno[2], ArrayRetorno[1], ArrayRetorno[0], ArrayRetorno[3]);
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
                        throw;
                    };
                }

                
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
                Debug.WriteLine("Finalizando ...");
                throw;
            }
            finally
            {
                //Habilita o Timer novamente..
                this.timer.Enabled = true;
            }
        }
    }
}
