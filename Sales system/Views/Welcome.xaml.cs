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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Sales_system.Views
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class Welcome : Page
  {

    public Welcome()
    {
      this.InitializeComponent();
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);


      if (e.Parameter is User user)
      {
        WelcomeTextBlock.Text = "Bem vindo, " + user.Name.ToString();
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Frame.Navigate(typeof(MainPage));
    }
  }
}
