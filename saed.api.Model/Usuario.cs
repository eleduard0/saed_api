using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.Model
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Correo { get; set; }

        public string PasswordHash { get; set; }

        public string Rol { get; set; } // "Admin", "Alumno"
    }
}
