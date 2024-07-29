using Connection;
using Sales_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sales_system.Services;
using System.Diagnostics.Tracing;

namespace Sales_system.ViewModels
{
    internal class WelcomeViewModel : UserModel
    {
        public WelcomeViewModel() 
        {
            
            DataBaseUsers dataBaseUsers = new DataBaseUsers();
            /*dataBaseUsers.GetUserById();*/


            Helpers.UserLoggedIn += OnUserLoggedIn;
        }

        private void OnUserLoggedIn(string userName, int userId)
        {
            WelcomeMessage = "Bem vindo " + userName + " com id = " + userId;
        }

        ~WelcomeViewModel()
        {
          Helpers.UserLoggedIn -= OnUserLoggedIn;
        }
    }
}
