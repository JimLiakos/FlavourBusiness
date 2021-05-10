using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
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
    public class SupervisorPresentation : MarshalByRefObject, INotifyPropertyChanged, ISupervisorPresentation, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
       internal IServiceContextSupervisor Supervisor;
        private readonly IFlavoursServicesContextRuntime ServicesContextRuntime;

        public SupervisorPresentation(IServiceContextSupervisor supervisor, IFlavoursServicesContextRuntime servicesContextRuntime)
        {
            Supervisor = supervisor;
            ServicesContextRuntime = servicesContextRuntime;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public string SupervisorIdentity { 
            get
            {
                return Supervisor.Identity;
            }
        }

        public bool Suspended
        {
            get
            {
                return Supervisor.Suspended;
            }
        }

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
        public string PhotoUrl { get => _PhotoUrl; set { } }
    }
}
