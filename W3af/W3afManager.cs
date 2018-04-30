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

      /*
       * Bu fonksiyon tüm taramaların idlerini döndürür.
       * This function return all scans of ids.
       */
        public string GetScans()
        {
            return Session.GetExecuteCommand("/scans/");

        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        /*
         * Bu fonksiyon yeni bir tarama oluşturulmasını sağlar.         
         * 
         */
        //public string POSTScanCreate(string json)
        //{

        //    return Session.POSTExecuteCommand("/scans", json);
        //}
    }
}
