using Connection;
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
using Models;
using System.Diagnostics;
using Sales_system.Views;

namespace Sales_system.ViewModels
{
  internal class SignupViewModel : UserModel
  {
    private TextBox _textBoxName;
    private TextBox _textBoxEmail;
    private PasswordBox _textBoxPass;
    private PasswordBox _textBoxConfirmPass;
    private string date = DateTime.Now.ToString("dd/MM/yyy");
    private Frame rootFrame = Window.Current.Content as Frame;
    private Connections _conn;

    public SignupViewModel(object[] campos)
    {
      _textBoxName = (TextBox)campos[0];
      _textBoxEmail = (TextBox)campos[1];
      _textBoxPass = (PasswordBox)campos[2];
      _textBoxConfirmPass = (PasswordBox)campos[3];
      _conn = new Connections();
    }

    private ICommand _signUpcommand;

    public ICommand SignUpCommand
    {
      get
      {
        return _signUpcommand ?? (_signUpcommand = new CommandHandler(async () =>
        {
          await SignUp();
        }));
      }
    }

    private async Task SignUp()
    {
      if (isValidCredentials(Email, Password))
      {
        DataBaseUsers dataBaseUsers = new DataBaseUsers();

        await dataBaseUsers.CreateDataBase();

        User user = new User()
        {
          Name = Name,
          Email = Email,
          Password = Password,
          AccessToken = Guid.NewGuid().ToString(),
          AccessTokenExpires = DateTime.UtcNow.AddDays(1).ToString("o"),
          Active = true
        };


        if (dataBaseUsers.DoesUserExists(user))
        {
          GeneralMessage = "The user already exists.";
          GeneralTextColor = "#FFC43131";
        }
        else
        {
          await dataBaseUsers.AddUser(user);
          MetaDataManager.GetInstance().SetAccessToken(user.AccessToken);
          MetaDataManager.GetInstance().SetSignedInStatus(true);
          StorageSA.saveData();
          ((Frame)Window.Current.Content).Navigate(typeof(Welcome), user);
        }
      }
    }

    private bool isValidCredentials(string email, string password)
    {
      if (string.IsNullOrEmpty(Name))
      {
        NameMessage = "Ingresse el nombre";
        _textBoxName.Focus(FocusState.Programmatic);
      }
      else
      {
        if (string.IsNullOrEmpty(Email))
        {
          EmailMessage = "Ingresse el email";
          _textBoxEmail.Focus(FocusState.Programmatic);
        }
        else
        {
          if (!TextBoxEvent.IsValidEmail(Email))
          {
            EmailMessage = "El email no es valido";
            _textBoxEmail.Focus(FocusState.Programmatic);
          }
          else
          {
            if (string.IsNullOrEmpty(Password))
            {
              PasswordMessage = "Ingrese el password";
              _textBoxPass.Focus(FocusState.Programmatic);
            }
            else
            {
              if (string.IsNullOrEmpty(ConfirmPassword))
              {
                ConfirmPasswordMessage = "Ingrese el password de confirmación";
                _textBoxConfirmPass.Focus(FocusState.Programmatic);
              }
              else
              {

                if (string.Compare(Password, ConfirmPassword) != 0)
                {
                  ConfirmPasswordMessage = "El password de confirmación no son lo mismo. Ingrese otra vez";
                  _textBoxConfirmPass.Focus(FocusState.Programmatic);
                }
                else
                {
                  return true;
                }
              }
            }
          }
        }
      }

      return false;
    }
  }
}
