using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaWalletPos
{
    public class Pos : IPos
    {

        static VivaWalletAppPos VivaWalletAppPos = new VivaWalletAppPos();

        public static VivaWalletPaymentTerminal VivaWalletPaymentTerminal { get; private set; }
        public static POSType TerminalType { get; private set; }

        public void Confing(POSType terminalType, string ipAddress = "", int port = 0, double waitTimeOutInSec = 0)
        {
            TerminalType=terminalType;

            if (terminalType==POSType.AppPOS)
                VivaWalletAppPos=new VivaWalletAppPos();
            else
            {
                VivaWalletPaymentTerminal = new VivaWalletPaymentTerminal(ipAddress, port, TimeSpan.FromSeconds(waitTimeOutInSec));
            }


        }

        public Task<PaymentData> ReceivePayment(decimal total, decimal tips)
        {
            if (TerminalType==POSType.AppPOS)
                return VivaWalletAppPos.ReceivePayment(total, tips);

            if (TerminalType==POSType.TerminalPos)
                return Task<PaymentData>.FromResult(VivaWalletPaymentTerminal.ReceivePayment(total, tips));
            throw new NotImplementedException();
        }
    }
}
