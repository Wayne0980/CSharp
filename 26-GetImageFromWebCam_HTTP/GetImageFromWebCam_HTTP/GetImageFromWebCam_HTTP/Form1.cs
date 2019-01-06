using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace GetImageFromWebCam_HTTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
              HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("your webcam address");
              using (WebResponse wr = req.GetResponse())
              {
                  using (Stream stream = wr.GetResponseStream())
                  {
                      Image img = Image.FromStream(stream);
                      pictureBox1.Image = img;
                  }
              }
        }
    }
}
