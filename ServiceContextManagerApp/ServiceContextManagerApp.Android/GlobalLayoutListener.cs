using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

using Object = Java.Lang.Object;

namespace KeyboardService.Services.Keyboard
{
    internal class GlobalLayoutListener : Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private static InputMethodManager _inputManager;
        //private readonly SoftwareKeyboardService _softwareKeyboardService;

        private static void ObtainInputManager()
        {
            //_inputManager = (InputMethodManager)TinyIoCContainer.Current.Resolve<Activity>()
            //    .GetSystemService(Context.InputMethodService);
        }

        public GlobalLayoutListener()
        {
            _inputManager  = (InputMethodManager)Xamarin.Essentials.Platform.CurrentActivity.GetSystemService(Android.Content.Context.InputMethodService); ;
            //_softwareKeyboardService = softwareKeyboardService;
            //ObtainInputManager();
        }

        public void OnGlobalLayout()
        {
            if (_inputManager.Handle == IntPtr.Zero)
            {
                ObtainInputManager();
            }
            try
            {
                if (_inputManager.IsAcceptingText)
                {
                    //_softwareKeyboardService.InvokeKeyboardShow(new SoftwareKeyboardEventArgs(true));
                }
                else
                {
                    //_softwareKeyboardService.InvokeKeyboardHide(new SoftwareKeyboardEventArgs(false));
                }
            }
            catch (Exception error)
            {

                
            }
        }
    }
}