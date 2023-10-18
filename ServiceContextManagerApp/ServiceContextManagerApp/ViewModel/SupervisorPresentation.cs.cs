using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;

using OOAdvantech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
using FlavourBusinessManager.HumanResources;
#endif

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{7e25de00-a04d-4177-ad82-00eeb29302e2}</MetaDataID>
    public class SupervisorPresentation : MarshalByRefObject, INotifyPropertyChanged, ISupervisorPresentation, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{9d98c0c2-b07a-4d3b-a8d0-22e63b8320b2}</MetaDataID>
        internal IServiceContextSupervisor Supervisor;
        /// <MetaDataID>{8b9db1ac-b7df-434d-ba26-b99e59da70d4}</MetaDataID>
        private readonly IFlavoursServicesContextRuntime ServicesContextRuntime;

        /// <MetaDataID>{fa571c81-5ed1-47a4-a69a-cbdac0ff2c8c}</MetaDataID>
        public SupervisorPresentation(IServiceContextSupervisor supervisor, IFlavoursServicesContextRuntime servicesContextRuntime)
        {
            Supervisor = supervisor;
            ServicesContextRuntime = servicesContextRuntime;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{27833946-9877-4976-8922-2f22d69cd7a7}</MetaDataID>
        public string SupervisorIdentity
        {
            get
            {
                return Supervisor.Identity;
            }
        }

        /// <MetaDataID>{94888f42-2b9f-4970-acc6-43953d6dffbf}</MetaDataID>
        public bool Suspended
        {
            get
            {
                return Supervisor.Suspended;
            }
        }

        /// <MetaDataID>{87b82250-53e6-4008-8743-6aeecee51ebd}</MetaDataID>
        public string Email
        {
            get
            {
                return Supervisor.Email;
            }

            set
            {

            }
        }


        /// <MetaDataID>{280698f9-92ac-48d8-a7d1-8e9502fc3277}</MetaDataID>
        public string FullName
        {
            get
            {
                return Supervisor.FullName;
            }

            set
            {
            }
        }



        /// <MetaDataID>{ff92134f-0d3e-46d3-bf57-d65e3f583bcc}</MetaDataID>
        public string UserName
        {
            get
            {
                return Supervisor.UserName;
            }
            set
            {

            }
        }

        /// <exclude>Excluded</exclude>
        string _PhotoUrl;
        /// <MetaDataID>{9548972f-d549-4409-a57a-e0133ae3ea7a}</MetaDataID>
        public string PhotoUrl { get => _PhotoUrl; set { } }

        IShiftWork ActiveShiftWork;

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
        public void GetActiveShiftWork()
        {
            ActiveShiftWork = Supervisor.ActiveShiftWork;
        }
        public async void ShiftWorkStart(DateTime startedAt, double timespanInHours)
        {
            ActiveShiftWork = Supervisor.NewShiftWork(startedAt, timespanInHours);

            if (ActiveShiftWork != null)
            {
             //   UpdateFoodShippings(Courier.GetFoodShippings());

                //IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(IDeviceOOAdvantechCore)) as IDeviceOOAdvantechCore;
                //_TakeAwaySession = await FlavoursOrderServer.GetFoodServicesClientSessionViewModel(TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));
                ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));
            }


        }
        public void ExtendShiftWorkStart(double timespanInHours)
        {
            throw new NotImplementedException();
        }
        public event ObjectChangeStateHandle ObjectChangeState;
    }
}
