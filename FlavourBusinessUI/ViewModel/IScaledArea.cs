using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessUI.ViewModel
{
    /// <MetaDataID>{ff6c0b23-b555-4883-975f-a0ae67fca56f}</MetaDataID>
    public interface IScaledArea
    {
        double ZoomPercentage { get; set; }
        string ZoomPercentageLabel { get; }
    }
}
