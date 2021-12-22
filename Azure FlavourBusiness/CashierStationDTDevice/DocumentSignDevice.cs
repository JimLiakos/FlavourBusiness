using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FinanceFacade;

namespace CashierStationDevice
{
    /// <MetaDataID>{4b187271-a093-454c-9fb4-b7f320c22047}</MetaDataID>
    public class DocumentSignDevice: IDocumentSignDevice
    {
        public event EventHandler<DeviceStatus> DeviceStatusChanged;
        public DocumentSignDevice()
        {

            SignDeviceStateTimer.Elapsed += SignDeviceStateTimer_Elapsed;
            SignDeviceStateTimer.Interval = 5000;
            SignDeviceStateTimer.Enabled = true;
            SignDeviceStateTimer.Start();

        }
        public static void Init()
        {
            if (_CurrentDocumentSignDevice == null)
                _CurrentDocumentSignDevice = new DocumentSignDevice();
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

        public List<string> CheckStatusForError()
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
                    DeviceStatusChanged?.Invoke(this, value);
                }
            }
        }
        public SignatureData SignDocument(string document)
        {
            lock (ConnectionLock)
            {
                using (System.Net.Sockets.Socket DeviceCommunicationSocket = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
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
                                //[<]800696676;;;173;;12;0.00;0.00;6.10;0.00;0.00;0.00;0.00;1.40;0.00;7.50;0;;;[>]
                                signDocumentResult = result.Substring(result.IndexOf("ΔΦΣΣ ") + "ΔΦΣΣ ".Length, result.IndexOf(",") - (result.IndexOf("ΔΦΣΣ ") + "ΔΦΣΣ ".Length));

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

        public string PrepareEpsilonLine(string epsilon_line)
        {
            return "[<]" + epsilon_line + "[>]";
        }
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


        /// <MetaDataID>{7dbf720f-5fba-49ed-be53-7d53eea2bd88}</MetaDataID>
        static DocumentSignDevice _CurrentDocumentSignDevice;
        /// <MetaDataID>{3bbfd8a1-db71-4ba3-96b5-4922d11e714e}</MetaDataID>
        public static DocumentSignDevice CurrentDocumentSignDevice
        {
            get
            {
                if (_CurrentDocumentSignDevice == null)
                    _CurrentDocumentSignDevice = new DocumentSignDevice();

                return _CurrentDocumentSignDevice;
            }
        }



    }

    /// <MetaDataID>{6a6a78fc-14d2-463e-ba92-5f1d0cdc7af8}</MetaDataID>
    public class SignatureData
    {
        public string Signuture;
        public string QRCode;
    }

    public interface IDocumentSignDevice
    {
        SignatureData SignDocument(string document);
        string PrepareEpsilonLine(string epsilon_line);
    }


    /// <MetaDataID>{500f1a84-0bc9-4e36-bb81-3ebb1e348cc0}</MetaDataID>
    public class DocumentSignDeviceA : ICashierDevice
    {
        public event EventHandler<DeviceStatus> DeviceStatusChanged;
        public DocumentSignDeviceA()
        {
            SignDeviceStateTimer.Elapsed += SignDeviceStateTimer_Elapsed;
            SignDeviceStateTimer.Interval = 5000;
            SignDeviceStateTimer.Enabled = true;
            SignDeviceStateTimer.Start();

        }
        public static void Init()
        {
            if (_CurrentDocumentSignDevice == null)
                _CurrentDocumentSignDevice = new DocumentSignDevice();
        }

        System.Net.Sockets.Socket DeviceCommunicationSocket = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

        private void SignDeviceStateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (DeviceCommunicationSocket)
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


                byte[] buffer = new byte["<<ΑΚΣΣ>>".Length + 2];
                int i = 1;
                foreach (var _byte in Encoding.GetEncoding(CodePage).GetBytes("<<ΑΚΣΣ>>"))
                    buffer[i++] = _byte;
                buffer[0] = 2;
                buffer[buffer.Length - 1] = 3;

                DeviceCommunicationSocket.Send(buffer);
                buffer = new byte[1024];
                int rededByte = DeviceCommunicationSocket.Receive(buffer);

                string result = Encoding.GetEncoding(1253).GetString(buffer, 0, rededByte);

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
        }

        public List<string> CheckStatusForError()
        {
            List<string> statusMessages = new List<string>();

            if (Status.SamtecDriverConnectionError)
            {
                statusMessages.Add(Properties.Resources.SamtecDriverConnectionErrorMessage);
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
                    DeviceStatusChanged?.Invoke(this, value);
                }
            }
        }
        public string SignDocument(string document)
        {
            lock (DeviceCommunicationSocket)
            {


                if (!DeviceCommunicationSocket.Connected)
                    DeviceCommunicationSocket.Connect("127.0.0.1", 6001);

                document = "<<ΔΦΣΣ>>" + document;
                bool busy = false;
                string signDocumentResult = "";
                var signDocumentReΤries = SignDocumentReΤries;
                do
                {



                    byte[] buffer = new byte[document.Length + 2];
                    int i = 1;
                    foreach (var _byte in Encoding.GetEncoding(CodePage).GetBytes(document))
                        buffer[i++] = _byte;
                    buffer[0] = 2;
                    buffer[buffer.Length - 1] = 3;

                    DeviceCommunicationSocket.Send(buffer);
                    byte[] resBuffer = new byte[1024];
                    int rededByte = DeviceCommunicationSocket.Receive(resBuffer);

                    string result = Encoding.GetEncoding(CodePage).GetString(resBuffer, 0, rededByte);

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
                                resBuffer = new byte[1024];
                                rededByte = DeviceCommunicationSocket.Receive(resBuffer);
                                result = Encoding.GetEncoding(CodePage).GetString(resBuffer, 0, rededByte);
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
                        //[<]800696676;;;173;;12;0.00;0.00;6.10;0.00;0.00;0.00;0.00;1.40;0.00;7.50;0;;;[>]
                        signDocumentResult = result.Substring(result.IndexOf("ΔΦΣΣ ") + "ΔΦΣΣ ".Length, result.IndexOf(",") - (result.IndexOf("ΔΦΣΣ ") + "ΔΦΣΣ ".Length));

                    if (string.IsNullOrWhiteSpace(signDocumentResult))
                        busy = Status.FiscalDocSignUnitBusy == "1" || Status.OnDocumentSign == "1";
                    else
                        busy = false;

                    if (busy)
                    {
                        System.Threading.Thread.Sleep(IntervalBetweenSignReΤries);
                        signDocumentReΤries--;
                    }


                }
                while (busy && signDocumentReΤries > -1);
                return signDocumentResult;
            }

        }

        public string PrepareEpsilonLine(string epsilon_line)
        {
            return "[<]" + epsilon_line + "[>]";
        }
        private string SendCloseFisicalOpenDay()
        {


            //string closeFisicalDay = "<<ΔΗΦΑΣΣ>>";
            //byte[] buffer = new byte[closeFisicalDay.Length + 2];
            //int i = 1;
            //foreach (var _byte in Encoding.GetEncoding(CodePage).GetBytes(closeFisicalDay))
            //    buffer[i++] = _byte;
            //buffer[0] = 2;
            //buffer[buffer.Length - 1] = 3;

            //DeviceCommunicationSocket.Send(buffer);
            //buffer = new byte[1024];
            //int rededByte = DeviceCommunicationSocket.Receive(buffer);
            //return Encoding.GetEncoding(CodePage).GetString(buffer, 0, rededByte);

            return "";
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


        /// <MetaDataID>{7dbf720f-5fba-49ed-be53-7d53eea2bd88}</MetaDataID>
        static DocumentSignDevice _CurrentDocumentSignDevice;
        /// <MetaDataID>{3bbfd8a1-db71-4ba3-96b5-4922d11e714e}</MetaDataID>
        public static DocumentSignDevice CurrentDocumentSignDevice
        {
            get
            {
                if (_CurrentDocumentSignDevice == null)
                    _CurrentDocumentSignDevice = new DocumentSignDevice();

                return _CurrentDocumentSignDevice;
            }
        }

        public static Dictionary<string, int> VatAcounts = new Dictionary<string, int> { { "a1", 0 }, { "b1", 1 }, { "c1", 2 }, { "d1", 3 } };
        /// <MetaDataID>{f549381f-3821-4725-b1c4-255b4647dd9e}</MetaDataID>
        public void PrintReceipt(ITransaction transaction)
        {

            string printText = null;
            Print(transaction, "13", "", false, "", "", ref printText);

            string myafm = transaction.PayeeRegistrationNumber;
            string clientafm = "";
            string linikiID = "173";
            string series = "";
            string taxDocNumber = "12";
            decimal net_a = 0;
            decimal net_b = 0;
            decimal net_c = 0;
            decimal net_d = 0;
            decimal net_e = 0;
            decimal vat_a = 0;
            decimal vat_b = 0;
            decimal vat_c = 0;
            decimal vat_d = 0;
            decimal total_to_pay_poso = 0;

            foreach (var item in transaction.Items)
            {
                total_to_pay_poso += item.Price;

                if (VatAcounts[item.Taxes[0].AccountID] == 0)
                {
                    vat_a += item.Taxes[0].Amount;
                    net_a += item.Price - item.Taxes[0].Amount;
                }
                if (VatAcounts[item.Taxes[0].AccountID] == 1)
                {
                    vat_b += item.Taxes[0].Amount;
                    net_b += item.Price - item.Taxes[0].Amount;
                }

                if (VatAcounts[item.Taxes[0].AccountID] == 2)
                {
                    vat_c += item.Taxes[0].Amount;
                    net_c += item.Price - item.Taxes[0].Amount;
                }

                if (VatAcounts[item.Taxes[0].AccountID] == 3)
                {
                    vat_d += item.Taxes[0].Amount;
                    net_d += item.Price - item.Taxes[0].Amount;
                }

            }

            string afdsDoc = printText;



            afdsDoc += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "[<]{0};{1};;{2};{3};{4};", myafm, clientafm, linikiID, series, taxDocNumber);
            afdsDoc += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};{1:N2};{2:N2};{3:N2};{4:N2};", net_a, net_b, net_c, net_d, net_e);
            afdsDoc += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};{1:N2};{2:N2};{3:N2};", vat_a, vat_b, vat_c, vat_d);
            afdsDoc += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};0;;;[>]", total_to_pay_poso);

            string signature = SignDocument(afdsDoc);
            if (string.IsNullOrWhiteSpace(signature))
            {

            }
            else
                Print(transaction, "13", "", false, "", signature, ref printText);

        }


        /// <MetaDataID>{41559a71-b5b4-435c-8bab-4a00e79412d5}</MetaDataID>
        public static bool Print(ITransaction transaction, string tableNumber, string ReportPath, bool change, string comments, string signuture, ref string printTxt)
        {




            int currPrintOrder = -200;


            try
            {

                string Water = "Liakos";
                System.Collections.Generic.Dictionary<string, decimal> VatSums = new Dictionary<string, decimal>();

                decimal price = 0;
                decimal negprice = 0;

                string printReport = null;

                byte[] data = Read(typeof(CashierStationDevice.DocumentSignDevice).Assembly.GetManifestResourceStream("CashierStationDevice.Resources.order.bin"));
                printReport = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);




                System.IO.StringWriter orderPrintWr = new System.IO.StringWriter();
                System.IO.StringReader orderPrint = new System.IO.StringReader(printReport);


                string lineString = orderPrint.ReadLine();
                while (lineString != null && lineString.IndexOf("&") == -1)
                {
                    if (lineString.IndexOf("#=#") != -1)
                    {
                        if (change == false)
                            lineString = lineString.Replace("#=#", "");
                        else
                            lineString = CenterStringOnLine(lineString, "Order changed");
                    }

                    //Order Code
                    string orderCode = "AA12";
                    if (lineString.IndexOf("~") != -1)
                        lineString = FixLengthReplace(orderCode, lineString, "~");

                    //table number 
                    if (lineString.IndexOf("@") != -1)
                        lineString = FixLengthReplace(tableNumber, lineString, "@");

                    //waiter
                    if (lineString.IndexOf("+") != -1)
                        lineString = FixLengthReplace(Water, lineString, "+");


                    #region DateTime line
                    if (lineString.IndexOf("^") != -1 || lineString.IndexOf("%") != -1)
                    {
                        int len = lineString.Length;

                        int year = System.DateTime.Now.Year - 2000;

                        if (ReportPath.IndexOf("WaiterClear.txt") == 1)
                            year = DateTime.Now.Year - 2000;

                        string yearstr = year.ToString();
                        if (yearstr.Length == 1)
                            yearstr = "0" + yearstr;

                        if (ReportPath.IndexOf("WaiterClear.txt") == -1)
                            lineString = lineString.Replace("^", string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, yearstr));
                        else
                            lineString = lineString.Replace("^", string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, yearstr));



                        string hourStr = DateTime.Now.Hour.ToString();
                        if (hourStr.Length == 1)
                            hourStr = "0" + hourStr;

                        string minuteStr = DateTime.Now.Minute.ToString();
                        if (minuteStr.Length == 1)
                            minuteStr = "0" + minuteStr;
                        lineString = lineString.Replace("%", string.Format("{0}:{1}", hourStr, minuteStr));
                    }
                    #endregion




                    if (lineString.IndexOf("$") != -1)
                    {
                        string companyName = "";// PropertiesUI.GetCompanyName();
                        int len = (lineString.Length - companyName.Length) / 2;
                        while (len >= 0)
                        {
                            companyName = " " + companyName;
                            len--;
                        }
                        orderPrintWr.WriteLine(companyName);
                    }
                    else
                        orderPrintWr.WriteLine(lineString);
                    lineString = orderPrint.ReadLine();
                }
                //System.Windows.Forms.MessageBox.Show(PageHeader);
                string OrderItemLine = lineString;
                string ingrSpace = null;

                lineString = orderPrint.ReadLine();
                while (lineString != null && lineString.IndexOf("##") == -1)
                {
                    ingrSpace += lineString;
                    lineString = orderPrint.ReadLine();
                }

                string IngredientLine = lineString;




                foreach (var orderItem in transaction.Items)
                {


                    foreach (var taxAmount in orderItem.Taxes)
                    {
                        if (!VatSums.ContainsKey(taxAmount.AccountID))
                            VatSums.Add(taxAmount.AccountID, 0);

                        decimal totalOnVat = VatSums[taxAmount.AccountID];
                        totalOnVat += orderItem.Price;
                        VatSums[taxAmount.AccountID] = orderItem.Price;
                    }


                    if (orderItem.Price > 0)
                        price += orderItem.Price;
                    else
                        negprice += orderItem.Price;


                    string currOrderItemLine = OrderItemLine;
                    currOrderItemLine = FixLengthReplace(orderItem.Quantity.ToString(), currOrderItemLine, "&");
                    currOrderItemLine = FixLengthReplace(orderItem.Name, currOrderItemLine, "*");
                    currOrderItemLine = FixLengthReplace(GetPriceAsString(orderItem.Price), currOrderItemLine, "!", "!!");
                    orderPrintWr.WriteLine(currOrderItemLine);



                }

                lineString = orderPrint.ReadLine();
                string neglineString = "";

                while (lineString != null)
                {
                    if (lineString.IndexOf("?") != -1)
                    {
                        if (negprice != 0)
                            neglineString = FixLengthReplace(GetPriceAsString(negprice), lineString, "?");
                        lineString = FixLengthReplace(GetPriceAsString(price), lineString, "?");
                    }

                    if (lineString.IndexOf("!@#$%^") != -1)
                    {

                        string temp = "PAraa\r\nsdfsdfa";

                        System.IO.StringReader LastString = new System.IO.StringReader(comments);
                        string LastStringline = LastString.ReadLine();
                        if (LastStringline == null)
                            lineString = "";

                        while (LastStringline != null)
                        {
                            string NextLine = LastString.ReadLine();
                            if (NextLine != null)
                                orderPrintWr.WriteLine(LastStringline);
                            else
                                lineString = LastStringline;
                            LastStringline = NextLine;

                        }

                    }


                    if (lineString.IndexOf("!@") != -1)
                    {
                        if (signuture != null)
                        {
                            do
                            {
                                orderPrintWr.WriteLine(FixLengthReplaceWithRemain(signuture, lineString, "!@", out signuture));

                            }
                            while (signuture != null);
                        }
                    }
                    else if (lineString.IndexOf(" | ") != -1)
                    {
                        foreach (var entry in VatSums)
                        {
                            string VatLine = "" + entry.Key.ToString() + "%   " + GetPriceAsString((double)entry.Value);//entry.Value.ToString();

                            orderPrintWr.WriteLine(VatLine);
                        }

                    }
                    else
                    {

                        orderPrintWr.WriteLine(lineString);
                        if (neglineString.Length > 0)
                        {
                            orderPrintWr.WriteLine(neglineString);
                            neglineString = "";
                        }
                    }
                    lineString = orderPrint.ReadLine();

                }



                string FinalOrderStr = orderPrintWr.ToString();

                //System.IO.Stream OutorderPrintStream = System.IO.File.Open("\\Temp\\OutOrderWin.txt", System.IO.FileMode.OpenOrCreate);
                //OutorderPrintStream.SetLength(0);
                byte[] outByteStr = System.Text.ASCIIEncoding.Default.GetBytes(FinalOrderStr);

                //OutorderPrintStream.Write(outByteStr, 0, outByteStr.Length);
                //OutorderPrintStream.Close();
                printTxt = FinalOrderStr;
                return true;
                int ewte = 0;
            }
            catch (System.Exception error)
            {
                int tst = 0;
            }

            return false;

        }


        /// <MetaDataID>{661189ff-24a2-4352-ad42-af1762b7bf6e}</MetaDataID>
        private static byte[] Read(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }


        /// <MetaDataID>{597db01e-1ff3-4f60-a6f0-76570d120071}</MetaDataID>
        private static string CenterStringOnLine(string lineString, string stringToCenter)
        {
            int len = (lineString.Length - stringToCenter.Length) / 2;
            while (len >= 0)
            {
                stringToCenter = " " + stringToCenter;
                len--;
            }
            return stringToCenter;
        }

        /// <MetaDataID>{a4310878-48f3-4bd7-9871-1271c4dc783c}</MetaDataID>
        private static string FixLengthReplace(string tableNumber, string lineString, string signChar, string startSign = null)
        {
            if (startSign == null)
                startSign = signChar;
            int startPos = lineString.IndexOf(startSign);
            int endPos = lineString.LastIndexOf(signChar);


            string TableNumberStr = null;
            if (tableNumber.ToString().Length > endPos - startPos + 1)
                TableNumberStr = tableNumber.ToString().Substring(0, endPos - startPos + 1);
            else
                TableNumberStr = tableNumber.ToString();

            while (TableNumberStr.Length < endPos - startPos + 1)
                TableNumberStr += " ";
            string TableChars = null;
            while (startPos <= endPos)
            {
                startPos++;
                TableChars += signChar;
            }
            lineString = lineString.Replace(TableChars, TableNumberStr);
            int rtr = 0;
            return lineString;
        }


        /// <MetaDataID>{a8674a01-c8c5-4d5c-a9dc-07bed3a898e1}</MetaDataID>
        private static string FixLengthReplaceWithRemain(string inputString, string lineString, string signChar, out string remainString)
        {
            string startSign = null;
            if (startSign == null)
                startSign = signChar;
            int startPos = lineString.IndexOf(startSign);
            int endPos = lineString.LastIndexOf(signChar) + signChar.Length - 1;


            string TableNumberStr = null;
            if (inputString.ToString().Length > endPos - startPos + 1)
            {
                TableNumberStr = inputString.ToString().Substring(0, endPos - startPos + 1);
                remainString = inputString.ToString().Substring(TableNumberStr.Length);
            }
            else
            {
                remainString = null;
                TableNumberStr = inputString.ToString();
            }

            while (TableNumberStr.Length < endPos - startPos + 1)
                TableNumberStr += " ";
            string TableChars = null;
            while (startPos <= endPos)
            {
                startPos += signChar.Length;
                TableChars += signChar;
            }
            lineString = lineString.Replace(TableChars, TableNumberStr);
            int rtr = 0;
            return lineString;
        }

        /// <MetaDataID>{ce5a0191-6c8f-46c8-a1f2-8b0c407a5fa3}</MetaDataID>
        private static string GetPriceAsString(object price)
        {
            return string.Format("{0:N2}", price);
        }

    }
}


//7.1. Table 1, Reply codes / error codes
//+---+----------------------------------+----------------------------------------+
//|Hex| Meaning                          | Suggested action                       |
//+---+----------------------------------+----------------------------------------+
//| 00| No errors - success              | None                                   |
//| 01| Wrong number of fields           | Check the command's field count        |
//| 02| Field too long                   | A field is long: check it & retry      |
//| 03| Field too small                  | A field is small: check it & retry     |
//| 04| Field fixed size mismatch        | A field size is wrong: check it & retry|
//| 05| Field range or type check failed | Check ranges or types in command       |
//| 06| Bad request code | Correct the request code(unknown)                      |
//| 09| Printing type bad | Correct the specified printing style                  |
//| 0A| Cannot execute with day open | Issue a Z report to close the day          |
//| 0B| RTC programming requires jumper | Short the 'clock' jumper and retry      |
//| 0C| RTC date or time invalid | Check the date / time range.Also check         |
//|   |                                  | if date is prior to a date of a fiscal |
//|   |                                  | record.                                |
//| 0D| No records in fiscal period | No suggested action; the operation can-     |
//|   |                                  | not be executed in the specified period|
//| 0E| Device is busy in another task   | Wait for the device to get ready       |
//| 0F| No more header records allowed   | No suggested action; the header pro-   |
//|   |                                  | gramming cannot be executed because the|
//|   |                                  | fiscal memory cannot hold more records |
//| 10| Cannot execute with block open   | The specified command requires no open |
//|   |                                  | signature block for proceeding.Close   |
//|   |                                  | the block and retry.                   |
//| 11| Block not open                   | The specified command requires a signa-|
//|   |                                  | ture block to be open to execute.Open  |
//|   |                                  | a block and retry.                     |
//| 12| Bad data stream                  | Means that the passed data to be signed|
//|   |                                  | are of incorrect format. The expected  |
//|   |                                  | format is in HEX (hexadecimal) pairs,  |
//|   |                                  | so expected field must have an even    |
//|   |                                  | size and its contents must be in range |
//|   |                                  | '0'-'9' or 'A'-'F' inclusive.          |
//| 13| Bad signature field              | Means that the passed signature is of  |
//|   |                                  | incorrect format.The expected format   |
//|   |                                  | is of 40 characters formatted as 20 HEX|
//|   |                                  | (hexadecimal) pairs.                   |
//| 14| Z closure time limit             | Means that 24 hours passed from the    |
//|   |                                  | last Z closure.Issue a Z and retry.    |
//| 15| Z closure not found              | The specified Z closure number does not|
//|   |                                  | exist.Pass an existing Z number.       |
//| 16| Z closure record bad             | The requested Z record is unreadable   |
//|   |                                  | (damaged). Device requires service     |
//| 17| User browsing in progress        | The user is accessing the device by    |
//|   |                                  | manual operation.The protocol usage    |
//|   |                                  | is suspended until the user terminates |
//|   |                                  | the keyboard browsing.Just wait or     |
//|   |                                  | inform application user.               |
//| 18| Signature daily limit reached    | The max number of signatures in a day  |
//|   |                                  | have been issued.A Z closure is needed |
//|   |                                  | to free the daily storage memory.      |
//| 19| Printer paper end detected       | Replace the paper roll and retry       |
//| 1A| Printer is offline               | Printer disconnection. Service required|
//| 1B| Fiscal unit is offline           | Fiscal disconnection. Service required.|
//| 1C| Fatal hardware error             | Mostly fiscal errors.Service required. |
//| 1D| Fiscal unit is full              | Need fiscal replacement.Service        |
//| 1E| No data passed for signature     | Need to pass some data to close block  |
//| 1F| Signature does not exist         | Correct requested signature number     |
//| 20| Battery fault detected           | If problem persists, service required  |
//| 21| Recovery in progress             | This command is not allowed when a     |
//|   |                                  | recovery has started.Finish the        |
//|   |                                  | recovery procedure and retry           |
//| 22| Recovery only after CMOS reset   | Attempted to initiate a recovery       |
//|   |                                  | procedure without a previous CMOS      |
//|   |                                  | reset.The recovery is not needed.     |
//| 23| Real-Time Clock needs programming| This means that the RTC has invalid    |
//|   |                                  | data and needs to be reprogrammed. As  |
//|   |                                  | a consequence, service is needed.      |
//| 24| Z closure date warning           | This is an error returned by a closure |
//|   |                                  | request, when the RTC's date has a     |
//|   |                                  | value at least 48 hours later than the |
//|   |                                  | last closure time stamp (see XZreport) |
//| 25| Bad character in stream          | This error is returned when a stream   |
//|   |                                  | sent contains one or more invalid      |
//|   |                                  | characters.A table of allowed binary  |
//|   |                                  | values is defined in 'table 2'. This   |
//|   |                                  | error means that device has rejected   |
//|   |                                  | the specified frame.A filtering of    |
//|   |                                  | data sent to the device* must* be      |
//|   |                                  | performed by host.                     |
//+---+----------------------------------+----------------------------------------+


