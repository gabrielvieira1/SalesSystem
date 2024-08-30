using LinqToDB.Tools;
using Microsoft.Data.Sqlite;
using Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation.Diagnostics;
using Windows.Storage;


namespace Connection
{
  public class DataBaseUsers
  {
    public async Task CreateDataBase()
    {
      using (var conn = new Connections())
      {
        await conn.Database.EnsureCreatedAsync();
      }
    }

    public async Task AddUser(Models.User user)
    {
      using (var conn = new Connections())
      {
        conn.Users.Add(user);
        await conn.SaveChangesAsync();
      }
    }

    public Boolean DoesUserExists(User user)
    {
      using (var conn = new Connections())
      {
        if (conn.DoesUserExists(user))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    
      /*var sql = "SELECT * FROM Users WHERE email=@email";

          var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db");
          conn.Open();

          var command = new SqliteCommand(sql, conn);
          command.Parameters.AddWithValue("@email", user.Email);

          var reader = command.ExecuteReader();

          if (reader.HasRows)
          {
              return true;
          }
          else
          {
              return false;
          }*/
    }
  }
}
