using Sales_system.Library;
using Sales_system.Models;
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
using Sales_system.Util;
using Connection;


namespace Sales_system
{
  internal class SASignoutViewModel : ISaSdkResponse
  {
    private ICommand _command;
    private static readonly int RANDOM_STRING_LENGTH = 32;
    public static String state = null;


    public ICommand IniciarCommand
    {
      get
      {
        return _command ?? (_command = new CommandHandler(async () =>
        {
          await IniciarAsync();
        }));
      }
    }

    private async Task IniciarAsync()
    {
      state = Util.StateGenerator.GenerateStateValue(RANDOM_STRING_LENGTH);
      ValueSet bundle = new ValueSet();

      MetaDataManager.GetInstance().SetState(state);
      MetaDataManager.GetInstance().SetCurrentRequest(Convert.ToInt32(Constants.ServiceType.signOut));
      MetaDataManager.GetInstance().SetSaSDKResponseListener(this);
      SignOutRequest signOutRequest = new SignOutRequest(StaticConstants.redirectUri);
      signOutRequest.SetClientID(StaticConstants.clientID).SetState(state);
      SamsungAccountManager.SignOut(signOutRequest);
    }

    String ProcessSignoutResponse(SignOutResponse signOutResponse)
    {
      String uiMessage;
      try
      {
        String responseState = SamsungAccountManager.Decrypt(signOutResponse.GetState(), state);
        if (Utils.IsSuccessed(signOutResponse.GetResult(), responseState))
          MetaDataManager.GetInstance().SetSignedInStatus(false);
        
        Storage.clearData();
        string result = signOutResponse.GetResult();
        string action = signOutResponse.GetAction();
        string errorCode = signOutResponse.GetErrorCode();
        string errorMessage = signOutResponse.GetErrorMessage();
        string sdkState = SamsungAccountManager.Decrypt(signOutResponse.GetState(), state);

        ((Frame)Window.Current.Content).Navigate(typeof(MainPage));

        uiMessage = $"ResponseType: {signOutResponse.ResponseType}, Result: {result}," +
            $"Action: {action}, State: {sdkState}, ErrorCode: {errorCode}, ErrorMessage: {errorMessage}.";
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
      else if (response.ResponseType == SaResponseType.signOut)
      {
        SignOutResponse signOutResponse = response as SignOutResponse;
        uiMessage = ProcessSignoutResponse(signOutResponse);
      }

      /*  UpdateUi(uiMessage);*/
    }

  }
}
