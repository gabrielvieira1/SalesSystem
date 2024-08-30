using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_system.Util
{
  class Constants
  {
    public static readonly Boolean isDebug = true;
    public static readonly Boolean isStaging = false;
    public static readonly Boolean isNoProxy = true;

    public static readonly String RESULT = "RESULT";

    public static class ServiceType
    {
      public static readonly int none = 100;
      public static readonly int checkDomain = 101;
      public static readonly int getEntryPointOfIDM = 102;
      public static readonly int signUp = 103;
      public static readonly int signUpWithPartnerAccount = 104;
      public static readonly int signIn = 105;
      public static readonly int confirmPassword = 106;
      public static readonly int getUserProfileData = 107;
      public static readonly int signOut = 108;
      public static readonly int changePassword = 109;
      public static readonly int validateSAUser = 110;
      public static readonly int getAccessToken = 111;
      public static readonly int OrganizerPassword = 112;
    }

    public static class ThirdParty
    {
      public static class Request
      {
        public static readonly String clientId = "client_id";
        public static readonly String clientSecret = "client_secret";
        public static readonly String accessToken = "access_token";
        public static readonly String replacableClientId = "replaceable_client_id";
        public static readonly String redirectUri = "redirect_uri";
        public static readonly String iptLoginId = "ipt_login_id";
        public static readonly String birthDate = "birth_date";
        public static readonly String firstName = "first_name";
        public static readonly String lastName = "last_name";
        public static readonly String replaceableDevicePhysicalAddressText = "replaceable_device_physical_address_text";
        public static readonly String scope = "scope";
        public static readonly String state = "state";
        public static readonly String refreshToken = "refresh_token";
        public static readonly String auth_code = "auth_code";
        public static readonly String codeVerifier = "code_verifier";
        public static readonly String physicalAddressText = "physical_address_text";
        public static readonly String id_token = "id_token";
      }

      public static class Response
      {
        public static readonly String authCode = "auth_code";
        public static readonly String codeExpiresIn = "code_expires_in";
        public static readonly String apiServerUrl = "api_server_url";
        public static readonly String authServerUrl = "auth_server_url";
        public static readonly String result = "result";
        public static readonly String message = "message";
      }

      public static class Result
      {
        public static readonly String TRUE = "true";
        public static readonly String FALSE = "false";
      }

      public static class Message
      {
        public static readonly String noBrowsersAvailable = "No usable browsers available !!!";
      }
      public static class GoogleCredential
      {
        public const String client_id = "420276374420-2qlp6jhthl8juuanlg9h45rc4ikf1sgm.apps.googleusercontent.com";
        public const String redirect_uri = "urn:ietf:wg:oauth:2.0:oob";
      }
    }

    public static class ResponseParameters
    {
      public const String authCode = "auth_code";
      public const String errorCode = "error_code";
      public static readonly String codeExpiresIn = "code_expires_in";
      public static readonly String apiServerUrl = "api_server_url";
      public static readonly String authServerUrl = "auth_server_url";
      public static readonly String state = "state";
      public static readonly String scope = "scope";
      public static readonly String result = "result";
      public static readonly String signInURI = "signInURI";
      public static readonly String signUpURI = "signUpURI";
      public static readonly String confirmPWURI = "confirmPWURI";
      public static readonly String signOutURI = "signOutURI";
      public static readonly String chkDoNum = "chkDoNum";
      public static readonly String pkiPublicKey = "pkiPublicKey";
      public static readonly String pbeKySpcIters = "pbeKySpcIters";
      public static readonly String iptLoginId = "ipt_login_id";
      public static readonly String birthDate = "birth_date";
      public static readonly String firstName = "first_name";
      public static readonly String lastName = "last_name";
      public static readonly String retValue = "retValue";
      public static readonly String message = "message";
      public static readonly String action = "action";
    }
  }
}
