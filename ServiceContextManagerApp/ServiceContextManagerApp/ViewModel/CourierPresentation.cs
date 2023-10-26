using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{50154c26-f1f3-479a-a182-3f3e0f14afbd}</MetaDataID>
    public class CourierPresentation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ICourierPresentation
    {
        /// <MetaDataID>{ad596b08-29b3-41b8-9780-bc7a1384aba0}</MetaDataID>
        private readonly ICourier Courier;
        /// <MetaDataID>{4413f7cd-0cc9-4671-8ac4-a845e032b019}</MetaDataID>
        public IShiftWork ActiveShiftWork {get; set;}
        /// <MetaDataID>{3a590f53-8ab7-468d-9450-8704f1ce21e1}</MetaDataID>
        private readonly IFlavoursServicesContextRuntime ServicesContextRuntime;

        /// <MetaDataID>{7e702328-8200-4589-a10f-140dd3fa5d67}</MetaDataID>
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
        /// <MetaDataID>{ed83a4f4-f975-4531-b420-1b6f51ee8ce7}</MetaDataID>
        public string UserName { get => Courier.UserName; set { } }
        /// <MetaDataID>{6e0fbcbf-c5b8-4145-923f-306593b3fcb6}</MetaDataID>
        public string Email { get => Courier.Email; set { } }
        /// <MetaDataID>{b95d622c-c188-4dcb-9e21-4a72825235a7}</MetaDataID>
        public string PhotoUrl { get => Courier.PhotoUrl; set { } }

        /// <MetaDataID>{1b0b930c-6cc5-448a-abef-3b37005684e6}</MetaDataID>
        public string CourierIdentity => Courier.Identity;

        /// <MetaDataID>{4899cf09-f060-4597-9cdf-abc439b8f407}</MetaDataID>
        public bool Suspended => false;

        /// <MetaDataID>{e7461043-e364-4c73-b81b-978a1a3b3d0d}</MetaDataID>
        public bool InActiveShiftWork
        {
        get
            {
                if (ActiveShiftWork?.IsActive() == true)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{b5fc373c-201a-4f94-a151-0ceeb7bc5eaa}</MetaDataID>
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
        /// <MetaDataID>{51e49d6b-25a0-4c60-80de-ffe883f33a21}</MetaDataID>
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

        public IServicesContextWorker ServicesContextWorker => Courier;

        /// <MetaDataID>{a61fa943-0074-4c3a-b3e4-ad41903d62dd}</MetaDataID>
        public void ChangeSiftWork(DateTime startedAt, double timespanInHours)
        {
            ServicesContextRuntime.ChangeSiftWork(this.Courier.ActiveShiftWork, startedAt, timespanInHours);
        }

        /// <MetaDataID>{fa9fcab6-6710-4cf3-8ae4-425af19b6142}</MetaDataID>
        public List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate)
        {

            return Courier.GetSifts(startDate, endDate);
        }

        /// <MetaDataID>{908c1264-0286-45d7-8541-bbce3db08973}</MetaDataID>
        public List<IServingShiftWork> GetLastThreeSifts()
        {
            return Courier.GetLastThreeSifts();
        }

   
    }
}
