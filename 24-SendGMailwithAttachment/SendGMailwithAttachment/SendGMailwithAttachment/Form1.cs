using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace SendGMailwithAttachment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void SendGMailWithAttached(string MailList, string Subject, string Body, string picture_name)
        {
            MailMessage msg = new MailMessage();

            msg.To.Add(MailList);
            msg.From = new MailAddress("your mail address", "your name", System.Text.Encoding.UTF8);
            //郵件標題 
            msg.Subject = Subject;
            //郵件標題編碼  
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            //郵件內容
            msg.Body = Body;
            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.Priority = MailPriority.Normal;

            #region Other Host
            /*
             *  outlook.com smtp.live.com port:25
             *  yahoo smtp.mail.yahoo.com.tw port:465
            */
            #endregion
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
            //設定你的帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential("your mail address", "your mail password");
            //Gmial 的 smtp 使用 SSL
            MySmtp.EnableSsl = true;

            Attachment attachment = null;


            string pathfilename = picture_name;
            string extName = Path.GetExtension(pathfilename).ToLower();

            attachment = new Attachment(pathfilename, MediaTypeNames.Application.Octet);

            ContentDisposition cd = attachment.ContentDisposition;
            cd.CreationDate = File.GetCreationTime(pathfilename);
            cd.ModificationDate = File.GetLastWriteTime(pathfilename);
            cd.ReadDate = File.GetLastAccessTime(pathfilename);
            msg.Attachments.Add(attachment);
            MySmtp.Send(msg);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SendGMailWithAttached("mail address to who", "Test", "Mail Test", "download.jpg");
        }
    }
}
