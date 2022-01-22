using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ps5LinkGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddList();
            AddCombo();
            flatComboBox1.SelectedIndex = 0;
        }

        List<Ps5List> list = new List<Ps5List>();
        private void AddList()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            WebClient wc = new WebClient();
            string[] data = wc.DownloadString("https://raw.githubusercontent.com/halolck/Ps5UpdatorGenerator/main/Data").Split('\n');

            foreach(string item in data)
            {
                if(item == "")
                    continue;

                string[] datawake = item.Split('|');
                list.Add(new Ps5List{ Version = datawake[0], ObfuscatedString = datawake[1], BuildDate = datawake[2], Type_SHA256 = datawake[3] });
            }
        }

        private void AddCombo()
        {
            foreach(var item in list)
            {
                flatComboBox1.Items.Add(item.Version);
            }
        }

        public struct Ps5List
        {
            public string ObfuscatedString;
            public string BuildDate;
            public string Type_SHA256;
            public string Version;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(var a in list)
            {
                if(a.Version ==flatComboBox1.Text)
                {
                    textBox1.Text = $"http://pc.ps5.update.playstation.net/update/ps5/official/{a.ObfuscatedString}/image/{a.BuildDate}/{a.Type_SHA256}/PS5UPDATE.PUP";
                }
            }
        }
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(
            IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //マウスのキャプチャを解除
                ReleaseCapture();
                //タイトルバーでマウスの左ボタンが押されたことにする
                SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void flatComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                return;

            System.Diagnostics.Process.Start(textBox1.Text);
        }
    }
}
