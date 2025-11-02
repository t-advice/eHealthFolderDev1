using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using eHealthFolderDev.Models;

namespace eHealthFolderDev.Services
{
    public class eHealthFolderDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public eHealthFolderDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Person>().Wait();
            _database.CreateTableAsync<Appointments>().Wait();
            _database.CreateTableAsync<Visits>().Wait();
        }

        // Person Table Methods
        public Task<List<Person>> GetPersonsAsync() => _database.Table<Person>().ToListAsync();
        public Task<int> SavePersonAsync(Person person) => _database.InsertOrReplaceAsync(person);
        public Task<int> DeletePersonAsync(Person person) => _database.DeleteAsync(person);


        // Visits Table Methods
        public Task<List<Visits>> GetVisitsAsync() => _database.Table<Visits>().OrderByDescending(v => v.Date).ToListAsync();
        public Task<int> SaveVisitAsync(Visits visits) => _database.InsertOrReplaceAsync(visits);
        public Task<int> DeleteVisitAsync(Visits visits) => _database.DeleteAsync(visits);

        // Appointments Table Methods
        public Task<List<Appointments>> GetAppointmentsAsync() => _database.Table<Appointments>().OrderByDescending(a => a.Date).ToListAsync();
        public Task<int> SaveAppointmentAsync(Appointments appointments) => _database.InsertOrReplaceAsync(appointments);
        public Task<int> DeleteAppointmentAsync(Appointments appointments) => _database.DeleteAsync(appointments);


        // Get most recent visit/ appointment
        public async Task<Visits> GetMostRecentVisitAsync() => (await GetVisitsAsync()).FirstOrDefault();
        public async Task<Appointments> GetMostRecentAppointmentAsync() => (await GetAppointmentsAsync()).FirstOrDefault();

    }
}
