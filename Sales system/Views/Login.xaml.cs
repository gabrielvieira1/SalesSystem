using Models;
using Sales_system.ViewModels;
using SamsungAccountLibrary.SARequest;
using SamsungAccountLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Sales_system
{
  public sealed partial class Login : Page
  {
    public static String state = null;
    public static String code_verifier = null;
    private static String TEST_CLIENT_ID = "5z959u0k0k";
    private static String TEST_CLIENT_SECRET = "731DC767CFAF351549E8C269ABFC747D";
    private string accountServerType;
    private static readonly int RANDOM_STRING_LENGTH = 32;

    public Login()
    {
      InitializeComponent();
      Object[] campos = { Email, Password };
      DataContext = new LoginViewModel(campos);
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Frame.Navigate(typeof(Signup));
    }

    private void LoginWithSA_Click(object sender, RoutedEventArgs e)
    {
      InitializeComponent();
      SALoginViewModel salogin = new SALoginViewModel();
      salogin.SignInCommand.Execute(null);
    }
  }
}
