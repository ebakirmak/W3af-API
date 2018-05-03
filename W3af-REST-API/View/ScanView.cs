using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W3af;
using W3af_REST_API.Controller;
using W3af_REST_API.Model;

namespace W3af_REST_API.View
{
    public static class ScanView
    {
        private static ScanController scanController { get; set; }




        /*
         * Bu fonksiyon Taramaları ekrana yazar.
         * This function writes the Scans to screen.
         */
        public static void PrintScan(W3afManager manager)
        {
            try
            {
                scanController = new ScanController();
                ScanCreateResponse scans = scanController.ListScan(manager);
                if (scans.Items.Count > 0)
                {
                    Console.WriteLine(scans.Items[0].Id);
                }
                else
                {
                    Console.WriteLine("Herhangi bir tarama mevcut değildir.");
                }
               
            }
            catch (Exception ex)
            {

                Console.WriteLine("ScanView::PrintScan() Error Message:" + ex.Message);
            }
           
        }


        /*
         * Bu fonksiyon yeni bir Tarama oluşturur ve oluşturulan ID'yi ekrana yazar.
         * 
         */
         public static void CreateScan(W3afManager manager)
        {

            string scanProfile = System.IO.File.ReadAllText(@"C:\Users\emreakirmak\Desktop\deneme.txt").Replace("\r\n", "\n");
            System.IO.File.WriteAllText(@"C:\Users\emreakirmak\Desktop\deneme2.txt", scanProfile);
            //scanProfile = "[profile]\ndescription = A profile\nname = A\n[grep.strange_headers]\n\n[crawl.web_spider]\nonly_forward = False\nfollow_regex = .*\nignore_regex = \n\n"
            string url = "[http://ebakirmak.com]";
            ScanCreate scanCreate = new ScanCreate(scanProfile,url);
            scanController = new ScanController();
            string json = JsonConvert.SerializeObject(scanCreate);
            Console.WriteLine(scanController.CreateScan(manager, json));
        }
    }
}
 