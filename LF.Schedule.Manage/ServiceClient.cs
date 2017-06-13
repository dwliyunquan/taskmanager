using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace LF.Schedule.Manage
{
    internal class ServiceClient
    {
        private readonly HttpClient _client;

        private readonly JavaScriptSerializer _serializer;

        public ServiceClient()
        {
            _serializer = new JavaScriptSerializer();
            _client = new HttpClient();
        }

        public string HttpGet(string serviceUrl)
        {
            var httpResponseMessage = _client.GetAsync(ConvertUriString(serviceUrl)).Result;
            return httpResponseMessage.Content.ReadAsStringAsync().Result;
        }

        public string HttpPost(string url, Dictionary<string, object> data)
        {
            var serializerData = Serialize(data);
            var httpResponseMessage =
                _client.PostAsync(ConvertUriString(url),
                        new StringContent(serializerData, Encoding.UTF8, "application/json"))
                    .Result;
            return httpResponseMessage.Content.ReadAsStringAsync().Result;
        }

        public TResult HttpGet<TResult>(string url)
        {
            return Deserialize<TResult>(HttpGet(url));
        }

        public TResult HttpPost<TResult>(string url, Dictionary<string, object> data)
        {
            var httpResponseResult = HttpPost(url, data);

            var deserializeResult = Deserialize<TResult>(httpResponseResult);

            return deserializeResult;

        }


        private TResult Deserialize<TResult>(string result)
        {
            try
            {
                return _serializer.Deserialize<TResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"异常信息:{ex.Message}--数据:{result}");
            }
        }

        private static Uri ConvertUriString(string serviceUrl)
        {
            var uri = new Uri(serviceUrl);
            if (!uri.IsAbsoluteUri)
            {
                throw new InvalidCastException($"{serviceUrl} 并非是一个合法的网络地址。");
            }
            return uri;
        }

        private string Serialize(Dictionary<string, object> data)
        {
            return _serializer.Serialize(data);
        }
    }
}
