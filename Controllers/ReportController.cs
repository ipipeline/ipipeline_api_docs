using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;

namespace DundasBi.Controllers
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }
    public class ReportController : Controller
    {
        string baseUri = "http://dundasbipoc.ipipenet.com:8000/";
        string customAttributeId = "39826212-bb4a-492c-8812-487adb33cd20";
        /*
         * For the POC we are tgetting the GAID from ther query string
         * Note: In practice/production the GAIDs will be taken from 
         * Ping user attributes or RSA ProfileID attributes, both of which is server side only
        */
        public IActionResult Index([FromQuery]string gaid)
        {
           
            if (gaid == null) gaid = "1581";

            var sessionId = HttpContext.Session.Get<string>("Bob's sessionId");

            using (HttpClient httpClient = new HttpClient())
            {
                //  session not in cache
                if (sessionId == null)
                {
                    var logon = new DundasHelper.Logon("ReportSvcUser", "****", baseUri);
                    var result = logon.GetSessionId(httpClient);
                    if (result)
                    {
                        ViewData["Bob's sessionId"] = logon.sessionId;
                        logon.SetCustomAttribute(httpClient, customAttributeId, gaid);
                        HttpContext.Session.Set<string>("Bob's sessionId", logon.sessionId);
                    }
                    else {
                        throw new Exception("Logon Failure");
                    }
                }
                else
                {
                    var logon = new DundasHelper.Logon(baseUri,sessionId);
                    // always call set custom attribute, using  GAID in query string - for poc only
                    logon.SetCustomAttribute(httpClient, customAttributeId, gaid);
                    // from cache
                    ViewData["sessionId"] = sessionId;
                   
                }
            }
           return View();
        }      
    }
}
