using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using GreekTaxAuthority;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Globalization;
using Xml2CSharp;

namespace GreekTaxAuthority.MyData
{
    /// <MetaDataID>{04f0fdfa-3e9d-4658-8aa9-ac0c7483d5f5}</MetaDataID>
    [XmlRoot(ElementName = "issuer", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Issuer
    {
        [XmlElement(ElementName = "vatNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatNumber { get; set; }
        [XmlElement(ElementName = "country", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Country { get; set; }
        [XmlElement(ElementName = "branch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Branch { get; set; }

        internal void Serialize(XElement issuerElement, XNamespace defaultNs)
        {
            XElement vatNumberElement = new XElement(defaultNs + "vatNumber") { Value = VatNumber };
            XElement countryElement = new XElement(defaultNs + "country") { Value = Country };
            XElement branchElement = new XElement(defaultNs + "branch") { Value = Branch };

            issuerElement.Add(vatNumberElement);
            issuerElement.Add(countryElement);
            issuerElement.Add(branchElement);

        }
    }

    /// <MetaDataID>{b697a0fa-ff9b-4045-ab73-ff59811a72fe}</MetaDataID>
    [XmlRoot(ElementName = "address", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Address
    {
        [XmlElement(ElementName = "postalCode", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "city", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string City { get; set; }

        internal void Serialize(XElement addressElement, XNamespace defaultNs)
        {
            XElement postalCodeElement = new XElement(defaultNs + "postalCode") { Value = PostalCode };
            XElement cityElement = new XElement(defaultNs + "city") { Value = City };

            addressElement.Add(postalCodeElement);
            addressElement.Add(cityElement);
        }
    }

    /// <MetaDataID>{e064935e-8bd8-4351-ba35-0db37f2274ff}</MetaDataID>
    [XmlRoot(ElementName = "counterpart", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Counterpart
    {
        [XmlElement(ElementName = "vatNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatNumber { get; set; }
        [XmlElement(ElementName = "country", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Country { get; set; }
        [XmlElement(ElementName = "branch", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Branch { get; set; }
        [XmlElement(ElementName = "address", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Address Address { get; set; }

        internal void Serialize(XElement counterpartElement, XNamespace defaultNs)
        {
            XElement vatNumberElement = new XElement(defaultNs + "vatNumber") { Value = VatNumber };
            XElement countryElement = new XElement(defaultNs + "country") { Value = Country };
            XElement branchElement = new XElement(defaultNs + "branch") { Value = Branch };
            XElement addressElement = new XElement(defaultNs + "address");

            counterpartElement.Add(vatNumberElement);
            counterpartElement.Add(countryElement);
            counterpartElement.Add(branchElement);
            counterpartElement.Add(addressElement);

            Address.Serialize(addressElement, defaultNs);




        }
    }

    /// <MetaDataID>{4235fb87-e7ca-4b3f-889f-75a0eeb35fd9}</MetaDataID>
    [XmlRoot(ElementName = "invoiceHeader", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class InvoiceHeader
    {
        [XmlElement(ElementName = "series", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Series { get; set; }
        [XmlElement(ElementName = "aa", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Aa { get; set; }
        [XmlElement(ElementName = "issueDate", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string IssueDateStr { get; set; }
        public DateTime IssueDate { get; set; }

        [XmlElement(ElementName = "invoiceType", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string InvoiceType { get; set; }
        [XmlElement(ElementName = "currency", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Currency { get; set; }

        [XmlElement(ElementName = "selfPricing", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string SelfPricing { get; set; }


        internal void Serialize(XElement invoiceHeaderElement, XNamespace defaultNs)
        {

            XElement seriesElement = new XElement(defaultNs + "series") { Value = Series };
            invoiceHeaderElement.Add(seriesElement);

            XElement aaElement = new XElement(defaultNs + "aa") { Value = Aa };
            invoiceHeaderElement.Add(aaElement);

            XElement issueDateElement = null;

            if (IssueDateStr != null)
                issueDateElement = new XElement(defaultNs + "IssueDate") { Value = IssueDateStr };
            else
                issueDateElement = new XElement(defaultNs + "IssueDate") { Value = IssueDate.ToString("yyyy-MM-dd") };

            invoiceHeaderElement.Add(issueDateElement);

            XElement invoiceTypeElement = new XElement(defaultNs + "invoiceType") { Value = InvoiceType };
            invoiceHeaderElement.Add(invoiceTypeElement);

            if (SelfPricing != null)
            {
                XElement selfPricingElement = new XElement(defaultNs + "currency") { Value = SelfPricing };
                invoiceHeaderElement.Add(selfPricingElement);
            }




        }
    }

    /// <MetaDataID>{7a07affa-d404-4f59-b4a6-9ffd5ba2c399}</MetaDataID>
    [XmlRoot(ElementName = "paymentMethodDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class PaymentMethodDetails
    {
        [XmlElement(ElementName = "type", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Type { get; set; }
        [XmlElement(ElementName = "amount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string Amount { get; set; }
        [XmlElement(ElementName = "paymentMethodInfo", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string PaymentMethodInfo { get; set; }

        internal void Serialize(XElement paymentMethodDetailsElement, XNamespace defaultNs)
        {

            XElement typeElement = new XElement(defaultNs + "type") { Value = Type };
            XElement amountElement = new XElement(defaultNs + "amount") { Value = Amount };
            XElement paymentMethodInfoElement = new XElement(defaultNs + "paymentMethodInfo") { Value = PaymentMethodInfo };

            paymentMethodDetailsElement.Add(typeElement);
            paymentMethodDetailsElement.Add(amountElement);
            paymentMethodDetailsElement.Add(paymentMethodInfoElement);

        }
    }

    /// <MetaDataID>{6d100d8d-d662-4bad-8e49-ccdfd648096e}</MetaDataID>
    [XmlRoot(ElementName = "paymentMethods", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class PaymentMethods
    {
        public PaymentMethods()
        {

        }
        [XmlElement(ElementName = "paymentMethodDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<PaymentMethodDetails> PaymentMethodDetails { get; set; } = new List<PaymentMethodDetails>();
        //public PaymentMethodDetails PaymentMethodDetails { get; set; }

        internal void Serialize(XElement paymentMethodsElement, XNamespace defaultNs)
        {
            foreach (PaymentMethodDetails paymentMethodDetails in PaymentMethodDetails)
            {
                XElement paymentMethodDetailsElement = new XElement(defaultNs + "paymentMethodDetails");
                paymentMethodDetails.Serialize(paymentMethodDetailsElement, defaultNs);
                paymentMethodsElement.Add(paymentMethodDetailsElement);
            }

            // PaymentMethodDetails
        }
    }

    /// <MetaDataID>{04a92083-a0bd-4f47-b8dd-494f25649be2}</MetaDataID>
    [XmlRoot(ElementName = "incomeClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class IncomeClassification
    {
        [XmlElement(ElementName = "classificationType", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
        public string ClassificationType { get; set; }
        [XmlElement(ElementName = "classificationCategory", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
        public string ClassificationCategory { get; set; }
        [XmlElement(ElementName = "amount", Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
        public string AmountStr { get; set; }
        public decimal Amount { get; set; }

        internal void Serialiaze(XElement incomeClassificationElement)
        {
            XNamespace ns = "icls";


            if (ClassificationType != null)
            {
                XElement classificationTypeElement = new XElement(ns + "classificationType") { Value = ClassificationType };
                incomeClassificationElement.Add(classificationTypeElement);
            }

            if (ClassificationCategory != null)
            {
                XElement classificationCategoryElement = new XElement(ns + "classificationCategory") { Value = ClassificationCategory };
                incomeClassificationElement.Add(classificationCategoryElement);
            }

            if (AmountStr != null)
            {
                XElement amountElement = new XElement(ns + "amount") { Value = AmountStr };
                incomeClassificationElement.Add(amountElement);
            }
            else if (Amount != 0)
            {
                XElement amountElement = new XElement(ns + "amount") { Value = Amount.ToString(CultureInfo.GetCultureInfo(1033)) };
                incomeClassificationElement.Add(amountElement);
            }





        }
    }


    /// <MetaDataID>{9de02cad-8223-4e9e-8914-53d815853164}</MetaDataID>
    [XmlRoot(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class ExpensesClassification
    {
        [XmlElement(ElementName = "classificationType", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public string ClassificationType { get; set; }
        [XmlElement(ElementName = "classificationCategory", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public string ClassificationCategory { get; set; }
        [XmlElement(ElementName = "amount", Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public string AmountStr { get; set; }
        public decimal Amount { get; set; }



        internal void Serialiaze(XElement expensesClassificationElement)
        {

            XNamespace ns = "ecls";
            if (ClassificationType != null)
            {
                XElement classificationTypeElement = new XElement(ns + "classificationType") { Value = ClassificationType };
                expensesClassificationElement.Add(classificationTypeElement);
            }
            if (ClassificationCategory != null)
            {
                XElement classificationCategoryElement = new XElement(ns + "classificationCategory") { Value = ClassificationCategory };
                expensesClassificationElement.Add(classificationCategoryElement);
            }

            if (AmountStr != null)
            {
                XElement amountElement = new XElement(ns + "amount") { Value = AmountStr };
                expensesClassificationElement.Add(amountElement);
            }
            else if (Amount != 0)
            {
                XElement amountElement = new XElement(ns + "amount") { Value = Amount.ToString(CultureInfo.GetCultureInfo(1033)) };
                expensesClassificationElement.Add(amountElement);
            }
        }

    }

    /// <MetaDataID>{4cffbae5-78e3-4136-86d9-d6e4934004b8}</MetaDataID>
    [XmlRoot(ElementName = "invoiceDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class InvoiceDetails
    {
        [XmlElement(ElementName = "lineNumber", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string LineNumberStr { get; set; }

        public int LineNumber { get; set; }


        [XmlElement(ElementName = "netValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string NetValueStr { get; set; }

        public decimal NetValue { get; set; }



        [XmlElement(ElementName = "withheldAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string WithheldAmountStr { get; set; }

        public decimal WithheldAmount { get; set; }


        [XmlElement(ElementName = "withheldPercentCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string WithheldPercentCategory { get; set; }

        [XmlElement(ElementName = "stampDutyAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string StampDutyAmountStr { get; set; }

        public decimal StampDutyAmount { get; set; }


        [XmlElement(ElementName = "stampDutyPercentCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string StampDutyPercentCategory { get; set; }
        [XmlElement(ElementName = "feesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string FeesAmountStr { get; set; }

        public decimal FeesAmount { get; set; }



        [XmlElement(ElementName = "feesPercentCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string FeesPercentCategory { get; set; }
        [XmlElement(ElementName = "otherTaxesPercentCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string OtherTaxesPercentCategory { get; set; }

        [XmlElement(ElementName = "otherTaxesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string OtherTaxesAmountStr { get; set; }

        public decimal OtherTaxesAmount { get; set; }






        [XmlElement(ElementName = "quantity", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string QuantityStr { get; set; }

        public decimal Quantity { get; set; }




        [XmlElement(ElementName = "vatCategory", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatCategoryStr { get; set; }

        public int VatCategory { get; set; }

        [XmlElement(ElementName = "vatAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string VatAmountStr { get; set; }

        public decimal VatAmount { get; set; }


        [XmlElement(ElementName = "discountOption", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string DiscountOption { get; set; }
        [XmlElement(ElementName = "incomeClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<IncomeClassification> IncomeClassifications { get; set; } = new List<IncomeClassification>();


        [XmlElement(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<ExpensesClassification> ExpensesClassifications { get; set; } = new List<ExpensesClassification>();

        internal void Serialize(XElement invoiceDetailElement, XNamespace defaultNs)
        {
            XElement lineNumberElement = null;
            if (LineNumberStr != null)
                lineNumberElement = new XElement(defaultNs + "lineNumber") { Value = LineNumberStr };
            else
                lineNumberElement = new XElement(defaultNs + "lineNumber") { Value = LineNumber.ToString(CultureInfo.GetCultureInfo(1033)) };


            invoiceDetailElement.Add(lineNumberElement);

            XElement netValueElement = null;
            if (NetValueStr != null)
                netValueElement = new XElement(defaultNs + "netValue") { Value = NetValueStr };
            else
                netValueElement = new XElement(defaultNs + "netValue") { Value = NetValue.ToString(CultureInfo.GetCultureInfo(1033)) };

            invoiceDetailElement.Add(lineNumberElement);

            XElement vatCategoryElement = null;
            if (VatCategoryStr != null)
                vatCategoryElement = new XElement(defaultNs + "vatCategory") { Value = VatCategoryStr };
            else
                vatCategoryElement = new XElement(defaultNs + "vatCategory") { Value = VatCategory.ToString(CultureInfo.GetCultureInfo(1033)) };

            
            invoiceDetailElement.Add(vatCategoryElement);
            XElement vatAmountElement = null;
            if (VatAmountStr != null)
                vatAmountElement = new XElement(defaultNs + "vatAmount") { Value = VatAmountStr };
            else
                vatAmountElement = new XElement(defaultNs + "vatAmount") { Value = VatAmount.ToString(CultureInfo.GetCultureInfo(1033)) };

            invoiceDetailElement.Add(vatAmountElement);

            if (DiscountOption != null)
            {
                XElement discountOptionElement = new XElement(defaultNs + "discountOption") { Value = DiscountOption };
                invoiceDetailElement.Add(vatAmountElement);
            }

            foreach (var incomeClassification in IncomeClassifications)
            {
                XElement incomeClassificationElement = new XElement(defaultNs + "incomeClassification");
                incomeClassification.Serialiaze(incomeClassificationElement);
                invoiceDetailElement.Add(incomeClassification);
            }

            foreach (var expensesClassification in ExpensesClassifications)
            {
                XElement expensesClassificationElement = new XElement(defaultNs + "expensesClassification");
                expensesClassification.Serialiaze(expensesClassificationElement);
                invoiceDetailElement.Add(expensesClassificationElement);
            }

            //XElement incomeClassificationElement = new XElement(defaultNs+"incomeClassification");
            //IncomeClassification.Serialiaze(incomeClassificationElement);

            //invoiceDetailElement.Add(incomeClassificationElement);

        }
    }

    /// <MetaDataID>{411a9c3f-4872-45b1-b350-f1f9de8725f2}</MetaDataID>
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
    }

    /// <MetaDataID>{6229df5b-5a88-4b8e-8620-229c3ec180aa}</MetaDataID>
    [XmlRoot(ElementName = "taxesTotals", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class TaxesTotals
    {
        [XmlElement(ElementName = "taxes", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Taxes Taxes { get; set; }
    }

    /// <MetaDataID>{09dbf74a-869d-4735-b2bc-5a8218046593}</MetaDataID>
    [XmlRoot(ElementName = "invoiceSummary", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class InvoiceSummary
    {
        /// <summary>
        /// Σύνολο Καθαρής Αξίας 
        /// </summary>
        [XmlElement(ElementName = "totalNetValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalNetValueStr { get; set; }


        /// <summary>
        /// Σύνολο Καθαρής Αξίας 
        /// </summary>
        public decimal TotalNetValue { get; set; }

        /// <summary>
        /// Σύνολο ΦΠΑ
        /// </summary>
        [XmlElement(ElementName = "totalVatAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalVatAmountStr { get; set; }

        /// <summary>
        /// Σύνολο ΦΠΑ
        /// </summary>
        public decimal TotalVatAmount { get; set; }

        /// <summary>
        /// Σύνολο Παρακρατήσεων Φόρων
        /// </summary>
        [XmlElement(ElementName = "totalWithheldAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalWithheldAmountStr { get; set; }

        /// <summary>
        /// Σύνολο Παρακρατήσεων Φόρων
        /// </summary>
        public decimal TotalWithheldAmount { get; set; }

        /// <summary>
        /// Σύνολο Τελών
        /// </summary>
        [XmlElement(ElementName = "totalFeesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalFeesAmountStr { get; set; }

        /// <summary>
        /// Σύνολο Τελών
        /// </summary>
        public decimal TotalFeesAmount { get; set; }


        /// <summary>
        /// Σύνολο Χαρτοσήμου 
        /// </summary>
        [XmlElement(ElementName = "totalStampDutyAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalStampDutyAmountStr { get; set; }

        /// <summary>
        /// Σύνολο Χαρτοσήμου 
        /// </summary>
        public decimal TotalStampDutyAmount { get; set; }


        /// <summary>
        /// Σύνολο Λοιπών Φόρων
        /// </summary>
        [XmlElement(ElementName = "totalOtherTaxesAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalOtherTaxesAmountStr { get; set; }

        /// <summary>
        /// Σύνολο Λοιπών Φόρων
        /// </summary>
        public decimal TotalOtherTaxesAmount { get; set; }

        /// <summary>
        /// Σύνολο Κρατήσεων
        /// </summary>
        [XmlElement(ElementName = "totalDeductionsAmount", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalDeductionsAmountStr { get; set; }

        /// <summary>
        /// Σύνολο Κρατήσεων
        /// </summary>
        public decimal TotalDeductionsAmount { get; set; }

        /// <summary>
        /// Συνολική Αξία
        /// </summary>
        [XmlElement(ElementName = "totalGrossValue", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public string TotalGrossValueStr { get; set; }

        /// <summary>
        /// Συνολική Αξία
        /// </summary>
        public decimal TotalGrossValue { get; set; }



        [XmlElement(ElementName = "incomeClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<IncomeClassification> IncomeClassifications { get; set; } = new List<IncomeClassification>();

        [XmlElement(ElementName = "expensesClassification", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<ExpensesClassification> ExpensesClassifications { get; set; } = new List<ExpensesClassification>();

        internal void Serialize(XElement invoiceSummaryElement, XNamespace defaultNs)
        {

            XElement totalNetValueElement = null;
            if (TotalNetValueStr != null)
                totalNetValueElement = new XElement(defaultNs + "totalNetValue") { Value = TotalNetValueStr };
            else
                totalNetValueElement = new XElement(defaultNs + "totalNetValue") { Value = TotalNetValue.ToString(CultureInfo.GetCultureInfo(1033)) };


            XElement totalVatAmountElement = null;
            if (TotalVatAmountStr != null)
                totalVatAmountElement = new XElement(defaultNs + "totalVatAmount") { Value = TotalVatAmountStr };
            else
                totalVatAmountElement = new XElement(defaultNs + "totalVatAmount") { Value = TotalVatAmount.ToString(CultureInfo.GetCultureInfo(1033)) };

            XElement totalWithheldAmountElement = null;
            if (TotalWithheldAmountStr != null)
                totalWithheldAmountElement = new XElement(defaultNs + "totalWithheldAmount") { Value = TotalWithheldAmountStr };
            else
                totalWithheldAmountElement = new XElement(defaultNs + "totalWithheldAmount") { Value = TotalWithheldAmount.ToString(CultureInfo.GetCultureInfo(1033)) };

            XElement totalFeesAmountElement = null;
            if (TotalFeesAmountStr != null)
                totalFeesAmountElement = new XElement(defaultNs + "totalFeesAmount") { Value = TotalFeesAmountStr };
            else
                totalFeesAmountElement = new XElement(defaultNs + "totalFeesAmount") { Value = TotalFeesAmount.ToString(CultureInfo.GetCultureInfo(1033)) };

            XElement totalStampDutyElement = null;
            if (TotalStampDutyAmountStr != null)
                totalStampDutyElement = new XElement(defaultNs + "totalStampDuty") { Value = TotalStampDutyAmountStr };
            else
                totalStampDutyElement = new XElement(defaultNs + "totalStampDuty") { Value = TotalStampDutyAmount.ToString(CultureInfo.GetCultureInfo(1033)) };

            XElement totalOtherTaxesAmountElement = null;
            if (TotalOtherTaxesAmountStr != null)
                totalOtherTaxesAmountElement = new XElement(defaultNs + "totalOtherTaxesAmount") { Value = TotalOtherTaxesAmountStr };
            else
                totalOtherTaxesAmountElement = new XElement(defaultNs + "totalOtherTaxesAmount") { Value = TotalOtherTaxesAmount.ToString(CultureInfo.GetCultureInfo(1033)) };

            XElement totalDeductionsAmountElement = null;
            if (TotalDeductionsAmountStr != null)
                totalDeductionsAmountElement = new XElement(defaultNs + "totalDeductionsAmount") { Value = TotalDeductionsAmountStr };
            else
                totalDeductionsAmountElement = new XElement(defaultNs + "totalDeductionsAmount") { Value = TotalDeductionsAmount.ToString(CultureInfo.GetCultureInfo(1033)) };

            XElement totalGrossValueElement = null;
            if (TotalGrossValueStr != null)
                totalGrossValueElement = new XElement(defaultNs + "totalGrossValue") { Value = TotalGrossValueStr };
            else
                totalGrossValueElement = new XElement(defaultNs + "totalGrossValue") { Value = TotalGrossValue.ToString(CultureInfo.GetCultureInfo(1033)) };


            invoiceSummaryElement.Add(totalNetValueElement);
            invoiceSummaryElement.Add(totalVatAmountElement);
            invoiceSummaryElement.Add(totalWithheldAmountElement);
            invoiceSummaryElement.Add(totalFeesAmountElement);
            invoiceSummaryElement.Add(totalStampDutyElement);
            invoiceSummaryElement.Add(totalOtherTaxesAmountElement);
            invoiceSummaryElement.Add(totalDeductionsAmountElement);

            invoiceSummaryElement.Add(totalGrossValueElement);



            foreach (var incomeClassification in IncomeClassifications)
            {
                XElement incomeClassificationElement = new XElement(defaultNs + "incomeClassification");
                incomeClassification.Serialiaze(incomeClassificationElement);
                invoiceSummaryElement.Add(incomeClassification);
            }


            foreach (var expensesClassification in ExpensesClassifications)
            {
                XElement expensesClassificationElement = new XElement(defaultNs + "expensesClassification");
                expensesClassification.Serialiaze(expensesClassificationElement);
                invoiceSummaryElement.Add(expensesClassificationElement);
            }

        }
    }

    /// <MetaDataID>{e88942f5-1e46-4b38-87bb-939182c83c17}</MetaDataID>
    [XmlRoot(ElementName = "invoice", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class Invoice
    {
        [XmlElement(ElementName = "issuer", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Issuer Issuer { get; set; } = new Issuer();
        [XmlElement(ElementName = "counterpart", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public Counterpart Counterpart { get; set; }
        [XmlElement(ElementName = "invoiceHeader", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public InvoiceHeader InvoiceHeader { get; set; } = new InvoiceHeader();
        [XmlElement(ElementName = "paymentMethods", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public PaymentMethods PaymentMethods { get; set; }
        [XmlElement(ElementName = "invoiceDetails", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<InvoiceDetails> InvoiceDetails { get; set; } = new List<InvoiceDetails>();
        [XmlElement(ElementName = "taxesTotals", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public TaxesTotals TaxesTotals { get; set; }
        [XmlElement(ElementName = "invoiceSummary", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public InvoiceSummary InvoiceSummary { get; set; } = new InvoiceSummary();

        public void CalculateInvoiceSummary()
        {

            this.InvoiceSummary.TotalVatAmount = this.InvoiceDetails.Sum(x => x.VatAmount);
            this.InvoiceSummary.TotalNetValue = this.InvoiceDetails.Sum(x => x.NetValue);

            this.InvoiceSummary.TotalWithheldAmount = this.InvoiceDetails.Sum(x => x.WithheldAmount);

            this.InvoiceSummary.TotalStampDutyAmount = this.InvoiceDetails.Sum(x => x.StampDutyAmount);
            this.InvoiceSummary.TotalFeesAmount = this.InvoiceDetails.Sum(x => x.FeesAmount);

            this.InvoiceSummary.TotalOtherTaxesAmount = this.InvoiceDetails.Sum(x => x.OtherTaxesAmount);

            this.InvoiceSummary.TotalGrossValue = this.InvoiceSummary.TotalVatAmount + this.InvoiceSummary.TotalNetValue - this.InvoiceSummary.TotalWithheldAmount;


            foreach (var summaryIncomeClassification in (from invoiceDetails in InvoiceDetails
                                                         from incomeClassification in invoiceDetails.IncomeClassifications
                                                         group incomeClassification by incomeClassification.ClassificationCategory + incomeClassification.ClassificationType into incomeClassifications
                                                         select new
                                                         {
                                                             incomeClassification = incomeClassifications.First(),
                                                             totalAmount = incomeClassifications.Sum(x => x.Amount)
                                                         }))
            {
                var incomeClassification = new IncomeClassification();
                incomeClassification.ClassificationCategory = summaryIncomeClassification.incomeClassification.ClassificationCategory;
                incomeClassification.ClassificationType = summaryIncomeClassification.incomeClassification.ClassificationType;

                incomeClassification.Amount = summaryIncomeClassification.totalAmount;
                InvoiceSummary.IncomeClassifications.Add(incomeClassification);

            }

            foreach (var summaryExpensesClassification in (from invoiceDetails in InvoiceDetails
                                                           from expensesClassification in invoiceDetails.ExpensesClassifications
                                                           group expensesClassification by expensesClassification.ClassificationCategory + expensesClassification.ClassificationType into expensesClassifications
                                                           select new
                                                           {
                                                               expensesClassification = expensesClassifications.First(),
                                                               totalAmount = expensesClassifications.Sum(x => x.Amount)
                                                           }))
            {
                var expensesClassification = new ExpensesClassification();
                expensesClassification.ClassificationCategory = summaryExpensesClassification.expensesClassification.ClassificationCategory;
                expensesClassification.ClassificationType = summaryExpensesClassification.expensesClassification.ClassificationType;

                expensesClassification.Amount = summaryExpensesClassification.totalAmount;
                InvoiceSummary.ExpensesClassifications.Add(expensesClassification);

            }


        }

        internal void Serialize(XElement invoiceElement, XNamespace defaultNs)
        {
            if (Issuer != null)
            {
                XElement issuerElement = new XElement(defaultNs + "issuer");

                Issuer.Serialize(issuerElement, defaultNs);
                invoiceElement.Add(issuerElement);


            }
            if (Counterpart != null)
            {
                XElement counterpartElement = new XElement(defaultNs + "counterpart");

                Counterpart.Serialize(counterpartElement, defaultNs);
                invoiceElement.Add(counterpartElement);
            }

            if (InvoiceHeader != null)
            {
                XElement invoiceHeaderElement = new XElement(defaultNs + "invoiceHeader");

                InvoiceHeader.Serialize(invoiceHeaderElement, defaultNs);
                invoiceElement.Add(invoiceHeaderElement);
            }

            if (PaymentMethods != null)
            {
                XElement paymentMethodsElement = new XElement(defaultNs + "paymentMethods");
                PaymentMethods.Serialize(paymentMethodsElement, defaultNs);
                invoiceElement.Add(paymentMethodsElement);
            }

            foreach (var invoiceDetail in InvoiceDetails)
            {
                XElement invoiceDetailElement = new XElement(defaultNs + "invoiceDetails");
                invoiceDetail.Serialize(invoiceDetailElement, defaultNs);
                invoiceElement.Add(invoiceDetailElement);
            }

            if (InvoiceSummary != null)
            {
                XElement invoiceSummaryElement = new XElement(defaultNs + "invoiceSummary");
                InvoiceSummary.Serialize(invoiceSummaryElement, defaultNs);
                invoiceElement.Add(invoiceSummaryElement);
            }

        }
    }

    /// <MetaDataID>{bc6daa64-f20d-469d-b7a9-ddb7c2616e4b}</MetaDataID>
    [XmlRoot(ElementName = "InvoicesDoc", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public class InvoicesDoc
    {
        [XmlElement(ElementName = "invoice", Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();

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



        public static InvoicesDoc GetInvoiceDocFromXMLStream(Stream xmlStream)
        {
            try
            {
                XDocument doc = XDocument.Load(xmlStream);
                var serializer = new XmlSerializer(typeof(InvoicesDoc));
                return serializer.Deserialize(doc.Root.CreateReader()) as InvoicesDoc;
            }
            catch (Exception error)
            {
                return null;
            }
        }
        public static XDocument Serialize<T>(T obj)
        {


            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "http://www.aade.gr/myDATA/invoice/v1.0");
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            namespaces.Add("xsi:schemaLocation", "http://www.aade.gr/myDATA/invoice/v1.0/InvoicesDoc-v0.6.xsd");
            namespaces.Add("xmlns:icls", "https://www.aade.gr/myDATA/incomeClassificaton/v1.0");
            namespaces.Add("xmlns:ecls", "https://www.aade.gr/myDATA/expensesClassificaton/v1.0");



            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    xsSubmit.Serialize(stream, obj, namespaces);

                    stream.Position = 0;
                    XDocument doc = XDocument.Load(stream);

                    return doc;
                }
            }
        }

        public static void Test()
        {
            var names = typeof(InvoicesDoc).Assembly.GetManifestResourceNames();

            foreach (var item in names.Where(x => x.IndexOf("GreekTaxAuthority.Resources.SampleXML") == 0))
            {
                //"GreekTaxAuthority.Resources.SampleXML_1.1_taxes_per_invoice (ΤΙΜ-ΠΩΛΗΣΗΣ).xml"
                var stream = typeof(InvoicesDoc).Assembly.GetManifestResourceStream(item);


                try
                {
                    var invoiceDoc = GetInvoiceDocFromXMLStream(stream);

                    XDocument doc = invoiceDoc.Serialize();
                    // XDocument document = Serialize<InvoicesDoc>(invoiceDoc);
                }
                catch (Exception error)
                {

                }

            }

        }

        public XDocument Serialize()
        {

            var invoicesDoc = XDocument.Parse(@"<InvoicesDoc 
                xmlns=""http://www.aade.gr/myDATA/invoice/v1.0"" 
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                xsi:schemaLocation=""http://www.aade.gr/myDATA/invoice/v1.0/InvoicesDoc-v0.6.xsd"" 
                xmlns:icls=""https://www.aade.gr/myDATA/incomeClassificaton/v1.0""
                xmlns:ecls=""https://www.aade.gr/myDATA/expensesClassificaton/v1.0"">
            </InvoicesDoc>");

            XNamespace defaultNs = "http://www.aade.gr/myDATA/invoice/v1.0";

            foreach (var invoice in this.Invoices)
            {
                XElement invoiceElement = new XElement(defaultNs + "invoice");
                invoice.Serialize(invoiceElement, defaultNs);
                invoicesDoc.Root.Add(invoiceElement);
                invoicesDoc.Root.Add(invoiceElement);
            }

            return invoicesDoc;

        }
    }

}

