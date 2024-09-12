using Sales_system.Controls;
using Sales_system.ViewModels;
using Sales_system.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sales_system
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (!StorageSA.GetSignedStatus())
      {
        InnerFrame.Navigate(typeof(Login));
      }
      else
      {
        InnerFrame.Navigate(typeof(Welcome));
      }
      /*this.GoBackButton.IsEnabled = InnerFrame.BackStack.Any();*/
      SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = InnerFrame.BackStack.Any() ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
    }
  }
}
