using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{135c302f-bf11-4105-824d-ceb7f4d97077}</MetaDataID>
    [BackwardCompatibilityID("{135c302f-bf11-4105-824d-ceb7f4d97077}")]
    [Persistent()]
    internal class AuditCourierDelay : IAuditWorkerEvents
    {

        public AuditCourierDelay()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double _DelayInMinutes;

        /// <MetaDataID>{0661e403-5568-4362-a976-411f7f465f30}</MetaDataID>
        [PersistentMember(nameof(_DelayInMinutes))]
        [BackwardCompatibilityID("+4")]
        public double DelayInMinutes
        {
            get => _DelayInMinutes;
            set
            {
                if (_DelayInMinutes != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DelayInMinutes = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        DateTime _StartOfDelayTimeStamp;

        /// <MetaDataID>{7b7f4584-dc65-4f0a-a754-4b3a4686f072}</MetaDataID>
        [PersistentMember(nameof(_StartOfDelayTimeStamp))]
        [BackwardCompatibilityID("+3")]
        public DateTime StartOfDelayTimeStamp
        {
            get => _StartOfDelayTimeStamp;
            set
            {
                if (_StartOfDelayTimeStamp != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StartOfDelayTimeStamp = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{6749f5e0-1b9c-4978-99de-1f22fc56ce5d}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get => _Description;
            set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        DateTime _EventTimeStamp;

        /// <MetaDataID>{f81b5e1b-35a7-4f79-894a-f10ba87680b0}</MetaDataID>
        [PersistentMember(nameof(_EventTimeStamp))]
        [BackwardCompatibilityID("+2")]
        public System.DateTime EventTimeStamp
        {
            get => _EventTimeStamp;
            set
            {
                if (_EventTimeStamp != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _EventTimeStamp = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}
