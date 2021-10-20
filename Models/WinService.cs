using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire.Models
{
    public class WinService
    {
        // Имя сервера
        public string Server { get; set; }

        // Имя сервиса
        public string ServiceName { get; set; }

        // Название сервиса
        public string DisplayName { get; set; }
        
        // Статус службы
        public string Status { get; set; }
    }
}
