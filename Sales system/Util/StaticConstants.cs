using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace Sales_system
{
  class StaticConstants
  {
    public static string clientID = "5z959u0k0k";
    //smartthings id : j5thx0lob5
    public static string redirectUri = "com.salessystem";
    public static string msRedirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;
    public static string SetReplaceableDevicePhysicalAddressText = "";
    public static string scope = "offline.access";
  }
}
