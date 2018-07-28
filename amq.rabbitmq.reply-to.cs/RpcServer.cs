using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amq.rabbitmq.reply_to.cs
{
    public class RpcServer
    {
        private IConnection connection;
        private IModel channel;
        private RpcServerOptions options = new RpcServerOptions();
        private static int idCounter = 0;

        private RpcServer(RpcServerOptions options)
        {
            if (options != null)
            {
                this.options = options;
            }
            if (this.options.ServerId == "")
            {
                this.options.ServerId = $"{RpcServer.idCounter++}";
            }
        }

        public static RpcServer Create(RpcServerOptions options)
        {
            var result = new RpcServer(options);

            result.Startconsuming();

            return result;
        }

        public string GetId()
        {
            return this.options.ServerId;
        }

        private void Startconsuming()
        {


            var factory = new ConnectionFactory();
            factory.UserName = this.options.UserName;
            factory.Password = this.options.Password;
            factory.VirtualHost = "/";
            factory.Protocol = this.options.Protocol;
            factory.HostName = this.options.HostName;
            factory.Port = this.options.Port;

            try
            {
                //log.Info("MessageQueueImpl hostname " + this.Context.rabbitmqHostname);
                connection = factory.CreateConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("RpcServer factory.CreateConnection() error " + e.Message);
            }

            channel = connection.CreateModel();
            channel.QueueDeclare(queue: RpcServerOptions.RPC_QUEUE,
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);
            channel.BasicQos(0, 1, false);
            //consumer = new QueueingBasicConsumer(channel);

            var consumer = new EventingBasicConsumer(channel);


            channel.BasicConsume(queue: "rpc_queue",
                                 consumer: consumer);

            consumer.Received += Consumer_Received;
            Console.WriteLine(string.Format("MessageQueueImpl id={0} started", options.ServerId));

        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            //var n = int.Parse(Encoding.UTF8.GetString(body));

            //var response = $"${n * 10}";

            var response = this.options.callback(Encoding.UTF8.GetString(body), this);

            var responseBytes = Encoding.UTF8.GetBytes(response);
            //log.InfoFormat("MessageQueueImpl: {0} before BasicPublish props.ReplyTo = {1} CorrelationId = {2}", label, props.ReplyTo, replyProps.CorrelationId);
            channel.BasicPublish(exchange: "",
                                 routingKey: props.ReplyTo,
                                 basicProperties: replyProps,
                                 body: responseBytes);
            //log.InfoFormat("MessageQueueImpl: {0} before BasicAck props.ReplyTo = {1} CorrelationId = {2}", label, props.ReplyTo, replyProps.CorrelationId);
            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                             multiple: false);

        }
    }
}
