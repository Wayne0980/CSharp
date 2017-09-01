﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class User
    { 
        public TcpClient client {get; private set; }
        public BinaryReader br  {get; private set; }
        public BinaryWriter bw  {get; private set; }
        public string userName  { get; private set; }
        public User(TcpClient client)
        {
            this.client = client;
            NetworkStream networkStream = client.GetStream();
            br = new BinaryReader(networkStream);
            bw = new BinaryWriter(networkStream);
        }
        public void close()
        {
            br.Close();
            bw.Close();
            client.Close();
        }
    }
}