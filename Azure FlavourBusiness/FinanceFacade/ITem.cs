using System;
using System.Collections.Generic;
using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

using System.Linq;
using OOAdvantech.Json;

namespace FinanceFacade
{

    /// <MetaDataID>{1efed191-267f-4beb-90d6-8ffe00bd2fa8}</MetaDataID>
    [BackwardCompatibilityID("{1efed191-267f-4beb-90d6-8ffe00bd2fa8}")]
    public interface IItem
    {
        /// <MetaDataID>{ac10223e-ee3c-4e25-b315-1269b70191b3}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        string QuantityDescription { get; set; }

        /// <MetaDataID>{8d545d0c-7b8c-4f0a-8501-e41cdfabf17b}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        string uid { get; set; }

        /// <MetaDataID>{50f519ce-2209-4ef9-827f-a2bd8c8cc5a2}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [Association("ItemTaxes", Roles.RoleA, "c9808e66-f681-4644-8ba2-98d00a7acd25")]
        [RoleBMultiplicityRange(0, 1)]
        IList<TaxAmount> Taxes { get; }


        /// <MetaDataID>{1cb77c22-fd7f-4f62-9583-255dff947245}</MetaDataID>
        void AddTax(TaxAmount taxAmount);

        /// <MetaDataID>{3168fc8d-dd3c-4fb0-8dfa-d642bb8fb596}</MetaDataID>
        void RemoveTax(TaxAmount taxAmount);

        //
        // Summary:
        //     /// Category type of the item. ///
        /// <MetaDataID>{b8f353b8-33d4-4fb3-9a63-c907e11d0845}</MetaDataID>
        [BackwardCompatibilityID("+1")]
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
        [BackwardCompatibilityID("+2")]
        string Description { get; set; }


        //
        // Summary:
        //     /// Item name. 127 characters max. ///
        /// <MetaDataID>{dfd8fa23-486f-4db4-9926-882aa8e32b55}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Name { get; set; }

        //
        // Summary:
        //     /// Item cost. 10 characters max. ///

        /// <MetaDataID>{ac726086-6b84-4c99-907d-9a047a53094b}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        decimal Price { get; set; }
        //
        // Summary:
        //     /// Number of a particular item. 10 characters max. ///

        /// <MetaDataID>{26e6f527-6ad6-44a8-ac17-1b5923504fc2}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        decimal Quantity { get; set; }
        //
        // Summary:
        //     /// Stock keeping unit corresponding (SKU) to item. ///

        /// <MetaDataID>{83f35f0f-2421-4d4a-b93f-f56a31e35085}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        string Sku { get; set; }



    }


    /// <MetaDataID>{f96ee9e1-2527-443f-b2b1-42414f6affbf}</MetaDataID>
    [BackwardCompatibilityID("{f96ee9e1-2527-443f-b2b1-42414f6affbf}")]
    [Persistent()]
    public class Item : IItem
    {
        /// <exclude>Excluded</exclude>
        string _QuantityDescription;
        /// <MetaDataID>{8e495f37-bad8-481b-a8f2-fd4be507d26d}</MetaDataID>
        [PersistentMember(nameof(_QuantityDescription))]
        [BackwardCompatibilityID("+8")]
        public string QuantityDescription
        {
            get => _QuantityDescription;
            set
            {

                if (_QuantityDescription != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _QuantityDescription = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <MetaDataID>{4d913c21-f612-428e-8d2b-de79963c6329}</MetaDataID>
        public Item()
        {
        }

        /// <MetaDataID>{5e431ad4-251b-4434-9021-d89f37d5e5ca}</MetaDataID>
        [JsonConstructor]
        public Item(string category, string description, string name, decimal price, decimal quantity, string sku, System.Collections.Generic.List<FinanceFacade.TaxAmount> taxes, string uid, string quantityDescription)
        {
            _Category = category;
            _Description = description;
            _Name = name;
            _Price = price;
            _Quantity = quantity;
            _Sku = sku;
            _Taxes = new Set<TaxAmount>(taxes);
            _QuantityDescription = quantityDescription;
            this.uid = uid;

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

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



        /// <MetaDataID>{a98c3d71-b06b-497a-9fa5-941aade21e83}</MetaDataID>
        public void AddTax(TaxAmount taxAmount)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Taxes.Add(taxAmount);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{6c8832cb-4dcc-4cbb-bf67-852e0b90c7c9}</MetaDataID>
        public void RemoveTax(TaxAmount taxAmount)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Taxes.Remove(taxAmount);
                stateTransition.Consistent = true;
            }

        }

        /// <exclude>Excluded</exclude>
        Set<TaxAmount> _Taxes = new Set<TaxAmount>();

        /// <MetaDataID>{74c47746-adec-4859-91bf-07340565354e}</MetaDataID>

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

        /// <MetaDataID>{55f5b0bc-8d8f-42af-b1b6-5b1c60383ef0}</MetaDataID>
        public decimal Amount { get => Quantity * Price; }

        /// <MetaDataID>{38cd5338-a098-41bd-a30a-80ad5a068e6e}</MetaDataID>
        public string uid { get; set; }


    }

}