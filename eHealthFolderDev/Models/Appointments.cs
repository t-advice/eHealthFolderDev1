using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace eHealthFolderDev.Models
{
    public class Appointments
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Location { get; set;}
        public string Hospital { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Notes { get; set; }



    }
}
