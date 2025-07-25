using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BpDmTerminal.Models
{
    public class CardRequester
    {
        public static CardResponse IssueCard()
        {
            var url = "http://localhost:8080/issue-card/";

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                // Пустое тело запроса (если требуется)
                byte[] data = Encoding.UTF8.GetBytes("{}");
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();
                    var cardResp = JsonConvert.DeserializeObject<CardResponse>(json);
                    return cardResp;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                using (var reader = stream != null ? new StreamReader(stream) : null)
                {
                    string error = reader?.ReadToEnd();
                    Console.WriteLine("❌ Ошибка запроса: " + error);

                    return new CardResponse
                    {
                        success = false,
                        error = error
                    };
                }
            }
        }
    }
}