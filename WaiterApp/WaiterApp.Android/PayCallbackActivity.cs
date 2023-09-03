using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaiterApp.Droid
{
    /// <MetaDataID>{abdc3a50-baa5-4888-a32e-adcd2c272844}</MetaDataID>
    [Activity(Name = "com.microneme.dontwaitwaiterapp.PayCallbackActivity", Label = "PayCallbackActivity", Exported = true)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataHost = "result", DataScheme = "waitercallbackscheme")]
    public class PayCallbackActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public PayCallbackActivity()
        {

        }
        /// <MetaDataID>{f91efe5a-d2de-4c9d-8af6-a9caf0cc4ea1}</MetaDataID>
        public static string PayResult;
        protected override void OnCreate(Bundle savedInstanceState)
        {   
            base.OnCreate(savedInstanceState);

            try
            {
                var result = this.Intent.Data;
                string resultStr = result.ToString();

                Finish();
                //"0mycallbackscheme://result?status=fail&message=(-4) USER_CANCEL&action=sale&clientTransactionId=&amount=1&tipAmount=0&verificationMethod= - &referenceNumber=0&tid=16145986&orderCode=0&shortOrderCode=0"
                var resutlParams = resultStr.Split('?')[1].Split('&');
                string messageParameter =  resutlParams.Where(x => x.IndexOf("message") == 0).FirstOrDefault();
                
                if (resultStr.Contains("status=fail"))
                    VivaWalletPos.Android.VivaWalletAppPos.SalesCompleted(false, resultStr);
                else
                    VivaWalletPos.Android.VivaWalletAppPos.SalesCompleted(true, resultStr);



                
            }
            catch (Exception error)
            {


            }
            //**Java.Lang.RuntimeException:** 'Unable to instantiate activity ComponentInfo{com.companyname.vivawallettest/com.companyname.vivawallettest.PayCallbackActivity}: java.lang.ClassNotFoundException:
            //Didn't find class "com.companyname.vivawallettest.PayCallbackActivity" on path: DexPathList[[zip file "/data/app/com.companyname.vivawallettest-0WxsJP5gCvVgiGqJD3ZeTg==/base.apk"],nativeLibraryDirectories=[/data/app/com.companyname.vivawallettest-0WxsJP5gCvVgiGqJD3ZeTg==/lib/arm64, /data/app/com.companyname.vivawallettest-0WxsJP5gCvVgiGqJD3ZeTg==/base.apk!/lib/arm64-v8a, /system/lib64, /system/product/lib64, /oppo_product/lib64]]'
            // Create your application here
        }
    }
}