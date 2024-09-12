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
using static LinqToDB.Common.Configuration;
using Windows.Foundation.Collections;
using Sales_system.Util;

namespace Sales_system.ViewModels
{
  public class LoginViewModel : UserModel
  {
    private TextBox _textBoxEmail;
    private PasswordBox _textBoxPass;
    private string date = DateTime.Now.ToString("dd/MM/yyy");
    private Frame rootFrame = Window.Current.Content as Frame;
    private Connections _conn;

    public LoginViewModel(object[] campos)
    {
      _textBoxEmail = (TextBox)campos[0];
      _textBoxPass = (PasswordBox)campos[1];
      _conn = new Connections();
    }

    private ICommand _signInCommand;

    public ICommand SignInCommand
    {
      get
      {
        return _signInCommand ?? (_signInCommand = new CommandHandler(async () =>
        {
          await SignIn();
        }));
      }
    }

    private async Task SignIn()
    {
      if (isValidCredentials())
      {
        DataBaseUsers dataBaseUsers = new DataBaseUsers();

        await dataBaseUsers.CreateDataBase();

        User user = dataBaseUsers.GetUserByEmail(Email);

        if (user != null)
        {
          if (Security.VerifyHashedPassword(Password, user.Password))
          {
            user.AccessToken = Guid.NewGuid().ToString();
            user.AccessTokenExpires = DateTime.UtcNow.AddDays(1).ToString("o");

            await Task.Run(() => dataBaseUsers.UpdateUserTokens(user.Id, user.AccessToken, user.AccessTokenExpires));

            MetaDataManager.GetInstance().SetAccessToken(user.AccessToken);
            MetaDataManager.GetInstance().SetSignedInStatus(true);
            StorageSA.saveData();

            ((Frame)Window.Current.Content).Navigate(typeof(Welcome));
          }
          else
          {
            GeneralMessage = "Invalid credentials. Please check your password.";
            GeneralTextColor = "#FFC43131";
          }
        }
        else
        {
          GeneralMessage = "User not found.";
          GeneralTextColor = "#FFC43131";
        }
      }
      else
      {
        GeneralMessage = "Please correct the highlighted errors.";
        GeneralTextColor = "#FFC43131";
      }
    }

    private bool isValidCredentials()
    {
      bool isValid = true;

      if (!Security.EmailIsValid(Email))
      {
        EmailMessage = "Invalid Email";
        isValid = false;
      }
      else
      {
        EmailMessage = string.Empty;
      }

      if (!Security.PasswordIsValid(Password))
      {
        PasswordMessage = "Invalid Password";
        isValid = false;
      }
      else
      {
        PasswordMessage = string.Empty;
      }

      return isValid;
    }
  }
}
