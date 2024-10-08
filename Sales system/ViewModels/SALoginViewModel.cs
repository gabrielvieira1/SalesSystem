﻿using Sales_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Sales_system.Library;
using Connection;
using Models;
using System.Diagnostics;
using Sales_system.Views;
using Sales_system.Services;
using static LinqToDB.Common.Configuration;
using SamsungAccountLibrary;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using SamsungAccountLibrary.SAResponse;
using SamsungAccountLibrary.SARequest.AccountServerTypeRequest;
using LinqToDB.Common;
using Microsoft.VisualBasic;
using SamsungAccountLibrary.SARequest;
using System.Xml.Linq;

namespace Sales_system.ViewModels
{
  public class SALoginViewModel : ISaSdkResponse
  {
    public static String state = null;
    public static String code_verifier = null;
    private static String TEST_CLIENT_ID = "5z959u0k0k";
    private static String TEST_CLIENT_SECRET = "731DC767CFAF351549E8C269ABFC747D";
    private string accountServerType;
    private static readonly int RANDOM_STRING_LENGTH = 32;

    private ICommand _getAccessTokenCommand;
    public ICommand GetAccessTokenCommand
    {
      get
      {
        return _getAccessTokenCommand ?? (_getAccessTokenCommand = new CommandHandler(() =>
        {
          GetAccessTokenSA();
        }));
      }
    }

    private ICommand _getAccessTokenLocalCommand;
    public ICommand GetAccessTokenLocalCommand
    {
      get
      {
        return _getAccessTokenLocalCommand ?? (_getAccessTokenLocalCommand = new CommandHandler(() =>
        {
          CheckIfUserIsLoggedIn();
        }));
      }
    }

    private ICommand _signOutSACommand;
    public ICommand SignOutSACommand
    {

      get
      {
        return _signOutSACommand ?? (_signOutSACommand = new CommandHandler(() =>
        {
          SignOutSA();
        }));
      }
    }

    private ICommand _signInSACommand;
    public ICommand SignInSACommand
    {
      get
      {
        return _signInSACommand ?? (_signInSACommand = new CommandHandler(() =>
        {
          SignInSA();
        }));
      }
    }

    private ICommand _getProfileDataCommand;
    public ICommand GetProfileDataCommand
    {
      get
      {
        return _getProfileDataCommand ?? (_getProfileDataCommand = new CommandHandler(() =>
        {
          GetProfileDataSA();
        }));
      }
    }

    public SALoginViewModel()
    {
      SamsungAccountManager.SubscribeLogging();
    }

    private void SignInSA()
    {
      ValueSet signInData = new ValueSet();

      // create random code
      code_verifier = Util.StateGenerator.GenerateStateValue(50);
      state = Util.StateGenerator.GenerateStateValue(RANDOM_STRING_LENGTH);
      MetaDataManager.GetInstance().SetState(state);

      MetaDataManager.GetInstance().SetCurrentRequest(Convert.ToInt32(Constants.ServiceType.signIn));
      MetaDataManager.GetInstance().SetAuthorizationCodeVerifier(code_verifier);
      MetaDataManager.GetInstance().SetSaSDKResponseListener(this);

      SignInRequest signInDataObj = new SignInRequest(StaticConstants.redirectUri);
      signInDataObj.SetClientID(TEST_CLIENT_ID).SetCodeVerifier(code_verifier).SetScope(StaticConstants.scope).SetState(state);

      SamsungAccountManager.SignIn(signInDataObj);
    }

    private void SignOutSA()
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

    private void GetAccessTokenSA()
    {
      GetAccessTokenRequest getAccessTokenRequest = new GetAccessTokenRequest();
      getAccessTokenRequest.SetClientId(StaticConstants.clientID);

      SamsungAccountManager.GetAccessToken(this, getAccessTokenRequest);
    }

    private async void GetProfileDataSA()
    {
      GetUserProfileRequest request = new GetUserProfileRequest();
      request.SetClientId(StaticConstants.clientID)
             .SetUserId(MetaDataManager.GetInstance().GetUserId())
             .SetAccessToken(MetaDataManager.GetInstance().GetAccessToken());
      IResponse response = await SamsungAccountManager.GetUserProfileData(request);
      string result = ProcessGetProfileResponse(response);
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
        uiMessage = ProcessSigninResponse(signInResponse);
      }
      else if (response.ResponseType == SaResponseType.signOut)
      {
        SignOutResponse signOutResponse = response as SignOutResponse;
        uiMessage = ProcessSignoutResponse(signOutResponse);

      }
      else if (response.ResponseType == SaResponseType.confirmPassword)
      {
        ConfirmPasswordResponse confirmPasswordResponse = response as ConfirmPasswordResponse;
        uiMessage = ProcessConfirmPasswordResponse(confirmPasswordResponse);

      }
      else if (response.ResponseType == SaResponseType.changePassword)
      {
        ChangePasswordResponse changePasswordResponse = response as ChangePasswordResponse;
        uiMessage = ProcessChangePasswordResponse(changePasswordResponse);

      }
      else if (response.ResponseType == SaResponseType.organizerPassword)
      {
        OrganizerPasswordResponse organizerPasswordResponse = response as OrganizerPasswordResponse;
        uiMessage = ProcessOrganizerPasswordResponse(organizerPasswordResponse);

      }
      else if (response.ResponseType == SaResponseType.accessToken)
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
      else if (response.ResponseType == SaResponseType.immediateError)
      {
        ImmediateErrorResponse immediateErrorResponse = response as ImmediateErrorResponse;
        uiMessage = $"ResponseType: {immediateErrorResponse.ResponseType}, Result: {immediateErrorResponse.GetResult()}, Error Code: " +
                $"{immediateErrorResponse.GetErrorCode()}, Error Message: {immediateErrorResponse.GetErrorMessage()}";
      }
      else if (response.ResponseType == SaResponseType.accountView)
      {
        AccountViewResponse accountViewResponse = response as AccountViewResponse;
        uiMessage = $"ResponseType: {accountViewResponse.ResponseType}, Result: {accountViewResponse.GetResult()}, Error Code: " +
                $"{accountViewResponse.GetErrorCode()}, Error Message: {accountViewResponse.GetErrorMessage()}";
      }
    }

    String ProcessSigninResponse(SignInResponse signInResponse)
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

          GetAccessTokenSA();

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

    String ProcessSignoutResponse(SignOutResponse signOutResponse)
    {
      String uiMessage;
      try
      {
        String responseState = SamsungAccountManager.Decrypt(signOutResponse.GetState(), state);
        string result = signOutResponse.GetResult();
        string action = signOutResponse.GetAction();
        string errorCode = signOutResponse.GetErrorCode();
        string errorMessage = signOutResponse.GetErrorMessage();
        string sdkState = SamsungAccountManager.Decrypt(signOutResponse.GetState(), state);

        WelcomeViewModel welcomeViewModel = new WelcomeViewModel();
        welcomeViewModel.SignOutCommand.Execute(null);

        uiMessage = $"ResponseType: {signOutResponse.ResponseType}, Result: {result}," +
            $"Action: {action}, State: {sdkState}, ErrorCode: {errorCode}, ErrorMessage: {errorMessage}.";
      }
      catch (Exception ex)
      {
        uiMessage = ex.Message;
      }

      return uiMessage;
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

        AddUserSA(accessTokenResponse.GetAccessToken(), accessTokenResponse.GetAccessTokenExpiresIn());

      }
      else
      {
        uiMessage = $"ResponseType: {accessTokenResponse.ResponseType}, Result: {accessTokenResponse.GetResult()}, Error Code: " +
            $"{accessTokenResponse.GetErrorCode()}, Error Message: {accessTokenResponse.GetErrorMessage()}.";
      }

      return uiMessage;
    }

    private async void AddUserSA(string accessToken, string accessTokenExpires)
    {
      GetUserProfileRequest request = new GetUserProfileRequest();
      request.SetClientId(StaticConstants.clientID)
             .SetUserId(MetaDataManager.GetInstance().GetUserId())
             .SetAccessToken(MetaDataManager.GetInstance().GetAccessToken());
      IResponse response = await SamsungAccountManager.GetUserProfileData(request);

      var profileResponse = response as GetUserProfileResponse;

      DataBaseUsers dataBaseUsers = new DataBaseUsers();
      await dataBaseUsers.CreateDataBase();

      User existingUser = dataBaseUsers.GetUserByEmail(profileResponse.GetEmailLoginID());

      if (existingUser != null)
      {
        dataBaseUsers.UpdateUserTokens(existingUser.Id, accessToken, accessTokenExpires);
      }
      else
      {
        User user = new User()
        {
          Name = profileResponse.GetUserName(),
          Email = profileResponse.GetEmailLoginID(),
          AccessToken = accessToken,
          AccessTokenExpires = accessTokenExpires,
          Active = true
        };
        await dataBaseUsers.AddUser(user);
      }
      StorageSA.saveData();

      ((Frame)Window.Current.Content).Navigate(typeof(Welcome));
    }

    private void CheckIfUserIsLoggedIn()
    {

      var status = StorageSA.GetSignedStatus();

      // Recupera os dados salvos, incluindo o AccessToken
      ValueSet savedData = StorageSA.readData();

      if (savedData != null && savedData.ContainsKey("access_token"))
      {
        string accessToken = savedData["access_token"].ToString();

        // Se o access_token existir e não estiver vazio, considerar o usuário logado
        if (!string.IsNullOrEmpty(accessToken))
        {
          // Buscar o userId armazenado
          int userId = int.Parse(savedData["userId"].ToString());

          DataBaseUsers dataBaseUsers = new DataBaseUsers();
          User loggedUser = dataBaseUsers.GetUserById(userId);

          // Redireciona o usuário logado para a tela de Welcome
          Debug.WriteLine("User already logged in: " + loggedUser.Name);
          ((Frame)Window.Current.Content).Navigate(typeof(Welcome), loggedUser);
        }
      }
    }

    String ProcessConfirmPasswordResponse(ConfirmPasswordResponse confirmPasswordResponse)
    {
      String uiMessage;
      try
      {
        String responseState = SamsungAccountManager.Decrypt(confirmPasswordResponse.GetState(), state);
        if (!responseState.Equals(state))
        {
          uiMessage = "Man in middle attack";
          return uiMessage;
        }
        string result = confirmPasswordResponse.GetResult();
        string action = confirmPasswordResponse.GetAction();
        string sdkState = SamsungAccountManager.Decrypt(confirmPasswordResponse.GetState(), state);
        uiMessage = $"ResponseType: {confirmPasswordResponse.ResponseType}, Result: {result}," +
            $"Action: {action}, State: {sdkState}.";
      }
      catch (Exception ex)
      {
        uiMessage = ex.Message;
      }
      return uiMessage;
    }

    String ProcessChangePasswordResponse(ChangePasswordResponse changePasswordResponse)
    {
      String uiMessage;
      try
      {
        String responseState = SamsungAccountManager.Decrypt(changePasswordResponse.GetState(), state);
        if (!responseState.Equals(state))
        {
          uiMessage = "Man in middle attack";
          return uiMessage;
        }
        string result = changePasswordResponse.GetResult();
        string action = changePasswordResponse.GetAction();
        string sdkState = SamsungAccountManager.Decrypt(changePasswordResponse.GetState(), state);
        uiMessage = $"ResponseType: {changePasswordResponse.ResponseType}, Result: {result}," +
            $"Action: {action}, State: {sdkState}.";
      }
      catch (Exception ex)
      {
        uiMessage = ex.Message;
      }
      return uiMessage;
    }

    String ProcessOrganizerPasswordResponse(OrganizerPasswordResponse organizerPasswordResponse)
    {
      String uiMessage;
      try
      {
        string result = organizerPasswordResponse.GetResult();
        string action = organizerPasswordResponse.GetAction();
        uiMessage = $"ResponseType: {organizerPasswordResponse.ResponseType}, Result: {result}, Action: {action}.";
      }
      catch (Exception ex)
      {
        uiMessage = ex.Message;
      }
      return uiMessage;
    }

    String ProcessGetProfileResponse(IResponse response)
    {
      if (Utils.IsSuccessed(response.GetResult()))
      {
        var profileResponse = response as GetUserProfileResponse;
        return $"ResponseType: {profileResponse.ResponseType}, Result: {profileResponse.GetResult()}, EmailID: {profileResponse.GetEmailLoginID()}," +
            $" PhoneId: {profileResponse.GetPhoneLoginID()}, Birthdate: {profileResponse.GetBirthday()}, UserId: {profileResponse.GetUserID()}," +
            $" UserDisplayName: {profileResponse.GetUserDisplayName()}.";
      }
      else
      {
        return $"ResponseType: {response.ResponseType}, Result: {response.GetResult()}, Error Code: " +
            $"{response.GetErrorCode()}, Error Message: {response.GetErrorMessage()},";
      }
    }

    String ProcessFamilyDataResponse(IResponse response)
    {
      if (Utils.IsSuccessed(response.GetResult()))
      {
        var getFamilyDataResponse = response as GetFamilyDataResponse;
        Family family = getFamilyDataResponse.GetFamilyData();
        StringBuilder sb = new StringBuilder($" Family Owner: {family.GroupOwner} Family members size: {family.Members.Count}, ");
        sb.AppendLine();
        foreach (var member in family.Members)
        {
          sb.Append($"Name: {member.FullName}, ");
          sb.Append($"MemberType: {member.MemberType}, ");
          sb.Append($"UserId: {member.UserId}");
          sb.AppendLine();
        }
        return sb.ToString();
      }
      else
      {
        return $"ResponseType: {response.ResponseType}, Result: {response.GetResult()}," +
            $" Error Code: {response.GetErrorCode()}, Error Message: {response.GetErrorMessage()}";
      }
    }

    String ProcessLoginIdResponse(IResponse response)
    {
      if (Utils.IsSuccessed(response.GetResult()))
      {
        var loginIdResponse = response as GetLoginIdResponse;
        return $"ResponseType: {loginIdResponse.ResponseType}, Result: {loginIdResponse.GetResult()} LoginID {loginIdResponse.GetLoginId()}";
      }
      else
      {
        return $"ResponseType: {response.ResponseType}, Result: {response.GetResult()}," +
            $" Error Code: {response.GetErrorCode()}, Error Message: {response.GetErrorMessage()}";
      }
    }
  }
}
