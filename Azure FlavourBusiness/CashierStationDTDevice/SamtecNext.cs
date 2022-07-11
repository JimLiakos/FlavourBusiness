using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationDevice
{
    /// <MetaDataID>{4b187271-a093-454c-9fb4-b7f320c22047}</MetaDataID>
    public class SamtecNext : DocumentSignDevice
    {
        public override event EventHandler<EventArgs> DeviceStatusChanged;
        public SamtecNext()
        {

            SignDeviceStateTimer.Elapsed += SignDeviceStateTimer_Elapsed;
            SignDeviceStateTimer.Interval = 5000;
            SignDeviceStateTimer.Enabled = true;
            SignDeviceStateTimer.Start();

        }
       


        object ConnectionLock = new object();






        private void SignDeviceStateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (ConnectionLock)
            {
                using (System.Net.Sockets.Socket DeviceCommunicationSocket = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
                {

                    if (!DeviceCommunicationSocket.Connected)
                    {
                        try
                        {
                            DeviceCommunicationSocket.Connect("127.0.0.1", 6001);
                            //Status = new DeviceStatus();
                        }
                        catch (Exception error)
                        {

                            Status = new DeviceStatus() { SamtecDriverConnectionError = true };
                        }

                    }

                    if (DeviceCommunicationSocket.Connected)
                    {

                        byte[] buffer = new byte["<<ΑΚΣΣ>>".Length + 2];
                        int i = 1;
                        foreach (var _byte in Encoding.GetEncoding(CodePage).GetBytes("<<ΑΚΣΣ>>"))
                            buffer[i++] = _byte;
                        buffer[0] = 2;
                        buffer[buffer.Length - 1] = 3;

                        DeviceCommunicationSocket.Send(buffer);
                        //buffer = new byte[1024];
                        //int rededByte = DeviceCommunicationSocket.Receive(buffer);

                        //string result = Encoding.GetEncoding(1253).GetString(buffer, 0, rededByte);

                        string result = ReadAnswer(DeviceCommunicationSocket);

                        if (result.IndexOf("[[Error017") != -1)
                        {
                            Status = new DeviceStatus() { FiscalDeviceCommunicationError = true };
                        }


                        if (result.IndexOf("[[Error018") != -1)
                        {
                            result = result.Substring(result.IndexOf("[[Error018 ") + "[[Error018 ".Length, result.IndexOf("]]") - (result.IndexOf("[[Error018 ") + "[[Error018 ".Length));
                            string[] error18Result = result.Split(',');
                            string[] statusResult = CopyErrorResultToStatusResult(error18Result);
                            Status = new DeviceStatus(statusResult);
                        }
                        else if (result.IndexOf("[[ΑΚΣΣ ") != -1)
                        {

                            result = result.Substring(result.IndexOf("[[ΑΚΣΣ ") + "[[ΑΚΣΣ ".Length, result.IndexOf("]]") - (result.IndexOf("[[ΑΚΣΣ ") + "[[ΑΚΣΣ ".Length));
                            string[] statusResult = result.Split(',');

                            Status = new DeviceStatus(statusResult);
                        }



                        //if (/*Status.FiscalDayOpen == "1" ||*/ Status.SignsLimitExceeded == "1")
                        //{
                        //    result = SendCloseFisicalOpenDay();
                        //    //if (result.IndexOf("[[ΔΗΦΑΣΣ") != -1)
                        //    //    Status.OutofPaper = "0";

                        //    //else if (result.IndexOf("[[Error018") != -1)
                        //    //{
                        //    //    result = result.Substring(result.IndexOf("[[Error018 ") + "[[Error018 ".Length, result.IndexOf("]]") - (result.IndexOf("[[Error018 ") + "[[Error018 ".Length));
                        //    //    string[] error18Result = result.Split(',');
                        //    //    string[] statusResult = CopyErrorResultToStatusResult(error18Result);
                        //    //    Status = new DeviceStatus(statusResult);
                        //    //}
                        //}


                        CheckStatusForError();
                    }
                    else
                        Status = new DeviceStatus() { SamtecDriverConnectionError = true };
                }

            }
        }

        public override List<string> CheckStatusForError()
        {
            List<string> statusMessages = new List<string>();

            if (Status.SamtecDriverConnectionError)
            {
                statusMessages.Add(Properties.Resources.FiscalDeviceCommunicationErrorMessage);
                return statusMessages;
            }

            if (Status.FiscalDeviceCommunicationError)
                statusMessages.Add(Properties.Resources.FiscalDeviceCommunicationErrorMessage);
            if (Status.OutofPaper == "1")
                statusMessages.Add(Properties.Resources.OutofPaperMessage);

            //if (Status.FiscalDayOpen == "1")
            //    statusMessages.Add(Properties.Resources.FisicalDayOpenMessage);

            if (Status.InUserInterfaceMode == "1")
                statusMessages.Add(Properties.Resources.UserMenuModeMessage);

            if (Status.CriticalError == "1")
                statusMessages.Add(Properties.Resources.CriticalErrorMessage);

            if (Status.CMOSReset == "1")
                statusMessages.Add(Properties.Resources.CriticalErrorMessage);

            if (Status.FiscalMemoryOverflow == "1")
                statusMessages.Add(Properties.Resources.FiscalMemoryOverflowMessage);

            //if (Status.FiscalMemoryAlmostFull == "1")
            //    statusMessages.Add(Properties.Resources.FiscalMemoryAlmostFullMessage);

            //if(Status.ResponseCode=="255")
            if (Status.ErrorDescription != null)
            {
                statusMessages.Clear();
                statusMessages.Add(Status.ErrorDescription);
            }



            return statusMessages;
        }

        int SignDocumentReΤries = 5;
        int IntervalBetweenSignReΤries = 300;
        int CodePage = 1253;

        public string[] CopyErrorResultToStatusResult(string[] error18Result)
        {
            string[] statusResult = new string[16];
            int k = 0;
            for (int i = 2; i < error18Result.Length; i++)
            {
                if (k > 15)
                    statusResult[15] += "," + error18Result[i];
                else
                    statusResult[k] = error18Result[i];

                k++;
            }

            return statusResult;
        }

        /// <exclude>Excluded</exclude>
        DeviceStatus _Status;

        public DeviceStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    DeviceStatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public override SignatureData SignDocument(string document, EpsilonLineData epsilonLineData)
        {
            document +=Environment.NewLine+  PrepareEpsilonLine(epsilonLineData);
            lock (ConnectionLock)
            {
                using (Socket DeviceCommunicationSocket = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
                {


                    if (!DeviceCommunicationSocket.Connected)
                        DeviceCommunicationSocket.Connect("127.0.0.1", 6001);

                    document = "<<ΔΦΣΣ>>" + document;
                    bool busy = false;
                    string signDocumentResult = "";
                    string[] signatureParts = new string[0];
                    var signDocumentReΤries = SignDocumentReΤries;
                    do
                    {



                        byte[] buffer = new byte[document.Length + 2];
                        int i = 1;
                        foreach (var _byte in Encoding.GetEncoding(CodePage).GetBytes(document))
                            buffer[i++] = _byte;
                        buffer[0] = 2;
                        buffer[buffer.Length - 1] = 3;

                        //int readedByte = 0;

                        string result = "";

                        DeviceCommunicationSocket.Send(buffer);

                        //byte[] resBuffer = new byte[1024];

                        result = ReadAnswer(DeviceCommunicationSocket);


                        //throw new Exception(result);
                        try
                        {
                            if (result.IndexOf("[[Error018") != -1)
                            {
                                result = result.Substring(result.IndexOf("[[Error018 ") + "[[Error018 ".Length, result.IndexOf("]]") - (result.IndexOf("[[Error018 ") + "[[Error018 ".Length));

                                string[] error18Result = result.Split(',');
                                string[] statusResult = CopyErrorResultToStatusResult(error18Result);
                                Status = new DeviceStatus(statusResult);

                                if (Status.ResponseCode == 0x14 || Status.ResponseCode == 0x18)//(Status.FiscalDayOpen == "1" || Status.SignsLimitExceeded == "1")
                                {
                                    result = SendCloseFisicalOpenDay();
                                    if (result.IndexOf("[[ΔΗΦΑΣΣ") != -1)
                                    {
                                        DeviceCommunicationSocket.Send(buffer);

                                        //resBuffer = new byte[1024];
                                        //readedByte = DeviceCommunicationSocket.Receive(resBuffer);
                                        //result = Encoding.GetEncoding(CodePage).GetString(resBuffer, 0, readedByte);
                                        result = ReadAnswer(DeviceCommunicationSocket);
                                        signDocumentResult = result.Substring(result.IndexOf("ΔΦΣΣ ") + "ΔΦΣΣ ".Length, result.IndexOf(",") - (result.IndexOf("ΔΦΣΣ ") + "ΔΦΣΣ ".Length));
                                    }
                                    else if (result.IndexOf("[[Error018") != -1)
                                    {
                                        result = result.Substring(result.IndexOf("[[Error018 ") + "[[Error018 ".Length, result.IndexOf("]]") - (result.IndexOf("[[Error018 ") + "[[Error018 ".Length));
                                        error18Result = result.Split(',');
                                        statusResult = CopyErrorResultToStatusResult(error18Result);
                                        Status = new DeviceStatus(statusResult);

                                    }
                                }
                            }
                            else
                            {
                                if (result.IndexOf("[[Error") != -1)
                                    return new SignatureData { Error = result };

                                //[<]800696676;;;173;;12;0.00;0.00;6.10;0.00;0.00;0.00;0.00;1.40;0.00;7.50;0;;;[>]
                                signDocumentResult = result.Substring(result.IndexOf("ΔΦΣΣ ") + "ΔΦΣΣ ".Length, result.IndexOf(",") - (result.IndexOf("ΔΦΣΣ ") + "ΔΦΣΣ ".Length));
                            }

                            signatureParts = result.Split(',');
                            if (string.IsNullOrWhiteSpace(signDocumentResult))
                                busy = Status.FiscalDocSignUnitBusy == "1" || Status.OnDocumentSign == "1";
                            else
                                busy = false;

                            if (busy)
                            {
                                System.Threading.Thread.Sleep(IntervalBetweenSignReΤries);
                                signDocumentReΤries--;
                            }
                            var valid = result.Split(',')[0] == signDocumentResult;
                        }
                        catch (Exception error)
                        {

                            throw new Exception(result, error);
                        }


                    }
                    while (busy && signDocumentReΤries > -1);

                    SignatureData signatureData = new SignatureData() { Signuture = signDocumentResult, QRCode = signatureParts[signatureParts.Length - 1] };
                    return signatureData;
                }
            }

        }

         string PrepareEpsilonLine(EpsilonLineData epsilonLineData)
        {
            string epsilon_line = string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0};{1};;{2};{3};{4};", epsilonLineData.afm_publisher, epsilonLineData.afm_recipient, epsilonLineData.transactionTypeID, epsilonLineData.series, epsilonLineData.taxDocNumber);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};{1:N2};{2:N2};{3:N2};{4:N2};", epsilonLineData.net_a, epsilonLineData.net_b, epsilonLineData.net_c, epsilonLineData.net_d, epsilonLineData.net_e);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};{1:N2};{2:N2};{3:N2};", epsilonLineData.vat_a, epsilonLineData.vat_b, epsilonLineData.vat_c, epsilonLineData.vat_d);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};0;;;", epsilonLineData.total_to_pay_poso);
            return "[<]" + epsilon_line + "[>]";
        }

        public override bool IsOnline { get => Status != null && !Status.SamtecDriverConnectionError; }
        private string ReadAnswer(Socket DeviceCommunicationSocket)
        {
            byte[] resBuffera = new byte[1024];
            int bytes = 0;
            MemoryStream mc = new MemoryStream();
            do
            {
                bytes = DeviceCommunicationSocket.Receive(resBuffera);
                if (bytes > 0)
                    mc.Write(resBuffera, 0, bytes);
            }
            while (bytes > 0);
            mc.Position = 0;
            resBuffera = mc.ToArray();
            var result = Encoding.GetEncoding(CodePage).GetString(resBuffera);
            return result;
        }

        private string SendCloseFisicalOpenDay()
        {
            lock (ConnectionLock)
            {
                using (System.Net.Sockets.Socket DeviceCommunicationSocket = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
                {

                    if (!DeviceCommunicationSocket.Connected)
                    {
                        try
                        {
                            DeviceCommunicationSocket.Connect("127.0.0.1", 6001);
                        }
                        catch (Exception error)
                        {

                            Status = new DeviceStatus() { SamtecDriverConnectionError = true };
                        }

                    }
                    if (DeviceCommunicationSocket.Connected)
                    {
                        string closeFisicalDay = "<<ΔΗΦΑΣΣ>>";
                        byte[] buffer = new byte[closeFisicalDay.Length + 2];
                        int i = 1;
                        foreach (var _byte in Encoding.GetEncoding(CodePage).GetBytes(closeFisicalDay))
                            buffer[i++] = _byte;
                        buffer[0] = 2;
                        buffer[buffer.Length - 1] = 3;

                        DeviceCommunicationSocket.Send(buffer);



                        //buffer = new byte[1024];
                        //int rededByte = DeviceCommunicationSocket.Receive(buffer);
                        //return Encoding.GetEncoding(CodePage).GetString(buffer, 0, rededByte);
                        return ReadAnswer(DeviceCommunicationSocket);
                    }
                    return "";
                }
            }

            // return "";
        }

        public class DeviceStatus
        {

            public static bool operator ==(DeviceStatus left, DeviceStatus right)
            {
                if (!(left is DeviceStatus) && !(right is DeviceStatus))
                    return true;
                if (!(left is DeviceStatus) && (right is DeviceStatus))
                    return false;
                if ((left is DeviceStatus) && !(right is DeviceStatus))
                    return false;


                return left.BatteryStatus == right.BatteryStatus &&
                left.CMOSReset == right.CMOSReset &&
                left.CriticalError == right.CriticalError &&
                left.ErrorDescription == right.ErrorDescription &&
                left.FiscalDayOpen == right.FiscalDayOpen &&
                left.FiscalDeviceCommunicationError == right.FiscalDeviceCommunicationError &&
                left.FiscalDocSignUnitBusy == right.FiscalDocSignUnitBusy &&
                left.FiscalDayOpen == right.FiscalDayOpen &&
                left.FiscalDocSignUnitOnline == right.FiscalDocSignUnitOnline &&
                left.FiscalMemoryAlmostFull == right.FiscalMemoryAlmostFull &&
                left.FiscalMemoryOverflow == right.FiscalMemoryOverflow &&
                left.InUserInterfaceMode == right.InUserInterfaceMode &&
                left.OutofPaper == right.OutofPaper &&
                left.SignsLimitExceeded == right.SignsLimitExceeded &&
                left.ResponseCode == right.ResponseCode;
            }
            public static bool operator !=(DeviceStatus left, DeviceStatus right)
            {
                return !(left == right);
            }

            public bool SamtecDriverConnectionError;

            public DeviceStatus(string[] statusResult)
            {
                ResponseCode = int.Parse(statusResult[0]);
                FiscalDocSignUnitBusy = statusResult[1];
                CriticalError = statusResult[2];
                OutofPaper = statusResult[3];
                CMOSReset = statusResult[4];
                PrinterOnline = statusResult[5];
                InUserInterfaceMode = statusResult[6];
                FiscalDocSignUnitOnline = statusResult[7];
                BatteryStatus = statusResult[8];
                FiscalDayOpen = statusResult[9];
                OnDocumentSign = statusResult[10];
                OnRefeeding = statusResult[11];
                FiscalMemoryAlmostFull = statusResult[12];
                SignsLimitExceeded = statusResult[13];
                FiscalMemoryOverflow = statusResult[14];
                if (statusResult.Length > 15)
                    ErrorDescription = statusResult[15];
            }
            public DeviceStatus()
            {

            }

            /// <summary>
            /// Error 17
            /// Communication error or FiscalDevice is power off. 
            /// </summary>
            public bool FiscalDeviceCommunicationError;


            /// <summary>
            /// << 1 >> Κωδικός απάντησης
            /// </summary>
            public int ResponseCode;
            /// <summary>
            /// << 2 >>: Συσκευή απασχολημένη
            /// </summary>
            public string FiscalDocSignUnitBusy;
            /// <summary>
            /// << 3 >>: Κρίσιμο σφάλμα
            /// </summary>
            public string CriticalError;
            /// <summary>
            /// << 4 >>: Τέλος χαρτιού
            /// </summary>
            public string OutofPaper;
            /// <summary>
            /// << 5 >>: CMOS reset
            /// </summary>
            public string CMOSReset;
            /// <summary>
            /// << 6 >>: Εκτυπωτής online
            /// </summary>
            public string PrinterOnline;
            /// <summary>
            /// << 7 >>: Χειρισμός μενού
            /// </summary>
            public string InUserInterfaceMode;
            /// <summary>
            /// << 8 >>: Φορολογική μονάδα online
            /// </summary>
            public string FiscalDocSignUnitOnline;
            /// <summary>
            /// << 9 >>: Κατάσταση μπαταρίας
            /// </summary>
            public string BatteryStatus;
            /// <summary>
            /// << 10 >>: Ανοιχτή ημέρα πρέπει να εκδοθεί Ζ
            /// </summary>
            public string FiscalDayOpen;
            /// <summary>
            /// << 11 >>: Διαδικασία έκδοσης σήμανσης σε εξέλιξη
            /// </summary>
            public string OnDocumentSign;
            /// <summary>
            /// << 12 >>: Διαδικασία επανατροφοδότησης σε εξέλιξη
            /// </summary>
            public string OnRefeeding;
            /// <summary>
            /// << 13 >>: Φορολογική μνήμη σχεδόν γεμάτη
            /// </summary>
            public string FiscalMemoryAlmostFull;
            /// <summary>
            /// << 14 >>: Όριο έκδοσης ημερήσιων σημάνσεων -απαιτείται Ζ
            /// </summary>
            public string SignsLimitExceeded;
            /// <summary>
            /// << 15 >>: Φορολογική μνήμη πλήρης
            /// </summary>
            public string FiscalMemoryOverflow;

            /// <summary>
            /// << 16 >>: Περιγραφή του σφάλματος
            /// </summary>
            public string ErrorDescription;
        }

        //Error 38
        //<<1>>: Το αποτέλεσμα της επικοινωνίας με τη συσκευή σήμανσης(χαμηλού επιπέδου επικοινωνία), 
        //<<2>>: Το αποτέλεσμα της επικοινωνίας με τη συσκευή σήμανσης(σε επίπεδο λειτουργίας), 
        //<<3>>: Το Reply Code της συσκευής σήμανσης όταν έγινε το σφάλμα, 

        //<<4>>: Συσκευή απασχολημένη, 
        //<<5>>: Κρίσιμο σφάλμα, 
        //<<6>>: Τέλος χαρτιού, 
        //<<7>>: CMOS reset, 
        //<<8>>: Εκτυπωτής online, 
        //<<9>>: Χειρισμός μενού, 
        //<<10>>: Φορολογική μονάδα online, 
        //<<11>>: Κατάσταση μπαταρίας, 
        //<<12>>: Ανοιχτή ημέρα, 
        //<<13>>: Διαδικασία έκδοσης σήμανσης σε εξέλιξη, 
        //<<14>>: Διαδικασία επανατροφοδότησης σε εξέλιξη, 
        //<<15>>: Φορολογική μνήμη σχεδόν γεμάτη, 
        //<<16>>: Όριο έκδοσης ημερήσιων σημάνσεων - απαιτείται Ζ, 
        //<<17>>: Φορολογική μνήμη πλήρης, 
        //<<18>>: Περιγραφή του σφάλματος

        //<<1>>: Κωδικός απάντησης

        //<<2>>: Συσκευή απασχολημένη
        //<<3>>: Κρίσιμο σφάλμα
        //<<4>>: Τέλος χαρτιού
        //<<5>>: CMOS reset
        //<<6>>: Εκτυπωτής online
        //<<7>>: Χειρισμός μενού
        //<<8>>: Φορολογική μονάδα online
        //<<9>>: Κατάσταση μπαταρίας
        //<<10>>: Ανοιχτή ημέρα
        //<<11>>: Διαδικασία έκδοσης σήμανσης σε εξέλιξη
        //<<12>>: Διαδικασία επανατροφοδότησης σε εξέλιξη
        //<<13>>: Φορολογική μνήμη σχεδόν γεμάτη
        //<<14>>: Όριο έκδοσης ημερήσιων σημάνσεων - απαιτείται Ζ
        //<<15>>: Φορολογική μνήμη πλήρης



        System.Timers.Timer SignDeviceStateTimer = new System.Timers.Timer();


 


    }




}
