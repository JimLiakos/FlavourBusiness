namespace FinanceFacade
{
    /// <MetaDataID>{0f548f7a-569e-4247-93e0-0de805f5d7f1}</MetaDataID>
    public interface ICashierDevice
    {
        /// <MetaDataID>{4e6e5a74-a982-42ac-83dc-10a5276f51a7}</MetaDataID>
        void PrintReceipt(ITransaction transaction);
    }
}