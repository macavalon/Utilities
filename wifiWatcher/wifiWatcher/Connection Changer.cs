using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Management;


namespace wifiWatcher
{
    public class ConnectionChanger
    {
        //ManagementObject cable = null;
        ManagementObject wifi = null;
        //NetworkInterface cableInterface = null;
        NetworkInterface wifiInterface = null;
        string cableConnectionName = "";
        string wifiConnectionName = "";

        public ConnectionChanger()
        {
            getWifiConnection();


        }

        private ManagementObjectCollection getNetworkAdapterCollection()
        {
            SelectQuery wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(wmiQuery);
            return searchProcedure.Get();
        }

        public bool IsInternetAlive()
        {
            bool alive = false;
            Ping pingSender = new Ping ();
            try
            {
                PingReply reply = pingSender.Send("www.google.com");

                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine("Connected to internet");
                    alive = true;
                }
                else
                {
                    Console.WriteLine("Not connected to internet");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not connected to internet");
            }

            return alive;
        }

        public void EnableWifi()
        {
            try
            {

                wifi.InvokeMethod("Enable", null);
                Console.WriteLine("Enabled WiFi");

                EnableAllConnections();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot enable WiFi");
            }
            

        }

        public void DisableWifi()
        {

            try
            {

                wifi.InvokeMethod("Disable", null);
                Console.WriteLine("Disabled WiFi");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot Disable WiFi");
            }
            
        }

        private void EnableAllConnections()
        {
            //turns on all the connections
            foreach (ManagementObject item in this.getNetworkAdapterCollection())
            {
                //NetworkAdapter adapter = new NetworkAdapter(item);
                //adapter.Enable();
                item.InvokeMethod("Enable", null);
            }
        }

        private void getWifiConnection()
        {
            EnableAllConnections();
            //gets names of the connections
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType.ToString().Contains("Wireless"))
                {
                    wifiConnectionName = ni.Name;
                }
            }

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType.ToString().Contains("Wireless"))
                {
                    wifiInterface = ni;
                }
            }

            getAdapters();
        }

       

        private void getAdapters()
        {
            SelectQuery wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(wmiQuery);
            foreach (ManagementObject item in searchProcedure.Get())
            {
                if (item["NetConnectionID"].ToString().Contains(wifiConnectionName))
                {
                    wifi = item;
                }
            }
        }
    }
}
