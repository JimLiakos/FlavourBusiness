//using System;
//using System.Xml.Serialization;
//using System.Collections.Generic;
//namespace Xml2CSharpp
//{
//    [XmlRoot(ElementName = "issuer", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class Issuer
//    {
//        [XmlElement(ElementName = "vatNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string VatNumber { get; set; }
//        [XmlElement(ElementName = "country", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Country { get; set; }
//        [XmlElement(ElementName = "branch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Branch { get; set; }
//    }

//    [XmlRoot(ElementName = "address", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class Address
//    {
//        [XmlElement(ElementName = "postalCode", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string PostalCode { get; set; }
//        [XmlElement(ElementName = "city", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string City { get; set; }
//    }

//    [XmlRoot(ElementName = "counterpart", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class Counterpart
//    {
//        [XmlElement(ElementName = "vatNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string VatNumber { get; set; }
//        [XmlElement(ElementName = "country", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Country { get; set; }
//        [XmlElement(ElementName = "branch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Branch { get; set; }
//        [XmlElement(ElementName = "address", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public Address Address { get; set; }
//    }

//    [XmlRoot(ElementName = "invoiceHeader", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class InvoiceHeader
//    {
//        [XmlElement(ElementName = "series", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Series { get; set; }
//        [XmlElement(ElementName = "aa", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Aa { get; set; }
//        [XmlElement(ElementName = "issueDate", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string IssueDate { get; set; }
//        [XmlElement(ElementName = "invoiceType", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string InvoiceType { get; set; }
//        [XmlElement(ElementName = "currency", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Currency { get; set; }
//        [XmlElement(ElementName = "selfPricing", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string SelfPricing { get; set; }
//    }

//    [XmlRoot(ElementName = "paymentMethodDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class PaymentMethodDetails
//    {
//        [XmlElement(ElementName = "type", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Type { get; set; }
//        [XmlElement(ElementName = "amount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string Amount { get; set; }
//        [XmlElement(ElementName = "paymentMethodInfo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string PaymentMethodInfo { get; set; }
//    }

//    [XmlRoot(ElementName = "paymentMethods", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class PaymentMethods
//    {
//        [XmlElement(ElementName = "paymentMethodDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public PaymentMethodDetails PaymentMethodDetails { get; set; }
//    }

//    [XmlRoot(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class ExpensesClassification
//    {
//        [XmlElement(ElementName = "classificationType", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
//        public string ClassificationType { get; set; }
//        [XmlElement(ElementName = "classificationCategory", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
//        public string ClassificationCategory { get; set; }
//        [XmlElement(ElementName = "amount", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
//        public string Amount { get; set; }
//    }

//    [XmlRoot(ElementName = "invoiceDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class InvoiceDetails
//    {
//        [XmlElement(ElementName = "lineNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string LineNumber { get; set; }
//        [XmlElement(ElementName = "netValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string NetValue { get; set; }
//        [XmlElement(ElementName = "vatCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string VatCategory { get; set; }
//        [XmlElement(ElementName = "vatAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string VatAmount { get; set; }
//        [XmlElement(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public List<ExpensesClassification> ExpensesClassification { get; set; }
//    }

//    [XmlRoot(ElementName = "invoiceSummary", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class InvoiceSummary
//    {
//        [XmlElement(ElementName = "totalNetValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string TotalNetValue { get; set; }
//        [XmlElement(ElementName = "totalVatAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string TotalVatAmount { get; set; }
//        [XmlElement(ElementName = "totalWithheldAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string TotalWithheldAmount { get; set; }
//        [XmlElement(ElementName = "totalFeesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string TotalFeesAmount { get; set; }
//        [XmlElement(ElementName = "totalStampDutyAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string TotalStampDutyAmount { get; set; }
//        [XmlElement(ElementName = "totalOtherTaxesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string TotalOtherTaxesAmount { get; set; }
//        [XmlElement(ElementName = "totalDeductionsAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string TotalDeductionsAmount { get; set; }
//        [XmlElement(ElementName = "totalGrossValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public string TotalGrossValue { get; set; }
//        [XmlElement(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public List<ExpensesClassification> ExpensesClassification { get; set; }
//    }

//    [XmlRoot(ElementName = "invoice", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class Invoice
//    {
//        [XmlElement(ElementName = "issuer", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public Issuer Issuer { get; set; }
//        [XmlElement(ElementName = "counterpart", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public Counterpart Counterpart { get; set; }
//        [XmlElement(ElementName = "invoiceHeader", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public InvoiceHeader InvoiceHeader { get; set; }
//        [XmlElement(ElementName = "paymentMethods", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public PaymentMethods PaymentMethods { get; set; }
//        [XmlElement(ElementName = "invoiceDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public InvoiceDetails InvoiceDetails { get; set; }
//        [XmlElement(ElementName = "invoiceSummary", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public InvoiceSummary InvoiceSummary { get; set; }
//    }

//    [XmlRoot(ElementName = "InvoicesDoc", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//    public class InvoicesDoc
//    {
//        [XmlElement(ElementName = "invoice", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
//        public Invoice Invoice { get; set; }
//        [XmlAttribute(AttributeName = "xmlns")]
//        public string Xmlns { get; set; }
//        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
//        public string Xsi { get; set; }
//        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
//        public string SchemaLocation { get; set; }
//        [XmlAttribute(AttributeName = "icls", Namespace = "http://www.w3.org/2000/xmlns/")]
//        public string Icls { get; set; }
//        [XmlAttribute(AttributeName = "ecls", Namespace = "http://www.w3.org/2000/xmlns/")]
//        public string Ecls { get; set; }
//    }

//}
