using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TimeUpdater_Windows_.Data; 

namespace TimeUpdater_Windows_
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create("https://kiev.vremja.org/kiev_get_time.php");
            httpWebRequest.Method = "GET";
            SystemTime st = new SystemTime();
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkApp.SetValue("TimeUpdater", Assembly.GetExecutingAssembly().Location);
            using (WebResponse response = httpWebRequest.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    var data = sr.ReadToEnd().Trim('"');
                    string username = Regex.Replace(data, @"(@|&|'|\(|\)|\}|\{|\r|\n|\|<|>|#|)", "");
                    string result = Regex.Replace(username, @"[^\d]", "");

                    if (result.Length == 12)
                    {
                        st.Day = (ushort)int.Parse(result.Substring(0, 2));
                        st.Year = (ushort)int.Parse(result.Substring(2, 4));
                        st.Hour = (ushort)(int.Parse(result.Substring(6, 2))-3);
                        st.Minute = (ushort)int.Parse(result.Substring(8, 2));
                        st.Second = (ushort)int.Parse(result.Substring(10, 2));
                    }
                    var month = Enum.GetValues(typeof(Month));
                    for(ushort i = 0; i<month.Length;i++)
                    {
                        if (data.Contains(month.GetValue(i).ToString()))
                        {
                            st.Month = (ushort)(i + 1);
                            break;
                        }
                    }
                }
            } 

            SetSystemTime(ref st);
        }

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public static extern bool SetSystemTime(ref SystemTime st);
    }
}
