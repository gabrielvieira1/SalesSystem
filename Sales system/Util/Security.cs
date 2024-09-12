using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCrypt.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Sales_system.Util
{
  public class Security
  {
    public static string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 14);
    }
    public static bool VerifyHashedPassword(string password, string hashedPassword)
    {
      return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
    public static bool PasswordIsValid(string passWord)
    {
      int validConditions = 0;

      if (string.IsNullOrEmpty(passWord)) return false;
      if (passWord.Length < 8 || passWord.Length >= 100) return false;


      foreach (char c in passWord)
      {
        if (c >= 'a' && c <= 'z')
        {
          validConditions++;
          break;
        }
      }
      foreach (char c in passWord)
      {
        if (c >= 'A' && c <= 'Z')
        {
          validConditions++;
          break;
        }
      }
      if (validConditions == 0) return false;
      foreach (char c in passWord)
      {
        if (c >= '0' && c <= '9')
        {
          validConditions++;
          break;
        }
      }
      if (validConditions == 1) return false;
      if (validConditions == 2)
      {
        char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' };   
        if (passWord.IndexOfAny(special) == -1) return false;
      }
      return true;
    }
    public static bool InputIsValid(string input)
    {
      if (string.IsNullOrEmpty(input))
        return false;

      string pattern = @"^[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]+$";
      Regex rgx = new Regex(pattern);
      if (!rgx.IsMatch(input))
        return false;

      return true;
    }
    public static bool EmailIsValid(string input)
    {
      if (string.IsNullOrEmpty(input))
        return false;

      string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
      Regex rgx = new Regex(pattern);
      if (!rgx.IsMatch(input))
        return false;

      return true;
    }
  }
}
