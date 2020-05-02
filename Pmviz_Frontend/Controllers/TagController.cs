using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pmviz_Frontend.Models;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Pmviz_Frontend.Controllers
{
    public class TagController : Controller
    {
        string rfidJson = "";
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                // GET ALL MOULDS
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/moulds"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var moulds = JsonConvert.DeserializeObject<List<Mould>>(apiResponse);
                        ViewData["moulds"] = moulds;
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Moulds not available. Please try again later.";
                        return View();
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(int mouldSelected)
        {
            using (var httpClient = new HttpClient())
            {
                // GET ALL MOULDS
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/moulds"))
                {
                    var moulds = new List<Mould>();
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        moulds = JsonConvert.DeserializeObject<List<Mould>>(apiResponse);
                        ViewData["moulds"] = moulds;
                    }
                    else
                    {
                        ViewBag.Error = "Moulds not available. Please try again later.";
                    }
                }
                // GET ALL PARTS FROM THAT MOULD
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/moulds/" + mouldSelected + "/parts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var parts = JsonConvert.DeserializeObject<List<Part>>(apiResponse);
                        ViewData["parts"] = parts;
                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorPart = "Parts not available. Please try again later.";
                        return View();
                    }
                }
            }

        }

        public void Tag([FromQuery(Name = "id")] string codePart)
        {
            Connect(codePart);
        }

        public void Connect(string codePart)
        {
            rfidJson = "";
            string brokerAddress = "test.mosquitto.org";
            MqttClient mClient = new MqttClient(brokerAddress);
            string clientId = Guid.NewGuid().ToString();
            mClient.Connect(clientId);


            if (!mClient.IsConnected)
            {
                ViewBag.ErrorConn = "Error connecting. Please try again later!";
            }
            else
            {
                Parallel.Invoke
                (
                    () => Publish(mClient, codePart),
                    () => Subscribe(mClient, codePart)

                );

                var obj = JObject.Parse(rfidJson);
                var rfid = obj["rfid"];

                //SubmitRFID(codePart, rfid.ToString());


            }
        }

        public async void SubmitRFID(string partCode, string rfid)
        {
            /*Part part = new Part()
            {
                Code = partCode,
                Rfid = rfid
            };*/

            var content = new StringContent("");

            using (var httpClient = new HttpClient())
            {
              
                using (var response = await httpClient.PutAsync("http://localhost:8080/api/parts/"+partCode+"/tag/"+rfid, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        System.Diagnostics.Debug.WriteLine(apiResponse);
                        
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(apiResponse);
                        ViewBag.Error = "Moulds not available. Please try again later.";;
                    }
                }
            }
        }

        public void Publish(MqttClient mClient, string codePart)
        {
            string topic = "tagPart";

            mClient.Publish(topic, Encoding.UTF8.GetBytes(codePart), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

            return;

        }


        public void Subscribe(MqttClient mClient, string codePart)
        {
            string topic = "part" + codePart;

            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            mClient.Subscribe(new string[] { topic }, qosLevels);
            do
            {
                mClient.MqttMsgPublishReceived += MClient_MqttMsgPublishedReceived;

            } while (rfidJson == "");

            return;
        }

        private void MClient_MqttMsgPublishedReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Message);

            // FALTA VALIDAÇÃO PARA NÂO REBENTAR


            rfidJson = msg;

        }
    }
}