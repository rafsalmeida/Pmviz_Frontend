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
using Newtonsoft.Json.Schema;
using Pmviz_Frontend.Models;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Pmviz_Frontend.Controllers
{
    public class TagController : Controller
    {
        string rfidJson = "";
        public async Task<IActionResult> Index([FromQuery(Name ="error")]string error)
        {
            if(error == "1")
            {
                ViewBag.Error = "Invalid RFID!";
            }
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
        public async Task<IActionResult> Indexx(string mouldSelected)
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
                        return View("Index", "Tag");
                    }
                    else
                    {
                        ViewBag.ErrorPart = "Parts not available. Please try again later.";
                        return View("Index", "Tag");
                    }
                }
            }

        }

        public async Task<IActionResult> Tag([FromQuery(Name = "id")] string codePart)
        {
            #region Connect to Broker
            rfidJson = "";

            // CONNECT TO BROKER
            string brokerAddress = "test.mosquitto.org";
            MqttClient mClient = new MqttClient(brokerAddress);
            string clientId = Guid.NewGuid().ToString();
            mClient.Connect(clientId);


            if (!mClient.IsConnected)
            {
                TempData["ErrorConn"] = "Error connecting. Please try again later!";
                return RedirectToAction("Index", "Tag");
            }
            else
            {
                // INVOKE PUBLISH AND SUBSCRIBE AT THE SAME TIME
                Parallel.Invoke
                (
                    () => Publish(mClient, codePart),
                    () => Subscribe(mClient, codePart)

                );
                string topic = "part" + codePart;

                if(rfidJson == "1")
                {
                    return RedirectToAction("Index", "Tag", new { error = "1"});

                }
                var obj = JObject.Parse(rfidJson);
                var rfid = obj["rfid"];


                #region Consume endpoint
                string payload = JsonConvert.SerializeObject("");

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {

                    using (var response = await httpClient.PutAsync("http://localhost:8080/api/parts/" + codePart + "/tag/" + rfid, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var status = response.IsSuccessStatusCode;
                        if (status == true)
                        {
                            // DISCONNECT AND UNSUBSCRIBE
                            mClient.Unsubscribe(new string[] { topic });
                            mClient.Disconnect();

                            TempData["SuccessRFID"] = "Part with the code " + codePart + " sucessfully tagged with the rfid: " +rfid ;

                            return RedirectToAction("Index", "Tag");
                        }
                        else
                        {
                            // DISCONNECT AND UNSUBSCRIBE

                            mClient.Unsubscribe(new string[] { topic });
                            mClient.Disconnect();

                            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                            {
                                TempData["ErrorRFID"] = "Tag not found. Use an available RFID.";
                                return RedirectToAction("Index", "Tag");
                            }

                            TempData["ErrorRFID"] = "Error saving the RFID. Please try again later!";
                            return RedirectToAction("Index", "Tag");

                        }
                    }
                }
                #endregion



            }
            #endregion
        }


        public IActionResult Cancel([FromQuery(Name = "id")] string codePart)
        {
            // CONNECT TO BROKER
            string brokerAddress = "test.mosquitto.org";
            MqttClient mClient = new MqttClient(brokerAddress);
            string clientId = Guid.NewGuid().ToString();
            mClient.Connect(clientId);


            if (!mClient.IsConnected)
            {
                TempData["ErrorConn"] = "Error connecting. Please try again later!";
                return RedirectToAction("Index", "Tag");
            } else
            {
                string topic = "cancelTagPart";

                mClient.Publish(topic, Encoding.UTF8.GetBytes(codePart), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                //mClient.Disconnect();

                TempData["Cancel"] = "Cancelation of tagging part "+codePart +" successfull.";

                return RedirectToAction("Index", "Tag");
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
            try
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
            catch (System.OutOfMemoryException e)
            {
                return;
            }



        }

        private void MClient_MqttMsgPublishedReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Message);

            if (ValidateJSON(msg))
            {
                rfidJson = msg;

            }  else
            {
                rfidJson = "1";
            }


        }

        public bool ValidateJSON(string msgReceived)
        {
            msgReceived = msgReceived.Trim();
            if ((msgReceived.StartsWith("{") && msgReceived.EndsWith("}"))) 
            {
                try
                {
                    var obj = JObject.Parse(msgReceived);
                    System.Diagnostics.Debug.WriteLine(obj.ContainsKey("rfid"));
                    if (!obj.ContainsKey("rfid"))
                    {
                        return false;
                    }
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}