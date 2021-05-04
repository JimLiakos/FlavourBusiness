namespace FinanceFacade
{
    /// <MetaDataID>{16ac0a20-93a6-4323-98ad-94e75fc02aef}</MetaDataID>
    public struct Amount
    {

        //
        // Summary:
        //     /// 3-letter [currency code](https://developer.paypal.com/docs/integration/direct/rest_api_payment_country_currency_support/).
        //     PayPal does not support all currencies. ///
        /// <MetaDataID>{f4611c90-5caa-466d-a3e0-d837950d0a6f}</MetaDataID>
        public string Currency { get; set; }

        //
        // Summary:
        //     /// Total amount charged from the payer to the payee. In case of a refund, this
        //     is the refunded amount to the original payer from the payee. 10 characters max
        //     with support for 2 decimal places. ///
        /// <MetaDataID>{382677a3-2754-4c31-971d-7fc7c86d95c8}</MetaDataID>
        public decimal Total { get; set; }
    }

}