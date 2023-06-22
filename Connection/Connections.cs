using Microsoft.EntityFrameworkCore;
using Models;
using System.Data;
using Windows.Storage;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder.UseSqlite($"Data Source={ApplicationData.Current.LocalFolder.Path}/Sales_System"));
    }

  }
}
