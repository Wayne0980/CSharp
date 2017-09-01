using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server
{
    public partial class Form1 : Form
    {
        private List<User> userList = new List<User>();
        IPAddress localAddress;
        private const int port = 51888;
        private TcpListener myListener;
        bool isNormalExit = false;

        public Form1()
        {
            InitializeComponent();

            IPAddress[] addrIP = Dns.GetHostAddresses(Dns.GetHostName());
            localAddress = addrIP[0];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            myListener = new TcpListener(localAddress, port);
            myListener.Start();
            label3.Text = "Wait for client connecting....";
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
        }
        private void ListenClientConnect()
        {
            TcpClient newClient = null;
            while (true)
            {

                try
                {
                    newClient = myListener.AcceptTcpClient();
                }
                catch {

                    break;
                }
                User user = new User(newClient);
                Thread threadReceive = new Thread(ReceiveData);
                threadReceive.Start(user);
                userList.Add(user);
                //label3.Text = "Add new client";
            }
        }
        private void ReceiveData(object userState)
        {
            User user = (User)userState;
            TcpClient client = user.client;
            while (isNormalExit == false)
            {
                string receiveString = null;
                try
                {
                    receiveString = user.br.ReadString();
                }
                catch {
                    if (isNormalExit == false)
                    {
                        //label3.Text = "losing client";
                        RemoveUser(user);
                        break;
                    }
                }
                //label3.Text = receiveString;
                UpdatingLabel(receiveString);
                SendToClient(user, receiveString);
            }
        }

        delegate void LabelDelegate(string message);
        private void UpdatingLabel(string msg)
        {
            if (this.label3.InvokeRequired)
                this.label3.Invoke(new LabelDelegate(UpdatingLabel), new object[] { msg });
            else
                this.label3.Text = msg;
        }

        private void SendToClient(User user, string message)
        {
            try
            {
                user.bw.Write(message);
                user.bw.Flush();

            }
            catch {
                label3.Text = "send message error";
            }
        }
        private void RemoveUser(User user)
        {
            userList.Remove(user);
            user.close();
        }
    }   
}
