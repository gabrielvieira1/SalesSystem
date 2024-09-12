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
using Windows.Foundation.Collections;
using User = Models.User;

namespace Sales_system.ViewModels
{
  internal class WelcomeViewModel : UserModel
  {
    private ICommand _signOutCommand;

    public ICommand SignOutCommand
    {
      get
      {
        return _signOutCommand ?? (_signOutCommand = new CommandHandler(async () =>
        {
          await SignOut();
        }));
      }
    }

    private async Task SignOut()
    {
      var user = GetInfoUserLoggedIn();

      if(user != null)
      {
        MetaDataManager.GetInstance().SetSignedInStatus(false);
        StorageSA.clearData();

        DataBaseUsers dataBaseUsers = new DataBaseUsers();
        await dataBaseUsers.ClearAccessToken(user.Id);

        ((Frame)Window.Current.Content).Navigate(typeof(Login));
      }      
    }
    public WelcomeViewModel() { }

    public User GetInfoUserLoggedIn()
    {
      ValueSet savedData = StorageSA.readData();

      if (savedData != null && savedData.ContainsKey("access_token"))
      {
        string accessToken = savedData["access_token"].ToString();

        if (!string.IsNullOrEmpty(accessToken))
        {
          DataBaseUsers dataBaseUsers = new DataBaseUsers();
          User loggedUser = dataBaseUsers.GetUserByAccessToken(accessToken);
          return loggedUser;
        }
      }
      return null;
    }

    public Boolean LoggedWithSA()
    {
      var user = GetInfoUserLoggedIn();

      return user.Password == null;
    }
  }
}
