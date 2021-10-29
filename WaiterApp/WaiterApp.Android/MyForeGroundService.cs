using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DestoPesto.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaiterApp.Droid
{
    /// <MetaDataID>{52453757-2621-49c4-a293-43e18ae14e6a}</MetaDataID>
  
    [Service(Name = "com.microneme.dontwaitwaiterapp.MyForeGroundService")]
    public class MyForeGroundService : ForeGroundService
    {
    }
}
