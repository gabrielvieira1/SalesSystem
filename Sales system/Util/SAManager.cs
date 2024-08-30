/*
  * Copyright (c) 2000-2018 Samsung Electronics Co., Ltd All Rights Reserved
  *
  * PROPRIETARY/CONFIDENTIAL
  *
  * This software is the confidential and proprietary information of
  * SAMSUNG ELECTRONICS ("Confidential Information").
  * You shall not disclose such Confidential Information and shall
  * use it only in accordance with the terms of the license agreement
  * you entered into with SAMSUNG ELECTRONICS.
  * SAMSUNG make no representations or warranties about the suitability
  * of the software, either express or implied, including but not
  * limited to the implied warranties of merchantability, fitness for
  * a particular purpose, or non-infringement.
  * SAMSUNG shall not be liable for any damages suffered by licensee as
  * a result of using, modifying or distributing this software or its derivatives.
*/
using Connection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation.Collections;
using Windows.Web.Http;

namespace Sales_system.Util
{
  public static class SAManager
  {
    internal static async Task<ValueSet> GetAccessTokenAsync(ValueSet bundle)
    {
      HttpRequestMessage request = new HttpRequestMessage(); ;
      var mParams = new Dictionary<String, String>();

      mParams["grant_type"] = "authorization_code";
      mParams["client_id"] = bundle[Constants.ThirdParty.Request.clientId].ToString();
      mParams["client_secret"] = bundle[Constants.ThirdParty.Request.clientSecret].ToString();
      mParams["code"] = bundle[Constants.ThirdParty.Request.auth_code].ToString();
      mParams["code_verifier"] = bundle[Constants.ThirdParty.Request.codeVerifier].ToString();
      mParams["physical_address_text"] = bundle[Constants.ThirdParty.Request.physicalAddressText].ToString();
      if (mParams.Any()) request.Content = new HttpFormUrlEncodedContent(mParams);

      request.Method = HttpMethod.Post;

      String url = MetaDataManager.GetInstance().GetHttpProtocol() + MetaDataManager.GetInstance().GetAuthServerUrl() + "/auth/oauth2/token";
      Uri urlObj = null;
      Uri.TryCreate(url.Trim(), UriKind.Absolute, out urlObj);
      request.RequestUri = urlObj;

      HttpClient httpClient = new HttpClient();
      CancellationTokenSource cts = new CancellationTokenSource();
      //cts.CancelAfter(10000); // to set timeout in case of http request fail

      String strContent;
      ValueSet result = null;
      try
      {
        var response = await httpClient.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask(cts.Token);
        strContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
          result = new ValueSet();
          JsonObject jObject = JsonObject.Parse(strContent);
          result["access_token"] = jObject.GetNamedString("access_token").ToString();
          result["token_type"] = jObject.GetNamedString("token_type").ToString();
          result["access_token_expires_in"] = jObject.GetNamedValue("access_token_expires_in").ToString();
          result["expires_in"] = jObject.GetNamedValue("expires_in").ToString();
          result["refresh_token"] = jObject.GetNamedString("refresh_token").ToString();
          result["refresh_token_expires_in"] = jObject.GetNamedValue("refresh_token_expires_in").ToString();
          result["userId"] = jObject.GetNamedString("userId").ToString();
        }
      }
      catch (Exception e)
      {
        result = null;
      }
      finally
      {
        httpClient.Dispose();
        request.Dispose();
        cts.Dispose();
      }

      return result;

    }

    internal static async Task<ValueSet> GetAccessTokenUsingRefreshTokenAsync(ValueSet bundle)
    {
      HttpRequestMessage request = new HttpRequestMessage(); ;
      var mParams = new Dictionary<String, String>();

      mParams["grant_type"] = "refresh_token";
      mParams["client_id"] = bundle[Constants.ThirdParty.Request.clientId].ToString();
      mParams["client_secret"] = bundle[Constants.ThirdParty.Request.clientSecret].ToString();
      mParams["refresh_token"] = bundle[Constants.ThirdParty.Request.refreshToken].ToString();
      if (mParams.Any()) request.Content = new HttpFormUrlEncodedContent(mParams);

      request.Method = HttpMethod.Post;

      String url = MetaDataManager.GetInstance().GetHttpProtocol() + MetaDataManager.GetInstance().GetAuthServerUrl() + "/auth/oauth2/token";
      Uri urlObj = null;
      Uri.TryCreate(url.Trim(), UriKind.Absolute, out urlObj);
      request.RequestUri = urlObj;

      HttpClient httpClient = new HttpClient();
      var cts = new CancellationTokenSource();
      //cts.CancelAfter(10000); // to set timeout in case of http request fail

      String strContent;
      ValueSet result = null;
      try
      {
        var response = await httpClient.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask(cts.Token);
        strContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
          result = new ValueSet();
          JsonObject jObject = JsonObject.Parse(strContent);
          result["access_token"] = jObject.GetNamedString("access_token").ToString();
          result["token_type"] = jObject.GetNamedString("token_type").ToString();
          result["access_token_expires_in"] = jObject.GetNamedValue("access_token_expires_in").ToString();
          result["expires_in"] = jObject.GetNamedValue("expires_in").ToString();
          result["refresh_token"] = jObject.GetNamedString("refresh_token").ToString();
          result["refresh_token_expires_in"] = jObject.GetNamedValue("refresh_token_expires_in").ToString();
        }
      }
      catch (Exception e)
      {
        throw new TimeoutException();
      }

      finally
      {
        httpClient.Dispose();
        request.Dispose();
        cts.Dispose();

      }

      return result;
    }

    //internal static async Task<ValueSet> ValidateAccessTokenAsync(ValueSet bundle)
    //{
    //    HttpRequestMessage request = new HttpRequestMessage();
    //    var mHeaders = new Dictionary<string, string>();

    //    request.Method = HttpMethod.Get;

    //    string myAccessToken = bundle[SamsungAccountLibrary.SaConstants.ThirdParty.Request.accessToken].ToString();
    //    string client_id = bundle[SamsungAccountLibrary.SaConstants.ThirdParty.Request.clientID].ToString();
    //    string client_secret = bundle[SamsungAccountLibrary.SaConstants.ThirdParty.Request.clientSecret].ToString();

    //    String url = MetaDataManager.GetInstance().GetHttpProtocol() + MetaDataManager.GetInstance().GetAuthServerUrl() + "/v2/license/security/"
    //    + "authorizeToken" + "?authToken=" + myAccessToken;
    //    Uri urlObj = null;
    //    Uri.TryCreate(url.Trim(), UriKind.Absolute, out urlObj);
    //    request.RequestUri = urlObj;

    //    HttpClient httpClient = new HttpClient();
    //    HttpResponseMessage response = null;
    //    CancellationTokenSource cts = new CancellationTokenSource();
    //    //cts.CancelAfter(10000); // to set timeout in case of http request fail

    //    String id = client_id + ":" + client_secret;
    //    String encodedID = ""; if (id == null) encodedID = "";
    //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(id);
    //    encodedID = Encode(plainTextBytes);
    //    mHeaders["Authorization"] = "Basic " + encodedID;
    //    mHeaders["x-osp-appId"] = client_id;

    //    var headers = httpClient.DefaultRequestHeaders;
    //    foreach (var header in mHeaders)
    //        headers.TryAppendWithoutValidation(header.Key, header.Value);

    //    String strContent;
    //    ValueSet responseData = null;
    //    try
    //    {
    //        response = await httpClient.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask(cts.Token);
    //        strContent = await response.Content.ReadAsStringAsync();

    //        if (response.IsSuccessStatusCode)
    //        {
    //            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(AuthorizeTokenResultVO));
    //            AuthorizeTokenResultVO authorizationRes = null;
    //            using (StringReader sr = new StringReader(strContent))
    //            {
    //                authorizationRes = (AuthorizeTokenResultVO)ser.Deserialize(sr);
    //            }
    //            responseData = new ValueSet
    //            {
    //                ["authorizeCode"] = authorizationRes.authorizeCode,
    //                ["authorizeDesc"] = authorizationRes.authorizeDesc,
    //                ["authenticateUserID"] = authorizationRes.authenticateUserID,
    //                ["remainExpireTimeSec"] = authorizationRes.remainExpireTimeSec,
    //            };

    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        responseData = null;
    //    }

    //    finally
    //    {
    //        httpClient.Dispose();
    //        request.Dispose();
    //        cts.Dispose();
    //    }

    //    return responseData;
    //}

    internal static async Task<ValueSet> RevokeTokenAsync(ValueSet bundle)
    {
      HttpRequestMessage request = new HttpRequestMessage(); ;
      var mParams = new Dictionary<String, String>();

      mParams["client_id"] = bundle[Constants.ThirdParty.Request.clientId].ToString();
      mParams["client_secret"] = bundle[Constants.ThirdParty.Request.clientSecret].ToString();
      if (bundle.ContainsKey(Constants.ThirdParty.Request.accessToken))
      {
        mParams["access_token"] = bundle[Constants.ThirdParty.Request.accessToken].ToString();
      }
      else if (bundle.ContainsKey(Constants.ThirdParty.Request.refreshToken))
      {
        mParams["refresh_token"] = bundle[Constants.ThirdParty.Request.refreshToken].ToString();
      }
      if (mParams.Any()) request.Content = new HttpFormUrlEncodedContent(mParams);

      request.Method = HttpMethod.Post;

      String url = MetaDataManager.GetInstance().GetHttpProtocol() + MetaDataManager.GetInstance().GetAuthServerUrl() + "/auth/oauth2/revoke";
      Uri urlObj = null;
      Uri.TryCreate(url.Trim(), UriKind.Absolute, out urlObj);
      request.RequestUri = urlObj;

      HttpClient httpClient = new HttpClient();
      CancellationTokenSource cts = new CancellationTokenSource();

      String strContent;
      ValueSet result = null;
      try
      {
        var response = await httpClient.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask(cts.Token);
        strContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
          result = new ValueSet();
          JsonObject jObject = JsonObject.Parse(strContent);
          result["result"] = jObject.GetNamedString("result").ToString();
        }
        else
        {
          result = new ValueSet();
          JsonObject jObject = JsonObject.Parse(strContent);
          result["result"] = "false";
          result["error"] = jObject.GetNamedString("error").ToString();
          result["error_code"] = jObject.GetNamedString("error_code").ToString();
          result["error_description"] = jObject.GetNamedString("error_description").ToString();
        }
      }
      catch (Exception e)
      {
        result = null;
      }
      finally
      {
        httpClient.Dispose();
        request.Dispose();
        cts.Dispose();
      }

      return result;
    }

    private const string STG_URL = "https://stg-account.samsung.com/accounts/WINSDK/signInGate";
    private const string STG2_URL = "https://stg-us.account.samsung.com/accounts/WINSDK/signInGate";
    private const string PRD_URL = "https://us.account.samsung.com/accounts/WINSDK/signInGate";

    internal static async Task<ValueSet> TestApiForCWG(ValueSet bundle)
    {
      HttpRequestMessage request = new HttpRequestMessage(); ;
      var mParams = new Dictionary<String, String>();

      // Query parameters
      //    - code
      //    - id_token
      //    - redirect_uri
      //    - state

      mParams["google_code"] = bundle[Constants.ThirdParty.Request.auth_code].ToString();
      mParams["state"] = bundle[Constants.ThirdParty.Request.state].ToString();
      mParams["google_redirect_uri"] = bundle[Constants.ThirdParty.Request.redirectUri].ToString();
      mParams["google_id_token"] = bundle[Constants.ThirdParty.Request.id_token].ToString();

      String url = "";
      string server = MetaDataManager.GetInstance().GetServerType().ToUpper();

      switch (server)
      {
        case "STG":
          url = STG_URL;
          break;
        case "STG2":
          url = STG2_URL;
          break;
        case "PRD":
          url = PRD_URL;
          break;
        default:
          break;
      }

      url += string.Format($"?google_code={mParams["google_code"]}&state={mParams["state"]}&google_id_token={mParams["google_id_token"]}&google_redirect_uri={mParams["google_redirect_uri"]}");

      Uri urlObj = null;
      Uri.TryCreate(url.Trim(), UriKind.Absolute, out urlObj);

      request.Method = HttpMethod.Get;
      request.RequestUri = urlObj;

      HttpClient httpClient = new HttpClient();

      String strContent;
      ValueSet result = null;
      try
      {
        var response = await httpClient.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead);
        strContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
          result = new ValueSet();
          JsonObject jObject = JsonObject.Parse(strContent);
        }
      }
      catch (Exception e)
      {
        result = null;
      }
      finally
      {
        httpClient.Dispose();
        request.Dispose();
      }

      return result;
    }


    private static string Encode(byte[] bytesToEncode)
    { return (bytesToEncode == null) ? "" : System.Convert.ToBase64String(bytesToEncode); }

  }
}