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
    private ICommand _command;
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
          GeneralMessage = "Usuario logado correctamente.";
          //*((Frame)Window.Current.Content).Navigate(typeof(Welcome), user);*//*
          Debug.WriteLine("user name - ", user.Name);

          ((Frame)Window.Current.Content).Navigate(typeof(Welcome), user);
        }
        else
        {
          GeneralMessage = "Usuario no encontrado.";
          GeneralTextColor = "#FFC43131";

        }
      }
    }

    private bool isValidCredentials(string email, string password)
    {
      if (string.IsNullOrEmpty(email))
      {
        EmailMessage = "Ingresse el email";
        _textBoxEmail.Focus(FocusState.Programmatic);
      }
      else
      {
        if (TextBoxEvent.IsValidEmail(email))
        {
          if (string.IsNullOrEmpty(password))
          {
            PasswordMessage = "Ingrese el password";
            _textBoxPass.Focus(FocusState.Programmatic);
          }
          else
          {
            return true;
          }
        }
        else
        {
          EmailMessage = "El email no es valido";
          _textBoxEmail.Focus(FocusState.Programmatic);
        }
      }

      return false;
    }
  }
}
