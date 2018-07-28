using amq.rabbitmq.reply_to.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new RpcServerOptions();
            options.callback = Program.Callback;
            var rpcServer = RpcServer.Create(options);
        }

        private static string CallbackTimesTen(string inputString, RpcServer rpcServer)
        {
            var n = int.Parse(inputString);

            var response = $"@{n * 10}";
            return response;
        }

        private static string Callback(string inputString, RpcServer rpcServer)
        {
            var testMessage = TestMessage.FromString(inputString);
            var testMessageReply = new TestMessageReply();
            testMessageReply.messageid = "0";
            testMessageReply.consumerid = rpcServer.GetId();
            testMessageReply.content = testMessage.content * 10;
            testMessageReply.testmessage = testMessage;

            var result = testMessageReply.ToJson();

            return result;

        }
    }
}
