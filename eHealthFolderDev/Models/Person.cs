using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace eHealthFolderDev.Models
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int  IDNumber { get; set; }
        public string Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Allergies { get; set; }
        public string email { get; set; }
        public string Gender { get; set; }
    }
}
