using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_system.Services
{
    public static class Helpers
    {
        public static event Action<string, int> UserLoggedIn;

        public static void RaiseUserLoggedIn(string userName, int userId)
        {
            UserLoggedIn?.Invoke(userName, userId);
        }
    }
}
