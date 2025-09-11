using GAS.Data;
using GAS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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
        public void AddRequest(AppRequest r)
        {
            if (r.ClientId.HasValue && !_db.Clients.Any(c => c.Id == r.ClientId.Value))
                throw new InvalidOperationException("Невалидный клиент");

            try
            {
                _db.Requests.Add(r);
                _db.SaveChanges();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                // получаем «глубочайшую» ошибку из цепочки InnerException
                var inner = ex.GetBaseException();

                // Пока просто выбрасываем её с более читаемым сообщением
                throw new InvalidOperationException(
                    $"Ошибка при добавлении заявки: {inner.Message}", inner);
            }
        }
        public void UpdateRequest(AppRequest r)
        {
            var existing = _db.Requests.FirstOrDefault(x => x.Id == r.Id);
            if (existing == null)
                throw new InvalidOperationException($"Заявка с Id={r.Id} не найдена.");

            _db.Entry(existing).CurrentValues.SetValues(r);
            existing.ClientId = r.ClientId;
            _db.Entry(existing).Reference(x => x.Client).IsModified = false;

            _db.SaveChanges();
        }

        public AppRequest GetRequestById(int id)
        {
            // Загружаем оригинал из контекста с навигацией Client (если она нужна в UI)
            var entity = _db.Requests
                            .Include(r => r.Client) // можно убрать, если клиент в диалоге не нужен
                            .FirstOrDefault(r => r.Id == id);

            if (entity == null)
                throw new InvalidOperationException($"Заявка с Id={id} не найдена.");

            return entity;
        }

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

