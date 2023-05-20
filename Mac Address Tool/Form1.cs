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

                if (prefix.Contains("."))
                {
                    prefix = prefix.Replace(".", "");
                }

                if (prefix.Contains(":"))
                {
                    prefix = prefix.Replace(":", "");
                }

                if (prefix.Contains("-"))
                {
                    prefix = prefix.Replace("-", "");
                }

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
                item = item.Replace(".", "");
                item = item.Replace(":", "");
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
            if (prefix.Contains("."))
            {
                prefix = prefix.Replace(".", "");
            }

            if (prefix.Contains(":"))
            {
                prefix = prefix.Replace(":", "");
            }

            if (prefix.Contains("-"))
            {
                prefix = prefix.Replace("-", "");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label4.Text = "[" + textBox3.Lines.Length + "]"; 
                
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox3.Text;
        }

        private void label6_Click(object sender, EventArgs e)
        {
    
            int a = 0;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string[] brands = File.ReadAllLines("brands.txt");

            string[] data = File.ReadAllLines("oui.txt");
            List<string> list = new List<string>();
            string content = "";
            foreach (var item in data)
            {
                content += item + Environment.NewLine;

                if (item == "")
                {
                    list.Add(content);
                    content = "";
                }



            }

            Dictionary<string, List<string>> rs = new Dictionary<string, List<string>>();
            foreach (var brand in brands)
            {
                List<string> rsChild = new List<string>();
                foreach (var item2 in list)
                {

                    if (item2.ToLower().Contains("\t" + brand.ToLower().Trim()) || item2.ToLower().Contains(" " + brand.ToLower().Trim() + " "))
                    {
                        string value = item2.Trim();
                        rsChild.Add(value.Substring(0,8));
                        int b = 0;
                    }
                }
                rs.Add(brand, rsChild);
                File.WriteAllText(@"brands/" + brand.ToLower().Trim() + @".txt", string.Join(Environment.NewLine, rsChild));
            }
            MessageBox.Show("updated");
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = comboBox3.SelectedItem.ToString();

            if (value == "random brand")
            {
                comboBox3.SelectedIndex = new Random().Next(2,comboBox3.Items.Count -1);
            }
            else if (value == "no select")
            {

            }
            else
            {
                string[] lines = File.ReadAllLines("brands/" + value.ToLower().Trim() + ".txt");
                textBox4.Text = lines[new Random().Next(0, lines.Length - 1)];
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
    }
}
