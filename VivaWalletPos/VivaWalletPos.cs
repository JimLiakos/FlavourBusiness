using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VivaWalletPos
{
    public class VivaWalletPos
    {

        TimeSpan WaitTimeOut;
        string PosAddress;
        int Port;
        public VivaWalletPos(string posAddress, int port, TimeSpan waitTimeOut)
        {
            WaitTimeOut = waitTimeOut;
            PosAddress = posAddress;
            Port = port;
        }

        Task<PaymentData> SalesTask;

        int Sessionid = 1;
        public PaymentData Sale(double amount)
        {
#if DEBUG
            amount = 0.01;
#endif


            SalesTask = Task<PaymentData>.Run(() =>
            {
                txSaleRequest txRequest = new txSaleRequest() { sessionId = Sessionid++, msgType = "200", msgCode = "00", uniqueTxnId = Guid.NewGuid().ToString("N"), amount = amount, msgOpt = "0000" };
                string msm = txRequest.Message;
                //string message = "0061|002690|200|00|12345678901234567890123456789012|0.10|0000||||";
                try
                {

                    TcpClient client = new TcpClient(PosAddress, Port);
                    NetworkStream stream = client.GetStream();

                    PaymentData paymentData = null;
                    try
                    {
                        client.ReceiveTimeout = (int)WaitTimeOut.TotalMilliseconds;



                        // Translate the passed message into ASCII and store it as a Byte array.
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(msm);

                        // Send the message to the connected TcpServer.
                        stream.Write(data, 0, data.Length);


                        Console.WriteLine("Sent: {0}", msm);

                        // Receive the TcpServer.response.

                        // Buffer to store the response bytes.
                        data = new Byte[256];

                        // String to store the response ASCII representation.
                        String responseData = String.Empty;

                        // Read the first batch of the TcpServer response bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                        if (responseData.IndexOf("0014|810|00|") == 0)
                        {
                            bytes = stream.Read(data, 0, data.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            txSalesResponse response = txSalesResponse.Parse(responseData);

                            //txSalesResponse response = txSalesResponse.Parse("0135|000065|210|00|00|Contactless/None~RRN:108813789134|VISA|406001******4011|789134|181644|000001|0.05|1010|00|00|00|00|00|00|    16038894");
                            if (response.respCodeResp == "00")
                            {
                                paymentData = new PaymentData();
                                paymentData.CardType = response.cardTypeResp;
                                paymentData.AccountNum = response.accNumberResp;
                                paymentData.PayAmount = response.amount.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo(1033));
                                paymentData.TransactionID = response.refNumResp;


                            }

                            //"0135|000065|210|00|00|Contactless/None~RRN:108813789134|VISA|406001******4011|789134|181644|000001|0.05|1010|00|00|00|00|00|00|    16038894"
                            //"0119|000066|210|00|UC|Cancelled by User|MASTERCARD|535142******7137|000000||000001|0.05|1010|00|00|00|00|00|00|    16038894"
                            //"0093|000068|210|00|UC|Cancelled by User|||000000||000001|0.05|1010|00|00|00|00|00|00|    16038894"
                        }
                    }
                    catch (Exception error)
                    {
                    }



                    // Close everything.
                    stream.Close();
                    client.Close();
                    return paymentData;
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }

                return default(PaymentData);

            });

            SalesTask.Wait();
            return SalesTask.Result;

        }

    }


    class txSaleRequest
    {
        public int sessionId;
        public string msgType;
        public string msgCode;
        public string uniqueTxnId;
        public double amount;
        public string msgOpt;

        public string Message
        {
            get
            {
                string sessionIdStr = sessionId.ToString();
                while (sessionIdStr.Length < 6)
                    sessionIdStr = "0" + sessionIdStr;

                string message = "|" + sessionIdStr;

                if (msgType.Length > 3)
                    msgType = msgType.Substring(msgType.Length - 3);
                while (msgType.Length < 3)
                    msgType = "0" + msgType;
                message = message + "|" + msgType;

                if (msgCode.Length > 2)
                    msgCode = msgType.Substring(msgCode.Length - 2);
                while (msgCode.Length < 2)
                    msgCode = "0" + msgCode;
                message = message + "|" + msgCode;

                if (uniqueTxnId.Length > 32)
                    uniqueTxnId = msgType.Substring(uniqueTxnId.Length - 32);
                while (uniqueTxnId.Length < 32)
                    uniqueTxnId = "0" + uniqueTxnId;
                message = message + "|" + uniqueTxnId;

                var amountStr = amount.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo(1033));
                if (amountStr.Length > 13)
                    amountStr = msgType.Substring(amountStr.Length - 13);

                message = message + "|" + amountStr;

                if (msgOpt.Length > 4)
                    msgOpt = msgType.Substring(msgOpt.Length - 4);
                while (msgOpt.Length < 4)
                    msgOpt = "0" + msgOpt;
                message = message + "|" + msgOpt;

                message += "||||";

                string messageLength = (message.Length).ToString();

                while (messageLength.Length < 4)
                    messageLength = "0" + messageLength;

                message = messageLength + message;


                return message;


            }
        }

    }



    class txSalesResponse
    {
        public int seqTxnId;
        public string msgTypeResp;
        public string msgCodeResp;
        public string respCodeResp;
        public string respMessageResp;

        public string cardTypeResp;
        public string accNumberResp;
        public string refNumResp;
        public string authCodeResp;

        public string batchNumResp;



        public double amount;
        public string msgOptResp;


        public string tipAmountResp;
        public string foreignAmountResp;
        public string foreignCurrencyCode;

        public string exchangeRageInclMarkupResp;
        public string dccMarkcupPercentage;
        public string dccExchangeDateOfRateResp;
        public string eftTidResp;

        internal static txSalesResponse Parse(string message)
        {
            var txSalesResponse = new txSalesResponse();
            txSalesResponse.seqTxnId = int.Parse(message.Split('|')[1]);
            txSalesResponse.msgTypeResp = message.Split('|')[2];
            txSalesResponse.msgCodeResp = message.Split('|')[3];
            txSalesResponse.respCodeResp = message.Split('|')[4];
            txSalesResponse.respMessageResp = message.Split('|')[5];
            txSalesResponse.cardTypeResp = message.Split('|')[6];
            txSalesResponse.accNumberResp = message.Split('|')[7];
            txSalesResponse.refNumResp = message.Split('|')[8];
            txSalesResponse.authCodeResp = message.Split('|')[9];
            txSalesResponse.batchNumResp = message.Split('|')[10];
            txSalesResponse.amount = double.Parse(message.Split('|')[11], System.Globalization.CultureInfo.GetCultureInfo(1033));

            return txSalesResponse;
        }
    }
}
