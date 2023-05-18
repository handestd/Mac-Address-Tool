using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mac_Address_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < textBox1.Lines.Length; i++)
            {
                string item = textBox1.Lines[i].ToUpper();
                if (item != "")
                {

                    PhysicalAddress MACAddress = null;

                    try
                    {
                        MACAddress = PhysicalAddress.Parse(item);
                    }
                    catch 
                    {
                        MessageBox.Show($"{item} is not valid format");
                    }

                    if (MACAddress != null) 
                    {
                        var vendorInfo = setting.addressMatcher.FindInfo(MACAddress);
                      
                        textBox2.AppendText(item + " " + $"\t{vendorInfo}" + Environment.NewLine);
                        
                    }


                
                }
            }
        }
        private static readonly Random Random = new Random();

    

        public static string GetRandomMac()
        {
            string[] macBytes = new[]
            {
            Random.Next(1, 256).ToString("X"),
            Random.Next(1, 256).ToString("X"),
            Random.Next(1, 256).ToString("X"),
            Random.Next(1, 256).ToString("X"),
            Random.Next(1, 256).ToString("X"),
            Random.Next(1, 256).ToString("X")
        };

            return string.Join("-", macBytes);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(GetRandomMac());
       
            MessageBox.Show(GetSignatureRandomMac("-", "B2"));

        }


        public static string GetSignatureRandomMac(string separator, string generic = "AA")
        {
            string[] macBytes = null;
            if (generic.Length == 1)
            {
                macBytes = new[]
                {
                    generic + Random.Next(1, 128).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X")
                };
            }
            else if (generic.Length == 2)
            {
                macBytes = new[]
                {
                    generic,
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X")
                };
            }
            else if (generic.Length == 3)
            {
                macBytes = new[]
                {
                    generic.Substring(0,2),
                    generic.Substring(2,1) + Random.Next(1, 128).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X")
                };
            }
            else if (generic.Length == 4)
            {
                macBytes = new[]
                {
                    generic.Substring(0,2),
                    generic.Substring(2,2),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X")
                };
            }
            else if (generic.Length == 5)
            {
                macBytes = new[]
                {
                    generic.Substring(0,2),
                    generic.Substring(2,2),
                    generic.Substring(4,1) + Random.Next(1, 128).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X")
                };
            }
            else if (generic.Length == 6)
            {
                macBytes = new[]
                {
                    generic.Substring(0,2),
                    generic.Substring(2,2),
                    generic.Substring(4,2),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X"),
                    Random.Next(1, 256).ToString("X")
                };
            }

            return string.Join(separator, macBytes);
        }
    }
}
