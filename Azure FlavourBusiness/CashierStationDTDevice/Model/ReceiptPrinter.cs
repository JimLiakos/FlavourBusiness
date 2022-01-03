using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationDevice.Model
{
    /// <MetaDataID>{234c682f-bbbb-4221-8484-e2b3f26bf040}</MetaDataID>
    [BackwardCompatibilityID("{234c682f-bbbb-4221-8484-e2b3f26bf040}")]
    [Persistent()]
    public class TransactionPrinter
    {
        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{c1330dbb-89bc-4bdd-a2dc-6d8c00261572}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+6")]
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
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        bool _IsDefault;

        /// <MetaDataID>{2f3555d1-8bcc-4cf5-9807-5c0a8407d191}</MetaDataID>
        [PersistentMember(nameof(_IsDefault))]
        [BackwardCompatibilityID("+1")]
        public bool IsDefault
        {
            get => _IsDefault;
            set
            {

                if (_IsDefault != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsDefault = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _Series;

        /// <MetaDataID>{77dcd376-d68d-4879-a242-9c83ef8c34d9}</MetaDataID>
        [PersistentMember(nameof(_Series))]
        [BackwardCompatibilityID("+2")]
        public string Series
        {
            get => _Series;
            set
            {

                if (_Series != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Series = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>\
        int _AutoNumber;

        /// <MetaDataID>{a4c383d4-a1a7-4e03-a1e7-7d387dd339fc}</MetaDataID>
        [PersistentMember(nameof(_AutoNumber))]
        [BackwardCompatibilityID("+3")]
        public int AutoNumber
        {
            get => _AutoNumber;
            set
            {

                if (_AutoNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AutoNumber = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _PrinterName;

        /// <MetaDataID>{6667d0aa-b038-403f-888c-a5fca430b2f7}</MetaDataID>
        [PersistentMember(nameof(_PrinterName))]
        [BackwardCompatibilityID("+4")]
        public string PrinterName
        {
            get => _PrinterName;
            set
            {

                if (_PrinterName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PrinterName = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _RawPrinterAddress;

        /// <MetaDataID>{969e1e7d-bd8d-4858-b40e-d94cb48e0a99}</MetaDataID>
        [PersistentMember(nameof(_RawPrinterAddress))]
        [BackwardCompatibilityID("+5")]
        public string RawPrinterAddress
        {
            get => _RawPrinterAddress;
            set
            {

                if (_RawPrinterAddress != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RawPrinterAddress = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        int _PrinterCodePage;

        /// <MetaDataID>{71a4d598-c3ed-471b-98d0-628999b049e1}</MetaDataID>
        [PersistentMember(nameof(_PrinterCodePage))]
        [BackwardCompatibilityID("+7")]
        public int PrinterCodePage
        {
            get => _PrinterCodePage; set
            {
                if (_PrinterCodePage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PrinterCodePage = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}
