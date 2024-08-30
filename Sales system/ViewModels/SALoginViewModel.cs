using Sales_system.Library;
using SamsungAccountLibrary.SARequest;
using SamsungAccountLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Collections;
using SamsungAccountLibrary.SAResponse;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Diagnostics;
using System.CodeDom.Compiler;
using Models;
using SamsungAccountLibrary.SARequest.AccountServerTypeRequest;
using LinqToDB;



namespace Sales_system
{

  internal class SALoginViewModel : ISaSdkResponse
  {
    public static String state = null;
    public static String code_verifier = null;
    private static String TEST_CLIENT_ID = "5z959u0k0k";
    private static String TEST_CLIENT_SECRET = "731DC767CFAF351549E8C269ABFC747D";
    private string accountServerType;
    private static readonly int RANDOM_STRING_LENGTH = 32;

    public SALoginViewModel()
    {
      SamsungAccountManager.SubscribeLogging();

      ValueSet signInData = new ValueSet();

      // create random code
      code_verifier = Util.StateGenerator.GenerateStateValue(50);
      state = Util.StateGenerator.GenerateStateValue(RANDOM_STRING_LENGTH);
      MetaDataManager.GetInstance().SetState(state);
      MetaDataManager.GetInstance().SetCurrentRequest(Convert.ToInt32(Util.Constants.ServiceType.signIn));
      MetaDataManager.GetInstance().SetAuthorizationCodeVerifier(code_verifier);
      MetaDataManager.GetInstance().SetSaSDKResponseListener(this);


      SignInRequest signInDataObj = new SignInRequest(StaticConstants.redirectUri);
      signInDataObj.SetClientID(TEST_CLIENT_ID).SetCodeVerifier(code_verifier).SetScope(StaticConstants.scope).SetState(state);

      SamsungAccountManager.SignIn(signInDataObj);
    }

 
    async Task<String> ProcessSigninResponse(SignInResponse signInResponse)
    {
      String uiMessage;
      try
      {
        String responseState = SamsungAccountManager.Decrypt(signInResponse.GetState(), state);

        if (Utils.IsSuccessed(signInResponse.GetResult(), responseState))
        {
          MetaDataManager.GetInstance().SetSignedInStatus(true);
          string authCode = SamsungAccountManager.Decrypt(signInResponse.GetCode(), state);
          string authServerUrl = SamsungAccountManager.Decrypt(signInResponse.GetAuthServerUrl(), state);
          string apiServerUrl = SamsungAccountManager.Decrypt(signInResponse.GetApiServerUrl(), state);
          string codeExpiresIn = signInResponse.GetCodeExpiresIn();
          string result = signInResponse.GetResult();
          string action = signInResponse.GetAction();
          string sdkState = SamsungAccountManager.Decrypt(signInResponse.GetState(), state);
          MetaDataManager.GetInstance().SetAuthorizationCode(authCode);
          MetaDataManager.GetInstance().SetAuthServerUrl(authServerUrl);


          /* Store authCode and authServerUrl here (or in the getter and setter) in database*/
          /* pass user info with user obj in Navigate */


          /* Generate access token here */

          /*await getAccessToken();
*/
          

          ((Frame)Window.Current.Content).Navigate(typeof(Welcome));

          uiMessage = $"ResponseType: {signInResponse.ResponseType}, Result: {result}, Code: {authCode}, AuthServerURL: {authServerUrl}, " +
              $"ApiServerURL: {apiServerUrl}, CodeExpiresIn: {codeExpiresIn}, Action: {action}, State: {sdkState}.";

        }
        else
        {
          string authCode = SamsungAccountManager.Decrypt(signInResponse.GetCode(), state);
          string result = signInResponse.GetResult();
          string errorCode = signInResponse.GetErrorCode();
          string errorMessage = signInResponse.GetErrorMessage();
          string action = signInResponse.GetAction();
          string sdkState = SamsungAccountManager.Decrypt(signInResponse.GetState(), state);

          uiMessage = $"ResponseType: {signInResponse.ResponseType}, Result: {result}, Code: {authCode}, " +
              $"Action: {action}, State: {sdkState}, ErrorCode: {errorCode}, ErrorMessage: {errorMessage}.";
        }

      }
      catch (Exception ex)
      {
        uiMessage = ex.Message;
      }

      return uiMessage;
    }

    public void OnResponseReceived(IResponse response)
    {
      String uiMessage = string.Empty;
      if (response == null)
      {
        uiMessage = $"Response is null";
      }
      else if (response.ResponseType == SaResponseType.signIn)
      {
        SignInResponse signInResponse = response as SignInResponse;
        uiMessage = ProcessSigninResponse(signInResponse).Result;
      }
    }

    private void Initialize_Click(object sender, RoutedEventArgs e)
    {
      AccountServerType accType = AccountServerType.PROD;
      if (accountServerType == "STG1")
        accType = AccountServerType.STG1;
      else if (accountServerType == "STG2")
        accType = AccountServerType.STG2;
      else if (accountServerType == "PROD")
        accType = AccountServerType.PROD;

      SamsungAccountManager.Initialize(this, accType);
    }
  }


}
