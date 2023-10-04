using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessManager.HumanResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
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
            ActiveShiftWork = Waiter.ActiveShiftWork;
            ServicesContextRuntime = servicesContextRuntime;
            NativeUser=Waiter.NativeUser;

            var DASD = Waiter.FullName;
            var SDS = Waiter.UserName;
        }
        public bool NativeUser { get; set ; }

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

        public bool Suspended
        {
            get
            {
                return false;
                //return Waiter.Suspended;
            }
        }

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
        public string PhotoUrl { get => _PhotoUrl; set { } }

        IShiftWork ActiveShiftWork;
        private readonly IFlavoursServicesContextRuntime ServicesContextRuntime;

        public bool InActiveShiftWork
        {
            get
            {
                if (ActiveShiftWork != null)
                {
                    var startedAt = ActiveShiftWork.StartsAt;
                    var workingHours = ActiveShiftWork.PeriodInHours;

                    var hour = System.DateTime.UtcNow.Hour + (((double)System.DateTime.UtcNow.Minute) / 60);
                    hour = Math.Round((hour * 2)) / 2;
                    var utcNow = DateTime.Today + TimeSpan.FromHours(hour);
                    if (utcNow >= startedAt.ToUniversalTime() && utcNow <= startedAt.ToUniversalTime() + TimeSpan.FromHours(workingHours))
                    {
                        return true;
                    }
                    else
                    {
                        ActiveShiftWork = null;
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
                    return ActiveShiftWork.StartsAt;
                else
                    return DateTime.MinValue;
            }
        }

        public DateTime ActiveShiftWorkEndsAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ActiveShiftWork.StartsAt + TimeSpan.FromHours(ActiveShiftWork.PeriodInHours);
                else
                    return DateTime.MinValue;
            }
        }

        

        public void ChangeSiftWork(DateTime startedAt, double timespanInHours)
        {
            ServicesContextRuntime.ChangeSiftWork(this.Waiter.ActiveShiftWork, startedAt, timespanInHours);
        }
        public List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate)
        {

            return Waiter.GetSifts(startDate, endDate);
        }

        public List<IServingShiftWork> GetLastThreeSifts()
        {
            return Waiter.GetLastThreeSifts();
        }

    }
}
