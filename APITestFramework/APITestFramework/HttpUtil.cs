
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APITestFramework
{
    public class HttpUtil
    {
        private string contentType;
        public HttpClient Client
        {
            get; set;
        }
        //Constructor 
        public HttpUtil(string configLocation, string contentType = "Application/json", bool authorize = true)
        {
            string file_path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + configLocation;
            dynamic data = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            this.contentType = contentType;

            //To handle certification error
            //Used to ignore bad certificate
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyerrors) =>{return true;}
                
            };

             Client = new HttpClient(handler);
            //Create client with default content type
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(this.contentType));
            string url = data.ApiBaseUrl.ToString();
            Client.BaseAddress = new Uri(url);

            //for authentication
            if (authorize)
            { //set authorization header with Bearer
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
            }
        }
        private enum Method
        {
            GET,
            POST,
            PUT,
            DELETE,
            PATCH
        }

        private async Task<HttpResponseMessage> Request(Method method, string endpoint, string body = null)
        { 
            switch (method)
            {
                case Method.GET:
                    return await Client.GetAsync(endpoint);
                case Method.POST:
                    return await Client.PostAsync(endpoint, new StringContent(body, Encoding.UTF8, contentType));
                case Method.PUT:
                    return await Client.PutAsync(endpoint, new StringContent(body, Encoding.UTF8, contentType));

                case Method.DELETE:
                    return await Client.DeleteAsync(endpoint);

                default:
                    return null;
            }
        }
        public HttpResponseMessage GetRequest(string endpoint)
        {
            Task<HttpResponseMessage> task = Request(Method.GET, endpoint);
            task.Wait();
            return task.Result;
        }
        //Method for call post API request by passing jsonbody object as parameter
        public HttpResponseMessage PostRequest(string endpoint, object body)
        {
            string jsonBody = SerializeToJson(body);
            Task<HttpResponseMessage> task = Request(Method.POST, endpoint, jsonBody);
            task.Wait();
            return task.Result;
        }
        public HttpResponseMessage PutRequest(string endpoint, object body)
        {
            string jsonBody = SerializeToJson(body);
            Task<HttpResponseMessage> task = Request(Method.PUT, endpoint, jsonBody);
            task.Wait();
            return task.Result;
        }

        public HttpResponseMessage PatchRequest(string endpoint, string body)
        {

            Task<HttpResponseMessage> task = Request(Method.PATCH, endpoint, body);
            task.Wait();
            return task.Result;
        }
        public HttpResponseMessage DeleteRequest(string endpoint)
        {

            Task<HttpResponseMessage> task = Request(Method.DELETE, endpoint);
            task.Wait();
            return task.Result;
        }
        /// <summary>
        /// this method for get the whole response from httpresponse message as we called as actual response
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public string GetResponseBody(HttpResponseMessage responseMessage)
        {
            Task<string> task = responseMessage.Content.ReadAsStringAsync();
            task.Wait();
            return task.Result;
        }
        //To convert object to json string
        public string SerializeToJson(object obj)
        {
            string json;

            json = JsonConvert.SerializeObject(obj);

            return json;
        }
        
    }
}

