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
        public Scan ListScan(W3afManager manager)
        {

            var scans = JsonConvert.DeserializeObject<Scan>(manager.GetScans());

            return scans;
        }

    }
}
