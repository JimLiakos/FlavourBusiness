using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.HashFunction.CRC;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace ServiceContextManagerApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{dd711854-5598-4817-8d0c-1d729c403638}</MetaDataID>
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();


        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;


        protected override void OnStartup(StartupEventArgs e)
        {
            FlavourBusinessManager.FireBase.Init();

            MemberValuesJsonTest();
            SerializeTaskScheduler.RunAsync();

            FlavourBusinessApps.ServiceContextManagerApp.WPF.ServiceContextManagerApp.Startup();

            DeviceSelectorWindow mainWindow = new DeviceSelectorWindow();
            mainWindow.Show();

            //ServiceContextManagerApp.WPF.MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();

            //SendVerificationEmail("jim.liakos@gmail.com");
            //SendEmail();

            base.OnStartup(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme",new FlavourBusinessApps.FirebaseAuth());
            base.OnActivated(e);
        }

        public class VerifyEmailConfig
        {
            public string Server { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Port { get; set; }
            public string SSL { get; set; }
            public string Displayname { get; set; }
            public string Subject { get; set; }
            public string Textbody { get; set; }
            public string Email { get; set; }
            public string Signature { get; set; }
        }

        public void SendVerificationEmail(string emailAddress)
        {

            string code = Math.Abs(BitConverter.ToInt16(CRCFactory.Instance.Create(CRCConfig.CRC32).ComputeHash(System.Text.Encoding.UTF8.GetBytes(emailAddress.ToLower())).Hash, 0)).ToString();
            while (code.Length < 6)
                code = code.Insert(1, "0");
            var getJsonTask = new HttpClient().GetStringAsync("http://dontwaitwaiter.com/config/EmailVerifyConfig.json");
            getJsonTask.Wait();
            var emailVerifyConfig = getJsonTask.Result;
            VerifyEmailConfig verifyEmailConfig = OOAdvantech.Json.JsonConvert.DeserializeObject<VerifyEmailConfig>(emailVerifyConfig);

            //verifyEmailConfig.Signature = "<p><strong><span style=\"font-family: verdana,geneva; font-size: 10pt;\">SALES CONTACT</span></strong><br /><span style=\"font-family: verdana,geneva; font-size: 10pt;\"><a href=\"mailto:info@arion-software.co.uk\">info@arion-software.co.uk</a> </span><br /><span style=\"font-family: verdana,geneva; font-size: 10pt;\"><a href=\"http://www.arion-software.co.uk\">www.arion-software.co.uk</a> </span><br /><span style=\"font-family: verdana,geneva; font-size: 10pt;\">Tel No.: +44 (0)207 193 7039</span></p>";
            //verifyEmailConfig.Textbody = "<p><span style=\"font-family: verdana,geneva; font-size: 12pt;\">The verification code is:</span><br /><span style=\"color: #000000; background-color: #00ffff;\"><strong><span style=\"font-family: verdana,geneva; font-size: 14pt;\">{0}</span></strong></span><br /><span style=\"font-family: verdana,geneva; font-size: 12pt;\">Use this code and follow the instructions to complete registration</span><br /><span style=\"font-family: verdana,geneva; font-size: 10pt;\"></span></p> <p>&nbsp;</p> <p><span style=\"font-family: verdana,geneva; font-size: 10pt;\">This is an automated email, please don't reply. If you didn't initiate this, let us know immediately</span></p>\" signature=\"<p><strong><span style=\"font-family: verdana,geneva; font-size: 10pt;\">SALES CONTACT</span></strong><br /><span style=\"font-family: verdana,geneva; font-size: 10pt;\"><a href=\"mailto:info@arion-software.co.uk\">info@arion-software.co.uk</a> </span><br /><span style=\"font-family: verdana,geneva; font-size: 10pt;\"><a href=\"http://www.arion-software.co.uk\">www.arion-software.co.uk</a> </span><br /><span style=\"font-family: verdana,geneva; font-size: 10pt;\">Tel No.: +44 (0)207 193 7039</span></p>";
            //try
            //{
            //    emailVerifyConfig = OOAdvantech.Json.JsonConvert.SerializeObject(verifyEmailConfig);
            //    verifyEmailConfig = OOAdvantech.Json.JsonConvert.DeserializeObject<VerifyEmailConfig>(emailVerifyConfig);

            //}
            //catch (Exception error)
            //{
            //}
            try
            {
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress(verifyEmailConfig.Email, verifyEmailConfig.Displayname);
                m.To.Add(new MailAddress(emailAddress));
                //similarly BCC

                m.Subject = verifyEmailConfig.Subject;
                m.Body = string.Format(verifyEmailConfig.Textbody, code);
                m.Body += verifyEmailConfig.Signature;

                m.IsBodyHtml = true;


                sc.Host = verifyEmailConfig.Server;// "smtp.gmail.com";
                int port = 0;
                int.TryParse(verifyEmailConfig.Port, out port);
                sc.Port = port;

                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new System.Net.NetworkCredential(verifyEmailConfig.Username, verifyEmailConfig.Password);
                bool ssl_bool;
                bool.TryParse(verifyEmailConfig.SSL, out ssl_bool);
                sc.EnableSsl = ssl_bool; // runtime encrypt the SMTP communications using SSL
                sc.Send(m);
            }
            catch (Exception error)
            {


            }

        }

        static string smtpAddress = "smtp.gmail.com";
        static int portNumber = 587;
        static bool enableSSL = true;
        static string emailFromAddress = "dontwaitwaiter@gmail.com"; //Sender Email Address  
        static string password = "zsccwkkflaytpmrk"; //Sender Password  
        static string emailToAddress = "jim.liakos@hotmail.com"; //Receiver Email Address  
        static string subject = "Hello";
        static string body = "Hello, This is Email sending test using gmail.";

        public static void SendEmail()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  


                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                //using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                //{
                //    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                //    smtp.EnableSsl = enableSSL;
                //    smtp.Send(mail);
                //}
            }
        }

        void MemberValuesJsonTest()

        {
            ObjRef objRef = new ObjRef();
            objRef.ChannelData = new ChannelData("local-device");
            objRef.MembersValues["Name"] = "Liakos";
            objRef.MembersValues["Childs"] = new List<string> { "Gerorge", "Anna" };

            var json = OOAdvantech.Json.JsonConvert.SerializeObject(objRef);

            var objrefA = OOAdvantech.Json.JsonConvert.DeserializeObject<ObjRef>(json);

        }
    }
}
