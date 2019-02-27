using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Sqlite
{
    [Table("RandomUser")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Street { get; set; }
        public string Imagen { get; set; }
    }
}
