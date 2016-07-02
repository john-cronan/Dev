using JC.DataAccess.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;
using System.Threading;

namespace JC.DataAccess.IntegrationTests
{
    public static class Program
    {
        public static void Main()
        {
            if (!TestDB.Exists())
            {
                TestDB.Create();
            }
            InsertRowsWithAnonymousTypeParameters();
            InsertRowsWithColumnAttributes();
            AbortTransaction();
            CommitTransaction();
            CallStoredProcedures();
            QueryScalarValues();
        }

        private static void InsertRowsWithAnonymousTypeParameters()
        {
            TestDB.TruncateTable("People");
            using (IDatabase db = new SqlServerDatabase(TestDB.ConnectionString))
            {
                int opens = 0;
                int closes = 0;
                (db as SqlServerDatabase).ConnectionClosed += (sender, e) => Interlocked.Increment(ref closes);
                (db as SqlServerDatabase).ConnectionOpened += (sender, e) => Interlocked.Increment(ref opens);
                string commandText = "INSERT INTO People VALUES (@FirstName, @LastName);";
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "John",
                    LastName = "Lennon"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Paul",
                    LastName = "McCartney"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "George",
                    LastName = "Harrison"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Ringo",
                    LastName = "Starr"
                });
                var rows = db.Query<PeopleRowNoAttributes>("SELECT * FROM People;", CommandType.Text).ToArray();
                Assert.IsTrue(rows.Any(x => x.FirstName == "John" && x.LastName == "Lennon"));
                Assert.IsTrue(rows.Any(x => x.FirstName == "Paul" && x.LastName == "McCartney"));
                Assert.IsTrue(rows.Any(x => x.FirstName == "George" && x.LastName == "Harrison"));
                Assert.IsTrue(rows.Any(x => x.FirstName == "Ringo" && x.LastName == "Starr"));
                Assert.IsTrue(opens == closes);
            }
        }

        private static void InsertRowsWithColumnAttributes()
        {
            TestDB.TruncateTable("People");
            using (IDatabase db = new SqlServerDatabase(TestDB.ConnectionString))
            {
                int opens = 0;
                int closes = 0;
                (db as SqlServerDatabase).ConnectionClosed += (sender, e) => Interlocked.Increment(ref closes);
                (db as SqlServerDatabase).ConnectionOpened += (sender, e) => Interlocked.Increment(ref opens);
                string commandText = "INSERT INTO People VALUES (@FirstName, @LastName);";
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "John",
                    LastName = "Lennon"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Paul",
                    LastName = "McCartney"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "George",
                    LastName = "Harrison"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Ringo",
                    LastName = "Starr"
                });
                var rows = db.Query<PeopleRow>("SELECT * FROM People;", CommandType.Text).ToArray();
                Assert.IsTrue(rows.Any(x => x.GivenName == "John" && x.Surname == "Lennon"));
                Assert.IsTrue(rows.Any(x => x.GivenName == "Paul" && x.Surname == "McCartney"));
                Assert.IsTrue(rows.Any(x => x.GivenName == "George" && x.Surname == "Harrison"));
                Assert.IsTrue(rows.Any(x => x.GivenName == "Ringo" && x.Surname == "Starr"));
                Assert.IsTrue(opens == closes);
            }
        }

        private static void AbortTransaction()
        {
            TestDB.TruncateTable("People");
            using (IDatabase db = new SqlServerDatabase(TestDB.ConnectionString))
            {
                int opens = 0;
                int closes = 0;
                (db as SqlServerDatabase).ConnectionClosed += (sender, e) => Interlocked.Increment(ref closes);
                (db as SqlServerDatabase).ConnectionOpened += (sender, e) => Interlocked.Increment(ref opens);
                string commandText = "INSERT INTO People VALUES (@FirstName, @LastName);";
                db.BeginTransaction();
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "John",
                    LastName = "Lennon"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Paul",
                    LastName = "McCartney"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "George",
                    LastName = "Harrison"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Ringo",
                    LastName = "Starr"
                });
                db.Abort();
                Assert.IsTrue(opens == 1);
                Assert.IsTrue(opens == closes);
                var rows = db.Query<PeopleRowNoAttributes>("SELECT * FROM People;", CommandType.Text);
                Assert.IsTrue(rows.Count() == 0);
                Assert.IsTrue(opens == 2);
            }
        }

        private static void CommitTransaction()
        {
            TestDB.TruncateTable("People");
            using (IDatabase db = new SqlServerDatabase(TestDB.ConnectionString))
            {
                int opens = 0;
                int closes = 0;
                (db as SqlServerDatabase).ConnectionClosed += (sender, e) => Interlocked.Increment(ref closes);
                (db as SqlServerDatabase).ConnectionOpened += (sender, e) => Interlocked.Increment(ref opens);
                string commandText = "INSERT INTO People VALUES (@FirstName, @LastName);";
                db.BeginTransaction();
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "John",
                    LastName = "Lennon"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Paul",
                    LastName = "McCartney"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "George",
                    LastName = "Harrison"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Ringo",
                    LastName = "Starr"
                });
                db.Commit();
                Assert.IsTrue(opens == 1);
                Assert.IsTrue(opens == closes);
                var rows = db.Query<PeopleRowNoAttributes>("SELECT * FROM People;", CommandType.Text);
                Assert.IsTrue(rows.Count() == 4);
                Assert.IsTrue(opens == 2);
                Assert.IsTrue(opens == closes);
            }
        }

        private static void CallStoredProcedures()
        {
            TestDB.TruncateTable("People");
            using (IDatabase db = new SqlServerDatabase(TestDB.ConnectionString))
            {
                int opens = 0;
                int closes = 0;
                (db as SqlServerDatabase).ConnectionClosed += (sender, e) => Interlocked.Increment(ref closes);
                (db as SqlServerDatabase).ConnectionOpened += (sender, e) => Interlocked.Increment(ref opens);
                db.Execute("CreatePerson", CommandType.StoredProcedure, new
                {
                    FirstName = "John",
                    LastName = "Lennon"
                });
                var row = db.Query<PeopleRow>("SELECT * FROM People;", CommandType.Text).First();
                Assert.IsTrue(row.GivenName == "John");
                Assert.IsTrue(row.Surname == "Lennon");
                Assert.IsTrue(opens == closes);
            }
        }

        private static void QueryScalarValues()
        {
            TestDB.TruncateTable("People");
            using (IDatabase db = new SqlServerDatabase(TestDB.ConnectionString))
            {
                string commandText = "INSERT INTO People VALUES (@FirstName, @LastName);";
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "John",
                    LastName = "Lennon"
                });
                db.Execute(commandText, CommandType.Text, new
                {
                    FirstName = "Paul",
                    LastName = "McCartney"
                });
                int count = db.QueryScalar<int>("SELECT COUNT(*) FROM People;", CommandType.Text);
                Assert.IsTrue(count == 2);
            }
        }

        private class PeopleRowNoAttributes
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private class PeopleRow
        {
            [DataColumn("FirstName")]
            public string GivenName { get; set; }

            [DataColumn("LastName")]
            public string Surname { get; set; }
        }
    }
}
