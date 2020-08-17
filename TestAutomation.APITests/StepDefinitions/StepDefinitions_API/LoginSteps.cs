using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TechTalk.SpecFlow;
using TestAutomation.APITests.Pages.Pages_Data;
using TestAutomation.Framework.Helpers.API_Helpers;
using TestAutomation.Framework.Helpers.Strings;

namespace TestAutomation.APITests.StepDefinitions.StepDefinitions_API
{
    [Binding]
    public sealed class LoginSteps : RestFramework
    {
        private IRestResponse restResponse;
        private readonly RestMethodsParameteres restMethod;
        private readonly APICommonData apiCommonData;

        public LoginSteps()
        {
            restMethod = Page<RestMethodsParameteres>();
            apiCommonData = Page<APICommonData>();
        }
                
        [Given(@"I provide valid credentials")]
        public void GivenIProvideValidCredentials()
        {
            restMethod.RestRequest.CreateRequestWithCredentials(apiCommonData.TestData);            
        }

        [Given(@"I add new user details")]
        public void GivenIAddNewUserDetails()
        {
            string url = "/Account/v1/User";
            string verb = "post";
            string randomUserName = StringExtentions.RandomString(10);

            // Method 1: String Interpolation
            string data = $@"{{""userName"":""{randomUserName}"", ""password"":""MyP@ss123""}}";

            // Method 2: Using property
            var testData = new
            {
                userName = randomUserName,
                password = "MyP@ss123"
            };
            data = JsonConvert.SerializeObject(testData);

            // Method 3: Dictionary
            Dictionary<string, string> testDataDictionary = new Dictionary<string, string>();
            testDataDictionary.Add("userName", randomUserName);
            testDataDictionary.Add("password", "MyP@ss123");

            data = JsonConvert.SerializeObject(testDataDictionary);

            // Method 4: Dynamic repalce request json data
            data = restMethod.RestRequest.ReplaceData(apiCommonData.TestData.GetType().GetProperty("Data").GetValue(apiCommonData.TestData, null), new string[] {"userName"}, randomUserName).ToString();
            data = JsonConvert.DeserializeObject(data).ToString().Replace(System.Environment.NewLine, string.Empty);
            data = string.Join("", data.Where(c => !char.IsWhiteSpace(c)));

            Framework.Helpers.Json.RequestBody requestData =
                  new Framework.Helpers.Json.RequestBody() { Url = url, Verb = verb, Data = data };
            var dynamicRequest = restMethod.RestRequest.CreateRequestWithCredentials(requestData);
            
        }

        [Given(@"I add new user")]
        public void GivenIAddNewUser()
        {
            string url = "/Account/v1/User";
            string verb = "post";
            string randomUserName = StringExtentions.RandomString(10);

            // Method 1: String Interpolation
            string data = $@"{{""userName"":""{randomUserName}"", ""password"":""MyP@ss123""}}";

            Framework.Helpers.Json.RequestBody requestData =
                  new Framework.Helpers.Json.RequestBody() { Url = url, Verb = verb, Data = data };
            var dynamicRequest = restMethod.RestRequest.CreateRequestWithCredentials(requestData);
        }

        [Given(@"I delete added user")]
        [When(@"I delete added user")]
        public void GivenIDeleteUser()
        {
            string createdUserId = ScenarioContext.Current.Get<string>("UserId");
            string createdUserName = ScenarioContext.Current.Get<string>("UserName");
            string url = $"/Account/v1/User/{createdUserId}";
            string verb = "delete";

            //string randomUserName = StringExtentions.RandomString(10);

            // Method 1: String Interpolation
            string data = $@"{{""userName"":""{createdUserName}"", ""password"":""MyP@ss123""}}";

            Framework.Helpers.Json.RequestBody requestData =
                  new Framework.Helpers.Json.RequestBody() { Url = url, Verb = verb, Data = data };
            var dynamicRequest = restMethod.RestRequest.CreateRequestWithCredentials(requestData, createdUserName, "MyP@ss123");
        }

        [When(@"I request for user details")]
        public void WhenIRequestForUserDetails()
        {
            string createdUserId = ScenarioContext.Current.Get<string>("UserId");
            string createdUserName = ScenarioContext.Current.Get<string>("UserName");
            string url = $"/Account/v1/User/{createdUserId}";
            string verb = "get";
            

            // Method 1: String Interpolation
            string data = $@"{{""userName"":""{createdUserName}"", ""password"":""MyP@ss123""}}";

            Framework.Helpers.Json.RequestBody requestData =
                  new Framework.Helpers.Json.RequestBody() { Url = url, Verb = verb, Data = data };
            var dynamicRequest = restMethod.RestRequest.CreateRequestWithCredentials(requestData, createdUserName, "MyP@ss123");
        }


        [Then(@"I am authorized to access bookstore application")]
        public void ThenIAmAuthorizedToAccessBookstoreApplication()
        {
            restResponse = restMethod.RestResponse.GetResponseWithCredentials();
            bool validStatusCode = restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.OK);
            if (validStatusCode)
            {
                restMethod.RestResponse.VerifyResponseContent("true");
            }
        }

        [Then(@"I am provided with valid token")]
        public void ThenIAmProvidedWithValidToken()
        {
            restResponse = restMethod.RestResponse.GetResponseWithCredentials();
            bool validStatusCode = restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.OK);
            if (validStatusCode)
            {
                restMethod.RestResponse.VerifyResponseExists("$.token");
            }
        }


        [Then(@"I am logged in to bookstore application")]
        public void ThenIAmLoggedInToBookstoreApplication()
        {
            restResponse = restMethod.RestResponse.GetResponseWithCredentials();
            restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.OK);            
        }


        [Then(@"I am provided with Username, UserId and token")]
        public void ThenIAmProvidedWithUsernameUserIdAndToken()
        {
            bool validStatusCode = restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.OK);
            if (validStatusCode)
            {
                // field validations
                restMethod.RestResponse.VerifyResponseExists("$.userId");
                restMethod.RestResponse.VerifyResponseExists("$.username");
                restMethod.RestResponse.VerifyResponseExists("$.password");
                restMethod.RestResponse.VerifyResponseExists("$.token");
                restMethod.RestResponse.VerifyResponseExists("$.expires");
                restMethod.RestResponse.VerifyResponseExists("$.created_date");
                restMethod.RestResponse.VerifyResponseExists("$.isActive");

                // Schema validations
                var expectedSchema = restMethod.RestRequest.GetRequestJsonParameter("$.Schema[0]");
                restMethod.RestResponse.VerifyResponseSchema("$", expectedSchema.ToString());

                // Response Content validations:                
                restMethod.RestResponse.VerifyResponseEquals("Captain America", "$.username");
                restMethod.RestResponse.VerifyResponseContains("America", "$.username");

                // Read Resonse parameters
                string userId = restMethod.RestResponse.GetResponseKeyValue("$.userId").ToString();

                // Read Request Parameters
                string userName = restMethod.RestRequest.GetRequestJsonParameter("$.TestData[0].Data.userName").ToString();
                //OR
                userName = restMethod.RestRequest.GetRequestJsonParameter("$.TestData[0]..userName").ToString();
            }
        }

        [Then(@"I am provided with user details")]
        public void ThenIAmProvidedWithUserDetails()
        {
            restResponse = restMethod.RestResponse.GetResponseWithCredentials();
            bool validStatusCode = restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.Created);

            if (validStatusCode)
            {
                restMethod.RestResponse.VerifyResponseExists("$.userID");
                restMethod.RestResponse.VerifyResponseExists("$.username");

                ScenarioContext.Current.Remove("UserId");
                ScenarioContext.Current.Add("UserId", restMethod.RestResponse.GetResponseKeyValue("$.userID").ToString());

                ScenarioContext.Current.Remove("UserName");
                ScenarioContext.Current.Add("UserName", restMethod.RestResponse.GetResponseKeyValue("$.username").ToString());
            }
        }

        [Then(@"I am provided with requested user details")]
        public void ThenIAmProvidedWithRequestedUserDetails()
        {
            restResponse = restMethod.RestResponse.GetResponseWithCredentials();
            bool validStatusCode = restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.OK);

            if (validStatusCode)
            {
                restMethod.RestResponse.VerifyResponseExists("$.userId");
                restMethod.RestResponse.VerifyResponseExists("$.username");
            }
        }

        [Then(@"User details should not be available")]
        public void ThenUserDetailsShouldNotBeAvailable()
        {
            restResponse = restMethod.RestResponse.GetResponseWithCredentials();
            bool validStatusCode = restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.OK);

            if (validStatusCode)
            {
                
            }
        }

    }
}
