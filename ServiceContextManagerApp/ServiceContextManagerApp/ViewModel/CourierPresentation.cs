using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FlavourBusinessManager.HumanResources;

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{50154c26-f1f3-479a-a182-3f3e0f14afbd}</MetaDataID>
    public class CourierPresentation : MarshalByRefObject, ICourierPresentation, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        private readonly ICourier Courier;
        private IShiftWork ActiveShiftWork;
        private readonly IFlavoursServicesContextRuntime ServicesContextRuntime;

        public string FullName
        {
            get
            {
                string fullName = Courier.FullName;
                if (!string.IsNullOrWhiteSpace(fullName))
                    return fullName;
                else
                    return Courier.Name;
            }

            set
            {
            }
        }

        public string WorkerIdentity { get => this.CourierIdentity; }
        public string UserName { get => Courier.UserName; set { } }
        public string Email { get => Courier.Email; set { } }
        public string PhotoUrl { get => Courier.PhotoUrl; set { } }

        public string CourierIdentity => Courier.Identity;

        public bool Suspended => false;

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
        /// <MetaDataID>{3a681272-0abd-4435-88ac-b528e651dce8}</MetaDataID>
        public CourierPresentation(ICourier courier, IFlavoursServicesContextRuntime servicesContextRuntime)
        {
            Courier = courier;
            ActiveShiftWork = courier.ActiveShiftWork;
            ServicesContextRuntime = servicesContextRuntime;
            NativeUser=courier.NativeUser;
        }
        public bool NativeUser { get; set; }
        public void ChangeSiftWork(DateTime startedAt, double timespanInHours)
        {
            ServicesContextRuntime.ChangeSiftWork(this.Courier.ActiveShiftWork, startedAt, timespanInHours);
        }

        public List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate)
        {

            return Courier.GetSifts(startDate, endDate);
        }

        public List<IServingShiftWork> GetLastThreeSifts()
        {
            return Courier.GetLastThreeSifts();
        }
    }
}
