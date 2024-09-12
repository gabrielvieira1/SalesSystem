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
using System.Threading.Tasks;
using Sales_system.Views;
using Windows.UI.Core;


namespace Sales_system
{
  public sealed partial class Login : Page
  {
    public Login()
    {
      InitializeComponent();
      Object[] campos = { Email, Password };
      DataContext = new LoginViewModel(campos);
    }

    private void LoginWithSA_Click(object sender, RoutedEventArgs e)
    {
      InitializeComponent();
      SALoginViewModel salogin = new SALoginViewModel();
      salogin.SignInSACommand.Execute(null);
    }

    private void Signup_Click(object sender, RoutedEventArgs e)
    {
      Frame.Navigate(typeof(Signup));
    }
  }
}
