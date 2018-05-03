using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W3af;
using W3af_REST_API.Model;
using W3af_REST_API.View;

namespace W3af_REST_API
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (W3afSession session = new W3afSession("206.189.96.44", 443, "ebakirmak", "1234"))
                {
                    using (W3afManager manager = new W3afManager(session))
                    {
                        if (session.W3afServiceState())
                        {
                            //PrintScan(manager);
                            CreateScan(manager);
                        }
                        else
                        {
                            Console.WriteLine("Not OKEY");
                        }
                        Console.Read();
                    }
                       
                   
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static void PrintScan(W3afManager manager)
        {
            ScanView.PrintScan(manager);
        }

        private static void CreateScan(W3afManager manager)
        {
            ScanView.CreateScan(manager);
        }
    }
}
