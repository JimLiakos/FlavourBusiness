using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ServiceContextManagerApp
{



    /// <MetaDataID>{09137653-bc72-436a-9eed-a74b54848058}</MetaDataID>
    public class TakeawayCashierPresentation : MarshalByRefObject, INotifyPropertyChanged, ITakeawayCashierPresentation, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{f427a271-25e9-45e1-b9b7-1561c560e3fe}</MetaDataID>
        internal ITakeawayCashier TakeawayCashier;
        /// <MetaDataID>{7d0b6644-4e8c-4bb6-b24b-37001826fa64}</MetaDataID>
        public TakeawayCashierPresentation(ITakeawayCashier takeawayCashier, IFlavoursServicesContextRuntime servicesContextRuntime)
        {
            TakeawayCashier = takeawayCashier;
            ActiveShiftWork = TakeawayCashier.ActiveShiftWork;
            ServicesContextRuntime = servicesContextRuntime;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{edf5cf93-b741-4640-9ff3-f28ff364d91f}</MetaDataID>
        public string TakeawayCashierIdentity
        {
            get
            {
                return TakeawayCashier.Identity;
            }
        }

        /// <MetaDataID>{ce464688-6c62-4ebf-8a45-9bb5939f60ca}</MetaDataID>
        public bool Suspended
        {
            get
            {
                return false;
                //return Waiter.Suspended;
            }
        }

        /// <MetaDataID>{d1060873-5f43-486c-a68d-1d7eb9189f07}</MetaDataID>
        public string Email
        {
            get
            {
                return TakeawayCashier.Email;
            }

            set
            {

            }
        }


        /// <MetaDataID>{f90c4776-0704-4031-8bbc-4eca429dd42b}</MetaDataID>
        public string FullName
        {
            get
            {
                return TakeawayCashier.Name;
            }

            set
            {
            }
        }



        /// <MetaDataID>{c665d38b-2ec0-4cf1-8bd7-a09d7c50dda5}</MetaDataID>
        public string UserName
        {
            get
            {
                return TakeawayCashier.UserName;
            }
            set
            {

            }
        }

        /// <exclude>Excluded</exclude>
        string _PhotoUrl;
        /// <MetaDataID>{87011970-7d69-4528-bce5-ef00c5cfec4e}</MetaDataID>
        public string PhotoUrl { get => _PhotoUrl; set { } }

        /// <MetaDataID>{cbe2c3df-1c68-429e-8d39-a32933a5d121}</MetaDataID>
        IShiftWork ActiveShiftWork;
        /// <MetaDataID>{d5bd6cd8-d051-4d3c-9f64-c15125131abf}</MetaDataID>
        private readonly IFlavoursServicesContextRuntime ServicesContextRuntime;

        /// <MetaDataID>{5a8fd88c-3f92-438e-868c-6bc8c54e7b68}</MetaDataID>
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

        /// <MetaDataID>{82e74749-2679-4240-924d-fa27e77c3d96}</MetaDataID>
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

        /// <MetaDataID>{64241734-b6bf-4297-a578-edc4d7baa0be}</MetaDataID>
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

        /// <MetaDataID>{b06f0ab8-a3fa-472f-9dc4-be98d4ec4e4b}</MetaDataID>
        public void ChangeSiftWork(DateTime startedAt, double timespanInHours)
        {
            ServicesContextRuntime.ChangeSiftWork(this.TakeawayCashier.ActiveShiftWork, startedAt, timespanInHours);
        }
        /// <MetaDataID>{d474eeea-d67c-469e-b1f2-d77d85a51cbe}</MetaDataID>
        public List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate)
        {

            return TakeawayCashier.GetSifts(startDate, endDate);
        }

        /// <MetaDataID>{e4a3cea5-73d0-4ca7-a5ed-2718ab6eb197}</MetaDataID>
        public List<IServingShiftWork> GetLastThreeSifts()
        {
            return TakeawayCashier.GetLastThreeSifts();
        }

    }
}