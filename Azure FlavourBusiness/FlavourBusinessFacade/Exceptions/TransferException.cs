using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade.Exceptions
{

    /// <MetaDataID>{a4f2bdb6-04f6-47cc-9045-4a2686761f4b}</MetaDataID>
    public class TransferException : System.Exception
    {
        public TransferException(string message, int hResult) : base(message)
        {
            HResult = hResult;
        }

    }
}
