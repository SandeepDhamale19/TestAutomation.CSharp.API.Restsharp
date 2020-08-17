using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TechTalk.SpecFlow;
using TestAutomation.APITests.Pages.Pages_Data;
using TestAutomation.Framework.Helpers.API_Helpers;
using TestAutomation.Framework.Helpers.Json;
using TestAutomation.Framework.Helpers.Strings;

namespace TestAutomation.APITests.StepDefinitions.StepDefinitions_API
{
    [Binding]
    public sealed class BookstoreSteps : RestFramework
    {
        private IRestResponse restResponse;
        private readonly RestMethodsParameteres restMethod;
        private readonly APICommonData apiCommonData;
        private readonly JsonHelper jsonHelper = new JsonHelper();
        public BookstoreSteps()
        {
            restMethod = Page<RestMethodsParameteres>();
            apiCommonData = Page<APICommonData>();
        }

        [Given(@"I access books from bookstore with (.*), (.*) and (.*)")]
        public void GivenIAccessBooksFromBookstoreWithBookStoreVBooksGetAnd(string url, string verb, string data)
        {
            Framework.Helpers.Json.RequestBody requestData =
                  new Framework.Helpers.Json.RequestBody() { Url = url, Verb = verb, Data = data };
            var dynamicRequest = restMethod.RestRequest.CreateRequestWithCredentials(requestData);
        }

        [Then(@"I am provided with information about books")]
        public void ThenIAmProvidedWithInformationAboutBooks()
        {
            restResponse = restMethod.RestResponse.GetResponseWithCredentials();
            bool validStatusCode = restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.OK);
            if (validStatusCode)
            {               
                // Get list of all values for a key
                var allISBN = jsonHelper.GetJsonContentList(restResponse.Content, "isbn");
                var allISBNReq = jsonHelper.GetJsonContentList(apiCommonData.VerificationData.ToString(), "isbn");

                // Comapre list of all values for a key
                StringBuilder sb = new StringBuilder();
                bool compareLists = ListExtentions.ListOfStringsComparer(allISBN, allISBNReq, sb);

                var allTitles = jsonHelper.GetJsonContentList(restResponse.Content, "title");
                var allSubTitles = jsonHelper.GetJsonContentList(restResponse.Content, "subtitle");

            }
        }

        [Given(@"I add isbn for a book")]
        public void GivenIAddIsbnForABook()
        {
            restMethod.RestRequest.CreateRequestWithCredentials(apiCommonData.TestData);
            //string url = "/BookStore/v1/Books";
            //string verb = "post";

            //// Method 1: String Interpolation
            //string data = apiCommonData.TestData.ToString();

            //Framework.Helpers.Json.RequestBody requestData =
            //      new Framework.Helpers.Json.RequestBody() { Url = url, Verb = verb, Data = data };
            //var dynamicRequest = restMethod.RestRequest.CreateRequestWithCredentials(requestData, "Captain America", "MyP@ss123");

        }

        [Then(@"book is added to my bookstore")]
        public void ThenBookIsAddedToMyBookstore()
        {
            restResponse = restMethod.RestResponse.GetResponseWithCredentials();
            bool validStatusCode = restMethod.RestResponse.VerifyStatusCodeEquals((int)HttpStatusCode.Created);
        }



    }
}
