using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{3836664e-5f1a-4e75-9bbb-4c9d6f963fe0}</MetaDataID>
    [HttpVisible]
    public interface IPayment
    {
        [Association("PaymentItem", Roles.RoleA, "d4adad7e-5b24-4d4c-9785-07c27b196a3f")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        System.Collections.Generic.List<IItem> Items { get; }



        /// <MetaDataID>{45d235f7-ea8f-476a-a5b7-93112d49437f}</MetaDataID>
        [CachingDataOnClientSide]
        decimal Amount { get; set; }


        /// <MetaDataID>{3df25632-5ef6-420a-95bf-a342f7e3588a}</MetaDataID>
        [CachingDataOnClientSide]
        PaymentType PaymentType { get; set; }

        /// <summary>
        /// Defines ISO Currency Symbol 
        /// </summary>
        /// <MetaDataID>{c25d9c8d-f279-4a72-85fd-1f53655030c8}</MetaDataID>
        [CachingDataOnClientSide]
        string Currency { get; set; }


        /// <MetaDataID>{def3eb8b-4fc3-41a3-9d56-8a4c83b95e0f}</MetaDataID>
        [CachingDataOnClientSide]
        string Identity { get; set; }

    }

    /// <MetaDataID>{c29b8985-3d96-4fea-91b6-7b3040b4d715}</MetaDataID>
    public enum PaymentType
    {
        None,
        Cash,
        DebitCard,
        CreditCard,
        Check
    }
}