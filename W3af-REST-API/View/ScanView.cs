using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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


        //Server IP
        public static string IP { get; set; }
        //Server Port
        public static int Port { get; set; }
        //Username
        public static string Username { get; set; }
        //Password
        public static string Password { get; set; }
        //Certificate
        public static bool Certificate { get; set; }

        /// <summary>
        /// Bu fonksiyon ip ve port numaralarını ayarlar.
        /// This function sets up ip and port number.
        /// </summary>
        public static void SetIPAndPort()
        {
            do
            {
                try
                {

                    Console.Write("IP Adresi ve Port Adresini değiştirmek istiyor musunuz?(E/H)");
                    string selected = Console.ReadLine().ToUpper();
                    if (selected == "E")
                    {
                        Console.Write("IP Adresini Giriniz: ");
                        IP = Console.ReadLine();

                        Console.Write("Port Numarasını Giriniz: ");
                        Port = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Username Giriniz: ");
                        Username = Console.ReadLine();

                        Console.Write("Parola Giriniz: ");
                        Password = Console.ReadLine();

                        Console.Write("Bağlantı Https üzerinden mi? (E/H): ");
                        string certificate = Console.ReadLine();
                        Certificate = certificate.ToUpper() == "E" ? true : false;

                        break;
                    }
                    else if (selected == "H")
                    {
                        IP = "172.17.7.221";
                        Port = 5000;
                        Username = "admin";
                        Password = "secret";
                        Certificate = true;
                        break;
                    }

                }
                catch (FormatException e)
                {
                    Console.WriteLine("Input format biçimi hatalı. Kontrol ediniz." + e.Message);
                    //throw;
                }
            } while (true);


        }



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

                    Console.WriteLine("Devam Eden Tarama ID: " + scans.Items[0].Id);
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

            string currentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            try
            {
                //Profile Name is scan settings namely it is policy. Profile Adı tarama ayarlarıdır yani policydir.
                string scanProfileName = SelectProfile();
                Console.WriteLine(scanProfileName);
            

                string scanProfile = System.IO.File.ReadAllText(currentDir+"\\Model\\Policys\\"+scanProfileName);
                string targetURL = SelectTargetURL();


                ScanCreate scanCreate = new ScanCreate(scanProfile, targetURL);
                ScanController = new ScanController();


                string json = JsonConvert.SerializeObject(scanCreate);
                string responseJson = ScanController.CreateScan(manager, json);

                if(responseJson == null)
                {
                    Console.WriteLine("Sistemde herhangi bir tarama mevcut ise öncelikle onu siliniz.");
                    return;
                }

                ScanCreateResponse scanCreateResponse = JsonConvert.DeserializeObject<ScanCreateResponse>(responseJson);
                Console.WriteLine("Oluşturulan Tarama ID: " + scanCreateResponse.ID);
            }
            catch (Exception ex)
            {

                Console.WriteLine("ScanView::CreateScan Exception: " + ex.Message);
            }
           
        }

        /// <summary>
        /// Bu fonksiyon tarama profillerini (policy) listeler.
        /// This function lists scan profiles (policy).
        /// </summary>
        /// <returns></returns>
        private static string SelectProfile()
        {
            try
            {
                string[] w3afFiles = Directory.GetFiles(@"..\\..\\Model\\Policys\\", "*.pw3af")
                                   .Select(Path.GetFileName)
                                   .ToArray();
                int counter = 1;
                Console.Write("\n");
                foreach (var item in w3afFiles)
                {
                    Console.WriteLine(counter + ") " + item.ToString());
                    counter += 1;
                }

                Console.Write("\n Policy Seçiniz: ");
                int policyId = Convert.ToInt32(Console.ReadLine());
                return w3afFiles[policyId - 1];
            }
            catch (Exception ex)  
            {
                Console.WriteLine("Scanview::SelectProfile Exception: " + ex.Message);
                return null;
            }
          
        }

        /// <summary>
        /// Bu fonksiyon taranacak hedefi seçer.
        /// This function selects scan target.
        /// </summary>
        /// <returns></returns>
        private static string SelectTargetURL()
        {
            Console.Write("Taramak istediğiniz hedef:");
            return Console.ReadLine().ToLower();
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
                if (scanStatus != null && scanStatus.IsRunning.ToString().ToLower() == "true")
                    Console.WriteLine("Tarama Devam Ediyor.");
                else if (scanStatus != null && scanStatus.IsRunning.ToString().ToLower() == "false")
                    Console.WriteLine("Tarama Sona Erdi.");
                else if (scanStatus==null)
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
                string scanID = GetScanID(manager);
                int vulnCount = GetScanVulnerabilitiesCount(manager);
                string jsonResponse = ScanController.GetScanVulnerabilitiesDetails(manager, scanID , vulnCount);
                XmlDocument xmlDocument = JsonConvert.DeserializeXmlNode("{\"Row\":" + jsonResponse + "}", "root");
                string strPath = Environment.GetFolderPath(
                          System.Environment.SpecialFolder.DesktopDirectory);
                System.IO.File.WriteAllText(strPath + "\\w3af.xml",xmlDocument.InnerXml);
                Console.WriteLine("Masaüstüne Kaydedildi.");
            }
            catch (Exception ex)
            {
               Console.WriteLine("ScanView::SaveScanVulnerabiliesAsXML Exception " + ex.Message);
            }
        }
    }
}
 