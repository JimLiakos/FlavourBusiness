using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulMar.Atapi;
//https://github.com/markjulmar/itapi3
namespace CallerIDDevice
{
    public delegate void IncomingCall(string ControlerID, string caller, string calledNumber);
    public delegate void EndCall(string ControlerID, string caller, string calledNumber); //(int ControlerID, string Caller, int TypeID, string Type, string CalledNumber)

    /// <MetaDataID>{1929e28a-f194-4e4d-87ec-9bd217aefe9c}</MetaDataID>
    public class Tapi2CallerID
    {


        public event IncomingCall IncomingCall;

        public event IncomingCall EndCall;

        TapiManager tapiManager = new TapiManager("TapiCallMonitor.net");

        const int COLUMNS_CID = 1;
        const int COLUMNS_STATE = 2;
        const int COLUMNS_CALLER = 3;
        const int COLUMNS_CALLED = 4;

        public Tapi2CallerID()
        {
            //System.Windows.Forms.MessageBox.Show("Initializetapi3");
            Initializetapi();
        }
        string tapiDeviceName = "LSI USB 2.0 Soft Modem";
        void Initializetapi()
        {
            try
            {
                if (tapiManager.Initialize() == false)
                {

                    return;
                }


                //Microsoft.Win32.RegistryKey settinsKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\ArionSoftware\Commerce\Settings");
                //if (settinsKey == null)
                //    return;

                //tapiDeviceName = settinsKey.GetValue("TapiDeviceName") as string;
                //settinsKey.Close();
                if (string.IsNullOrEmpty(tapiDeviceName))
                    return;
                foreach (TapiLine line in tapiManager.Lines)
                {
                    try
                    {
                        if (line.Name == tapiDeviceName)
                        {

                            line.Open(MediaModes.All);
                            line.NewCall += OnNewCall;
                            line.CallInfoChanged += new EventHandler<CallInfoChangeEventArgs>(line_CallInfoChanged);
                            //line.Monitor();
                        }
                    }
                    catch (TapiException ex)
                    {

                    }
                }

            }
            catch (Exception e)
            {

            }
        }


        void line_CallInfoChanged(object sender, CallInfoChangeEventArgs e)
        {
            try
            {
                TapiCall call = e.Call;
                if (e.Change == CallInfoChangeTypes.CallerId)
                {

                    //if (_EventConsumer != null)
                    {
                        lock (Calls)
                        {
                            if (!Calls.ContainsKey(call.ToString()))
                            {
                                Calls[call.ToString()] = System.DateTime.Now;
                                string callerNumber = call.CallerId;
                                string calledNumber = call.CalledId;

                                IncomingCall.Invoke(tapiDeviceName, callerNumber, calledNumber);
                                //_EventConsumer.IncomingCall(0, callerNumber, 0, "", calledNumber);
                            }
                            else if ((System.DateTime.Now - Calls[call.ToString()]).TotalSeconds > 30)
                            {

                                Calls[call.ToString()] = System.DateTime.Now;
                                string callerNumber = call.CallerId;
                                string calledNumber = call.CalledId;
                                IncomingCall.Invoke(tapiDeviceName, callerNumber, calledNumber);
                                //_EventConsumer.IncomingCall(0, callerNumber, 0, "", calledNumber);
                            }

                        }
                    }

                }
            }
            catch (Exception error)
            {


            }
        }
        System.Collections.Generic.Dictionary<string, DateTime> Calls = new Dictionary<string, DateTime>();
        private void OnNewCall(object sender, NewCallEventArgs e)
        {
            //if (this.InvokeRequired)
            //{
            //    EventHandler<NewCallEventArgs> eh = OnNewCall;
            //    this.BeginInvoke(eh, new object[] { sender, e });
            //    return;
            //}

            try
            {
                TapiLine line = (TapiLine)sender;
                TapiCall call = e.Call;

                //if (_EventConsumer != null && !string.IsNullOrEmpty(call.CallerId))
                {

                    lock (Calls)
                    {
                        if (!Calls.ContainsKey(call.ToString()))
                        {
                            Calls[call.ToString()] = System.DateTime.Now;
                            string callerNumber = call.CallerId;
                            string calledNumber = call.CalledId;
                            IncomingCall.Invoke(tapiDeviceName, callerNumber, calledNumber);
                            //_EventConsumer.IncomingCall(0, callerNumber, 0, "", calledNumber);
                        }
                        else if ((System.DateTime.Now - Calls[call.ToString()]).TotalSeconds > 30)
                        {

                            Calls[call.ToString()] = System.DateTime.Now;
                            string callerNumber = call.CallerId;
                            string calledNumber = call.CalledId;
                            IncomingCall.Invoke(tapiDeviceName, callerNumber, calledNumber);
                            //_EventConsumer.IncomingCall(0, callerNumber, 0, "", calledNumber);
                        }

                    }


                }
            }
            catch (Exception error)
            {


            }



        }



    }
}
