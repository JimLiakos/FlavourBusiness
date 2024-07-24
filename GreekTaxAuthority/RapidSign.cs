using OOAdvantech.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekTaxAuthority.RapidSign
{


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    /// <MetaDataID>{a4d9efe3-81ac-4171-a330-7fbbe01ef6d5}</MetaDataID>
    public class Address
    {
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
    }

    /// <MetaDataID>{0c2fc412-94bd-40d4-b155-6b8720d90b0d}</MetaDataID>
    public class Counterpart
    {
        public string VatNumber { get; set; }
        public int CountryId { get; set; }
        public int Branch { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Activity { get; set; }
        public string TaxOffice { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double PrevBalance { get; set; }
        public double NextBalance { get; set; }
        public Address Address { get; set; }
    }

    /// <MetaDataID>{f1cfb0c3-ab5c-47f6-8f22-01e69b7c25d6}</MetaDataID>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class InvoiceDetail
    {
        public int Line { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int MUnitId { get; set; } = 7;
        public double Qty { get; set; }
        public double ItemPrc { get; set; }
        public double TotPrcAfterDisc { get; set; }
        public double NetValue { get; set; }
        public double VatAmount { get; set; }
        public int VatCatId { get; set; }
        public int? IncomeCatId { get; set; }
        public int? IncomeValId { get; set; }
    }

    /// <MetaDataID>{c96a594d-a353-4a11-80a6-3336405f3df4}</MetaDataID>
    public class InvoiceHeader
    {
        public int InvoiceTypeId { get; set; }
        public bool IncludesVat { get; set; }
        public string Series { get; set; }
        public string Aa { get; set; }
        public DateTime IssueDate { get; set; }
        public int CurrencyId { get; set; }
    }

    /// <MetaDataID>{5fb6b923-ad41-40ec-b7ac-830c876deea6}</MetaDataID>
    public class Issuer
    {
        public string VatNumber { get; set; }
        public int CountryId { get; set; }
        public int Branch { get; set; }
        public string Name { get; set; }
        public string Activity { get; set; }
        public string TaxOffice { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    /// <MetaDataID>{1983c6e3-7a31-4456-aa23-f10c155b1a54}</MetaDataID>
    public class PaymentMethod
    {
        public string PayGuid { get; set; }
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public double TipAmount { get; set; }
        public int AcquirerId { get; set; }
        public int PaymentStatus { get; set; }
    }

    /// <MetaDataID>{6eec626b-ed7b-4565-962f-15e1d51b5225}</MetaDataID>
    public class Invoice
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public bool TransmissionFailure { get; set; }
        public int Template { get; set; } = 3;
        public int FileType { get; set; } = 2;
        public int PaymentStatus { get; set; }
        public InvoiceHeader InvoiceHeader { get; set; } = new RapidSign.InvoiceHeader();
        public Issuer Issuer { get; set; } = new Issuer();
        public Counterpart Counterpart { get; set; }
        public Shipping Shipping { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
        public List<PaymentMethod> PaymentMethods { get; set; }
    }

    /// <MetaDataID>{9c842ca9-e758-4340-b652-0c22f3f46c17}</MetaDataID>
    public class Shipping
    {
        public int PurposeId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Method { get; set; }
        public string Vehicle { get; set; }
    }


}
