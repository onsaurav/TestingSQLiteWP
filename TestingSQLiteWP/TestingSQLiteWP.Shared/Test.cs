using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestingSQLiteWP
{
    [Table("Tests")]
    public class Test
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] image { get; set; }
    }
}
