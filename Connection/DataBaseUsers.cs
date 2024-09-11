﻿using Microsoft.Data.Sqlite;
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

    public Boolean DoesUserExists(Models.User user)
    {
      var sql = "SELECT * FROM Users WHERE email=@email";

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
      }
    }

    public int LoginUserExists(Models.User user)
    {
      var sql = "SELECT * FROM Users WHERE email=@email AND password=@password";

      var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db");
      conn.Open();

      var command = new SqliteCommand(sql, conn);
      command.Parameters.AddWithValue("@email", user.Email);
      command.Parameters.AddWithValue("@password", user.Password);

      var id = 0;

      var reader = command.ExecuteReader();

      if (reader.HasRows)
      {
        while (reader.Read())
        {
          id = reader.GetInt32(0);
        }

        return id;

      }
      else
      {
        return -1;
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
          command.Parameters.AddWithValue("@id", id);

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

    public Models.User GetUserByAccessToken(string accessToken)
    {
      var sql = "SELECT id, name, email FROM Users WHERE AccessToken=@accessToken";

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
