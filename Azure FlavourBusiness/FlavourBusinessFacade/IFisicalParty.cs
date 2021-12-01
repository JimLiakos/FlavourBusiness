namespace FinanceFacade
{
    /// <MetaDataID>{55f92046-b447-4988-bfaa-355da614ed9a}</MetaDataID>
    public interface IFisicalParty
    {
        string Name { get; set; }
        string Branch { get; set; }
        string CountryCode { get; set; }
        string VATNumber { get; set; }
    }
}

//GoogleApiKey : AIzaSyBzBywuLSty8mrTU4q-u5oGQuyakfe3vZ4