using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W3af;
using W3af_REST_API.Controller;

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
            scanController = new ScanController();
            Console.WriteLine(scanController.ListScan(manager));
        }

    }
}
