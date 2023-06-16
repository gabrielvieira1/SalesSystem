using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection
{
  public class DataBaseUsers
  {
    public async Task CreateDataBase()
    {
      using(var conn = new Connections())
      {
        await conn.Database.EnsureCreatedAsync();
      }
    }

    public async Task AddUser(User user)
    {
      using(var conn = new Connections())
      {
        conn.Users.Add(user);
        await conn.SaveChangesAsync();
      }
    }

  }
}
