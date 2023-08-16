using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading.Tasks;

namespace VivaWalletPos
{
    /// <MetaDataID>{c3bc410e-12bb-4eaa-8be3-11c201f46a41}</MetaDataID>
    public interface IPos
    {
        Task<PaymentData> ReceivePayment(decimal total, decimal tips);

        void Confing(POSType terminalType, string ipAddress = "", int port = 0, double waitTimeOutInSec = 0);
    }

    /// <MetaDataID>{c8c2a935-a649-4275-ac2f-ed37ad7701c0}</MetaDataID>
    public class PaymentData
    {
        public string CardType { get; set; }
        public string AccountNum { get; set; }
        public string TransactionID { get; set; }
        public string PayAmount { get; set; }

        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <MetaDataID>{6f631af7-2702-4293-aa97-7eadd1c7c2cf}</MetaDataID>
    public enum POSType
    {
        AppPOS,
        TerminalPos
    }

}
