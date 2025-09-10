using GAS.Data;
using GAS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GAS.Services
{
    public class DataService
    {
        private readonly AppDbContext _db;
        private readonly string _connectionString;
        private readonly string _defaultBackupFolder;

        public DataService()
        {
            _db = new AppDbContext();

            var cfg = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            _connectionString = cfg.GetConnectionString("Default") ?? "";
            _defaultBackupFolder = cfg["Backup:DefaultFolder"] ?? "C:\\Backups\\GAS";
        }

        // Clients
        public List<Client> GetClients() => _db.Clients.OrderBy(c => c.Id).ToList();
        public void AddClient(Client c) { _db.Clients.Add(c); _db.SaveChanges(); }
        public void UpdateClient(Client c) { _db.Clients.Update(c); _db.SaveChanges(); }
        public void DeleteClient(Client c) { _db.Clients.Remove(c); _db.SaveChanges(); }

        // Requests
        public List<AppRequest> GetRequests() => _db.Requests.OrderBy(r => r.Id).ToList();
        public void AddRequest(AppRequest r) { _db.Requests.Add(r); _db.SaveChanges(); }
        public void UpdateRequest(AppRequest r) { _db.Requests.Update(r); _db.SaveChanges(); }
        public void DeleteRequest(AppRequest r) { _db.Requests.Remove(r); _db.SaveChanges(); }

        // Equipment
        public List<Equipment> GetEquipment() => _db.Equipment.OrderBy(e => e.Id).ToList();
        public void AddEquipment(Equipment e) { _db.Equipment.Add(e); _db.SaveChanges(); }
        public void UpdateEquipment(Equipment e) { _db.Equipment.Update(e); _db.SaveChanges(); }
        public void DeleteEquipment(Equipment e) { _db.Equipment.Remove(e); _db.SaveChanges(); }

        // Backup/Restore
        public string BackupDatabase(string? path = null)
        {
            Directory.CreateDirectory(_defaultBackupFolder);
            var file = path ?? Path.Combine(_defaultBackupFolder, $"GAS_{System.DateTime.Now:yyyyMMdd_HHmmss}.bak");

            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"BACKUP DATABASE [GAS] TO DISK=@p WITH INIT";
            cmd.Parameters.AddWithValue("@p", file);
            cmd.ExecuteNonQuery();
            return file;
        }

        public void RestoreDatabase(string path)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            // Переводим БД в single_user и восстанавливаем
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "ALTER DATABASE [GAS] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                cmd.ExecuteNonQuery();
            }
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "RESTORE DATABASE [GAS] FROM DISK=@p WITH REPLACE";
                cmd.Parameters.AddWithValue("@p", path);
                cmd.ExecuteNonQuery();
            }
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "ALTER DATABASE [GAS] SET MULTI_USER";
                cmd.ExecuteNonQuery();
            }
        }
    }
}

