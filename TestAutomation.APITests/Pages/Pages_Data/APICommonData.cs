using TestAutomation.Framework.Constants;
using TestAutomation.Framework.Helpers.Json;
using TestAutomation.Framework.Helpers.Setup;

namespace TestAutomation.APITests.Pages.Pages_Data
{
    public class APICommonData : BasePage<APICommonData>
    {
        readonly JsonHelper jHelper = new JsonHelper();


        public const string testData = "TestData";
        public object TestData => jHelper.GetAPIData(testData);

        public const string verificationData = "VerificationData";
        public object VerificationData => jHelper.GetJsonValue(verificationData);

    }
}
