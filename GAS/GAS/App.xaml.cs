using System.Windows;
using GAS.Data;
using Microsoft.EntityFrameworkCore;

namespace GAS
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаём/мигрируем базу перед открытием окон
            using var db = new AppDbContext();
            db.Database.Migrate(); // миграции EF Core (или EnsureCreated для простой схемы)

            // Запуск окна входа
            new Views.LoginView().Show();
        }
    }
}
