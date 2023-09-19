using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CourierApp.ViewModel
{
    
    public interface ICourierActivityPresentation
    {
        /// <summary>
        /// Request Permission to access infrastructure for QR code scanning 
        /// </summary>
        /// <returns>
        /// for granted  return true
        /// else return false
        /// </returns>
        Task<bool> RequestPermissionsForQRCodeScan();


        
        bool InActiveShiftWork { get; }

        
        System.DateTime ActiveShiftWorkStartedAt { get; }

        
        System.DateTime ActiveShiftWorkEndsAt { get; }
    }
}
