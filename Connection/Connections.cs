using Microsoft.EntityFrameworkCore;
using Models;
using System.Data;
using System.Diagnostics;
using Windows.Storage;
using Models;
using Microsoft.Data.Sqlite;

namespace Connection
{
  //public class Connections : DataConnection
  //{

  //    public Connections() : base(new SqlServerDataProvider("", SqlServerVersion.v2017),
  //        "Data Source=GABRIEL-DELL\\SQLEXPRESS;Database=ByteBank;Integrated Security=True;TrustServerCertificate=True;") { }
  //}

  public class Connections: DbContext
  {

    public DbSet<User> Users { get; set; }

    public Connections() { }

    public bool DoesUserExists(User user)
    {
      var sql = "SELECT * FROM Users WHERE email=@email AND password=@password";

      var conn = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db");
      conn.Open();

      var command = new SqliteCommand(sql, conn);
      command.Parameters.AddWithValue("@email", user.Email);
      command.Parameters.AddWithValue("@password", user.Password);

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder.UseSqlite($"Data Source={ApplicationData.Current.LocalFolder.Path}/ByteBank.db"));
    }

  }
}
