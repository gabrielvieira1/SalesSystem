using Microsoft.Data.Sqlite;
using Models;
using Sales_system.Util;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;


namespace Sales_system
{
  public static class StorageSA
  {
    private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
    public static void SetSignedStatus(bool signedIn)
    {
      localSettings.Values["SignedInStatus"] = signedIn;
    }
    public static bool GetSignedStatus()
    {
      return localSettings.Values.ContainsKey("SignedInStatus") ? (bool)localSettings.Values["SignedInStatus"] : false;
    }
    public static void saveData()
    {
      ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
      composite["access_token"] = MetaDataManager.GetInstance().GetAccessToken();
      composite["refresh_token"] = MetaDataManager.GetInstance().GetRefreshToken();
      composite["userId"] = MetaDataManager.GetInstance().GetUserId();
      composite["auth_code"] = MetaDataManager.GetInstance().GetAuthorizationCode();
      composite["auth_server_url"] = MetaDataManager.GetInstance().GetAuthServerUrl();
      composite["code_verifier"] = MetaDataManager.GetInstance().GetAuthorizationCodeVerifier();

      localSettings.Values["dataCompositeSetting"] = composite;
    }
    public static ValueSet readData()
    {
      ApplicationDataCompositeValue composite =
         (ApplicationDataCompositeValue)localSettings.Values["dataCompositeSetting"];
      ValueSet bundle = new ValueSet();
      if (composite == null)
      {
        return bundle;
      }
      else
      {

        bundle["access_token"] = composite.GetValue("access_token", "").ToString();
        bundle["refresh_token"] = composite.GetValue("refresh_token", "").ToString();
        bundle["userId"] = composite.GetValue("userId", "").ToString();
        bundle["auth_code"] = composite.GetValue("auth_code", "").ToString();
        bundle["auth_server_url"] = composite.GetValue("auth_server_url", "").ToString();
        bundle["code_verifier"] = composite.GetValue("code_verifier", "").ToString();

      }
      return bundle;
    }
    public static void clearData()
    {
      ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
      composite["access_token"] = "";
      composite["refresh_token"] = "";
      composite["userId"] = "";
      composite["auth_code"] = "";
      composite["auth_server_url"] = "";
      composite["code_verifier"] = "";

      localSettings.Values["dataCompositeSetting"] = composite;
    }
    public static void deleteComposite()
    {
      // Delete a composite setting

      localSettings.Values.Remove("dataCompositeSetting");
    }
    public static void populateMetaData()
    {
      ValueSet bundle = null;
      bundle = readData();
      if (bundle != null)
      {
        MetaDataManager.GetInstance().SetAccessToken(bundle.GetValue("access_token", "").ToString());
        MetaDataManager.GetInstance().SetRefreshToken(bundle.GetValue("refresh_token", "").ToString());
        MetaDataManager.GetInstance().SetUserId(bundle.GetValue("userId", "").ToString());
        MetaDataManager.GetInstance().SetAuthorizationCode(bundle.GetValue("auth_code", "").ToString());
        MetaDataManager.GetInstance().SetAuthServerUrl(bundle.GetValue("auth_server_url", "").ToString());
        MetaDataManager.GetInstance().SetAuthorizationCodeVerifier(bundle.GetValue("code_verifier", "").ToString());

      }
    }

    public static void clearMetaData()
    {
      MetaDataManager.GetInstance().SetAccessToken("");
      MetaDataManager.GetInstance().SetRefreshToken("");
      MetaDataManager.GetInstance().SetUserId("");
      MetaDataManager.GetInstance().SetAuthorizationCode("");
      MetaDataManager.GetInstance().SetAuthServerUrl("");
    }

  }
}
