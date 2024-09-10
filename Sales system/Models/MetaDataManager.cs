using LinqToDB.Common;
using SamsungAccountLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_system
{
  internal class MetaDataManager
  {
    private static MetaDataManager mMetaDataManager = null;
    private String state;
    private int mCurrentRequest;
    private ISaSdkResponse mSaSDKResponseListener;
    private static readonly object padlock = new object();
    private String authorizationCode;
    private String authCodeVerifier;
    private String refreshToken;
    private String accessToken;
    private String userId;
    private String authUrl;
    private String iptLoginId;
    private String firstName;
    private String lastName;
    private String birthDate;
    private bool isStaging;
    private bool? signedInStatus;
    private string serverType;
    private MetaDataManager() { }

    public static MetaDataManager GetInstance()
    {
      lock (Padlock)
      {
        if (mMetaDataManager == null)
        {
          mMetaDataManager = new MetaDataManager();
        }
        return mMetaDataManager;
      }
    }
    public void SetServerType(string value)
    {
      serverType = value;
    }
    public string GetServerType()
    {
      return serverType;
    }
    public void SetSignedInStatus(bool signedIn)
    {
      this.signedInStatus = signedIn;
      StorageSA.SetSignedStatus(signedIn);
    }
    public bool GetSignedInStatus()
    {
      return signedInStatus ?? StorageSA.GetSignedStatus();
    }
    public ISaSdkResponse MSaSDKResponseListener { get => mSaSDKResponseListener; set => mSaSDKResponseListener = value; }

    public static object Padlock => padlock;

    public Boolean IsValidState(String value)
    {
      if (value.Any())
      {
        if (state.Equals(value))
          return true;
        else
          return false;
      }
      else
        return false;

    }
    public void SetIsStaging(bool isStaged)
    {
      this.isStaging = isStaged;

    }
    public bool GetIsStaging()
    {
      return isStaging;
    }
    public string GetIptLoginId()
    {
      return iptLoginId;
    }
    public string GetFirstName()
    {
      return firstName;
    }
    public string GetLastName()
    {
      return lastName;
    }
    public String GetState() { return state; }

    public void SetState(String value) { state = value; }

    public int GetCurrentRequest() { return mCurrentRequest; }

    public void SetCurrentRequest(int request) { mCurrentRequest = request; }

    public ISaSdkResponse GetSaSDKResponseListener() { return MSaSDKResponseListener; }

    public void SetSaSDKResponseListener(ISaSdkResponse request)
    {
      MSaSDKResponseListener = request;
    }

    public void SetIptLoginId(String loginId)
    {
      this.iptLoginId = loginId;
    }
    public void SetFirstName(String firstName)
    {
      this.firstName = firstName;
    }
    public void SetLastName(String lastName)
    {
      this.lastName = lastName;
    }
    public void SetAuthorizationCode(String code)
    {
      this.authorizationCode = code;
    }
    public String GetAuthorizationCode()
    {
      return this.authorizationCode;
    }
    public void SetAuthorizationCodeVerifier(String codeVerifier)
    {
      this.authCodeVerifier = codeVerifier;
    }
    public String GetAuthorizationCodeVerifier()
    {
      return this.authCodeVerifier;
    }
    public void SetRefreshToken(String refreshToken)
    {
      this.refreshToken = refreshToken;
    }
    public String GetRefreshToken()
    {
      return this.refreshToken;
    }
    public void SetAccessToken(String accessToken)
    {
      this.accessToken = accessToken;
    }
    public String GetAccessToken()
    {
      return this.accessToken;
    }
    public void SetUserId(String userId)
    {
      this.userId = userId;
    }
    public String GetUserId()
    {
      return this.userId;
    }

    public void SetAuthServerUrl(String authUrl)
    {
      if (Utils.IsValidServerUrl(authUrl))
      {
        this.authUrl = authUrl;
      }
    }
    public String GetAuthServerUrl()
    {
      return this.authUrl;
    }

    public String GetHttpProtocol()
    {
      //if (isStaging)
      //    return "http://";
      //else
      return "https://";

    }

    public void SetBirthDate(String birthDate)
    {
      this.birthDate = birthDate;
    }
    public String GetBirthDate()
    {
      return birthDate;
    }
  }
}
