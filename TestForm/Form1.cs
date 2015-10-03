    using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Windows.Forms;
using RouterTestFramework;

//By Robin Miklinski

namespace TestForm
{
    public partial class Form1 : Form
    {
        public string _ip = "";
        Microsoft.Win32.RegistryKey ipRegKey;
        public Form1()
        {
            InitializeComponent();
            getMyIp();
            ipBox.Text = setInputIP();
            logAction("Previous IP configuration: " + getLastSetIPfromRegistry());
            logAction("Client IP: " + getMyIp());
        }
        public string getMyIp()
        {
            string tempip = "";
            IPAddress[] ipv4Addresses = Array.FindAll(
            Dns.GetHostEntry(string.Empty).AddressList,
            a => a.AddressFamily == AddressFamily.InterNetwork);
            foreach (IPAddress ip in ipv4Addresses)
            {
                tempip = ip.ToString();
            }
            return tempip;
        }

        public string getLastSetIPfromRegistry() 
        {
            string ip = Registry.GetValue(@"HKEY_CURRENT_USER\LastIPSetting\", "IP", "NULL").ToString();
            return ip;
        }           

        private string setInputIP()
        {
            string myip = getMyIp();
            string[] breaks = myip.Split(new[] { '.' });
            string last = breaks[3];

            return last;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                logAction("Loading browser...");
                Pages.PortPage.GoTo();
                logAction(Browser.getUrl());
                Pages.PortPage.selectRadioBtn();
                Pages.PortPage.clickEdit();
                logAction(Browser.getUrl());
                Pages.ActionPage.editIPField(_ip);
                Pages.ActionPage.getNewIPFromWebpage();
                logAction("Current IP configuration: " + Pages._currentIP);
                logAction(Browser.getUrl());
                logAction("New IP configuration: " + Pages._finalIP);
                logAction("Testing new config...");

                ipHelper iph = new ipHelper();
                iph.lastIP = Pages._finalIP;

                if (ipHasBeenUpdated())
                {
                    logAction("Router IP assignment successful!");
                    logAction("Closing browser...");
                    logAction("Storing registry value...");
                    ipRegKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("LastIPSetting");
                    ipRegKey.SetValue("IP", Pages._finalIP);
                    ipRegKey.Close();
                    logAction("...done");
                    Browser.Close();
                    checkSlskState();
                } else 
                    {
                    logAction("IP update failed");
                    }
            }
            catch (Exception ex)
            {
                logAction(ex.Message);
            }
        }

        private void checkSlskState()
        {
            Process[] slskProcess = Process.GetProcessesByName("slsk");
            if (slskProcess.Length == 0)
            {
                logAction("Launching Soulseek...");
                Process.Start(@"C:\Program Files\SoulseekNS\slsk.exe");
            }
            else
            {
                logAction("Closing Soulseek...");
                slskProcess[0].Kill();
                if (slskProcess[0].WaitForExit(1000))
                {
                   logAction("Launching Soulseek...");
                   Process.Start(@"C:\Program Files\SoulseekNS\slsk.exe");   
                }           
            }
        }
        private void logAction(string action)
        {
            string time = DateTime.Now.ToString("g");
            listBox1.Items.Add(time + " - " + action);
        }

        private void ipBox_TextChanged(object sender, EventArgs e)
        {
            _ip = ipBox.Text;
            Pages._currentIP = "192.168.0." + ipBox.Text;
        }                   
   
        private bool ipHasBeenUpdated()
        {
            if (Pages._finalIP == getMyIp())
            return true;
            else
            {
                return false;
            }
        }
    }
}
