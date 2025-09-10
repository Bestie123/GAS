using System;

namespace GAS.Models
{
    public class AppRequest
    {
        public int Id { get; set; }
        public string Status { get; set; } = "Новая"; // Новая/В работе/Закрыта
        public int? ClientId { get; set; }
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public string House { get; set; } = "";
        public string Index { get; set; } = "";
        public string Corp { get; set; } = "";
        public string Entrance { get; set; } = "";
        public string Apartment { get; set; } = "";
        public string Essence { get; set; } = "";
        public string CompletedWork { get; set; } = "";
        public DateTime DateRequest { get; set; } = DateTime.Today;

        // Убрали проблемные типы (Method) — маппинг без ошибок
    }
}

