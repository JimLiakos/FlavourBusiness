using System;
using System.Collections.Generic;
using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

using System.Linq;

namespace FinanceFacade
{

    /// <MetaDataID>{1efed191-267f-4beb-90d6-8ffe00bd2fa8}</MetaDataID>
    public interface IItem
    {
        /// <MetaDataID>{50f519ce-2209-4ef9-827f-a2bd8c8cc5a2}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [Association("ItemTaxes", Roles.RoleA, "c9808e66-f681-4644-8ba2-98d00a7acd25")]
        [RoleBMultiplicityRange(0, 1)]
        IList<TaxAmount> Taxes { get; }


        void AddTax(TaxAmount taxAmount);

        void RemoveTax(TaxAmount taxAmount);

        //
        // Summary:
        //     /// Category type of the item. ///
        /// <MetaDataID>{b8f353b8-33d4-4fb3-9a63-c907e11d0845}</MetaDataID>
        string Category { get; set; }
        //
        // Summary:
        //     /// 3-letter [currency code](https://developer.paypal.com/docs/integration/direct/rest_api_payment_country_currency_support/).
        //     ///

        //
        // Summary:
        //     /// Description of the item. Only supported when the `payment_method` is set
        //     to `paypal`. ///
        /// <MetaDataID>{63d34d84-fbf8-49b7-a68c-ad8da36023ae}</MetaDataID>
        string Description { get; set; }


        //
        // Summary:
        //     /// Item name. 127 characters max. ///
        /// <MetaDataID>{dfd8fa23-486f-4db4-9926-882aa8e32b55}</MetaDataID>
        string Name { get; set; }

        //
        // Summary:
        //     /// Item cost. 10 characters max. ///

        /// <MetaDataID>{ac726086-6b84-4c99-907d-9a047a53094b}</MetaDataID>
        decimal Price { get; set; }
        //
        // Summary:
        //     /// Number of a particular item. 10 characters max. ///

        /// <MetaDataID>{26e6f527-6ad6-44a8-ac17-1b5923504fc2}</MetaDataID>
        decimal Quantity { get; set; }
        //
        // Summary:
        //     /// Stock keeping unit corresponding (SKU) to item. ///

        /// <MetaDataID>{83f35f0f-2421-4d4a-b93f-f56a31e35085}</MetaDataID>
        string Sku { get; set; }



    }


    /// <MetaDataID>{f96ee9e1-2527-443f-b2b1-42414f6affbf}</MetaDataID>
    [BackwardCompatibilityID("{f96ee9e1-2527-443f-b2b1-42414f6affbf}")]
    [Persistent()]
    public class Item : IItem
    {


        /// <exclude>Excluded</exclude>
        string _Category;

        /// <MetaDataID>{654bddcb-fc56-47dc-bb82-ad3e5ffd22c5}</MetaDataID>
        [PersistentMember(nameof(_Category))]
        [BackwardCompatibilityID("+1")]
        public string Category
        {
            get
            {
                return _Category;
            }

            set
            {

                if (_Category != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Category = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{3acb114e-f97d-4a1a-894f-e6476b732c74}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+3")]
        public string Description
        {
            get
            {
                return _Description;
            }

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
        string _Name;

        /// <MetaDataID>{90fa7c85-613e-4e19-976d-cb96e5118f59}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+4")]
        public string Name
        {
            get
            {
                return _Name;
            }

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
        decimal _Price;

        /// <MetaDataID>{b69f4281-d067-46fa-82e2-6a725b637e9a}</MetaDataID>
        [PersistentMember(nameof(_Price))]
        [BackwardCompatibilityID("+5")]
        public decimal Price
        {
            get
            {
                return _Price;
            }

            set
            {

                if (_Price != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Price = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        decimal _Quantity;

        /// <MetaDataID>{eca6dce8-f5b7-40d2-b7c0-cb4af60ee163}</MetaDataID>
        [PersistentMember(nameof(_Quantity))]
        [BackwardCompatibilityID("+6")]
        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }

            set
            {

                if (_Quantity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Quantity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _Sku;

        /// <MetaDataID>{315499a2-6cc2-4a8e-a122-71a2965b503e}</MetaDataID>
        [PersistentMember(nameof(_Sku))]
        [BackwardCompatibilityID("+7")]
        public string Sku
        {
            get
            {
                return _Sku;
            }

            set
            {

                if (_Sku != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Sku = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
        }



        public void AddTax(TaxAmount taxAmount)
        {
            _Taxes.Add(taxAmount);
        }

        public void RemoveTax(TaxAmount taxAmount)
        {
            _Taxes.Remove(taxAmount);
        }

        /// <exclude>Excluded</exclude>
        Set<TaxAmount> _Taxes = new Set<TaxAmount>();

        /// <MetaDataID>{74c47746-adec-4859-91bf-07340565354e}</MetaDataID>
        [PersistentMember(nameof(_Taxes))]
        [BackwardCompatibilityID("+2")]
        public System.Collections.Generic.IList<FinanceFacade.TaxAmount> Taxes
        {
            get
            {
                lock (_Taxes)
                {
                    return _Taxes.ToList();
                }
            }
        }
    }

}