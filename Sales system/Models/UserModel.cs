using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_system.Models
{
    public class UserModel : PropertyChangedNotification
    {
        public string Name
        {
            get { return GetValue(() => Name); }
            set
            {
                SetValue(() => Name, value);
                NameMessage = "";
                Message = "";
            }
        }

        public string Email
        {
            get { return GetValue(() => Email); }
            set
            {
                SetValue(() => Email, value);
                EmailMessage = "";
                Message = "";
            }
        }

        public string Password
        {
            get { return GetValue(() => Password); }
            set
            {
                SetValue(() => Password, value);
                PasswordMessage = "";
                Message = "";
            }
        }

        public string ConfirmPassword
        {
            get { return GetValue(() => ConfirmPassword); }
            set
            {
                SetValue(() => ConfirmPassword, value);
                ConfirmPasswordMessage = "";
                Message = "";
            }
        }
        
        public string Message
        {
            get { return GetValue(() => Message); }
            set { SetValue(() => Message, value); }
        }
        public string NameMessage
        {
            get { return GetValue(() => NameMessage); }
            set { SetValue(() => NameMessage, value); }
        }
        public string EmailMessage
        {
            get { return GetValue(() => EmailMessage); }
            set { SetValue(() => EmailMessage, value); }
        }
        public string PasswordMessage
        {
            get { return GetValue(() => PasswordMessage); }
            set { SetValue(() => PasswordMessage, value); }
        }

        public string ConfirmPasswordMessage
        {
            get { return GetValue(() => ConfirmPasswordMessage); }
            set { SetValue(() => ConfirmPasswordMessage, value); }
        }

        public string GeneralMessage
        {
            get { return GetValue(() => GeneralMessage); }
            set { SetValue(() => GeneralMessage, value); }
        }

        public string GeneralTextColor
        {
            get { return GetValue(() => GeneralTextColor);}
            set { SetValue(() => GeneralTextColor, value);}
        }

        public string WelcomeMessage
        {
            get { return GetValue(() => WelcomeMessage);}
            set { SetValue(() => WelcomeMessage, value);}
        }
    }
}
