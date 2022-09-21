using ESD_DTool.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CashierStationDevice
{
    /// <MetaDataID>{6a723107-e61c-4464-9a63-0fbc71c78020}</MetaDataID>
    public class RBSDocSigner : DocumentSignDevice
    {
        ESDPROT RBSESD = new ESDPROT();

        public string UnlockKey { get; private set; }

        bool _IsOnline = true;
        public override bool IsOnline
        {
            get
            {
                lock (ConnectionLock)
                {
                    if (_IsOnline)
                        return _IsOnline;
                    else
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            /* Your code here */
                            CheckDeviceStatus();
                        }));
                        return _IsOnline;

                    }
                }
            }

        }

        //public string InvalidChars { get; private set; }

        System.Timers.Timer SignDeviceStateTimer = new System.Timers.Timer();

        public RBSDocSigner()
        {
            SignDeviceStateTimer.Elapsed += SignDeviceStateTimer_Elapsed;
            SignDeviceStateTimer.Interval = 5000;

        }
        internal static byte[] Read(Stream stream)
        {

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }
        Devices CurDEV = new Devices();

        public override event EventHandler<EventArgs> DeviceStatusChanged;
        object ConnectionLock = new object();
        public void Start(string ethernetIP, string AESKey, string AADESendDataUrl)
        {

            //byte[] data = Read(typeof(CashierStationDevice.SamtecNext).Assembly.GetManifestResourceStream("CashierStationDevice.Resources.ValidChars.txt"));
            //InvalidChars = System.Text.Encoding.Unicode.GetString(data, 0, data.Length);

            this.AADESendDataUrl = AADESendDataUrl;
            this.AESKey = AESKey;


            string unlockKey = RBSESD.ReadUnlockKey();
            CurDEV.GGPSKey = AESKey;
            CurDEV.EthernetIP = ethernetIP;

            CurDEV.IsEthernet = true;
            CurDEV.ProxyIP = "0.0.0.0";
            CurDEV.SerialNO = "***********";
            CurDEV.ComNo = 1;// Convert.ToByte(SerialPort.Text.Substring(3, 1));
            CurDEV.ActivationCode = unlockKey;
            //SignDeviceStateTimer.Enabled = true;
            //SignDeviceStateTimer.Start();
            //lock (ConnectionLock)
            //{

            //    Application.Current.Dispatcher.Invoke(new Action(() =>
            //    {
            //        /* Your code here */
            //        CheckDeviceStatus();
            //    }));

            //}

        }
        public void Stop()
        {
            SignDeviceStateTimer.Stop();
        }
        private void SignDeviceStateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SignDeviceStateTimer.Stop();
            try
            {
                lock (ConnectionLock)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        /* Your code here */
                        CheckDeviceStatus();
                    }));

                }

            }
            finally
            {
                SignDeviceStateTimer.Start();
            }
        }

        internal void SetOutputFolder(string documentSignerOutputFolder)
        {
            RBSESD.SetOutputFolder(documentSignerOutputFolder);
        }

        private void CheckDeviceStatus()
        {

            try
            {

                string reply = null;
                string ReturnedErrorStr = "";

                _IsOnline = true;
                int iret = RBSESD.SelectESDDevice(CurDEV);
                if (iret > 0)
                {
                    ESDPROT.CVB_FSL_ReleaseDevice();
                    _IsOnline = false;
                }

            }
            catch (Exception error)
            {

                _IsOnline = false;
            }
        }

        string errors;

        object rbsDiviceLock = new object();
        private string AADESendDataUrl = "";
        private string AESKey = "";

        public override SignatureData SignDocument(string document, EpsilonLineData epsilonLineData)
        {
            lock (ConnectionLock)
            {

                try
                {
                    //foreach (char invlidChar in InvalidChars)
                    //{
                    //    document=document.Replace(invlidChar.ToString(), "");

                    //}

                    using (StringReader reader = new StringReader(document))
                    {
                        document = null;
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = Regex.Replace(line, @"\p{C}+", string.Empty);
                            if (document != null)
                                document += Environment.NewLine;
                            document += line;
                        }
                    }



                    //document = Regex.Replace(document, @"\p{C}+", string.Empty);

                    SignatureData signatureData = new SignatureData();
                    errors = null;
                    if (!System.IO.Directory.Exists(ApplicationSettings.AppDataPath + "\\RBS"))
                        System.IO.Directory.CreateDirectory(ApplicationSettings.AppDataPath + "\\RBS");
                    string documentFile = CashierStationDevice.ApplicationSettings.AppDataPath + "\\RBS\\" + Guid.NewGuid().ToString("N") + ".txt";
                    //documentFile = @"F:\ESDtool.120522.RBS\ESD_DTool2\ESD_DTool\ESD_DTool\bin\Debug\TestDoc.txt";
                    //document=PrepareEpsilonLine(epsilonLineData) + Environment.NewLine + Environment.NewLine + document;


                    System.IO.File.WriteAllBytes(documentFile, Encoding.GetEncoding("windows-1253").GetBytes(document));


                    string signature = null;

                    //string vstream = System.IO.File.ReadLines(@"F:\ESDtool.120522.RBS\ESD_DTool2\ESD_DTool\ESD_DTool\bin\Debug\TestDoc.txt").First();
                    //string vstream2 = PrepareEpsilonLine(epsilonLineData);
                    //int ret = RBSESD.SignData(CurDEV, documentFile, vstream, ref signature);

                    if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.DocumentSignerOutputFolder))
                        RBSESD.SetOutputFolder(ApplicationSettings.Current.DocumentSignerOutputFolder);

                    int ret = RBSESD.SignData(CurDEV, documentFile, PrepareEpsilonLine(epsilonLineData), ref signature);

                    //int ret = RBSESD.SignData(CurDEV, documentFile, PrepareEpsilonLine(epsilonLineData), ref signature);
                    errors = RBSESD.RetErr;
                    if (ret == 0)
                    {
                        this.DeviceStatusChanged?.Invoke(this, EventArgs.Empty);

                        signatureData.Signuture = signature.Split('-')[0];
                        signatureData.QRCode = signature.Split('|')[1];
                        System.IO.File.Delete(documentFile);



                        return signatureData;
                    }
                    else
                        throw new Exception(errors);
                }
                catch (Exception error)
                {

                    throw;
                }

            }
        }
        string PrepareEpsilonLine(EpsilonLineData epsilonLineData)
        {
            string epsilon_line = string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0};{1};;{2};{3};{4};", epsilonLineData.afm_publisher, epsilonLineData.afm_recipient, epsilonLineData.transactionTypeID, epsilonLineData.series, epsilonLineData.taxDocNumber);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};{1:N2};{2:N2};{3:N2};{4:N2};", epsilonLineData.net_a, epsilonLineData.net_b, epsilonLineData.net_c, epsilonLineData.net_d, epsilonLineData.net_e);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};{1:N2};{2:N2};{3:N2};", epsilonLineData.vat_a, epsilonLineData.vat_b, epsilonLineData.vat_c, epsilonLineData.vat_d);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};0;;;", epsilonLineData.total_to_pay_poso);
            return epsilon_line;
        }

        public override List<string> CheckStatusForError()
        {
            if (!string.IsNullOrWhiteSpace(errors))
                return new List<string>() { errors };
            return new List<string>();
        }

        public bool IssueZreport(out string message)
        {
            int ret = RBSESD.IssueZreport(CurDEV);




            if (ret > 0)
            {
                message = RBSESD.RetErr;
                return false;
            }
            else
            {
                if (AESKey == null)
                    AESKey = "";

                string reply = "";
                string eeout = AADESendDataUrl.Trim() + "|" + 80 + "|" + AESKey.Trim();
                ret = RBSESD.Upload_S_DATA(CurDEV, eeout, ref reply);
                if (ret > 0)
                {
                    message =String.Format("Error({0}) {1}\r\n", ret, reply);
                    //message = RBSESD.RetErr;

                    return false;
                }


                message = "Procedure completed successfully\r\n";
                return true;
            }

        }

    }
}
