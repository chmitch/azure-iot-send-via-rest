using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace IOTHubRest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Build the object that will be converted to json
            IOTMessage body = new IOTMessage { Timestamp = DateTime.Now.ToString(), Property1 = "Some data" };

            //Setup some constants.
            string iotHub = "[iothub]";
            string deviceId = "[deviceid]";
            string api = "2016-02-03";

            //Build rest endpoint username etc.
            string restUri = String.Format("https://{0}.azure-devices.net/devices/{1}/messages/events?api-version={2}", iotHub, deviceId, api);

            //Use the Device Explorer from the Azure IOT SDK to Generate this
            //https://github.com/Azure/azure-iot-sdks/tree/master/tools/DeviceExplorer

            string sas = "SharedAccessSignature sr=[iothub].azure-devices.net%2Fdevices%2F[deviceid]&sig=[sastoken]&se=[expiration]";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", sas);

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var result = client.PostAsync(restUri, content).Result;

            if (result.StatusCode.ToString() != "204")
            {
                Console.WriteLine("Message Failed with code {0}", result.StatusCode.ToString());
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }

    public class IOTMessage
    {
        public string Timestamp { get; set; }
        public string Property1 { get; set; }
    }
}
