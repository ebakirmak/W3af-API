using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W3af
{
    public class W3afManager:IDisposable
    {
        private W3afSession Session { get; set; }

        public W3afManager(W3afSession session)
        {
            if (session != null)
            {
                this.Session = session;
            }
        }

 
        /// <summary>
        /// Bu fonksiyon devam eden Taramayı döndürür.
        /// This function return continued Scan.
        /// </summary>
        /// <returns></returns>
        public string GetScans()
        {
            return Session.ExecuteCommand("/scans/","GET",null);

        }


        /// <summary>
        /// Bu fonksiyon yeni bir tarama oluşturur.
        ///  This function creates a new Scan.
        /// </summary>
        /// <param name="json">String in valid JSON type</param>
        /// <returns></returns>
        public string CreateScan(string json)
        {
            return Session.ExecuteCommand("/scans/", "POST", json);
        }

   
        /// <summary>
        /// Bu fonksiyon taramayı siler.
        /// This function deletes the Scans
        /// </summary>
        /// <param name="id">Scan ID</param>
        /// <returns></returns>
        public string DeleteScan(string id)
        {
            return Session.ExecuteCommand("/scans/" + id, "DELETE", null);
        } 


        /// <summary>
        /// Bu fonksiyon  tarama durumunu getirir. 
        /// This function gets the Scan Status.
        /// </summary>
        /// <param name="id">Scan ID</param>
        /// <returns></returns>
        public string GetScanStatus(string id)
        {
            return Session.ExecuteCommand("/scans/" + id + "/status","GET",null);
        }

   
        /// <summary>
        /// Bu fonksiyon taramayı durdurur.
        /// This function stops the Scan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string StopScan(string id)
        {
            return Session.ExecuteCommand("/scans/" + id + "/stop", "GET", null);
        }

        /// <summary>
        /// Bu fonksiyon taramayı duraklatır.
        /// This function pauses the Scan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string PauseScan(string id)
        {
            return Session.ExecuteCommand("/scans/" + id + "/pause", "GET", null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
