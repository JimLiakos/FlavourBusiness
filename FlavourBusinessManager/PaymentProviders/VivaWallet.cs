using FinanceFacade;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.PaymentProviders
{
    public class VivaWallet
    {
        static AccessToken AccessToken { get; set; }

        internal static void CreatePaymentOrder(Payment payment)
        {
            PaymentOrderResponse paymentOrderResponse = null;
            if (!string.IsNullOrWhiteSpace(payment.PaymentProviderJson))
            {
                paymentOrderResponse = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrderResponse>(payment.PaymentProviderJson);

                if (paymentOrderResponse.expiring<DateTime.UtcNow.Ticks-TimeSpan.FromMinutes(1).Ticks)
                    payment.PaymentProviderJson=null;
                else
                    return;
            }



            string clientID = "y2k7klwocvzet38u0cq3mnozcujuhu7bpdehcrmx7j1m9.apps.vivapayments.com";
            string clientSecret = "BD3oUWdc0tk3HMBA7G34dn22A9Cj5P";

            string accessToken = GetAccessToken(clientID, clientSecret);
            var client = new RestClient("https://demo-api.vivapayments.com/checkout/v2/orders");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ak_bmsc=CA754BFFAC46566A3861BDCF0EECE27B~000000000000000000000000000000~YAAQTkUVAixpd4OEAQAA1+nmzRHNStaVAzCoktrpJOEhos8yoQJWDW0eGWBxgt9FhPCUVo2jyFdpqIeDJz0Oh3oNRlB9cdliBgsrNoABb3RJ1OGTi4QsuzQPZF/I40/hCrX89gxi1VZ5bWwep/o+bWqL898ihLLP3HCJ8cupTYndVK1ni8bnjUNVcujhVij52hzHc/YKVH8ZA+FbgiCw9L2xna7jP0SZ287FnPTcKTZ1LMJpwNgOu/PBIv5QQpqoIke5REAoghpPSkYJvO568Tr57Z9oW0pnWgqDViaInl+rW0Sg0yz8Q5JghbNPFka9kxYrHLQGHwCz3zEuj8z9uzSYu6pHmxlw6ShVJO/PPs04G3aesvsszWf3s64IrxgJwq4=");

            VivaPaymentOrder vivaPaymentOrder = new VivaPaymentOrder()
            {
                amount=(int)payment.Amount*100,
                customerTrns="a Short description of purchased items/services to display to your customer",
                //customer=new Customer()
                //{
                //    //email="jim.liakos@gmail.com",
                //    fullName="Jim Liakos",
                //    phone="30999999999",
                //    countryCode="GR",
                //    requestLang="el-GR"
                //},
                paymentTimeout=300,
                preauth=false,
                allowRecurring=false,
                maxInstallments=12,
                paymentNotification=true,
                tipAmount=(int)payment.TipsAmount*100,
                disableCash=true,
                disableWallet=false,
                merchantTrns="b Short description of items/services purchased by customer",
                tags=new List<string>()
                {
                    "tags for grouping and filtering the transactions",
                    "this tag can be searched on VivaWallet sales dashboard"
                }
            };
            string body = OOAdvantech.Json.JsonConvert.SerializeObject(vivaPaymentOrder);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            paymentOrderResponse = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrderResponse>(response.Content);
            paymentOrderResponse.expiring=(DateTime.UtcNow+TimeSpan.FromSeconds(vivaPaymentOrder.paymentTimeout)).Ticks;

            payment.PaymentProviderJson=OOAdvantech.Json.JsonConvert.SerializeObject(paymentOrderResponse);

        }
        public class PaymentOrderResponse
        {
            public long orderCode { get; set; }
            public long expiring { get; set; }
        }

        private static string GetAccessToken(string clientID, string clientSecret)
        {
            if (AccessToken!=null&&(DateTime.UtcNow- AccessToken.timestamp).TotalSeconds*0.9<AccessToken.expires_in)
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
            AccessToken= OOAdvantech.Json.JsonConvert.DeserializeObject<AccessToken>(response.Content);
            AccessToken.timestamp= DateTime.UtcNow;
            return AccessToken.access_token;

            //eyJhbGciOiJSUzI1NiIsImtpZCI6IjBEOEZCOEQ2RURFQ0Y1Qzk3RUY1MjdDMDYxNkJCMjMzM0FCNjVGOUZSUzI1NiIsIng1dCI6IkRZLTQxdTNzOWNsLTlTZkFZV3V5TXpxMlg1OCIsInR5cCI6ImF0K2p3dCJ9.eyJpc3MiOiJodHRwczovL2RlbW8tYWNjb3VudHMudml2YXBheW1lbnRzLmNvbSIsIm5iZiI6MTY2OTkwMDUwNiwiaWF0IjoxNjY5OTAwNTA2LCJleHAiOjE2Njk5MDQxMDYsImF1ZCI6WyJjb3JlX2FwaSIsImh0dHBzOi8vZGVtby1hY2NvdW50cy52aXZhcGF5bWVudHMuY29tL3Jlc291cmNlcyJdLCJzY29wZSI6WyJ1cm46dml2YTpwYXltZW50czpjb3JlOmFwaTphY3F1aXJpbmciLCJ1cm46dml2YTpwYXltZW50czpjb3JlOmFwaTphY3F1aXJpbmc6Y2FyZHRva2VuaXphdGlvbiIsInVybjp2aXZhOnBheW1lbnRzOmNvcmU6YXBpOnJlZGlyZWN0Y2hlY2tvdXQiXSwiY2xpZW50X2lkIjoieTJrN2tsd29jdnpldDM4dTBjcTNtbm96Y3VqdWh1N2JwZGVoY3JteDdqMW05LmFwcHMudml2YXBheW1lbnRzLmNvbSIsInVybjp2aXZhOnBheW1lbnRzOmNsaWVudF9wZXJzb25faWQiOiI1QTMzMDYyOS0wMEI3LTQ5RTctOUJFOS05QTMxN0Y3MUFGNTAifQ.MAKb3UTs7OQMkI_bhqOiHnOqHXZATFL_R5S0U6mvIFLbjRnck_urpBHDRDlWPOI1EA-tfcJlqnj-vmo6w3Av6HetjdS0urrkC1pyjh0QEWm9Hftmi0t4_SvVhQS3veshO4N9dUTguba-u0GoSlLTHpQhtz3Y6cGrDhMqKiGEc5mOKbp7Epqwvh9oi4MkZdcwCoO0_ywhFRBpQbpoXSXRXBSexMCuCzLiUAZoD3fxx7bNdIBdTAsIylNj72c09afOSl6rIdAeRb-wFF9WT4_b-Vo3xJHTp6pOF0knoliKZgDSqO-QL-UAiyeR6grVfh2WicOPUqZ6ranMVu0EhCpAsg
        }

        internal static void CompletePayment(IPayment payment)
        {
            
        }
    }


    public class AccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public DateTime timestamp { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Customer
    {
        public string email { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string countryCode { get; set; }
        public string requestLang { get; set; }
    }

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




}
