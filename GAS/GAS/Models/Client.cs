using System.Collections.Generic;

namespace GAS.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        // коллекция связанных заявок
        public ICollection<AppRequest> Requests { get; set; } = new List<AppRequest>();
    }
}

