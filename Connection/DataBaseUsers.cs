using Microsoft.Data.Sqlite;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

    public void UpdateUserTokens(int userId, string newAccessToken, string newAccessTokenExpires)
    {
      var sql = "UPDATE Users SET AccessToken=@AccessToken, AccessTokenExpires=@AccessTokenExpires WHERE Id=@UserId";

      using (var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db"))
      {
        conn.Open();
        using (var command = new SqliteCommand(sql, conn))
        {
          command.Parameters.AddWithValue("@AccessToken", newAccessToken);
          command.Parameters.AddWithValue("@AccessTokenExpires", newAccessTokenExpires);
          command.Parameters.AddWithValue("@UserId", userId);
          command.ExecuteNonQuery();
        }
      }
    }

    public Boolean DoesUserExists(Models.User user)
    {
      var sql = "SELECT * FROM Users WHERE email=@Email";

      using (var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db"))
      {
        conn.Open();
        using (var command = new SqliteCommand(sql, conn))
        {
          command.Parameters.AddWithValue("@Email", user.Email);
          using (var reader = command.ExecuteReader())
          {
            return reader.HasRows;
          }
        }
      }
    }

    public int LoginUserExists(Models.User user)
    {
      var sql = "SELECT * FROM Users WHERE email=@Email AND password=@Password";

      using (var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db"))
      {
        conn.Open();
        using (var command = new SqliteCommand(sql, conn))
        {
          command.Parameters.AddWithValue("@Email", user.Email);
          command.Parameters.AddWithValue("@Password", user.Password);

          using (var reader = command.ExecuteReader())
          {
            if (reader.HasRows)
            {
              while (reader.Read())
              {
                return reader.GetInt32(0);  // Retorna o ID do usuário
              }
            }
            return -1;
          }
        }
      }
    }

    public Models.User GetUserById(int id)
    {
      var sql = "SELECT id, name, email, AccessToken, AccessTokenExpires, CreatedDateTime, Active FROM Users WHERE id=@id";

      using (var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db"))
      {
        conn.Open();
        using (var command = new SqliteCommand(sql, conn))
        {
          command.Parameters.AddWithValue("@Id", id);
          using (var reader = command.ExecuteReader())
          {
            if (reader.HasRows)
            {
              Models.User user = new Models.User();
              while (reader.Read())
              {
                user.Id = reader.GetInt32(0);
                user.Name = reader.GetString(1);
                user.Email = reader.GetString(2);
                user.AccessToken = reader.GetString(3);
                user.AccessTokenExpires = reader.GetString(4);
                user.CreatedDateTime = reader.GetDateTime(5);
                user.Active = reader.GetBoolean(6);
              }
              return user;
            }
          }
        }
      }
      return null;
    }

    public Models.User GetUserByEmail(string email)
    {
      var sql = "SELECT id, name, email, password FROM Users WHERE email=@Email";

      using (var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db"))
      {
        conn.Open();
        using (var command = new SqliteCommand(sql, conn))
        {
          command.Parameters.AddWithValue("@Email", email);
          using (var reader = command.ExecuteReader())
          {
            if (reader.HasRows)
            {
              Models.User user = new Models.User();
              while (reader.Read())
              {
                user.Id = reader.GetInt32(0);
                user.Name = reader.GetString(1);
                user.Email = reader.GetString(2);
                user.Password = reader.IsDBNull(3) ? null : reader.GetString(3);
              }
              return user;
            }
          }
        }
      }
      return null;
    }

    public Models.User GetUserByAccessToken(string accessToken)
    {
      var sql = "SELECT id, name, email, password FROM Users WHERE AccessToken=@accessToken";

      using (var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db"))
      {
        conn.Open();

        using (var command = new SqliteCommand(sql, conn))
        {
          command.Parameters.AddWithValue("@accessToken", accessToken);

          using (var reader = command.ExecuteReader())
          {
            if (reader.HasRows)
            {
              Models.User user = new Models.User();

              while (reader.Read())
              {
                user.Id = reader.GetInt32(0);
                user.Name = reader.GetString(1);
                user.Email = reader.GetString(2);
                user.Password = reader.IsDBNull(3) ? null : reader.GetString(3);
              }

              return user;
            }
          }
        }
      }

      return null;
    }

    public async Task ClearAccessToken(int userId)
    {
      var sql = "UPDATE Users SET AccessToken = NULL, AccessTokenExpires = NULL WHERE id = @id";

      using (var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db"))
      {
        await conn.OpenAsync();

        using (var command = new SqliteCommand(sql, conn))
        {
          command.Parameters.AddWithValue("@id", userId);

          try
          {
            await command.ExecuteNonQueryAsync();
            Debug.WriteLine($"AccessToken and AccessTokenExpires cleared for user with ID: {userId}");
          }
          catch (Exception ex)
          {
            Debug.WriteLine($"Error clearing AccessToken for user {userId}: {ex.Message}");
          }
        }
      }
    }
  }
}
