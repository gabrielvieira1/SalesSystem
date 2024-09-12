using System;
using System.Collections.Generic;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Sales_system.Controls
{
  public sealed partial class ConfirmActionControl : UserControl
  {
    public ConfirmActionControl()
    {
      this.InitializeComponent();
    }

    public void ConfirmUserAction()
    {
    }

    private void CancelUserAction()
    {
    }

    public async void OnActionRequested(object sender, RoutedEventArgs e)
    {
      ContentDialogResult result = await ConfirmationDialog.ShowAsync();

      if (result == ContentDialogResult.Primary)
      {
        ConfirmUserAction();
      }
      else
      {
        CancelUserAction();
      }
    }
  }
}
