using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        string format = "";
        int amount = 1;
        string mycase = "up";
        string prefix = "";
        public Form1()
        {
            InitializeComponent();
            if (File.Exists("amount.txt"))
            {
                try
                {
                    amount = int.Parse(File.ReadAllText("amount.txt").Trim());
                    textBox5.Text = amount.ToString();
                }
                catch { }
            }
            else
            {
                textBox5.Text = "1";
            }
            if (File.Exists("case.txt"))
            {
                string value = File.ReadAllText("case.txt");
                if (value == "Uppercase")
                {
                    mycase = "up";
                    comboBox2.SelectedIndex = 0;
                }
                else if (value == "Lowercase")
                {
                    mycase = "lower";
                    comboBox2.SelectedIndex = 1;
                }
            }
            else
            {
                comboBox2.SelectedIndex = 0;
            }
            if (File.Exists("prefix"))
            {
                prefix = File.ReadAllText("prefix.txt");
                textBox4.Text = prefix;
            }
            if (File.Exists("format.txt"))
            {
                string value = File.ReadAllText("format.txt");
                if (value.Contains("."))
                {
                    format = ".".ToString();
                    comboBox1.SelectedIndex = 1;
                }
                else if (value.Contains(":"))
                {
                    format = ":".ToString();
                    comboBox1.SelectedIndex = 2;
                }
                else if (value.Contains("-"))
                {
                    format = "-".ToString();
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    format = "";
                    comboBox1.SelectedIndex = 3;
                }
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }

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


        private static string MACToString(byte[] Bytes, string format)
        {

            return BitConverter.ToString(Bytes).Replace("-", format).ToUpper();
        }
        public static string CreateMAC(string format)
        {
            Random RM = new Random();

            byte[] Bytes = new byte[6];
            RM.NextBytes(Bytes);

            Bytes[0] = (byte)(Bytes[0] | 0x02);
            Bytes[0] = (byte)(Bytes[0] & 0xfe);

            return MACToString(Bytes, format);
        }

       
        
        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(GetRandomMac());     
            for (int i = 0; i < amount; i++)
            {
                string item = GetSignatureRandomMac(format, prefix);

                if (mycase == "up")
                {
                    item = item.ToUpper();
                }
                else
                {
                    item = item.ToLower();
                }

                textBox3.AppendText(item+Environment.NewLine);
            }       
        }


        public static string GetSignatureRandomMac(string separator, string generic = "AA")
        {
            string[] macBytes = null;

            if (generic.Length == 0)
            {
                return CreateMAC(separator);
            }

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            File.WriteAllText("format.txt", comboBox1.SelectedItem.ToString());

            switch (comboBox1.SelectedIndex) 
            {
                case 0:
                    format = "-".ToString(); break;                
                case 1:
                    format = ".".ToString(); break;
                case 2: 
                    format = ":".ToString(); break;
                case 3:
                    format = "".ToString(); break;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("amount.txt", textBox5.Text);
            try
            {
                amount = int.Parse(textBox5.Text);
            }
            catch { }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            File.WriteAllText("case.txt", comboBox2.SelectedItem.ToString());

            if (comboBox2.SelectedIndex == 0)
            {
                mycase = "up";
            }
            else
            {
                mycase = "lower";
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("prefix.txt", textBox4.Text);
            prefix = textBox4.Text.ToUpper();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label4.Text = "[" + textBox3.Lines.Length + "]"; 
                
        }
    }
}
