using Sales_system.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Sales_system.Util;
using SamsungAccountLibrary;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Sales_system
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class Login : Page
  {
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

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      InitializeComponent();
      DataContext = new SALoginViewModel();
      /*Debug.Write("rtyuio");
      DataContext = new GetAccessTokenViewModel();*/

    }
  }
}
