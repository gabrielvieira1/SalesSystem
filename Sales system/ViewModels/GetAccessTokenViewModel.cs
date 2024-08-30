using System.Windows.Input;
using Windows.Foundation.Collections;
using SamsungAccountLibrary.SAResponse;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Sales_system.Util;
using Connection;
using System.Diagnostics;
using Sales_system.Library;
using SamsungAccountLibrary.SARequest;
using SamsungAccountLibrary;
using System;
using System.Threading.Tasks;
using SamsungAccountLibrary.SARequest.AccountServerTypeRequest;

namespace Sales_system
{
  internal class GetAccessTokenViewModel : ISaSdkResponse
  {
    private string accountServerType;
    public static String state = null;
    public static String code_verifier = null;
    private static String TEST_CLIENT_ID = "5z959u0k0k";
    private static String TEST_CLIENT_SECRET = "731DC767CFAF351549E8C269ABFC747D";
    private static readonly int RANDOM_STRING_LENGTH = 32;

    public GetAccessTokenViewModel()
    {

      GetAccessTokenRequest getAccessTokenRequest = new GetAccessTokenRequest();
      getAccessTokenRequest.SetClientId(StaticConstants.clientID).SetAuthCode(MetaDataManager.GetInstance().GetAuthorizationCode()).
          SetCodeVerifier(MetaDataManager.GetInstance().GetAuthorizationCodeVerifier());
      String uiMessage = $"ResponseType: {StaticConstants.clientID}, Result: {MetaDataManager.GetInstance().GetAuthorizationCode()}, Code: {MetaDataManager.GetInstance().GetAuthorizationCodeVerifier()}, ";
      
      MetaDataManager.GetInstance().SetSaSDKResponseListener(this);

      SamsungAccountManager.GetAccessToken(this, getAccessTokenRequest);
    }

    public void OnResponseReceived(IResponse response)
    {
      String uiMessage = "";


      if (response.ResponseType == SaResponseType.accessToken)
      {
        GetAccessTokenResponse getAccessTokenResponse = response as GetAccessTokenResponse;
        uiMessage = ProcessGetAccessTokenResponse(getAccessTokenResponse);
      }
      else if (response.ResponseType == SaResponseType.initialize)
      {
        if (response.GetResult().Equals("true"))
        {
          MetaDataManager.GetInstance().SetServerType(accountServerType);
        }
        InitializeResponse initializeResponse = response as InitializeResponse;
        uiMessage = $"ResponseType: {initializeResponse.ResponseType}, Result: {initializeResponse.GetResult()} {accountServerType} is now selected Server type change is prohibited in interim account flow.";
      }

      Debug.WriteLine("Messsage");
      Debug.WriteLine(uiMessage);
    }

    String ProcessGetAccessTokenResponse(GetAccessTokenResponse accessTokenResponse)
    {
      String uiMessage;
      if (Utils.IsSuccessed(accessTokenResponse.GetResult()))
      {
        MetaDataManager.GetInstance().SetAccessToken(accessTokenResponse.GetAccessToken());
        MetaDataManager.GetInstance().SetRefreshToken(accessTokenResponse.GetRefreshToken());
        MetaDataManager.GetInstance().SetUserId(accessTokenResponse.GetUserId());
        uiMessage = $"ResponseType: {accessTokenResponse.ResponseType}, Result: {accessTokenResponse.GetResult()}, " +
           $"Access Token: {accessTokenResponse.GetAccessToken()}, Refresh Token: {accessTokenResponse.GetRefreshToken()}, " +
           $"AccessToken expires in: {accessTokenResponse.GetAccessTokenExpiresIn()}, RefreshToken expires in: {accessTokenResponse.GetRefreshTokenExpiresIn()}, " +
           $"UserId: {accessTokenResponse.GetUserId()}, ServerTime: {accessTokenResponse.GetServerTime()}, " +
           $"AuthServerUrl: {accessTokenResponse.GetAuthServerUrl()}, ApiServerUrl: {accessTokenResponse.GetApiServerUrl()}";
      }
      else
      {
        uiMessage = $"ResponseType: {accessTokenResponse.ResponseType}, Result: {accessTokenResponse.GetResult()}, Error Code: " +
            $"{accessTokenResponse.GetErrorCode()}, Error Message: {accessTokenResponse.GetErrorMessage()}.";
      }

      return uiMessage;
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
