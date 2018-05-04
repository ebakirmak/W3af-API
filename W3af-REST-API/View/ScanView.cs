using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using W3af;
using W3af_REST_API.Controller;
using W3af_REST_API.Model;
using W3af_REST_API.Model.Scan;
using W3af_REST_API.Model.Vuln;

namespace W3af_REST_API.View
{
    public static class ScanView
    {
        private static ScanController ScanController = new ScanController();





        /// <summary>
        ///  Bu fonksiyon Taramaları ekrana yazar.
        ///  This function writes the Scans to screen.
        /// </summary>
        /// <param name="manager">W3afManager Object</param>
        public static void GetScan(W3afManager manager)
        {
            try
            {
                ScanController = new ScanController();
                Scan scans = ScanController.GetScan(manager);
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

      
        /// <summary>
        /// Bu fonksiyon Tarama ID döndürür.
        /// This function returns Scan ID.
        /// </summary>
        /// <param name="manager">W3afManager Object</param>
        /// <returns></returns>
        private static string GetScanID(W3afManager manager)
        {
            try
            {
                Scan scanCreateResponse = ScanController.GetScan(manager);
                if (scanCreateResponse.Items.Count()>0)
                {
                    return scanCreateResponse.Items[0].Id.ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nScanView::GetScanID Exception:\n " + ex.Message);
                return null;
            }

           
        }

        /// <summary>
        ///  Bu fonksiyon yeni bir Tarama oluşturur ve oluşturulan ID'yi ekrana yazar.
        ///  This function creates a new Scan and created ID writes to the screen.
        /// </summary>
        /// <param name="manager"></param>
        public static void CreateScan(W3afManager manager)
        {
            try
            {
                string scanProfile = System.IO.File.ReadAllText(@"C:\Users\emreakirmak\Desktop\fast_scan.txt");
                System.IO.File.WriteAllText(@"C:\Users\emreakirmak\Desktop\deneme2.txt", scanProfile);
                //scanProfile = "[profile]\ndescription = A profile\nname = A\n[grep.strange_headers]\n\n[crawl.web_spider]\nonly_forward = False\nfollow_regex = .*\nignore_regex = \n\n"
                //scanProfile = "[crawl.web_spider]\n[profile]\nname = fast_scan\n[audit.sqli]\n[grep.lang]\n[grep.html_comments]\n[grep.form_autocomplete]\n[grep.dom_xss]";
                string url = "http://php.testsparker.com";
                ScanCreate scanCreate = new ScanCreate(scanProfile, url);
                ScanController = new ScanController();
                string json = JsonConvert.SerializeObject(scanCreate);
                string responseJson = ScanController.CreateScan(manager, json);
                ScanCreateResponse scanCreateResponse = JsonConvert.DeserializeObject<ScanCreateResponse>(responseJson);
                Console.WriteLine(scanCreateResponse.ID);
            }
            catch (Exception ex)
            {

                Console.WriteLine("ScanView::CreateScan Exception: " + ex.Message);
            }
           
        }


        /// <summary>
        ///  Bu fonksiyon ilgili taramayı siler.
        ///  This function deletes the Scan.
        /// </summary>
        /// <param name="manager">W3afManager Object</param>
        public static void DeleteScan(W3afManager manager)
        {
            try
            {
                
                string response = StopScan(manager);
        
                if (response=="Tarama Durduruldu")
                {
                    ScanController = new ScanController();
                    string ScanID = GetScanID(manager);
                    response = ScanController.DeleteScan(manager, ScanID);
                    if(response==null)
                        Console.WriteLine("***\nTarama Durdurulamadı.\n***");
                    else
                        Console.WriteLine("Tarama Silindi");
                }
                else if (response == "Tarama Yok")
                {
                    Console.WriteLine("***\nSilinecek Tarama Yok.\n***");
                }
                else if(response == "Tarama Durdurulamadı")
                {
                    Console.WriteLine("***\nTarama Durdurulamadı.\n***");
                }
         
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nScanView::DeleteScan\n Exception: " + ex.Message);
            }
            
        }


        /// <summary>
        /// Bu fonksiyon  tarama durumunu getirir.
        ///  This function gets the Scan Status
        /// </summary>
        /// <param name="manager">W3afManager Object</param>
        public static void GetScanStatus(W3afManager manager)
        {
            try
            {              
                ScanStatus scanStatus = ScanController.GetScanStatus(manager, GetScanID(manager));
                if (scanStatus != null)
                    Console.WriteLine(scanStatus.IsRunning);
                else
                    Console.WriteLine("***Gösterilecek Tarama Yok.***");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nScanView::GetScanStatus\n Exception: " + ex.Message);
             
            }
          
        }

        /// <summary>
        /// Bu fonksiyon Taramayı durdurur.
        /// This function stops the Scan.
        /// </summary>
        /// <param name="manager">W3afManager Object</param>
        public static string StopScan(W3afManager manager)
        {
            try
            {
               
                string id = GetScanID(manager);
                if (id == null)
                    return "Tarama Yok";
                ScanStatus scanStatus = ScanController.GetScanStatus(manager, id);
             

                string jsonResponse = ScanController.StopScan(manager, GetScanID(manager));               
                if (scanStatus.IsRunning == false || jsonResponse != null)
                {
                    return "Tarama Durduruldu";
                }

                return "Tarama Durdurulamadı";
            }
            catch (Exception ex)
            {
                Console.WriteLine("ScanView::StopScan Exception: " + ex.Message);
                return "Tarama Durdurulamadı";
            }
        }

        /// <summary>
        /// Bu fonksiyon taramayı duraklatır.
        /// This function pauses the Scan
        /// </summary>
        /// <param name="manager">W3afManager Object</param>
        public static void PauseScan(W3afManager manager)
        {
            try
            {              
                ScanController.PauseScan(manager, GetScanID(manager));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ScanView::PauseScan Exception: " + ex.Message);
               
            }
        }

        /// <summary>
        /// Bu fonksiyon Taramada bulunan zafiyetleri gösterir.
        /// This function shows vulnerabilities found in Scan.
        /// </summary>
        /// <param name="manager"></param>
        public static void ShowScanVulnerabilities(W3afManager manager)
        {
            try
            {
                string id = GetScanID(manager);
                if (id != null)
                {
                    Vulnerabilities vuln = ScanController.GetScanVulnerabilities(manager, id);
                    foreach (var item in vuln.Items)
                    {
                        Console.WriteLine("\nID: " + item.Id +
                                          "\nName: " + item.Name +
                                          "\nHref: " + item.Href +
                                          "\nURL: " + item.Url);

                    }
                }
                else
                    Console.WriteLine("\n\n***Tarama Yok***\n");
              
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nScanView::howScanVulnerabilities\n Exception:" + ex.Message);
            }
        }
        /// <summary>
        /// Bu fonksiyon taramada bulunan zafiyet sayısını döndürür.
        /// This function returns found vulnerability count.
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        /// <returns></returns>
        private static int GetScanVulnerabilitiesCount(W3afManager manager)
        {
            try
            {
                int VulnerabilityCount = 0;
                string id = GetScanID(manager);
                if (id != null)
                {
                    Vulnerabilities vuln = ScanController.GetScanVulnerabilities(manager, id);
                    foreach (var item in vuln.Items)
                    {
                   
                        VulnerabilityCount = Convert.ToInt32(item.Id);
                    }
                }
                else
                    Console.WriteLine("\n\n***Tarama Yok***\n");

                return VulnerabilityCount;

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nScanView::howScanVulnerabilities\n Exception:" + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Bu fonksiyon Taramada bulunan zafiyetleri XML olarak kaydeder.
        /// This function saves as XML found vulnerability in Scan.
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        public static void SaveScanVulnerabilitiesAsXML(W3afManager manager)
        {
            try
            {
                string jsonResponse = ScanController.GetScanVulnerabilitiesDetails(manager, GetScanID(manager), GetScanVulnerabilitiesCount(manager));
                XmlDocument xmlDocument = JsonConvert.DeserializeXmlNode(jsonResponse);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
 