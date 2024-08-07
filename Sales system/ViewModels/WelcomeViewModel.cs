using Sales_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Sales_system.Library;
using Connection;
using Models;
using System.Diagnostics;
using Sales_system.Views;
using Sales_system.Services;
using Windows.System;

namespace Sales_system.ViewModels
{
  internal class WelcomeViewModel : UserModel
  {
    private ICommand _command;

    public ICommand IniciarCommand
    {
      get
      {
        return _command ?? (_command = new CommandHandler(async () =>
        {
          await IniciarAsync();
        }));
      }
    }

    private async Task IniciarAsync()
    {
      /*se existir uma sessão, limpar o usuário aqui*/
      ((Frame)Window.Current.Content).Navigate(typeof(MainPage));

    }
    public WelcomeViewModel()
    {

    }

  }
}
