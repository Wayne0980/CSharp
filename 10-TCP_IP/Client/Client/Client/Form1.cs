using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        private bool isExit = false;
        private TcpClient client;
        private BinaryReader br;
        private BinaryWriter bw;

        public Form1()
        {
            InitializeComponent();

            try
            {
                client = new TcpClient(Dns.GetHostName(), 51888);

            }
            catch
            {
                return;
            }
            NetworkStream networkStream = client.GetStream();
            br = new BinaryReader(networkStream);
            bw = new BinaryWriter(networkStream);
            SendMessage("Login");
            Thread threadRec = new Thread(new ThreadStart(ReceiveData));
            threadRec.Start();
        }
        delegate void LabelDelegate(string message);
        private void UpdatingLabel(string msg)
        {
            if (this.label3.InvokeRequired)
                this.label3.Invoke(new LabelDelegate(UpdatingLabel), new object[] { msg });
            else
                this.label3.Text = msg;
        }
        private void ReceiveData()
        {
            string recStr = null;
            while (true)
            {
                try
                {
                    recStr = br.ReadString();
                }
                catch
                {
                    if (isExit == false)
                    {

                    }
                    break;
                }
                UpdatingLabel(recStr);
            }
        }
        private void SendMessage(string message)
        {
            try
            {
                bw.Write(message);
                bw.Flush();
            }
            catch
            {

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(textBox1.Text.ToString());
        }
    }
}
