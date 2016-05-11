using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
 

namespace wifiWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionChanger connections = new ConnectionChanger();

            while (true)
            {
                if (!connections.IsInternetAlive())
                {
                    connections.DisableWifi();

                    Thread.Sleep(2000);

                    connections.EnableWifi();
                }

                Thread.Sleep(10000);
            }
        }
    }
}
