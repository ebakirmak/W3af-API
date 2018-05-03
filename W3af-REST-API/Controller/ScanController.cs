using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W3af;
using W3af_REST_API.Model;
using W3af_REST_API.View;

namespace W3af_REST_API.Controller
{
    public class ScanController
    {

        public ScanController()
        {

        }

        /*
         * Bu fonksiyon Taramaları döndürür.
         * This function is returned the scans.
         */
        public ScanCreateResponse ListScan(W3afManager manager)
        {
            try
            {
                string json = manager.GetScans();
                var scans = JsonConvert.DeserializeObject<ScanCreateResponse>(json);

                return scans;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        /*
         * Bu fonksiyon yeni bir Tarama oluşturur.
         * This function is created a new Scan.
         */
         public string CreateScan(W3afManager manager,string json)
        {

            
            return manager.CreateScan(json);
        }
    }
}
