using Mac_Address_Tool.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MacAddressVendorLookup;
namespace Mac_Address_Tool
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            setting.vendorInfoProvider = new MacVendorBinaryReader();

            using (var resourceStream =ManufBinResource.GetStream().Result)
            {
                setting.vendorInfoProvider.Init(resourceStream).Wait();
            }
            setting.addressMatcher = new AddressMatcher(setting.vendorInfoProvider);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
