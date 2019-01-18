using System;
using System.Collections.Generic;
using BPMClient;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var config = new BpmClientConfig("https:/bpmonline", "LOGIN", "PASSWORD");
            var client = new BpmClient(config);

            //var param = new Dictionary<string, object>
            //{
            //    {"param1", "some_value"},
            //    {"param2", DateTime.Now},
            //    {"param3",3 }
            //};



            var param = new Dictionary<string, object>
            {
                {"param1", "some_value"},
                {"param2", DateTime.Now},
                {"param3",3 }
            };
            var result = client.CallSeviceGet("SOME_SERVICE", "METHOD", param);
            Console.WriteLine(result);

        }
    }
}
