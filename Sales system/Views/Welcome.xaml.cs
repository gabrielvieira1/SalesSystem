using Models;
using Sales_system.Controls;
using Sales_system.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    private SALoginViewModel sALoginViewModel = null;
    private WelcomeViewModel welcomeViewModel = null;
    private LoginViewModel loginViewModel = null;

    public Welcome()
    {
      this.InitializeComponent();
      sALoginViewModel = new SALoginViewModel();
      welcomeViewModel = new WelcomeViewModel();
    }

    private async Task ShowLoadingAndNavigateAsync(Type pageType)
    {
      LoadingControl.Show();
      await Task.Delay(2000);
      LoadingControl.Hide();

      /* Frame.Navigate(pageType);*/
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
      await ShowLoadingAndNavigateAsync(typeof(Welcome));

      base.OnNavigatedTo(e);


      var user = welcomeViewModel.GetInfoUserLoggedIn();

      if (user != null)
      {
        WelcomeTextBlock.Text = "Bem vindo, " + user.Name;
      }
    }

    private async void SignOut_Click(object sender, RoutedEventArgs e)
    {
      ContentDialogResult result = await ConfirmationDialog.ShowAsync();

      if (result == ContentDialogResult.Primary)
      {
        // O usuário confirmou a ação
        if (welcomeViewModel.LoggedWithSA())
          sALoginViewModel.SignOutSACommand.Execute(null);
        else
          welcomeViewModel.SignOutCommand.Execute(null);
      }
      /*else
      {
        // O usuário cancelou a ação
        CancelUserAction();
      }*/
    }

    private void GetAccessToken_Click(object sender, RoutedEventArgs e)
    {
      sALoginViewModel.GetAccessTokenCommand.Execute(null);
    }

    private void GetProfileData_Click(object sender, RoutedEventArgs e)
    {
      sALoginViewModel.GetProfileDataCommand.Execute(null);
    }

    private void GetAccessLocal_Click(object sender, RoutedEventArgs e)
    {
      sALoginViewModel.GetAccessTokenLocalCommand.Execute(null);
    }
  }
}
