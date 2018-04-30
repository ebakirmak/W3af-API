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

        private HttpClient Client;

        /*
         * Yetkilendirme olmadan giriş yapılır.
         * UnAuthenticate 
         */
        public W3afSession(string ip, int port)
        {
            this.IPAddress = IPAddress.Parse(ip);
            this.ServerPort = port;
            this.Client = new HttpClient();
        }

        /*
         * Yetkilendirme yaparak giriş yapılır.
         * 
         */
        public W3afSession(string ip, int port,string username, string password )
        {
            this.Username = username;
            this.Password = password;
            this.IPAddress = IPAddress.Parse(ip);
            this.ServerPort = port;
        }

        /*
        * HttpClient ilgili sunucuda username ve parola ile basit yetkilendirme (Basic Authentication) işlemi yapılır.
        * 
        */
        public bool Authenticate()
        {
            try
            {
                this.Client = new HttpClient();
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

        /*
         * Servisin çalışıp çalışmadığını gösterir.
         * 
         */
        public bool W3afServiceState()
        {
            try
            {
                if (Authenticate())
                {

                    Uri serviceUrl = new Uri("http://" + this.IPAddress + ":" + this.ServerPort + "/scans/");
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


        /*
         * W3af servisinde istenilen komutu çalıştırır.
         * 
         */
        public string GetExecuteCommand(string command)
        {
            try
            {
                if (Authenticate())
                {
                    string response = TaskAsync(this.IPAddress, this.ServerPort.ToString(), command,null,"GET");
                    return response;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("W3afSession::GetExecuteCommand" + ex.Message);
                return null;
            }
        }

        /*
         *  HttpClient GET/POST/PUT/DELETE isteklerinden biri ile istenilen komutu çalıştırır.
         * 
         */
        private string TaskAsync(IPAddress serviceHost, string servicePort, string command,string json,string request)
        {
            try
            {

                Uri serviceUrl = new Uri("http://" + serviceHost + ":" + servicePort + command);
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
