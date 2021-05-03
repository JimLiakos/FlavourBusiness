using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace BingAPILibrary
{
    /// <MetaDataID>{4dec6161-96f9-43eb-8dad-e832f79d3d8b}</MetaDataID>
    public class Translator
    {
        // To learn more about app development with the Bing translate services, visit
        // http://www.bing.com/dev/en-us/translator

        // Microsoft Translator API in Azure data Market (up to 2M characters / montn free):
        // http://datamarket.azure.com/dataset/bing/microsofttranslator

        // You need to obtain a valid application key from the Azure Marketplace,
        // see http://msdn.microsoft.com/en-us/library/hh454950.aspx for details
        // THE TRANSLATOR CALLS WILL FAIL UNTIL YOU REPLACE THE TWO VALUES BELOW WITH
        // YOUR OWN ID & SECRET. sEE THE LINKS ABOVE FOR INSTRUCTIONS
        private readonly string _clientId = "ActiveNickBingTranslateDemo";
        private readonly string _clientSecret = "+IA+aBq4RzNt8AQR10BeXpozeNPnaVgF6JjmkRyj8d0=";

        private readonly Uri _dataMarketAddress = new Uri("https://datamarket.accesscontrol.windows.net/v2/OAuth2-13");
        private const string TEXT_TRANSLATION_API_SUBSCRIPTION_KEY = "da260c97e0dc41a4b6824e1ac2834beb";


        // We use a single HttpClient instance for all requests to Azure Marketplace
        private HttpClient _client = new HttpClient();
        AzureAuthToken tokenProvider;

        static Translator CurrentTranslator;

        public static string TranslateString(string strSource, string languageCode)
        {
            
            if (CurrentTranslator == null)
                CurrentTranslator = new Translator();

            if (CurrentTranslator.tokenProvider == null)
                CurrentTranslator.tokenProvider = new AzureAuthToken(TEXT_TRANSLATION_API_SUBSCRIPTION_KEY);
            string auth = CurrentTranslator.tokenProvider.GetAccessToken();// GetAzureDataMarketToken();


            if (languageCode == null)  //in case no language is selected.
            {
                languageCode = "en";

            }

            //*****BEGIN CODE TO MAKE THE CALL TO THE TRANSLATOR SERVICE TO PERFORM A TRANSLATION FROM THE USER TEXT ENTERED INCLUDES A CALL TO A SPEECH METHOD*****

            string txtToTranslate = strSource;

            string uri = string.Format("http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(txtToTranslate) + "&to={0}", languageCode);

            WebRequest translationWebRequest = WebRequest.Create(uri);

            translationWebRequest.Headers.Add("Authorization", CurrentTranslator.tokenProvider.GetAccessToken()); //header value is the "Bearer plus the token from ADM

            WebResponse response = null;

            response = translationWebRequest.GetResponse();

            Stream stream = response.GetResponseStream();

            Encoding encode = Encoding.GetEncoding("utf-8");

            StreamReader translatedStream = new StreamReader(stream, encode);

            System.Xml.XmlDocument xTranslation = new System.Xml.XmlDocument();

            xTranslation.LoadXml(translatedStream.ReadToEnd());

            string strTransText = xTranslation.InnerText;
            return strTransText;
            //translatedTextLabel.Content = "Translation -->   " + xTranslation.InnerText;

            ////_client.DefaultRequestHeaders.Add("Authorization", auth);
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
            //string RequestUri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" +
            //                    System.Net.WebUtility.HtmlEncode(strSource) +
            //                    "&to=" + language;
            //try
            //{
            //    Task<string> taskstrTranslated = _client.GetStringAsync(RequestUri);
            //    taskstrTranslated.Wait();
            //    string strTranslated = taskstrTranslated.Result;
            //    System.Xml.Linq.XDocument xTranslation = System.Xml.Linq.XDocument.Parse(strTranslated);
            //    string strTransText = xTranslation.Root.FirstNode.ToString();
            //    if (strTransText == strSource)
            //        return "";
            //    else
            //        return strTransText;
            //}
            //catch (Exception ex)
            //{
            //    // TO DO: Do something with the exception
            //    return "";
            //}
        }

        public List<string> GetLanguagesForTranslate()
        {
            List<string> languages = new List<string>();

            string auth = GetAzureDataMarketToken();

            //_client.DefaultRequestHeaders.Add("Authorization", auth);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
            string RequestUri = "http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForTranslate";
            try
            {
                Task<string> taskstrTranslated = _client.GetStringAsync(RequestUri);
                taskstrTranslated.Wait();
                string strTranslated = taskstrTranslated.Result;
                System.Xml.Linq.XDocument xTranslation = System.Xml.Linq.XDocument.Parse(strTranslated);

                string strTransText = xTranslation.Root.FirstNode.ToString();

                foreach (XElement xe in xTranslation.Root.FirstNode.ElementsAfterSelf())
                {

                    languages.Add(xe.Value.ToString());
                }

                return languages;
            }
            catch (Exception ex)
            {
                // TO DO: Do something with the exception
                return null;
            }
        }

        private string GetAzureDataMarketToken()
        {
            // First we issue async HTML form POST request to the Azure Marketplace to obtain an Access Token. 
            // See http://msdn.microsoft.com/en-us/library/hh454950.aspx for details

            // Create form parameters that we will send to data market.
            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id",  _clientId},
                { "client_secret", _clientSecret },
                { "scope", "http://api.microsofttranslator.com" }
            };

            FormUrlEncodedContent authentication = new FormUrlEncodedContent(properties);
            Task<HttpResponseMessage> taskDataMarketResponse = _client.PostAsync(_dataMarketAddress, authentication);
            taskDataMarketResponse.Wait();
            HttpResponseMessage dataMarketResponse = taskDataMarketResponse.Result;
            string dmResponse;
            Task<string> taskdmResponse = null;

            // If client authentication failed then we get a JSON response from Azure Market Place
            if (!dataMarketResponse.IsSuccessStatusCode)
            {
                //JToken error = await dataMarketResponse.Content.ReadAsAsync<JToken>();
                taskdmResponse = dataMarketResponse.Content.ReadAsStringAsync();
                taskdmResponse.Wait();

                dmResponse = taskdmResponse.Result;
                JToken error = Newtonsoft.Json.JsonConvert.DeserializeObject<JToken>(dmResponse);

                string errorType = error.Value<string>("error");
                string errorDescription = error.Value<string>("error_description");
                throw new HttpRequestException(string.Format("Azure market place request failed: {0} {1}", errorType, errorDescription));
            }

            // Get the access token to attach to the original request from the response body
            //AdmAccessToken accessToken = await dataMarketResponse.Content.ReadAsAsync<AdmAccessToken>();
            taskdmResponse = dataMarketResponse.Content.ReadAsStringAsync();
            taskdmResponse.Wait();
            dmResponse = taskdmResponse.Result;
            AdmAccessToken accessToken = Newtonsoft.Json.JsonConvert.DeserializeObject<AdmAccessToken>(dmResponse);

            return accessToken.access_token;
        }
    }

    /// <summary>
    /// This class describes an Access Token from Azure Data Market 
    /// </summary>
    /// <MetaDataID>{51f49ccd-d559-4b40-a04b-c5f452b47f9b}</MetaDataID>
    public class AdmAccessToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public string expires_in { get; set; }

        public string scope { get; set; }
    }


    /// <MetaDataID>{afe156c5-b394-4924-8605-a7960f22646d}</MetaDataID>
    public class AzureAuthToken
    {
        /// URL of the token service
        private static readonly Uri ServiceUrl = new Uri("https://api.cognitive.microsoft.com/sts/v1.0/issueToken");

        /// Name of header used to pass the subscription key to the token service
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        /// After obtaining a valid token, this class will cache it for this duration.
        /// Use a duration of 5 minutes, which is less than the actual token lifetime of 10 minutes.
        private static readonly TimeSpan TokenCacheDuration = new TimeSpan(0, 5, 0);

        /// Cache the value of the last valid token obtained from the token service.
        private string _storedTokenValue = string.Empty;

        /// When the last valid token was obtained.
        private DateTime _storedTokenTime = DateTime.MinValue;

        /// Gets the subscription key.
        public string SubscriptionKey { get; }

        /// Gets the HTTP status code for the most recent request to the token service.
        public HttpStatusCode RequestStatusCode { get; private set; }

        /// <summary>
        /// Creates a client to obtain an access token.
        /// </summary>
        /// <param name="key">Subscription key to use to get an authentication token.</param>
        public AzureAuthToken(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "A subscription key is required");
            }

            this.SubscriptionKey = key;
            this.RequestStatusCode = HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Gets a token for the specified subscription.
        /// </summary>
        /// <returns>The encoded JWT token prefixed with the string "Bearer ".</returns>
        /// <remarks>
        /// This method uses a cache to limit the number of request to the token service.
        /// A fresh token can be re-used during its lifetime of 10 minutes. After a successful
        /// request to the token service, this method caches the access token. Subsequent 
        /// invocations of the method return the cached token for the next 5 minutes. After
        /// 5 minutes, a new token is fetched from the token service and the cache is updated.
        /// </remarks>
        public async Task<string> GetAccessTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(this.SubscriptionKey))
            {
                return string.Empty;
            }

            // Re-use the cached token if there is one.
            if ((DateTime.Now - _storedTokenTime) < TokenCacheDuration)
            {
                return _storedTokenValue;
            }

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = ServiceUrl;
                request.Content = new StringContent(string.Empty);
                request.Headers.TryAddWithoutValidation(OcpApimSubscriptionKeyHeader, this.SubscriptionKey);
                client.Timeout = TimeSpan.FromSeconds(2);

                //Method: POST, RequestUri: 'https://api.cognitive.microsoft.com/sts/v1.0/issueToken', Version: 1.1, Content: System.Net.Http.StringContent, Headers:
                //{
                //    Ocp - Apim - Subscription - Key: da260c97e0dc41a4b6824e1ac2834beb
                //    Content - Type: text / plain; charset = utf - 8
                //    Content - Length: 0
                //}

                //Method: POST, RequestUri: 'https://api.cognitive.microsoft.com/sts/v1.0/issueToken', Version: 1.1, Content: System.Net.Http.StringContent, Headers:
                //{
                //    Ocp - Apim - Subscription - Key: da260c97e0dc41a4b6824e1ac2834beb
                //    Content - Type: text / plain; charset = utf - 8
                //}

                Task<HttpResponseMessage> taskDataMarketResponse = client.SendAsync(request);
                taskDataMarketResponse.Wait();
                var response = taskDataMarketResponse.Result;
                this.RequestStatusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();
                Task<string> tokenTask = response.Content.ReadAsStringAsync();
                tokenTask.Wait();
                var token = tokenTask.Result;// response.Content.ReadAsStringAsync();
                _storedTokenTime = DateTime.Now;
                _storedTokenValue = "Bearer " + token;
                return _storedTokenValue;
            }
        }

        /// <summary>
        /// Gets a token for the specified subscription. Synchronous version.
        /// Use of async version preferred
        /// </summary>
        /// <returns>The encoded JWT token prefixed with the string "Bearer ".</returns>
        /// <remarks>
        /// This method uses a cache to limit the number of request to the token service.
        /// A fresh token can be re-used during its lifetime of 10 minutes. After a successful
        /// request to the token service, this method caches the access token. Subsequent 
        /// invocations of the method return the cached token for the next 5 minutes. After
        /// 5 minutes, a new token is fetched from the token service and the cache is updated.
        /// </remarks>
        public string GetAccessToken()
        {
            // Re-use the cached token if there is one.
            if ((DateTime.Now - _storedTokenTime) < TokenCacheDuration)
            {
                return _storedTokenValue;
            }

            string accessToken = null;
            //var task = Task.Run(async () =>
            //{
            //    accessToken = await this.GetAccessTokenAsync();
            //});
            //accessToken
            var task = this.GetAccessTokenAsync();
            task.Wait();
            accessToken = task.Result;
            while (!task.IsCompleted)
            {
                System.Threading.Thread.Yield();
            }
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
            if (task.IsCanceled)
            {
                throw new Exception("Timeout obtaining access token.");
            }
            return accessToken;
        }

    }
}
