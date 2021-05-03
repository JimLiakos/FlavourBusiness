using System;
using System.Collections.Generic;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{1edf1ceb-e586-4722-9d12-6e9de1772451}</MetaDataID>
    [BackwardCompatibilityID("{1edf1ceb-e586-4722-9d12-6e9de1772451}")]
    [Persistent()]
    public class CallerIDServer : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ICallerIDServer
    {


        public event OOAdvantech.ObjectChangeStateHandle ObjectStateChanged;

        public CallerIDServer()
        {

        }

        /// <MetaDataID>{146983e6-5b90-45b7-bd45-621e5e41dc5f}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ICallerIDLine> _Lines = new OOAdvantech.Collections.Generic.Set<ICallerIDLine>();

        /// <MetaDataID>{84d349e5-ad3d-47a3-b87b-868048d5ed0b}</MetaDataID>
        [PersistentMember(nameof(_Lines))]
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        public IList<ICallerIDLine> Lines
        {
            get
            {
                return _Lines.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;

        /// <MetaDataID>{3d684b3c-39f0-4fbb-9b65-cf3feea03fb6}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+1")]
        public string ServicesContextIdentity
        {
            get
            {
                return _ServicesContextIdentity;
            }

            set
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

        /// <MetaDataID>{294c236a-27f8-4e74-b071-a750e0d9d8e5}</MetaDataID>
        public void AddCallerIDLine(ICallerIDLine callerIDLine)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Lines.Add(callerIDLine);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{20deea7d-b91e-4f2e-87cd-b34fc2d8ffdc}</MetaDataID>
        public void RemoveCallerIDLine(ICallerIDLine callerIDLine)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Lines.Remove(callerIDLine);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{45aea855-8044-4b8a-a478-4aee44fb2f65}</MetaDataID>
        public ICallerIDLine NewCallerIDLine()
        {

            ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(this);
            CallerIDLine callerIDLine = null;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                callerIDLine = new CallerIDLine();
                callerIDLine.LineDescription = "New phone line";
                objectStorage.CommitTransientObjectState(callerIDLine);
                _Lines.Add(callerIDLine);
                stateTransition.Consistent = true;
            }

            ObjectStateChanged?.Invoke(this, null);
            return callerIDLine;

        }
    }
}