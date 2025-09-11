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

            // ������/��������� ���� ����� ��������� ����
            using var db = new AppDbContext();
            db.Database.Migrate(); // �������� EF Core (��� EnsureCreated ��� ������� �����)

            // ������ ���� �����
            new Views.LoginView().Show();
        }
    }
}
