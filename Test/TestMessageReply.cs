using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class TestMessageReply
    {
        public string messageid;
        public string consumerid;
        public int content;
        public TestMessage testmessage;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TestMessageReply FromString(string json)
        {
            var result = JsonConvert.DeserializeObject<TestMessageReply>(json);
            return result;
        }
    }
}
