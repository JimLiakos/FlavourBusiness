using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Windows.Controls;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using FlavourBusinessManager.HumanResources;
using MarshalByRefObject = System.MarshalByRefObject;
#endif

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{b9add05d-9580-43ff-a30d-a91d44014079}</MetaDataID>
    public class WaiterPresentation : MarshalByRefObject, INotifyPropertyChanged, IWaiterPresentation, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        internal IWaiter Waiter;
        public WaiterPresentation(IWaiter waiter, IFlavoursServicesContextRuntime servicesContextRuntime)
        {
            Waiter = waiter;
            ShiftWork = Waiter.ShiftWork;
            ServicesContextRuntime = servicesContextRuntime;
            NativeUser = Waiter.NativeUser;

        }
        public bool NativeUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string WaiterIdentity
        {
            get
            {
                string rr = Waiter.Identity;
                return Waiter.Identity;
            }
        }

        public string WorkerIdentity { get => WaiterIdentity; }

        [CachingDataOnClientSide]
        public bool Suspended
        {
            get
            {
                return false;
                //return Waiter.Suspended;
            }
        }

        [CachingDataOnClientSide]
        public string Email
        {
            get
            {
                return Waiter.Email;
            }

            set
            {

            }
        }

        public IServicesContextWorker ServicesContextWorker => Waiter;

        [CachingDataOnClientSide]
        public string FullName
        {
            get
            {
                return Waiter.Name;
            }

            set
            {
            }
        }


        [CachingDataOnClientSide]
        public string UserName
        {
            get
            {
                return Waiter.UserName;
            }
            set
            {

            }
        }

        /// <exclude>Excluded</exclude>
        string _PhotoUrl;
        [CachingDataOnClientSide]
        public string PhotoUrl { get => _PhotoUrl; set { } }


        public IShiftWork ActiveShiftWork
        {
            get
            {
                if (ShiftWork?.IsActive() == true)
                    return ShiftWork;
                else
                    return null;
            }
        }

        public IShiftWork ShiftWork { get; set; }

        private readonly IFlavoursServicesContextRuntime ServicesContextRuntime;

        public bool InActiveShiftWork
        {
            get
            {
                if (ShiftWork != null)
                {
                    var startedAt = ShiftWork.StartsAt;
                    var workingHours = ShiftWork.PeriodInHours;

                    var hour = System.DateTime.UtcNow.Hour + (((double)System.DateTime.UtcNow.Minute) / 60);
                    hour = Math.Round((hour * 2)) / 2;
                    var utcNow = DateTime.Today + TimeSpan.FromHours(hour);
                    if (utcNow >= startedAt.ToUniversalTime() && utcNow <= startedAt.ToUniversalTime() + TimeSpan.FromHours(workingHours))
                    {
                        return true;
                    }
                    else
                    {
                        ShiftWork = null;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public DateTime ActiveShiftWorkStartedAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ShiftWork.StartsAt;
                else
                    return DateTime.MinValue;
            }
        }

        public DateTime ActiveShiftWorkEndsAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ShiftWork.StartsAt + TimeSpan.FromHours(ShiftWork.PeriodInHours);
                else
                    return DateTime.MinValue;
            }
        }



        public void ChangeSiftWork(DateTime startedAt, double timespanInHours)
        {
            ServicesContextRuntime.ChangeSiftWork(this.Waiter.ShiftWork, startedAt, timespanInHours);
        }
        public List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate)
        {

            return Waiter.GetSifts(startDate, endDate);
        }

        public List<IServingShiftWork> GetLastThreeSifts()
        {
            return Waiter.GetLastThreeSifts();
        }

        public void NewShiftWork(DateTime startedAt, double timespanInHours)
        {
            ShiftWork = ServicesContextWorker.NewShiftWork(startedAt, timespanInHours);
        }
    }
}
