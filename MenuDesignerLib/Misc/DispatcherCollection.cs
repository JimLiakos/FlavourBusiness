using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FLBManager.Misc
{
    /// <MetaDataID>{a66eae54-38ad-4c47-8663-f67aa2d017c3}</MetaDataID>
    class DispatcherCollection<T> : ObservableCollection<T>
    {
        public static ManualResetEvent Create(ref DispatcherCollection<T> var)
        {
            return Create(ref var, null);
        }

        public static ManualResetEvent Create(ref DispatcherCollection<T> var, ManualResetEvent resetEvent)
        {
            DispatcherCollection<T> collection = null;
            ManualResetEvent returnEvent = resetEvent ?? new ManualResetEvent(false);
            var dispatcher = Application.Current != null ? Application.Current.Dispatcher : null;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                collection = new DispatcherCollection<T>() { Dispatcher = dispatcher, DispatcherPriority = DispatcherPriority.Normal };
            }
            else
            {
                dispatcher.Invoke(() => Create(ref collection, returnEvent));
            }
            lock (typeof(DispatcherCollection<T>))
            {
                if (var == null)
                {
                    var = collection;
                    returnEvent.Set();
                }
            }
            return returnEvent;
        }

        public Dispatcher Dispatcher
        {
            get;
            private set;
        }

        public DispatcherPriority DispatcherPriority
        {
            get;
            set;
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Dispatcher != null && !Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke((Action)(() => base.OnCollectionChanged(e)), DispatcherPriority);
            }
            else
            {
                base.OnCollectionChanged(e);
            }
        }
    }

    /// <MetaDataID>{bf968279-9894-47dc-9c6e-92e3443cdeee}</MetaDataID>
    public static class DispatcherHelper
    {
        public static void Invoke(this Dispatcher dipatcher, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            dipatcher.Invoke(action, priority);
        }
    }

}
