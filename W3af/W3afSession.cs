using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace W3af
{
    public class W3afSession:IDisposable
    {
        private string Username { get; set; }

        private string Password { get; set; }

        private IPAddress IPAddress { get; set; }

        private int ServerPort { get; set; }

        private bool Https { get; set; }

        private HttpClient Client;

     
        /// <summary>
        /// Yetkilendirme olmadan giriş yapılır.
        ///  UnAuthenticate 
        /// </summary>
        /// <param name="ip">Server IP Address</param>
        /// <param name="port">Server Port Address</param>
        public W3afSession(string ip, int port)
        {
            this.IPAddress = IPAddress.Parse(ip);
            this.ServerPort = port;
          
            this.Client = new HttpClient();
        }

    
        /// <summary>
        /// Yetkilendirme yaparak giriş yapılır.
        /// </summary>
        /// <param name="ip">Server IP Address</param>
        /// <param name="port">Server Port Address</param>
        /// <param name="username">User Name</param>
        /// <param name="password">User Password</param>
        public W3afSession(string ip, int port,string username, string password, bool https)
        {
            this.Username = username;
            this.Password = password;
            this.IPAddress = IPAddress.Parse(ip);
            this.ServerPort = port;
            this.Https = https;
        }

   
        /// <summary>
        /// HttpClient ilgili sunucuda username ve parola ile basit yetkilendirme (Basic Authentication) işlemi yapılır.
        /// </summary>
        /// <returns></returns>
        public bool Authenticate()
        {
            /* Https bağlantıları için */
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            try
            {
                this.Client = new HttpClient();
                //var byteArray = Encoding.ASCII.GetBytes(this.Username+":"+this.Password);
                var byteArray = Encoding.ASCII.GetBytes("admin:secret");
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("W3afSession::Authenticate " + ex.Message);
                return false;
            }

        }

   
        /// <summary>
        /// Servisin çalışıp çalışmadığını gösterir.
        /// </summary>
        /// <returns></returns>
        public bool W3afServiceState()
        {
            try
            {
                if (Authenticate())
                {
                    Uri serviceUrl;
                    if (this.Https)
                        serviceUrl = new Uri("https://" + this.IPAddress + ":" + this.ServerPort +   "/scans/" );
                    else
                        serviceUrl = new Uri("http://" + this.IPAddress + ":" + this.ServerPort + "/scans/");

                    Client.BaseAddress = serviceUrl;
                    var response = Client.GetAsync(serviceUrl).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("Hatalı username veya parola");
                        return false;
                    }
                    return false;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nREST-Server çalışmıyor. \nHost adresini, Port numarasını ve Servisin çalışıp çalışmadığını kontrol ediniz.\n" + ex.Message);
                //Console.WriteLine("ArachniSession::ArachniServiceState " + ex.Message);
                return false;
            }

        }


        /// <summary>
        ///  W3af servisinde istenilen komutu çalıştırır.
        /// </summary>
        /// <param name="command">Command To Run</param>
        /// <param name="request">HTTP Request Type (GET/POST/PUT/DELETE)</param>
        /// <param name="json">Json To Send</param>
        /// <returns></returns>
        public string ExecuteCommand(string command,string request,string json)
        {
            try
            {
                if (Authenticate())
                {
                    string response = TaskAsync(this.IPAddress, this.ServerPort.ToString(), command,json,request);
                    return response;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("W3afSession::ExecuteCommand" + ex.Message);
                return null;
            }
        }

  
        /// <summary>
        /// HttpClient GET/POST/PUT/DELETE isteklerinden biri ile istenilen komutu çalıştırır.
        /// </summary>
        /// <param name="ip">Server IP Address</param>
        /// <param name="servicePort">Server Port Address</param>
        /// <param name="command">Command To Run</param>
        /// <param name="json">HTTP Request Type (GET/POST/PUT/DELETE)</param>
        /// <param name="request">Json To Send</param>
        /// <returns></returns>
        private string TaskAsync(IPAddress ip, string servicePort, string command,string json,string request)
        {
            try
            {
                Uri serviceUrl;
                if (this.Https)
                    serviceUrl = new Uri("https://" + ip + ":" + servicePort + command);
                else
                    serviceUrl = new Uri("http://" + ip + ":" + servicePort + command);

        
                Client.BaseAddress = serviceUrl;

                HttpResponseMessage response;
                if (request == "GET")
                {
                      response = Client.GetAsync(serviceUrl).Result;
                }
                else if(request == "POST")
                {
                    var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
                    response = Client.PostAsync(serviceUrl,jsonContent).Result;
                }
                else if(request == "PUT")
                {
                    response = Client.PutAsync(serviceUrl,null).Result;
                }
                else if (request == "DELETE")
                {
                    response = Client.DeleteAsync(serviceUrl).Result;
                }
                else
                {
                    return null;
                }
              

                if (response != null && response.IsSuccessStatusCode)
                {

                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    //Console.WriteLine(responseString);
                    return responseString;
                }



                return null;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Server'a ulaşılmıyor." + ex.Message);
                return null;
            }
        }





        public void Dispose()
        {
           // throw new NotImplementedException();
        }
    }
}
