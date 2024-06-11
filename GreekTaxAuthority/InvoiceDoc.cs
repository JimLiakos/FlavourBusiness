/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
    [XmlRoot(ElementName = "address", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Address
    {
        [XmlElement(ElementName = "street", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Street { get; set; }
        [XmlElement(ElementName = "number", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Number { get; set; }
        [XmlElement(ElementName = "postalCode", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "city", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string City { get; set; }
    }

    [XmlRoot(ElementName = "issuer", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Issuer
    {
        [XmlElement(ElementName = "vatNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatNumber { get; set; }
        [XmlElement(ElementName = "country", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Country { get; set; }
        [XmlElement(ElementName = "branch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Branch { get; set; }
        [XmlElement(ElementName = "name", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Name { get; set; }
        [XmlElement(ElementName = "address", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Address Address { get; set; }
        [XmlElement(ElementName = "documentIdNo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string DocumentIdNo { get; set; }
        [XmlElement(ElementName = "supplyAccountNo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SupplyAccountNo { get; set; }
        [XmlElement(ElementName = "countryDocumentId", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string CountryDocumentId { get; set; }
    }

    [XmlRoot(ElementName = "counterpart", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Counterpart
    {
        [XmlElement(ElementName = "vatNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatNumber { get; set; }
        [XmlElement(ElementName = "country", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Country { get; set; }
        [XmlElement(ElementName = "branch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Branch { get; set; }
        [XmlElement(ElementName = "name", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Name { get; set; }
        [XmlElement(ElementName = "address", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Address Address { get; set; }
        [XmlElement(ElementName = "documentIdNo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string DocumentIdNo { get; set; }
        [XmlElement(ElementName = "supplyAccountNo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SupplyAccountNo { get; set; }
        [XmlElement(ElementName = "countryDocumentId", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string CountryDocumentId { get; set; }
    }

    [XmlRoot(ElementName = "entityData", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class EntityData
    {
        [XmlElement(ElementName = "vatNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatNumber { get; set; }
        [XmlElement(ElementName = "country", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Country { get; set; }
        [XmlElement(ElementName = "branch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Branch { get; set; }
        [XmlElement(ElementName = "name", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Name { get; set; }
        [XmlElement(ElementName = "address", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Address Address { get; set; }
        [XmlElement(ElementName = "documentIdNo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string DocumentIdNo { get; set; }
        [XmlElement(ElementName = "supplyAccountNo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SupplyAccountNo { get; set; }
        [XmlElement(ElementName = "countryDocumentId", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string CountryDocumentId { get; set; }
    }

    [XmlRoot(ElementName = "otherCorrelatedEntities", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class OtherCorrelatedEntities
    {
        [XmlElement(ElementName = "type", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Type { get; set; }
        [XmlElement(ElementName = "entityData", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public EntityData EntityData { get; set; }
    }

    [XmlRoot(ElementName = "loadingAddress", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class LoadingAddress
    {
        [XmlElement(ElementName = "street", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Street { get; set; }
        [XmlElement(ElementName = "number", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Number { get; set; }
        [XmlElement(ElementName = "postalCode", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "city", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string City { get; set; }
    }

    [XmlRoot(ElementName = "deliveryAddress", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class DeliveryAddress
    {
        [XmlElement(ElementName = "street", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Street { get; set; }
        [XmlElement(ElementName = "number", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Number { get; set; }
        [XmlElement(ElementName = "postalCode", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "city", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string City { get; set; }
    }

    [XmlRoot(ElementName = "otherDeliveryNoteHeader", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class OtherDeliveryNoteHeader
    {
        [XmlElement(ElementName = "loadingAddress", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public LoadingAddress LoadingAddress { get; set; }
        [XmlElement(ElementName = "deliveryAddress", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public DeliveryAddress DeliveryAddress { get; set; }
        [XmlElement(ElementName = "startShippingBranch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string StartShippingBranch { get; set; }
        [XmlElement(ElementName = "completeShippingBranch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string CompleteShippingBranch { get; set; }
    }

    [XmlRoot(ElementName = "invoiceHeader", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class InvoiceHeader
    {
        [XmlElement(ElementName = "series", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Series { get; set; }
        [XmlElement(ElementName = "aa", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Aa { get; set; }
        [XmlElement(ElementName = "issueDate", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string IssueDate { get; set; }
        [XmlElement(ElementName = "invoiceType", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string InvoiceType { get; set; }
        [XmlElement(ElementName = "vatPaymentSuspension", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatPaymentSuspension { get; set; }
        [XmlElement(ElementName = "currency", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Currency { get; set; }
        [XmlElement(ElementName = "exchangeRate", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string ExchangeRate { get; set; }
        [XmlElement(ElementName = "correlatedInvoices", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<string> CorrelatedInvoices { get; set; }
        [XmlElement(ElementName = "selfPricing", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SelfPricing { get; set; }
        [XmlElement(ElementName = "dispatchDate", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string DispatchDate { get; set; }
        [XmlElement(ElementName = "dispatchTime", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string DispatchTime { get; set; }
        [XmlElement(ElementName = "vehicleNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VehicleNumber { get; set; }
        [XmlElement(ElementName = "movePurpose", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string MovePurpose { get; set; }
        [XmlElement(ElementName = "fuelInvoice", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string FuelInvoice { get; set; }
        [XmlElement(ElementName = "specialInvoiceCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SpecialInvoiceCategory { get; set; }
        [XmlElement(ElementName = "invoiceVariationType", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string InvoiceVariationType { get; set; }
        [XmlElement(ElementName = "otherCorrelatedEntities", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<OtherCorrelatedEntities> OtherCorrelatedEntities { get; set; }
        [XmlElement(ElementName = "otherDeliveryNoteHeader", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public OtherDeliveryNoteHeader OtherDeliveryNoteHeader { get; set; }
        [XmlElement(ElementName = "isDeliveryNote", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string IsDeliveryNote { get; set; }
        [XmlElement(ElementName = "otherMovePurposeTitle", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string OtherMovePurposeTitle { get; set; }
        [XmlElement(ElementName = "thirdPartyCollection", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string ThirdPartyCollection { get; set; }
    }

    [XmlRoot(ElementName = "ProvidersSignature", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class ProvidersSignature
    {
        [XmlElement(ElementName = "SigningAuthor", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SigningAuthor { get; set; }
        [XmlElement(ElementName = "Signature", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Signature { get; set; }
    }

    [XmlRoot(ElementName = "ECRToken", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class ECRToken
    {
        [XmlElement(ElementName = "SigningAuthor", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SigningAuthor { get; set; }
        [XmlElement(ElementName = "SessionNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SessionNumber { get; set; }
    }

    [XmlRoot(ElementName = "paymentMethodDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class PaymentMethodDetails
    {
        [XmlElement(ElementName = "type", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Type { get; set; }
        [XmlElement(ElementName = "amount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Amount { get; set; }
        [XmlElement(ElementName = "paymentMethodInfo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string PaymentMethodInfo { get; set; }
        [XmlElement(ElementName = "tipAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TipAmount { get; set; }
        [XmlElement(ElementName = "transactionId", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TransactionId { get; set; }
        [XmlElement(ElementName = "ProvidersSignature", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public ProvidersSignature ProvidersSignature { get; set; }
        [XmlElement(ElementName = "ECRToken", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public ECRToken ECRToken { get; set; }
        [XmlElement(ElementName = "amount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Amount3 { get; set; }
    }

    [XmlRoot(ElementName = "paymentMethods", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class PaymentMethods
    {
        [XmlElement(ElementName = "paymentMethodDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<PaymentMethodDetails> PaymentMethodDetails { get; set; }
    }

    [XmlRoot(ElementName = "dienergia", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Dienergia
    {
        [XmlElement(ElementName = "applicationId", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string ApplicationId { get; set; }
        [XmlElement(ElementName = "applicationDate", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string ApplicationDate { get; set; }
        [XmlElement(ElementName = "doy", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Doy { get; set; }
        [XmlElement(ElementName = "shipId", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string ShipId { get; set; }
    }

    [XmlRoot(ElementName = "classificationType", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public class ClassificationType
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "classificationCategory", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public class ClassificationCategory
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "amount", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public class Amount
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "id", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public class Id
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "incomeClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class IncomeClassification
    {
        [XmlElement(ElementName = "classificationType", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
        public ClassificationType ClassificationType { get; set; }
        [XmlElement(ElementName = "classificationCategory", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
        public ClassificationCategory ClassificationCategory { get; set; }
        [XmlElement(ElementName = "amount", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
        public Amount Amount { get; set; }
        [XmlElement(ElementName = "id", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
        public Id Id { get; set; }
    }

    [XmlRoot(ElementName = "classificationType", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public class ClassificationType2
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "classificationCategory", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public class ClassificationCategory2
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "amount", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public class Amount2
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "vatAmount", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public class VatAmount
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "vatCategory", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public class VatCategory
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "vatExemptionCategory", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public class VatExemptionCategory
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "id", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public class Id2
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class ExpensesClassification
    {
        [XmlElement(ElementName = "classificationType", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public ClassificationType2 ClassificationType2 { get; set; }
        [XmlElement(ElementName = "classificationCategory", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public ClassificationCategory2 ClassificationCategory2 { get; set; }
        [XmlElement(ElementName = "amount", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public Amount2 Amount2 { get; set; }
        [XmlElement(ElementName = "vatAmount", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public VatAmount VatAmount { get; set; }
        [XmlElement(ElementName = "vatCategory", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public VatCategory VatCategory { get; set; }
        [XmlElement(ElementName = "vatExemptionCategory", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public VatExemptionCategory VatExemptionCategory { get; set; }
        [XmlElement(ElementName = "id", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public Id2 Id2 { get; set; }
    }

    [XmlRoot(ElementName = "invoiceDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class InvoiceDetails
    {
        [XmlElement(ElementName = "lineNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string LineNumber { get; set; }
        [XmlElement(ElementName = "recType", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string RecType { get; set; }
        [XmlElement(ElementName = "TaricNo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TaricNo { get; set; }
        [XmlElement(ElementName = "itemCode", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string ItemCode { get; set; }
        [XmlElement(ElementName = "itemDescr", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string ItemDescr { get; set; }
        [XmlElement(ElementName = "fuelCode", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string FuelCode { get; set; }
        [XmlElement(ElementName = "quantity", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Quantity { get; set; }
        [XmlElement(ElementName = "measurementUnit", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string MeasurementUnit { get; set; }
        [XmlElement(ElementName = "invoiceDetailType", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string InvoiceDetailType { get; set; }
        [XmlElement(ElementName = "netValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string NetValue { get; set; }
        [XmlElement(ElementName = "vatCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatCategory { get; set; }
        [XmlElement(ElementName = "vatAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatAmount { get; set; }
        [XmlElement(ElementName = "vatExemptionCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatExemptionCategory { get; set; }
        [XmlElement(ElementName = "dienergia", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Dienergia Dienergia { get; set; }
        [XmlElement(ElementName = "discountOption", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string DiscountOption { get; set; }
        [XmlElement(ElementName = "withheldAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string WithheldAmount { get; set; }
        [XmlElement(ElementName = "withheldPercentCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string WithheldPercentCategory { get; set; }
        [XmlElement(ElementName = "stampDutyAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string StampDutyAmount { get; set; }
        [XmlElement(ElementName = "stampDutyPercentCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string StampDutyPercentCategory { get; set; }
        [XmlElement(ElementName = "feesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string FeesAmount { get; set; }
        [XmlElement(ElementName = "feesPercentCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string FeesPercentCategory { get; set; }
        [XmlElement(ElementName = "otherTaxesPercentCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string OtherTaxesPercentCategory { get; set; }
        [XmlElement(ElementName = "otherTaxesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string OtherTaxesAmount { get; set; }
        [XmlElement(ElementName = "deductionsAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string DeductionsAmount { get; set; }
        [XmlElement(ElementName = "lineComments", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string LineComments { get; set; }
        [XmlElement(ElementName = "incomeClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<IncomeClassification> IncomeClassification { get; set; }
        [XmlElement(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<ExpensesClassification> ExpensesClassification { get; set; }
        [XmlElement(ElementName = "quantity15", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Quantity15 { get; set; }
        [XmlElement(ElementName = "otherMeasurementUnitQuantity", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string OtherMeasurementUnitQuantity { get; set; }
        [XmlElement(ElementName = "otherMeasurementUnitTitle", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string OtherMeasurementUnitTitle { get; set; }
        [XmlElement(ElementName = "vatCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatCategory2 { get; set; }
        [XmlElement(ElementName = "vatAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatAmount2 { get; set; }
        [XmlElement(ElementName = "vatExemptionCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatExemptionCategory2 { get; set; }
    }

    [XmlRoot(ElementName = "taxes", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Taxes
    {
        [XmlElement(ElementName = "taxType", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TaxType { get; set; }
        [XmlElement(ElementName = "taxCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TaxCategory { get; set; }
        [XmlElement(ElementName = "underlyingValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string UnderlyingValue { get; set; }
        [XmlElement(ElementName = "taxAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TaxAmount { get; set; }
        [XmlElement(ElementName = "id", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Id3 { get; set; }
    }

    [XmlRoot(ElementName = "taxesTotals", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class TaxesTotals
    {
        [XmlElement(ElementName = "taxes", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<Taxes> Taxes { get; set; }
    }

    [XmlRoot(ElementName = "invoiceSummary", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class InvoiceSummary
    {
        [XmlElement(ElementName = "totalNetValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalNetValue { get; set; }
        [XmlElement(ElementName = "totalVatAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalVatAmount { get; set; }
        [XmlElement(ElementName = "totalWithheldAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalWithheldAmount { get; set; }
        [XmlElement(ElementName = "totalFeesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalFeesAmount { get; set; }
        [XmlElement(ElementName = "totalStampDutyAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalStampDutyAmount { get; set; }
        [XmlElement(ElementName = "totalOtherTaxesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalOtherTaxesAmount { get; set; }
        [XmlElement(ElementName = "totalDeductionsAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalDeductionsAmount { get; set; }
        [XmlElement(ElementName = "totalGrossValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalGrossValue { get; set; }
        [XmlElement(ElementName = "incomeClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<IncomeClassification> IncomeClassification { get; set; }
        [XmlElement(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<ExpensesClassification> ExpensesClassification { get; set; }
    }

    [XmlRoot(ElementName = "otherTransportDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class OtherTransportDetails
    {
        [XmlElement(ElementName = "vehicleNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VehicleNumber { get; set; }
    }

    [XmlRoot(ElementName = "invoice", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Invoice
    {
        [XmlElement(ElementName = "uid", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Uid { get; set; }
        [XmlElement(ElementName = "mark", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Mark { get; set; }
        [XmlElement(ElementName = "cancelledByMark", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string CancelledByMark { get; set; }
        [XmlElement(ElementName = "authenticationCode", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string AuthenticationCode { get; set; }
        [XmlElement(ElementName = "transmissionFailure", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TransmissionFailure { get; set; }
        [XmlElement(ElementName = "issuer", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Issuer Issuer { get; set; }
        [XmlElement(ElementName = "counterpart", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Counterpart Counterpart { get; set; }
        [XmlElement(ElementName = "invoiceHeader", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public InvoiceHeader InvoiceHeader { get; set; }
        [XmlElement(ElementName = "paymentMethods", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public PaymentMethods PaymentMethods { get; set; }
        [XmlElement(ElementName = "invoiceDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<InvoiceDetails> InvoiceDetails { get; set; }
        [XmlElement(ElementName = "taxesTotals", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public TaxesTotals TaxesTotals { get; set; }
        [XmlElement(ElementName = "invoiceSummary", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public InvoiceSummary InvoiceSummary { get; set; }
        [XmlElement(ElementName = "qrCodeUrl", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string QrCodeUrl { get; set; }
        [XmlElement(ElementName = "otherTransportDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<OtherTransportDetails> OtherTransportDetails { get; set; }
    }

    [XmlRoot(ElementName = "InvoicesDoc", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class InvoicesDoc
    {
        [XmlElement(ElementName = "invoice", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<Invoice> Invoice { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "icls", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Icls { get; set; }
        [XmlAttribute(AttributeName = "ecls", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ecls { get; set; }
    }

}
