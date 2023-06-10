using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection
{
    public class Connections : DataConnection
    {

        public Connections() : base(new SqlServerDataProvider("", SqlServerVersion.v2017),
            "Data Source=GABRIEL-DELL\\SQLEXPRESS;Database=ByteBank;Integrated Security=True;TrustServerCertificate=True;") { }
    }
}
