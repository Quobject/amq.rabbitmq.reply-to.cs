using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amq.rabbitmq.reply_to.cs
{
    public delegate string ServerCallback(string inputString, RpcServer rpcServer);

    public class RpcServerOptions
    {
        public static string RPC_QUEUE = "rpc_queue";
        public string ServerId = "";
        public string UserName = "guest";
        public string Password = "guest";
        public string HostName = "127.0.0.1";
        public int Port = 5672;
        public IProtocol Protocol = Protocols.DefaultProtocol;
        public ServerCallback callback = RpcServerOptions.DefaultCallback;


        private static string DefaultCallback(string inputString, RpcServer rpcServer)
        {
            return inputString;
        }
    }
}
