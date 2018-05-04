using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W3af;
using W3af_REST_API.Model;
using W3af_REST_API.Model.Scan;
using W3af_REST_API.Model.Vuln;
using W3af_REST_API.View;

namespace W3af_REST_API.Controller
{
    public class ScanController
    {

        public ScanController()
        {

        }

        /// <summary>
        /// Bu fonksiyon Taramaları döndürür.
        /// This function  returns the scans.
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        /// <returns></returns>
        public Scan GetScan(W3afManager manager)
        {
            try
            {
                string json = manager.GetScans();
                var scans = JsonConvert.DeserializeObject<Scan>(json);

                return scans;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Bu fonksiyon yeni bir Tarama oluşturur.
        /// This function  creates a new Scan.
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        /// <param name="json">String in valid JSON type</param>
        /// <returns></returns>
        public string CreateScan(W3afManager manager, string json)
        {
            try
            {
                return manager.CreateScan(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// Bu fonksiyon taramayı siler.
        /// This function deletes the Scan
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        /// <param name="id">Scan ID</param>
        /// <returns></returns>
        public string DeleteScan(W3afManager manager, string id)
        {
            try
            {
                return manager.DeleteScan(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        ///  Bu fonksiyon  tarama durumunu getirir.
        ///  This function gets the Scan Status
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        /// <param name="id">Scan ID</param>
        /// <returns></returns>
        public ScanStatus GetScanStatus(W3afManager manager, string id)
        {
            try
            {
                if (id == null)
                    return null;
                string json = manager.GetScanStatus(id);
                ScanStatus scanStatus = JsonConvert.DeserializeObject<ScanStatus>(json);
                if (scanStatus != null)
                    return scanStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ScanController::GetScanStatus Message:" + ex.Message);
                return null;
            }

            return null;
        }



        /// <summary>
        /// Bu fonksiyon taramayı durdurur.
        ///  This function stopped the Scan.
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        /// <param name="id">Scan ID</param>
        /// <returns></returns>
        public string StopScan(W3afManager manager, string id)
        {
            return manager.StopScan(id);
        }

        /// <summary>
        /// Bu fonksiyon taramayı duraklatır.
        /// This function pauses the Scan.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string PauseScan(W3afManager manager,string id)
        {
            return manager.PauseScan(id);
        }

        /// <summary>
        /// Bu fonksiyon taramada bulunan zafiyetleri döndürür.
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        /// <param name="id">Scan ID</param>
        /// <returns></returns>
        public Vulnerabilities GetScanVulnerabilities(W3afManager manager, string id)
        {
            try
            {
                string response = manager.GetScanVulnerabilities(id);
                Vulnerabilities vuln;
                if (response != null)
                    return vuln = JsonConvert.DeserializeObject<Vulnerabilities>(response);
           
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ScanController::GetScanVulnerabilities Exception: " + ex.Message);
                return null;
            }
           
        }

        /// <summary>
        /// Bu fonksiyon taramada bulunan tüm zafiyetlerin detaylarını döndürür.
        /// This function returns found all vulnerebilities details.
        /// </summary>
        /// <param name="manager">W3afManager Instance</param>
        /// <param name="scanId">Scan ID</param>
        /// <param name="lastVulnerability">Last Vulnerability ID</param>
        /// <returns></returns>
        public string GetScanVulnerabilitiesDetails(W3afManager manager, string scanId, int lastVulnerability)
        {
            //List<VulnerabilityDetails> vulnerabilitiesDetails = new List<VulnerabilityDetails>();
            string jsonResponse="";
            for (int i = 0; i < lastVulnerability; i++)
            {
              jsonResponse  += manager.GetScanVulnerabilityDetails(scanId, i.ToString());
                //VulnerabilityDetails vulnerabilitiy = JsonConvert.DeserializeObject<VulnerabilityDetails>(jsonResponse);
                //vulnerabilitiesDetails.Add(vulnerabilitiy);
            }

            //return vulnerabilitiesDetails;
            return jsonResponse;
        }
    }
}
