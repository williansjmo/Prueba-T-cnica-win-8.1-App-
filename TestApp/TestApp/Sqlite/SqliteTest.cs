using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Sqlite
{
    class SqliteTest
    {
        SQLiteConnection db = new SQLiteConnection("Filename=sqliteSample.db");

        public bool Insertar(List<User> users)
        {
            try
            {
                db.BeginTransaction();
                db.InsertAll(users);
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                db.Rollback();
                return false;
            }

        }
        public List<User> GetUsers() => db.Query<User>("Select * from RandomUser");
        public void CrearTabla() => db.CreateTable<User>();
        public void EliminarTabla() => db.DropTable<User>();
    }
}
