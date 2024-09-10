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
      if (isValidCredentials(Email, Password))
      {
        DataBaseUsers dataBaseUsers = new DataBaseUsers();

        await dataBaseUsers.CreateDataBase();

        User user = new User()
        {
          Email = Email,
          Password = Password
        };

        if (dataBaseUsers.DoesUserExists(user))
        {
          int userId = dataBaseUsers.LoginUserExists(user);

          if (userId > 0)
          {
            User loggedUser = dataBaseUsers.GetUserById(userId);

            GeneralMessage = "User logged in successfully.";
            GeneralTextColor = "#FF00FF00";

            Debug.WriteLine("Logged in user: " + loggedUser.Name);

            ((Frame)Window.Current.Content).Navigate(typeof(Welcome), loggedUser);
          }
          else
          {
            GeneralMessage = "Invalid credentials. Please check your email and password.";
            GeneralTextColor = "#FFC43131";
          }
        }
        else
        {
          GeneralMessage = "User not found.";
          GeneralTextColor = "#FFC43131";
        }
      }
    }

    private bool isValidCredentials(string email, string password)
    {
      if (string.IsNullOrEmpty(email))
      {
        EmailMessage = "Please enter your email.";
        _textBoxEmail.Focus(FocusState.Programmatic);
      }
      else
      {
        if (TextBoxEvent.IsValidEmail(email))
        {
          if (string.IsNullOrEmpty(password))
          {
            PasswordMessage = "Please enter your password.";
            _textBoxPass.Focus(FocusState.Programmatic);
          }
          else
          {
            return true;
          }
        }
        else
        {
          EmailMessage = "The email entered is not valid.";
          _textBoxEmail.Focus(FocusState.Programmatic);
        }
      }

      return false;
    }
  }
}
