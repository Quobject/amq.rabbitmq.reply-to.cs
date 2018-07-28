using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class TestMessage
    {
        public string messageid;
        public string clientid;
        public int content;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TestMessage FromString(string json)
        {
            var result = JsonConvert.DeserializeObject<TestMessage>(json);
            return result;
        }
    }
}
