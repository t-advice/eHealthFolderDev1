using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace eHealthFolderDev.Models
{
    public class Visits
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Doctor { get; set; }
        public string? Department { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
        public string? Hospital { get; set; }


    }
}
