using FinanceFacade;
using FlavourBusinessFacade;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static FlavourBusinessManager.PaymentProviders.VivaWallet;

namespace FlavourBusinessManager.PaymentProviders
{
    /// <MetaDataID>{a98904ba-1e24-4771-93ce-73a3786988d7}</MetaDataID>
    public class VivaWallet
    {
        static AccessToken AccessToken { get; set; }

        internal static void CreatePaymentOrder(IPayment payment, decimal tipAmount)
        {
            if (payment.State==PaymentState.Completed)
                return;
            PaymentOrder paymentOrderResponse = payment.GetPaymentOrder();
            if (paymentOrderResponse!=null)
            {
                if(payment.TipsAmount!=tipAmount)
                    payment.SetPaymentOrder(null);
                else if (paymentOrderResponse.expiring < DateTime.UtcNow.Ticks - TimeSpan.FromMinutes(1).Ticks)
                    payment.SetPaymentOrder(null);
                else
                {
                    payment.State = PaymentState.InProgress;
                    return;
                }
            }

            

#if DEBUG
            string clientID = "y2k7klwocvzet38u0cq3mnozcujuhu7bpdehcrmx7j1m9.apps.vivapayments.com";
            string clientSecret = "BD3oUWdc0tk3HMBA7G34dn22A9Cj5P";
#endif

            string accessToken = GetAccessToken(clientID, clientSecret);

            var client = new RestClient("https://demo-api.vivapayments.com/checkout/v2/orders");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ak_bmsc=CA754BFFAC46566A3861BDCF0EECE27B~000000000000000000000000000000~YAAQTkUVAixpd4OEAQAA1+nmzRHNStaVAzCoktrpJOEhos8yoQJWDW0eGWBxgt9FhPCUVo2jyFdpqIeDJz0Oh3oNRlB9cdliBgsrNoABb3RJ1OGTi4QsuzQPZF/I40/hCrX89gxi1VZ5bWwep/o+bWqL898ihLLP3HCJ8cupTYndVK1ni8bnjUNVcujhVij52hzHc/YKVH8ZA+FbgiCw9L2xna7jP0SZ287FnPTcKTZ1LMJpwNgOu/PBIv5QQpqoIke5REAoghpPSkYJvO568Tr57Z9oW0pnWgqDViaInl+rW0Sg0yz8Q5JghbNPFka9kxYrHLQGHwCz3zEuj8z9uzSYu6pHmxlw6ShVJO/PPs04G3aesvsszWf3s64IrxgJwq4=");

            (payment as Payment).TipsAmount=tipAmount;

            VivaPaymentOrder vivaPaymentOrder = new VivaPaymentOrder()
            {
                

                amount = (int)((payment.Amount+payment.TipsAmount) * 100),
                customerTrns = "a Short description of purchased items/services to display to your customer",
                //customer=new Customer()
                //{
                //    //email="jim.liakos@gmail.com",
                //    fullName="Jim Liakos",
                //    phone="30999999999",
                //    countryCode="GR",
                //    requestLang="el-GR"
                //},
                paymentTimeout = 300,
                preauth = false,
                allowRecurring = false,
                maxInstallments = 12,
                paymentNotification = true,
                tipAmount = (int)(payment.TipsAmount * 100),
                disableCash = true,
                disableWallet = false,
                merchantTrns = "b Short description of items/services purchased by customer",
                tags = new List<string>()
                {
                    "tags for grouping and filtering the transactions",
                    "this tag can be searched on VivaWallet sales dashboard"
                }
            };
            string body = OOAdvantech.Json.JsonConvert.SerializeObject(vivaPaymentOrder);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            paymentOrderResponse = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrder>(response.Content);
            paymentOrderResponse.expiring = (DateTime.UtcNow + TimeSpan.FromSeconds(vivaPaymentOrder.paymentTimeout)).Ticks;


            using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Required))
            {
                payment.SetPaymentOrder(paymentOrderResponse);
                payment.State = PaymentState.InProgress;

                stateTransition.Consistent = true;
            }
        }
        public class PaymentOrder
        {
            public long orderCode { get; set; }
            public long expiring { get; set; }
            public string TransactionId { get; set; }
        }

        private static string GetAccessToken(string clientID, string clientSecret)
        {
            if (AccessToken != null && (DateTime.UtcNow - AccessToken.timestamp).TotalSeconds * 0.9 < AccessToken.expires_in)
                return AccessToken.access_token;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient("https://demo-accounts.vivapayments.com/connect/token");
            //client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Basic eTJrN2tsd29jdnpldDM4dTBjcTNtbm96Y3VqdWh1N2JwZGVoY3JteDdqMW05LmFwcHMudml2YXBheW1lbnRzLmNvbTpCRDNvVVdkYzB0azNITUJBN0czNGRuMjJBOUNqNVA=");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Cookie", "ak_bmsc=6A2596A5A00DEBB5B5B99A72FBD830FA~000000000000000000000000000000~YAAQTkUVAtQIdYOEAQAAaVN2zRHsJJzXkaiLJdcpUXYMZwvK7kkfL83fs2oboq1lWZ9C/cYoXDPtO3PimvuO3IwY2EigbzozS+W+r66JMCu6ofo9v4WyqFmIADYswRE/8SUOGr6RXBDrZlIBKXQKZ3147GY/fERU8viRr1XYBQ7g5zFpvkq4reCNd+Yh7nd+K2oM3sr32fZfFvy25lOZUX0BZ59mcbfJrr4CRBNaxt6CSaLkKwwQvFRxEatW3F8M9CcAp7rt3UbCWBFheS7kVz35mkpU/voPF1iEdpBtnpJ80Py/AoKLhYZgkmJAXjqZ9RFEHRze5DEZuDubS6FFxuUfmfST9r4rqf7ryae5fFY7IudfcMGPxWXlGOjO0TgyAxI=");
            request.AddParameter("grant_type", "client_credentials");
            IRestResponse response = client.Execute(request);
            AccessToken = OOAdvantech.Json.JsonConvert.DeserializeObject<AccessToken>(response.Content);
            AccessToken.timestamp = DateTime.UtcNow;
            return AccessToken.access_token;

            //eyJhbGciOiJSUzI1NiIsImtpZCI6IjBEOEZCOEQ2RURFQ0Y1Qzk3RUY1MjdDMDYxNkJCMjMzM0FCNjVGOUZSUzI1NiIsIng1dCI6IkRZLTQxdTNzOWNsLTlTZkFZV3V5TXpxMlg1OCIsInR5cCI6ImF0K2p3dCJ9.eyJpc3MiOiJodHRwczovL2RlbW8tYWNjb3VudHMudml2YXBheW1lbnRzLmNvbSIsIm5iZiI6MTY2OTkwMDUwNiwiaWF0IjoxNjY5OTAwNTA2LCJleHAiOjE2Njk5MDQxMDYsImF1ZCI6WyJjb3JlX2FwaSIsImh0dHBzOi8vZGVtby1hY2NvdW50cy52aXZhcGF5bWVudHMuY29tL3Jlc291cmNlcyJdLCJzY29wZSI6WyJ1cm46dml2YTpwYXltZW50czpjb3JlOmFwaTphY3F1aXJpbmciLCJ1cm46dml2YTpwYXltZW50czpjb3JlOmFwaTphY3F1aXJpbmc6Y2FyZHRva2VuaXphdGlvbiIsInVybjp2aXZhOnBheW1lbnRzOmNvcmU6YXBpOnJlZGlyZWN0Y2hlY2tvdXQiXSwiY2xpZW50X2lkIjoieTJrN2tsd29jdnpldDM4dTBjcTNtbm96Y3VqdWh1N2JwZGVoY3JteDdqMW05LmFwcHMudml2YXBheW1lbnRzLmNvbSIsInVybjp2aXZhOnBheW1lbnRzOmNsaWVudF9wZXJzb25faWQiOiI1QTMzMDYyOS0wMEI3LTQ5RTctOUJFOS05QTMxN0Y3MUFGNTAifQ.MAKb3UTs7OQMkI_bhqOiHnOqHXZATFL_R5S0U6mvIFLbjRnck_urpBHDRDlWPOI1EA-tfcJlqnj-vmo6w3Av6HetjdS0urrkC1pyjh0QEWm9Hftmi0t4_SvVhQS3veshO4N9dUTguba-u0GoSlLTHpQhtz3Y6cGrDhMqKiGEc5mOKbp7Epqwvh9oi4MkZdcwCoO0_ywhFRBpQbpoXSXRXBSexMCuCzLiUAZoD3fxx7bNdIBdTAsIylNj72c09afOSl6rIdAeRb-wFF9WT4_b-Vo3xJHTp6pOF0knoliKZgDSqO-QL-UAiyeR6grVfh2WicOPUqZ6ranMVu0EhCpAsg
        }




        internal static HookRespnose WebHook(string method, string webHookName, Dictionary<string, string> headers, string content)
        {
            var hookRespnose = new HookRespnose();
            if (method == "GET")
            {
                hookRespnose.StatusCode = System.Net.HttpStatusCode.OK;
                hookRespnose.Content = @"{""key"":""1234335""}";
                hookRespnose.Headers.Add("test-header", "value");
            }
            if (method == "POST")
            {
                if (webHookName == "VivaPayment")
                {
                    var jSetttings = new OOAdvantech.Json.JsonSerializerSettings() { DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK", DateTimeZoneHandling = OOAdvantech.Json.DateTimeZoneHandling.Utc };
                    VivaEvent vivaEvent = OOAdvantech.Json.JsonConvert.DeserializeObject<VivaEvent>(content, jSetttings);


                    if (vivaEvent.EventTypeId == 1796)
                    {




                        #region Payment completed
                        var InProgressPayments = (from openSession in ServicePointRunTime.ServicesContextRunTime.Current.OpenSessions
                                                  from payment in openSession.BillingPayments
                                                  where payment.State == PaymentState.InProgress
                                                  select new { payment, paymentOrder = payment.GetPaymentOrder() }).ToList();

                        var inProgressPayment = InProgressPayments.Where(x => x.paymentOrder != null && x.paymentOrder.orderCode == vivaEvent.EventData.OrderCode).FirstOrDefault();

                        var paymentOrder = inProgressPayment.payment.GetPaymentOrder();
                        paymentOrder.TransactionId = vivaEvent.EventData.TransactionId;
                        inProgressPayment.payment.SetPaymentOrder(paymentOrder);

#if DEBUG
                        string clientID = "y2k7klwocvzet38u0cq3mnozcujuhu7bpdehcrmx7j1m9.apps.vivapayments.com";
                        string clientSecret = "BD3oUWdc0tk3HMBA7G34dn22A9Cj5P";
#endif

                        //{"orderCode":7138070025172605,"expiring":638057368377003513}

                        try
                        {

                            var client = new RestClient($"https://demo-api.vivapayments.com/checkout/v2/transactions/{paymentOrder.TransactionId}");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("Authorization", $"Bearer {GetAccessToken(clientID, clientSecret)}");
                            IRestResponse response = client.Execute(request);
                            TransactionData transactionData = OOAdvantech.Json.JsonConvert.DeserializeObject<TransactionData>(response.Content);


                            inProgressPayment.payment.CardPaymentCompleted(vivaEvent.EventData.BankId, vivaEvent.EventData.CardNumber, false, paymentOrder.TransactionId, 0);


                        }
                        catch (Exception error)
                        {

                            throw;
                        }

                        #endregion
                    }




                }

                hookRespnose.StatusCode = System.Net.HttpStatusCode.OK;
                hookRespnose.Content = @"{""message"":""ok""}";
            }

            return hookRespnose;
        }
    }


    /// <MetaDataID>{cf2173b3-2255-46c8-ac80-4722f83fa9f5}</MetaDataID>
    public class AccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public DateTime timestamp { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    /// <MetaDataID>{b57a74a6-b0f3-46f3-81d2-a1db71aefffe}</MetaDataID>
    public class Customer
    {
        public string email { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string countryCode { get; set; }
        public string requestLang { get; set; }
    }

    /// <MetaDataID>{a776e12d-0694-4049-bf39-e658fe42556e}</MetaDataID>
    public class VivaPaymentOrder
    {
        public int amount { get; set; }
        public string customerTrns { get; set; }
        public Customer customer { get; set; }
        public int paymentTimeout { get; set; }
        public bool preauth { get; set; }
        public bool allowRecurring { get; set; }
        public int maxInstallments { get; set; }
        public bool paymentNotification { get; set; }
        public int tipAmount { get; set; }
        public bool disableExactAmount { get; set; }
        public bool disableCash { get; set; }
        public bool disableWallet { get; set; }
        public string merchantTrns { get; set; }
        public List<string> tags { get; set; }
        public List<object> cardTokens { get; set; }
    }


    /// <MetaDataID>{2d3f8aa4-5414-46e0-ae39-8d26255d69f9}</MetaDataID>
    public class EventData
    {
        public bool Moto { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BankId { get; set; }
        public bool Systemic { get; set; }
        public bool Switching { get; set; }
        public object ParentId { get; set; }
        public int Amount { get; set; }
        public string ChannelId { get; set; }
        public int TerminalId { get; set; }
        public string MerchantId { get; set; }
        public long OrderCode { get; set; }
        public string ProductId { get; set; }
        public string StatusId { get; set; }
        public string FullName { get; set; }
        public string ResellerId { get; set; }
        public DateTime InsDate { get; set; }
        public int TotalFee { get; set; }
        public string CardUniqueReference { get; set; }
        public string CardToken { get; set; }
        public string CardNumber { get; set; }
        public int TipAmount { get; set; }
        public string SourceCode { get; set; }
        public string SourceName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string CompanyName { get; set; }
        public string TransactionId { get; set; }
        public string CompanyTitle { get; set; }
        public string PanEntryMode { get; set; }
        public int ReferenceNumber { get; set; }
        public string ResponseCode { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderCulture { get; set; }
        public string MerchantTrns { get; set; }
        public string CustomerTrns { get; set; }
        public bool IsManualRefund { get; set; }
        public string TargetPersonId { get; set; }
        public string TargetWalletId { get; set; }
        public bool LoyaltyTriggered { get; set; }
        public int TransactionTypeId { get; set; }
        public int TotalInstallments { get; set; }
        public string CardCountryCode { get; set; }
        public string CardIssuingBank { get; set; }
        public int RedeemedAmount { get; set; }
        public object ClearanceDate { get; set; }
        public int CurrentInstallment { get; set; }
        public List<string> Tags { get; set; }
        public string BillId { get; set; }
        public string ResellerSourceCode { get; set; }
        public string ResellerSourceName { get; set; }
        public string ResellerCompanyName { get; set; }
        public string ResellerSourceAddress { get; set; }
        public DateTime CardExpirationDate { get; set; }
        public string RetrievalReferenceNumber { get; set; }
        public List<object> AssignedMerchantUsers { get; set; }
        public List<object> AssignedResellerUsers { get; set; }
        public int CardTypeId { get; set; }
        public int DigitalWalletId { get; set; }
        public string ResponseEventId { get; set; }
        public string ElectronicCommerceIndicator { get; set; }
    }

    /// <MetaDataID>{0f4eb427-24c5-40b8-b849-5ff6405a51d2}</MetaDataID>
    public class VivaEvent
    {
        public string Url { get; set; }
        public EventData EventData { get; set; } = new EventData();
        public DateTime Created { get; set; }
        public string CorrelationId { get; set; }
        public int EventTypeId { get; set; }
        public string Delay { get; set; }
        public string MessageId { get; set; }
        public string RecipientId { get; set; }
        public int MessageTypeId { get; set; }
    }

    /// <MetaDataID>{9977de77-c743-4074-8e65-61eff156cdc7}</MetaDataID>
    public static class VivaIPaymentExtMethods
    {
        public static PaymentOrder GetPaymentOrder(this IPayment payment)
        {
            if (string.IsNullOrEmpty(payment?.PaymentProviderJson))
                return null;

            var paymentOrderResponse = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrder>(payment.PaymentProviderJson);
            return paymentOrderResponse;
        }

        public static void SetPaymentOrder(this IPayment payment, PaymentOrder paymentOrder)
        {
            if (paymentOrder!=null)
                payment.PaymentProviderJson = OOAdvantech.Json.JsonConvert.SerializeObject(paymentOrder);
            else
                payment.PaymentProviderJson= null;

        }

    }


    /// <MetaDataID>{b00a81fe-a55d-488c-80c3-ff61e5793da5}</MetaDataID>
    public class TransactionData
    {
        public string email { get; set; }
        public double amount { get; set; }
        public long orderCode { get; set; }
        public string statusId { get; set; }
        public string fullName { get; set; }
        public DateTime insDate { get; set; }
        public string cardNumber { get; set; }
        public string currencyCode { get; set; }
        public string customerTrns { get; set; }
        public string merchantTrns { get; set; }
        public string cardUniqueReference { get; set; }
        public int transactionTypeId { get; set; }
        public bool recurringSupport { get; set; }
        public int totalInstallments { get; set; }
        public object cardCountryCode { get; set; }
        public object cardIssuingBank { get; set; }
        public int currentInstallment { get; set; }
        public int cardTypeId { get; set; }
    }


}
