using GreekTaxAuthority.MyData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GreekTaxAuthority
{
    /// <MetaDataID>{0a9a8862-4385-4a02-b60c-2935192c43af}</MetaDataID>
    public class aade
    {
        public static async Task<string> SendIncomeInvoice(FinanceFacade.ITransaction transaction)
        {

            ///classificationCategory 
            /// aadeId="category1_2" 
            /// RapidSign ="1"
            ///Έσοδα από Πώληση Προϊόντων (+) / (-)
            /// 
            /// classificationType 
            ///aadeId="E3_561_003"
            /// RapidSign ="8"
            ///Description="Πωλήσεις αγαθών και υπηρεσιών Λιανικές - Ιδιωτική Πελατεία"
            ///
            //8.6 Δελτίο Παραγγελίας Εστίασης

            string mark = null;

            try
            {
                RapidSign.Invoice r_Invoice = new RapidSign.Invoice();



                MyData.InvoicesDoc invoicesDoc = new MyData.InvoicesDoc();
                MyData.Invoice invoice = new MyData.Invoice();
                invoicesDoc.Invoices.Add(invoice);
                invoice.InvoiceHeader.InvoiceType = "8.6";
                r_Invoice.InvoiceHeader.InvoiceTypeId = 19; //"8.6"; 
                r_Invoice.InvoiceHeader.IncludesVat = true;
                invoice.InvoiceHeader.Series = transaction.GetSeries();

                if (invoice.InvoiceHeader.Series == null)
                    invoice.InvoiceHeader.Series = "0";

                r_Invoice.InvoiceHeader.Series = transaction.GetSeries();
                if (r_Invoice.InvoiceHeader.Series == null)
                    r_Invoice.InvoiceHeader.Series = "0";


                invoice.InvoiceHeader.Aa = transaction.GetAutoNumber()?.ToString();
                r_Invoice.InvoiceHeader.Aa = transaction.GetAutoNumber()?.ToString();

                invoice.InvoiceHeader.IssueDate = transaction.GetIssueDate().Value;
                r_Invoice.InvoiceHeader.IssueDate = transaction.GetIssueDate().Value;

                invoice.InvoiceHeader.Currency = "EUR";
                r_Invoice.InvoiceHeader.CurrencyId = 47;//"EUR";

                invoice.Issuer.VatNumber = "800696676";
                r_Invoice.Issuer.VatNumber = "800696676";

                invoice.Issuer.Country = "GR";
                r_Invoice.Issuer.CountryId = 87;// "GR";
                invoice.Issuer.Branch = "0";
                r_Invoice.Issuer.Branch = 0;


                int invoiceDetails_aa = 0;
                foreach (var item in transaction.Items)
                {
                    MyData.InvoiceDetails invoiceDetails = new MyData.InvoiceDetails();

                    RapidSign.InvoiceDetail r_invoiceDetails = new RapidSign.InvoiceDetail();

                    invoiceDetails.LineNumber = (++invoiceDetails_aa);
                    r_invoiceDetails.Line = invoiceDetails.LineNumber;

                    var vatTax = item.Taxes.Where(x => aade.VatAccounts.ContainsKey(x.AccountID)).First();

                    invoiceDetails.VatAmount = Math.Round(vatTax.Amount, 2);
                    r_invoiceDetails.VatAmount = (double)Math.Round(vatTax.Amount, 2);

                    invoiceDetails.NetValue = Math.Round(item.Amount, 2) - Math.Round(item.Taxes.Sum(x => x.Amount), 2);
                    r_invoiceDetails.NetValue = (double)(Math.Round(item.Amount, 2) - Math.Round(item.Taxes.Sum(x => x.Amount), 2));
                    r_invoiceDetails.TotPrcAfterDisc=r_invoiceDetails.NetValue+r_invoiceDetails.VatAmount;
                    r_invoiceDetails.Name= item.Name;
                    r_invoiceDetails.Code= item.Sku;
                    r_invoiceDetails.Qty=(double)item.Quantity;

                    invoiceDetails.VatCategory = aade.VatAccounts[vatTax.AccountID];
                    r_invoiceDetails.VatCatId = aade.VatAccounts[vatTax.AccountID];

                    var incomeClassification = new IncomeClassification();
                    incomeClassification.ClassificationCategory = "category1_2";
                    incomeClassification.ClassificationType = "E3_561_003";
                    incomeClassification.Amount = invoiceDetails.NetValue;
                    invoiceDetails.IncomeClassifications.Add(incomeClassification);


                    var incomeTax = item.Taxes.Where(x => aade.IncomeCatIds.ContainsKey(x.AccountID)).First();
                    if (incomeTax!=null)
                        r_invoiceDetails.IncomeCatId = aade.IncomeCatIds[incomeTax.AccountID];//


                    invoice.InvoiceDetails.Add(invoiceDetails);
                    r_Invoice.InvoiceDetails.Add(r_invoiceDetails);

                }

                RapidSign.PaymentMethod r_PaymentMethod = new RapidSign.PaymentMethod();

                //" \"jsonData\": {\"idNames\": [{\"id\": 0,\"name\": \"Παρακαλώ επιλέξτε...\"},{\"id\": 1,\"name\": \"Επαγ. Λογαριασμός Πληρωμών Ημεδαπής\"},{\"id\": 2,\"name\": \"Επαγ. Λογαριασμός Πληρωμών Αλλοδαπής\"},{\"id\": 3,\"name\": \"Μετρητά\"},{\"id\": 4,\"name\": \"Επιταγή\"},{\"id\": 5,\"name\": \"Επί Πιστώσει\"},{\"id\": 6,\"name\": \"Web banking\"},{\"id\": 7,\"name\": \"POS / e-POS\"}]"


                r_PaymentMethod.PaymentId=3;//μετρητά
                r_PaymentMethod.PayGuid=Guid.NewGuid().ToString();
                //{ "profilen": null, "extCode": 100, "statusDescription": "SUCCESS", "message": "SUCCESS", "extraData": "", "token": "", "jsonData": { "idNames": [ { "id": 103, "name": "Euronet Merchant Services" }, { "id": 104, "name": "ProCredit Bank (Bulgaria)" }, { "id": 108, "name": "NBG PAY" }, { "id": 109, "name": "Worldline Merchant Acquiring" }, { "id": 110, "name": "Cosmote Payments" }, { "id": 111, "name": "Everypay" }, { "id": 113, "name": "Συνεταιριστική Τράπεζα Καρδίτσας" }, { "id": 114, "name": "Attica Bank" }, { "id": 116, "name": "Viva Υπηρεσίες Πληρωμών" }, { "id": 117, "name": "Συνεταιριστική Τράπεζα Ηπείρου" }, { "id": 118, "name": "Συνεταριστική Τράπεζα Κεντρικής Μακεδονίας" }, { "id": 120, "name": "Euronet Card Services" }, { "id": 122, "name": "CardLink" }, { "id": 127, "name": "EDPS" }, { "id": 128, "name": "Καινοτόμες Λύσεις και Υπηρεσίες ΜΕΠΕ INSS" }, { "id": 130, "name": "Mellon Technologies" }, { "id": 137, "name": "Nexi Greece Processing Services" }, { "id": 138, "name": "Nexi Πληρωμών Ελλάς" }, { "id": 143, "name": "TORA Direct" }, { "id": 619, "name": "myPOS Limited" }, { "id": 624, "name": "myPOS Technologies" }, { "id": 651, "name": "Adyen N.V." } ], "data": null, "matException": null, "dataLite": null, "providersSignature": null, "invRaw": null, "invoiceStatus": null, "orderStatus": null } }
                r_PaymentMethod.AcquirerId=116;
                r_PaymentMethod.Amount=


                invoice.CalculateInvoiceSummary();

                string invoicesDocXML = invoicesDoc.Serialize().ToString();
                string r_invoiceJson = OOAdvantech.Json.JsonConvert.SerializeObject(r_Invoice);

                string token = await GetProvideBearerToken();


                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://dev.rapidsign.com.gr/api/v1.0/provider/PostUnsignedInvoice?debug=true");

                request.Headers.Add("Authorization", $"Bearer {token}");
                var content = new StringContent(r_invoiceJson);
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var resonse = await response.Content.ReadAsStringAsync();



            }
            catch (Exception error)
            {


            }







            return mark;

        }

        private static async Task<string> GetProvideBearerToken()
        {


            if (rbsToken != null)
            {
                return rbsToken.token;
            }
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dev.rapidsign.com.gr/api/v1.0/provider/Authorize");

            request.Headers.Add("Cookie", ".AspNetCore.Identity.Application=CfDJ8G3BtpZWXRBKsRB37tZW3Zj-zkYiH54MBvnihzSETBp3i3ZyLxOIE1z4ohLH3fwPe8cJgQd5XamKwGLW4E0o9_AUDOyETdpjAzFHaPRttJW4NGap_Px7o0gk3qCpgBYQ1_K31X6Li-9cGQ_1-usyBARX53mk668sGFbLIZt0RSN3yatF0iJ1mSYv9hWetMTiOwf3c7DBq2cTOUo_0DG7LM8QULGBOF2FzYlT9jMWGwExn03iorRJQ-Ey5SEbGylWARt92v_TpDLSMfjGC-WVj0wOEZIrCI0T6Y80y2_xhBs80vRTRZljcNkS1EUymaQfCTJ-UlpGd1ELTwDK5iorln05CzoYWyUi-q8saMFhmgKgrCxIgA5gAF3qtridMIq0vyYMaG6_o25N_6O1P5nSulENHFeu3qG-p_eKWGAd9ucpgn-6EDdukDqSWa-h4vkjh7pHESp8gELEanKE1STsIdBIuhxS3r2J3EaNahIyhjzoYZrg7jPWijjZuP6azLKsFu4G-hLldx2-XHHiG7CbFb888N5OEwLSs8kidrt1uzPtlS6Nzg-mIoiM7wQBzAb7th7PQ_fp02Repl0-e33-wzsxxbJp_Pn5PPUoSEktbx2WWcQKRSQCFNStJCPka_IlfoJTwnt7V2xXin7C_vNhTpyg9MgSMlM2UECUGpm59Sr_eQl9ATBS_MDFb8VfEGWLv6Xgto-QCxgMFoxTEruaAsBNjYDj0a6Ueh7J3eR0c-yk; ARRAffinity=f62dc48792b6e16789f38b9331562ab71aac9ec805fac06e15282a091076b114; ARRAffinitySameSite=f62dc48792b6e16789f38b9331562ab71aac9ec805fac06e15282a091076b114");
            var content = new StringContent("{\r\n\"username\":\"yiannis@arion.gr\",\r\n\"password\":\"Rbs@123456\",\r\n\"activationCode\":\"EDA-016-936-504\"\r\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();


            //var content = new StringContent("{\r\n\"username\":\"yiannis@arion.gr\",\r\n\"password\":\"Rbs@123456\",\r\n\"activationCode\":\"EDA-016-936-504\"\r\n}", null, "application/json");
            //request.Content = content;
            //var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            string tokenBodyForRefresh = await response.Content.ReadAsStringAsync();
            content = new StringContent(tokenBodyForRefresh, null, "application/json");
            request = new HttpRequestMessage(HttpMethod.Post, "https://dev.rapidsign.com.gr/api/v1.0/provider/RefreshToken");
            request.Content = content;
            response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string tokenBody = await response.Content.ReadAsStringAsync();
            rbsToken = OOAdvantech.Json.JsonConvert.DeserializeObject<RBSToken>(tokenBody);
            return rbsToken.token;

            return null;

        }
        public class RBSToken
        {
            public string token { get; set; }
            public DateTime expires { get; set; }
        }


        public static Dictionary<string, int> VatAccounts = new Dictionary<string, int>{
            {"a1",0},
            {"b1",1},
            {"c1",2},
            {"d1",3},
            {"C54.00.70.0006",0},
            {"C54.00.70.0013",1},
            {"C54.00.70.0024",2},
            {"C54.00.70.0036",3},
            {"C54.00.70.0000",4},
            {"C54.00.79.0004",0},
            {"C54.00.79.0009",1},
            {"C54.00.79.0017",2},
            {"C54.00.79.0025",3}
        };

        //The IncomeCategoryId from /api/v{version}/provider/IncomeCategories endpoint

        public static Dictionary<string, int> IncomeCatIds = new Dictionary<string, int>{
            {"a1",1},
            {"b1",1},
            {"c1",1},
            {"d1",1},
            {"C54.00.70.0006",1},
            {"C54.00.70.0013",1},
            {"C54.00.70.0024",1},
            {"C54.00.70.0036",1},
            {"C54.00.70.0000",1},
            {"C54.00.79.0004",1},
            {"C54.00.79.0009",1},
            {"C54.00.79.0017",1},
            {"C54.00.79.0025",1}
        };
        private static RBSToken rbsToken;
    }




}




