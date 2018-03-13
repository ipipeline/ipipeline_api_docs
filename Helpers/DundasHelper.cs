using System;
using System.Net.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json;


namespace DundasBi.DundasHelper
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Logon
    {
        public string logOnToken;
        public string sessionId;
        string username;
        string password;
        string baseUri;

        public Logon(string baseUri, string sessionId)
        {
            this.baseUri = baseUri;
            this.sessionId = sessionId;
        }
        public Logon(string username, string password, string baseUri)
        {
            this.username = username;
            this.password = password;
            this.baseUri = baseUri;
        }

        private string GetLogonToken(HttpClient client)
        {
            string url = baseUri + "API/LogOn/Token/";
            var Token = "";
            var logonOptions = new
            {
                accountName = this.username,
                password = this.password,
                cultureName = string.Empty,
                deleteOtherSessions = false,
                isWindowsLogOn = false

            };

            var requestBodyAsString = JsonConvert.SerializeObject(logonOptions);

            StringContent requestBody =
              new StringContent(
                  requestBodyAsString,
                  Encoding.UTF8,
                  "application/json"
                  );

            using (var response2 = client.PostAsync(url, requestBody).Result)
            {
                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    string jsonObject = response2.Content.ReadAsStringAsync().Result;
                    dynamic obj = JsonConvert.DeserializeObject(jsonObject);
                    if (obj.logOnFailureReason.ToString() == "None")
                    {
                        Token = obj.logOnToken.ToString();
                    }
                    else
                    {
                        throw new Exception("Login failed: GetLogonToken");
                    }
                }
                else
                    return "";
            }
            return Token;
        }

        internal bool SetCustomAttribute(HttpClient client, string id, string gaid)
        {
            var result = false;
            try
            {
                var ga = gaid.Split(",");

                var customAttributes = new[] {
                   new {
                        key = id,
                        value = new
                        {
                            attributeId = id,
                            values = new string[ga.Length]
                        }
                    }
                };

                int i = 0;
                foreach (string g in ga)
                {
                    customAttributes[0].value.values[i++] = g;
                }
                var requestBodyAsString = JsonConvert.SerializeObject(customAttributes);
                Console.WriteLine(requestBodyAsString);

                StringContent content =
                    new StringContent(
                        requestBodyAsString,
                        Encoding.UTF8,
                        "application/json"
                        );
                var url = baseUri + "API/Session/CustomAttributes/" + this.sessionId + "?sessionId=" + this.sessionId;
                using (var response = client.PostAsync(url, content).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = true;
                    }
                    else
                    {
                        throw new Exception("SetCustomAttribute: " + response.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public bool GetSessionId(HttpClient client)
        {

            string logonUri = baseUri + "Api/LogOn/";
            string result = string.Empty;

            var logonOptions = new
            {
                accountName = username,
                password = password,
                cultureName = string.Empty,
                deleteOtherSessions = false,
                isWindowsLogOn = false

            };


            var requestBodyAsString = JsonConvert.SerializeObject(logonOptions);

            StringContent content =
                new StringContent(
                    requestBodyAsString,
                    Encoding.UTF8,
                    "application/json"
                    );

            using (var response = client.PostAsync(logonUri, content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonString =
                        response.Content.ReadAsStringAsync().Result;

                    dynamic obj = JsonConvert.DeserializeObject(jsonString);

                    if (obj.logOnFailureReason.ToString() == "None")
                    {
                        result = obj.sessionId.ToString();
                    }
                    else
                    {
                        throw new Exception("Login failed");
                    }

                }

            }
            this.sessionId = result;
            return true;
        }
    }
}
