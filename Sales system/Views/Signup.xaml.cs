﻿using Sales_system.ViewModels;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Sales_system
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class Signup : Page
  {
    public Signup()
    {
      InitializeComponent();
      Object[] campos = { Name, Email, Password, ConfirmPassword };
      DataContext = new SignupViewModel(campos);
    }

    private void Signin_Click(object sender, RoutedEventArgs e)
    {
      Frame.Navigate(typeof(Login));
    }
  }
}
