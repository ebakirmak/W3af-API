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
                ScanView.SetIPAndPort();
                using (W3afSession session = new W3afSession(ScanView.IP, ScanView.Port, ScanView.Username, ScanView.Password))
                {
                    using (W3afManager manager = new W3afManager(session))
                    {
                        if (session.W3afServiceState())
                        {
                            
                            string inputSelection = "";
                            do
                            {
                                Console.Write("\nYapmak istediğiniz işlemi seçiniz." +
                                              "\nA: Tarama Oluşturmak İçin" +
                                              "\nB: Tarama ID döndürmek için " +
                                              "\nC: Tarama Durumunu Görüntülemek İçin" +
                                              "\nD: Tarama Silmek İçin" +
                                              "\nE: Zafiyetleri Görmek İçin" +
                                              "\nF: Zafiyetleri XML Olarak Masaüstüne Kaydet" +
                                              "\nQ: Çıkış İçin" +
                                              "\nSeçiminiz: ");
                              inputSelection = Console.ReadLine().ToUpper();
                                switch (inputSelection)
                                {
                                    case "A":
                                        ScanView.CreateScan(manager);
                                        break;
                                    case "B":
                                        ScanView.GetScan(manager);
                                        break;
                                    case "C":
                                        ScanView.GetScanStatus(manager);
                                        break;
                                    case "D":
                                        ScanView.DeleteScan(manager);
                                        break;
                                    case "E":
                                        ScanView.ShowScanVulnerabilities(manager);
                                        break;
                                    case "F":
                                        ScanView.SaveScanVulnerabilitiesAsXML(manager);
                                        break;
                                    case "Q":
                                        break;
                                    default:
                                        Console.WriteLine("\n***Hatalı Seçim. Lütfen Seçiminizi kontrol ediniz.***\n");
                                        break;
                                }
                            } while (inputSelection!="Q");
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

       
    }
}
