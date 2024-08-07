using Models;
using Sales_system.ViewModels;
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


namespace Sales_system.Views
{
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

    /*protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      if (e.Parameter is User user)
      {

      }
    }*/
  }
}
