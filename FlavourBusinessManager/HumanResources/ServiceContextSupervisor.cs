using System;
using System.Collections.Generic;
using System.Linq;
using FlavourBusinessFacade.HumanResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{5b713668-2a33-4008-b07d-09f02c6eaf9e}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{5b713668-2a33-4008-b07d-09f02c6eaf9e}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public class ServiceContextSupervisor : MarshalByRefObject, IServiceContextSupervisor, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IAccountability> _Responsibilities = new OOAdvantech.Collections.Generic.Set<IAccountability>();

        /// <MetaDataID>{ecace4fc-1944-4bfc-8904-99b28a462fe6}</MetaDataID>
        [PersistentMember(nameof(_Responsibilities))]
        [BackwardCompatibilityID("+15")]
        public List<IAccountability> Responsibilities => _Responsibilities.ToThreadSafeList();

        ///// <exclude>Excluded</exclude>
        //OOAdvantech.Collections.Generic.Set<IActivity> _Activities = new OOAdvantech.Collections.Generic.Set<IActivity>();

        ///// <MetaDataID>{dcbf148f-a539-4fdf-9cfc-57fa4a043758}</MetaDataID>
        //[PersistentMember(nameof(_Activities))]
        //[BackwardCompatibilityID("+14")]
        //public List<IActivity> Activities => _Activities.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        string _PhotoUrl;

        /// <MetaDataID>{4b207892-7520-4166-80f5-ab3971b72312}</MetaDataID>
        [PersistentMember(nameof(_PhotoUrl))]
        [BackwardCompatibilityID("+13")]
        public string PhotoUrl
        {
            get => _PhotoUrl;
            set
            {
                if (_PhotoUrl != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhotoUrl = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Suspended;

        /// <MetaDataID>{baf9f201-3f15-4863-a82b-edf75aeda0c2}</MetaDataID>
        [PersistentMember(nameof(_Suspended))]
        [BackwardCompatibilityID("+11")]
        public bool Suspended
        {
            get
            {
                return _Suspended;
            }
            set
            {
                if (_Suspended != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Suspended = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> _Roles;
        /// <MetaDataID>{7d76965f-4c9c-4f0b-84a5-7da4a769c3cd}</MetaDataID>
        public System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> Roles
        {
            get
            {
                if (_Roles == null)
                {
                    FlavourBusinessFacade.UserData.UserRole role = new FlavourBusinessFacade.UserData.UserRole() { User = this, RoleType = FlavourBusinessFacade.UserData.UserRole.GetRoleType(GetType().FullName) };
                    _Roles = new List<FlavourBusinessFacade.UserData.UserRole>() { role };
                }
                return _Roles;
            }
        }


        /// <exclude>Excluded</exclude>
        object objectLock = new object();

        /// <exclude>Excluded</exclude>
        string _FlavoursServiceContextDescription;
        /// <MetaDataID>{4dde3797-84f0-441d-a11e-1bc6a5988ebf}</MetaDataID>
        public string FlavoursServiceContextDescription
        {
            get
            {
                lock (objectLock)
                {
                    if (_FlavoursServiceContextDescription == null)
                    {
                        var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                        if (objectStorage != null)
                        {
                            OOAdvantech.Linq.Storage linqStorage = new OOAdvantech.Linq.Storage(objectStorage);
                            var flavoursServicesContextRuntime = (from serviceContext in linqStorage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
                                                                  select serviceContext).FirstOrDefault();

                            if (flavoursServicesContextRuntime != null)
                                _FlavoursServiceContextDescription = flavoursServicesContextRuntime.Description;

                        }
                    }
                    return _FlavoursServiceContextDescription;
                }

            }
        }

        /// <exclude>Excluded</exclude>
        ServicePointRunTime.ServicesContextRunTime _ServicesContextRunTime;
        /// <MetaDataID>{14ce2c73-3d77-4fbf-8e09-382273ad9380}</MetaDataID>
        public ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime
        {
            get
            {
                if (_ServicesContextRunTime == null)
                {
                    var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    if (objectStorage != null)
                    {
                        OOAdvantech.Linq.Storage linqStorage = new OOAdvantech.Linq.Storage(objectStorage);
                        _ServicesContextRunTime = (from serviceContext in linqStorage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
                                                   select serviceContext).FirstOrDefault();
                    }
                }
                return _ServicesContextRunTime;
            }
        }

        /// <exclude>Excluded</exclude>
        string _SupervisorAssignKey;

        /// <MetaDataID>{da2f05f0-c1f2-4848-b589-26609995340d}</MetaDataID>
        [PersistentMember(nameof(_SupervisorAssignKey))]
        [BackwardCompatibilityID("+10")]
        public string SupervisorAssignKey
        {
            get => _SupervisorAssignKey;
            set
            {

                if (_SupervisorAssignKey != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SupervisorAssignKey = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{c7071f68-4f6b-4387-baa6-4174603351b2}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+5")]
        public string Identity
        {
            get
            {
                if (_Identity == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Identity = Guid.NewGuid().ToString("N");
                        stateTransition.Consistent = true;
                    }
                }
                return _Identity;
            }
        }

        /// <exclude>Excluded</exclude>
        string _Email;

        /// <MetaDataID>{78ae1470-1c8e-4501-a902-b3d4a2974039}</MetaDataID>
        [PersistentMember(nameof(_Email))]
        [BackwardCompatibilityID("+7")]
        public string Email
        {
            get => _Email;
            set
            {
                if (_Email != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Email = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{689a286d-17af-4963-9400-47139cf35c73}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        public string FullName
        {
            get => Name;
            set
            {
                Name = value;
            }
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{f0a9d825-ce12-422e-bad3-b968c165a5c6}</MetaDataID>
        string _UserName;

        /// <MetaDataID>{21770742-0679-4dba-916b-befe234f8f3a}</MetaDataID>
        [PersistentMember(nameof(_UserName))]
        [BackwardCompatibilityID("+8")]
        public string UserName
        {
            get => _UserName;
            set
            {
                if (_UserName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1a6a3f08-9bdf-49a7-b94e-af59321dac71}</MetaDataID>
        string _PhoneNumber;

        /// <MetaDataID>{a09e9d44-39ff-4f12-be05-44ea59b57623}</MetaDataID>
        [PersistentMember(nameof(_PhoneNumber))]
        [BackwardCompatibilityID("+6")]
        public string PhoneNumber
        {
            get => _PhoneNumber;
            set
            {
                if (_PhoneNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhoneNumber = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _SupervisorIdentity;

        /// <MetaDataID>{27e771fe-6707-4c7c-8691-3e7f2bb2b93f}</MetaDataID>
        [PersistentMember(nameof(_SupervisorIdentity))]
        [BackwardCompatibilityID("+4")]
        public string SupervisorIdentity
        {
            get
            {

                if (_SupervisorIdentity == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SupervisorIdentity = Guid.NewGuid().ToString("N");
                        stateTransition.Consistent = true;
                    }
                }
                return _SupervisorIdentity;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _SignUpUserIdentity;

        /// <MetaDataID>{1726a5c8-bb41-4e24-a782-51d9a2154645}</MetaDataID>
        [PersistentMember(nameof(_SignUpUserIdentity))]
        [BackwardCompatibilityID("+1")]
        public string SignUpUserIdentity
        {
            get => _SignUpUserIdentity;
            set
            {
                if (_SignUpUserIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SignUpUserIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;

        /// <MetaDataID>{edeff6e3-d631-45ad-a6be-844eb7d051b6}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+2")]
        public string ServicesContextIdentity
        {
            get => _ServicesContextIdentity; set
            {

                if (_ServicesContextIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{6fc67fe2-2205-4c6c-b145-40a9d6577078}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+3")]
        public string Name
        {
            get => _Name;
            set
            {

                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IShiftWork> _ShiftWorks = new OOAdvantech.Collections.Generic.Set<IShiftWork>();

        public event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{7b7ffc20-6b0a-4d53-98bc-e24665c9e4fd}</MetaDataID>
        [PersistentMember(nameof(_ShiftWorks))]
        [BackwardCompatibilityID("+12")]
        public System.Collections.Generic.IList<FlavourBusinessFacade.HumanResources.IShiftWork> ShiftWorks => _ShiftWorks.ToThreadSafeList();

        /// <MetaDataID>{d5055dba-e0e3-4e02-96b4-7d38e7182a2e}</MetaDataID>
        public IShiftWork ActiveShiftWork
        {
            get
            {
                return null;
            }

        }

        /// <MetaDataID>{83f015dc-7ec7-4910-9574-de82d9545def}</MetaDataID>
        public List<IAccountability> Commissions => new List<IAccountability>();



        /// <MetaDataID>{9edad4d3-658b-46b1-b372-49f859fbd7b9}</MetaDataID>
        public void RemoveShiftWork(IShiftWork shiftWork)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ShiftWorks.Remove(shiftWork);
                stateTransition.Consistent = true;
            }

        }

        internal void AddShiftWork(ShiftWork shiftWork)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ShiftWorks.Add(shiftWork);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{bbc190ff-d979-456d-af43-5dff0a41cb54}</MetaDataID>
        public IShiftWork NewShiftWork(DateTime startedAt, double timespanInHours)
        {
            ShiftWork shiftWork = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {
                shiftWork = new ShiftWork(Name);
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(shiftWork);
                shiftWork.StartsAt = startedAt;
                shiftWork.PeriodInHours = timespanInHours;
                AddShiftWork(shiftWork);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));
            return shiftWork;
        }

        /// <MetaDataID>{c434b17c-ae50-4c82-9466-428980ae6adb}</MetaDataID>
        public void ChangeSiftWork(IShiftWork shiftWork, DateTime startedAt, double timespanInHours)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                (shiftWork as ShiftWork).StartsAt = startedAt;
                (shiftWork as ShiftWork).PeriodInHours = timespanInHours;
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));
        }
    }
}