using FinanceFacade;
using FlavourBusinessFacade;
using OOAdvantech.Remoting.RestApi;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace PaymentProviders
{
    /// <MetaDataID>{a98904ba-1e24-4771-93ce-73a3786988d7}</MetaDataID>
    public class VivaWallet : IPaymentProvider
    {
        /// <MetaDataID>{6fabfadb-1527-4ba8-92d2-e555de55f5d8}</MetaDataID>
        static AccessToken AccessToken { get; set; }

        /// <MetaDataID>{fd4a9530-d7ce-47d2-8181-1eee214705e9}</MetaDataID>
        public static void CreatePaymentOrder(Payment payment, decimal tipAmount)
        {

            if (payment.State == PaymentState.Completed)
                return;
            PaymentOrder paymentOrderResponse = payment.GetPaymentOrder();
            if (paymentOrderResponse != null)
            {
                if (payment.TipsAmount != tipAmount)
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

            (payment as Payment).TipsAmount = tipAmount;

            VivaPaymentOrder vivaPaymentOrder = new VivaPaymentOrder()
            {


                amount = (int)((payment.Amount + payment.TipsAmount) * 100),
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
                payment.PaymentGetwayID = "Viva";
                payment.State = PaymentState.InProgress;

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{d23d89fb-6134-4df6-b5f4-04cec6d1564f}</MetaDataID>
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

        /// <MetaDataID>{7b530fcc-aa21-4e8d-a1d3-744b119e21d4}</MetaDataID>
        private static string GetWebHookKeyJson(string merchantID, string apiKey)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient("https://demo.vivapayments.com/api/messages/config/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Basic NWEzMzA2MjktMDBiNy00OWU3LTliZTktOWEzMTdmNzFhZjUwOkNPck1DNQ==");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Cookie", "_abck=0ED3FFBA7C810CF465BE0762C72C5813~-1~YAAQTkUVAiISuYOEAQAAbKb73AnMlPRPpe1EZ5nNfHvbfqJF4T5FKX3WKkyFop5jcVyxGb/TSwKraDvjz6NOspK9TACi8SzCfr3p9//L8zBqG83sRem1HaIt43d67u1G9xWTJR83Z/YK42WvPzsX/i2GYjQ1Nf+ATy2EbK6oJkibfm41jgwezwChjGV60ih/+Wpf+R8bqJVq946oXLeqNWYDyNC1Cyg4kx+82nVesGN3lafeTEWKVvTftRhzWBYbYKpXeRQmd3upxmF75xFUwDOkXAf5t6Z7asof7CE2wG5SgDZyT7WwQQO9SUt75IepZNXTsyxbWf06vWftytD6LA2guju3KkETknnH4rD4dKRYo6psPNkEt6Kmhsvp+cc=~-1~-1~-1; ak_bmsc=ECD3803817876A1A34051AF609EEEF70~000000000000000000000000000000~YAAQVHtlX1244M2FAQAABOIbDBKsPTQdWZ6cJwNn5LCOjfA5LUOQp6C86zumLu1E6nyNKcblPfmQ3nUvkk7PmFhopRSWv+4eXy1ZVUGycSjAO9RJG7Xr8RL3joLxWGZWxK+Z4bFfB3RqdZRPzT9A0zAdL/tw0bZK07W19/YZajjLpEFA7+uLyiGDbiv2vKjgGfIVxE9+ecsdZApviDaWQz+9yJ5H125h6KS4Y+xYiKKTGtuSdGDp2OHJXw6WCNWn6k0ZuM+tx1WZ0BKGNleI3TpHBxjooT8XvNOxiILkEaYjfoSgIOkCgDE1h3p1jUb5xXenJ1s2W+lwSSlKhC6Lauasoer+1XVWO2sDZ1X3n1QZ9DeZhEENVkvDjalIee/Io186");
            request.AddParameter("grant_type", "client_credentials");
            IRestResponse response = client.Execute(request);

            return response.Content;
        }






        /// <MetaDataID>{abf594e0-bcdf-45b1-ab07-962a4aec5289}</MetaDataID>
        public HookRespnose WebHook(string method, string webHookName, Dictionary<string, string> headers, string content)
        {

            //  System.Threading.Thread.Sleep(30000);
#if DEBUG
            string clientID = "y2k7klwocvzet38u0cq3mnozcujuhu7bpdehcrmx7j1m9.apps.vivapayments.com";
            string clientSecret = "BD3oUWdc0tk3HMBA7G34dn22A9Cj5P";
            string merchantID = "5a330629-00b7-49e7-9be9-9a317f71af50";
            string apiKey = "COrMC5";
            string vivaPaymentGateWayAccountUrl = "https://demo.vivapayments.com";
            string vivaPaymentGateWayApiUrl = "https://demo-api.vivapayments.com";

#endif

            var hookRespnose = new HookRespnose();
            if (method == "GET")
            {
                bool th = false;
                if (th)
                {
                    hookRespnose.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    hookRespnose.Content = @"eror";
                    return hookRespnose;
                }

                string webHookKeyResponse = GetWebHookKeyJson(merchantID, apiKey);
                hookRespnose.StatusCode = System.Net.HttpStatusCode.OK;
                hookRespnose.Content = webHookKeyResponse;// @"{""key"":""1234335""}";
                hookRespnose.Headers.Add("test-header", "value");
            }
            if (method == "POST")
            {



                bool th = false;
                if (th)
                {
                    hookRespnose.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    hookRespnose.Content = @"eror";
                    return hookRespnose;
                }


                var jSetttings = new OOAdvantech.Json.JsonSerializerSettings() { DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK", DateTimeZoneHandling = OOAdvantech.Json.DateTimeZoneHandling.Utc };
                VivaEvent vivaEvent = OOAdvantech.Json.JsonConvert.DeserializeObject<VivaEvent>(content, jSetttings);
                var InsDate = vivaEvent.EventData.InsDate;

                if (vivaEvent.EventTypeId == 1796)
                {

                    #region Payment completed
                    var inProgressPayment = Payment.PaymentFinder.FindPayment("Viva", vivaEvent.EventData.OrderCode.ToString());

                    if (inProgressPayment != null)
                    {
                        var paymentOrder = inProgressPayment.GetPaymentOrder();
                        paymentOrder.TransactionId = vivaEvent.EventData.TransactionId;
                        inProgressPayment.SetPaymentOrder(paymentOrder);



                        try
                        {

                            var client = new RestClient($"{vivaPaymentGateWayApiUrl}/checkout/v2/transactions/{paymentOrder.TransactionId}");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("Authorization", $"Bearer {GetAccessToken(clientID, clientSecret)}");
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                TransactionData transactionData = OOAdvantech.Json.JsonConvert.DeserializeObject<TransactionData>(response.Content);
                                //inProgressPayment.Subject.PaymentCompleted()
                                inProgressPayment.CardPaymentCompleted(vivaEvent.EventData.BankId, vivaEvent.EventData.CardNumber, false, paymentOrder.TransactionId, 0);
                            }

                        }
                        catch (Exception error)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        //hookRespnose.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                        //hookRespnose.Content = @"";
                    }

                    #endregion
                }




                hookRespnose.StatusCode = System.Net.HttpStatusCode.OK;
                hookRespnose.Content = @"{""message"":""ok""}";
            }

            return hookRespnose;
        }

        /// <MetaDataID>{c8147d58-7c28-490e-a591-dfcd1da95aa6}</MetaDataID>
        public void CheckPaymentProgress(IPayment payment)
        {
            var paymentOrder = payment.GetPaymentOrder();
            if (!string.IsNullOrWhiteSpace(paymentOrder.TransactionId))
            {
                string clientID = "y2k7klwocvzet38u0cq3mnozcujuhu7bpdehcrmx7j1m9.apps.vivapayments.com";
                string clientSecret = "BD3oUWdc0tk3HMBA7G34dn22A9Cj5P";
                string merchantID = "5a330629-00b7-49e7-9be9-9a317f71af50";
                string apiKey = "COrMC5";
                string vivaPaymentGateWayAccountUrl = "https://demo.vivapayments.com";
                string vivaPaymentGateWayApiUrl = "https://demo-api.vivapayments.com";


                //var inProgressPayment = InProgressPayments.Where(x => x.paymentOrder != null && x.paymentOrder.orderCode == paymentOrder.orderCode).FirstOrDefault();
                // var inProgressPayment = Payment.PaymentFinder.FindPayment("Viva", vivaEvent.EventData.OrderCode.ToString());




                var client = new RestClient($"{vivaPaymentGateWayApiUrl}/checkout/v2/transactions/{paymentOrder.TransactionId}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {GetAccessToken(clientID, clientSecret)}");
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    TransactionData transactionData = OOAdvantech.Json.JsonConvert.DeserializeObject<TransactionData>(response.Content);
                    payment.CardPaymentCompleted(null, transactionData.cardNumber, false, paymentOrder.TransactionId, 0);
                }
            }
        }
    }


    /// <MetaDataID>{cf2173b3-2255-46c8-ac80-4722f83fa9f5}</MetaDataID>
    class AccessToken
    {
        /// <MetaDataID>{6e4721de-cb7c-488c-b634-e6a2e26f360f}</MetaDataID>
        public string access_token { get; set; }
        /// <MetaDataID>{4b12be47-03f0-4253-a5bf-44062e6e2016}</MetaDataID>
        public int expires_in { get; set; }
        /// <MetaDataID>{9b57cdb5-28a6-4335-9ebe-b61b4717bd59}</MetaDataID>
        public string token_type { get; set; }
        /// <MetaDataID>{761ba1bc-2a60-40d6-abdd-4838cd252c25}</MetaDataID>
        public string scope { get; set; }
        /// <MetaDataID>{bd19dcb8-3282-499d-8c89-ed2547e5f7b5}</MetaDataID>
        public DateTime timestamp { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    /// <MetaDataID>{b57a74a6-b0f3-46f3-81d2-a1db71aefffe}</MetaDataID>
    class Customer
    {
        /// <MetaDataID>{1519f400-eecf-46f4-b99e-5d75606144c7}</MetaDataID>
        public string email { get; set; }
        /// <MetaDataID>{4a1ba893-9775-44b6-b89d-f03f4a81200e}</MetaDataID>
        public string fullName { get; set; }
        /// <MetaDataID>{8da23b07-fdd8-48d0-8362-c011280904b5}</MetaDataID>
        public string phone { get; set; }
        /// <MetaDataID>{569d27f0-2b33-46ae-b2fa-48a6bab71d50}</MetaDataID>
        public string countryCode { get; set; }
        /// <MetaDataID>{70d6c172-1015-4be2-b32f-cb0a6e7b1389}</MetaDataID>
        public string requestLang { get; set; }
    }

    /// <MetaDataID>{a776e12d-0694-4049-bf39-e658fe42556e}</MetaDataID>
    class VivaPaymentOrder
    {
        /// <MetaDataID>{ee43afe0-05c2-4f5c-8c3a-ac43c63160ae}</MetaDataID>
        public int amount { get; set; }
        /// <MetaDataID>{305fddab-0bf6-49cd-955a-afdcef76669a}</MetaDataID>
        public string customerTrns { get; set; }
        /// <MetaDataID>{ae9f91ff-5d74-46db-9ea9-62bfb6911256}</MetaDataID>
        public Customer customer { get; set; }
        /// <MetaDataID>{ac84f395-450e-42e3-9e3e-a0683cb06605}</MetaDataID>
        public int paymentTimeout { get; set; }
        /// <MetaDataID>{2688e73d-45ae-46b2-8f3f-1acdd827c331}</MetaDataID>
        public bool preauth { get; set; }
        /// <MetaDataID>{788ece47-28fa-4ed9-b1a3-cf50ba71191b}</MetaDataID>
        public bool allowRecurring { get; set; }
        /// <MetaDataID>{9b743d5b-ae6d-484b-aab1-d5de4b0df6f7}</MetaDataID>
        public int maxInstallments { get; set; }
        /// <MetaDataID>{628b2da6-4a4d-42c7-9523-1aeb076a81cc}</MetaDataID>
        public bool paymentNotification { get; set; }
        /// <MetaDataID>{4ce96386-d08a-4ff3-b1dc-25c995c42a67}</MetaDataID>
        public int tipAmount { get; set; }
        /// <MetaDataID>{35f569ab-bed7-4beb-9a3c-d721b5995713}</MetaDataID>
        public bool disableExactAmount { get; set; }
        /// <MetaDataID>{388ac15f-2b3f-401b-8c65-acb5e8aaca7d}</MetaDataID>
        public bool disableCash { get; set; }
        /// <MetaDataID>{45c08ec3-59c1-48c4-8476-d898b3df98c5}</MetaDataID>
        public bool disableWallet { get; set; }
        /// <MetaDataID>{a7bb72e9-6cbf-4225-ba2e-c1b9610176c7}</MetaDataID>
        public string merchantTrns { get; set; }
        /// <MetaDataID>{403b8f84-fb4b-44e7-84d7-af2f013e5c83}</MetaDataID>
        public List<string> tags { get; set; }
        /// <MetaDataID>{35014ab4-0f68-490d-8639-9d121ad8c880}</MetaDataID>
        public List<object> cardTokens { get; set; }
    }


    ///// <MetaDataID>{2d3f8aa4-5414-46e0-ae39-8d26255d69f9}</MetaDataID>
    //public class EventData
    //{
    //    public bool Moto { get; set; }
    //    public string Email { get; set; }
    //    public string Phone { get; set; }
    //    public string BankId { get; set; }
    //    public bool Systemic { get; set; }
    //    public bool Switching { get; set; }
    //    public object ParentId { get; set; }
    //    public int Amount { get; set; }
    //    public string ChannelId { get; set; }
    //    public int TerminalId { get; set; }
    //    public string MerchantId { get; set; }
    //    public long OrderCode { get; set; }
    //    public string ProductId { get; set; }
    //    public string StatusId { get; set; }
    //    public string FullName { get; set; }
    //    public string ResellerId { get; set; }
    //    public DateTime InsDate { get; set; }
    //    public int TotalFee { get; set; }
    //    public string CardUniqueReference { get; set; }
    //    public string CardToken { get; set; }
    //    public string CardNumber { get; set; }
    //    public int TipAmount { get; set; }
    //    public string SourceCode { get; set; }
    //    public string SourceName { get; set; }
    //    public decimal Latitude { get; set; }
    //    public decimal Longitude { get; set; }
    //    public string CompanyName { get; set; }
    //    public string TransactionId { get; set; }
    //    public string CompanyTitle { get; set; }
    //    public string PanEntryMode { get; set; }
    //    public int ReferenceNumber { get; set; }
    //    public string ResponseCode { get; set; }
    //    public string CurrencyCode { get; set; }
    //    public string OrderCulture { get; set; }
    //    public string MerchantTrns { get; set; }
    //    public string CustomerTrns { get; set; }
    //    public bool IsManualRefund { get; set; }
    //    public string TargetPersonId { get; set; }
    //    public string TargetWalletId { get; set; }
    //    public bool LoyaltyTriggered { get; set; }
    //    public int TransactionTypeId { get; set; }
    //    public int TotalInstallments { get; set; }
    //    public string CardCountryCode { get; set; }
    //    public string CardIssuingBank { get; set; }
    //    public int RedeemedAmount { get; set; }
    //    public object ClearanceDate { get; set; }
    //    public int CurrentInstallment { get; set; }
    //    public List<string> Tags { get; set; }
    //    public string BillId { get; set; }
    //    public string ResellerSourceCode { get; set; }
    //    public string ResellerSourceName { get; set; }
    //    public string ResellerCompanyName { get; set; }
    //    public string ResellerSourceAddress { get; set; }
    //    public DateTime CardExpirationDate { get; set; }
    //    public string RetrievalReferenceNumber { get; set; }
    //    public List<object> AssignedMerchantUsers { get; set; }
    //    public List<object> AssignedResellerUsers { get; set; }
    //    public int CardTypeId { get; set; }
    //    public int DigitalWalletId { get; set; }
    //    public string ResponseEventId { get; set; }
    //    public string ElectronicCommerceIndicator { get; set; }
    //}

    ///// <MetaDataID>{0f4eb427-24c5-40b8-b849-5ff6405a51d2}</MetaDataID>
    //public class VivaEvent
    //{
    //    public string Url { get; set; }
    //    public EventData EventData { get; set; } = new EventData();
    //    public DateTime Created { get; set; }
    //    public string CorrelationId { get; set; }
    //    public int EventTypeId { get; set; }
    //    public string Delay { get; set; }
    //    public string MessageId { get; set; }
    //    public string RecipientId { get; set; }
    //    public int MessageTypeId { get; set; }
    //}

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    /// <MetaDataID>{25aaf5fe-7d39-43d7-943f-66472a998bac}</MetaDataID>
    class EventData
    {
        /// <MetaDataID>{41959cd0-cfb9-49a1-9a2d-d0afca442cb3}</MetaDataID>
        public bool Moto { get; set; }
        /// <MetaDataID>{d8b3ea4e-ed1a-466e-9d2d-68ef389616df}</MetaDataID>
        public string Ucaf { get; set; }
        /// <MetaDataID>{0bb12877-4bc0-4b77-bead-e94debe6fb7e}</MetaDataID>
        public string Email { get; set; }
        /// <MetaDataID>{99a8db55-1308-4a1b-8522-024570408780}</MetaDataID>
        public object Phone { get; set; }
        /// <MetaDataID>{886954ff-6f8b-4204-9c54-5e08bd1a0641}</MetaDataID>
        public string BankId { get; set; }
        /// <MetaDataID>{cb53752f-3153-4291-a48a-5f24e984edf2}</MetaDataID>
        public bool Systemic { get; set; }
        /// <MetaDataID>{24a99e8a-28be-4b94-ad8c-831ec038d1f5}</MetaDataID>
        public bool Switching { get; set; }
        /// <MetaDataID>{2ad84d90-9fd1-4c10-b8b4-c9ccde5dd266}</MetaDataID>
        public object ParentId { get; set; }
        /// <MetaDataID>{a36f6623-267e-445d-86ab-a2554f87c5e2}</MetaDataID>
        public double Amount { get; set; }
        /// <MetaDataID>{2340d3bb-40d9-4947-ba6a-4864532f2166}</MetaDataID>
        public string ChannelId { get; set; }
        /// <MetaDataID>{72b33d61-97bb-4e05-89a5-eae33fd25caf}</MetaDataID>
        public int TerminalId { get; set; }
        /// <MetaDataID>{626ecdc4-2f2c-41bd-937c-6ce90d0212ac}</MetaDataID>
        public string MerchantId { get; set; }
        /// <MetaDataID>{4375872d-157d-4ce4-ba82-90359928d92f}</MetaDataID>
        public long OrderCode { get; set; }
        /// <MetaDataID>{cc9dcea7-f0c9-4c33-a967-bdf2e9369995}</MetaDataID>
        public object ProductId { get; set; }
        /// <MetaDataID>{bf433a6a-6661-45b2-bab6-9579b973dcf1}</MetaDataID>
        public string StatusId { get; set; }
        /// <MetaDataID>{557a0678-e9fa-4436-b580-a7d576117e96}</MetaDataID>
        public string FullName { get; set; }
        /// <MetaDataID>{1e154e6b-d290-4149-a664-0a6e7fa75a18}</MetaDataID>
        public object ResellerId { get; set; }
        /// <MetaDataID>{81aa7aea-27c3-44ce-b7b0-277251ce7393}</MetaDataID>
        public bool DualMessage { get; set; }
        /// <MetaDataID>{bb872521-c87c-456f-8a54-3e80c8ddedc5}</MetaDataID>
        public DateTime InsDate { get; set; }
        /// <MetaDataID>{36d6c169-d052-4440-afe6-ba12c0054767}</MetaDataID>
        public double TotalFee { get; set; }
        /// <MetaDataID>{81fd7f44-948b-4d7e-b4b4-f354202bfa66}</MetaDataID>
        public string CardToken { get; set; }
        /// <MetaDataID>{b4d7adda-f3e6-4ad6-ad5d-be944940b9ef}</MetaDataID>
        public string CardNumber { get; set; }
        /// <MetaDataID>{a8f6df8a-90c5-4feb-9a1a-d41988f1ba99}</MetaDataID>
        public double TipAmount { get; set; }
        /// <MetaDataID>{dc570d87-69ae-46de-b737-337089000da1}</MetaDataID>
        public string SourceCode { get; set; }
        /// <MetaDataID>{848ab1c8-8950-422c-a5b2-2a34fa6a13ab}</MetaDataID>
        public string SourceName { get; set; }
        /// <MetaDataID>{0b1d9e74-c122-42ab-934a-1f9a99d09e35}</MetaDataID>
        public object Latitude { get; set; }
        /// <MetaDataID>{c1610e0d-0186-425d-bc05-655f1efedd89}</MetaDataID>
        public object Longitude { get; set; }
        /// <MetaDataID>{e91fe54f-7eab-412a-8555-5acea4cfcf08}</MetaDataID>
        public object CompanyName { get; set; }
        /// <MetaDataID>{4903b219-c9d0-45f3-a46a-cda53fcb2376}</MetaDataID>
        public string TransactionId { get; set; }
        /// <MetaDataID>{b9c275a2-4451-44c1-bd2f-2ab8e9412f80}</MetaDataID>
        public object CompanyTitle { get; set; }
        /// <MetaDataID>{3fc37aea-b4f4-41cd-9c11-832bbb39f243}</MetaDataID>
        public string PanEntryMode { get; set; }
        /// <MetaDataID>{016eb332-1d3d-4953-be3e-05ddc3d3dce7}</MetaDataID>
        public int ReferenceNumber { get; set; }
        /// <MetaDataID>{d17978cf-174a-4e95-8cc3-fff0ef95b6bf}</MetaDataID>
        public string ResponseCode { get; set; }
        /// <MetaDataID>{41f4f26d-6984-4040-915b-986b2a451b90}</MetaDataID>
        public string CurrencyCode { get; set; }
        /// <MetaDataID>{efeed03a-f110-4569-ad06-c23c0f5bd0eb}</MetaDataID>
        public string OrderCulture { get; set; }
        /// <MetaDataID>{e1b2e35f-2435-45e8-b653-631614e32e89}</MetaDataID>
        public string MerchantTrns { get; set; }
        /// <MetaDataID>{3878826f-64a5-40e4-a890-b7aded5d29e0}</MetaDataID>
        public string CustomerTrns { get; set; }
        /// <MetaDataID>{eb8352d6-292f-450f-940f-e084efc8ad04}</MetaDataID>
        public bool IsManualRefund { get; set; }
        /// <MetaDataID>{7b94409d-e6d3-41ec-af4b-726bf287a1c0}</MetaDataID>
        public object TargetPersonId { get; set; }
        /// <MetaDataID>{344f975e-a081-45a6-8278-2339c1281e8f}</MetaDataID>
        public object TargetWalletId { get; set; }
        /// <MetaDataID>{d21e4861-dbcf-465a-97c6-64eea89a1649}</MetaDataID>
        public bool AcquirerApproved { get; set; }
        /// <MetaDataID>{28755ab3-04bc-4b5e-9fdf-f8f66ab5dce8}</MetaDataID>
        public bool LoyaltyTriggered { get; set; }
        /// <MetaDataID>{a0daf2cd-3f72-448b-a606-5f072ecdac3a}</MetaDataID>
        public int TransactionTypeId { get; set; }
        /// <MetaDataID>{f0fc0a85-a89e-420a-9485-1d18f556cd7c}</MetaDataID>
        public string AuthorizationId { get; set; }
        /// <MetaDataID>{6bed1b6b-593c-45b3-9f7d-aefd1e71806c}</MetaDataID>
        public int TotalInstallments { get; set; }
        /// <MetaDataID>{6fa8a3c1-a4c3-4fe4-ae2b-48a78c55db35}</MetaDataID>
        public string CardCountryCode { get; set; }
        /// <MetaDataID>{1493aad1-5192-4e02-aba8-cafd4240ff61}</MetaDataID>
        public string CardIssuingBank { get; set; }
        /// <MetaDataID>{013976b6-967c-4f57-8c7e-41d4e9f62743}</MetaDataID>
        public double RedeemedAmount { get; set; }
        /// <MetaDataID>{0e694b81-dfa6-4e76-9d56-5b89be9952d1}</MetaDataID>
        public object ClearanceDate { get; set; }
        /// <MetaDataID>{5e5a133b-2254-44f4-9d3d-3dbb0ac00aeb}</MetaDataID>
        public int CurrentInstallment { get; set; }
        /// <MetaDataID>{d0a9d928-eee4-4d0d-bf1c-2248a5f920a1}</MetaDataID>
        public List<string> Tags { get; set; }
        /// <MetaDataID>{310d594c-85e5-4b23-941b-13df6d22850d}</MetaDataID>
        public object BillId { get; set; }
        /// <MetaDataID>{dca65bdd-1b8f-49c1-8bfd-3b8651e93287}</MetaDataID>
        public object ResellerSourceCode { get; set; }
        /// <MetaDataID>{3d608436-2c4f-4c5f-a312-fd4d131e515e}</MetaDataID>
        public object ResellerSourceName { get; set; }
        /// <MetaDataID>{c6132c9a-de91-4103-a95c-5297d5a4cdd8}</MetaDataID>
        public object ResellerCompanyName { get; set; }
        /// <MetaDataID>{c27cc32d-d943-4b57-9a13-662250e4ef47}</MetaDataID>
        public string CardUniqueReference { get; set; }
        /// <MetaDataID>{3b0087f2-034c-4650-b3ac-ba0bcf8022f0}</MetaDataID>
        public object ResellerSourceAddress { get; set; }
        /// <MetaDataID>{8d2aa91f-18c5-4330-bd29-7c4667fd1d6a}</MetaDataID>
        public DateTime CardExpirationDate { get; set; }
        /// <MetaDataID>{e9093bcb-3ec2-4fb3-a537-ba7651fbf641}</MetaDataID>
        public string RetrievalReferenceNumber { get; set; }
        /// <MetaDataID>{287b5cee-8e87-4da0-a0df-3eb5a5bb9b6a}</MetaDataID>
        public List<object> AssignedMerchantUsers { get; set; }
        /// <MetaDataID>{e0640f5b-31de-4810-b520-e1ff472d1141}</MetaDataID>
        public List<object> AssignedResellerUsers { get; set; }
        /// <MetaDataID>{1711230a-9f19-427e-8120-29f3920585d1}</MetaDataID>
        public int CardTypeId { get; set; }
        /// <MetaDataID>{1ca94b82-a924-4e9d-908c-9a1d57dafba3}</MetaDataID>
        public object ResponseEventId { get; set; }
        /// <MetaDataID>{15b1cbbc-de67-420e-984c-515e03da33d2}</MetaDataID>
        public string ElectronicCommerceIndicator { get; set; }
        /// <MetaDataID>{85dfe965-a58f-4b8b-8895-0b1cc28beffd}</MetaDataID>
        public int OrderServiceId { get; set; }
        /// <MetaDataID>{1c735beb-e52b-4cad-97cf-54ab0f274428}</MetaDataID>
        public object DigitalWalletId { get; set; }
    }

    /// <MetaDataID>{55d05b0f-5355-4370-b493-f4cbbb7d9425}</MetaDataID>
    class VivaEvent
    {
        /// <MetaDataID>{b9f760c2-3fe8-4e96-acd5-130a402b85f9}</MetaDataID>
        public string Url { get; set; }
        /// <MetaDataID>{33ee0d70-53fd-48e6-bb2c-1763ee6dbc09}</MetaDataID>
        public EventData EventData { get; set; }
        /// <MetaDataID>{75c8c21b-a3a4-42e5-b878-ad02adf50f8f}</MetaDataID>
        public DateTime Created { get; set; }
        /// <MetaDataID>{3c30a236-5cb8-4105-a488-8334b190c597}</MetaDataID>
        public string CorrelationId { get; set; }
        /// <MetaDataID>{8f436365-33de-4a16-9108-602fa45608cf}</MetaDataID>
        public int EventTypeId { get; set; }
        /// <MetaDataID>{e89c0ebc-7ab1-4f6e-a336-b93b5786f322}</MetaDataID>
        public object Delay { get; set; }
        /// <MetaDataID>{f41ccf29-cd36-4245-89ed-fde46d72f917}</MetaDataID>
        public string MessageId { get; set; }
        /// <MetaDataID>{4ba5bcd4-50b5-4dd6-b1cf-cbfce7d98095}</MetaDataID>
        public string RecipientId { get; set; }
        /// <MetaDataID>{f4135eff-8546-41fd-a017-2d0fc533e324}</MetaDataID>
        public int MessageTypeId { get; set; }
    }





    /// <MetaDataID>{b00a81fe-a55d-488c-80c3-ff61e5793da5}</MetaDataID>
    class TransactionData
    {
        /// <MetaDataID>{02b84baa-8cd0-42a3-a367-57590c9b5436}</MetaDataID>
        public string email { get; set; }
        /// <MetaDataID>{562bc11a-1255-4cc7-9b63-dd5ff2c0def1}</MetaDataID>
        public double amount { get; set; }
        /// <MetaDataID>{a0df10b8-1e39-4dc9-a906-0b767b19ca37}</MetaDataID>
        public long orderCode { get; set; }
        /// <MetaDataID>{95d7d9c3-f43d-4373-8713-4dbcb516f641}</MetaDataID>
        public string statusId { get; set; }
        /// <MetaDataID>{d4be5914-94f9-4245-bc32-963933d3784b}</MetaDataID>
        public string fullName { get; set; }
        /// <MetaDataID>{8770911e-8f48-4962-b402-39a6c105f7b1}</MetaDataID>
        public DateTime insDate { get; set; }
        /// <MetaDataID>{57637720-19b3-4063-8a51-e12a07e6371e}</MetaDataID>
        public string cardNumber { get; set; }
        /// <MetaDataID>{c25517b5-e1c4-48f2-8d9e-147782c07839}</MetaDataID>
        public string currencyCode { get; set; }
        /// <MetaDataID>{e2fd6a81-8cdd-431b-b298-334ba846ecf3}</MetaDataID>
        public string customerTrns { get; set; }
        /// <MetaDataID>{34d7ae67-cb04-48b5-a8b7-3850cb7a7b7a}</MetaDataID>
        public string merchantTrns { get; set; }
        /// <MetaDataID>{19fb1e80-de91-4e49-a539-36e14664dd37}</MetaDataID>
        public string cardUniqueReference { get; set; }
        /// <MetaDataID>{d27f21ea-bc93-4afb-bbda-edd0ffd7263f}</MetaDataID>
        public int transactionTypeId { get; set; }
        /// <MetaDataID>{9fac4e08-e5d7-4260-b5d7-08be7bfd0e9c}</MetaDataID>
        public bool recurringSupport { get; set; }
        /// <MetaDataID>{08cae485-a3f4-420f-bb52-5d0e9f378f46}</MetaDataID>
        public int totalInstallments { get; set; }
        /// <MetaDataID>{48b75351-3a5d-4e86-9e49-ced74a9592cc}</MetaDataID>
        public object cardCountryCode { get; set; }
        /// <MetaDataID>{1943af58-a10c-4053-923f-a21cf4a6e629}</MetaDataID>
        public object cardIssuingBank { get; set; }
        /// <MetaDataID>{13e9e7be-5d7c-4d4f-a587-fbec7c06400f}</MetaDataID>
        public int currentInstallment { get; set; }
        /// <MetaDataID>{4a17b2f9-8c6a-40e8-bd7c-51521289dc6b}</MetaDataID>
        public int cardTypeId { get; set; }
    }



}
